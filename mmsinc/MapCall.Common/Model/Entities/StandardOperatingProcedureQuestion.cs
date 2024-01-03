using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StandardOperatingProcedureQuestion : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual string Question { get; set; }
        public virtual string Answer { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual StandardOperatingProcedure StandardOperatingProcedure { get; set; }

        #endregion
    }
}
