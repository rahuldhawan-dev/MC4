using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using NHibernate.Mapping;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PositionHistory : IEntity, IThingWithNotes, IThingWithDocuments
    {
        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string StatusChangeReason { get; set; }
        public virtual string PositionSubcategory { get; set; }
        public virtual string VacationGrouping { get; set; }
        public virtual bool? FullyQualified { get; set; }

        [Display(Name = "On-Call Requirement")]
        public virtual bool? OnCallRequirement { get; set; }

        #endregion

        #region References

        public virtual Position Position { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual DepartmentName Department { get; set; }
        public virtual Facility ReportingFacility { get; set; }
        public virtual ScheduleType ScheduleType { get; set; }

        public virtual IList<PositionHistoryNote> PositionHistoryNotes { get; set; }
        public virtual IList<PositionHistoryDocument> PositionHistoryDocuments { get; set; }

        #endregion

        #region Logical Properties

        public virtual string TableName => PositionHistoryMap.TABLE_NAME;

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return PositionHistoryNotes.Map(x => (INoteLink)x); }
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return PositionHistoryDocuments.Map(x => (IDocumentLink)x); }
        }

        [Display(Name = "Notes")]
        public virtual int NoteCount => PositionHistoryNotes.Count;

        [Display(Name = "Documents")]
        public virtual int DocumentCount => PositionHistoryDocuments.Count;

        #endregion

        #endregion

        #region Constructors

        public PositionHistory()
        {
            PositionHistoryNotes = new List<PositionHistoryNote>();
            PositionHistoryDocuments = new List<PositionHistoryDocument>();
        }

        #endregion
    }
}
