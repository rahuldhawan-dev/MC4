using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180604210518468), Tags("Production")]
    public class UndoCorrectFieldTypeAndPutGarbageInBecauseDemo : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "ALTER TABLE [dbo].[ShortCycleWorkOrdersEquipmentIds] DROP CONSTRAINT [DF_ShortCycleWorkOrdersEquipmentIds_ProcessingIndicator]");
            Alter.Table("ShortCycleWorkOrdersEquipmentIds")
                 .AlterColumn("ProcessingIndicator").AsAnsiString(1).Nullable();
            Execute.Sql(
                "UPDATE [ShortCycleWorkOrdersEquipmentIds] SET [ProcessingIndicator] = 'X' WHERE [ProcessingIndicator] = '1'");
            Execute.Sql(
                "UPDATE [ShortCycleWorkOrdersEquipmentIds] SET [ProcessingIndicator] = null WHERE [ProcessingIndicator] = '0'");
        }

        public override void Down()
        {
            Execute.Sql(
                "UPDATE [ShortCycleWorkOrdersEquipmentIds] SET [ProcessingIndicator] = 1 WHERE IsNull([ProcessingIndicator],'') = 'X'");
            Execute.Sql(
                "UPDATE [ShortCycleWorkOrdersEquipmentIds] SET [ProcessingIndicator] = 0 WHERE IsNull([ProcessingIndicator],'') = ''");
            Alter.Table("ShortCycleWorkOrdersEquipmentIds")
                 .AlterColumn("ProcessingIndicator").AsBoolean().WithDefaultValue(false).NotNullable();
        }
    }
}
