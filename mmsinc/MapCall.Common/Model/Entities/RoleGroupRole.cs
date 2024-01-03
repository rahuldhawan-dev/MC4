using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RoleGroupRole : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual RoleGroup RoleGroup { get; set; }
        public virtual Module Module { get; set; }
        public virtual RoleAction Action { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        #endregion
    }
}
