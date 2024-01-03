using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200914113717674), Tags("Production")]
    public class MC2514ChangeConfinedSpaceToInt : Migration
    {
        public override void Up()
        {
            // Turns the string column into a bool. some values like 'yeah' and 'nope' will be ignored.
            Alter.Table("tblFacilities").AddColumn("HasConfinedSpaceRequirement").AsBoolean().Nullable();
            Execute.Sql(
                "UPDATE tblFacilities SET HasConfinedSpaceRequirement = 1 WHERE upper(Confined_Space_Requirement) like 'YES';");
            Execute.Sql(
                "UPDATE tblFacilities SET HasConfinedSpaceRequirement = 0 WHERE upper(Confined_Space_Requirement) NOT like 'YES';");
            Delete.Column("Confined_Space_Requirement").FromTable("tblFacilities");
        }

        public override void Down()
        {
            Alter.Table("tblFacilities").AddColumn("Confined_Space_Requirement").AsAnsiString(255).Nullable();
            Execute.Sql(
                "UPDATE tblFacilities SET Confined_Space_Requirement = 'Yes' WHERE HasConfinedSpaceRequirement = 1;");
            Delete.Column("HasConfinedSpaceRequirement").FromTable("tblFacilities");
        }
    }
}
