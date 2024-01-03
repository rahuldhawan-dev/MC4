using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170816112823981), Tags("Production")]
    public class AddStateToRegionCodesForPremiseImportBug4017 : Migration
    {
        public override void Up()
        {
            Alter.Table("RegionCodes").AddForeignKeyColumn("StateId", "States", "StateID");
            Execute.Sql(
                "UPDATE RegionCodes SET StateID = (SELECT TOP 1 StateID from Premises P join OperatingCenters OC on OC.OperatingCenterID = P.OperatingCenterID where P.RegionCodeID = RegionCodes.Id);" +
                "UPDATE RegionCodes SET StateID = (SELECT StateID FROM States WHERE Abbreviation = 'PA') WHERE Description = 'HERSHEY' AND SAPCode = 'HRSH';" +
                "INSERT INTO RegionCodes SELECT 'City Of Alton', '0101', StateID FROM States where Abbreviation = 'IL';" +
                "UPDATE Premises Set RegionCodeId = (Select Id from RegionCodes where StateId = (SELECT StateID FROM States where Abbreviation = 'IL') and SAPCode = '0101') WHERE RegionCodeId = 39 AND OperatingCenterID = 21;");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("RegionCodes", "StateId", "States", "StateID");
        }
    }
}
