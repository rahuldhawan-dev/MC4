using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public abstract class BaseMainCrossingInspectionViewModel : ViewModel<MainCrossingInspection>
    {
        #region Properties

        [DisplayName("Main Crossing"), DoesNotAutoMap("Display only")]
        public MainCrossing DisplayMainCrossing { get; internal set; }

        [Required, DisplayFormat(DataFormatString=CommonStringFormats.DATE, ApplyFormatInEditMode=true)]
        public DateTime? InspectedOn { get; set; }

        [Multiline] // This is an ntext field, no max string length.
        public string Comments { get; set; }

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(MainCrossingInspectionAssessmentRating))]
        public int? AssessmentRating { get; set; }

        [Required, DisplayName("Is in service")]
        public bool? PipeIsInService { get; set; }

        [Required, DisplayName("Has excessive corrosion")]
        public bool? PipeHasExcessiveCorrosion { get; set; }

        [Required, DisplayName("Has delaminated steel")]
        public bool? PipeHasDelaminatedSteel { get; set; }

        [Required, DisplayName("Pipe is damaged")]
        public bool? PipeIsDamaged { get; set; }

        [Required, DisplayName("Has cracks")]
        public bool? PipeHasCracks { get; set; }

        [Required, DisplayName("Has concrete spools")]
        public bool? PipeHasConcreteSpools { get; set; }

        [Required, DisplayName("Lacks insulation")]
        public bool? PipeLacksInsulation { get; set; }

        [Required, DisplayName("Are leaking")]
        public bool? JointsAreLeaking { get; set; }

        [Required, DisplayName("Failed separated")]
        public bool? JointsFailedSeparated { get; set; }

        [Required, DisplayName("Restraint damaged")]
        public bool? JointsRestraintDamaged { get; set; }

        [Required, DisplayName("Bond straps damaged")]
        public bool? JointsBondStrapsDamaged { get; set; }

        [Required, DisplayName("Have deficient support")]
        public bool? SupportsHaveDeficientSupport { get; set; }

        [Required, DisplayName("Supports are damaged")]
        public bool? SupportsAreDamaged { get; set; }

        [Required, DisplayName("Have corrosion")]
        public bool? SupportsHaveCorrosion { get; set; }

        [Required, DisplayName("Is in hazardous location")]
        public bool? EnvironmentIsInHazardousLocation { get; set; }

        [Required, DisplayName("Has debris build up")]
        public bool? EnvironmentHasDebrisBuildUp { get; set; }

        [Required, DisplayName("Is submerged in water")]
        public bool? EnvironmentIsSubmergedInWater { get; set; }

        [Required, DisplayName("Is exposed to vehicle impact")]
        public bool? EnvironmentIsExposedToVehicleImpact { get; set; }

        [Required, DisplayName("Is not secured from public")]
        public bool? EnvironmentIsNotSecuredFromPublic { get; set; }

        [Required, DisplayName("Is susceptible to storm damage")]
        public bool? EnvironmentIsSusceptibleToStormDamage { get; set; }

        [Required, DisplayName("Has bank erosion")]
        public bool? AdjacentFacilityHasBankErosion { get; set; }

        [Required, DisplayName("Has bridge damage")]
        public bool? AdjacentFacilityHasBridgeDamage { get; set; }

        [Required, DisplayName("Has pavement failure")]
        public bool? AdjacentFacilityHasPavementFailure { get; set; }

        [Required, DisplayName("Overhead power lines are down")]
        public bool? AdjacentFacilityOverheadPowerLinesAreDown { get; set; }

        [Required, DisplayName("Has property damage")]
        public bool? AdjacentFacilityHasPropertyDamage { get; set; }

        #endregion

        #region Constructor

        protected BaseMainCrossingInspectionViewModel(IContainer container) : base(container) {}
        
        #endregion
    }

    public class CreateMainCrossingInspection : BaseMainCrossingInspectionViewModel
    {
        #region Properties

        // This is not allowed to be changed due to the way that they get to the edit screen.
        [Required, EntityMap, EntityMustExist(typeof(MainCrossing))]
        public int? MainCrossing { get; set; }

        #endregion

        #region Constructors

        public CreateMainCrossingInspection(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override MainCrossingInspection MapToEntity(MainCrossingInspection entity)
        {
            base.MapToEntity(entity);

            var curUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            entity.InspectedBy = curUser;

            return entity;
        }

        #endregion
    }

    public class EditMainCrossingInspection : BaseMainCrossingInspectionViewModel
    {
        #region Properties
        
        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(User))]
        public int? InspectedBy { get; set; } 
        
        #endregion

        #region Constructors

        public EditMainCrossingInspection(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateInspectedByForSiteAdmins()
        {
            var authServ = _container.GetInstance<IAuthenticationService<User>>();
            var existingInspection =
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<MainCrossingInspection>>().Find(Id);

            if (existingInspection.InspectedBy.Id != InspectedBy && !authServ.CurrentUserIsAdmin)
            {
                yield return new ValidationResult("Inspected By value can not be changed by a user.", new[] { "InspectedBy" });
            } 
        }

        #endregion

        #region Public Methods

        public override void Map(MainCrossingInspection entity)
        {
            base.Map(entity);
            DisplayMainCrossing = entity.MainCrossing;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateInspectedByForSiteAdmins());
        }

        #endregion
    }
}