using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerTerminationTypeMap : EntityLookupMap<SewerTerminationType>
    {
        public SewerTerminationTypeMap()
        {
            Id(x => x.Id, "SewerTerminationTypeId");
        }
    }
}
