using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RecurringProjectRegulatoryStatusMap : EntityLookupMap<RecurringProjectRegulatoryStatus>
    {
        public const string TABLE_NAME = "RecurringProjectRegulatoryStatuses";

        public RecurringProjectRegulatoryStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
