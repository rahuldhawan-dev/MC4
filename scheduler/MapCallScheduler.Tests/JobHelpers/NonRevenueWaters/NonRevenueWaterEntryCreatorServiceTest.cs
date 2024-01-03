using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallScheduler.JobHelpers.NonRevenueWaterEntryCreator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.NonRevenueWaters
{
    [TestClass]
    public class NonRevenueWaterEntryCreatorServiceTest : InMemoryDatabaseTest<NonRevenueWaterEntry>
    {
        private Mock<ILog> _log;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IRepository<OperatingCenter>> _operatingCenterRepositoryMock;
        private Mock<IRepository<WorkOrder>> _workOrderRepositoryMock;
        private Mock<IHydrantInspectionRepository> _hydrantInspectionRepositoryMock;
        private Mock<IRepository<NonRevenueWaterEntry>> _nonRevenueWaterRepositoryMock;
        private NonRevenueWaterEntryCreatorService _target;
        
        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject((_log = new Mock<ILog>()).Object);
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject((_operatingCenterRepositoryMock = new Mock<IRepository<OperatingCenter>>()).Object);
            _container.Inject((_workOrderRepositoryMock = new Mock<IRepository<WorkOrder>>()).Object);
            _container.Inject((_hydrantInspectionRepositoryMock = new Mock<IHydrantInspectionRepository>()).Object);
            _container.Inject((_nonRevenueWaterRepositoryMock = new Mock<IRepository<NonRevenueWaterEntry>>()).Object);
            _container.Inject(_dateTimeProvider.Object);
            _target = _container.GetInstance<NonRevenueWaterEntryCreatorService>();

            var operatingCenters = new List<OperatingCenter> {
                GetFactory<OperatingCenterFactory>().Create(new {
                     OperatingCenterCode = "PA1", OperatingCenterName = "Downingtown"
                }),
                GetFactory<OperatingCenterFactory>().Create(new {
                    OperatingCenterCode = "PA2", OperatingCenterName = "Lyndell"
                }),
                GetFactory<OperatingCenterFactory>().Create(new {
                    OperatingCenterCode = "PA3", OperatingCenterName = "Milford Mills"
                })
            }.AsQueryable();
            
            var hydrantFlushingReportItems = new List<HydrantFlushingReportItem> {
                new HydrantFlushingReportItem {
                    Month = DateTime.Now.AddMonths(-1).Month,
                    BusinessUnit = "8675309",
                    TotalGallons = 100
                }
            }.AsEnumerable();
            
            var workDescription = new WorkDescription { Description = "WATER MAIN BREAK REPAIR" };
            workDescription.SetPropertyValueByName("Id", WorkDescriptionRepository.MAIN_BREAKS[0]);

            var workOrder = new WorkOrder {
                DateCompleted = DateTime.Now.AddMonths(-1),
                LostWater = 9999,
                BusinessUnit = "123456",
                WorkDescription = workDescription,
                OperatingCenter = operatingCenters.FirstOrDefault(x => x.OperatingCenterCode == "PA1")
            };
            var workOrders = new List<WorkOrder> { workOrder };

            _workOrderRepositoryMock.Setup(x => x.Save(workOrder));
            _workOrderRepositoryMock.Setup(x => 
                                         x.Where(It.IsAny<Expression<Func<WorkOrder, bool>>>()))
                                    .Returns(workOrders.AsQueryable());

            _operatingCenterRepositoryMock.Setup(x => x.Linq)
                                          .Returns(operatingCenters);

            _hydrantInspectionRepositoryMock
               .Setup(x =>
                    x.GetFlushingReport(It.Is<SearchHydrantFlushing>(s =>
                        s.OperatingCenter == 2 && s.Year == DateTime.Now.AddMonths(-1).Year)))
               .Returns(hydrantFlushingReportItems);
        }

        [TestMethod]
        public void Test_Process_DoesNotContinueProcessingWhenCurrentDayIsNotTheFirstOfTheMonth()
        {
            _dateTimeProvider.Setup(x => 
                x.GetCurrentDate()).Returns(DateTime.Now.GetEndOfMonth().Date);
            
            _target.Process();
            
            _operatingCenterRepositoryMock.Verify(x => x.Linq, Times.Never);
        }

        [TestMethod]
        public void Test_Process_CreatesNonRevenueWaterEntriesOnTheFirstOfTheMonth()
        {
            _dateTimeProvider.Setup(x => 
                x.GetCurrentDate()).Returns(DateTime.Now.GetBeginningOfMonth().Date);
            
            _target.Process();
            
            _operatingCenterRepositoryMock.Verify(x => x.Linq, Times.Once);
            _log.Verify(x => x.Info($"NonRevenueWater entry created for PA1 - Downingtown with 1 water loss records and 0 hydrant flushing records."));
            _log.Verify(x => x.Info($"NonRevenueWater entry created for PA2 - Lyndell with 0 water loss records and 1 hydrant flushing records."));
            _log.Verify(x => x.Info($"NonRevenueWater entry created for PA3 - Milford Mills with 0 water loss records and 0 hydrant flushing records."));
            _nonRevenueWaterRepositoryMock.Verify(x => x.Save(It.Is<List<NonRevenueWaterEntry>>(l => l.Count == 3)), Times.Exactly(1));
        }
    }
}
