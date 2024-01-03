using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EmployeeHeadCount : IEntity, IThingWithDocuments, IThingWithNotes
    {
        #region Consts

        public struct StringLengths
        {
            public const int CREATED_BY = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual BusinessUnit BusinessUnit { get; set; }
        public virtual int? Year { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? StartDate { get; set; }
        
        [View(FormatStyle.Date)]
        public virtual DateTime? EndDate { get; set; }
        
        public virtual EmployeeHeadCountCategory Category { get; set; }

        [View("Non Union")]
        public virtual int NonUnionCount { get; set; }

        [View("Union")]
        public virtual int UnionCount { get; set; }

        [View("Other")]
        public virtual int OtherCount { get; set; }

        [View("Total")]
        public virtual int TotalCount { get; set; }

        [View("Notes"), Multiline]
        public virtual string MiscNotes { get; set; }

        public virtual DateTime? CreatedAt { get; set; }
        public virtual string CreatedBy { get; set; }

        #region Notes/Docs

        [DoesNotExport]
        public virtual string TableName => nameof(EmployeeHeadCount) + "s";

        public virtual IList<Document<EmployeeHeadCount>> Documents { get; set; }
        public virtual IList<Note<EmployeeHeadCount>> Notes { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #region Logical props

        public virtual OperatingCenter OperatingCenter => BusinessUnit?.OperatingCenter;

        #endregion

        #endregion

        #region Constructor

        public EmployeeHeadCount()
        {
            Documents = new List<Document<EmployeeHeadCount>>();
            Notes = new List<Note<EmployeeHeadCount>>();
        }

        #endregion
    }
}
