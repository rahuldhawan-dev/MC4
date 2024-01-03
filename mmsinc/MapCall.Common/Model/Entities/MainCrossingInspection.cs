using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MainCrossingInspection
        : IEntityWithCreationUserTracking<User>, IValidatableObject, IThingWithDocuments, IThingWithNotes
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual MainCrossing MainCrossing { get; set; }
        public virtual User CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime InspectedOn { get; set; }

        public virtual User InspectedBy { get; set; }
        public virtual MainCrossingInspectionAssessmentRating AssessmentRating { get; set; }

        // No string length on this, it's a text field.
        public virtual string Comments { get; set; }

        [DisplayName("Is in service")]
        public virtual bool PipeIsInService { get; set; }

        [DisplayName("Has excessive corrosion")]
        public virtual bool PipeHasExcessiveCorrosion { get; set; }

        [DisplayName("Has delaminated steel")]
        public virtual bool PipeHasDelaminatedSteel { get; set; }

        [DisplayName("Pipe is damaged")]
        public virtual bool PipeIsDamaged { get; set; }

        [DisplayName("Has cracks")]
        public virtual bool PipeHasCracks { get; set; }

        [DisplayName("Has concrete spools")]
        public virtual bool PipeHasConcreteSpools { get; set; }

        [DisplayName("Lacks insulation")]
        public virtual bool PipeLacksInsulation { get; set; }

        [DisplayName("Are leaking")]
        public virtual bool JointsAreLeaking { get; set; }

        [DisplayName("Failed separated")]
        public virtual bool JointsFailedSeparated { get; set; }

        [DisplayName("Restraint damaged")]
        public virtual bool JointsRestraintDamaged { get; set; }

        [DisplayName("Bond straps damaged")]
        public virtual bool JointsBondStrapsDamaged { get; set; }

        [DisplayName("Have deficient support")]
        public virtual bool SupportsHaveDeficientSupport { get; set; }

        [DisplayName("Supports are damaged")]
        public virtual bool SupportsAreDamaged { get; set; }

        [DisplayName("Have corrosion")]
        public virtual bool SupportsHaveCorrosion { get; set; }

        [DisplayName("Is in hazardous location")]
        public virtual bool EnvironmentIsInHazardousLocation { get; set; }

        [DisplayName("Has debris build up")]
        public virtual bool EnvironmentHasDebrisBuildUp { get; set; }

        [DisplayName("Is submerged in water")]
        public virtual bool EnvironmentIsSubmergedInWater { get; set; }

        [DisplayName("Is exposed to vehicle impact")]
        public virtual bool EnvironmentIsExposedToVehicleImpact { get; set; }

        [DisplayName("Is not secured from public")]
        public virtual bool EnvironmentIsNotSecuredFromPublic { get; set; }

        [DisplayName("Is susceptible to storm damage")]
        public virtual bool EnvironmentIsSusceptibleToStormDamage { get; set; }

        [DisplayName("Has bank erosion")]
        public virtual bool AdjacentFacilityHasBankErosion { get; set; }

        [DisplayName("Has bridge damage")]
        public virtual bool AdjacentFacilityHasBridgeDamage { get; set; }

        [DisplayName("Has pavement failure")]
        public virtual bool AdjacentFacilityHasPavementFailure { get; set; }

        [DisplayName("Overhead power lines are down")]
        public virtual bool AdjacentFacilityOverheadPowerLinesAreDown { get; set; }

        [DisplayName("Has property damage")]
        public virtual bool AdjacentFacilityHasPropertyDamage { get; set; }

        public virtual string TableName => nameof(MainCrossingInspection) + "s";

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<MainCrossingInspectionDocument> Documents { get; set; }

        public virtual IList<MainCrossingInspectionNote> Notes { get; set; }

        #endregion

        #region Constructor

        public MainCrossingInspection()
        {
            Documents = new List<MainCrossingInspectionDocument>();
            Notes = new List<MainCrossingInspectionNote>();
        }

        #endregion

        #region Public Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
