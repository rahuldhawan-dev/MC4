using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// An end of pipe exceedance is when the effluent (liquid waste or sewage) where it enters the environment (aka end of pipe)
    /// exceeds a level established by permit or regulation.  Most commonly used in waste water.
    /// </summary>
    [Serializable]
    public class EndOfPipeExceedance : IEntity, IThingWithOperatingCenter, IThingWithActionItems, IThingWithNotes, IThingWithDocuments
    {
        public struct StringLengths
        {
            public const int EndOfPipeExceedanceRootCauseOtherReason = 255,
                             EndOfPipeExceedanceTypeOtherReason = 255;
        }

        public const string NEW_ACQUISITION_DESCRIPTION = "Did this occur within a year of the acquisition of the system?",
                            CONSENT_ORDER_DESCRIPTION = "Did this occur while the system was under a consent order?";

        #region Properties

        public virtual int Id { get; set; }
        public virtual State State { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        [View(DisplayName = WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public virtual WasteWaterSystem WasteWaterSystem { get; set; }
        public virtual Facility Facility { get; set; }
        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime EventDate { get; set; }
        public virtual EndOfPipeExceedanceType EndOfPipeExceedanceType { get; set; }
        public virtual string EndOfPipeExceedanceTypeOtherReason { get; set; }
        public virtual EndOfPipeExceedanceRootCause EndOfPipeExceedanceRootCause { get; set; }
        public virtual LimitationType LimitationType { get; set; }
        public virtual string EndOfPipeExceedanceRootCauseOtherReason { get; set; }
        [View(Description = CONSENT_ORDER_DESCRIPTION)] 
        public virtual bool ConsentOrder { get; set; }
        [View(Description = NEW_ACQUISITION_DESCRIPTION)]
        public virtual bool NewAcquisition { get; set; }
        public virtual string BriefDescription { get; set; }
        public virtual IList<Note<EndOfPipeExceedance>> Notes { get; set; }
        public virtual IList<Document<EndOfPipeExceedance>> Documents { get; set; }
        public virtual IList<ActionItem<EndOfPipeExceedance>> ActionItems { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();
        public virtual IList<IActionItemLink> LinkedActionItems => ActionItems.Cast<IActionItemLink>().ToList();
        [DoesNotExport]
        public virtual string TableName => nameof(EndOfPipeExceedance) + "s";

        #endregion

        public EndOfPipeExceedance()
        {
            Documents = new List<Document<EndOfPipeExceedance>>();
            Notes = new List<Note<EndOfPipeExceedance>>();
            ActionItems = new List<ActionItem<EndOfPipeExceedance>>();
        }
    }
}

