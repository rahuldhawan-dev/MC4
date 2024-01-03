using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160125151156173), Tags("Production")]
    public class MoveTownAbbreviationToTownOperatingCentersForBug2772 : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCentersTowns").AddColumn("Abbreviation").AsAnsiString(4).Nullable();
            Execute.Sql(
                "UPDATE OperatingCentersTowns SET Abbreviation = (SELECT AB from Towns where Towns.TownID = OperatingCentersTowns.TownID);" +
                "UPDATE OperatingCentersTowns SET Abbreviation = 'EDS' WHERE TownID = 227 and OperatingCenterID = 19");
            Alter.Table("OperatingCentersTowns").AlterColumn("Abbreviation").AsAnsiString(4).NotNullable();
            Delete.Column("AB").FromTable("Towns");
        }

        public override void Down()
        {
            Alter.Table("Towns").AddColumn("AB").AsAnsiString(4).Nullable();
            Execute.Sql(
                "UPDATE Towns SET AB = (SELECT TOP 1 Abbreviation from OperatingCentersTowns where OperatingCentersTowns.TownID = Towns.TownID)");
            Delete.Column("Abbreviation").FromTable("OperatingCentersTowns");
        }
    }
}
