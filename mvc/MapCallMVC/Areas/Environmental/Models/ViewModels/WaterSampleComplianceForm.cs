using DataAnnotationsExtensions;
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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class WaterSampleComplianceFormViewModel : ViewModel<WaterSampleComplianceForm>
    {
        #region Properties

        [Secured]
        [Required, EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public int? PublicWaterSupply { get; set; }

        [View("Public Water Supply"), DoesNotAutoMap("Display only")]
        public PublicWaterSupply PublicWaterSupplyDisplay
        {
            get
            {
                if (PublicWaterSupply.HasValue)
                {
                    return _container.GetInstance<IRepository<PublicWaterSupply>>().Find(PublicWaterSupply.Value);
                }

                return null;
            }
        }

        [AutoMap(MapDirections.ToViewModel)] // This is for display purposes only.
        public DateTime? DateCertified { get; set; }

        [AutoMap(MapDirections.ToViewModel)] // This is a read-only property on the entity anyway.
        public string CertifiedMonthYearDisplay { get; set; }

        [AutoMap(MapDirections.ToViewModel, SecondaryPropertyName = "CertifiedBy.FullName")]
        public string CertifiedBy { get; set; }
        public string NoteText { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? CentralLabSamplesHaveBeenCollected { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? CentralLabSamplesHaveBeenReported { get; set; }

        [RequiredWhen(nameof(CentralLabSamplesHaveBeenCollected), WaterSampleComplianceFormAnswerType.Indices.NO)]
        [RequiredWhen(nameof(CentralLabSamplesHaveBeenReported), WaterSampleComplianceFormAnswerType.Indices.NO)]
        public string CentralLabSamplesReason { get; set; }
       
        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? ContractedLabsSamplesHaveBeenCollected { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? ContractedLabsSamplesHaveBeenReported { get; set; }

        [RequiredWhen(nameof(ContractedLabsSamplesHaveBeenCollected), WaterSampleComplianceFormAnswerType.Indices.NO)]
        [RequiredWhen(nameof(ContractedLabsSamplesHaveBeenReported), WaterSampleComplianceFormAnswerType.Indices.NO)]
        public string ContractedLabsSamplesReason { get; set; }
       
        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? InternalLabsSamplesHaveBeenCollected { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? InternalLabsSamplesHaveBeenReported { get; set; }

        [RequiredWhen(nameof(InternalLabsSamplesHaveBeenCollected), WaterSampleComplianceFormAnswerType.Indices.NO)]
        [RequiredWhen(nameof(InternalLabsSamplesHaveBeenReported), WaterSampleComplianceFormAnswerType.Indices.NO)]
        public string InternalLabSamplesReason { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? BactiSamplesHaveBeenCollected { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? BactiSamplesHaveBeenReported { get; set; }

        [RequiredWhen(nameof(BactiSamplesHaveBeenCollected), WaterSampleComplianceFormAnswerType.Indices.NO)]
        [RequiredWhen(nameof(BactiSamplesHaveBeenReported), WaterSampleComplianceFormAnswerType.Indices.NO)]
        public string BactiSamplesReason { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? LeadAndCopperSamplesHaveBeenCollected { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? LeadAndCopperSamplesHaveBeenReported { get; set; }

        [RequiredWhen(nameof(LeadAndCopperSamplesHaveBeenCollected), WaterSampleComplianceFormAnswerType.Indices.NO)]
        [RequiredWhen(nameof(LeadAndCopperSamplesHaveBeenReported), WaterSampleComplianceFormAnswerType.Indices.NO)]
        public string LeadAndCopperSamplesReason { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? WQPSamplesHaveBeenCollected { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? WQPSamplesHaveBeenReported { get; set; }

        [RequiredWhen(nameof(WQPSamplesHaveBeenCollected), WaterSampleComplianceFormAnswerType.Indices.NO)]
        [RequiredWhen(nameof(WQPSamplesHaveBeenReported), WaterSampleComplianceFormAnswerType.Indices.NO)]
        public string WQPSamplesReason { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? SurfaceWaterPlantSamplesHaveBeenCollected { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? SurfaceWaterPlantSamplesHaveBeenReported { get; set; }

        [RequiredWhen(nameof(SurfaceWaterPlantSamplesHaveBeenCollected), WaterSampleComplianceFormAnswerType.Indices.NO)]
        [RequiredWhen(nameof(SurfaceWaterPlantSamplesHaveBeenReported), WaterSampleComplianceFormAnswerType.Indices.NO)]
        public string SurfaceWaterPlantSamplesReason { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? ChlorineResidualsHaveBeenCollected { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(WaterSampleComplianceFormAnswerType))]
        public int? ChlorineResidualsHaveBeenReported { get; set; }

        [RequiredWhen(nameof(ChlorineResidualsHaveBeenCollected), WaterSampleComplianceFormAnswerType.Indices.NO)]
        [RequiredWhen(nameof(ChlorineResidualsHaveBeenReported), WaterSampleComplianceFormAnswerType.Indices.NO)]
        public string ChlorineResidualsReason { get; set; }

        #endregion

        #region Constructors

        public WaterSampleComplianceFormViewModel(IContainer container) : base(container) { }

        #endregion
    }
    
    public class CreateWaterSampleComplianceForm : WaterSampleComplianceFormViewModel
    {
        #region Constructors

        public CreateWaterSampleComplianceForm(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            var certifiedMonthYear = new WaterSampleComplianceMonthYear(now);
            CertifiedMonthYearDisplay = WaterSampleComplianceMonthYear.GetFormattedValue(certifiedMonthYear.Month, certifiedMonthYear.Year);
            CertifiedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.FullName;
        }

        private IEnumerable<ValidationResult> ValidatePublicWaterSupply()
        {
            // pwsid is required, bail out if it's null.
            var pws = PublicWaterSupplyDisplay;
            if (pws != null)
            {
                // There are null AWOwned records for some reason.
                if (pws.AWOwned != true)
                {
                    yield return new ValidationResult("Compliance forms can only be entered for American Water owned public water supplies.");
                }

                if (pws.HasWaterSampleComplianceFormForTheCurrentMonth)
                {
                    yield return new ValidationResult("This public water supply is already certified for the current month.");
                }
            }
        }

        #endregion

        #region Public Methods

        public override WaterSampleComplianceForm MapToEntity(WaterSampleComplianceForm entity)
        {
            base.MapToEntity(entity);

            entity.CertifiedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            // Set DateCertified to now. Also set CertifiedMonth/CertifiedYear to the correct thingies
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.DateCertified = now;
            var certifiedMonthYear = new WaterSampleComplianceMonthYear(now);
            entity.CertifiedMonth = certifiedMonthYear.Month;
            entity.CertifiedYear = certifiedMonthYear.Year;

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidatePublicWaterSupply());
        }

        #endregion
    }

    public class EditWaterSampleComplianceForm : WaterSampleComplianceFormViewModel
    {
        #region Constructors

        public EditWaterSampleComplianceForm(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchWaterSampleComplianceForm : SearchSet<WaterSampleComplianceForm>
    {
        #region Properties

        [SearchAlias("PublicWaterSupply.OperatingCenterPublicWaterSupplies", "OperatingCenter.Id")]
        [MultiSelect, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }

        [SearchAlias("PublicWaterSupply", "Id", Required = true)] // This must exist in order to search by OperatingCenter.
        [MultiSelect("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = "OperatingCenter")]
        public int[] PublicWaterSupply { get; set; }

        [SearchAlias("PublicWaterSupply", "State.Id")]
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        public DateRange DateCertified { get; set; }

        [Range(1, 12)]
        public int? CertifiedMonth { get; set; }

        [Min(2000)] // They must put in a four digit year and we didn't start doing this until 2018. 
        public int? CertifiedYear { get; set; }

        #endregion
    }
}