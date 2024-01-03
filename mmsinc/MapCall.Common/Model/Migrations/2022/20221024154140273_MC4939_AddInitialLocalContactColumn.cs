using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221024154140273), Tags("Production")]
    public class MC4939AddInitialLocalContactColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("WaterQualityComplaints")
                 .AddForeignKeyColumn("InitialLocalContactId", "tblEmployee", "tblEmployeeID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WaterQualityComplaints", "InitialLocalContactId", "tblEmployee", "tblEmployeeID");
        }
    }
}
