using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class LicensedOperatorCategoryMap : EntityLookupMap<LicensedOperatorCategory>
    {
        public LicensedOperatorCategoryMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
