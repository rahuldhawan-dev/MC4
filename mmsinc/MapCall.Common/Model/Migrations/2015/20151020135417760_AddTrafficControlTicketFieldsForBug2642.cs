using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151020135417760), Tags("Production")]
    public class AddTrafficControlTicketFieldsForBug2642 : Migration
    {
        public struct TableNames
        {
            public const string TICKETS = "TrafficControlTickets",
                                USERS = "tblPermissions",
                                CHECKS = "TrafficControlTicketChecks",
                                BILLING_PARTIES = "BillingParties",
                                STATUSES = "TrafficControlTicketStatuses";
        }

        public override void Up()
        {
            Alter.Table(TableNames.TICKETS)
                 .AddColumn("PaymentReceivedAt").AsDateTime().Nullable()
                 .AddColumn("TotalCharged").AsDecimal(8, 2).Nullable()
                 .AddColumn("PaymentTransactionId").AsAnsiString(255).Nullable()
                 .AddColumn("PaymentAuthorizationCode").AsAnsiString(255).Nullable()
                 .AddColumn("PaymentProfileId").AsAnsiString(255).Nullable()
                 .AddColumn("ProcessingFee").AsDecimal(5, 2).Nullable()
                 .AddColumn("TrackingNumber").AsAnsiString(22).Nullable()
                 .AddColumn("SubmittedAt").AsDateTime().Nullable()
                 .AddColumn("CanceledAt").AsDateTime().Nullable();
            Alter.Table(TableNames.TICKETS)
                 .AddForeignKeyColumn("SubmittedBy", TableNames.USERS, "RecId");
            Alter.Table(TableNames.TICKETS)
                 .AddForeignKeyColumn("CanceledBy", TableNames.USERS, "RecId");

            Create.Table(TableNames.CHECKS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("TrafficControlTicketId", TableNames.TICKETS)
                  .WithColumn("Amount").AsDecimal(8, 2).NotNullable()
                  .WithColumn("CheckNumber").AsInt32().NotNullable()
                  .WithColumn("Memo").AsAnsiString(255).Nullable()
                  .WithColumn("Reconciled").AsBoolean().Nullable();

            this.CreateLookupTableWithValues(TableNames.STATUSES, "Open", "Awaiting Payment", "Pending Submittal",
                "Submitted", "Canceled", "Cleared");

            Execute.Sql("INSERT INTO DocumentType Values('Invoice', 174);");

            Alter.Table(TableNames.BILLING_PARTIES)
                 .AddColumn("Payee").AsAnsiString(255).Nullable();

            Execute.Sql(
                "INSERT INTO NotificationPurposes SELECT (select ModuleID from Modules where name = 'Work Management' and ApplicationID = 1), 'Traffic Control Payment Submitted'");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM NotificationPurposes Where ModuleID = (select ModuleID from Modules where name = 'Work Management' and ApplicationID = 1) and Purpose = 'Traffic Control Payment Submitted'");
            Delete.Column("Payee").FromTable(TableNames.BILLING_PARTIES);
            Execute.Sql("delete from [DocumentLink] WHERE DataTypeID = 174");
            Execute.Sql(
                "delete from [Document] WHERE DocumentTypeId IN (SELECT DocumentTypeID FROM [DocumentType] WHERE DataTypeID = 174)");
            Execute.Sql("delete from DocumentType where Document_Type = 'Invoice' and DataTypeID = 174;");

            Delete.Table(TableNames.CHECKS);
            Delete.Table(TableNames.STATUSES);

            Delete.Column("PaymentReceivedAt").FromTable(TableNames.TICKETS);
            Delete.Column("TotalCharged").FromTable(TableNames.TICKETS);
            Delete.Column("PaymentTransactionId").FromTable(TableNames.TICKETS);
            Delete.Column("PaymentAuthorizationCode").FromTable(TableNames.TICKETS);
            Delete.Column("PaymentProfileId").FromTable(TableNames.TICKETS);
            Delete.Column("ProcessingFee").FromTable(TableNames.TICKETS);
            Delete.Column("TrackingNumber").FromTable(TableNames.TICKETS);
            Delete.Column("SubmittedAt").FromTable(TableNames.TICKETS);
            Delete.Column("CanceledAt").FromTable(TableNames.TICKETS);
            Delete.ForeignKeyColumn(TableNames.TICKETS, "SubmittedBy", TableNames.USERS, "RecId");
            Delete.ForeignKeyColumn(TableNames.TICKETS, "CanceledBy", TableNames.USERS, "RecId");
        }
    }
}
