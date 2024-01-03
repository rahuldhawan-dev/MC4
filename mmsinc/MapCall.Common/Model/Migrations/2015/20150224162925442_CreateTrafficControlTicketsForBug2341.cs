using FluentMigrator;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150224162925442), Tags("Production")]
    public class CreateTrafficControlTicketsForBug2341 : Migration
    {
        public struct TableNames
        {
            public const string TRAFFIC_CONTROL_TICKETS = "TrafficControlTickets",
                                BILLING_PARTIES = "BillingParties";
        }

        public struct StringLengths
        {
            public struct TrafficControlTickets
            {
                public const int ACCOUNTING_CODE = 30,
                                 INVOICE_NUMBER = 30;
            }

            public struct BillingParties
            {
                public const int DESCRIPTION = 50;
            }
        }

        public override void Up()
        {
            Create.Table(TableNames.BILLING_PARTIES)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(StringLengths.BillingParties.DESCRIPTION);

            Create.Table(TableNames.TRAFFIC_CONTROL_TICKETS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("WorkOrderId", "WorkOrders", "WorkOrderId")
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId", false)
                  .WithColumn("SAPWorkOrderNumber").AsInt64().NotNullable()
                  .WithColumn("StreetNumber").AsString(20).NotNullable()
                  .WithForeignKeyColumn("StreetId", "Streets", "StreetId", false)
                  .WithForeignKeyColumn("MunicipalityId", "Towns", "TownId", false)
                  .WithColumn("Date").AsDateTime().NotNullable()
                  .WithColumn("TotalHours").AsInt32().NotNullable()
                  .WithColumn("NumberOfOfficers").AsInt32().NotNullable()
                  .WithColumn("AccountingCode").AsString(StringLengths.TrafficControlTickets.ACCOUNTING_CODE).Nullable()
                  .WithColumn("InvoiceNumber").AsString(StringLengths.TrafficControlTickets.INVOICE_NUMBER).Nullable()
                  .WithColumn("InvoiceAmount").AsCurrency().Nullable()
                  .WithColumn("InvoiceDate").AsDateTime().Nullable()
                  .WithColumn("DateApproved").AsDateTime().Nullable()
                  .WithForeignKeyColumn("ApprovedById", "tblPermissions", "RecId")
                  .WithForeignKeyColumn("BillingPartyId", TableNames.BILLING_PARTIES);

            this.CreateDocumentType(TableNames.TRAFFIC_CONTROL_TICKETS, TableNames.TRAFFIC_CONTROL_TICKETS);

            Execute.Sql(@"INSERT INTO BillingParties (Description)

SELECT
	t.Town + ', ' + c.Name + ' County, ' + s.Abbreviation
FROM Towns t
INNER JOIN Counties c on t.CountyId = c.CountyID
INNER JOIN States s on t.StateId = s.StateId
WHERE t.CountyId IS NOT NULL

UNION

SELECT
	c.Name + ' County, ' + s.Abbreviation
FROM Counties c
INNER JOIN States s on c.stateID = s.StateId

UNION

SELECT
	s.Name
FROM States s;");
        }

        public override void Down()
        {
            Delete.Table(TableNames.TRAFFIC_CONTROL_TICKETS);

            this.DeleteDataType(TableNames.TRAFFIC_CONTROL_TICKETS);

            Delete.Table(TableNames.BILLING_PARTIES);
        }
    }
}
