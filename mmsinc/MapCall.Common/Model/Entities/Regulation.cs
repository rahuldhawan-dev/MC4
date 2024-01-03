using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Regulation
        : IEntityLookup, IThingWithNotes, IThingWithDocuments, IEntityWithCreationTimeTracking
    {
        #region Constants

        public struct StringLengths
        {
            public const int REGULATION_SHORT = 100;
        }

        #endregion

        #region Private Members

        private RegulationDisplayItem _display;
        private RegulationOSHADisplayItem _oshaDisplay;

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        [DisplayName("Regulation")]
        public virtual string RegulationShort { get; set; }

        public virtual string Title { get; set; }
        public virtual string Statute { get; set; }
        public virtual string Citation { get; set; }
        public virtual DateTime? EffectiveDate { get; set; }
        public virtual string Purpose { get; set; }
        public virtual string GeneralDescription { get; set; }
        public virtual string Requirements { get; set; }
        public virtual string UtilitiesCovered { get; set; }
        public virtual string CostImpact { get; set; }
        public virtual string Notes { get; set; }
        public virtual bool AllAreas { get; set; }
        public virtual bool FieldOperations { get; set; }
        public virtual bool Production { get; set; }
        public virtual bool Environmental { get; set; }
        public virtual bool WaterQuality { get; set; }
        public virtual bool Engineering { get; set; }

        #endregion

        #region References

        public virtual RegulationAgency Agency { get; set; }
        public virtual RegulationStatus Status { get; set; }
        public virtual IList<TrainingRequirement> TrainingRequirements { get; set; }
        public virtual IList<RegulationNote> RegulationNotes { get; set; }
        public virtual IList<RegulationDocument> RegulationDocuments { get; set; }

        #endregion

        #region Logical Properties

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return RegulationDocuments.Map(x => (IDocumentLink)x); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return RegulationNotes.Map(x => (INoteLink)x); }
        }

        public virtual string TableName => nameof(Regulation) + "s";

        public virtual string Description => (_display ?? (_display = new RegulationDisplayItem {
            RegulationShort = RegulationShort,
            Title = Title
        })).Display;

        public virtual string OSHADescription => (_oshaDisplay ?? (_oshaDisplay = new RegulationOSHADisplayItem {
            Citation = Citation,
            Title = Title
        })).Display;

        #endregion

        #endregion

        #region Constructors

        public Regulation()
        {
            TrainingRequirements = new List<TrainingRequirement>();
            RegulationNotes = new List<RegulationNote>();
            RegulationDocuments = new List<RegulationDocument>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }

    [Serializable]
    public class RegulationDisplayItem : DisplayItem<Regulation>
    {
        public string RegulationShort { get; set; }
        public string Title { get; set; }

        public override string Display =>
            String.IsNullOrWhiteSpace(RegulationShort) ? Title : $"{Title} - {RegulationShort}";
    }

    [Serializable]
    public class RegulationOSHADisplayItem : DisplayItem<Regulation>
    {
        public string Citation { get; set; }
        public string Title { get; set; }

        public override string Display => $"{Citation} - {Title}";
    }
}
