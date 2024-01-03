using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160415162337073), Tags("Production")]
    public class UpdateTablesForRecurringProjectsForBug2822 : Migration
    {
        public struct TableNames
        {
            public const string
                RECURRING_PROJECTS = "RecurringProjects",
                RECURRING_PROJECT_STATUSES = "RecurringProjectStatuses",
                RECURRING_PROJECT_TYPES = "RecurringProjectTypes",
                RECURRING_PROJECT_REGULATORY_STATUSES = "RecurringProjectRegulatoryStatuses",
                PIPE_DATA_LOOKUP_TYPES = "PipeDataLookupTypes",
                ENDORSEMENT_STATUSES = "EndorsementStatuses",
                PIPE_DIAMETERS = "PipeDiameters",
                PIPE_MATERIALS = "PipeMaterials",
                HIGH_COST_FACTORS = "HighCostFactors",
                PIPE_DATA_LOOKUP_VALUES = "PipeDataLookupValues",
                RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUES = "RecurringProjectsPipeDataLookupValues",
                RECURRING_PROJECT_ENDORSEMENTS = "RecurringProjectEndorsements",
                RECURRING_PROJECTS_HIGH_COST_FACTORS = "RecurringProjectsHighCostFactors";
        }

        public struct TableNamesOld
        {
            public const string
                RECURRING_PROJECTS = "RPProjects",
                RECURRING_PROJECT_STATUSES = "RPProjectStatuses",
                RECURRING_PROJECT_TYPES = "ProjectTypes",
                RECURRING_PROJECT_REGULATORY_STATUSES = "RPProjectRegulatoryStatuses",
                RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUES = "RPProjectsPipeDataLookupValues",
                RECURRING_PROJECT_ENDORSEMENTS = "RPProjectEndorsements",
                RECURRING_PROJECTS_HIGH_COST_FACTORS = "RPProjectsHighCostFactors";
        }

        public struct ColumnNames
        {
            public const string ID = "Id",
                                RECURRING_PROJECT_ID = "RecurringProjectID",
                                RECURRING_PROJECT_REGULATORY_STATUS_ID = "RecurringProjectRegulatoryStatusId";
        }

        public struct ColumnNamesOld
        {
            public const string
                RECURRING_PROJECT_ID = "RPProjectID",
                RECURRING_PROJECT_STATUSES_ID = "RPProjectStatusID",
                RECURRING_PROJECT_TYPES_ID = "ProjectTypeID",
                PIPE_DATA_LOOKUP_TYPE_ID = "PipeDataLookupTypeID",
                ENDORSEMENT_STATUSES_ID = "EndorsementStatusID",
                PIPE_DIAMETERS_ID = "PipeDiameterID",
                PIPE_MATERIALS_ID = "PipeMaterialID",
                HIGH_COST_FACTOR_ID = "HighCostFactorID",
                PIPE_DATA_LOOKUP_VALUE_ID = "PipeDataLookupValueID",
                RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUE_ID = "RPProjectsPipeDataLookupValueID",
                RECURRING_PROJECTS_RECURRING_PROJECT_ENDORSEMENT_ID = "RPProjectEndorsementID",
                RECURRING_PROJECT_REGULATORY_STATUS_ID = "RPProjectRegulatoryStatusId",
                VARIABLE_SCORE = "VariableScore",
                PRIORITY_WEIGHTED_SCORE = "PriorityWeightedScore",
                CREATED_ON = "CreatedOn",
                CREATED_BY = "CreatedBy";
        }

        public override void Up()
        {
            // Recurring Projects
            Rename.Table(TableNamesOld.RECURRING_PROJECTS).To(TableNames.RECURRING_PROJECTS);
            Rename.Column(ColumnNamesOld.RECURRING_PROJECT_ID).OnTable(TableNames.RECURRING_PROJECTS)
                  .To(ColumnNames.ID);
            Rename.Column(ColumnNamesOld.RECURRING_PROJECT_REGULATORY_STATUS_ID).OnTable(TableNames.RECURRING_PROJECTS)
                  .To(ColumnNames.RECURRING_PROJECT_REGULATORY_STATUS_ID);
            Alter.Table(TableNames.RECURRING_PROJECTS).AddForeignKeyColumn("CreatedById", "tblPermissions", "RecId");
            Execute.Sql(
                "UPDATE RecurringProjects SET CreatedById = COALESCE((Select RecID from tblPermissions where UserName = CreatedBy), (SELECT RecID from tblPermissions where username='mcadmin'))");
            Delete.Column("CreatedBy").FromTable(TableNames.RECURRING_PROJECTS);

            // Recurring Project Statuses
            Rename.Table(TableNamesOld.RECURRING_PROJECT_STATUSES).To(TableNames.RECURRING_PROJECT_STATUSES);
            Rename.Column(ColumnNamesOld.RECURRING_PROJECT_STATUSES_ID).OnTable(TableNames.RECURRING_PROJECT_STATUSES)
                  .To(ColumnNames.ID);
            // Recurring Project Types
            Rename.Table(TableNamesOld.RECURRING_PROJECT_TYPES).To(TableNames.RECURRING_PROJECT_TYPES);
            Rename.Column(ColumnNamesOld.RECURRING_PROJECT_TYPES_ID).OnTable(TableNames.RECURRING_PROJECT_TYPES)
                  .To(ColumnNames.ID);
            // Recurring Projct Regulatory Statuses
            Rename.Table(TableNamesOld.RECURRING_PROJECT_REGULATORY_STATUSES)
                  .To(TableNames.RECURRING_PROJECT_REGULATORY_STATUSES);
            // Pipe Data Lookup Type
            Rename.Column(ColumnNamesOld.PIPE_DATA_LOOKUP_TYPE_ID).OnTable(TableNames.PIPE_DATA_LOOKUP_TYPES)
                  .To(ColumnNames.ID);
            // Endorsement Statuses
            Rename.Column(ColumnNamesOld.ENDORSEMENT_STATUSES_ID).OnTable(TableNames.ENDORSEMENT_STATUSES)
                  .To(ColumnNames.ID);
            // PipeDiameters
            Rename.Column(ColumnNamesOld.PIPE_DIAMETERS_ID).OnTable(TableNames.PIPE_DIAMETERS).To(ColumnNames.ID);
            Delete.Column(ColumnNamesOld.VARIABLE_SCORE).FromTable(TableNames.PIPE_DIAMETERS);
            Delete.Column(ColumnNamesOld.PRIORITY_WEIGHTED_SCORE).FromTable(TableNames.PIPE_DIAMETERS);
            Delete.Column(ColumnNamesOld.CREATED_BY).FromTable(TableNames.PIPE_DIAMETERS);
            Delete.Column(ColumnNamesOld.CREATED_ON).FromTable(TableNames.PIPE_DIAMETERS);
            // PipeMaterials
            Rename.Column(ColumnNamesOld.PIPE_MATERIALS_ID).OnTable(TableNames.PIPE_MATERIALS).To(ColumnNames.ID);
            Delete.Column(ColumnNamesOld.VARIABLE_SCORE).FromTable(TableNames.PIPE_MATERIALS);
            Delete.Column(ColumnNamesOld.PRIORITY_WEIGHTED_SCORE).FromTable(TableNames.PIPE_MATERIALS);
            Delete.Column(ColumnNamesOld.CREATED_BY).FromTable(TableNames.PIPE_MATERIALS);
            Delete.Column(ColumnNamesOld.CREATED_ON).FromTable(TableNames.PIPE_MATERIALS);
            // HighCostFactors
            Rename.Column(ColumnNamesOld.HIGH_COST_FACTOR_ID).OnTable(TableNames.HIGH_COST_FACTORS).To(ColumnNames.ID);
            Delete.Column(ColumnNamesOld.VARIABLE_SCORE).FromTable(TableNames.HIGH_COST_FACTORS);
            Delete.Column(ColumnNamesOld.PRIORITY_WEIGHTED_SCORE).FromTable(TableNames.HIGH_COST_FACTORS);
            Delete.Column(ColumnNamesOld.CREATED_BY).FromTable(TableNames.HIGH_COST_FACTORS);
            Delete.Column(ColumnNamesOld.CREATED_ON).FromTable(TableNames.HIGH_COST_FACTORS);

            // PipeDataLookupValues 
            Rename.Column(ColumnNamesOld.PIPE_DATA_LOOKUP_VALUE_ID).OnTable(TableNames.PIPE_DATA_LOOKUP_VALUES)
                  .To(ColumnNames.ID);
            // RecurringProjectsPipeDataLookupValues
            Rename.Table(TableNamesOld.RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUES)
                  .To(TableNames.RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUES);
            Rename.Column(ColumnNamesOld.RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUE_ID)
                  .OnTable(TableNames.RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUES).To(ColumnNames.ID);
            Rename.Column(ColumnNamesOld.RECURRING_PROJECT_ID)
                  .OnTable(TableNames.RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUES).To(ColumnNames.RECURRING_PROJECT_ID);
            // RecurringProjectEndorsements
            Rename.Table(TableNamesOld.RECURRING_PROJECT_ENDORSEMENTS).To(TableNames.RECURRING_PROJECT_ENDORSEMENTS);
            Rename.Column(ColumnNamesOld.RECURRING_PROJECTS_RECURRING_PROJECT_ENDORSEMENT_ID)
                  .OnTable(TableNames.RECURRING_PROJECT_ENDORSEMENTS).To(ColumnNames.ID);
            Rename.Column(ColumnNamesOld.RECURRING_PROJECT_ID).OnTable(TableNames.RECURRING_PROJECT_ENDORSEMENTS)
                  .To(ColumnNames.RECURRING_PROJECT_ID);
            Alter.Table(TableNames.RECURRING_PROJECT_ENDORSEMENTS)
                 .AddForeignKeyColumn("UserId", "tblPermissions", "RecID");
            Execute.Sql(
                "Update RecurringProjectEndorsements SET UserId = (Select RecID from tblPermissions where username = tblEmployeeId)");
            Delete.Column("tblEmployeeId").FromTable(TableNames.RECURRING_PROJECT_ENDORSEMENTS);
            // RecurringProjectsHighCostFactors
            Rename.Table(TableNamesOld.RECURRING_PROJECTS_HIGH_COST_FACTORS)
                  .To(TableNames.RECURRING_PROJECTS_HIGH_COST_FACTORS);
            Alter.Table(TableNames.RECURRING_PROJECTS_HIGH_COST_FACTORS).AddColumn("Id").AsInt32().PrimaryKey()
                 .Identity().NotNullable();
            Rename.Column(ColumnNamesOld.RECURRING_PROJECT_ID).OnTable(TableNames.RECURRING_PROJECTS_HIGH_COST_FACTORS)
                  .To(ColumnNames.RECURRING_PROJECT_ID);

            Execute.Sql("update DataType set Table_Name = 'RecurringProjects' where DataTypeID = 153");
        }

        public override void Down()
        {
            Execute.Sql("update DataType set Table_Name = 'RPProjects' where DataTypeID = 153;");
            // Recurring Projects
            Alter.Table(TableNames.RECURRING_PROJECTS).AddColumn("CreatedBy").AsAnsiString(50).Nullable();
            Execute.Sql(
                "UPDATE RecurringProjects SET CreatedBy = (SELECT UserName from TblPermissions where RecId = CreatedById)");
            Delete.ForeignKeyColumn("RecurringProjects", "CreatedById", "tblPermissions", "RecID");

            Rename.Column(ColumnNames.RECURRING_PROJECT_REGULATORY_STATUS_ID).OnTable(TableNames.RECURRING_PROJECTS)
                  .To(ColumnNamesOld.RECURRING_PROJECT_REGULATORY_STATUS_ID);
            Rename.Column(ColumnNames.ID).OnTable(TableNames.RECURRING_PROJECTS)
                  .To(ColumnNamesOld.RECURRING_PROJECT_ID);
            Rename.Table(TableNames.RECURRING_PROJECTS).To(TableNamesOld.RECURRING_PROJECTS);
            // Recurring Project Statuses
            Rename.Column(ColumnNames.ID).OnTable(TableNames.RECURRING_PROJECT_STATUSES)
                  .To(ColumnNamesOld.RECURRING_PROJECT_STATUSES_ID);
            Rename.Table(TableNames.RECURRING_PROJECT_STATUSES).To(TableNamesOld.RECURRING_PROJECT_STATUSES);
            // Recurring Project Types
            Rename.Column(ColumnNames.ID).OnTable(TableNames.RECURRING_PROJECT_TYPES)
                  .To(ColumnNamesOld.RECURRING_PROJECT_TYPES_ID);
            Rename.Table(TableNames.RECURRING_PROJECT_TYPES).To(TableNamesOld.RECURRING_PROJECT_TYPES);
            // Recurring Projct Regulatory Statuses
            Rename.Table(TableNames.RECURRING_PROJECT_REGULATORY_STATUSES)
                  .To(TableNamesOld.RECURRING_PROJECT_REGULATORY_STATUSES);
            // Pipe Data Lookup Type
            Rename.Column(ColumnNames.ID).OnTable(TableNames.PIPE_DATA_LOOKUP_TYPES)
                  .To(ColumnNamesOld.PIPE_DATA_LOOKUP_TYPE_ID);
            // Endorsement Statuses
            Rename.Column(ColumnNames.ID).OnTable(TableNames.ENDORSEMENT_STATUSES)
                  .To(ColumnNamesOld.ENDORSEMENT_STATUSES_ID);
            // PipeDiameters
            Rename.Column(ColumnNames.ID).OnTable(TableNames.PIPE_DIAMETERS).To(ColumnNamesOld.PIPE_DIAMETERS_ID);
            Create.Column(ColumnNamesOld.CREATED_BY).OnTable(TableNames.PIPE_DIAMETERS).AsAnsiString(50).Nullable();
            Create.Column(ColumnNamesOld.CREATED_ON).OnTable(TableNames.PIPE_DIAMETERS).AsDateTime().Nullable();
            Create.Column(ColumnNamesOld.VARIABLE_SCORE).OnTable(TableNames.PIPE_DIAMETERS).AsDecimal(18, 2).Nullable();
            Create.Column(ColumnNamesOld.PRIORITY_WEIGHTED_SCORE).OnTable(TableNames.PIPE_DIAMETERS).AsDecimal(18, 2)
                  .Nullable();
            // PipeMaterials
            Rename.Column(ColumnNames.ID).OnTable(TableNames.PIPE_MATERIALS).To(ColumnNamesOld.PIPE_MATERIALS_ID);
            Create.Column(ColumnNamesOld.CREATED_BY).OnTable(TableNames.PIPE_MATERIALS).AsAnsiString(50).Nullable();
            Create.Column(ColumnNamesOld.CREATED_ON).OnTable(TableNames.PIPE_MATERIALS).AsDateTime().Nullable();
            Create.Column(ColumnNamesOld.VARIABLE_SCORE).OnTable(TableNames.PIPE_MATERIALS).AsDecimal(18, 2).Nullable();
            Create.Column(ColumnNamesOld.PRIORITY_WEIGHTED_SCORE).OnTable(TableNames.PIPE_MATERIALS).AsDecimal(18, 2)
                  .Nullable();
            // HighCostFactors
            Rename.Column(ColumnNames.ID).OnTable(TableNames.HIGH_COST_FACTORS).To(ColumnNamesOld.HIGH_COST_FACTOR_ID);
            Create.Column(ColumnNamesOld.CREATED_BY).OnTable(TableNames.HIGH_COST_FACTORS).AsAnsiString(50).Nullable();
            Create.Column(ColumnNamesOld.CREATED_ON).OnTable(TableNames.HIGH_COST_FACTORS).AsDateTime().Nullable();
            Create.Column(ColumnNamesOld.VARIABLE_SCORE).OnTable(TableNames.HIGH_COST_FACTORS).AsDecimal(18, 2)
                  .Nullable();
            Create.Column(ColumnNamesOld.PRIORITY_WEIGHTED_SCORE).OnTable(TableNames.HIGH_COST_FACTORS).AsDecimal(18, 2)
                  .Nullable();
            // PipeDataLookupValues
            Rename.Column(ColumnNames.ID).OnTable(TableNames.PIPE_DATA_LOOKUP_VALUES)
                  .To(ColumnNamesOld.PIPE_DATA_LOOKUP_VALUE_ID);
            // RecuringProjectsPipeDataLookupValues
            Rename.Column(ColumnNames.ID).OnTable(TableNames.RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUES)
                  .To(ColumnNamesOld.RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUE_ID);
            Rename.Column(ColumnNames.RECURRING_PROJECT_ID)
                  .OnTable(TableNames.RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUES)
                  .To(ColumnNamesOld.RECURRING_PROJECT_ID);
            Rename.Table(TableNames.RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUES)
                  .To(TableNamesOld.RECURRING_PROJECTS_PIPE_DATA_LOOKUP_VALUES);
            // RecurringProjectsRecurringProjectEndorsments
            Rename.Column(ColumnNames.ID).OnTable(TableNames.RECURRING_PROJECT_ENDORSEMENTS)
                  .To(ColumnNamesOld.RECURRING_PROJECTS_RECURRING_PROJECT_ENDORSEMENT_ID);
            Rename.Column(ColumnNames.RECURRING_PROJECT_ID).OnTable(TableNames.RECURRING_PROJECT_ENDORSEMENTS)
                  .To(ColumnNamesOld.RECURRING_PROJECT_ID);
            Alter.Table(TableNames.RECURRING_PROJECT_ENDORSEMENTS).AddColumn("tblEmployeeId").AsAnsiString(20)
                 .Nullable();
            Execute.Sql(
                "UPDATE RecurringProjectEndorsements SET tblEmployeeId = (SELECT username from tblPermissions WHERE RecID = UserId)");
            Delete.ForeignKeyColumn("RecurringProjectEndorsements", "UserId", "tblPermissions", "RecID");
            Rename.Table(TableNames.RECURRING_PROJECT_ENDORSEMENTS).To(TableNamesOld.RECURRING_PROJECT_ENDORSEMENTS);
            // RecurringProjectsHighCostFactors
            Delete.Column(ColumnNames.ID).FromTable(TableNames.RECURRING_PROJECTS_HIGH_COST_FACTORS);
            Rename.Column(ColumnNames.RECURRING_PROJECT_ID).OnTable(TableNames.RECURRING_PROJECTS_HIGH_COST_FACTORS)
                  .To(ColumnNamesOld.RECURRING_PROJECT_ID);
            Rename.Table(TableNames.RECURRING_PROJECTS_HIGH_COST_FACTORS)
                  .To(TableNamesOld.RECURRING_PROJECTS_HIGH_COST_FACTORS);
        }
    }
}
