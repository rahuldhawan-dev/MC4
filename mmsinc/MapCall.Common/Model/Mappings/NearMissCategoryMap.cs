using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class NearMissCategoryMap : EntityLookupMap<NearMissCategory>
    {
        public const string TABLE_NAME = "NearMissCategories";

        public NearMissCategoryMap()
        {
            Table(TABLE_NAME);
            References(x => x.Type);
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
