using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220814194140322), Tags("Production")]
    public class MC4037AddHardCodedComplianceRequirementsToMaintenancePlan : Migration
    {
        public override void Up()
        {
            Alter.Table("MaintenancePlans")
                 .AddColumn("HasCompanyRequirement").AsBoolean().NotNullable()
                 .AddColumn("HasOshaRequirement").AsBoolean().NotNullable()
                 .AddColumn("HasPsmRequirement").AsBoolean().NotNullable()
                 .AddColumn("HasRegulatoryRequirement").AsBoolean().NotNullable()
                 .AddColumn("HasOtherCompliance").AsBoolean().NotNullable()
                 .AddColumn("OtherComplianceReason").AsAnsiString(255).Nullable();
            Delete.Table("ComplianceRequirementsMaintenancePlans");
        }

        public override void Down()
        {
            Delete.Column("HasCompanyRequirement")
                  .Column("HasOshaRequirement")
                  .Column("HasPsmRequirement")
                  .Column("HasRegulatoryRequirement")
                  .Column("HasOtherCompliance")
                  .Column("OtherComplianceReason")
                  .FromTable("MaintenancePlans");
            Create.Table("ComplianceRequirementsMaintenancePlans")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ComplianceRequirementId", "ComplianceRequirements")
                  .WithForeignKeyColumn("MaintenancePlanId", "MaintenancePlans");
        }
    }
}