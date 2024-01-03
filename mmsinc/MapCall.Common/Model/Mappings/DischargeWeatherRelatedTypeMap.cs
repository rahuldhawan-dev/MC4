using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class DischargeWeatherRelatedTypeMap : EntityLookupMap<DischargeWeatherRelatedType>
    {
        private const string TABLE_NAME = "DischargeWeatherRelatedTypes";
        
        public DischargeWeatherRelatedTypeMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
