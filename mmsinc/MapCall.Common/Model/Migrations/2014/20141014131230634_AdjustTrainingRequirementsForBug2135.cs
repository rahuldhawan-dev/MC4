using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141014131230634), Tags("Production")]
    public class AdjustTrainingRequirementsForBug2135 : Migration
    {
        #region Constants

        public struct TableNames
        {
            public const string TRAINING_REQUIREMENTS = "TrainingRequirements",
                                TRAINING_MODULES = "tblTrainingModules",
                                TRAINING_MODULES_TRAINING_REQUIREMENTS = "TrainingModulesTrainingRequirements",
                                TRAINING_MODULE_RECURRANT_TYPES = "TrainingModuleRecurrantTypes",
                                POSITION_GROUP_COMMON_NAMES = "PositionGroupCommonNames",
                                POSITION_GROUP_COMMON_NAMES_TRAINING_REQUIREMENTS =
                                    "PositionGroupCommonNamesTrainingRequirements",
                                POSITION_GROUP_COMMON_NAMES_TRAINING_MODULES =
                                    "PositionGroupCommonNamesTrainingModules";
        }

        public struct ColumnNames
        {
            public const string ACTIVE_INITIAL_TRAINING_MODULE = "ActiveInitialTrainingModuleId",
                                ACTIVE_RECURRING_TRAINING_MODULE = "ActiveRecurringTrainingModuleId",
                                ACTIVE_INITIAL_AND_RECURRING_TRAINING_MODULE =
                                    "ActiveInitialAndRecurringTrainingModuleId",
                                TRAINING_MODULE_ID = "TrainingModuleID",
                                TRAINING_REQUIREMENT_ID = "TrainingRequirementId",
                                TRAINING_MODULE_RECURRANT_TYPE_ID = "TrainingModuleRecurrantTypeId",
                                INITIAL_TRAINING_REQUIREMENT = "InitialTrainingRequirements",
                                RECURRING_TRAINING_REQUIREMENT = "RecurringTrainingRequirement",
                                POSITION_GROUP_COMMON_NAME_ID = "PositionGroupCommonNameId";
        }

        public struct Sql
        {
            public const string
                UPDATE_TRAINING_MODULES = @"
                    UPDATE tblTrainingModules SET TrainingRequirementId = (SELECT TOP 1 tmtr.trainingRequirementId from TrainingModulesTrainingRequirements tmtr WHERE tmtr.TrainingModuleId = tblTrainingModules.TrainingModuleID);
                    UPDATE tblTrainingModules SET TrainingModuleRecurrantTypeId = (SELECT Id FROM TrainingModuleRecurrantTypes WHERE Description = 'Initial') where RecurringTrainingRequirement = 0 AND InitialTrainingRequirements = 1;
                    UPDATE tblTrainingModules SET TrainingModuleRecurrantTypeId = (SELECT Id FROM TrainingModuleRecurrantTypes WHERE Description = 'Recurring') where RecurringTrainingRequirement = 1 AND InitialTrainingRequirements = 0;
                    UPDATE tblTrainingModules SET TrainingModuleRecurrantTypeId = (SELECT Id FROM TrainingModuleRecurrantTypes WHERE Description = 'Initial/Recurring') where RecurringTrainingRequirement = 1 AND InitialTrainingRequirements = 1;",
                ROLLBACK_TRAINING_MODULES = @"
                    INSERT INTO TrainingModulesTrainingRequirements SELECT TrainingModuleId, TrainingRequirementID from tblTrainingModules where TrainingRequirementID is not null;
                    UPDATE tblTrainingModules SET InitialTrainingRequirements = 1 where TrainingModuleRecurrantTypeId in (select Id from TrainingModuleRecurrantTypes where Description in ('Initial','Initial/Recurring'));
                    UPDATE tblTrainingModules SET RecurringTrainingRequirement = 1 where TrainingModuleRecurrantTypeId in (select Id from TrainingModuleRecurrantTypes where Description in ('Recurring','Initial/Recurring'));";
        }

        #endregion

        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.TRAINING_MODULE_RECURRANT_TYPES, "Initial", "Recurring",
                "Initial and Recurring");

            Alter.Table(TableNames.TRAINING_REQUIREMENTS)
                 .AddForeignKeyColumn(ColumnNames.ACTIVE_INITIAL_TRAINING_MODULE, TableNames.TRAINING_MODULES,
                      ColumnNames.TRAINING_MODULE_ID);
            Alter.Table(TableNames.TRAINING_REQUIREMENTS)
                 .AddForeignKeyColumn(ColumnNames.ACTIVE_RECURRING_TRAINING_MODULE, TableNames.TRAINING_MODULES,
                      ColumnNames.TRAINING_MODULE_ID);
            Alter.Table(TableNames.TRAINING_REQUIREMENTS)
                 .AddForeignKeyColumn(ColumnNames.ACTIVE_INITIAL_AND_RECURRING_TRAINING_MODULE,
                      TableNames.TRAINING_MODULES, ColumnNames.TRAINING_MODULE_ID);

            Alter.Table(TableNames.TRAINING_MODULES)
                 .AddForeignKeyColumn(ColumnNames.TRAINING_REQUIREMENT_ID, TableNames.TRAINING_REQUIREMENTS);
            Alter.Table(TableNames.TRAINING_MODULES)
                 .AddForeignKeyColumn(ColumnNames.TRAINING_MODULE_RECURRANT_TYPE_ID,
                      TableNames.TRAINING_MODULE_RECURRANT_TYPES);

            Rename.Column("RecurringTrainingFrequencyDays").OnTable(TableNames.TRAINING_REQUIREMENTS)
                  .To("TrainingFrequency");
            Alter.Column("TrainingFrequency").OnTable(TableNames.TRAINING_REQUIREMENTS).AsInt32().Nullable();
            Alter.Table(TableNames.TRAINING_REQUIREMENTS).AddColumn("TrainingFrequencyUnit").AsString(10).Nullable();
            Execute.Sql(
                "UPDATE TrainingRequirements SET TrainingFrequencyUnit = 'D' WHERE IsNull(TrainingFrequency, 0)>0");

            Create.Table(TableNames.POSITION_GROUP_COMMON_NAMES_TRAINING_REQUIREMENTS)
                  .WithColumn(ColumnNames.POSITION_GROUP_COMMON_NAME_ID).AsInt32().ForeignKey(
                       "FK_PositionGroupCommonNamesTrainingRequirements_PositionGroupCommonNames_PositionGroupCommonNameId",
                       TableNames.POSITION_GROUP_COMMON_NAMES, "Id")
                  .WithColumn(ColumnNames.TRAINING_REQUIREMENT_ID).AsInt32().ForeignKey(
                       "FK_PositionGroupCommonNamesTrainingRequirements_TrainingRequirements_TrainingRequirementId",
                       TableNames.TRAINING_REQUIREMENTS, "Id");

            Execute.Sql(Sql.UPDATE_TRAINING_MODULES);
            Delete.Column(ColumnNames.INITIAL_TRAINING_REQUIREMENT).FromTable(TableNames.TRAINING_MODULES);
            Delete.Column(ColumnNames.RECURRING_TRAINING_REQUIREMENT).FromTable(TableNames.TRAINING_MODULES);
            Delete.Table(TableNames.TRAINING_MODULES_TRAINING_REQUIREMENTS);
            Delete.Table(TableNames.POSITION_GROUP_COMMON_NAMES_TRAINING_MODULES);

            Delete.Column("PSM_TCPA_Requirement").FromTable(TableNames.TRAINING_MODULES);
            Delete.Column("DPCC_Requirement").FromTable(TableNames.TRAINING_MODULES);
            Delete.Column("OSHA_Requirement").FromTable(TableNames.TRAINING_MODULES);
            Delete.Column("DOT_Requirement").FromTable(TableNames.TRAINING_MODULES);
            Delete.Column("RecurringTrainingFrequencyDays").FromTable(TableNames.TRAINING_MODULES);
            Delete.Column("FieldOperations").FromTable(TableNames.TRAINING_MODULES);

            Delete.Column("IsInitialRequirement").FromTable(TableNames.TRAINING_REQUIREMENTS);

            Delete.Column("TrainingFrequency").FromTable(TableNames.TRAINING_MODULES);
            Delete.Column("TrainingFrequencyUnit").FromTable(TableNames.TRAINING_MODULES);
        }

        public override void Down()
        {
            Rename.Column("TrainingFrequency").OnTable(TableNames.TRAINING_REQUIREMENTS)
                  .To("RecurringTrainingFrequencyDays");
            Alter.Column("RecurringTrainingFrequencyDays").OnTable(TableNames.TRAINING_REQUIREMENTS).AsInt32()
                 .NotNullable();
            Delete.Column("TrainingFrequencyUnit").FromTable(TableNames.TRAINING_REQUIREMENTS);

            Alter.Table("tblTrainingModules").AddColumn("TrainingFrequency").AsInt32().Nullable();
            Alter.Table("tblTrainingModules").AddColumn("TrainingFrequencyUnit").AsString(10).Nullable();

            Alter.Table(TableNames.TRAINING_REQUIREMENTS).AddColumn("IsInitialRequirement").AsBoolean().Nullable();

            Alter.Table(TableNames.TRAINING_MODULES).AddColumn("FieldOperations").AsBoolean().Nullable();
            Alter.Table(TableNames.TRAINING_MODULES).AddColumn("DOT_Requirement").AsBoolean().Nullable();
            Alter.Table(TableNames.TRAINING_MODULES).AddColumn("OSHA_Requirement").AsBoolean().Nullable();
            Alter.Table(TableNames.TRAINING_MODULES).AddColumn("DPCC_Requirement").AsBoolean().Nullable();
            Alter.Table(TableNames.TRAINING_MODULES).AddColumn("PSM_TCPA_Requirement").AsBoolean().Nullable();
            Alter.Table(TableNames.TRAINING_MODULES).AddColumn("RecurringTrainingFrequencyDays").AsInt32().Nullable();

            Create.Table(TableNames.TRAINING_MODULES_TRAINING_REQUIREMENTS)
                  .WithColumn("TrainingModuleId").AsInt32().NotNullable()
                  .ForeignKey("FK_TrainingModulesTrainingRequirements_tblTrainingModules_TrainingModuleId",
                       "tblTrainingModules", "TrainingModuleId")
                  .WithColumn("TrainingRequirementId").AsInt32().NotNullable()
                  .ForeignKey("FK_TrainingModulesTrainingRequirements_TrainingRequirements_TrainingRequirementId",
                       "TrainingRequirements", "Id");

            Delete.Table(TableNames.POSITION_GROUP_COMMON_NAMES_TRAINING_REQUIREMENTS);

            Create.Table("PositionGroupCommonNamesTrainingModules")
                  .WithColumn("PositionGroupCommonNameId").AsInt32().NotNullable()
                  .ForeignKey(
                       "FK_PositionGroupCommonNamesTrainingModules_PositionGroupCommonNames_PositionGroupCommonNameId",
                       "PositionGroupCommonNames", "Id")
                  .WithColumn("TrainingModuleId").AsInt32().NotNullable()
                  .ForeignKey("FK_PositionGroupCommonNamesTrainingModules_tblTrainingModules_TrainingModuleId",
                       "tblTrainingModules", "TrainingModuleId");

            Alter.Table(TableNames.TRAINING_MODULES)
                 .AddColumn(ColumnNames.INITIAL_TRAINING_REQUIREMENT).AsBoolean().Nullable()
                 .AddColumn(ColumnNames.RECURRING_TRAINING_REQUIREMENT).AsBoolean().Nullable();

            Execute.Sql(Sql.ROLLBACK_TRAINING_MODULES);

            Delete.ForeignKeyColumn(
                TableNames.TRAINING_REQUIREMENTS,
                ColumnNames.ACTIVE_INITIAL_TRAINING_MODULE,
                TableNames.TRAINING_MODULES,
                ColumnNames.TRAINING_MODULE_ID);
            Delete.ForeignKeyColumn(
                TableNames.TRAINING_REQUIREMENTS,
                ColumnNames.ACTIVE_RECURRING_TRAINING_MODULE,
                TableNames.TRAINING_MODULES,
                ColumnNames.TRAINING_MODULE_ID);
            Delete.ForeignKeyColumn(
                TableNames.TRAINING_REQUIREMENTS,
                ColumnNames.ACTIVE_INITIAL_AND_RECURRING_TRAINING_MODULE,
                TableNames.TRAINING_MODULES,
                ColumnNames.TRAINING_MODULE_ID);
            Delete.ForeignKeyColumn(
                TableNames.TRAINING_MODULES,
                ColumnNames.TRAINING_REQUIREMENT_ID,
                TableNames.TRAINING_REQUIREMENTS);
            Delete.ForeignKeyColumn(
                TableNames.TRAINING_MODULES,
                ColumnNames.TRAINING_MODULE_RECURRANT_TYPE_ID,
                TableNames.TRAINING_MODULE_RECURRANT_TYPES);
            Delete.Table(TableNames.TRAINING_MODULE_RECURRANT_TYPES);
        }
    }
}
