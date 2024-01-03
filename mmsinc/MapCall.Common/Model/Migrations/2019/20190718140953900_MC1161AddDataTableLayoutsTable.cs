using System.Linq;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190718140953900), Tags("Production")]
    public class MC1161AddDataTableLayoutsTable : Migration
    {
        #region Private Methods

        private void CreateBasicLayoutForShortCycleWorkOrderCompletions()
        {
            Insert.IntoTable("DataTableLayouts").Row(new
                {LayoutName = "Basic", Area = "ShortCycle", Controller = "ShortCycleWorkOrderCompletion"});

            var props = new[] {
                "Id",
                "WorkOrderNumber",
                "FunctionalLocation",
                "CustomerAccount",
                "Premise",
                "MATCode",
                "EncoderIds",
                "MIUNumber",
                "ReadType",
                "OldRead",
                "NewRead",
                "ReasonCode",
                "Activities",
                "UserStatus",
                "CompletionStatus",
                "AdditionalWorkNeeded",
                "Purpose",
                "DateCompleted",
                "ServiceFound",
                "ServiceLeft",
                "OperatedPointOfControl",
                "FSRId",
                "ManufacturerSerialNumber",
                "MeterSerialNumber",
                "DeviceCategory",
                "Installation"
            }.ToList();

            foreach (var prop in props)
            {
                Execute.Sql(
                    $@"insert into [DataTableLayoutProperties] (DataTableLayoutId, PropertyName) VALUES ((select dtl.Id from DataTableLayouts dtl where dtl.LayoutName = 'Basic'), '{prop}')");
            }
        }

        #endregion

        public override void Up()
        {
            Create.Table("DataTableLayouts")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("LayoutName").AsString(100).NotNullable()
                  .WithColumn("Area").AsString(100).Nullable()
                  .WithColumn("Controller").AsString(100).NotNullable();

            Create.Table("DataTableLayoutProperties")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("DataTableLayoutId").AsInt32().NotNullable()
                  .ForeignKey("FK_DataTableLayoutProperties_DataTableLayouts_Id", "DataTableLayouts", "Id")
                  .WithColumn("PropertyName").AsString(100).NotNullable();

            CreateBasicLayoutForShortCycleWorkOrderCompletions();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_DataTableLayoutProperties_DataTableLayouts_Id").OnTable("DataTableLayoutProperties");
            Delete.Table("DataTableLayoutProperties");
            Delete.Table("DataTableLayouts");
        }
    }
}
