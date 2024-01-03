using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StandardOperatingProcedure : IEntity, IThingWithDocuments, IThingWithNotes, IThingWithVideos
    {
        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }
        public virtual int? SopCrossRefId { get; set; }
        public virtual string Description { get; set; }
        public virtual int? EquipmentId { get; set; }
        public virtual DateTime? DateApproved { get; set; }
        public virtual DateTime? DateIssued { get; set; }
        public virtual string Revision { get; set; }
        public virtual string ReviewFrequencyDays { get; set; }
        public virtual bool? PsmTcpa { get; set; }
        public virtual bool? Dpcc { get; set; }
        public virtual bool? Osha { get; set; }
        public virtual bool? Company { get; set; }
        public virtual bool? Sox { get; set; }
        public virtual bool? Safety { get; set; }

        /// <summary>
        /// This is a formula property.
        /// </summary>
        public virtual bool HasReviewRequirements { get; protected set; }

        #endregion

        #region References

        public virtual SOPSection Section { get; set; }
        public virtual SOPSubSection SubSection { get; set; }
        public virtual PolicyPractice PolicyPractice { get; set; }
        public virtual FunctionalArea FunctionalArea { get; set; }
        public virtual SOPStatus Status { get; set; }
        public virtual SOPCategory Category { get; set; }
        public virtual SOPSystem System { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual IList<StandardOperatingProcedureQuestion> Questions { get; set; }

        public virtual IList<StandardOperatingProcedurePositionGroupCommonNameRequirement> PGCNRequirements
        {
            get;
            set;
        }

        public virtual IList<TrainingModule> TrainingModules { get; set; }

        #endregion

        #region Logical Properties

        /// <summary>
        /// Returns the review questions that are currently active for this SOP.
        /// </summary>
        public virtual IEnumerable<StandardOperatingProcedureQuestion> ActiveQuestions
        {
            get { return Questions.Where(x => x.IsActive); }
        }

        #region Documents

        public virtual IList<StandardOperatingProcedureDocument> StandardOperatingProcedureDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return StandardOperatingProcedureDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return StandardOperatingProcedureDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<StandardOperatingProcedureNote> StandardOperatingProcedureNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return StandardOperatingProcedureNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return StandardOperatingProcedureNotes.Map(n => n.Note); }
        }

        #endregion

        #region Videos

        public virtual IList<StandardOperatingProcedureVideo> Videos { get; set; }

        public virtual IEnumerable<IVideoLink> LinkedVideos => Videos;

        #endregion

        public virtual string TableName => StandardOperatingProcedureMap.TABLE_NAME;

        #endregion

        #endregion

        #region Constructors

        public StandardOperatingProcedure()
        {
            StandardOperatingProcedureDocuments = new List<StandardOperatingProcedureDocument>();
            StandardOperatingProcedureNotes = new List<StandardOperatingProcedureNote>();
            Questions = new List<StandardOperatingProcedureQuestion>();
            PGCNRequirements = new List<StandardOperatingProcedurePositionGroupCommonNameRequirement>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
