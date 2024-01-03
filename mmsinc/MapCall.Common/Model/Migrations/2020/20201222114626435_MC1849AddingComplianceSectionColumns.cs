using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201222114626435), Tags("Production")]
    public class MC1849AddingComplianceSectionColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment")
                 .AddColumn("HasProcessSafetyManagement").AsBoolean().WithDefaultValue(false).NotNullable()
                 .AddColumn("HasCompanyRequirement").AsBoolean().WithDefaultValue(false).NotNullable()
                 .AddColumn("HasRegulatoryRequirement").AsBoolean().WithDefaultValue(false).NotNullable()
                 .AddColumn("HasOshaRequirement").AsBoolean().WithDefaultValue(false).NotNullable()
                 .AddColumn("HasEPARequirement").AsBoolean().WithDefaultValue(false).NotNullable();
        }

        public override void Down()
        {
            Delete
               .Column("HasProcessSafetyManagement")
               .Column("HasCompanyRequirement")
               .Column("HasRegulatoryRequirement")
               .Column("HasOshaRequirement")
               .Column("HasEPARequirement")
               .FromTable("Equipment");
        }
    }
}

