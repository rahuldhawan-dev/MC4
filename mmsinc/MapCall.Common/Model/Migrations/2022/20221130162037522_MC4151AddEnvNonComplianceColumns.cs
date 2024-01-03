using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221130162037522), Tags("Production")]
    public class MC4151AddEnvNonComplianceColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("EnvironmentalNonComplianceEvents").AddColumn("NOVWorkGroupReviewDate").AsDateTime().Nullable();
            Alter.Table("EnvironmentalNonComplianceEvents").AddColumn("ChiefEnvOfficerApprovalDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("NOVWorkGroupReviewDate").FromTable("EnvironmentalNonComplianceEvents");
            Delete.Column("ChiefEnvOfficerApprovalDate").FromTable("EnvironmentalNonComplianceEvents");
        }
    }
}