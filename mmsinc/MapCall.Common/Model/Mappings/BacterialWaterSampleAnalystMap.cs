using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BacterialWaterSampleAnalystMap : ClassMap<BacterialWaterSampleAnalyst>
    {
        public BacterialWaterSampleAnalystMap()
        {
            Id(x => x.Id);

            Map(x => x.IsActive).Not.Nullable();

            References(x => x.Employee).Not.Nullable();

            HasManyToMany(x => x.OperatingCenters)
               .Table("BacterialWaterSampleAnalystsOperatingCenters")
               .ParentKeyColumn("BacterialWaterSampleAnalystId")
               .ChildKeyColumn("OperatingCenterId");
        }
    }
}
