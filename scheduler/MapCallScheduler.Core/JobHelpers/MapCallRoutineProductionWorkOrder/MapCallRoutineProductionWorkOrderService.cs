using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;

namespace MapCallScheduler.JobHelpers.MapCallRoutineProductionWorkOrder
{
    public class MapCallRoutineProductionWorkOrderService : IMapCallRoutineProductionWorkOrderService
    {
        #region Constants

        private const string NOTIFICATION_PURPOSE = "Production Work Order Assigned";
        private const int BATCH_SIZE = 2000;

        #endregion

        #region Private Members

        private readonly ILog _log;
        private readonly IMaintenancePlanRepository _maintenancePlanRepo;
        private readonly IProductionWorkOrderRepository _productionWorkOrderRepo;
        private readonly INotificationService _notificationService;
        private readonly string _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

        #endregion
       
        #region Constructors

        public MapCallRoutineProductionWorkOrderService(ILog log, 
            IMaintenancePlanRepository maintenancePlanRepo,
            IProductionWorkOrderRepository productionWorkOrderRepo,
            INotificationService notificationService)
        {
            _log = log;
            _maintenancePlanRepo = maintenancePlanRepo;
            _productionWorkOrderRepo = productionWorkOrderRepo;
            _notificationService = notificationService;
        }

        #endregion

        #region Exposed Methods

        public void Process()
        {
            var scheduledPlansBatchList = new List<List<ScheduledMaintenancePlan>>();
            var total = 0;
            
            _log.Info("Running MapCall Routine Production Work Order service.");
            
            _log.Info("Retrieving scheduled Maintenance Plans...");
            var scheduledPlans = _maintenancePlanRepo.GetOnlyScheduledMaintenancePlans().ToList();
            var totalScheduledPlansToProcess = scheduledPlans.Count;
            _log.Info($"Retrieved {totalScheduledPlansToProcess} scheduled Maintenance Plans");

            // If the number of Scheduled Plans exceeds the BATCH_SIZE, we want to chunk them into batches of
            // 2000. This fixes a sql error received on 1.1.2024 where too many parameters were passed to a sql 
            // "Where In(...) clause - can't pass more than 2100 at a time, so we went with 2000 as a nice even number.
            while (total < totalScheduledPlansToProcess)
            {
                scheduledPlansBatchList.Add(scheduledPlans.Skip(total).Take(BATCH_SIZE).ToList());
                total += BATCH_SIZE;
            }

            foreach (var batch in scheduledPlansBatchList)
            {
                var workOrdersToCancel = _productionWorkOrderRepo.GetAutoCancelRoutineProductionWorkOrders(batch);
                var generatedWorkOrders = _productionWorkOrderRepo.BuildRoutineProductionWorkOrdersFromScheduledPlans(batch);

                _log.Info($"AutoCancel is updating existing Production Work Order records...{batch.Count}");
                _productionWorkOrderRepo.CancelOrders(workOrdersToCancel);

                _log.Info("Creating new Production Work Order records...");
                var assignments = _productionWorkOrderRepo.SaveAllAndGetAssignmentsForNotifications(generatedWorkOrders);
            
                _maintenancePlanRepo.TrimScheduledAssignmentsUpToToday();

                _log.Info("Sending notifications to assigned Employees...");
                SendNotifications(assignments);
            }
            
            _log.Info("Completed MapCall Routine Production Work Order service.");
        }
        
        #endregion

        private void SendNotifications(IEnumerable<EmployeeAssignment> assignments)
        {
            foreach (var assignment in assignments)
            {
                if (!string.IsNullOrEmpty(assignment.AssignedTo?.EmailAddress))
                {
                    var notification = new ProductionWorkOrderNotification {
                        ProductionWorkOrder = assignment.ProductionWorkOrder,
                        RecordUrl = GetRecordUrl(assignment.ProductionWorkOrder)
                    };

                    _notificationService.Notify(new NotifierArgs {
                        OperatingCenterId = assignment.ProductionWorkOrder.OperatingCenter.Id,
                        Data = notification,
                        Address = assignment.AssignedTo.EmailAddress,
                        Module = EmployeeAssignmentRepository.ROLE,
                        Purpose = NOTIFICATION_PURPOSE
                    });
                }
            }
        }

        private string GetRecordUrl(ProductionWorkOrder entity) => 
            $"{_baseUrl}Production/{nameof(ProductionWorkOrder)}/Show/{entity.Id}";
    }
}
