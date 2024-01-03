using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallScheduler.JobHelpers.MapCallRoutineProductionWorkOrder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using NHibernate.Impl;

namespace MapCallScheduler.Tests.JobHelpers.MapCallRoutineProductionWorkOrder
{
    [TestClass]
    public class MapCallRoutineProductionWorkOrderServiceTest : InMemoryDatabaseTest<ProductionWorkOrder>
    {
        private MapCallRoutineProductionWorkOrderService _target;
        private Mock<ILog> _log;
        private Mock<IMaintenancePlanRepository> _maintenancePlanRepo;
        private Mock<IProductionWorkOrderRepository> _productionWorkOrderRepo;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<INotificationService> _notificationService;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject((_log = new Mock<ILog>()).Object);
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Parse("01/01/2030"));
            _container.Inject(_dateTimeProvider.Object);

            _maintenancePlanRepo = new Mock<IMaintenancePlanRepository>();
            _container.Inject(_maintenancePlanRepo.Object);

            _productionWorkOrderRepo = new Mock<IProductionWorkOrderRepository>();
            _container.Inject(_productionWorkOrderRepo.Object);

            _notificationService = new Mock<INotificationService>();
            _container.Inject(_notificationService.Object);

            _target = _container.GetInstance<MapCallRoutineProductionWorkOrderService>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestProcessCreatesAndCancelsWorkOrdersOnSchedule()
        {
            // Arrange
            var today = _dateTimeProvider.Object.GetCurrentDate().Date;
            var startDate = today.AddMonths(-6);

            var dailyFrequency = GetFactory<ProductionWorkOrderFrequencyFactory>().Create();
            var planFactory = GetFactory<MaintenancePlanFactory>();
           
            var plan = planFactory.Build(new {
                Id = 1,
                Start = startDate,
                IsActive = true,
                ProductionWorkOrderFrequency = dailyFrequency,
            });
            var planWithAutoCancel = planFactory.Build(new {
                Id = 2,
                Start = startDate,
                IsActive = true,
                ProductionWorkOrderFrequency = dailyFrequency,
                HasACompletionRequirement = true,
            });

            var employeeAssignment = GetFactory<EmployeeAssignmentFactory>().Build();

            var pwoFactory = GetFactory<ProductionWorkOrderFactory>();

            var existingOrder = pwoFactory.Build(new {
                Id = 1,
                MaintenancePlan = planWithAutoCancel,
            });

            planWithAutoCancel.ProductionWorkOrders.Add(existingOrder);

            // Mock setup
            var scheduledPlans = new List<ScheduledMaintenancePlan> {
                new ScheduledMaintenancePlan(plan),
                new ScheduledMaintenancePlan(planWithAutoCancel),
            };

            _maintenancePlanRepo.Setup(x => 
                x.GetOnlyScheduledMaintenancePlans()).Returns(scheduledPlans);

            _productionWorkOrderRepo.Setup(x => x.BuildRoutineProductionWorkOrdersFromScheduledPlans(It.IsAny<IEnumerable<ScheduledMaintenancePlan>>()))
                                    .Returns(pwoFactory.BuildList(scheduledPlans.Count));

            var autoCancelOrders = new List<int> { existingOrder.Id };
            _productionWorkOrderRepo.Setup(x => x.GetAutoCancelRoutineProductionWorkOrders(It.IsAny<IEnumerable<ScheduledMaintenancePlan>>()))
                                    .Returns(autoCancelOrders);

            IEnumerable<ProductionWorkOrder> savedWorkOrders = null;
            _productionWorkOrderRepo.Setup(x => x.SaveAllAndGetAssignmentsForNotifications(It.IsAny<IEnumerable<ProductionWorkOrder>>()))
                                    .Callback((IEnumerable<ProductionWorkOrder> items) => savedWorkOrders = items)
                                    .Returns(new[] { employeeAssignment });

            IEnumerable<int> cancelledWorkOrderIds = null;
            _productionWorkOrderRepo.Setup(x => x.CancelOrders(It.IsAny<IEnumerable<int>>()))
                                    .Callback((IEnumerable<int> items) => cancelledWorkOrderIds = items);

            // Act
            _target.Process();

            // Assert
            _maintenancePlanRepo.Verify(x => x.GetOnlyScheduledMaintenancePlans(), Times.Once);
            _productionWorkOrderRepo.Verify(x => x.BuildRoutineProductionWorkOrdersFromScheduledPlans(It.IsAny<IEnumerable<ScheduledMaintenancePlan>>()), Times.Once);
            _productionWorkOrderRepo.Verify(x => x.CancelOrders(It.IsAny<IEnumerable<int>>()), Times.Once);
            _productionWorkOrderRepo.Verify(x => x.SaveAllAndGetAssignmentsForNotifications(It.IsAny<IEnumerable<ProductionWorkOrder>>()), Times.Once);
            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);

            Assert.AreEqual(2, savedWorkOrders.Count());
            Assert.AreEqual(1, cancelledWorkOrderIds.Count());
        }

        [TestMethod]
        public void TestProcessDoesNothingWhenThereAreNoScheduledPlansToday()
        {
            // Mock setup
            _maintenancePlanRepo.Setup(x => x.GetOnlyScheduledMaintenancePlans())
                                .Returns(new EnumerableQuery<ScheduledMaintenancePlan>(new List<ScheduledMaintenancePlan>()));

            _productionWorkOrderRepo.Setup(x => x.BuildRoutineProductionWorkOrdersFromScheduledPlans(It.IsAny<IEnumerable<ScheduledMaintenancePlan>>()))
                                    .Returns(new List<ProductionWorkOrder>());

            _productionWorkOrderRepo.Setup(x => x.GetAutoCancelRoutineProductionWorkOrders(It.IsAny<IEnumerable<ScheduledMaintenancePlan>>()))
                                    .Returns(new List<int>());

            IEnumerable<ProductionWorkOrder> savedWorkOrders = null;
            _productionWorkOrderRepo.Setup(x => x.SaveAllAndGetAssignmentsForNotifications(It.IsAny<IEnumerable<ProductionWorkOrder>>()))
                                    .Callback((IEnumerable<ProductionWorkOrder> items) => savedWorkOrders = items);

            IEnumerable<int> cancelledWorkOrderIds = null;
            _productionWorkOrderRepo.Setup(x => x.CancelOrders(It.IsAny<IEnumerable<int>>()))
                                    .Callback((IEnumerable<int> items) => cancelledWorkOrderIds = items);

            // Act
            _target.Process();

            // Assert
            _maintenancePlanRepo.Verify(x => x.GetOnlyScheduledMaintenancePlans(), Times.Once);
            _productionWorkOrderRepo.Verify(x => x.SaveAllAndGetAssignmentsForNotifications(It.IsAny<IEnumerable<ProductionWorkOrder>>()), Times.Once);
            _productionWorkOrderRepo.Verify(x => x.CancelOrders(It.IsAny<IEnumerable<int>>()), Times.Once);
            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
            Assert.AreEqual(savedWorkOrders.Count(), 0);
            Assert.AreEqual(cancelledWorkOrderIds.Count(), 0);
        }

        #endregion
    }
}