using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DocumentType : IEntity
    {
        #region Constants

        public struct Indices
        {
            public const int CONTRACT = 1, 
                             STANDARD_OPERATING_PROCEDURE = 441, 
                             NEAR_MISS_DOCUMENT = 609, 
                             PREMISE = 198;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual DataType DataType { get; set; }

        public virtual IList<Document> Documents { get; set; }

        #endregion

        #region Constructors

        public DocumentType()
        {
            Documents = new List<Document>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
