using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class BondTypeMap : EntityLookupMap<BondType>
    {
        public BondTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("BondTypeID").Not.Nullable();
        }
    }
}
