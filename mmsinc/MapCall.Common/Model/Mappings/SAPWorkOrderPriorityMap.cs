using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SAPWorkOrderPriorityMap : EntityLookupMap<SAPWorkOrderPriority>
    {
        public const string TABLE_NAME = "SAPWorkOrderPriorities";

        public SAPWorkOrderPriorityMap()
        {
            Table(TABLE_NAME);
        }
    }
}
