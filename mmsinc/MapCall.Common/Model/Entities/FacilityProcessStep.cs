using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityProcessStep : IEntity, IThingWithDocuments, IThingWithNotes
    {
        #region Consts

        public const int MAX_DESCRIPTION_LENGTH = 50;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        /// <summary>
        /// NULLABLE! Do not use this to get the facility. Use FacilityProcess instead.
        /// </summary>
        public virtual Equipment Equipment { get; set; }

        public virtual FacilityProcess FacilityProcess { get; set; }

        [DisplayName("Process Measurement")]
        public virtual FacilityProcessStepSubProcess FacilityProcessStepSubProcess { get; set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        [DisplayName("Elevation(ASL-FT)")]
        public virtual int ElevationInFeet { get; set; }

        public virtual decimal ProcessTarget { get; set; }
        public virtual decimal NormalRangeMin { get; set; }
        public virtual decimal NormalRangeMax { get; set; }
        public virtual string Description { get; set; }

        [Range(0.01, 999999.99),
         DisplayFormat(DataFormatString = CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS,
             ApplyFormatInEditMode = true)]
        public virtual decimal StepNumber { get; set; }

        public virtual string ContingencyOperation { get; set; }
        public virtual string LossOfCommunicationPowerImpact { get; set; }

        public virtual IList<FacilityProcessStepTrigger> Triggers { get; set; }

        #endregion

        #region Logical Properties

        #region Documents

        public virtual IList<FacilityProcessStepDocument> FacilityProcessStepDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return FacilityProcessStepDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return FacilityProcessStepDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<FacilityProcessStepNote> FacilityProcessStepNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return FacilityProcessStepNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return FacilityProcessStepNotes.Map(n => n.Note); }
        }

        #endregion

        [DoesNotExport]
        public virtual string TableName => nameof(FacilityProcessStep) + "s";

        [DoesNotExport]
        public virtual bool HasScadaReadings => Equipment?.ScadaTagName != null;

        #endregion

        #endregion

        #region Constructors

        public FacilityProcessStep()
        {
            FacilityProcessStepDocuments = new List<FacilityProcessStepDocument>();
            FacilityProcessStepNotes = new List<FacilityProcessStepNote>();
            Triggers = new List<FacilityProcessStepTrigger>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return String.Format("{0} - {1} {2}", Id, FacilityProcess.Facility, FacilityProcess);
        }

        #endregion
    }

    [Serializable]
    public class FacilityProcessStepAlarm : EntityLookup { }

    [Serializable]
    public class FacilityProcessStepTriggerLevel : EntityLookup { }

    [Serializable]
    public class FacilityProcessStepTriggerType : EntityLookup { }
}
