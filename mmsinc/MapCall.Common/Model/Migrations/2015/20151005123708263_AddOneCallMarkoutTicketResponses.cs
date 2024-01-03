using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151005123708263), Tags("Production")]
    public class AddOneCallMarkoutTicketResponses : Migration
    {
        public struct TableNames
        {
            public const string
                RESPONSES = "OneCallMarkoutResponses",
                RESPONSE_STATUSES = "OneCallMarkoutResponseStatuses",
                RESPONSE_TECHNIQUES = "OneCallMarkoutResponseTechniques",
                TICKETS = CreateMarkoutTicketsTablesAndSuchForBug2630.TableNames.MARKOUT_TICKETS;
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.RESPONSE_STATUSES, "M/O Performed", "No Facilities",
                "Cancelled", "Co. Not Required", "Unable to Locate", "Time Lapsed");
            this.CreateLookupTableWithValues(TableNames.RESPONSE_TECHNIQUES, "Center Line", "Outside Wall", "Offset",
                "All Types", "C/L & Outside Wall", "C/L & Offset", "Outside Wall & Offset", "Outside Wall & C/L",
                "Offset & C/L");

            Create.Table(TableNames.RESPONSES)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OneCallMarkoutTicketId", TableNames.TICKETS).NotNullable()
                  .WithForeignKeyColumn("CompletedById", "tblPermissions", "RecId").NotNullable()
                  .WithColumn("CompletedAt").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("OneCallMarkoutResponseStatusId", TableNames.RESPONSE_STATUSES).Nullable()
                  .WithColumn("Comments").AsCustom("text").Nullable()
                  .WithForeignKeyColumn("OneCallMarkoutResponseTechniqueId", TableNames.RESPONSE_TECHNIQUES).Nullable()
                  .WithColumn("ReqNotified").AsBoolean().Nullable()
                  .WithColumn("Paint").AsBoolean().Nullable()
                  .WithColumn("Flag").AsBoolean().Nullable()
                  .WithColumn("Stake").AsBoolean().Nullable()
                  .WithColumn("Over500Feet").AsBoolean().Nullable()
                  .WithColumn("CrewMarkoutIsNeeded").AsBoolean().Nullable()
                  .WithColumn("NumberOfCSMO").AsInt32().Nullable()
                  .WithColumn("NumberOfCSMOUnableToLocate").AsInt32().Nullable()
                  .WithColumn("TotalTimeSpentForCSMO").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableNames.RESPONSES);
            Delete.Table(TableNames.RESPONSE_STATUSES);
            Delete.Table(TableNames.RESPONSE_TECHNIQUES);
        }
    }
}
