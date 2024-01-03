using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OperatingCenterStockedMaterialMap : ClassMap<OperatingCenterStockedMaterial>
    {
        public OperatingCenterStockedMaterialMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("OperatingCenterStockedMaterialID");

            References(x => x.OperatingCenter);
            References(x => x.Material);

            Map(x => x.Cost);
        }
    }
}
