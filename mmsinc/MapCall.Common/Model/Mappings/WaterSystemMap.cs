using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WaterSystemMap : EntityLookupMap<WaterSystem>
    {
        public WaterSystemMap()
        {
            Map(x => x.LongDescription);

            HasManyToMany(x => x.OperatingCenters)
               .Table("OperatingCenterWaterSystems")
               .ParentKeyColumn("WaterSystemID")
               .ChildKeyColumn("OperatingCenterID")
               .Cascade.All();
        }
    }
}
