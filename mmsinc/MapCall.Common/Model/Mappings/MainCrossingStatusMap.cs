using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainCrossingStatusMap : EntityLookupMap<MainCrossingStatus>
    {
        public const string TABLE_NAME = "MainCrossingStatuses";

        public MainCrossingStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
