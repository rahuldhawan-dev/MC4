using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181024134359842), Tags("Production")]
    public class FixExistingValuesForMC686 : Migration
    {
        public override void Up()
        {
            Execute.Sql("update RecurringProjects set TotalInfoMasterScore = '1' WHERE TotalInfoMasterScore = 'A';");
            Execute.Sql(
                "update RecurringProjects set TotalInfoMasterScore = '2' WHERE TotalInfoMasterScore in ('A-', 'B'); ");
            Execute.Sql(
                "update RecurringProjects set TotalInfoMasterScore = '3' WHERE TotalInfoMasterScore in ('B-', 'C'); ");
            Execute.Sql(
                "update RecurringProjects set TotalInfoMasterScore = '4' WHERE TotalInfoMasterScore in ('C-', 'D'); ");
            Execute.Sql(
                "update RecurringProjects set TotalInfoMasterScore = '5' WHERE TotalInfoMasterScore in ('D-', 'F'); ");
            Alter.Table("RecurringProjects").AlterColumn("TotalInfoMasterScore").AsAnsiString(10).Nullable();
        }

        public override void Down()
        {
            Execute.Sql("update RecurringProjects set TotalInfoMasterScore = 'A' WHERE TotalInfoMasterScore = '1';");
            Execute.Sql("update RecurringProjects set TotalInfoMasterScore = 'B' WHERE TotalInfoMasterScore = '2'; ");
            Execute.Sql("update RecurringProjects set TotalInfoMasterScore = 'C' WHERE TotalInfoMasterScore = '3'; ");
            Execute.Sql("update RecurringProjects set TotalInfoMasterScore = 'D' WHERE TotalInfoMasterScore = '4'; ");
            Execute.Sql("update RecurringProjects set TotalInfoMasterScore = 'F' WHERE TotalInfoMasterScore = '5'; ");
            Alter.Table("RecurringProjects").AlterColumn("TotalInfoMasterScore").AsAnsiString(2).Nullable();
        }
    }
}
