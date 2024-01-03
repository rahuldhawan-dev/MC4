using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230720133608588), Tags("Production")]
    public class MC5538_AddDataTypeForeignKeyReferenceToActionItemType : Migration
    {
        public struct TableNames
        {
            public const string ACTION_ITEM_TYPES = "ActionItemTypes", DATA_TYPES = "DataType";
        }

        public struct ColumnNames
        {
            public const string DATA_TYPE_KEY_FIELD = "DataTypeId";
        }
        
        public override void Up()
        {
            Alter.Table(TableNames.ACTION_ITEM_TYPES)
                 .AddForeignKeyColumn(ColumnNames.DATA_TYPE_KEY_FIELD, TableNames.DATA_TYPES, ColumnNames.DATA_TYPE_KEY_FIELD);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(TableNames.ACTION_ITEM_TYPES, ColumnNames.DATA_TYPE_KEY_FIELD, TableNames.DATA_TYPES, ColumnNames.DATA_TYPE_KEY_FIELD);
        }
    }
}