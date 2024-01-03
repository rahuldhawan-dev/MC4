using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201106135842351), Tags("Production")]
    public class MC2578ConfinedSpaceFormTDWorkOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("ConfinedSpaceForms").AddForeignKeyColumn("WorkOrderId", "WorkOrders", "WorkOrderID")
                 .Nullable();
            Alter.Table("WorkOrders").AddColumn("IsConfinedSpaceFormRequired").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("IsConfinedSpaceFormRequired").FromTable("WorkOrders");
            Delete.ForeignKeyColumn("ConfinedSpaceForms", "WorkOrderId", "WorkOrders", "WorkOrderId");
        }
    }
}
