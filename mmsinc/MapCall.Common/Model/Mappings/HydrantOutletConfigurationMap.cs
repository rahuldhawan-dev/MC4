using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantOutletConfigurationMap : EntityLookupMap<HydrantOutletConfiguration>
    {
        public HydrantOutletConfigurationMap()
        {
            Map(x => x.SAPCode);
        }
    }
}
