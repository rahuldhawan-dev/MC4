using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderPriorityMap : EntityLookupMap<WorkOrderPriority>
    {
        #region Constants

        public const string TABLE_NAME = "WorkOrderPriorities";

        #endregion

        #region Constructors

        public WorkOrderPriorityMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "WorkOrderPriorityID").GeneratedBy.Assigned();
        }

        #endregion
    }
}
