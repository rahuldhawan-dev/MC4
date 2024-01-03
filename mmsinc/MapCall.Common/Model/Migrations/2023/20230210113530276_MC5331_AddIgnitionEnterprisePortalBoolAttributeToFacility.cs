using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230210113530276), Tags("Production")]
    public class MC5331_AddIgnitionEnterprisePortalBoolAttributeToFacility : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("IgnitionEnterprisePortal").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("IgnitionEnterprisePortal").FromTable("tblFacilities");
        }
    }
}

