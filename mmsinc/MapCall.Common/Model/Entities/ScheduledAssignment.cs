using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ScheduledAssignment : IEntity
    {
        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }
        public virtual MaintenancePlan MaintenancePlan { get; set; }
        public virtual Employee AssignedTo { get; set; }
        public virtual Employee CreatedBy { get; set; }
        public virtual DateTime AssignedFor { get; set; }
        public virtual DateTime ScheduledDate { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public override string ToString() => $"{AssignedTo.FullName} - {AssignedFor.ToShortDateString()}";

        #endregion
    }
}

