using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using WorkDescriptionEntity = MapCall.Common.Model.Entities.WorkDescription;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    public class EditGeneralWorkOrderModel : WorkOrderViewModel
    {
        #region Private Members

        private WorkOrder _displayWorkOrder;

        #endregion

        #region Constructors

        public EditGeneralWorkOrderModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public WorkOrder WorkOrder
        {
            get
            {
                if (_displayWorkOrder == null)
                {
                    _displayWorkOrder = Original ?? _container.GetInstance<IWorkOrderRepository>().Find(Id);
                }
                return _displayWorkOrder;
            }
        }

        [DropDown("", nameof(TownSection), "ActiveByTownId", DependsOn = "Town", PromptText = "Please select a town")]
        [EntityMap, EntityMustExist(typeof(TownSection))]
        public int? TownSection { get; set; }

        [AutoComplete(
            "",
            nameof(Street),
            "GetByTownIdAndPartialStreetName",
            DependsOn = nameof(Town),
            PlaceHolder = "Please select a Town and enter more than 2 characters",
            DisplayProperty = nameof(MapCall.Common.Model.Entities.Street.FullStName))]
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }

        [AutoComplete(
            "",
            nameof(Street),
            "GetByTownIdAndPartialStreetName",
            DependsOn = nameof(Town),
            PlaceHolder = "Please select a Town and enter more than 2 characters",
            DisplayProperty = nameof(MapCall.Common.Model.Entities.Street.FullStName))]
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? NearestCrossStreet { get; set; }

        [StringLength(WorkOrder.StringLengths.ZIP_CODE)]
        public string ZipCode { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(WorkOrderPurpose))]
        public int? Purpose { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(WorkOrderPriority))]
        public int? Priority { get; set; }

        [RequiredWhen(nameof(OriginalWorkDescription), ComparisonType.EqualToAny,
            nameof(MainBreakWorkDescriptions), typeof(EditGeneralWorkOrderModel)), 
         DropDown, EntityMap, EntityMustExist(typeof(CustomerImpactRange))]
        public int? EstimatedCustomerImpact { get; set; }

        [RequiredWhen(nameof(OriginalWorkDescription), ComparisonType.EqualToAny,
             nameof(MainBreakWorkDescriptions), typeof(EditGeneralWorkOrderModel)), 
         DropDown, EntityMap, EntityMustExist(typeof(RepairTimeRange))]
        public int? AnticipatedRepairTime { get; set; }

        public bool? AlertIssued { get; set; }

        public bool? SignificantTrafficImpact { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(MarkoutRequirement))]
        public int? MarkoutRequirement { get; set; }

        [CheckBox]
        public bool TrafficControlRequired { get; set; }

        [CheckBox]
        public bool StreetOpeningPermitRequired { get; set; }

        [CheckBox]
        public override bool DigitalAsBuiltRequired { get; set; }

        [CheckBox]
        public bool DigitalAsBuiltCompleted { get; set; }

        [AutoMap(MapDirections.None)]
        public int? OriginalWorkDescription => WorkOrder?.WorkDescription?.Id;

        [Required, DropDown("FieldOperations", nameof(WorkDescription), "ActiveByAssetTypeIdAndIsRevisit", DependsOn = nameof(AssetType) + "," + nameof(IsRevisit), PromptText = "Please select an Asset Type above."),
         EntityMap, EntityMustExist(typeof(WorkDescriptionEntity))]
        public override int? WorkDescription { get; set; }

        [DropDown("", "User", "FieldServicesWorkManagementUsersByOperatingCenter", DependsOn = nameof(OperatingCenter), PromptText = "Please select an Operating Center above."), 
         EntityMap, EntityMustExist(typeof(User)),
         RequiredWhen(nameof(RequestedBy), ComparisonType.EqualTo, WorkOrderRequester.Indices.EMPLOYEE)]
        public int? RequestingEmployee { get; set; }

        [StringLength(WorkOrder.StringLengths.CUSTOMER_NAME),
         RequiredWhen(nameof(RequestedBy), ComparisonType.EqualTo, WorkOrderRequester.Indices.CUSTOMER)]
        public string CustomerName { get; set; }

        [StringLength(WorkOrder.StringLengths.PHONE_NUMBER),
         RequiredWhen(nameof(RequestedBy), ComparisonType.EqualTo, WorkOrderRequester.Indices.CUSTOMER)]
        public string PhoneNumber { get; set; }

        [StringLength(WorkOrder.StringLengths.SECONDARY_PHONE_NUMBER)]
        public string SecondaryPhoneNumber { get; set; }

        public DateTime? PlannedCompletionDate { get; set; }

        [DoesNotAutoMap]
        public override string MeterSerialNumber { get; set; }

        [DoesNotAutoMap]
        public long? DeviceLocation { get; set; }

        [DoesNotAutoMap]
        public long? Installation { get; set; }

        [DoesNotAutoMap]
        public override long? SAPEquipmentNumber { get; set; }

        [DoesNotAutoMap]
        public override int? Service { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MeterLocation))]
        public int? MeterLocation { get; set; }

        [Multiline, StringLength(int.MaxValue)]
        public string SpecialInstructions { get; set; }
        
        #region Logical Properties

        [AutoMap(MapDirections.None)]
        public bool WorkDescriptionEditable { get; set; }

        [AutoMap(MapDirections.None)]
        public bool PlantMaintenanceActivityTypeOverrideEditable { get; set; }

        [AutoMap(MapDirections.None)]
        public bool AccountNumberEditable { get; set; }

        [AutoMap(MapDirections.None)]
        public bool? IsRevisit { get; set; }

        #endregion

        #endregion

        #region Private Methods

        public static int[] MainBreakWorkDescriptions() => WorkDescriptionEntity.GetMainBreakWorkDescriptions();

        public override void Map(WorkOrder entity)
        {
            base.Map(entity);
            if (entity.AssetType.IncludesCoordinate)
            {
                CoordinateId = entity.Asset.Coordinate.Id;
            }
            else if (!entity.AssetType.IncludesCoordinate &&
                entity.Coordinate.Id == 0 &&
                entity.Latitude != null &&
                entity.Longitude != null)
            {
                var coordinate = _container.GetInstance<IRepository<Coordinate>>()
                                           .Where(x => x.Latitude == entity.Latitude && x.Longitude == entity.Longitude)
                                           .FirstOrDefault();
                if (coordinate != null)
                {
                    CoordinateId = coordinate.Id;
                }
            }

            IsRevisit = entity.WorkDescription?.Revisit;

            PlantMaintenanceActivityTypeOverrideEditable = !entity.IsSAPUpdatableWorkOrder ||
                                                           (entity.PlantMaintenanceActivityTypeOverride == null && entity.ApprovedOn == null);

            WorkDescriptionEditable = (!entity.IsSAPUpdatableWorkOrder || entity.ApprovedOn == null) &&
                                      !entity.IsNewServiceInstallation;

            if (entity.PlantMaintenanceActivityTypeOverride != null &&
                entity.PlantMaintenanceActivityTypeOverride.Id == PlantMaintenanceActivityType.Indices.PBC)
            {
                AccountNumberEditable = false;
            }
            else
            {
                AccountNumberEditable = !entity.IsSAPUpdatableWorkOrder || entity.ApprovedOn == null;
            }
        }

        #endregion
    }
}