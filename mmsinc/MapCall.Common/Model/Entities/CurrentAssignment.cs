using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CurrentAssignment : IEntity
    {
        /// <summary>
        /// This is actually `WorkOrder.Id`
        /// </summary>
        public virtual int Id { get; set; }
        public virtual CrewAssignment CrewAssignment { get; set; }
        public virtual Crew Crew { get; set; }
        public virtual DateTime? AssignedFor { get; set; }
        public virtual string CrewName { get; set; }
    }
}
