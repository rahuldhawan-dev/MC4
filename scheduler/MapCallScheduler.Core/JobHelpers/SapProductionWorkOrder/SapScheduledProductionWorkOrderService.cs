using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallScheduler.Library.Common;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate.Util;

namespace MapCallScheduler.JobHelpers.SapProductionWorkOrder
{
    public class SapScheduledProductionWorkOrderService : ISapScheduledProductionWorkOrderService
    {
        #region Constants

        public const string NO_ORDER_NUMBER = "Unable to import order as the order number is null";

        #endregion
        #region Private Members

        protected readonly ILog _log;
        protected readonly ISAPCreatePreventiveWorkOrderRepository _remoteRepo;
        protected readonly IRepository<ProductionWorkOrder> _localRepo;
        protected readonly IRepository<PlanningPlant> _planningPlantRepository;
        protected readonly IRepository<Equipment> _equipmentRepository;
        protected readonly IRepository<Coordinate> _coordinateRepository;
        protected readonly IRepository<ProductionWorkOrderPriority> _productionWorkOrderPriorityRepository;
        protected readonly IRepository<WorkOrderPurpose> _workOrderPurposeRepository;
        protected readonly IRepository<ProductionWorkDescription> _productionWorkDescriptionRepository;
        protected readonly IRepository<OrderType> _orderTypeRepository;
        protected readonly IRepository<PlantMaintenanceActivityType> _plantMaintenanceActivityTypeRepository;
        protected readonly IRepository<ProductionSkillSet> _productionSkillSetRepository;
        protected readonly IDateTimeProvider _dateTimeProvider;
        
        #endregion

        public SapScheduledProductionWorkOrderService(ILog log, ISAPCreatePreventiveWorkOrderRepository remoteRepo, IRepository<ProductionWorkOrder> localRepo, 
            IRepository<PlanningPlant> planningPlantRepository, 
            IRepository<Coordinate> coordinateRepository,
            IRepository<ProductionWorkDescription> productionWorkDescriptionRepository,
            IRepository<WorkOrderPurpose> workOrderPurposeRepository,
            IRepository<ProductionWorkOrderPriority> workOrderPriorityRepository,
            IRepository<Equipment> equipmentRepository, 
            IRepository<OrderType> orderTypeRepository, 
            IRepository<PlantMaintenanceActivityType> plantMaintenanceActivityTypeRepository,
            IRepository<ProductionSkillSet> productionSkillSetRepository, IDateTimeProvider dateTimeProvider)
        {
            _log = log;
            _remoteRepo = remoteRepo;
            _localRepo = localRepo;
            _planningPlantRepository = planningPlantRepository;
            _coordinateRepository = coordinateRepository;
            _equipmentRepository = equipmentRepository;
            _productionWorkDescriptionRepository = productionWorkDescriptionRepository;
            _productionWorkOrderPriorityRepository = workOrderPriorityRepository;
            _workOrderPurposeRepository = workOrderPurposeRepository;
            _orderTypeRepository = orderTypeRepository;
            _plantMaintenanceActivityTypeRepository = plantMaintenanceActivityTypeRepository;
            _productionSkillSetRepository = productionSkillSetRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public virtual void Process()
        {
            var now = _dateTimeProvider.GetCurrentDate();
            var fifteenMinutesAgo = now.Subtract(TimeSpan.FromMinutes(15)).ToString("hhmmss");

            var search = new SAPCreatePreventiveWorkOrder {CreatedOn = now.ToString("yyyyMMdd"), LastRunTime = fifteenMinutesAgo };
            
            _log.Info($"Processing Production Work Orders: Date :{search.CreatedOn } Time: {search.LastRunTime}");

            var newOrders = _remoteRepo.Search(search);
            if (newOrders.Count() == 1 && newOrders.First().SAPErrorCode == "No Records found for given input.")
                return;
            ProcessOrders(newOrders, search);
        }

        protected void ProcessOrders(SAPCreatePreventiveWorkOrderCollection newOrders, SAPCreatePreventiveWorkOrder search)
        {
            foreach (var order in newOrders)
            {
                if (string.IsNullOrWhiteSpace(order.OrderNumber))
                {
                    _log.Info(NO_ORDER_NUMBER);
                }
                /// Don't replace this with a .Any, it won't like that, and you'll be confused, but it's likely due to the HasOne relationship that looks at a View.
                else if (_localRepo.Where(x => x.SAPWorkOrder == order.OrderNumber).Count() == 0)
                {
                    var entity = MapRecord(order, search);
                    if (entity?.PlanningPlant == null)
                    {
                        _log.Info($"Production Work Order Error: Unable to map order {order.OrderNumber} for pp/eq: {order.PlanningPlant}/{order.Equipment} Child Eq: {entity?.Equipments.Count(x => x.Equipment != null)}");
                        continue;
                    }
                    else if ((entity.Equipment != null || entity.Equipments.Any(x => x.Equipment != null)))
                    {
                        _localRepo.Save(entity);
                        _log.Info($"Inserting : ProductionWorkOrder {entity.Id}, {entity.OperatingCenter}, {entity.SAPWorkOrder}, {order.Equipment }");
                    }
                    else
                    {
                        _log.Info($"Production Work Order Error: Unable to map order {order.OrderNumber} for pp/eq: {order.PlanningPlant}/{order.Equipment} Child Eq: {entity?.Equipments.Count(x => x.Equipment != null)}");
                    }
                }
                else
                {
                    _log.Info($"Order#: {order.OrderNumber} Already exists");
                }
            }
        }
        
        protected ProductionWorkOrder MapRecord(SAPCreatePreventiveWorkOrder order, SAPCreatePreventiveWorkOrder search)
        {
            var planningPlant = _planningPlantRepository.Where(x => x.Code == order.PlanningPlant).FirstOrDefault();
            var equipment = _equipmentRepository.Where(x => order.Equipment != null && x.SAPEquipmentId.ToString() == order.Equipment.TrimStart('0')).FirstOrDefault();
            
            if (planningPlant != null)
            {
                var priority = _productionWorkOrderPriorityRepository.Where(p => p.Id == (int)ProductionWorkOrderPriority.Indices.ROUTINE).Single();
                
                var entity = new ProductionWorkOrder {
                    OperatingCenter = planningPlant.OperatingCenter,
                    PlanningPlant = planningPlant,
                    Facility = equipment?.Facility,
                    FunctionalLocation = order.FunctionalLocation,
                    SAPWorkOrder = order.OrderNumber,
                    Coordinate = equipment?.Coordinate ?? equipment?.Facility?.Coordinate,
                    Priority = priority,
                    DateReceived = DateTime.ParseExact(search.CreatedOn, "yyyyMMdd", CultureInfo.InvariantCulture),
                    BreakdownIndicator = false,
                    EquipmentType = equipment?.EquipmentType
                };
                if (!string.IsNullOrWhiteSpace(order.BasicStart) && order.BasicStart != "00000000")
                    entity.BasicStart = DateTime.ParseExact(order.BasicStart, "yyyyMMdd", CultureInfo.InvariantCulture);
                if (!string.IsNullOrWhiteSpace(order.BasicFinish) && order.BasicFinish != "00000000")
                    entity.BasicFinish = DateTime.ParseExact(order.BasicFinish, "yyyyMMdd", CultureInfo.InvariantCulture);
                
                MapChildEquipment(order, equipment, entity);

                if (!string.IsNullOrEmpty(order.SAPNotificationNumber))
                    entity.SAPNotificationNumber = long.Parse(order.SAPNotificationNumber);

                // We want to do the same check that we are doing in Process() to avoid creating duplicate Work Descriptions or work descriptions not linked to work orders
                if (entity.Equipment != null || entity.Equipments.Any())
                {
                    entity.ProductionWorkDescription = GetProductionWorkDescription(order, equipment);
                }

                return entity;
            }
            return null;
        }

        private void MapChildEquipment(SAPCreatePreventiveWorkOrder order, Equipment equipment, ProductionWorkOrder entity)
        {
            var equipments = new List<ProductionWorkOrderEquipment>();

            if (order.SapWorkOrderObjectList != null)
            {
                foreach (var eq in order.SapWorkOrderObjectList)
                {
                    var childequipment = _equipmentRepository
                        .Where(x => x.SAPEquipmentId.ToString() == eq.SAPEquipmentNumber.TrimStart('0'))
                        .FirstOrDefault();
                    
                    // we don't want the child equipment if it's the same as the parent equipment, we already added it above
                    if (entity.Equipment != null && childequipment != null && equipment.Id == childequipment.Id)
                        continue;

                    // if the order's Facility is null try to set it to a matched equipments facility.
                    if (entity.Equipment == null && entity.Facility == null && childequipment != null && childequipment.Facility != null)
                        entity.Facility = childequipment.Facility;

                    // add all the equipment sent even if it doesn't link
                    if (!string.IsNullOrEmpty(eq.SAPEquipmentNumber))
                    {
                        equipments.Add(new ProductionWorkOrderEquipment {
                            SAPEquipmentId = int.Parse(eq.SAPEquipmentNumber),
                            Equipment = childequipment,
                            ProductionWorkOrder = entity
                        });
                    }
                }
            }

            // add parent equipment to equipments list if it's not already in there
            if (equipment != null && equipments.All(e => e.Equipment.Id != equipment.Id))
            {
                equipments.Add(new ProductionWorkOrderEquipment { Equipment = equipment, ProductionWorkOrder = entity, IsParent = true });
            }
            // if we only have one piece of equipment, it's the parent
            if (equipments.Count == 1)
            {
                equipments[0].IsParent = true;
            }

            entity.Equipments = new HashSet<ProductionWorkOrderEquipment>(equipments.Distinct().ToList());
        }

        protected ProductionWorkDescription GetProductionWorkDescription(SAPCreatePreventiveWorkOrder order, Equipment equipment)
        {
            var description = _productionWorkDescriptionRepository.Where(x =>
                x.Description == order.ShortText
                && x.OrderType.SAPCode == order.OrderType
                && x.PlantMaintenanceActivityType.Code == order.MaintenanceActivityType
                && x.BreakdownIndicator == false
                && x.ProductionSkillSet.Description == order.MaintenanceWorkcenter).FirstOrDefault();
            if (description != null)
                return description;

            var orderType = _orderTypeRepository.Where(ot => ot.SAPCode == order.OrderType).FirstOrDefault();
            var pmat = _plantMaintenanceActivityTypeRepository.Where(x => x.Code == order.MaintenanceActivityType).FirstOrDefault();
            var skillSet = _productionSkillSetRepository.Where(x => x.Description == order.MaintenanceWorkcenter).FirstOrDefault();

            description = _productionWorkDescriptionRepository.Save(new ProductionWorkDescription {
                        Description = order.ShortText,
                        EquipmentType = equipment?.EquipmentType,
                        OrderType = orderType,
                        PlantMaintenanceActivityType = pmat,
                        BreakdownIndicator = false,
                        ProductionSkillSet = skillSet
                    });

            return description;
        }

    }

    public interface ISapScheduledProductionWorkOrderService : IProcessableService { }
}
