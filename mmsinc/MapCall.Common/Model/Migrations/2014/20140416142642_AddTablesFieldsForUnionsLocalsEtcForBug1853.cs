using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    /// <summary>
    /// 5. Create Division table under Admin. The text fields should be State, Division, and SAP Union Desc. We will populate the Division and SAP Union Desc data. These field are all text fields. 
    /// 6. Add to the Union table these 3 fields (required on new)
    ///    Add State, drop down
    ///    Add Division, drop down (filtered by state selection)
    ///    Add SAP Union Desc, drop down (filtered by Division Selection)
    /// 7. Add Active / Inactive Status fields to Operating Centers and Locals. Include in search criteria.
    /// 8. Add two date fields to Grievances "Union Due Date" and "Management Due Date".
    /// </summary>
    [Migration(20140416142642), Tags("Production")]
    public class AddTablesFieldsForUnionsLocalsEtcForBug1853 : Migration
    {
        #region Constants

        public struct TableNames
        {
            public const string DIVISIONS = "Divisions",
                                STATES = "States",
                                OPERATING_CENTERS = "OperatingCenters",
                                LOCALS = "LocalBargainingUnits",
                                GRIEVANCES = "UnionGrievances";
        }

        public struct ColumnNames
        {
            public const string DIVISION_ID = "DivisionId",
                                STATE_ID = "StateId",
                                DESCRIPTION = "Description",
                                SAP_UNION_DESCRIPTION = "SAPUnionDescription",
                                IS_ACTIVE = "IsActive",
                                UNION_DUE_DATE = "UnionDueDate",
                                MANAGEMENT_DUE_DATE = "ManagementDueDate";
        }

        public struct ForeignKeys
        {
            public const string FK_DIVISIONS_STATES = "FK_Divisions_States_StateId";
        }

        public struct SqlStatements
        {
            public const string SET_ISACTIVE_TRUE_OPERATING_CENTERS =
                                    "UPDATE " + TableNames.OPERATING_CENTERS + " SET IsActive = 1",
                                SET_ISACTIVE_TRUE_LOCALS = "UPDATE " + TableNames.LOCALS + " SET IsActive = 1";
        }

        #endregion

        public override void Up()
        {
            // Division table
            Create.Table(TableNames.DIVISIONS)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(ColumnNames.STATE_ID).AsInt32().NotNullable().ForeignKey(ForeignKeys.FK_DIVISIONS_STATES,
                       TableNames.STATES, ColumnNames.STATE_ID)
                  .WithColumn(ColumnNames.DESCRIPTION).AsAnsiString(StringLengths.MAX_DEFAULT_VALUE).NotNullable()
                  .WithColumn(ColumnNames.SAP_UNION_DESCRIPTION).AsAnsiString(StringLengths.MAX_DEFAULT_VALUE)
                  .NotNullable();

            // Locals
            Alter.Table(TableNames.LOCALS)
                 .AddColumn(ColumnNames.DIVISION_ID).AsInt32().Nullable()
                 .AddColumn(ColumnNames.STATE_ID).AsInt32().Nullable();

            //Operating Centers IsActive
            Alter.Table(TableNames.OPERATING_CENTERS).AddColumn(ColumnNames.IS_ACTIVE).AsBoolean().NotNullable()
                 .WithDefaultValue(true);
            //Execute.Sql(SqlStatements.SET_ISACTIVE_TRUE_OPERATING_CENTERS);
            //Alter.Column(ColumnNames.IS_ACTIVE).OnTable(TableNames.OPERATING_CENTERS).AsBoolean().NotNullable();

            //Locals IsActive
            Alter.Table(TableNames.LOCALS).AddColumn(ColumnNames.IS_ACTIVE).AsBoolean().NotNullable()
                 .WithDefaultValue(true);
            //Execute.Sql(SqlStatements.SET_ISACTIVE_TRUE_LOCALS);
            //Alter.Column(ColumnNames.IS_ACTIVE).OnTable(TableNames.LOCALS).AsBinary().NotNullable();

            //Grievance Columns
            Alter.Table(TableNames.GRIEVANCES).AddColumn(ColumnNames.MANAGEMENT_DUE_DATE).AsDateTime().Nullable();
            Alter.Table(TableNames.GRIEVANCES).AddColumn(ColumnNames.UNION_DUE_DATE).AsDateTime().Nullable();
        }

        public override void Down()
        {
            //Grievance Columns
            Delete.Column(ColumnNames.UNION_DUE_DATE).FromTable(TableNames.GRIEVANCES);
            Delete.Column(ColumnNames.MANAGEMENT_DUE_DATE).FromTable(TableNames.GRIEVANCES);

            //Locals IsActive
            Delete.Column(ColumnNames.IS_ACTIVE).FromTable(TableNames.LOCALS);
            //Operating Center IsActive
            Delete.Column(ColumnNames.IS_ACTIVE).FromTable(TableNames.OPERATING_CENTERS);

            Delete.Column(ColumnNames.STATE_ID).FromTable(TableNames.LOCALS);
            Delete.Column(ColumnNames.DIVISION_ID).FromTable(TableNames.LOCALS);

            //Divisions
            Delete.Table(TableNames.DIVISIONS);
        }
    }
}
