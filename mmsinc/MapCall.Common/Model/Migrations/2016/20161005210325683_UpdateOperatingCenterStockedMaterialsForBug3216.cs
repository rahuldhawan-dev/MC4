using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161005210325683), Tags("Production")]
    public class UpdateOperatingCenterStockedMaterialsForBug3216 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "DELETE OCSM FROM OperatingCenterStockedMaterials OCSM JOIN Materials M on M.MaterialID = OCSM.MaterialID WHERE M.IsActive = 0");
        }

        public override void Down() { }
    }
}
