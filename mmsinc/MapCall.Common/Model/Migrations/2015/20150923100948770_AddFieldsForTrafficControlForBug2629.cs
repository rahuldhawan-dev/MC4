using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150923100948770), Tags("Production")]
    public class AddFieldsForTrafficControlForBug2629 : Migration
    {
        public struct TableNames
        {
            public const string TRAFFIC_CONTROL_TICKETS = "TrafficControlTickets",
                                BILLING_PARTIES = "BillingParties";
        }

        public override void Up()
        {
            Rename.Column("Date").OnTable(TableNames.TRAFFIC_CONTROL_TICKETS).To("WorkStartDate");

            Alter.Table(TableNames.TRAFFIC_CONTROL_TICKETS)
                 .AddColumn("WorkEndDate").AsDateTime().Nullable()
                 .AddColumn("InvoiceTotalHours").AsDecimal(5, 2).Nullable()
                 .AddColumn("Notes").AsCustom("text").Nullable();
            Alter.Table(TableNames.TRAFFIC_CONTROL_TICKETS)
                 .AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID");
            Alter.Table(TableNames.TRAFFIC_CONTROL_TICKETS)
                 .AddForeignKeyColumn("CrossStreetId", "Streets", "StreetID");

            Alter.Table(TableNames.BILLING_PARTIES).AddColumn("EstimatedHourlyRate").AsDecimal(6, 2).Nullable();

            Execute.Sql(
                "INSERT INTO NotificationPurposes SELECT (select ModuleID from Modules where name = 'Work Management' and ApplicationID = 1), 'Traffic Control Ticket Invoice Entered'");

            Create.Table("BillingPartyContactTypes")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ContactTypeId", "ContactTypes", "ContactTypeID");
            Create.Table("BillingPartiesContacts")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ContactId", "Contacts", "ContactId")
                  .WithForeignKeyColumn("BillingPartyId", "BillingParties")
                  .WithForeignKeyColumn("ContactTypeId", "ContactTypes", "ContactTypeID");

            Execute.Sql(@"SET IDENTITY_INSERT ContactTypes ON
                    INSERT INTO ContactTypes(ContactTypeID, Name) Values(9, 'Traffic Control')
                    SET IDENTITY_INSERT ContactTypes OFF;
                    INSERT INTO BillingPartyContactTypes Values(9);
                    ");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM BillingPartiesContacts");
            Execute.Sql("DELETE FROM BillingPartyContactTypes;");
            Execute.Sql("DELETE FROM ContactTypes WHERE ContactTypeID = 9;");

            Delete.Table("BillingPartiesContacts");
            Delete.Table("BillingPartyContactTypes");

            Execute.Sql(
                "DELETE FROM NotificationPurposes Where ModuleID = (select ModuleID from Modules where name = 'Work Management' and ApplicationID = 1) and Purpose = 'Traffic Control Ticket Invoice Entered'");
            Delete.Column("EstimatedHourlyRate").FromTable(TableNames.BILLING_PARTIES);

            Delete.ForeignKeyColumn(TableNames.TRAFFIC_CONTROL_TICKETS, "CrossStreetId", "Streets", "StreetID");
            Delete.ForeignKeyColumn(TableNames.TRAFFIC_CONTROL_TICKETS, "CoordinateId", "Coordinates", "CoordinateID");
            Delete.Column("Notes").FromTable(TableNames.TRAFFIC_CONTROL_TICKETS);
            Delete.Column("InvoiceTotalHours").FromTable(TableNames.TRAFFIC_CONTROL_TICKETS);
            Delete.Column("WorkEndDate").FromTable(TableNames.TRAFFIC_CONTROL_TICKETS);

            Rename.Column("WorkStartDate").OnTable(TableNames.TRAFFIC_CONTROL_TICKETS).To("Date");
        }
    }
}
