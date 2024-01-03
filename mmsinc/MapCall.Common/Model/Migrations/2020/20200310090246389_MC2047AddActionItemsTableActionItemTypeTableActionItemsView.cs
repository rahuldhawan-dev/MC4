using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200310090246389), Tags("Production")]
    public class MC2047AddActionItemsTableActionItemTypeTableActionItemsView : Migration
    {
        public const string VIEW_NAME = "ActionItemLinkView";
        public const string DROP_VIEW = "DROP VIEW [" + VIEW_NAME + "];";

        public const string CREATE_VIEW = "CREATE VIEW [" + VIEW_NAME + @"] AS
SELECT
  ActionItems.Id as Id,
  ActionItems.LinkedId as LinkedId,
  ActionItems.DataTypeId,
  dt.Table_Name as TableName
FROM
  ActionItems
INNER JOIN
  DataType dt
ON
  dt.DataTypeId = ActionItems.DataTypeId;";

        public override void Up()
        {
            // Build Action Items Type table
            this.CreateLookupTableWithValues("ActionItemTypes",
                "Procedures",
                "Training",
                "Quality Control",
                "Communications",
                "Management System",
                "Human Engineering",
                "Work Direction",
                "Not Applicable/None Identified",
                "Equipment Difficulty",
                "Natural Disaster / Sabotage",
                "Other(Specify)");

            // Build Action Items Table
            Create.Table("ActionItems")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("DataTypeId", "DataType", "DataTypeID", false)
                  .WithForeignKeyColumn("ResponsibleOwnerId", "tblPermissions", "RecId", true)
                  .WithForeignKeyColumn("ActionItemTypeId", "ActionItemTypes", "Id", false)
                  .WithColumn("NotListedType").AsAnsiString(200).Nullable()
                  .WithColumn("Note").AsAnsiString(int.MaxValue)
                  .WithColumn("CreatedBy").AsAnsiString(50)
                  .WithColumn("DateAdded").AsDateTime().NotNullable()
                  .WithColumn("LinkedId").AsInt32()
                  .WithColumn("DateCompleted").AsDateTime().Nullable()
                  .WithColumn("TargetedCompletionDate").AsDateTime().NotNullable();

            // Build ActionItemsView cause we need to link all the things together

            Execute.Sql(CREATE_VIEW);
        }

        public override void Down()
        {
            // Drop all the things above

            Execute.Sql(DROP_VIEW);
            Delete.ForeignKeyColumn("ActionItems", "DataTypeId", "DataType", "DataTypeID");
            Delete.ForeignKeyColumn("ActionItems", "ResponsibleOwnerId", "tblPermissions", "RecId");
            Delete.ForeignKeyColumn("ActionItems", "ActionItemTypeId", "ActionItemTypes");
            Delete.Table("ActionItems");
            Delete.Table("ActionItemTypes");
        }
    }
}
