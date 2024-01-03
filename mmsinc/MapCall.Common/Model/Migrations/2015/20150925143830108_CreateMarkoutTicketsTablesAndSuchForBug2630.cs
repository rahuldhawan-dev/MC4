using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150925143830108), Tags("Production")]
    public class CreateMarkoutTicketsTablesAndSuchForBug2630 : Migration
    {
        public struct TableNames
        {
            public const string MESSAGE_TYPES = "OneCallMarkoutMessageTypes",
                                MARKOUT_TICKETS = "OneCallMarkoutTickets",
                                AUDITS = "OneCallMarkoutAudits",
                                AUDIT_TICKET_NUMBERS = "OneCallMarkoutAuditTicketNumbers";
        }

        public override void Up()
        {
            Create.Table(TableNames.MESSAGE_TYPES)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(20).NotNullable();

            Execute.Sql("INSERT INTO [{0}] (Description) VALUES ('Routine')", TableNames.MESSAGE_TYPES);
            Execute.Sql("INSERT INTO [{0}] (Description) VALUES ('Emergency')", TableNames.MESSAGE_TYPES);
            Execute.Sql("INSERT INTO [{0}] (Description) VALUES ('Broadcast')", TableNames.MESSAGE_TYPES);
            Execute.Sql("INSERT INTO [{0}] (Description) VALUES ('Update')", TableNames.MESSAGE_TYPES);
            Execute.Sql("INSERT INTO [{0}] (Description) VALUES ('Cancelled')", TableNames.MESSAGE_TYPES);

            Create.Table(TableNames.MARKOUT_TICKETS)
                  .WithIdentityColumn()
                  .WithColumn("RequestNumber").AsInt32().NotNullable().Unique()
                  .WithColumn("RelatedRequestNumber").AsInt32().Nullable()
                  .WithColumn("DateTransmitted").AsDateTime().NotNullable()
                  .WithColumn("DateReceived").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("MessageTypeId", TableNames.MESSAGE_TYPES, nullable: false)
                  .WithForeignKeyColumn("TownId", "Towns", "TownId")
                  .WithColumn("Town").AsString(50).Nullable()
                  .WithForeignKeyColumn("StreetId", "Streets", "StreetId")
                  .WithColumn("Street").AsString(50).Nullable()
                  .WithForeignKeyColumn("NearestCrossStreetId", "Streets", "StreetId")
                  .WithColumn("NearestCrossStreet").AsString(50).Nullable()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId", false)
                  .WithColumn("FullText").AsText().NotNullable();

            Create.Table(TableNames.AUDITS)
                  .WithIdentityColumn()
                  .WithColumn("DateTransmitted").AsDateTime().NotNullable()
                  .WithColumn("DateReceived").AsDateTime().NotNullable()
                  .WithColumn("Success").AsBoolean().NotNullable()
                  .WithColumn("FullText").AsText().NotNullable();

            Create.Table(TableNames.AUDIT_TICKET_NUMBERS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("AuditId", TableNames.AUDITS, nullable: false)
                  .WithColumn("RequestNumber").AsInt32().NotNullable().Unique()
                  .WithForeignKeyColumn("MessageTypeId", TableNames.MESSAGE_TYPES, nullable: false);
        }

        public override void Down()
        {
            Delete.Table(TableNames.AUDIT_TICKET_NUMBERS);
            Delete.Table(TableNames.AUDITS);
            Delete.Table(TableNames.MARKOUT_TICKETS);
            Delete.Table(TableNames.MESSAGE_TYPES);
        }
    }
}
