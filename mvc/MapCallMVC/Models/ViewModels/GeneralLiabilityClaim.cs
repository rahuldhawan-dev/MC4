using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class GeneralLiabilityClaimViewModel : ViewModel<GeneralLiabilityClaim>
    {
        #region Properties

        [Required, EntityMap]
        [EntityMustExist(typeof(GeneralLiabilityClaimType))]
        [DropDown]
        public virtual int? GeneralLiabilityClaimType { get; set; }
        [Required, EntityMap]
        [EntityMustExist(typeof(CrashType))]
        [DropDown]
        public virtual int? CrashType { get; set; }

        [RequiredWhen(
            nameof(CrashType),
            ComparisonType.EqualTo,
            MapCall.Common.Model.Entities.CrashType.Indices.OTHER,
            FieldOnlyVisibleWhenRequired = true)]
        [StringLength(GeneralLiabilityClaim.StringLengths.OTHER_TYPE_OF_CRASH)]
        public string OtherTypeOfCrash { get; set; }
        [Required, EntityMap]
        [EntityMustExist(typeof(LiabilityType))]
        [DropDown]
        public virtual int? LiabilityType { get; set; }
        [Required, Coordinate(AddressField = "LocationOfIncident", IconSet = IconSets.GeneralLiabilityClaims), EntityMap]
        [EntityMustExist(typeof(Coordinate))]
        public virtual int? Coordinate { get; set; }
        [Required, EntityMap]
        [EntityMustExist(typeof(OperatingCenter))]
        [DropDown]
        [DisplayName("District")]
        public virtual int? OperatingCenter { get; set; }
        [Required, EntityMap]
        [EntityMustExist(typeof(Employee))]
        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public virtual int? CompanyContact { get; set; }
        [Required, EntityMap]
        [EntityMustExist(typeof(ClaimsRepresentative))]
        [DropDown]
        public virtual int? ClaimsRepresentative { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.CLAIM_NUMBER)]
        public virtual string ClaimNumber { get; set; }
        public virtual bool MeterBox { get; set; }
        public virtual bool CurbValveBox { get; set; }
        public virtual bool Excavation { get; set; }
        public virtual bool Barricades { get; set; }
        public virtual bool Vehicle { get; set; }
        public virtual bool WaterMeter { get; set; }
        public virtual bool FireHydrant { get; set; }
        public virtual bool Backhoe { get; set; }
        public virtual bool WaterQuality { get; set; }
        public virtual bool WaterPressure { get; set; }
        public virtual bool WaterMain { get; set; }
        public virtual bool ServiceLine { get; set; }
        [Multiline, StringLengthNotRequired("text field")]
        [Required]
        public virtual string Description { get; set; }
        [DisplayName("Claimant Name")]
        [StringLength(GeneralLiabilityClaim.StringLengths.NAME)]
        public virtual string Name { get; set; }
        [DisplayName("Claimant Phone Number")]
        [StringLength(GeneralLiabilityClaim.StringLengths.PHONE_NUMBER)]
        public virtual string PhoneNumber { get; set; }
        [DisplayName("Claimant Address")]
        [Multiline, StringLengthNotRequired("ntext field")]
        public virtual string Address { get; set; }
        [DisplayName("Claimant Email")]
        [Multiline, StringLength(GeneralLiabilityClaim.StringLengths.EMAIL)]
        public virtual string Email { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.DRIVER_NAME)]
        [DisplayName("Driver/Employee Name")]
        public virtual string DriverName { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.DRIVER_PHONE)]
        [DisplayName("Driver/Employee Phone")]
        public virtual string DriverPhone { get; set; }
        [DisplayName("Has PHH/Element been contacted for damage to our fleet vehicle?"), BoolFormat("Yes", "No", "N/A")]
        public virtual bool? PhhContacted { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.OTHER_DRIVER)]
        public virtual string OtherDriver { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.OTHER_DRIVER_PHONE)]
        public virtual string OtherDriverPhone { get; set; }
        [Multiline, StringLengthNotRequired("ntext field")]
        public virtual string OtherDriverAddress { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.LOCATION_OF_INCIDENT)]
        public virtual string LocationOfIncident { get; set; }
        [Required]
        [DateTimePicker]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime? IncidentDateTime { get; set; }
        [DisplayName("Other Vehicle Year")]
        public virtual int? VehicleYear { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.VEHICLE_MAKE)]
        [DisplayName("Other Vehicle Make")]
        public virtual string VehicleMake { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.VEHICLE_VIN)]
        [DisplayName("Other Vehicle VIN")]
        public virtual string VehicleVin { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.LICENSE_NUMBER)]
        [DisplayName("Other Vehicle License Plate Number")]
        public virtual string LicenseNumber { get; set; }
        public virtual bool? PoliceCalled { get; set; }

        // TODO: figure out how to do two conditions for these two
        [RequiredWhen("PoliceCalled", true)]
        //[RequiredWhen("CompletedDate", ComparisonType.NotEqualTo, null)]
        [StringLength(GeneralLiabilityClaim.StringLengths.POLICE_DEPARTMENT)]
        public virtual string PoliceDepartment { get; set; }
        [RequiredWhen("PoliceCalled", true)]
        //[RequiredWhen("CompletedDate", ComparisonType.NotEqualTo, null)]
        [StringLength(GeneralLiabilityClaim.StringLengths.POLICE_CASE_NUMBER)]
        public virtual string PoliceCaseNumber { get; set; }
        public virtual bool? WitnessStatement { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.WITNESS)]
        public virtual string Witness { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.WITNESS_PHONE)]
        public virtual string WitnessPhone { get; set; }
        public virtual bool? AnyInjuries { get; set; }
        [Required]
        [StringLength(GeneralLiabilityClaim.StringLengths.REPORTED_BY)]
        public virtual string ReportedBy { get; set; }
        [StringLength(GeneralLiabilityClaim.StringLengths.REPORTED_BY_PHONE)]
        public virtual string ReportedByPhone { get; set; }
        [Required]
        [DateTimePicker]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime? IncidentNotificationDate { get; set; }
        [Required]
        [DateTimePicker]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS, ApplyFormatInEditMode = true)]
        public virtual DateTime? IncidentReportedDate { get; set; }
        public virtual DateTime? CompletedDate { get; set; } 
        [StringLength(GeneralLiabilityClaim.StringLengths.SAP_WORK_ORDER_ID)]
        public virtual string SAPWorkOrderId { get; set; }

        [Required]
        public bool? FiveWhysCompleted { get; set; }

        [RequiredWhen("FiveWhysCompleted", true, ErrorMessage = "This field is required when 'Five Whys Completed' is Yes.")]
        [StringLength(GeneralLiabilityClaim.StringLengths.FIVE_WHYS)]
        public virtual string Why1 { get; set; }

        [StringLength(GeneralLiabilityClaim.StringLengths.FIVE_WHYS)]
        public virtual string Why2 { get; set; }

        [StringLength(GeneralLiabilityClaim.StringLengths.FIVE_WHYS)]
        public virtual string Why3 { get; set; }

        [StringLength(GeneralLiabilityClaim.StringLengths.FIVE_WHYS)]
        public virtual string Why4 { get; set; }

        [StringLength(GeneralLiabilityClaim.StringLengths.FIVE_WHYS)]
        public virtual string Why5 { get; set; }

        [RequiredWhen("FiveWhysCompleted", true, ErrorMessage = "This field is required when 'Five Whys Completed' is Yes.")]
        public virtual DateTime? DateSubmitted { get; set; }

        #endregion

        #region Constructors

        public GeneralLiabilityClaimViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void Map(GeneralLiabilityClaim entity)
        {
            base.Map(entity);
        }

        public override GeneralLiabilityClaim MapToEntity(GeneralLiabilityClaim entity)
        {
            if (CrashType != MapCall.Common.Model.Entities.CrashType.Indices.OTHER)
            {
                OtherTypeOfCrash = null;
            }
            return base.MapToEntity(entity);
        }

        #endregion
    }

    public class CreateGeneralLiabilityClaim : GeneralLiabilityClaimViewModel
    {
        #region Constructors

        public CreateGeneralLiabilityClaim(IContainer container) : base(container) { }

        #endregion

        public override void SetDefaults()
        {
            FiveWhysCompleted = false;
        }

        public override GeneralLiabilityClaim MapToEntity(GeneralLiabilityClaim entity)
        {
            var mapped = base.MapToEntity(entity);
            // Make sure this is called before calling base. The base needs these
            // values set when it maps new Comments and CrewMembers objects.
            mapped.CreatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.FullName;
            return mapped;
        }
    }

    public class EditGeneralLiabilityClaim : GeneralLiabilityClaimViewModel
    {
        #region Constructors

        public EditGeneralLiabilityClaim(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchGeneralLiabilityClaim : SearchSet<GeneralLiabilityClaim>
    {
        [View(DisplayName = "Id")]
        public int? Id { get; set; }
        
        [EntityMap, EntityMustExist(typeof(State))]
        [DropDown, SearchAlias("OperatingCenter", "State.Id", Required = true)]
        public virtual int? State { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateIdOrAll", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }

        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterIds", DependsOn = "OperatingCenter", PromptText = "Select an Operating Center above")]
        public virtual int? CompanyContact { get; set; }

        [DropDown]
        public virtual int? ClaimsRepresentative { get; set; }
        public virtual string ClaimNumber { get; set; }
        [View("Claimant Name")]
        public virtual string Name { get; set; }
        [DropDown]
        public virtual int? LiabilityType { get; set; }
        [DropDown]
        public virtual int? GeneralLiabilityClaimType { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CrashType))]
        public virtual int? CrashType { get; set; }
        [View("Has PHH been contacted for damage to our fleet vehicle?")]
        public virtual bool? PhhContacted { get; set; }
        public virtual DateRange IncidentDateTime { get; set; }
        public virtual bool? PoliceCalled { get; set; }
        public virtual bool? AnyInjuries { get; set; }
        public virtual DateTime? IncidentNotificationDate { get; set; }
    }
}