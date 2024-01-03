using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190710132839920), Tags("Production")]
    public class MC1155AddColumnsForEnviromentPermitRequirements : Migration
    {
        public override void Up()
        {
            // lookup table for communication types - replace ReportSendDetails column
            // letter, electronic, upload, email, pdf, agency submittal form, other.
            this.CreateLookupTableWithValues("CommunicationTypes", "Letter", "Electronic", "Upload", "Email", "PDF",
                "Agency Submittal Form", "Other");
            Alter.Table("EnvironmentalPermitRequirements")
                 .AddForeignKeyColumn("CommunicationTypeId", "CommunicationTypes")
                 .AddColumn("CommunicationEmail").AsAnsiString(254).Nullable()
                 .AddColumn("CommunicationLink").AsAnsiString(1024).Nullable();
            Delete.Column("ReportSendDetails").FromTable("EnvironmentalPermitRequirements");
        }

        public override void Down()
        {
            Delete.Column("CommunicationEmail").FromTable("EnvironmentalPermitRequirements");
            Delete.Column("CommunicationLink").FromTable("EnvironmentalPermitRequirements");
            Delete.ForeignKeyColumn("EnvironmentalPermitRequirements", "CommunicationTypeId", "CommunicationTypes");
            Delete.Table("CommunicationTypes");
            Alter.Table("EnvironmentalPermitRequirements").AddColumn("ReportSendDetails").AsCustom("text").Nullable();
        }
    }
}
