using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20160126141008870), Tags("Production")]
    public class AddSOPTablesForBug2662 : Migration
    {
        public const string THE_WORST_TABLE_NAME_EVER = "StandardOperatingProcedurePositionGroupCommonNameRequirements";
        public const int TOKEN_ACTION_LENGTH = 75;

        public override void Up()
        {
            // Need to increase the length of the Action column due to overly long action names.
            Alter.Column("Action").OnTable("SecureFormTokens")
                 .AsAnsiString(TOKEN_ACTION_LENGTH)
                 .NotNullable();

            Create.Table("StandardOperatingProcedureQuestions")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("Question").AsCustom("ntext").NotNullable()
                  .WithColumn("Answer").AsCustom("ntext").NotNullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable()
                  .WithColumn("StandardOperatingProcedureId").AsInt32().NotNullable()
                  .ForeignKey("FK_StandardOperatingProcedureQuestions_tblSOP_StandardOperatingProcedureId", "tblSOP",
                       "SOP_ID");

            Create.Table("StandardOperatingProcedureReviews")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("StandardOperatingProcedureId").AsInt32().NotNullable()
                  .ForeignKey("FK_StandardOperatingProcedureReviews_tblSOP_StandardOperatingProcedureId", "tblSOP",
                       "SOP_ID")
                  .WithColumn("AnsweredByUserId").AsInt32().NotNullable()
                  .ForeignKey("FK_StandardOperatingProcedureReviews_tblPermissions_AnsweredByUserId", "tblPermissions",
                       "RecID")
                  .WithColumn("AnsweredAt").AsDateTime().NotNullable()
                  .WithColumn("ReviewedByUserId").AsInt32().Nullable()
                  .ForeignKey("FK_StandardOperatingProcedureReviews_tblPermissions_ReviewedByUserId", "tblPermissions",
                       "RecID")
                  .WithColumn("ReviewedAt").AsDateTime().Nullable();

            Create.Table("StandardOperatingProcedureReviewAnswers")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("StandardOperatingProcedureReviewId").AsInt32().NotNullable()
                  .ForeignKey(
                       "FK_StandardOperatingProcedureReviewAnswers_StandardOperatingProcedureReviews_StandardOperatingProcedureReviewId",
                       "StandardOperatingProcedureReviews", "Id")
                  .WithColumn("StandardOperatingProcedureQuestionId").AsInt32().NotNullable()
                  .ForeignKey(
                       "FK_StandardOperatingProcedureReviewAnswers_StandardOperatingProcedureQuestions_StandardOperatingProcedureQuestionId",
                       "StandardOperatingProcedureQuestions", "Id")
                  .WithColumn("Answer").AsCustom("ntext").NotNullable()
                  .WithColumn("IsCorrect").AsBoolean().Nullable();

            Create.Table(THE_WORST_TABLE_NAME_EVER)
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("Frequency").AsInt32().NotNullable()
                  .WithColumn("FrequencyUnitId").AsInt32().NotNullable()
                  .ForeignKey("FK_" + THE_WORST_TABLE_NAME_EVER + "_RecurringFrequencyUnits_FrequencyUnitId",
                       "RecurringFrequencyUnits", "Id")
                  .WithColumn("StandardOperatingProcedureId").AsInt32().NotNullable()
                  .ForeignKey("FK_" + THE_WORST_TABLE_NAME_EVER + "_tblSOP_StandardOperatingProcedureId", "tblSOP",
                       "SOP_ID")
                  .WithColumn("PositionGroupCommonNameId").AsInt32().NotNullable()
                  .ForeignKey("FK_" + THE_WORST_TABLE_NAME_EVER + "_PositionGroupCommonNames_PositionGroupCommonNameId",
                       "PositionGroupCommonNames", "Id");

            Create.Table("StandardOperatingProceduresTrainingModules")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("StandardOperatingProcedureId").AsInt32().NotNullable()
                  .ForeignKey("FK_StandardOperatingProceduresTrainingModules_tblSOP_StandardOperatingProcedureId",
                       "tblSOP", "SOP_ID")
                  .WithColumn("TrainingModuleId").AsInt32().NotNullable()
                  .ForeignKey("FK_StandardOperatingProceduresTrainingModules_tblTrainingModules_TrainingModuleId",
                       "tblTrainingModules", "TrainingModuleId");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_StandardOperatingProceduresTrainingModules_tblTrainingModules_TrainingModuleId")
                  .OnTable("StandardOperatingProceduresTrainingModules");
            Delete.ForeignKey("FK_StandardOperatingProceduresTrainingModules_tblSOP_StandardOperatingProcedureId")
                  .OnTable("StandardOperatingProceduresTrainingModules");
            Delete.Table("StandardOperatingProceduresTrainingModules");

            Delete.ForeignKey("FK_" + THE_WORST_TABLE_NAME_EVER + "_RecurringFrequencyUnits_FrequencyUnitId")
                  .OnTable(THE_WORST_TABLE_NAME_EVER);
            Delete.ForeignKey("FK_" + THE_WORST_TABLE_NAME_EVER + "_tblSOP_StandardOperatingProcedureId")
                  .OnTable(THE_WORST_TABLE_NAME_EVER);
            Delete.ForeignKey("FK_" + THE_WORST_TABLE_NAME_EVER + "_PositionGroupCommonNames_PositionGroupCommonNameId")
                  .OnTable(THE_WORST_TABLE_NAME_EVER);

            Delete
               .ForeignKey(
                    "FK_StandardOperatingProcedureReviewAnswers_StandardOperatingProcedureQuestions_StandardOperatingProcedureQuestionId")
               .OnTable("StandardOperatingProcedureReviewAnswers");
            Delete
               .ForeignKey(
                    "FK_StandardOperatingProcedureReviewAnswers_StandardOperatingProcedureReviews_StandardOperatingProcedureReviewId")
               .OnTable("StandardOperatingProcedureReviewAnswers");
            Delete.ForeignKey("FK_StandardOperatingProcedureReviews_tblPermissions_AnsweredByUserId")
                  .OnTable("StandardOperatingProcedureReviews");
            Delete.ForeignKey("FK_StandardOperatingProcedureReviews_tblPermissions_ReviewedByUserId")
                  .OnTable("StandardOperatingProcedureReviews");
            Delete.ForeignKey("FK_StandardOperatingProcedureReviews_tblSOP_StandardOperatingProcedureId")
                  .OnTable("StandardOperatingProcedureReviews");
            Delete.ForeignKey("FK_StandardOperatingProcedureQuestions_tblSOP_StandardOperatingProcedureId")
                  .OnTable("StandardOperatingProcedureQuestions");

            Delete.Table(THE_WORST_TABLE_NAME_EVER);
            Delete.Table("StandardOperatingProcedureReviewAnswers");
            Delete.Table("StandardOperatingProcedureReviews");
            Delete.Table("StandardOperatingProcedureQuestions");
        }
    }
}
