using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210818140821037), Tags("Production")]
    public class MC3292AddingLaborRelationsBusinessPartnerToGrievances : Migration
    {
        public override void Up()
        {
            Alter.Table("UnionGrievances")
                 .AddForeignKeyColumn("LaborRelationsBusinessPartnerEmployeeId", "tblEmployee", "tblEmployeeID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("UnionGrievances", "LaborRelationsBusinessPartnerEmployeeId", "tblEmployee");
        }
    }
}

