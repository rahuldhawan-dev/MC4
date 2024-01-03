using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140429105811), Tags("Production")]
    public class MoveUnionDescriptionFieldToLocals : Migration
    {
        public struct TableNames
        {
            public const string DIVISIONS = "Divisions", LOCALS = "LocalBargainingUnits";
        }

        public struct ColumnNames
        {
            public const string SAP_UNION_DESCRIPTION = "SAPUnionDescription";
        }

        public override void Up()
        {
            Delete.Column(ColumnNames.SAP_UNION_DESCRIPTION).FromTable(TableNames.DIVISIONS);
            Alter.Table(TableNames.LOCALS).AddColumn(ColumnNames.SAP_UNION_DESCRIPTION)
                 .AsAnsiString(StringLengths.MAX_DEFAULT_VALUE).Nullable();
            Execute.Sql(String.Format("UPDATE [{0}] set [{1}] = ''", TableNames.LOCALS,
                ColumnNames.SAP_UNION_DESCRIPTION));
            Alter.Column(ColumnNames.SAP_UNION_DESCRIPTION).OnTable(TableNames.LOCALS)
                 .AsAnsiString(StringLengths.MAX_DEFAULT_VALUE).NotNullable();
        }

        public override void Down()
        {
            Delete.Column(ColumnNames.SAP_UNION_DESCRIPTION).FromTable(TableNames.LOCALS);
            Alter.Table(TableNames.DIVISIONS).AddColumn(ColumnNames.SAP_UNION_DESCRIPTION)
                 .AsAnsiString(StringLengths.MAX_DEFAULT_VALUE).Nullable();
            Execute.Sql(String.Format("UPDATE [{0}] set [{1}] = ''", TableNames.DIVISIONS,
                ColumnNames.SAP_UNION_DESCRIPTION));
            Alter.Column(ColumnNames.SAP_UNION_DESCRIPTION).OnTable(TableNames.DIVISIONS)
                 .AsAnsiString(StringLengths.MAX_DEFAULT_VALUE).NotNullable();
        }
    }
}
