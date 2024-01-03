using System;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DocumentData : IEntity
    {
        #region Consts

        public const int MAX_HASH_LENGTH = CreateDocumentDataTable.HASH_STRING_LENGTH;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Hash { get; set; }
        public virtual int FileSize { get; set; }

        /// <summary>
        /// Only set this property when creating a NEW DocumentData instance.
        /// </summary>
        public virtual byte[] BinaryData { get; set; }

        #endregion
    }
}
