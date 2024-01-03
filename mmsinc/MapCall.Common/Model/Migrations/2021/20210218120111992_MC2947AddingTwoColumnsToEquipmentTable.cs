using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210218120111992), Tags("Production")]
    public class MC2947AddingTwoColumnsToEquipmentTable : Migration
    {
        public override void Up()
        {
            Delete.Column("HasEPARequirement").FromTable("Equipment");
            Alter.Table("Equipment")
                 .AddColumn("OtherCompliance").AsBoolean().WithDefaultValue(false).NotNullable()
                 .AddColumn("OtherComplianceReason").AsString(255).Nullable();
        }

        public override void Down()
        {
            Alter.Table("Equipment").AddColumn("HasEPARequirement").AsBoolean().WithDefaultValue(false).NotNullable();
            Delete.Column("OtherCompliance").FromTable("Equipment");
            Delete.Column("OtherComplianceReason").FromTable("Equipment");
        }
    }
}

