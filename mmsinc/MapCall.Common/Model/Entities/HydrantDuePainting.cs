using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Represents the paint status of a given <see cref="Hydrant"/>, when the last time it was painted was,
    /// and whether or not it currently requires paint given its (or its <see cref="OperatingCenter"/>'s)
    /// hydrant painting schedule (with or without zones involved).
    /// </summary>
    [Serializable]
    public class HydrantDuePainting : IEntity
    {
        /// <summary>
        /// This is actually `Hydrant.Id`
        /// </summary>
        public virtual int Id { get; set; }

        public virtual Hydrant Hydrant { get; set; }

        public virtual bool RequiresPainting { get; set; }
        public virtual DateTime? LastPaintedAt { get; set; }
    }
}
