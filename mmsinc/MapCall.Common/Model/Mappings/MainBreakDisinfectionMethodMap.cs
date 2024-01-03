using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainBreakDisinfectionMethodMap : EntityLookupMap<MainBreakDisinfectionMethod>
    {
        public MainBreakDisinfectionMethodMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MainBreakDisinfectionMethodID").Not.Nullable();
        }
    }
}
