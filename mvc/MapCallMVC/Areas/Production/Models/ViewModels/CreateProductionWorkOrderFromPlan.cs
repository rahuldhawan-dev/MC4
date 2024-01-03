using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CreateProductionWorkOrderFromPlan : ViewModel<ProductionWorkOrder>
    {
        #region Properties

        [Required, DateTimePicker]
        public DateTime? BasicStart { get; set; }

        [EntityMap, EntityMustExist(typeof(MaintenancePlan))]
        public int? MaintenancePlan { get; set; }

        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(PlanningPlant))]
        public int? PlanningPlant { get; set; }

        [EntityMap, EntityMustExist(typeof(Facility))]
        public int? Facility { get; set; }

        [EntityMap, EntityMustExist(typeof(EquipmentType))]
        public int? EquipmentType { get; set; }

        [EntityMap, EntityMustExist(typeof(ProductionWorkDescription))]
        public int? ProductionWorkDescription { get; set; }

        public string FunctionalLocation { get; set; }
        
        [EntityMap, EntityMustExist(typeof(ProductionWorkOrderPriority))]
        public int? Priority { get; set; }

        [EntityMap, EntityMustExist(typeof(CorrectiveOrderProblemCode))]
        public int? CorrectiveOrderProblemCode { get; set; }

        // Equipment is logical property and not a table property in production work order entity.
        // All equipments related to pwo are stored in ProductionWorkOrderEquipment table.
        // This is mapped in MapToEntity method.
        [Required, DoesNotAutoMap, EntityMustExist(typeof(Equipment))]
        public int[] Equipment { get; set; }

        public bool? BreakdownIndicator { get; set; } = true;

        [AutoMap(MapDirections.None)]
        public decimal? Latitude { get; set; }

        [AutoMap(MapDirections.None)]
        public decimal? Longitude { get; set; }

        [EntityMap, EntityMustExist(typeof(Employee))]
        public int? RequestedBy { get; set; }

        [StringLength(ProductionWorkOrder.StringLengths.NOTES)]
        public string OrderNotes { get; set; }

        [StringLength(ProductionWorkOrder.StringLengths.LOCAL_TASK_DESCRIPTION)]
        public string LocalTaskDescription { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? DueDate { get; set; }

        #endregion

        #region Constructors

        public CreateProductionWorkOrderFromPlan(IContainer container) : base(container) { }

        #endregion

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            entity = base.MapToEntity(entity);

            if (Equipment != null)
            {
                foreach (var eq in Equipment)
                {
                    var singleEquipment = _container.GetInstance<RepositoryBase<Equipment>>().Find(eq);
                    entity.Equipments.Add(new ProductionWorkOrderEquipment {
                        ProductionWorkOrder = entity,
                        Equipment = singleEquipment,
                        IsParent = true
                    });
                }
            }

            entity.DateReceived = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();

            if (entity.MaintenancePlan == null)
            {
                return entity;
            }

            var plan = _container.GetInstance<RepositoryBase<MaintenancePlan>>().Find(entity.MaintenancePlan.Id);
            entity.StartDate = DateTime.Today;
            entity.DueDate = entity.ProductionWorkOrderFrequency.GetFrequencyNextEndDate(entity.DateReceived.Value);
            entity.EstimatedCompletionHours = plan.EstimatedHours ?? 0;

            return entity;
        }
    }
}



