using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Validation;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels
{
    // NOTE: If you change validation/field names/whatever in the BacterialWaterSampleViewModel or its Edit model, you may also
    // need to change those things in the InlineEditBacterialWaterSample class.

    public class BacterialWaterSampleViewModel : ViewModel<BacterialWaterSample>
    {
        #region Properties
            
        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("WaterQuality", "SampleSite", "ActiveBactiSitesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        [EntityMap, EntityMustExist(typeof(SampleSite))]
        [RequiredWhen("BacterialSampleType", ComparisonType.EqualToAny, new[] { MapCall.Common.Model.Entities.BacterialSampleType.Indices.ROUTINE, MapCall.Common.Model.Entities.BacterialSampleType.Indices.REPEAT })]
        public int? SampleSite { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(NonSheenColonyCountOperator))]
        public int? NonSheenColonyCountOperator { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SheenColonyCountOperator))]
        public int? SheenColonyCountOperator { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(BacterialSampleType))]
        public int? BacterialSampleType { get; set; }

        [RequiredWhen("BacterialSampleType", MapCall.Common.Model.Entities.BacterialSampleType.Indices.REPEAT), DropDown, EntityMap, EntityMustExist(typeof(BacterialWaterSampleRepeatLocationType))]
        public int? RepeatLocationType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EstimatingProject))]
        [RequiredWhen("BacterialSampleType", MapCall.Common.Model.Entities.BacterialSampleType.Indices.NEW_MAIN)]
        public int? EstimatingProject { get; set; }

        [EntityMap, EntityMustExist(typeof(BacterialWaterSample))]
        [DropDown("WaterQuality", "BacterialWaterSample", "GetBySampleSiteIdWithBracketSites", DependsOn = "SampleSite", PromptText = "Select a Sample Site above")]
        [RequiredWhen("BacterialSampleType", ComparisonType.EqualTo, MapCall.Common.Model.Entities.BacterialSampleType.Indices.REPEAT)]
        public int? OriginalBacterialWaterSample { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center"), EntityMap, EntityMustExist(typeof(Town))]
        [RequiredWhen("BacterialSampleType", MapCall.Common.Model.Entities.BacterialSampleType.Indices.PROCESS_CONTROL)]
        public int? SampleTown { get; set; }

        [Description("This will always be overriden by Sample Site's Coordinate if selected.")]
        [RequiredWhen("Address", ComparisonType.NotEqualTo, null)]
        [RequiredWhen(nameof(SampleSite), null)]
        [Coordinate(AddressCallback="BacterialWaterSample.getAddress"), EntityMap]
        public int? SampleCoordinate { get; set; }

        [Required, DateTimePicker]
        [View(MMSINC.Utilities.FormatStyle.DateTimeWithoutSeconds)] // Military time does not work with DateTimePicker.
        public DateTime? SampleCollectionDTM { get; set; }

        [DateTimePicker]
        public DateTime? ReceivedByLabDTM { get; set; }

        [CheckBox]
        public bool IsInvalid { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(BacterialWaterSampleReasonForInvalidation))]
        [RequiredWhen(nameof(IsInvalid), true)]
        public int? ReasonForInvalidation { get; set; }

        // This field is only editable for users with the WaterQualityGeneral admin role
        // Also this is a MapCall username that gets mapped to the proper entity.
        [AutoMap(MapDirections.None)]
        public string CollectedBy { get; set; }

        [RequiredWhen(nameof(SampleSite), null)]
        [StringLength(BacterialWaterSample.StringLengths.LOCATION)]
        public string Location { get; set; }

        [DisplayName("Sample Id")]
        [StringLength(BacterialWaterSample.StringLengths.SAMPLE_NUMBER)]
        public string SampleNumber { get; set; }

        [Remote("ValidateFreeChlorine", "BacterialWaterSample", AdditionalFields = nameof(SampleSite))]
        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_CL2FREE, BacterialWaterSample.Validation.MAX_CL2FREE)]
        public decimal? Cl2Free { get; set; }

        [Remote("ValidateTotalChlorine", "BacterialWaterSample", AdditionalFields = nameof(SampleSite))]
        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_CL2TOTAL, BacterialWaterSample.Validation.MAX_CL2TOTAL)]
        public decimal? Cl2Total { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_FLUSH_TIME_MINUTES, BacterialWaterSample.Validation.MAX_FLUSH_TIME_MINUTES)]
        public decimal? FlushTimeMinutes { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_NITRITE, BacterialWaterSample.Validation.MAX_NITRITE)]
        public decimal? Nitrite { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_NITRATE, BacterialWaterSample.Validation.MAX_NITRATE)]
        public decimal? Nitrate { get; set; }

        public decimal? FinalHPC { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_MONOCHLORAMINE, BacterialWaterSample.Validation.MAX_MONOCHLORAMINE)]
        public decimal? Monochloramine { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_FREE_AMMONIA, BacterialWaterSample.Validation.MAX_FREE_AMONIA)]
        public decimal? FreeAmmonia { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_PH, BacterialWaterSample.Validation.MAX_PH)]
        public decimal? Ph { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_TEMPERATURE, BacterialWaterSample.Validation.MAX_TEMPERATURE)]
        public decimal? Temperature { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_IRON, BacterialWaterSample.Validation.MAX_IRON)]
        public decimal? Iron { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_MANGANESE, BacterialWaterSample.Validation.MAX_MANGANESE)]
        public decimal? Manganese { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_TURBIDITY, BacterialWaterSample.Validation.MAX_TURBIDITY)]
        public decimal? Turbidity { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_ORTHOPHOSPHATEASP, BacterialWaterSample.Validation.MAX_ORTHOPHOSPHATEASP)]
        public decimal? OrthophosphateAsP { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_ORTHOPHOSPHATEASPO4, BacterialWaterSample.Validation.MAX_ORTHOPHOSPHATEASPO4)]
        public decimal? OrthophosphateAsPO4 { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_CONDUCTIVITY, BacterialWaterSample.Validation.MAX_CONDUCTIVITY)]
        public decimal? Conductivity { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_ALKALINITY, BacterialWaterSample.Validation.MAX_ALKALINITY)]
        public decimal? Alkalinity { get; set; }

        public int? NonSheenColonyCount { get; set; }
        public int? SheenColonyCount { get; set; }
        
        [BoolFormat("Present", "Absent")]
        [View(Description = "Check if present. Uncheck if absent.")]
        public bool ColiformConfirm { get; set; }

        [BoolFormat("Present", "Absent")]
        public bool? EColiConfirm { get; set; }
        public bool ComplianceSample { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(BacterialWaterSampleConfirmMethod))]
        public virtual int? ColiformConfirmMethod { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(BacterialWaterSampleConfirmMethod))]
        public virtual int? EColiConfirmMethod { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(BacterialWaterSampleConfirmMethod))]
        public virtual int? HPCConfirmMethod { get; set; }

        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public virtual int? ColiformSetupAnalyst { get; set; }

        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public virtual int? ColiformReadAnalyst { get; set; }

        [DateTimePicker]
        [View(MMSINC.Utilities.FormatStyle.DateTimeWithoutSeconds)] // Military time does not work with DateTimePicker.
        public DateTime? ColiformSetupDTM { get; set; }

        [DateTimePicker]
        [View(MMSINC.Utilities.FormatStyle.DateTimeWithoutSeconds)] // Military time does not work with DateTimePicker.
        public DateTime? ColiformReadDTM { get; set; }

        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public virtual int? HPCSetupAnalyst { get; set; }

        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public virtual int? HPCReadAnalyst { get; set; }

        [DateTimePicker]
        [View(MMSINC.Utilities.FormatStyle.DateTimeWithoutSeconds)] // Military time does not work with DateTimePicker.
        public DateTime? HPCSetupDTM { get; set; }

        [DateTimePicker]
        [View(MMSINC.Utilities.FormatStyle.DateTimeWithoutSeconds)] // Military time does not work with DateTimePicker.
        public DateTime? HPCReadDTM { get; set; }

        [StringLength(BacterialWaterSample.StringLengths.SAP_WORK_ORDER_ID)]
        [RequiredWhen("BacterialSampleType", MapCall.Common.Model.Entities.BacterialSampleType.Indices.SYSTEM_REPAIR)]
        public string SAPWorkOrderId { get; set; }

        [StringLength(BacterialWaterSample.StringLengths.ADDRESS)]
        public string Address { get; set; }

        public bool IsSpreader { get; set; }

        [DoesNotAutoMap("This has to be mapped manually.")]
        public bool? IsReadyForLIMS { get; set; }

        [DoesNotAutoMap("This is for view logic")]
        public bool ShouldDisplayReadyForLIMSField
        {
            get { return IsReadyForLIMS != null; }
        }

        #endregion

        #region Constructors

        public BacterialWaterSampleViewModel(IContainer container) : base(container) {}

        #endregion

        #region Private Methods

        private bool CurrentUserIsWaterQualityGeneralUserAdmin()
        {
            return _container.GetInstance<IRoleService>().CanAccessRole(RoleModules.WaterQualityGeneral, RoleActions.UserAdministrator);
        }

        #endregion

        #region Public Methods

        public override void Map(BacterialWaterSample entity)
        {
            base.Map(entity);

            IsReadyForLIMS = BacterialWaterSampleViewModelHelper.GetIsReadyForLIMS(entity);
        }

        public override BacterialWaterSample MapToEntity(BacterialWaterSample entity)
        {
            base.MapToEntity(entity);

            // Setting the CollectedBy value is now editable by users with WaterQualityGeneral UserAdmin role.
            // This needs to be taken into consideration when mapping. An admin user can set this value to whatever
            // they want. If the value is null, it should be set to their username. The value should be validated that
            // it's an actual username in the system. Also this should be done in the base model.
            if (!CurrentUserIsWaterQualityGeneralUserAdmin() || string.IsNullOrWhiteSpace(CollectedBy))
            {
                // We do not overwrite the CollectedBy value if it is already set if it would otherwise become unset.
               // if (string.IsNullOrWhiteSpace(entity.CollectedBy))
                if (entity.CollectedBy == null)
                {
                    entity.CollectedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
                }
            }
            else
            {
                entity.CollectedBy = _container.GetInstance<IUserRepository>().GetUserByUserName(CollectedBy);
            }

            BacterialWaterSampleViewModelHelper.SetLIMSStatus(_container, entity, IsReadyForLIMS);

            return entity;
        }

        private IEnumerable<ValidationResult> ValidateCollectedBy()
        {
            // CollectedBy only needs to be validated if the user is allowed to edit it. The values are otherwise
            // not sent to the server and it's defaulted to the current user.

            if (!string.IsNullOrWhiteSpace(CollectedBy) && CurrentUserIsWaterQualityGeneralUserAdmin())
            {
                if (_container.GetInstance<IUserRepository>().TryGetUserByUserName(CollectedBy) == null)
                {
                    yield return new ValidationResult("Username does not match an existing user.", new[] { nameof(CollectedBy) });
                }
            }
        }

        private IEnumerable<ValidationResult> ValidateCl2Total()
        {
            var helper = _container.GetInstance<BacterialWaterSampleValidationHelper>();
            if (!helper.ValidateTotalChlorineForSampleSite(Cl2Total, SampleSite))
            {
                yield return new ValidationResult($"{nameof(Cl2Total)} is required for the selected Sample Site.", new[] { nameof(Cl2Total) });
            }
        }

        private IEnumerable<ValidationResult> ValidateCl2Free()
        {
            var helper = _container.GetInstance<BacterialWaterSampleValidationHelper>();
            if (!helper.ValidateFreeChlorineForSampleSite(Cl2Free, SampleSite))
            {
                yield return new ValidationResult($"{nameof(Cl2Free)} is required for the selected Sample Site.", new[] { nameof(Cl2Free) });
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateCollectedBy()).Concat(ValidateCl2Total()).Concat(ValidateCl2Free());
        }

        #endregion
    }

    public class CreateBacterialWaterSample : BacterialWaterSampleViewModel
    {
        #region Properties

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetActiveByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        public override int? ColiformSetupAnalyst { get => base.ColiformSetupAnalyst; set => base.ColiformSetupAnalyst = value; }

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetActiveByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        public override int? ColiformReadAnalyst { get => base.ColiformReadAnalyst; set => base.ColiformReadAnalyst = value; }

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetActiveByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        public override int? HPCReadAnalyst { get => base.HPCReadAnalyst; set => base.HPCReadAnalyst = value; }

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetActiveByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        public override int? HPCSetupAnalyst { get => base.HPCSetupAnalyst; set => base.HPCSetupAnalyst = value; }

        #endregion

        #region Constructors

        public CreateBacterialWaterSample(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            ComplianceSample = true;
            IsReadyForLIMS = false;
            CollectedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;
            if (SampleSite.HasValue)
            {
                var sampleSite = _container.GetInstance<IRepository<SampleSite>>().Find(SampleSite.Value);
                OperatingCenter = sampleSite.OperatingCenter.Id;
            }
        }

        public override BacterialWaterSample MapToEntity(BacterialWaterSample entity)
        {
            base.MapToEntity(entity);

            entity.DataEntered = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            
            return entity;
        }

        #endregion
    }

    public class EditBacterialWaterSample : BacterialWaterSampleViewModel
    {
        #region Properties

        // Edit dropdowns do not use the GetActiveByOperatingCenter action for cascades.
        // This is so analysts don't get lost if a record is edited that references
        // a deactivated analyst.

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        public override int? ColiformSetupAnalyst { get => base.ColiformSetupAnalyst; set => base.ColiformSetupAnalyst = value; }

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        public override int? ColiformReadAnalyst { get => base.ColiformReadAnalyst; set => base.ColiformReadAnalyst = value; }

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        public override int? HPCReadAnalyst { get => base.HPCReadAnalyst; set => base.HPCReadAnalyst = value; }

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        public override int? HPCSetupAnalyst { get => base.HPCSetupAnalyst; set => base.HPCSetupAnalyst = value; }

        #endregion

        #region Constructors

        public EditBacterialWaterSample(IContainer container) : base(container) {}

        #endregion

        public override void Map(BacterialWaterSample entity)
        {
            base.Map(entity);

            // Not all records have CollectedBy set.
            CollectedBy = entity.CollectedBy?.UserName;
        }
    }

    public class InlineEditBacterialWaterSample : ViewModel<BacterialWaterSample>
    {
        #region Properties

        #region Display Properties

        [AutoMap(MapDirections.None)]
        public string BacterialSampleType { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public bool ComplianceSample { get; set; }

        // This is not displayed, but the value is necessary for the cascading.
        public int OperatingCenter { get; set; }

        [DisplayName("Operating Center"), DoesNotAutoMap("Display only")]
        public string OperatingCenterStr { get; set; }

        [DisplayName("Sample Site"), DoesNotAutoMap("Display only")]
        public string SampleSiteStr { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public DateTime? SampleCollectionDTM { get; set; }

        [AutoMap(MapDirections.None)]
        public string CollectedBy { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public string Location { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public bool IsInvalid { get; set; }

        #endregion

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public int? ColiformSetupAnalyst { get; set; }

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public int? ColiformReadAnalyst { get; set; }

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public int? HPCSetupAnalyst { get; set; }

        [DropDown("WaterQuality", "BacterialWaterSampleAnalyst", "GetByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center")]
        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public int? HPCReadAnalyst { get; set; }

        [DisplayName("Received by Lab DTM")]
        [DateTimePicker]
        public DateTime? ReceivedByLabDTM { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_CL2FREE, BacterialWaterSample.Validation.MAX_CL2FREE)]
        public decimal? Cl2Free { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_CL2TOTAL, BacterialWaterSample.Validation.MAX_CL2TOTAL)]
        public decimal? Cl2Total { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_NITRITE, BacterialWaterSample.Validation.MAX_NITRITE)]
        public decimal? Nitrite { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_NITRATE, BacterialWaterSample.Validation.MAX_NITRATE)]
        public decimal? Nitrate { get; set; }

        public decimal? FinalHPC { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_MONOCHLORAMINE, BacterialWaterSample.Validation.MAX_MONOCHLORAMINE)]
        public decimal? Monochloramine { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_FREE_AMMONIA, BacterialWaterSample.Validation.MAX_FREE_AMONIA)]
        public decimal? FreeAmmonia { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_PH, BacterialWaterSample.Validation.MAX_PH)]
        public decimal? Ph { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_TEMPERATURE, BacterialWaterSample.Validation.MAX_TEMPERATURE)]
        public decimal? Temperature { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_IRON, BacterialWaterSample.Validation.MAX_IRON)]
        public decimal? Iron { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_MANGANESE, BacterialWaterSample.Validation.MAX_MANGANESE)]
        public decimal? Manganese { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_TURBIDITY, BacterialWaterSample.Validation.MAX_TURBIDITY)]
        public decimal? Turbidity { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_ORTHOPHOSPHATEASP, BacterialWaterSample.Validation.MAX_ORTHOPHOSPHATEASP)]
        public decimal? OrthophosphateAsP { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_ORTHOPHOSPHATEASPO4, BacterialWaterSample.Validation.MAX_ORTHOPHOSPHATEASPO4)]
        public decimal? OrthophosphateAsPO4 { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_CONDUCTIVITY, BacterialWaterSample.Validation.MAX_CONDUCTIVITY)]
        public decimal? Conductivity { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_ALKALINITY, BacterialWaterSample.Validation.MAX_ALKALINITY)]
        public decimal? Alkalinity { get; set; }

        [BoolFormat("Present", "Absent")]
        public bool ColiformConfirm { get; set; }

        [DateTimePicker]
        public DateTime? ColiformSetupDTM { get; set; }

        [DateTimePicker]
        public DateTime? ColiformReadDTM { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(BacterialWaterSampleConfirmMethod))]
        public virtual int? ColiformConfirmMethod { get; set; }

        public virtual bool IsSpreader { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(BacterialWaterSampleConfirmMethod))]
        public virtual int? HPCConfirmMethod { get; set; }

        [DateTimePicker]
        [View(MMSINC.Utilities.FormatStyle.DateTimeWithoutSeconds)] // Military time does not work with DateTimePicker.
        public DateTime? HPCSetupDTM { get; set; }

        [DateTimePicker]
        [View(MMSINC.Utilities.FormatStyle.DateTimeWithoutSeconds)] // Military time does not work with DateTimePicker.
        public DateTime? HPCReadDTM { get; set; }

        public string SampleNumber { get; set; }

        [DoesNotAutoMap("This has to be mapped manually.")]
        public bool? IsReadyForLIMS { get; set; }

        [DoesNotAutoMap("This is for view logic")]
        public bool ShouldDisplayReadyForLIMSField
        {
            get { return IsReadyForLIMS != null; }
        }

        #endregion

        #region Constructors

        public InlineEditBacterialWaterSample(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void Map(BacterialWaterSample entity)
        {
            base.Map(entity);
            OperatingCenterStr = entity.OperatingCenter.ToString();
            OperatingCenter = entity.OperatingCenter.Id;

            // sample site: (Town, Sample Site Name)
            SampleSiteStr = $"{entity.Town}, {entity.SampleSite.CommonSiteName}";
            BacterialSampleType = entity.BacterialSampleType.ToString();
            CollectedBy = entity.CollectedBy.ToString();

            IsReadyForLIMS = BacterialWaterSampleViewModelHelper.GetIsReadyForLIMS(entity);
        }

        public override BacterialWaterSample MapToEntity(BacterialWaterSample entity)
        {
            base.MapToEntity(entity);
            BacterialWaterSampleViewModelHelper.SetLIMSStatus(_container, entity, IsReadyForLIMS);
            return entity;
        }

        private IEnumerable<ValidationResult> ValidateCl2Total()
        {
            // This would be better as a RequiredWhen but it would also be more convoluted.
            var sampleSiteId = _container.GetInstance<IBacterialWaterSampleRepository>().Find(Id).SampleSite?.Id;
            var helper = _container.GetInstance<BacterialWaterSampleValidationHelper>();
            if (!helper.ValidateTotalChlorineForSampleSite(Cl2Total, sampleSiteId))
            {
                yield return new ValidationResult($"{nameof(Cl2Total)} is required for the selected Sample Site.", new[] { nameof(Cl2Total) });
            }
        }

        private IEnumerable<ValidationResult> ValidateCl2Free()
        {
            // This would be better as a RequiredWhen but it would also be more convoluted.
            var sampleSiteId = _container.GetInstance<IBacterialWaterSampleRepository>().Find(Id).SampleSite?.Id;
            var helper = _container.GetInstance<BacterialWaterSampleValidationHelper>();
            if (!helper.ValidateFreeChlorineForSampleSite(Cl2Free, sampleSiteId))
            {
                yield return new ValidationResult($"{nameof(Cl2Free)} is required for the selected Sample Site.", new[] { nameof(Cl2Free) });
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateCl2Total()).Concat(ValidateCl2Free());
        }

        #endregion

    }

    public class SearchBacterialWaterSample : SearchSet<BacterialWaterSample>
    {
        #region Properties

        // This is used by the BacterialWaterSampleMassExport controller for redirecting
        // the page after successfully saving a bunch of records.
       
        public int[] EntityId { get; set; }

        // TODO: Remove this BactiSite property and correctly filter the cascading results
        //       so that they aren't including the SampleSites where BactiSite != true. 

        //Only Search SampleSites.BactiSite == true
        [SearchAlias("SampleSite", "BactiSite")]
        public bool BactiSite => true;

        [SearchAlias("SampleSite", "SampleSite", "State.Id")]
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [MultiSelect]
        [SearchAlias("SampleSite", "SampleSite", "PublicWaterSupply.Id")]
        public virtual int[] PublicWaterSupply { get; set; }

        // operating center // via sample site
        [MultiSelect("", "OperatingCenter", "GetByPublicWaterSuppliesForWaterQuality", DependsOn = nameof(PublicWaterSupply), PromptText = "Please select a PWSID above.")]
        public int[] OperatingCenter { get; set; }

        //[Cascading("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [DropDown]
        public int? Town { get; set; }

        // sample result id
        [DisplayName("Bacterial Sample Result Id")]
        public int? Id { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(BacterialSampleType))]
        public int? BacterialSampleType { get; set; }

        public DateRange SampleCollectionDTM { get; set; }

        // collected by
        [SearchAlias("SampleSite", "Id")]
        [DisplayName("Sample Site ID")]
        public int? SampleSiteId { get; set; }

        // town
        [DropDown("WaterQuality", "SampleSite", "ByOperatingCenterIdWithMatrices", DependsOn= "OperatingCenter", PromptText = "Please select an Operating Center")]
        public int? SampleSite { get; set; }

        public SearchString SampleNumber { get; set; }

        [SearchAlias("CollectedBy", "UserName")]
        public SearchString CollectedBy { get; set; }

        // water constituent
        // parameter
        public NumericRange Cl2Free { get; set; }

        [SearchAlias("SampleSite", "Status.Id"), DropDown]
        public int? SampleSiteStatus { get; set; }

        public bool? ColiformConfirm { get; set; }
        public bool? EColiConfirm { get; set; }
        public bool? ComplianceSample { get; set; }
        public NumericRange Cl2Total { get; set; }
        public NumericRange Monochloramine { get; set; }
        public NumericRange FreeAmmonia { get; set; }
        public NumericRange Nitrite { get; set; }
        public NumericRange Nitrate { get; set; }
        public NumericRange FinalHPC { get; set; }
        public NumericRange Ph { get; set; }
        public NumericRange Conductivity { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(LIMSStatus))]
        public int? LIMSStatus { get; set; }

        public bool? IsInvalid { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(BacterialWaterSampleReasonForInvalidation))]
        public int? ReasonForInvalidation { get; set; }

        #endregion

        // poe
        // bacti site
        // process control site
        // njaw owned

        // chloramine monitor site
    }

    #region Mass Editing

    internal class MassEditBacterialAnalyst
    {
        public int Id { get; set; }
        public OperatingCenter OperatingCenter { get; set; }
        public Employee Employee { get; set; }
    }

    /// <summary>
    /// This inherits ViewModelBacterialWaterSample] only because ActionHelper methods
    /// require a ViewModel[T] that matches the entity/repository. This view model will 
    /// never be mapping to a single BacterialWaterSample record.
    /// </summary>
    //[ModelBinder(typeof(BacterialWaterSampleMassEditModelBinder))]
    public class CreateBacterialWaterSampleMassEdit : ViewModel<BacterialWaterSample>
    {
        #region Properties

        [DoesNotAutoMap("FileUploads don't map")]
        public ExcelAjaxFileUpload<BacterialWaterSampleMassEditExcelItem> FileUpload { get; set; }

        #endregion
        
        #region Constructors

        public CreateBacterialWaterSampleMassEdit(IContainer container) : base(container) { }

        #endregion

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var allIds = FileUpload.Items.Select(x => x.Id).ToList();
            var duplicateIds = allIds.GroupBy(x => x).Where(x => x.Count() > 1);

            if (duplicateIds.Any())
            {
                var duplicateStr = string.Join(", ", duplicateIds.Select(x => x.Key));
                yield return new ValidationResult($"The following record ids appear more than once in the file: {duplicateStr}.", new[] { "FileUpload" });
            }

            var repo = _container.GetInstance<IBacterialWaterSampleRepository>();

            // This is being done in place of using EntityMustExist because it's much faster
            // when dealing with a lot of records at one time.
            // TODO: Don't cast this
            var entitiesThatDoNotExist = repo.FindManyByIds(allIds).Where(x => x.Value == null).Select(x => x.Key).ToList();
            if (entitiesThatDoNotExist.Any())
            {
                var missingIds = string.Join(", ", entitiesThatDoNotExist);
                yield return new ValidationResult($"The following ids do not match an existing record: {missingIds}.", new[] { "FileUpload"});
            }
        }
    }

    public class BacterialWaterSampleMassEditExcelItem : ViewModel<BacterialWaterSample>
    {
        #region Properties

        // NOTE: There is no Id override with EntityMustExist here for performance reasons.
        // That validation is done in the CreateBacterialWaterSampleMassEdit model.

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_CL2FREE, BacterialWaterSample.Validation.MAX_CL2FREE)]
        public decimal? Cl2Free { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_CL2TOTAL, BacterialWaterSample.Validation.MAX_CL2TOTAL)]
        public decimal? Cl2Total { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_NITRITE, BacterialWaterSample.Validation.MAX_NITRITE)]
        public decimal? Nitrite { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_NITRATE, BacterialWaterSample.Validation.MAX_NITRATE)]
        public decimal? Nitrate { get; set; }

        public decimal? FinalHPC { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_MONOCHLORAMINE, BacterialWaterSample.Validation.MAX_MONOCHLORAMINE)]
        public decimal? Monochloramine { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_FREE_AMMONIA, BacterialWaterSample.Validation.MAX_FREE_AMONIA)]
        public decimal? FreeAmmonia { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_PH, BacterialWaterSample.Validation.MAX_PH)]
        public decimal? Ph { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_TEMPERATURE, BacterialWaterSample.Validation.MAX_TEMPERATURE)]
        public decimal? Temperature { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_IRON, BacterialWaterSample.Validation.MAX_IRON)]
        public decimal? Iron { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_MANGANESE, BacterialWaterSample.Validation.MAX_MANGANESE)]
        public decimal? Manganese { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_TURBIDITY, BacterialWaterSample.Validation.MAX_TURBIDITY)]
        public decimal? Turbidity { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_ORTHOPHOSPHATEASP, BacterialWaterSample.Validation.MAX_ORTHOPHOSPHATEASP)]
        public decimal? OrthophosphateAsP { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_ORTHOPHOSPHATEASPO4, BacterialWaterSample.Validation.MAX_ORTHOPHOSPHATEASPO4)]
        public decimal? OrthophosphateAsPO4 { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_CONDUCTIVITY, BacterialWaterSample.Validation.MAX_CONDUCTIVITY)]
        public decimal? Conductivity { get; set; }

        [Range(typeof(decimal), BacterialWaterSample.Validation.MIN_ALKALINITY, BacterialWaterSample.Validation.MAX_ALKALINITY)]
        public decimal? Alkalinity { get; set; }

        [BoolFormat("Present", "Absent")]
        public bool ColiformConfirm { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(BacterialWaterSampleConfirmMethod))]
        public virtual int? ColiformConfirmMethod { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(BacterialWaterSampleConfirmMethod))]
        public virtual int? HPCConfirmMethod { get; set; }

        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public virtual int? ColiformSetupAnalyst { get; set; }

        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public virtual int? ColiformReadAnalyst { get; set; }

        public DateTime? ColiformSetupDTM { get; set; }

        public DateTime? ColiformReadDTM { get; set; }

        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public virtual int? HPCSetupAnalyst { get; set; }

        [EntityMap, EntityMustExist(typeof(BacterialWaterSampleAnalyst))]
        public virtual int? HPCReadAnalyst { get; set; }

        public DateTime? HPCSetupDTM { get; set; }

        public DateTime? HPCReadDTM { get; set; }

        public bool IsSpreader { get; set; }

        [DoesNotAutoMap]
        public bool? IsReadyForLIMS { get; set; }

        #endregion

        #region Constructors

        public BacterialWaterSampleMassEditExcelItem(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override BacterialWaterSample MapToEntity(BacterialWaterSample entity)
        {
            base.MapToEntity(entity);
            BacterialWaterSampleViewModelHelper.SetLIMSStatus(_container, entity, IsReadyForLIMS);
            return entity;
        }

        private IEnumerable<ValidationResult> ValidateCl2Total()
        {
            // This would be better as a RequiredWhen but it would also be more convoluted.
            var sampleSiteId = _container.GetInstance<IBacterialWaterSampleRepository>().Find(Id).SampleSite?.Id;
            var helper = _container.GetInstance<BacterialWaterSampleValidationHelper>();
            if (!helper.ValidateTotalChlorineForSampleSite(Cl2Total, sampleSiteId))
            {
                yield return new ValidationResult($"{nameof(Cl2Total)} is required for the selected Sample Site.", new[] { nameof(Cl2Total) });
            }
        }

        private IEnumerable<ValidationResult> ValidateCl2Free()
        {
            // This would be better as a RequiredWhen but it would also be more convoluted.
            var sampleSiteId = _container.GetInstance<IBacterialWaterSampleRepository>().Find(Id).SampleSite?.Id;
            var helper = _container.GetInstance<BacterialWaterSampleValidationHelper>();
            if (!helper.ValidateFreeChlorineForSampleSite(Cl2Free, sampleSiteId))
            {
                yield return new ValidationResult($"{nameof(Cl2Free)} is required for the selected Sample Site.", new[] { nameof(Cl2Free) });
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateCl2Total()).Concat(ValidateCl2Free());
        }

        #endregion
    }

    #endregion
}
