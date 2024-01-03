using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130628124446), Tags("Production")]
    public class AddJobTitleCommonNamesTrainingModules : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string UPDATE_COMMON_NAME =
                                    "INSERT INTO JobTitleCommonNames SELECT DISTINCT Common_Name FROM tblPositions_Classifications WHERE ISNULL(Common_Name,'') <> '' ORDER BY 1;" +
                                    "UPDATE tblPositions_Classifications SET Common_Name = (SELECT JobTitleCommonNameID from JobTitleCommonNames where Description = Common_Name);",
                                ROLLBACK_COMMON_NAME =
                                    "UPDATE tblPositions_Classifications SET Common_Name = (SELECT Description from JobTitleCommonNames where JobTitleCommonNameID = Common_Name);",
                                TRANSFORM_LOOKUPS =
                                    "SET IDENTITY_INSERT [dbo].[ProcessStages] ON;" +
                                    "INSERT INTO ProcessStages(ProcessStageID, Description)" +
                                    "  SELECT LookupID, LookupValue FROM Lookup WHERE LookupType = 'ProcessStage' ORDER BY LookupValue;" +
                                    "SET IDENTITY_INSERT [dbo].[ProcessStages] OFF;" +
                                    "SET IDENTITY_INSERT [dbo].[TrainingModuleCategories] ON;" +
                                    "INSERT INTO TrainingModuleCategories(TrainingModuleCategoryID, Description)" +
                                    "  SELECT LookupID, LookupValue FROM Lookup WHERE LookupType = 'TrainingModuleCategory' ORDER BY LookupValue;" +
                                    "SET IDENTITY_INSERT [dbo].[TrainingModuleCategories] OFF;";
        }

        public struct Tables
        {
            public const string JOB_TITLE_COMMON_NAMES = "JobTitleCommonNames",
                                POSITIONS = "tblPositions_Classifications",
                                POSITIONS_TRAINING_MODULES = "PositionsTrainingModules",
                                JOB_TITLE_COMMON_NAMES_TRAINING_MODULES = "JobTitleCommonNamesTrainingModules",
                                TRAINING_MODULES = "tblTrainingModules",
                                TRAINING_MODULE_CATEGORIES = "TrainingModuleCategories",
                                PROCESS_STAGES = "ProcessStages",
                                LOOKUP = "Lookup";
        }

        public struct Columns
        {
            public const string DESCRIPTION = "Description",
                                COMMON_NAME = "Common_Name",
                                JOB_TITLE_COMMON_NAME_ID = "JobTitleCommonNameID",
                                TRAINING_MODULE_ID = "TrainingModuleID",
                                TRAINING_MODULE_CATEGORY_ID = "TrainingModuleCategoryID",
                                TRAINING_MODULE_CATEGORY = "TrainingModuleCategory",
                                PROCESS_STAGE_ID = "ProcessStageID",
                                PROCESS_STAGE = "ProcessStage",
                                LOOKUP_ID = "LookupID";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 50,
                             COMMON_NAME = 255;
        }

        public struct ForeignKeys
        {
            public const string FK_POSITIONS_JOB_TITLE_COMMON_NAMES =
                                    "FK_tblPositions_Classifications_JobTitleCommonNames_Common_Name",
                                FK_JOB_TITLE_COMMON_NAMES_TRAINING_MODULES_JOB_TITLE_COMMON_NAMES =
                                    "FK_JobTitleCommonNamesTrainingModules_JobTitleCommonNames_JobTitleCommonNameID",
                                FK_JOB_TITLE_COMMON_NAMES_TRAINING_MODULES_TRAINING_MODULES =
                                    "FK_JobTitleCommonNamesTrainingModules_tblTrainingModules_TrainingModuleID",
                                FK_TRAINING_MODULES_TRAINING_MODULE_CATEGORIES =
                                    "FK_tblTrainingModules_TrainingModuleCategories_TrainingModuleCategory",
                                FK_TRAINING_MODULES_PROCESS_STAGES = "FK_tblTrainingModules_ProcessStages_ProcessStage",
                                FK_TRAINING_MODULES_PROCESS_STAGES_EXISTING = "FK_tblTrainingModules_Lookup1",
                                FK_TRAINING_MODULES_TRAINING_MODULE_CATEGORIES_EXISTING =
                                    "FK_tblTrainingModules_Lookup";
        }

        #endregion

        public override void Up()
        {
            #region Add Tables

            Create.Table(Tables.JOB_TITLE_COMMON_NAMES)
                  .WithColumn(Columns.JOB_TITLE_COMMON_NAME_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.JOB_TITLE_COMMON_NAMES_TRAINING_MODULES)
                  .WithColumn(Columns.JOB_TITLE_COMMON_NAME_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.TRAINING_MODULE_ID).AsInt32().NotNullable();
            Create.Table(Tables.TRAINING_MODULE_CATEGORIES)
                  .WithColumn(Columns.TRAINING_MODULE_CATEGORY_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.PROCESS_STAGES)
                  .WithColumn(Columns.PROCESS_STAGE_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();

            #endregion

            #region Alter Data

            Execute.Sql(Sql.UPDATE_COMMON_NAME);
            Execute.Sql(Sql.TRANSFORM_LOOKUPS);

            #endregion

            #region Alter Columns

            Alter.Table(Tables.POSITIONS).AlterColumn(Columns.COMMON_NAME)
                 .AsInt32().Nullable();

            #endregion

            #region Foreign Keys

            Create.ForeignKey(ForeignKeys.FK_POSITIONS_JOB_TITLE_COMMON_NAMES)
                  .FromTable(Tables.POSITIONS).ForeignColumn(Columns.COMMON_NAME)
                  .ToTable(Tables.JOB_TITLE_COMMON_NAMES).PrimaryColumn(Columns.JOB_TITLE_COMMON_NAME_ID);
            Create.ForeignKey(ForeignKeys.FK_JOB_TITLE_COMMON_NAMES_TRAINING_MODULES_JOB_TITLE_COMMON_NAMES)
                  .FromTable(Tables.JOB_TITLE_COMMON_NAMES_TRAINING_MODULES)
                  .ForeignColumn(Columns.JOB_TITLE_COMMON_NAME_ID)
                  .ToTable(Tables.JOB_TITLE_COMMON_NAMES).PrimaryColumn(Columns.JOB_TITLE_COMMON_NAME_ID);
            Create.ForeignKey(ForeignKeys.FK_JOB_TITLE_COMMON_NAMES_TRAINING_MODULES_TRAINING_MODULES)
                  .FromTable(Tables.JOB_TITLE_COMMON_NAMES_TRAINING_MODULES)
                  .ForeignColumn(Columns.TRAINING_MODULE_ID)
                  .ToTable(Tables.TRAINING_MODULES).PrimaryColumn(Columns.TRAINING_MODULE_ID);
            Delete.ForeignKey(ForeignKeys.FK_TRAINING_MODULES_PROCESS_STAGES_EXISTING)
                  .OnTable(Tables.TRAINING_MODULES);
            Delete.ForeignKey(ForeignKeys.FK_TRAINING_MODULES_TRAINING_MODULE_CATEGORIES_EXISTING)
                  .OnTable(Tables.TRAINING_MODULES);
            Create.ForeignKey(ForeignKeys.FK_TRAINING_MODULES_PROCESS_STAGES)
                  .FromTable(Tables.TRAINING_MODULES).ForeignColumn(Columns.PROCESS_STAGE)
                  .ToTable(Tables.PROCESS_STAGES).PrimaryColumn(Columns.PROCESS_STAGE_ID);
            Create.ForeignKey(ForeignKeys.FK_TRAINING_MODULES_TRAINING_MODULE_CATEGORIES)
                  .FromTable(Tables.TRAINING_MODULES).ForeignColumn(Columns.TRAINING_MODULE_CATEGORY)
                  .ToTable(Tables.TRAINING_MODULE_CATEGORIES).PrimaryColumn(Columns.TRAINING_MODULE_CATEGORY_ID);

            #endregion

            #region Drop

            Delete.Table(Tables.POSITIONS_TRAINING_MODULES);

            #endregion
        }

        public override void Down()
        {
            #region Restore Tables

            Create.Table(Tables.POSITIONS_TRAINING_MODULES)
                  .WithColumn("PositionsTrainingModulesID").AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn("PositionID").AsInt32().NotNullable()
                  .WithColumn("TrainingModuleID").AsInt32().NotNullable();

            #endregion

            #region Foreign Keys

            Delete.ForeignKey(ForeignKeys.FK_JOB_TITLE_COMMON_NAMES_TRAINING_MODULES_TRAINING_MODULES)
                  .OnTable(Tables.JOB_TITLE_COMMON_NAMES_TRAINING_MODULES);
            Delete.ForeignKey(ForeignKeys.FK_JOB_TITLE_COMMON_NAMES_TRAINING_MODULES_JOB_TITLE_COMMON_NAMES)
                  .OnTable(Tables.JOB_TITLE_COMMON_NAMES_TRAINING_MODULES);
            Delete.ForeignKey(ForeignKeys.FK_POSITIONS_JOB_TITLE_COMMON_NAMES).OnTable(Tables.POSITIONS);

            Delete.ForeignKey(ForeignKeys.FK_TRAINING_MODULES_PROCESS_STAGES)
                  .OnTable(Tables.TRAINING_MODULES);
            Delete.ForeignKey(ForeignKeys.FK_TRAINING_MODULES_TRAINING_MODULE_CATEGORIES)
                  .OnTable(Tables.TRAINING_MODULES);
            Create.ForeignKey(ForeignKeys.FK_TRAINING_MODULES_PROCESS_STAGES_EXISTING)
                  .FromTable(Tables.TRAINING_MODULES).ForeignColumn(Columns.PROCESS_STAGE)
                  .ToTable(Tables.LOOKUP).PrimaryColumn(Columns.LOOKUP_ID);
            Execute.Sql(
                "UPDATE tblTrainingModules SET TrainingModuleCategory = NULL WHERE TrainingModuleCategory NOT IN (SELECT LookupId FROM LOOKUP);");
            Create.ForeignKey(ForeignKeys.FK_TRAINING_MODULES_TRAINING_MODULE_CATEGORIES_EXISTING)
                  .FromTable(Tables.TRAINING_MODULES).ForeignColumn(Columns.TRAINING_MODULE_CATEGORY)
                  .ToTable(Tables.LOOKUP).PrimaryColumn(Columns.LOOKUP_ID);

            #endregion

            #region Alter Columns

            Alter.Table(Tables.POSITIONS).AlterColumn(Columns.COMMON_NAME)
                 .AsAnsiString(StringLengths.COMMON_NAME).Nullable();

            #endregion

            #region Alter Data

            Execute.Sql(Sql.ROLLBACK_COMMON_NAME);

            #endregion

            #region Drop Tables

            Delete.Table(Tables.JOB_TITLE_COMMON_NAMES);
            Delete.Table(Tables.JOB_TITLE_COMMON_NAMES_TRAINING_MODULES);
            Delete.Table(Tables.PROCESS_STAGES);
            Delete.Table(Tables.TRAINING_MODULE_CATEGORIES);

            #endregion
        }
    }
}
