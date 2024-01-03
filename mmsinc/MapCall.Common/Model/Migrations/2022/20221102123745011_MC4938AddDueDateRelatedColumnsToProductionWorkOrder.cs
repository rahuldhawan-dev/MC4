using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221102123745011), Tags("Production")]
    public class Mc4938AddDueDateRelatedColumnsToProductionWorkOrder :
        Migration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrders")
                 .AddColumn("StartDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("StartDate").FromTable("ProductionWorkOrders");
        }
    }
}

