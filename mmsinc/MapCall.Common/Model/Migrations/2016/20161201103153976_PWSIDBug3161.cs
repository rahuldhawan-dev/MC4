using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161201103153976), Tags("Production")]
    public class PWSIDBug3161 : Migration
    {
        public override void Up()
        {
            Alter.Table("PublicWaterSupplies")
                 .AddColumn("ExecutiveDirectorEmployeeId").AsInt32().Nullable()
                 .ForeignKey("FK_PublicWaterSupplies_tblEmployee_ExecutiveDirectoryEmployeeId", "tblEmployee",
                      "tblEmployeeID");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_PublicWaterSupplies_tblEmployee_ExecutiveDirectoryEmployeeId")
                  .OnTable("PublicWaterSupplies");

            Delete.Column("ExecutiveDirectorEmployeeId").FromTable("PublicWaterSupplies");
        }
    }
}
