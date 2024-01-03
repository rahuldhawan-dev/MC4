using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160810095802313), Tags("Production")]
    public class AddColumnsToServicesForBug3096 : Migration
    {
        public struct TableNames
        {
            public const string OFFER_STATUS =
                                    "CustomerSideSLReplacementOfferStatuses",
                                FLUSHING_INSTRUCTIONS = "FlushingOfCustomerPlumbingInstructions",
                                REPLACED_BY = "CustomerSideSLReplacers",
                                WBS_NUMBERS = "WBSNumbers",
                                FLUSHING_NOTICE_TYPES = "WorkOrderFlushingNoticeTypes";
        }

        public override void Up()
        {
            Create.LookupTable(TableNames.OFFER_STATUS, 20);

            Insert.IntoTable(TableNames.OFFER_STATUS).Row(new {Description = "No"});
            Insert.IntoTable(TableNames.OFFER_STATUS).Row(new {Description = "Offered-Accepted"});
            Insert.IntoTable(TableNames.OFFER_STATUS).Row(new {Description = "Offered-Rejected"});

            Alter.Table("Services")
                 .AddForeignKeyColumn("CustomerSideSLReplacementId", TableNames.OFFER_STATUS);

            Create.LookupTable(TableNames.FLUSHING_INSTRUCTIONS, 30);

            Insert.IntoTable(TableNames.FLUSHING_INSTRUCTIONS)
                  .Row(new {Description = "Standard Flushing Instructions"});
            Insert.IntoTable(TableNames.FLUSHING_INSTRUCTIONS)
                  .Row(new {Description = "Extended Flushing Instructions"});

            Alter.Table("Services")
                 .AddForeignKeyColumn("FlushingOfCustomerPlumbingId", TableNames.FLUSHING_INSTRUCTIONS);

            Create.LookupTable(TableNames.REPLACED_BY, 20);

            Insert.IntoTable(TableNames.REPLACED_BY).Row(new {Description = "Company Forces"});
            Insert.IntoTable(TableNames.REPLACED_BY).Row(new {Description = "Contractor"});

            Alter.Table("Services")
                 .AddForeignKeyColumn("CustomerSideSLReplacedById", TableNames.REPLACED_BY);

            Alter.Table("Services")
                 .AddForeignKeyColumn("CustomerSideSLReplacementContractorId", "Contractors",
                      "ContractorID");

            Alter.Table("Services")
                 .AddColumn("LengthOfCustomerSideSLReplaced").AsInt32().Nullable()
                 .AddColumn("CustomerSideSLReplacementCost").AsCurrency().Nullable()
                 .AddColumn("CustomerSideReplacementDate").AsDate().Nullable()
                 .AddColumn("DateCreditProcessed").AsDate().Nullable();

            Create.Table(TableNames.WBS_NUMBERS)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(11).NotNullable();

            Insert.IntoTable(TableNames.WBS_NUMBERS).Row(new {Description = "B18-02-0001"});
            Insert.IntoTable(TableNames.WBS_NUMBERS).Row(new {Description = "B18-03-0001"});
            Insert.IntoTable(TableNames.WBS_NUMBERS).Row(new {Description = "B18-05-0001"});
            Insert.IntoTable(TableNames.WBS_NUMBERS).Row(new {Description = "B18-06-0001"});

            Alter.Table("OperatingCenters")
                 .AddForeignKeyColumn("DefaultServiceReplacementWBSNumberId", TableNames.WBS_NUMBERS);

            Execute.Sql(
                $"UPDATE {"OperatingCenters"} SET DefaultServiceReplacementWBSNumberId = 1 WHERE OperatingCenterCode = 'NJ3'");
            Execute.Sql(
                $"UPDATE {"OperatingCenters"} SET DefaultServiceReplacementWBSNumberId = 1 WHERE OperatingCenterCode = 'NJ4'");
            Execute.Sql(
                $"UPDATE {"OperatingCenters"} SET DefaultServiceReplacementWBSNumberId = 1 WHERE OperatingCenterCode = 'NJ7'");
            Execute.Sql(
                $"UPDATE {"OperatingCenters"} SET DefaultServiceReplacementWBSNumberId = 2 WHERE OperatingCenterCode = 'NJ6'");
            Execute.Sql(
                $"UPDATE {"OperatingCenters"} SET DefaultServiceReplacementWBSNumberId = 2 WHERE OperatingCenterCode = 'NJ8'");
            Execute.Sql(
                $"UPDATE {"OperatingCenters"} SET DefaultServiceReplacementWBSNumberId = 3 WHERE OperatingCenterCode = 'EW1'");
            Execute.Sql(
                $"UPDATE {"OperatingCenters"} SET DefaultServiceReplacementWBSNumberId = 3 WHERE OperatingCenterCode = 'EW2'");
            Execute.Sql(
                $"UPDATE {"OperatingCenters"} SET DefaultServiceReplacementWBSNumberId = 4 WHERE OperatingCenterCode = 'NJ5'");

            Alter.Table("Services")
                 .AddForeignKeyColumn("CustomerSideReplacementWBSNumberId", TableNames.WBS_NUMBERS);

            Create.Table(TableNames.FLUSHING_NOTICE_TYPES)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(30).NotNullable();

            Insert.IntoTable(TableNames.FLUSHING_NOTICE_TYPES).Row(new {Description = "Standard Flushing Notice left"});
            Insert.IntoTable(TableNames.FLUSHING_NOTICE_TYPES).Row(new {Description = "Extended Flushing Notice left"});

            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("FlushingNoticeTypeId",
                      TableNames.FLUSHING_NOTICE_TYPES);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WorkOrders",
                "FlushingNoticeTypeId", TableNames.FLUSHING_NOTICE_TYPES);

            Delete.Table(TableNames.FLUSHING_NOTICE_TYPES);

            Delete.ForeignKeyColumn("Services", "CustomerSideReplacementWBSNumberId",
                TableNames.WBS_NUMBERS);

            Delete.ForeignKeyColumn("OperatingCenters", "DefaultServiceReplacementWBSNumberId",
                TableNames.WBS_NUMBERS);

            Delete.Table(TableNames.WBS_NUMBERS);

            Delete.Column("LengthOfCustomerSideSLReplaced").FromTable("Services");
            Delete.Column("CustomerSideSLReplacementCost").FromTable("Services");
            Delete.Column("CustomerSideReplacementDate").FromTable("Services");
            Delete.Column("DateCreditProcessed").FromTable("Services");

            Delete.ForeignKeyColumn("Services", "CustomerSideSLReplacementContractorId",
                "Contractors", "ContractorId");

            Delete.ForeignKeyColumn("Services", "CustomerSideSLReplacementId", TableNames.OFFER_STATUS);
            Delete.Table(TableNames.OFFER_STATUS);

            Delete.ForeignKeyColumn("Services", "FlushingOfCustomerPlumbingId",
                TableNames.FLUSHING_INSTRUCTIONS);
            Delete.Table(TableNames.FLUSHING_INSTRUCTIONS);

            Delete.ForeignKeyColumn("Services", "CustomerSideSLReplacedById", TableNames.REPLACED_BY);
            Delete.Table(TableNames.REPLACED_BY);
        }
    }
}
