using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Utilities;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class ProductionWorkOrderViewModel : ViewModel<ProductionWorkOrder>
    {
        #region Private Members

        private int? _operatingCenter;

        #endregion

        #region Constructors

        public ProductionWorkOrderViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        #region Special

        [AutoMap(MapDirections.None)]
        public string GetOperatingCenterIsSAPWorkOrdersEnabled { get; set; }

        [AutoMap(MapDirections.None)]
        public bool SendToSAP { get; set; }

        #endregion

        [Required, EntityMap, EntityMustExist(typeof(ProductionWorkDescription))]
        [DropDown(Area = "Production", Controller = "ProductionWorkDescription", Action = "ByEquipmentTypeIdForCreate", DependsOn = "EquipmentType", PromptText = "Please select an equipment type.")]
        public int? ProductionWorkDescription { get; set; }

        [EntityMap, EntityMustExist(typeof(CorrectiveOrderProblemCode))]
        [DropDown(Area = "Production", Controller = "CorrectiveOrderProblemCode", Action = "ByEquipmentTypeId", DependsOn = "EquipmentType", PromptText = "Please select an equipment type.")]
        [RequiredWhen("ProductionWorkDescription", ComparisonType.EqualToAny, "GetCorrectiveWorkDescriptionIds", typeof(ProductionWorkOrderViewModel), FieldOnlyVisibleWhenRequired = true)]
        public virtual int? CorrectiveOrderProblemCode { get; set; }

        [Required]
        [RegularExpression(@"(?:\d*\.\d{1,2}|\d+)$", ErrorMessage = "Must be a number with maximum of 2 decimal places")]
        [View(ProductionWorkOrder.DisplayNames.ESTIMATED_COMPLETION_HOURS, FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public decimal? EstimatedCompletionHours { get; set; } 

        [Multiline, AllowHtml, RequiredWhen("CorrectiveOrderProblemCode", ComparisonType.EqualTo, MapCall.Common.Model.Entities.CorrectiveOrderProblemCode.Indices.OTHER)]
        public string OtherProblemNotes { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [DropDown("", "PlanningPlant", "ByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMap, EntityMustExist(typeof(PlanningPlant))]
        public virtual int? PlanningPlant { get; set; }

        [DropDown(Area = "", Controller = "Facility", Action = "ByOperatingCenterOrPlanningPlant", DependsOn = "OperatingCenter,PlanningPlant", PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        [EntityMap, EntityMustExist(typeof(Facility))]
        [Required]
        public virtual int? Facility { get; set; }

        [DropDown(Area = "", Controller = "FacilityFacilityArea", Action = "ByFacilityId", DependsOn = "Facility", PromptText = "Please select Facility above")]
        [EntityMap, EntityMustExist(typeof(FacilityFacilityArea))]
        public virtual int? FacilityFacilityArea { get; set; }

        [View(DisplayName = "SAP Function Location")]
        [StringLength(ProductionWorkOrder.StringLengths.FUNCTIONAL_LOCATION)]
        [UIHint("FunctionalLocation")]
        public virtual string FunctionalLocation { get; set; }

        [DropDown(Area = "", Controller = "EquipmentType", Action = "ByFacilityId", DependsOn = "Facility", PromptText = "Select a facility above.")]
        [EntityMap, EntityMustExist(typeof(EquipmentType))]
        public virtual int? EquipmentType { get; set; }

        [EntityMap, EntityMustExist(typeof(Equipment))]
        public virtual RedTagPermit RedTagPermit { get; set; }

        [Coordinate, EntityMap]
        public virtual int? Coordinate { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(ProductionWorkOrderPriority))]
        public virtual int? Priority { get; set; }

        public bool? IsEligibleForRedTagPermit { get; set; }

        public bool? IsRedTagPermitStillOpen { get; set; }

        [View(DisplayName = "WBS Override")]
        [DropDown, EntityMap, EntityMustExist(typeof(PlantMaintenanceActivityType))]
        public int? PlantMaintenanceActivityTypeOverride { get; set; }

        [EntityMap, EntityMustExist(typeof(Employee))]
        [Required]
        [DropDown]
        public virtual int? RequestedBy { get; set; }

        [Required, AllowHtml]
        [StringLength(ProductionWorkOrder.StringLengths.NOTES)]
        public virtual string OrderNotes { get; set; }

        public virtual bool? BreakdownIndicator { get; set; }

        public string SAPErrorCode { get; set; }

        //public long? SAPNotificationNumber { get; set; }

        [StringLength(ProductionWorkOrder.StringLengths.WBS_ELEMENT)]
        [RequiredWhen("PlantMaintenanceActivityTypeOverride", ComparisonType.NotEqualTo, null)]
        [ClientCallback("ProductionWorkOrderForm.validateWBSNumber", ErrorMessage = "WBS # is invalid.")]
        public string WBSElement { get; set; }

        public WorkOrderStatus Status { get; set; }

        [StringLength(ProductionWorkOrder.StringLengths.LOCAL_TASK_DESCRIPTION)]
        public virtual string LocalTaskDescription { get; set; }

        public virtual bool? AutoCreatedCorrectiveWorkOrder { get; set; }

        #endregion

        public static int[] GetCorrectiveWorkDescriptionIds()
        {
            return DependencyResolver.Current.GetService<IRepository<ProductionWorkDescription>>()
                                     .Where(wd => wd.OrderType.Id == OrderType.Indices.CORRECTIVE_ACTION_20)
                                     .Select(wd => wd.Id).ToArray();
        }

        public static int[] GetPMWorkDescriptionIds()
        {
            return DependencyResolver.Current.GetService<IRepository<ProductionWorkDescription>>()
                                     .Where(wd => wd.OrderType.Id == OrderType.Indices.PLANT_MAINTENANCE_WORK_ORDER_11)
                                     .Select(wd => wd.Id).ToArray();
        }

        #region Exposed Methods

        public override void Map(ProductionWorkOrder entity)
        {
            base.Map(entity);

            var operatingCenter = _container.GetInstance<IOperatingCenterRepository>().Find(OperatingCenter.Value);
            if (operatingCenter != null)
                SendToSAP = operatingCenter.SAPEnabled && !operatingCenter.IsContractedOperations && operatingCenter.SAPWorkOrdersEnabled;
        }

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            entity = base.MapToEntity(entity);

            if (entity.Coordinate == null)
            {
                entity.Coordinate = entity.Facility.Coordinate;
            }
            
            //Inherit Breakdown Indicator from the ProductionWorkDescription if available. 
            //Breakdown Indicator is true only for Corrective Work Order.
            entity.BreakdownIndicator = entity.ProductionWorkDescription?.BreakdownIndicator;

            return entity;
        }

        #endregion
    }
}