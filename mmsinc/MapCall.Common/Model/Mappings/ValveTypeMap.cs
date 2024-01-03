using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ValveTypeMap : EntityLookupMap<ValveType>
    {
        public ValveTypeMap()
        {
            Map(x => x.SAPCode);
        }
    }
}
