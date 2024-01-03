using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;
using Newtonsoft.Json;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Process : IEntity, IThingWithNotes, IThingWithDocuments
    {
        #region Consts

        public const int MAX_DESCRIPTION_LENGTH = 50;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Sequence { get; set; }

        [Multiline]
        public virtual string ProcessOverview { get; set; }

        [JsonIgnore]
        public virtual ProcessStage ProcessStage { get; set; }

        [JsonIgnore]
        public virtual IList<ProcessDocument> Documents { get; set; }

        [JsonIgnore]
        public virtual IList<ProcessNote> Notes { get; set; }

        [JsonIgnore]
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        [JsonIgnore]
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [JsonIgnore, DoesNotExport]
        public virtual string TableName => ProcessMap.TABLE_NAME;

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
