using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CreateTrafficControlTicketTest : MapCallMvcInMemoryDatabaseTestBase<TrafficControlTicket>
    {
        #region Fields

        private ViewModelTester<CreateTrafficControlTicket, TrafficControlTicket> _vmTester;
        private CreateTrafficControlTicket _viewModel;
        private TrafficControlTicket _entity;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_now = DateTime.Now));
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateTrafficControlTicket(_container);
            _entity = new TrafficControlTicket();
            _vmTester = new ViewModelTester<CreateTrafficControlTicket, TrafficControlTicket>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            var fee1 = GetEntityFactory<MerchantTotalFee>().Create(new {Fee = 0.021m, IsCurrent = true});
            _vmTester.CanMapBothWays(x => x.WorkStartDate);
            _vmTester.CanMapBothWays(x => x.WorkEndDate);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.SAPWorkOrderNumber);
            _vmTester.CanMapBothWays(x => x.TotalHours);
            _vmTester.CanMapBothWays(x => x.NumberOfOfficers);
            _vmTester.CanMapBothWays(x => x.AccountingCode);
            _vmTester.CanMapBothWays(x => x.TrafficControlTicketNotes);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WorkStartDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.StreetNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SAPWorkOrderNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Street);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TotalHours);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.NumberOfOfficers);
        }

        [TestMethod]
        public void TestCreatingWithWorkOrderIdPresetsFields()
        {
            //TODO: Fix this, create list doesn't work as it should for mapicons
            for (var x = 1; x <= 5; x++) 
            {
                GetFactory<MapIconFactory>().Create(new {FileName = "Foo." + x });
            }
            
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                AccountCharged = "1234",
                StreetNumber = "4321",
                SAPWorkOrderNumber = (long)42
            });
            var target = new CreateTrafficControlTicket(_container, workOrder.Id);

            Assert.AreEqual(target.WorkOrder, workOrder.Id);
            Assert.AreEqual(target.OperatingCenter, workOrder.OperatingCenter.Id);
            Assert.AreEqual(target.Town, workOrder.Town.Id);
            Assert.AreEqual(target.Street, workOrder.Street.Id);
            Assert.AreEqual(target.StreetNumber, workOrder.StreetNumber);
            Assert.AreEqual(target.AccountingCode, workOrder.AccountCharged);
            Assert.AreEqual(target.SAPWorkOrderNumber, workOrder.SAPWorkOrderNumber);
            Assert.AreEqual(target.CrossStreet, workOrder.NearestCrossStreet.Id);
            Assert.IsNotNull(target.Coordinate);
        }

        [TestMethod]
        public void TestDateDefaultsToCurrentDate()
        {
            var target = new CreateTrafficControlTicket(_container);

            Assert.AreEqual(_now, target.WorkStartDate);
        }

        [TestMethod]
        public void TestStreetCanMapBothWays()
        {
            var fee1 = GetEntityFactory<MerchantTotalFee>().Create(new { Fee = 0.021m, IsCurrent = true });
            var street = GetEntityFactory<Street>().Create(new {FullStName = "Foo", Name = "bar"});
            _entity.Street = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.Street);

            _entity.Street = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.Street);
        }

        [TestMethod]
        public void TestCrossStreetCanMapBothWays()
        {
            var fee1 = GetEntityFactory<MerchantTotalFee>().Create(new { Fee = 0.021m, IsCurrent = true });
            var street = GetEntityFactory<Street>().Create(new { FullStName = "Foo", Name = "bar" });
            _entity.CrossStreet = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.CrossStreet);

            _entity.CrossStreet = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.CrossStreet);
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var fee1 = GetEntityFactory<MerchantTotalFee>().Create(new { Fee = 0.021m, IsCurrent = true });
            var coordinate = GetFactory<CoordinateFactory>().Create();
            _entity.Coordinate = coordinate;

            _vmTester.MapToViewModel();

            Assert.AreEqual(coordinate.Id, _viewModel.Coordinate);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();

            Assert.AreSame(coordinate, _entity.Coordinate);
        }

        [TestMethod]
        public void TestMapToEntitySetsMerchantTotalFee()
        {
            var fee1 = GetEntityFactory<MerchantTotalFee>().Create(new { Fee = 0.021m, IsCurrent = false});
            var fee2 = GetEntityFactory<MerchantTotalFee>().Create(new { Fee = 0.023m, IsCurrent = true});
            var fee3 = GetEntityFactory<MerchantTotalFee>().Create(new { Fee = 0.025m, IsCurrent = false});

            var ticket = GetEntityFactory<TrafficControlTicket>().Create();
            var target = _viewModelFactory.Build<CreateTrafficControlTicket, TrafficControlTicket>(ticket);

            var result = target.MapToEntity(ticket);

            Assert.AreEqual(result.MerchantTotalFee, fee2);
        }

        #endregion
    }

    [TestClass]
    public class EditTrafficControlTicketTest : MapCallMvcInMemoryDatabaseTestBase<TrafficControlTicket>
    {
        #region Fields

        private ViewModelTester<EditTrafficControlTicket, TrafficControlTicket> _vmTester;
        private EditTrafficControlTicket _viewModel;
        private TrafficControlTicket _entity;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);

            i.For<IDateTimeProvider>().Use(new TestDateTimeProvider(DateTime.Now));
            i.For<IAuthenticationService<User>>().Use(new Mock<IAuthenticationService<User>>().Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditTrafficControlTicket(_container);
            _entity = new TrafficControlTicket();
            _vmTester = new ViewModelTester<EditTrafficControlTicket, TrafficControlTicket>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.WorkStartDate);
            _vmTester.CanMapBothWays(x => x.WorkEndDate);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.SAPWorkOrderNumber);
            _vmTester.CanMapBothWays(x => x.TotalHours);
            _vmTester.CanMapBothWays(x => x.NumberOfOfficers);
            _vmTester.CanMapBothWays(x => x.AccountingCode);
            _vmTester.CanMapBothWays(x => x.InvoiceNumber);
            _vmTester.CanMapBothWays(x => x.InvoiceDate);
            _vmTester.CanMapBothWays(x => x.InvoiceAmount);
            _vmTester.CanMapBothWays(x => x.InvoiceTotalHours);
            _vmTester.CanMapBothWays(x => x.DateApproved);
            _vmTester.CanMapBothWays(x => x.TrafficControlTicketNotes);
            _vmTester.CanMapBothWays(x => x.TrackingNumber);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WorkStartDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.StreetNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SAPWorkOrderNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Street);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TotalHours);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.NumberOfOfficers);
        }
        
        [TestMethod]
        public void TestStreetCanMapBothWays()
        {
            var street = GetEntityFactory<Street>().Create(new { FullStName = "Foo", Name = "bar" });
            _entity.Street = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.Street);

            _entity.Street = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.Street);
        }

        [TestMethod]
        public void TestCrossStreetCanMapBothWays()
        {
            var street = GetEntityFactory<Street>().Create(new { FullStName = "Foo", Name = "bar" });
            _entity.CrossStreet = street;

            _vmTester.MapToViewModel();

            Assert.AreEqual(street.Id, _viewModel.CrossStreet);

            _entity.CrossStreet = null;
            _vmTester.MapToEntity();

            Assert.AreSame(street, _entity.CrossStreet);
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var coordinate = GetFactory<CoordinateFactory>().Create();
            _entity.Coordinate = coordinate;

            _vmTester.MapToViewModel();

            Assert.AreEqual(coordinate.Id, _viewModel.Coordinate);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();

            Assert.AreSame(coordinate, _entity.Coordinate);
        }

        #endregion
    }
}
