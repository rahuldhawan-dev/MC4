using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MainFailureTypeMap : EntityLookupMap<MainFailureType>
    {
        public MainFailureTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("MainFailureTypeID").Not.Nullable();
        }
    }
}
