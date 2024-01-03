using System;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// <see cref="Hydrant"/>s rust, so they must be protected with paint.  Paint fades and deteriorates, so
    /// it must be reapplied.  These records track when <see cref="Hydrant"/>s are painted so they can be
    /// repainted on a schedule.  See <see cref="HydrantDuePainting"/> and its underlying sql view for more
    /// information.
    /// </summary>
    [Serializable]
    public class HydrantPainting : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual Hydrant Hydrant { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual User UpdatedBy { get; set; }

        public virtual DateTime PaintedAt { get; set; }

        #endregion
    }
}
