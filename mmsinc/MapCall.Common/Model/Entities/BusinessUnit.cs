using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BusinessUnit : IEntity, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public struct StringLengths
        {
            public const int BU = 6,
                             DESCRIPTION = 255;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual Department Department { get; set; }

        [StringLength(StringLengths.BU)]
        public virtual string BU { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual BusinessUnitArea Area { get; set; }

        public virtual int Order { get; set; }
        public virtual string Description { get; set; }

        [View("Is 271 Visible")]
        public virtual bool Is271Visible { get; set; }
        public virtual Employee EmployeeResponsible { get; set; }
        public virtual int? AuthorizedStaffingLevelTotal { get; set; }
        public virtual int? AuthorizedStaffingLevelManagement { get; set; }
        public virtual int? AuthorizedStaffingLevelNonBargainingUnit { get; set; }
        public virtual int? AuthorizedStaffingLevelBargainingUnit { get; set; }
        public virtual bool? IsActive { get; set; }

        #region Notes/Docs

        [DoesNotExport]
        public virtual string TableName => nameof(BusinessUnit) + "s";

        public virtual IList<Document<BusinessUnit>> Documents { get; set; }
        public virtual IList<Note<BusinessUnit>> Notes { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #endregion

        #region Constructor

        public BusinessUnit()
        {
            Documents = new List<Document<BusinessUnit>>();
            Notes = new List<Note<BusinessUnit>>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return BU;
        }

        #endregion
    }
}
