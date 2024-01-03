using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231005095847834), Tags("Production")]
    public class MC6208_AddNeedsToSyncColumnForServices : Migration
    {
        public const string
            UPDATE_DIVISION = @"
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'FS' WHERE [Type] = 'FPUB';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'FS' WHERE [Type] = 'FPVT';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'IRRG';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'WATR';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'SW' WHERE [Type] = 'WWTR';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'BULK';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'BUMA';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'DISC';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'DVST';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'SW' WHERE [Type] = 'FLAT';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'FREE';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'FS' WHERE [Type] = 'FSDC';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'GREY';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'MB1C';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'FS' WHERE [Type] = 'MBAC';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'NON';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'RUSE';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'TEMP';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'WT' WHERE [Type] = 'USER';
                UPDATE [dbo].[ServiceUtilityTypes] SET [Division] = 'SW' WHERE [Type] = 'WWDT';
                GO";

        public override void Up()
        {
            Create.Column("NeedsToSync").OnTable("Services").AsBoolean().NotNullable().WithDefaultValue(false);
            Create.Column("LastSyncedAt").OnTable("Services").AsDateTime().Nullable();
            Create.Column("Division").OnTable("ServiceUtilityTypes").AsAnsiString(2).Nullable();
            Execute.Sql(UPDATE_DIVISION);
        }

        public override void Down()
        {
            Delete.Column("NeedsToSync").FromTable("Services");
            Delete.Column("LastSyncedAt").FromTable("Services");
            Delete.Column("Division").FromTable("ServiceUtilityTypes");
        }
    }
}
