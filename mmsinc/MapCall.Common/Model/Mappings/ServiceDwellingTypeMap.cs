using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceDwellingTypeMap : EntityLookupMap<ServiceDwellingType>
    {
        public ServiceDwellingTypeMap()
        {
            Map(x => x.SewerGPD).Not.Nullable();
            Map(x => x.WaterGPD).Not.Nullable();
        }
    }
}
