using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200225095605285), Tags("Production")]
    public class MC833AddForeignKeyToLockoutFormsForProductionWorkOrder : Migration
    {
        public override void Up()
        {
            Alter.Table("LockoutForms").AddForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders").Nullable();
            Execute.Sql(
                "update LockoutForms SET ProductionWorkOrderId = (Select Id from ProductionWorkOrders where isnull(SAPWorkOrder,'') = isNull(SAPWorkOrderNumber,'')) WHERE IsNull(SAPWorkOrderNumber,'') <> ''");
            Delete.Column("SAPWorkOrderNumber").FromTable("LockoutForms");
            Alter.Table("ProductionWorkOrdersProductionPrerequisites").AddColumn("SkipRequirement").AsBoolean()
                 .NotNullable().WithDefaultValue(false)
                 .AddColumn("SkipRequirementComments").AsAnsiString(500).Nullable();
        }

        public override void Down()
        {
            Delete.Column("SkipRequirement").FromTable("ProductionWorkOrdersProductionPrerequisites");
            Delete.Column("SkipRequirementComments").FromTable("ProductionWorkOrdersProductionPrerequisites");
            Alter.Table("LockoutForms").AddColumn("SAPWorkOrderNumber").AsAnsiString(50).Nullable();
            Execute.Sql(
                "update LockoutForms SET SAPWorkOrderNumber = (SELECT SAPWorkOrder from ProductionWorkOrders PWO WHERE PWO.Id = ProductionWorkOrderId)");
            Delete.ForeignKeyColumn("LockoutForms", "ProductionWorkOrderId", "ProductionWorkOrders", "Id");
        }
    }
}
