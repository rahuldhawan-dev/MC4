using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerStoppageTypeMap : EntityLookupMap<SewerStoppageType>
    {
        public SewerStoppageTypeMap()
        {
            Id(x => x.Id, "SewerStoppageTypeID").Not.Nullable();
        }
    }
}
