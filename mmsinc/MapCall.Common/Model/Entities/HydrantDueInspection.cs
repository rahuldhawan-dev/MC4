using System;
using System.ComponentModel;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Represents the inspection status of a given <see cref="Hydrant"/>, when the last time it was
    /// inspected was, and whether or not it currently requires inspection given its (or its
    /// <see cref="OperatingCenter"/>'s) hydrant inspection schedule (with or without zones involved).
    /// </summary>
    [Serializable]
    public class HydrantDueInspection : IEntity
    {
        /// <summary>
        /// This is actually `Hydrant.Id`
        /// </summary>
        public virtual int Id { get; set; }

        public virtual Hydrant Hydrant { get; set; }

        public virtual bool RequiresInspection { get; set; }

        [DisplayName("Last Inspection")]
        public virtual DateTime? LastInspectionDate { get; set; }
    }
}
