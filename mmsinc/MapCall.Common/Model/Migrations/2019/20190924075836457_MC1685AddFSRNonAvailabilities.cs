using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190924075836457), Tags("Production")]
    public class MC1685AddFSRNonAvailabilities : Migration
    {
        public override void Up()
        {
            Create.Table("FieldServiceRepresentativeNonAvailabilities")
                  .WithIdentityColumn()
                  .WithColumn("EngineerId").AsAnsiString(64).Nullable()
                  .WithColumn("NonAvailabilityType").AsAnsiString(64).Nullable()
                  .WithColumn("Comment").AsCustom("text").Nullable()
                  .WithColumn("Start").AsDateTime().NotNullable()
                  .WithColumn("Finish").AsDateTime().NotNullable()
                  .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
                  .WithColumn("SAPCommunicationError").AsBoolean().WithDefaultValue(false)
                  .WithColumn("ReceivedAt").AsDateTime().NotNullable();
            // What's up with this table name using an abbreviation?
            // The error was The identifier that starts with
            // 'FK_FieldServiceRepresentativeNonAvailabilitiesEngineers_FieldServiceRepresentativeNonAvailabilities_FieldServiceRepresentativeNo'
            // is too long. Maximum length is 128
            Create.Table("FSRNonAvailabilitiesEngineers")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("FieldServiceRepresentativeNonAvailabilityId",
                       "FieldServiceRepresentativeNonAvailabilities", nullable: false)
                  .WithColumn("EngineerId").AsAnsiString(64).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("FSRNonAvailabilitiesEngineers");
            Delete.Table("FieldServiceRepresentativeNonAvailabilities");
        }
    }
}
