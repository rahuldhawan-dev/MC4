using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EstimatingProjectsMaterialsMap : ClassMap<EstimatingProjectMaterial>
    {
        public const string TABLE_NAME = CreateTablesForBug1775.TableNames.ESTIMATING_PROJECTS_MATERIALS;

        public EstimatingProjectsMaterialsMap()
        {
            Table(TABLE_NAME);
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.EstimatingProject).Not.Nullable();
            References(x => x.Material).Not.Nullable();
            References(x => x.AssetType).Not.Nullable();

            Map(x => x.Quantity).Not.Nullable();
            Map(x => x.MaterialCost)
               .DbSpecificFormula(
                    "(SELECT TOP 1 ocsm.Cost FROM EstimatingProjects ep INNER JOIN OperatingCenters oc ON ep.OperatingCenterId = oc.OperatingCenterID INNER JOIN OperatingCenterStockedMaterials ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID WHERE ep.Id = EstimatingProjectId AND ocsm.MaterialID = MaterialId)",
                    "(SELECT ocsm.Cost FROM EstimatingProjects ep INNER JOIN OperatingCenters oc ON ep.OperatingCenterId = oc.OperatingCenterID INNER JOIN OperatingCenterStockedMaterials ocsm ON ocsm.OperatingCenterID = oc.OperatingCenterID WHERE ep.Id = EstimatingProjectId AND ocsm.MaterialID = MaterialId LIMIT 1)");
        }
    }
}
