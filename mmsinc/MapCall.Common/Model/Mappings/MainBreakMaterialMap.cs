using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainBreakMaterialMap : EntityLookupMap<MainBreakMaterial>
    {
        public MainBreakMaterialMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MainBreakMaterialID").Not.Nullable();
        }
    }
}
