using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallScheduler.Library.Common;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate.Util;
using StructureMap;

namespace MapCallScheduler.JobHelpers.SapProductionWorkOrder
{
/*
#if DEBUG

    public class SapManualProductionWorkOrderService : SapScheduledProductionWorkOrderService, SapManualProductionWorkOrderService.ISapManualProductionWorkOrderService
    {
        public SapManualProductionWorkOrderService(
            ILog log, 
            ISAPCreatePreventiveWorkOrderRepository remoteRepo, 
            IRepository<ProductionWorkOrder> localRepo, 
            IRepository<PlanningPlant> planningPlantRepository, 
            IRepository<Coordinate> coordinateRepository, 
            IRepository<ProductionWorkDescription> productionWorkDescriptionRepository, 
            IRepository<WorkOrderPurpose> workOrderPurposeRepository, 
            IRepository<WorkOrderPriority> workOrderPriorityRepository, 
            IRepository<Equipment> equipmentRepository, 
            IRepository<OrderType> orderTypeRepository, 
            IRepository<PlantMaintenanceActivityType> plantMaintenanceActivityTypeRepository, 
            IRepository<ProductionSkillSet> productionSkillSetRepository,
            IDateTimeProvider dateTimeProvider) : 
            base(
                log, 
                remoteRepo, 
                localRepo, 
                planningPlantRepository, 
                coordinateRepository, 
                productionWorkDescriptionRepository, 
                workOrderPurposeRepository, 
                workOrderPriorityRepository, 
                equipmentRepository, 
                orderTypeRepository, 
                plantMaintenanceActivityTypeRepository, 
                productionSkillSetRepository, 
                dateTimeProvider)
        {

        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public override void Process()
        {
            var start = new DateTime(2018, 10, 24);
            var end = new DateTime(2018, 10, 24);
            foreach (DateTime day in EachDay(start, end))
            {
                var search = new SAPCreatePreventiveWorkOrder { CreatedOn = day.ToString("yyyyMMdd"), LastRunTime = "000000"};
                _log.Info($"Processing Production Work Orders: Date :{search.CreatedOn} Time: {search.LastRunTime}");

                var newOrders = _remoteRepo.Search(search);
                _log.Info($"Processing {newOrders.Count()} Production Work Orders: Date :{search.CreatedOn} Time: {search.LastRunTime}");

                if (newOrders.Count() == 1 && newOrders.First().SAPErrorCode == $"No Records found for given input. {day}")
                    return;
                ProcessOrders(newOrders, search);
            }
        }
        
        public interface ISapManualProductionWorkOrderService : IProcessableService { }
    }

#endif
*/
}
