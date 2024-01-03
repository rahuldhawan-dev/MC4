using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CategoryOfServiceGroupMap : EntityLookupMap<CategoryOfServiceGroup>
    {
        public CategoryOfServiceGroupMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("CategoryOfServiceGroupID").Not.Nullable();
        }
    }
}
