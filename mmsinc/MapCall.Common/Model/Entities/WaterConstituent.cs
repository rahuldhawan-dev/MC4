using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterConstituent : IEntityLookup, IThingWithDocuments, IThingWithNotes
    {
        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        [StringLength(255)]
        public virtual string Description { get; set; }

        [DisplayName("EPA Min")]
        public virtual float? Min { get; set; }

        [DisplayName("EPA Max")]
        public virtual float? Max { get; set; }

        [DisplayName("EPA MCL")]
        public virtual float? Mcl { get; set; }

        [DisplayName("EPA MCLG")]
        public virtual float? Mclg { get; set; }

        [DisplayName("EPA SMCL")]
        public virtual float? Smcl { get; set; }

        [StringLength(255)]
        [DisplayName("EPA Action Limit")]
        public virtual string ActionLimit { get; set; }

        [StringLength(255)]
        [DisplayName("EPA Regulation")]
        public virtual string Regulation { get; set; }

        [StringLength(255)]
        public virtual string SamplingFrequency { get; set; }

        [StringLength(255)]
        public virtual string SamplingMethod { get; set; }

        [StringLength(255)]
        public virtual string SampleContainerSizeMl { get; set; }

        [StringLength(255)]
        public virtual string HoldingTimeHrs { get; set; }

        [StringLength(255)]
        public virtual string PreservativeQuenchingAgent { get; set; }

        [StringLength(255)]
        public virtual string AnalyticalMethod { get; set; }

        [StringLength(255)]
        public virtual string TatBellvileDays { get; set; }

        public virtual bool NoEPALimits { get; set; }

        #endregion

        #region References

        public virtual UnitOfWaterSampleMeasure UnitOfMeasure { get; set; }
        public virtual WasteWaterContaminantCategory WasteWaterContaminantCategory { get; set; }
        public virtual DrinkingWaterContaminantCategory DrinkingWaterContaminantCategory { get; set; }

        public virtual IList<WaterConstituentStateLimit> StateLimits { get; set; }

        public virtual IList<WaterConstituentNote> WaterConstituentNotes { get; set; }
        public virtual IList<WaterConstituentDocument> WaterConstituentDocuments { get; set; }

        #endregion

        #region Logical Properties

        public virtual string TableName => nameof(WaterConstituent) + "s";

        public virtual IList<IDocumentLink> LinkedDocuments => WaterConstituentDocuments.Map(d => (IDocumentLink)d);
        public virtual IList<INoteLink> LinkedNotes => WaterConstituentNotes.Map(n => (INoteLink)n);

        #endregion

        #endregion

        #region Constructors

        public WaterConstituent()
        {
            StateLimits = new List<WaterConstituentStateLimit>();
            WaterConstituentNotes = new List<WaterConstituentNote>();
            WaterConstituentDocuments = new List<WaterConstituentDocument>();
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
}
