using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainInCasingStatusMap : EntityLookupMap<MainInCasingStatus>
    {
        public const string TABLE_NAME = "MainInCasingStatuses";

        public MainInCasingStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
