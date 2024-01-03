using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerOpeningTypeMap : EntityLookupMap<SewerOpeningType>
    {
        public SewerOpeningTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
