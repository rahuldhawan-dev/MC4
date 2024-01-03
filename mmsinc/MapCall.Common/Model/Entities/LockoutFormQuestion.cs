using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class LockoutFormQuestion : IEntity
    {
        public virtual int Id { get; set; }
        public virtual LockoutFormQuestionCategory Category { get; set; }
        public virtual string Question { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int DisplayOrder { get; set; }
    }
}
