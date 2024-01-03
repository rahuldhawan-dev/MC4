using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class HealthHazardMap : EntityLookupMap<HealthHazard>
    {
        protected override int DescriptionLength => 100;
    }
}
