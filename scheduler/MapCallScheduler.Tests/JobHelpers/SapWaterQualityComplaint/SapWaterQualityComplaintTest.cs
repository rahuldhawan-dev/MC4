using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallScheduler.JobHelpers.SapWaterQualityComplaint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SapWaterQualityComplaint
{
    [TestClass]
    public class SapWaterQualityComplaintTest
    {
        #region Private Fields

        private IContainer _container;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IRepository<PlanningPlant>> _planningPlantRepo;
        private Mock<IRepository<WaterQualityComplaint>> _wqComplaintRepository;
        private Mock<IPremiseRepository> _premiseRepository;
        private Mock<ICoordinateRepository> _coordinateRepository;
        private Mock<ISAPNotificationRepository> _sapNotificationRepository;
        private SapWaterQualityComplaintService _target;
        private Mock<IRepository<Note>> _noteRepo;
        private Mock<IDataTypeRepository> _dataTypeRepo;

        #endregion

        #region Test Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            // Set up our container and mocks
            _container = new Container();
            _container.Inject(new Mock<ILog>().Object);

            _container.Inject((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            _dateTimeProvider.Setup(s => s.GetCurrentDate()).Returns(DateTime.Now);

            _container.Inject((_planningPlantRepo = new Mock<IRepository<PlanningPlant>>()).Object);
            _container.Inject((_wqComplaintRepository = new Mock<IRepository<WaterQualityComplaint>>()).Object);
            _container.Inject((new Mock<IRepository<WaterQualityComplaintType>>()).Object);
            _container.Inject((new Mock<IRepository<Town>>()).Object);
            _container.Inject((new Mock<IRepository<State>>()).Object);
            _container.Inject((_premiseRepository = new Mock<IPremiseRepository>()).Object);
            _container.Inject((_coordinateRepository = new Mock<ICoordinateRepository>()).Object);
            _container.Inject((_sapNotificationRepository = new Mock<ISAPNotificationRepository>()).Object);
            _container.Inject((_noteRepo = new Mock<IRepository<Note>>()).Object);
            _container.Inject((_dataTypeRepo = new Mock<IDataTypeRepository>()).Object);
            _target = _container.GetInstance<SapWaterQualityComplaintService>();

            // data setup
            _planningPlantRepo.Setup(p => p.GetAll())
                              .Returns(new List<PlanningPlant> {
                                   new PlanningPlant() { Code = "ABC1" },
                                   new PlanningPlant() { Code = "DEF2" },
                                   new PlanningPlant() { Code = "GHI3" }
                               }.AsQueryable());
        }

        #endregion

        #region Private Methods

        private SAPNotificationCollection GetSampleNotification()
        {
            var notification = new SAPNotificationCollection();
            notification.Items.Add(new SAPNotification {
                SAPNotificationNumber = "123456789",
                Premise = "123456789",
                DateCreated = "20230101",
                TimeCreated = "145455",
                Telephone = "1324657980",
                NotificationLongText = "asdfasd Business Parter : 654987321 asdf asdf Notes : My notes. Premise : asdfasdf"
            });
            return notification;
        }

        #endregion

        [TestMethod]
        public void Test_SapSearchParameters_Are_Correct()
        {
            //arrange
            SearchSapNotification searchParam = null;
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(new SAPNotificationCollection())
                                      .Callback<SearchSapNotification>(s => searchParam = s);

            //act
            _target.Process();

            //assert
            var expectedDate = _dateTimeProvider.Object.GetCurrentDate().ToString("yyyyMMdd");
            Assert.IsTrue(searchParam.DateCreatedFrom == expectedDate);
            Assert.IsTrue(searchParam.DateCreatedTo == expectedDate);
            Assert.IsTrue(searchParam.NotificationType == "35");
        }

        [TestMethod]
        public void Test_SapSearchParameters_PlanningPlants_Are_Correct()
        {
            //arrange
            SearchSapNotification searchParam = null;
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(new SAPNotificationCollection())
                                      .Callback<SearchSapNotification>(s => searchParam = s);
            var planningPlantCodes = string.Join(",", _planningPlantRepo.Object.GetAll().Select(p => p.Code));

            //act
            _target.Process();

            //assert
            Assert.IsTrue(searchParam.PlanningPlant == planningPlantCodes);
        }

        [TestMethod]
        public void Test_Process_DoesNotSaveWhenSapReturnsEmptyItems()
        {
            //arrange
            var notification = new SAPNotificationCollection();
            notification.Items.Add(new SAPNotification {
                SAPNotificationNumber = null
            });
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);

            //act
            _target.Process();

            //assert
            _wqComplaintRepository.Verify(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()), Times.Never());
        }
        
        [TestMethod]
        [DataRow("20230101", "141223", true)]
        [DataRow("20231301", "141223", false)]
        [DataRow("20230101", "41223", false)]
        public void Test_Process_ParsesDateTime(string date, string time, bool isSuccessful)
        {
            //arrange;
            var notification = GetSampleNotification();
            notification.Items[0].DateCreated = date;
            notification.Items[0].TimeCreated = time;

            var notificationDateTime = !isSuccessful
                ? (DateTime?)null
                : DateTime.ParseExact(
                    $"{notification.Items.First().DateCreated} {notification.Items.First().TimeCreated}",
                    "yyyyMMdd HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);

            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);
            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);
            //act
            _target.Process();

            //assert
            Assert.IsNotNull(complaints);
            Assert.AreEqual(notificationDateTime, complaints.First().DateComplaintReceived);
        }

        [TestMethod]
        [DataRow("1234567890", "123-456-7890")]
        [DataRow("321-4567890", "321-456-7890")]
        [DataRow("13246", "13246")]
        public void Test_Process_FormatsPhoneNumber(string phone, string formattedPhone)
        {
            //arrange;
            var notification = GetSampleNotification();
            notification.Items[0].Telephone = phone;

            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);

            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);

            //act
            _target.Process();

            //assert
            Assert.IsNotNull(complaints);
            Assert.AreEqual(formattedPhone, complaints.First().HomePhoneNumber);
        }

        [TestMethod]
        [DataRow("asdlkfasf Business Partner : 123234123 asdlfkjasd", "123234123")]
        public void Test_Process_ParsesBusinessPartnerNumber(string longText, string expected)
        {
            //arrange;
            var notification = GetSampleNotification();
            notification.Items[0].NotificationLongText = longText;

            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);

            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);

            //act
            _target.Process();

            //assert
            Assert.IsNotNull(complaints);
            Assert.AreEqual(expected, complaints.First().AccountNumber);
        }

        [TestMethod]
        [DataRow("asdlkfasf Notes : This is a test note. Premise : ", "This is a test note.")]
        [DataRow("asdlkfasf Notes : 13123 +-/*& asdf note. Premise :12312123", "13123 +-/*& asdf note.")]
        public void Test_Process_ParsesNotes(string longText, string expected)
        {
            //arrange;
            var notification = GetSampleNotification();
            notification.Items[0].NotificationLongText = longText;

            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);

            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);

            //act
            _target.Process();

            //assert
            Assert.IsNotNull(complaints);
            Assert.AreEqual(expected, complaints.First().ComplaintDescription);
        }

        [TestMethod]
        public void Test_Process_SetsOperatingCenter_WhenSinglePremiseIsFound()
        {
            //arrange
            var notification = GetSampleNotification();
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);
            var expectedPremise = new Premise() {
                PremiseNumber = "123456789",
                OperatingCenter = new OperatingCenter() { OperatingCenterCode = "OC1" }
            };
            _premiseRepository
               .Setup(x => x.Linq)
               .Returns(new[] { expectedPremise }.AsQueryable());

            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);

            //act
            _target.Process();

            //assert
            Assert.IsNotNull(complaints);
            Assert.AreSame(expectedPremise.OperatingCenter, complaints.First().OperatingCenter);
        }

        [TestMethod]
        public void Test_Process_DoesNotSetOperatingCenter_WhenMultipleOperatingCentersAreFound()
        {
            //arrange
            var notification = GetSampleNotification();
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);
            
            var expectedPremise = new Premise() {
                PremiseNumber = "123456789",
                OperatingCenter = new OperatingCenter() { OperatingCenterCode = "OC1" }
            };
            var otherPremise = new Premise() {
                PremiseNumber = "123456789",
                OperatingCenter = new OperatingCenter() { OperatingCenterCode = "OC2" }
            };
            _premiseRepository
               .Setup(x => x.Linq)
               .Returns(new[] { expectedPremise, otherPremise }.AsQueryable());

            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);

            //act
            _target.Process();

            //assert
            Assert.IsNotNull(complaints);
            Assert.IsNull(complaints.First().OperatingCenter);
        }

        [TestMethod]
        public void Test_Process_DoesNotSetCoordinate_IfThereIsNoMatchingPremise()
        {
            //arrange
            var notification = GetSampleNotification();
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);
            _premiseRepository
               .Setup(x => x.Linq)
               .Returns(new List<Premise>().AsQueryable());

            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);

            //act
            _target.Process();

            //assert
            Assert.IsNotNull(complaints);
            Assert.IsNull(complaints.First().OperatingCenter);
            Assert.IsNull(complaints.First().Coordinate);
        }

        [TestMethod]
        public void Test_Process_SetsCoordinateToCloneOfSingleMatchingPremiseCoordinate()
        {
            //arrange
            var notification = GetSampleNotification();
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);
            var expectedCoordinate = new Coordinate { Latitude = 1, Longitude = 2 };
            var expectedClone = new Coordinate();
            var expectedPremise = new Premise() {
                PremiseNumber = "123456789",
                Coordinate = expectedCoordinate
            };
            _premiseRepository
               .Setup(x => x.Linq)
               .Returns(new[] { expectedPremise }.AsQueryable());

            _coordinateRepository.Setup(x => x.CloneAndSave(expectedCoordinate)).Returns(expectedClone);

            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);

            //act
            _target.Process();

            //assert
            Assert.IsNotNull(complaints);
            Assert.AreSame(
                expectedClone,
                complaints.First().Coordinate);
        }

        [TestMethod]
        public void
            Test_Process_SetsCoordinateToCloneOfMatchingPremiseCoordinate_IfMultiplePremiseResultsHaveTheSameLatAndLonValues()
        {
            //arrange
            var notification = GetSampleNotification();
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);

            var expectedClone = new Coordinate();
            var expectedPremise1 = new Premise {
                PremiseNumber = "123456789",
                Coordinate = new Coordinate { Latitude = 1, Longitude = 2 }
            };
            var expectedPremise2 = new Premise {
                PremiseNumber = "123456789",
                Coordinate = new Coordinate { Latitude = 1, Longitude = 2 }
            };
            _premiseRepository
               .Setup(x => x.Linq)
               .Returns(new[] { expectedPremise1, expectedPremise2 }.AsQueryable());

            // In this situation, the cloning is done using the first Coordinate result.
            _coordinateRepository.Setup(x => x.CloneAndSave(expectedPremise1.Coordinate)).Returns(expectedClone);

            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);

            //act
            _target.Process();

            //assert
            Assert.IsNotNull(complaints);
            Assert.AreSame(
                expectedClone,
                complaints.First().Coordinate);
        }

        [TestMethod]
        public void
            Test_Process_DoesNotSetCoordinate_IfMultipleMatchingPremisesDoNotHaveIdenticalLatLonValues()
        {
            //arrange
            var notification = GetSampleNotification();
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);

            var expectedClone = new Coordinate();
            var expectedPremise1 = new Premise {
                PremiseNumber = "9180742056",
                Coordinate = new Coordinate { Latitude = 1, Longitude = 2 }
            };
            var expectedPremise2 = new Premise {
                PremiseNumber = "9180742056",
                Coordinate = new Coordinate { Latitude = 132, Longitude = 24 }
            };
            _premiseRepository
               .Setup(x => x.Linq)
               .Returns(new[] { expectedPremise1, expectedPremise2 }.AsQueryable());

            // In this situation, the cloning is done using the first Coordinate result.
            _coordinateRepository.Setup(x => x.CloneAndSave(expectedPremise1.Coordinate)).Returns(expectedClone);

            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);

            //act
            _target.Process();

            //assert
            Assert.IsNotNull(complaints);
            Assert.IsNull(
                complaints.First().Coordinate);
        }
        
        [TestMethod]
        public void Test_Process_CreatesNotesFromNotificationLongText()
        {
            var notification = GetSampleNotification();

            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<SearchSapNotification>()))
                                      .Returns(notification);

            IEnumerable<WaterQualityComplaint> complaints = null;
            _wqComplaintRepository.Setup(x => x.Save(It.IsAny<IEnumerable<WaterQualityComplaint>>()))
                                  .Callback<IEnumerable<WaterQualityComplaint>>(c => complaints = c);

            var dataType = new DataType();
            _dataTypeRepo.Setup(x => x.GetByTableName(WaterQualityComplaintMap.TABLE_NAME)).Returns(new [] { dataType });
            
            _target.Process();
            
            var notes = complaints.Select(complaint => new Note { DataType = dataType, Text = complaint.ComplaintDescription, CreatedBy = "Scheduler - SAP Water Quality Complaint Service", LinkedId = complaint.Id });
           
            _noteRepo.Setup(x => x.Save(notes))
                     .Callback<IEnumerable<Note>>(n => notes = n);

            Assert.IsNotNull(complaints);
            Assert.IsNotNull(notes);
            Assert.AreEqual(notes.First().Text, complaints.First().ComplaintDescription);
            Assert.AreEqual(notes.First().LinkedId, complaints.First().Id);
        }
    }
}
