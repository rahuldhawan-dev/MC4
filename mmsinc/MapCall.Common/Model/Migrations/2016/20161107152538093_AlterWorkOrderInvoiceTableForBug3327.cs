using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161107152538093), Tags("Production")]
    public class AlterWorkOrderInvoiceTableForBug3327 : Migration
    {
        public override void Up()
        {
            Alter.Column("InvoiceNotes").OnTable("WorkOrderInvoices").AsString(int.MaxValue).Nullable();
        }

        public override void Down() { }
    }
}
