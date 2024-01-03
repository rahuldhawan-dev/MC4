using System;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPEmployeeAssignments
    {
        #region properties

        public virtual string CrewAssign { get; set; }
        public virtual string DateStart { get; set; }
        public virtual string DateEnd { get; set; }
        public virtual string DateCompleted { get; set; }
        public virtual string StartTime { get; set; }
        public virtual string EndTime { get; set; }
        public virtual double? TotalManHours { get; set; }
        public virtual string EmployeeId { get; set; }

        #endregion
    }
}
