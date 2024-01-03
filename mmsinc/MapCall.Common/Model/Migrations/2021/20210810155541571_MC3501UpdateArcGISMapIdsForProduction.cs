using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210810155541571), Tags("Production")]
    public class MC3501UpdateArcGISMapIdsForProduction : Migration
    {
        public override void Up()
        {
            Execute.Sql("UPDATE OperatingCenters SET MapId = '590efe21369e442192bd6e7fd2c806c3' WHERE MapId = 'e8ae5ea9dc304901b35f07d12854a6ba';");
        }

        public override void Down()
        {
            Execute.Sql("UPDATE OperatingCenters SET MapId = 'e8ae5ea9dc304901b35f07d12854a6ba' WHERE MapId = '590efe21369e442192bd6e7fd2c806c3'");
        }
    }
}

