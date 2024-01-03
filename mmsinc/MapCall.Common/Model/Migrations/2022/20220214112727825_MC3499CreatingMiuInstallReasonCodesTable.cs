using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220214112727825), Tags("Production")]
    public class MC3499CreatingMiuInstallReasonCodesTable : Migration
    {
        public override void Up()
        {
            this.Create.LookupTable("MIUInstallReasonCodes", 30)
                .WithColumn("SAPCode").AsAnsiString(2).Unique().NotNullable();
            Insert.IntoTable("MIUInstallReasonCodes").Rows(
                new { SAPCode = "33", Description = "New Install AMI" },
                new { SAPCode = "63", Description = @"New Install RF\AMR\Touchpad" }
            );
            Alter.Table("ServiceInstallations")
                 .AddForeignKeyColumn("MIUInstallReasonId", "MIUInstallReasonCodes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ServiceInstallations", "MIUInstallReasonId", "MIUInstallReasonCodes");
            Delete.Table("MIUInstallReasonCodes");
        }
    }
}

