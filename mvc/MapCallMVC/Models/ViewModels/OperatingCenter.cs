using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class EditOperatingCenter : ViewModel<OperatingCenter>
    {
        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        public override int Id { get; set; }

        [StringLength(OperatingCenter.MaxLengths.COMPANY_INFO)]
        public string CompanyInfo { get; set; }

        [StringLength(OperatingCenter.MaxLengths.FAX_NUMBER)]
        public string FaxNumber { get; set; }

        public int? ZoneStartYear { get; set; }

        public int? PaintingZoneStartYear { get; set; }

        [Required]
        public int? HydrantInspectionFrequency { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? HydrantInspectionFrequencyUnit { get; set; }

        [Required]
        public int? HydrantPaintingFrequency { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? HydrantPaintingFrequencyUnit { get; set; }

        [Required]
        public int? SewerOpeningInspectionFrequency { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? SewerOpeningInspectionFrequencyUnit { get; set; }

        [StringLength(OperatingCenter.MaxLengths.INFOMASTER_MAP_ID)]
        public string InfoMasterMapId { get; set; }
        public string InfoMasterMapLayerName { get; set; }

        [Required]
        public int? LargeValveInspectionFrequency { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? LargeValveInspectionFrequencyUnit { get; set; }

        [Required]
        public int? SmallValveInspectionFrequency { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(RecurringFrequencyUnit))]
        public int? SmallValveInspectionFrequencyUnit { get; set; }

        [StringLength(OperatingCenter.MaxLengths.MAILING_ADDRESS_NAME)]
        public string MailingAddressName { get; set; }

        [StringLength(OperatingCenter.MaxLengths.MAILING_ADDRESS_STREET)]
        public string MailingAddressStreet { get; set; }

        [StringLength(OperatingCenter.MaxLengths.MAILING_ADDRESS_CITY_STATE_ZIP)]
        public string MailingAddressCityStateZip { get; set; }

        // OperatingCenterCode is not editable because of tables that reference
        // its value instead of OperatingCenterId.
        [AutoMap(Direction=MapDirections.ToPrimary)]
        public string OperatingCenterCode { get; set; }

        [Required]
        [StringLength(OperatingCenter.MaxLengths.OPERATING_CENTER_NAME)]
        public string OperatingCenterName { get; set; }

        [StringLength(OperatingCenter.MaxLengths.PERMITS_CAPITAL_USER_NAME)]
        public string PermitsCapitalUserName { get; set; }

        [StringLength(OperatingCenter.MaxLengths.PERMITS_OM_USER_NAME)]
        public string PermitsOMUserName { get; set; }

        [StringLength(OperatingCenter.MaxLengths.PHONE_NUMBER)]
        public string PhoneNumber { get; set; }

        [StringLength(OperatingCenter.MaxLengths.SERVICE_CONTACT_PHONE_NUMBER)]
        public string ServiceContactPhoneNumber { get; set; }

        public bool WorkOrdersEnabled { get; set; }

        [DisplayName("RSA/Division #"), Range(0, 9999)]
        public int? RSADivisionNumber { get; set; }

        public bool IsActive { get; set; }

        [DropDown, Required, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }
        [DropDown("StateRegion", "ByStateId", DependsOn = "State", PromptText = "Please select a State above."), EntityMustExist(typeof(StateRegion))]
        public int? StateRegion { get; set; }

        public int? MaximumOverflowGallons { get; set; }
        
        [Required]
        public bool? IsContractedOperations { get; set; }

        [Required]
        public bool? SAPEnabled { get; set; }
        [Required]
        public bool? SAPWorkOrdersEnabled { get; set; }

        public bool UsesValveInspectionFrequency { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatedByOperatingCenter { get; set; }
        
        [StringLength(OperatingCenter.MaxLengths.MAP_ID)]
        public string MapId { get; set; }
        
        [StringLength(OperatingCenter.MaxLengths.ARC_MOBILE_MAP_ID)]
        public string ArcMobileMapId { get; set; }

        [StringLength(OperatingCenter.MaxLengths.DATA_COLLECTION_MAP_URL)]
        public string DataCollectionMapUrl { get; set; }

        [Coordinate(IconSet = IconSets.SingleDefaultIcon), EntityMap]
        public int? Coordinate { get; set; }

        [DropDown, EntityMap, EntityMustExist((typeof(TimeZone)))]
        public int? TimeZone { get; set; }

        #endregion

        #region Constructors

        public EditOperatingCenter(IContainer container) : base(container) { }

        #endregion

        #region Private Methods
        

        #endregion

        #region Public Methods

        public override string ToString()
        {
            // Where are we calling ToString on a view model? -Ross 1/19/2015
            return OperatingCenterCode;
        }

        #endregion
    }

    public class AddOperatingCenterAssetType : ViewModel<OperatingCenter>
    {
        #region Properties

        [DoesNotAutoMap]
        [Required, DropDown, EntityMustExist(typeof(AssetType))]
        public int? AssetType { get; set; }

        #endregion

        #region Constructors

        public AddOperatingCenterAssetType(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override OperatingCenter MapToEntity(OperatingCenter entity)
        {
            // NOTE: No base call for this.

            var assetType = _container.GetInstance<IAssetTypeRepository>().Find(AssetType.Value);

            // Don't add duplicates, just process as normal.
            if (!entity.OperatingCenterAssetTypes.Any(x => x.AssetType == assetType))
            {
                entity.OperatingCenterAssetTypes.Add(new OperatingCenterAssetType {
                    AssetType = assetType,
                    OperatingCenter = entity
                });
            }

            return entity;
        }

        #endregion
    }

    public class RemoveOperatingCenterAssetType : ViewModel<OperatingCenter>
    {
        #region Properties

        [Required, DoesNotAutoMap]
        public int? OperatingCenterAssetTypeId { get; set; }

        #endregion

        #region Constructors

        public RemoveOperatingCenterAssetType(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override OperatingCenter MapToEntity(OperatingCenter entity)
        {
            // NOTE: Don't call base method.

            var removable = entity.OperatingCenterAssetTypes.SingleOrDefault(x => x.Id == OperatingCenterAssetTypeId.Value);
            if (removable != null)
            {
                entity.OperatingCenterAssetTypes.Remove(removable);
            }

            return entity;
        }

        #endregion
    }

    public class AddOperatingCenterWaterSystem : ViewModel<OperatingCenter>
    {
        #region Properties

        [Required, DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(WaterSystem))]
        public int? WaterSystem { get; set; }

        #endregion

        #region Constructors

        public AddOperatingCenterWaterSystem(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override OperatingCenter MapToEntity(OperatingCenter entity)
        {
            // NOTE: No base call for this.
          
            // Don't add duplicates, just process as normal.
            if (!entity.WaterSystems.Any(x => x.Id == WaterSystem.Value))
            {
                var ws = _container.GetInstance<IWaterSystemRepository>().Find(WaterSystem.Value);
                entity.WaterSystems.Add(ws);
            }
            return entity;
        }

        #endregion
    }

    public class RemoveOperatingCenterWaterSystem : ViewModel<OperatingCenter>
    {
        #region Properties

        [Required, EntityMap(MapDirections.None), EntityMustExist(typeof(WaterSystem))]
        public int? WaterSystem { get; set; }

        #endregion

        #region Constructors

        public RemoveOperatingCenterWaterSystem(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override OperatingCenter MapToEntity(OperatingCenter entity)
        {
            // NOTE: Don't call base method.
            var ws = entity.WaterSystems.SingleOrDefault(x => x.Id == WaterSystem.Value);
            if (ws != null)
            {
                entity.WaterSystems.Remove(ws);
            }
            return entity;
        }

        #endregion
    }

    /// <summary>
    /// This is a class used for serialization in NotificationConfigurationController
    /// </summary>
    public class OperatingCenterState
    {
        public int OperatingCenterId { get; set; }
        public int StateId { get; set; }
    }
}