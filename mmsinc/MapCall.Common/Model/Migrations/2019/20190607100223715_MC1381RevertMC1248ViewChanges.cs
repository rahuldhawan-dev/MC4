using FluentMigrator;
using MapCall.Common.Model.Migrations._2018;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190607100223715), Tags("Production")]
    public class MC1381RevertMC1248ViewChanges : Migration
    {
        public override void Up()
        {
            Execute.Sql(AlterHydrantsDueInspectionViewToExcludeHydrantsNeverPreviouslyInspectedForMC78
                       .Sql.DROP_HYDRANT_VIEW);
            Execute.Sql(
                AlterHydrantsDueInspectionViewToExcludeHydrantsNeverPreviouslyInspectedForMC78
                   .Sql.CREATE_HYDRANT_VIEW_SQL_FORMAT, "GetDate()");
        }

        public override void Down()
        {
            Execute.Sql(AlterHydrantsDueInspectionViewToExcludeHydrantsNeverPreviouslyInspectedForMC78
                       .Sql.DROP_HYDRANT_VIEW);
            Execute.Sql(
                AddRequestRetirementHydrantsToRequiresInspectionViewForMC1248.Sql.CREATE_HYDRANT_VIEW_SQL_FORMAT,
                "GetDate()");
        }
    }
}
