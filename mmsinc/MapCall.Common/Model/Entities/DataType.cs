using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DataType : IEntity
    {
        public virtual int Id { get; set; }

        [StringLength(255)]
        public virtual string Name { get; set; }

        [StringLength(255)]
        public virtual string TableID { get; set; }

        [StringLength(255)]
        public virtual string TableName { get; set; }

        public virtual IList<DocumentType> DocumentTypes { get; set; }

        public DataType()
        {
            DocumentTypes = new List<DocumentType>();
        }
    }
}
