using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SmallMeterLocationMap : EntityLookupMap<SmallMeterLocation>
    {
        public SmallMeterLocationMap()
        {
            Map(x => x.SAPCode);

            HasManyToMany(x => x.MeterSupplementalLocations)
               .Table("MeterLocationMeterPositions")
               .ParentKeyColumn("SmallMeterLocationId")
               .ChildKeyColumn("MeterLocationId");
        }
    }
}
