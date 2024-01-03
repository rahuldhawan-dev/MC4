using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140429140600), Tags("Production")]
    public class AddPurchaseOrderColumnsForServicesAndRestorationsBug1875 : Migration
    {
        public struct TableNames
        {
            public const string SERVICES = "tblNJAWService",
                                RESTORATIONS = "tblNJAWRestore";
        }

        public struct ColumnNames
        {
            public const string PURCHASE_ORDER_NUMBER = "PurchaseOrderNumber",
                                ESTIMATED_VALUE = "EstimatedValue";
        }

        public struct StringLengths
        {
            public const int PURCHASE_ORDER_NUMBER = 20;
        }

        public override void Up()
        {
            Alter.Table(TableNames.SERVICES)
                 .AddColumn(ColumnNames.PURCHASE_ORDER_NUMBER).AsAnsiString(StringLengths.PURCHASE_ORDER_NUMBER)
                 .Nullable();
            Alter.Table(TableNames.RESTORATIONS)
                 .AddColumn(ColumnNames.PURCHASE_ORDER_NUMBER).AsAnsiString(StringLengths.PURCHASE_ORDER_NUMBER)
                 .Nullable()
                 .AddColumn(ColumnNames.ESTIMATED_VALUE).AsCurrency().Nullable();
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.PURCHASE_ORDER_NUMBER).FromTable(TableNames.SERVICES);
            Delete.Column(ColumnNames.PURCHASE_ORDER_NUMBER).FromTable(TableNames.RESTORATIONS);
            Delete.Column(ColumnNames.ESTIMATED_VALUE).FromTable(TableNames.RESTORATIONS);
        }
    }
}
