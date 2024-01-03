using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210413160020001), Tags("Production")]
    public class MC3100AddWbsToServiceRecords : Migration
    {
        public const int STRING_LENGTH = 25; 

        public override void Up()
        {
            Alter.Table("Services")
                 .AddColumn("LeadServiceReplacementWbs").AsAnsiString(STRING_LENGTH).Nullable()
                 .AddColumn("LeadServiceRetirementWbs").AsAnsiString(STRING_LENGTH).Nullable();
        }

        public override void Down()
        {
            Delete.Column("LeadServiceReplacementWbs").FromTable("Services");
            Delete.Column("LeadServiceRetirementWbs").FromTable("Services");
        }
    }
}