using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OperatingCenterServiceMaterialMap : ClassMap<OperatingCenterServiceMaterial>
    {
        public const string TABLE_NAME = "OperatingCentersServiceMaterials";

        public OperatingCenterServiceMaterialMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.OperatingCenter);
            References(x => x.ServiceMaterial);
            Map(x => x.NewServiceRecord);
        }
    }
}
