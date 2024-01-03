using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterSample : IEntityWithCreationTimeTracking, IThingWithDocuments, IThingWithNotes
    {
        public struct StringLengths
        {
            public const int COLLECTED_BY = 50,
                             ANALYSIS_PERFORMED_BY = 50;
        }

        #region Properties

        public virtual int Id { get; set; }
        public virtual SampleIdMatrix SampleIdMatrix { get; set; }
        public virtual UnitOfWaterSampleMeasure UnitOfMeasure { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? SampleDate { get; set; }
        public virtual string CollectedBy { get; set; }
        public virtual string AnalysisPerformedBy { get; set; }
        public virtual float? SampleValue { get; set; }
        public virtual string Notes { get; set; }

        /// <summary>
        /// If true, the sample value is below the range the equipment can detect.
        /// </summary>
        public virtual bool NonDetect { get; set; }

        /// <summary>
        /// If true, the sample value is considered invalid.
        /// </summary>
        public virtual bool IsInvalid { get; set; }

        public virtual ReasonForSampleInvalidation ReasonForInvalidation { get; set; }

        #region References

        public virtual IList<WaterSampleDocument> WaterSampleDocuments { get; set; }
        public virtual IList<WaterSampleNote> WaterSampleNotes { get; set; }

        #endregion

        #region Logical Properties

        public virtual string TableName => nameof(WaterSample) + "s";
        public virtual IList<IDocumentLink> LinkedDocuments => WaterSampleDocuments.Map(d => (IDocumentLink)d);
        public virtual IList<INoteLink> LinkedNotes => WaterSampleNotes.Map(n => (INoteLink)n);

        #endregion

        #endregion

        #region Constructors

        public WaterSample()
        {
            WaterSampleDocuments = new List<WaterSampleDocument>();
            WaterSampleNotes = new List<WaterSampleNote>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
