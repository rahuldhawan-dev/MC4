using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IContainer = StructureMap.IContainer;
using AssetTypeIndices = MapCall.Common.Model.Entities.AssetType.Indices;
using System.Linq;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public abstract class WorkOrderViewModel : ViewModel<WorkOrder>
    {
        public readonly string PREMISE_NUMBER_PLACEHOLDER = "0000000000";

        #region Constants

        public struct ErrorMessages
        {
            public const string
                STREET_NUMBER = "Please enter the nearest (or customer) house number.",
                COORDINATE = "Please enter the location for this order using the globe icon.",
                PREMISE_NUMBER = "The premise number is not valid.  Please enter notes explaining why a placeholder premise number was used.",
                BRB_PLANT_MAINTENANCE_ACTIVITY_CODE = "The plant maintenance activity code is required when selecting the current work description.";
        }

        public struct StringLengths
        {
            public const int CUSTOMER_NAME = 30,
                             ALERT_ID = 20,
                             PHONE_NUMBER = 14,
                             PREMISE_NUMBER = 10,
                             SECONDARY_PHONE_NUMBER = 14,
                             SERVICE_NUMBER = 50,
                             STREET_NUMBER = 20,
                             ZIP_CODE = 10,
                             MATERIALS_DOC_ID = 15,
                             CUSTOMER_ACCOUNT_NUMBER = 12,
                             ACCOUNT_CHARGED = 15,
                             INVOICE_NUMBER = 15,
                             BUSINESS_UNIT = 6;
        }

        #endregion

        #region Abstract Properties
        
        public abstract int? WorkDescription { get; set; }
        
        #endregion
        
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required(ErrorMessage = ErrorMessages.STREET_NUMBER),
         StringLength(WorkOrder.StringLengths.STREET_NUMBER)]
        public string StreetNumber { get; set; }

        [StringLength(WorkOrder.StringLengths.APARTMENT_ADDTL)]
        public string ApartmentAddtl { get; set; }

        [DropDown(
            "",
            nameof(Town),
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an operating center above")]
        [Required, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        //Cascade by OperatingCenter
        [Required, EntityMap, EntityMustExist(typeof(AssetType))]
        [DropDown(
            "",
            nameof(AssetType),
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Select an operating center above.")]
        public int? AssetType { get; set; }

        // ASSET IDS
        // Hydrant
        [RequiredWhen(
            nameof(AssetType),
            ComparisonType.EqualTo,
            MapCall.Common.Model.Entities.AssetType.Indices.HYDRANT)]
        [DropDown(
            "FieldOperations",
            nameof(Hydrant),
            "ByStreetIdForWorkOrders",
            DependsOn = nameof(Street),
            PromptText = "Select a street above.")]
        [EntityMap, EntityMustExist(typeof(Hydrant))]
        public int? Hydrant { get; set; }

        // Valve
        [RequiredWhen(
            nameof(AssetType),
            ComparisonType.EqualTo,
            MapCall.Common.Model.Entities.AssetType.Indices.VALVE)]
        [DropDown(
            "FieldOperations",
            nameof(Valve),
            "ByStreetIdForWorkOrders",
            DependsOn = nameof(Street),
            PromptText = "Select a street above.")]
        [EntityMap, EntityMustExist(typeof(Valve))]
        public int? Valve { get; set; }

        [EntityMap, EntityMustExist(typeof(Service))]
        public virtual int? Service { get; set; }

        // Sewer Opening
        [RequiredWhen(
            nameof(AssetType),
            ComparisonType.EqualTo,
            MapCall.Common.Model.Entities.AssetType.Indices.SEWER_OPENING)]
        [DropDown(
            "FieldOperations",
            nameof(SewerOpening),
            "ByStreetIdForWorkOrders",
            DependsOn = nameof(Street),
            PromptText = "Select a street above.")]
        [EntityMap, EntityMustExist(typeof(SewerOpening))]
        public int? SewerOpening { get; set; }

        // Equipment
        [RequiredWhen(
            nameof(AssetType),
            ComparisonType.EqualTo,
            MapCall.Common.Model.Entities.AssetType.Indices.EQUIPMENT)]
        [DropDown(
            "",
            nameof(Equipment),
            "ByTownIdForWorkOrders",
            DependsOn = nameof(Town),
            PromptText = "Select a town above.")]
        [EntityMap, EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }

        // Service
        // Sewer Lateral
        [RequiredWhen(
            nameof(AssetType),
            ComparisonType.EqualTo,
            AssetTypeIndices.SERVICE)]
        [ClientCallback("WorkOrders.validatePremiseNumber", ErrorMessage = ErrorMessages.PREMISE_NUMBER)]
        [StringLength(StringLengths.PREMISE_NUMBER, MinimumLength = 10)]
        public virtual string PremiseNumber { get; set; }

        [StringLength(StringLengths.SERVICE_NUMBER)]
        public virtual string ServiceNumber { get; set; }

        [RequiredWhen(
            nameof(AssetType),
            ComparisonType.EqualTo,
            MapCall.Common.Model.Entities.AssetType.Indices.MAIN_CROSSING)]
        [EntityMap, EntityMustExist(typeof(MainCrossing))]
        [DropDown(
            "Facilities",
            nameof(MainCrossing),
            "ByTownIdForWorkOrders",
            DependsOn = nameof(Town),
            PromptText = "Select a town above.")]
        public int? MainCrossing { get; set; }

        [DoesNotAutoMap("Mapped manually in MapToEntity. There's lots of stuff going on there.")]
        [Coordinate(AddressCallback = "WorkOrders.getAddress", IconSet = IconSets.WorkOrders)]
        [DisplayName("Coordinates")]
        [EntityMustExist(typeof(Coordinate))]
        [Required(ErrorMessage = ErrorMessages.COORDINATE)]
        public virtual int? CoordinateId { get; set; }

        // these should only be required when SAP enabled
        //[ClientCallback("WorkOrders.validateServiceForSap", ErrorMessage = "Required for SAP.")]
        //[RequiredWhen("GetOperatingCenterIsSAPWorkOrdersEnabled", ComparisonType.EqualTo, true)]
        [StringLength(WorkOrder.StringLengths.METER_SERIAL_NUMBER)]
        public virtual string MeterSerialNumber { get; set; }

        //[ClientCallback("WorkOrders.validateServiceForSap", ErrorMessage = "Required for SAP.")]
        [View(DisplayName = "Equipment")]
        public virtual long? SAPEquipmentNumber { get; set; }

        [View(DisplayName = "PMAT Override")]
        [DropDown, EntityMap, EntityMustExist(typeof(PlantMaintenanceActivityType))]
        [ClientCallback(
            "WorkOrders.validatePlantMaintenanceActivityCode",
            ErrorMessage = ErrorMessages.BRB_PLANT_MAINTENANCE_ACTIVITY_CODE)]
        public int? PlantMaintenanceActivityTypeOverride { get; set; }

        [RequiredWhen(
            nameof(PlantMaintenanceActivityTypeOverride),
            ComparisonType.EqualToAny,
            "GetOverrideCodesRequiringWBSNumber",
            typeof(PlantMaintenanceActivityType))]
        [ClientCallback("WorkOrders.validateWBSNumber", ErrorMessage = "WBS # is invalid.")]
        public string AccountCharged { get; set; }

        // Not used, in view only
        [DoesNotAutoMap]
        public virtual string PremiseAddress { get; set; }

        [DoesNotAutoMap]
        public virtual string ServiceUtilityType { get; set; }

        [DoesNotAutoMap]
        public virtual decimal? Latitude { get; set; }

        [DoesNotAutoMap]
        public virtual decimal? Longitude { get; set; }

        [DoesNotAutoMap("Not an actual View Property - set by MapToEntity and Used by Controller, also set in view via ajax")]
        public bool SendToSAP { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AcousticMonitoringType))]
        [RequiredWhen(nameof(RequestedBy), WorkOrderRequester.Indices.ACOUSTIC_MONITORING)]
        public int? AcousticMonitoringType { get; set; }

        // Requested By
        [Required, DropDown, EntityMap, EntityMustExist(typeof(WorkOrderRequester))]
        public int? RequestedBy { get; set; }

        public virtual bool DigitalAsBuiltRequired { get; set; }
        
        [DoesNotAutoMap("Not a view property. Used to set the premise id based on the premise number.")]
        public int? Premise { get; set; }

        #endregion

        #region Constructors

        public WorkOrderViewModel(IContainer container) : base(container) {}

        #endregion
        
        #region Private Methods

        internal static bool ShouldSendToSAP(OperatingCenter operatingCenter) =>
            operatingCenter.SAPEnabled &&
            operatingCenter.SAPWorkOrdersEnabled &&
            !operatingCenter.IsContractedOperations;

        internal static void MaybeMapDigitalAsBuiltRequired(
            WorkOrder entity,
            IContainer container,
            int? workDescription,
            bool required)
        {
            if (workDescription.HasValue)
            {
                var desc =
                    container.GetInstance<IRepository<WorkDescription>>().Find(workDescription.Value);

                entity.DigitalAsBuiltRequired =
                    desc.DigitalAsBuiltRequired || required;
            }
        }

        internal static void SetPremiseIfAvailable(WorkOrder entity, IContainer container)
        {
            if (string.IsNullOrWhiteSpace(entity.PremiseNumber))
            {
                entity.Premise = null;
                return;
            }
            
            var premise = container.GetInstance<IRepository<Premise>>().Where(p => p.PremiseNumber == entity.PremiseNumber && 
                p.DeviceLocation == entity.DeviceLocation.ToString() && 
                p.Installation == entity.Installation.ToString()).FirstOrDefault();
            entity.Premise = premise;
        }
        
        #endregion

        #region Exposed Methods

        public override void Map(WorkOrder entity)
        {
            base.Map(entity);
            var operatingCenter = _container
                                 .GetInstance<IOperatingCenterRepository>().Find(OperatingCenter.Value);
            if (operatingCenter != null)
            {
                SendToSAP = ShouldSendToSAP(operatingCenter);
            }

            SetPremiseIfAvailable(entity, _container);
        }

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            entity = base.MapToEntity(entity);
            var coordinate = _container.GetInstance<IRepository<Coordinate>>().Find(CoordinateId.Value);
            entity.Latitude = coordinate.Latitude;
            entity.Longitude = coordinate.Longitude;

            if (OperatingCenter.HasValue)
            {
                SendToSAP = ShouldSendToSAP(entity.OperatingCenter);
            }

            MaybeMapDigitalAsBuiltRequired(entity, _container, WorkDescription, DigitalAsBuiltRequired);
            
            SetPremiseIfAvailable(entity, _container);

            return entity;
        }

        #endregion
    }
}