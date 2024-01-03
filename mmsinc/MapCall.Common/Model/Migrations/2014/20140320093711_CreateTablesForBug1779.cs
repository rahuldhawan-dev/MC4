using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140320093711), Tags("Production")]
    public class CreateTablesForBug1779 : Migration
    {
        #region Constants

        public struct TableNames
        {
            public const string ESTIMATING_PROJECTS_PERMITS = "EstimatingProjectsPermits",
                                PERMIT_TYPES = "PermitTypes",
                                ESTIMATING_PROJECTS = "EstimatingProjects";
        }

        public struct ColumnNames
        {
            public const string ID = "Id",
                                PERMIT_TYPE_ID = "PermitTypeId",
                                ESTIMATING_PROJECT_ID = "EstimatingProjectId",
                                QUANTITY = "Quantity",
                                COST = "Cost";
        }

        public struct ForeignKeys
        {
            public const string
                FK_ESTIMATING_PROJECTS_PERMIT_TYPES_ESTIMATING_PROJECTS =
                    "FK_EstimatingProjectsPermitTypes_EstimatingProjects_EstimatingProjectId",
                FK_ESTIMATING_PROJECTS_PERMIT_TYPES_PERMIT_TYPES =
                    "FK_EstimatingProjectsPermitTypes_EstimatingProjects_PermitTypeId";
        }

        #endregion

        public override void Up()
        {
            Create.Table(TableNames.ESTIMATING_PROJECTS_PERMITS)
                  .WithColumn(ColumnNames.ID).AsInt32().Identity().PrimaryKey().NotNullable().Unique()
                  .WithColumn(ColumnNames.ESTIMATING_PROJECT_ID).AsInt32().NotNullable()
                  .ForeignKey(ForeignKeys.FK_ESTIMATING_PROJECTS_PERMIT_TYPES_ESTIMATING_PROJECTS,
                       TableNames.ESTIMATING_PROJECTS, ColumnNames.ID)
                  .WithColumn(ColumnNames.PERMIT_TYPE_ID).AsInt32().NotNullable()
                  .ForeignKey(ForeignKeys.FK_ESTIMATING_PROJECTS_PERMIT_TYPES_PERMIT_TYPES, TableNames.PERMIT_TYPES,
                       ColumnNames.PERMIT_TYPE_ID)
                  .WithColumn(ColumnNames.QUANTITY).AsInt32().NotNullable()
                  .WithColumn(ColumnNames.COST).AsCurrency().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableNames.ESTIMATING_PROJECTS_PERMITS);
        }
    }
}
