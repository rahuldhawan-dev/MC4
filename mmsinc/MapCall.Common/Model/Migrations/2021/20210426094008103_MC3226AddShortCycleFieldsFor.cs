using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210426094008103), Tags("Production")]
    public class MC3226AddShortCycleFieldsFor : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("ShortCycleSecureAccessTypes", 100,
                "Gated Community",
                "Locked Entry, Key Needed for Access",
                "Locked Entry, Customer Needed for Access",
                "Special Security Access Needed",
                "Secured Meter, Customer Does Not Need to Be Present");

            Alter.Table("ShortCycleWorkOrderCompletions")
                 .AddForeignKeyColumn("SecureAccessTypeId", "ShortCycleSecureAccessTypes").Nullable();

            Alter.Table("ShortCycleWorkOrderCompletionTestResults")
                 .AddColumn("Result").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Result").FromTable("ShortCycleWorkOrderCompletionTestResults");

            Delete.ForeignKeyColumn("ShortCycleWorkOrderCompletions", "SecureAccessTypeId", "ShortCycleSecureAccessTypes");

            Delete.Table("ShortCycleSecureAccessTypes");
        }
    }
}
