using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleIdMatrix : IEntity, IValidatableObject, IThingWithNotes, IThingWithDocuments
    {
        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual SampleSite SampleSite { get; set; }
        public virtual WaterConstituent WaterConstituent { get; set; }
        public virtual string Parameter { get; set; }

        public virtual bool RoutineSample { get; set; }
        public virtual string ProcessStage { get; set; }
        public virtual float? ProcessStageSequence { get; set; }
        public virtual float? ParameterSequence { get; set; }
        public virtual string SamplePurpose { get; set; }
        public virtual string ProcessReasonForSample { get; set; }
        public virtual string PerformedBy { get; set; }
        public virtual float? Frequency { get; set; }
        public virtual string DataStorageLocation { get; set; }
        public virtual string MethodInstrumentLaboratory { get; set; }

        [DisplayName("TAT Bellville Lab Hours")]
        public virtual string TatBellvilleLabHrs { get; set; }

        public virtual string BellevilleSampleId { get; set; }
        public virtual string InterferenceBy { get; set; }
        public virtual string DataStorageLocationOnLineInstrument { get; set; }

        [DisplayName("iHistoring SignalID Online Instrument")]
        public virtual string IHistorianSignalIdOnLineInstrument { get; set; }

        public virtual string ComplianceReq { get; set; }
        public virtual string ProcessTarget { get; set; }
        public virtual string TriggerPhase1 { get; set; }
        public virtual string ActionPhase1 { get; set; }
        public virtual string TriggerPhase2 { get; set; }
        public virtual string ActionPhase2 { get; set; }
        public virtual string Comment { get; set; }
        public virtual string ScadaNotes { get; set; }

        #endregion

        #region Logical Properties

        public virtual string TableName => SampleIdMatrixMap.TABLE_NAME;
        public virtual IList<IDocumentLink> LinkedDocuments => SampleIdMatrixDocuments.Map(d => (IDocumentLink)d);
        public virtual IList<INoteLink> LinkedNotes => SampleIdMatrixNotes.Map(n => (INoteLink)n);

        public virtual string Description => new SampleIdMatrixDisplayItem {
            Id = Id,
            SampleSite = SampleSite?.Id ?? 0,
            Parameter = Parameter,
            WaterConstituent = WaterConstituent?.Description
        }.Display;

        #endregion

        #region References

        public virtual IList<SampleIdMatrixDocument> SampleIdMatrixDocuments { get; set; }
        public virtual IList<SampleIdMatrixNote> SampleIdMatrixNotes { get; set; }
        public virtual IList<WaterSample> WaterSamples { get; set; }

        #endregion

        #endregion

        #region Constructor

        public SampleIdMatrix()
        {
            SampleIdMatrixDocuments = new List<SampleIdMatrixDocument>();
            SampleIdMatrixNotes = new List<SampleIdMatrixNote>();
            WaterSamples = new List<WaterSample>();
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
    public class SampleIdMatrixDisplayItem : DisplayItem<SampleIdMatrix>
    {
        [SelectDynamic("Id")]
        public int SampleSite { get; set; }

        public string Parameter { get; set; }

        [SelectDynamic("Description")]
        public string WaterConstituent { get; set; }

        public override string Display =>
            $"{Id}, {((SampleSite > 0) ? SampleSite.ToString() : string.Empty)}, {Parameter} : {WaterConstituent}";
    }
}
