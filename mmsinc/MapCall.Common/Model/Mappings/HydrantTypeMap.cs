using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantTypeMap : EntityLookupMap<HydrantType>
    {
        public HydrantTypeMap()
        {
            Map(x => x.SAPCode);
        }
    }
}
