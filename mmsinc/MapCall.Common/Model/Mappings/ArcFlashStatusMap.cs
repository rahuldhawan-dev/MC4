using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ArcFlashStatusMap : EntityLookupMap<ArcFlashStatus>
    {
        public const string TABLE_NAME = "ArcFlashStatuses";

        public ArcFlashStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
