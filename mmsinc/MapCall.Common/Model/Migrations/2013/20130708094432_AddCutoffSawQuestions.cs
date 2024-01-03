using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130708094432), Tags("Production")]
    public class AddCutoffSawQuestions : Migration
    {
        #region Constants

        public struct Sql { }

        public struct Tables
        {
            public const string CUTOFF_SAW_QUESTIONS = "CutoffSawQuestions",
                                CUTOFF_SAW_QUESTIONNAIRES = "CutoffSawQuestionnaires",
                                CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS =
                                    "CutoffSawQuestionnairesCutoffSawQuestions";
        }

        public struct Columns
        {
            public const string CUTOFF_SAW_QUESTION_ID = "CutoffSawQuestionID",
                                CUTOFF_SAW_QUESTIONNAIRE_ID = "CutoffSawQuestionnaireID",
                                WORK_ORDER_ID = "WorkOrderID",
                                //EMPLOYEE_ID = "RecID",
                                WORK_ORDER_SAP = "WorkOrderSAP",
                                LEAD_PERSON = "LeadPersonID",
                                SAW_OPERATOR = "SawOperatorID",
                                OPERATED_ON = "OperatedOn",
                                PIPE_MATERIAL_ID = "PipeMaterialID",
                                PIPE_DIAMETER_ID = "PipeDiameterID",
                                COMMENTS = "Comments",
                                CREATED_ON = "CreatedOn",
                                CREATED_BY = "CreatedBy",
                                QUESTION = "Question",
                                IS_ACTIVE = "IsActive",
                                SOT_ORDER = "SortOrder";
        }

        public struct StringLengths
        {
            public const int WORK_ORDER_SAP = 50,
                             CREATED_BY = 50;
        }

        public struct ForeignKeys
        {
            public const string FK_CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONNAIRE_ID =
                                    "FK_CutoffSawQuestionnairesCutoffSawQuestions_CutoffSawQuestionnaires_CutoffSawQuestionnaireID",
                                FK_CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS_CUTOFF_SAW_QUESTION_ID =
                                    "FK_CutoffSawQuestionnairesCutoffSawQuestions_CutoffSawQuestions_CutoffSawQuestionID";
        }

        #endregion

        public override void Up()
        {
            #region Add Tables

            Create.Table(Tables.CUTOFF_SAW_QUESTIONS)
                  .WithColumn(Columns.CUTOFF_SAW_QUESTION_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.QUESTION).AsCustom("Text").NotNullable()
                  .WithColumn(Columns.SOT_ORDER).AsInt32().Nullable()
                  .WithColumn(Columns.IS_ACTIVE).AsBoolean().NotNullable();

            Create.Table(Tables.CUTOFF_SAW_QUESTIONNAIRES)
                  .WithColumn(Columns.CUTOFF_SAW_QUESTIONNAIRE_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.WORK_ORDER_ID).AsInt32().Nullable()
                  .WithColumn(Columns.WORK_ORDER_SAP).AsAnsiString(StringLengths.WORK_ORDER_SAP).Nullable()
                  .WithColumn(Columns.LEAD_PERSON).AsInt32().NotNullable()
                  .WithColumn(Columns.SAW_OPERATOR).AsInt32().NotNullable()
                  .WithColumn(Columns.OPERATED_ON).AsDateTime().NotNullable()
                  .WithColumn(Columns.PIPE_MATERIAL_ID).AsInt32().Nullable()
                  .WithColumn(Columns.PIPE_DIAMETER_ID).AsInt32().Nullable()
                  .WithColumn(Columns.COMMENTS).AsCustom("text").Nullable()
                  .WithColumn(Columns.CREATED_BY).AsAnsiString(StringLengths.CREATED_BY).NotNullable()
                  .WithColumn(Columns.CREATED_ON).AsDateTime().NotNullable();

            Create.Table(Tables.CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS)
                  .WithColumn(Columns.CUTOFF_SAW_QUESTIONNAIRE_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.CUTOFF_SAW_QUESTION_ID).AsInt32().NotNullable();

            #endregion

            #region Foreign Keys

            Create.ForeignKey(
                       ForeignKeys.FK_CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONNAIRE_ID)
                  .FromTable(Tables.CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS)
                  .ForeignColumn(Columns.CUTOFF_SAW_QUESTIONNAIRE_ID)
                  .ToTable(Tables.CUTOFF_SAW_QUESTIONNAIRES).PrimaryColumn(Columns.CUTOFF_SAW_QUESTIONNAIRE_ID);
            Create.ForeignKey(
                       ForeignKeys.FK_CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS_CUTOFF_SAW_QUESTION_ID)
                  .FromTable(Tables.CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS)
                  .ForeignColumn(Columns.CUTOFF_SAW_QUESTION_ID)
                  .ToTable(Tables.CUTOFF_SAW_QUESTIONS).PrimaryColumn(Columns.CUTOFF_SAW_QUESTION_ID);

            #endregion
        }

        public override void Down()
        {
            #region Foreign Keys

            Delete.ForeignKey(
                       ForeignKeys.FK_CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONNAIRE_ID)
                  .OnTable(Tables.CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS);
            Delete.ForeignKey(
                       ForeignKeys.FK_CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS_CUTOFF_SAW_QUESTION_ID)
                  .OnTable(Tables.CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS);

            #endregion

            #region Drop Tables

            Delete.Table(Tables.CUTOFF_SAW_QUESTIONS);
            Delete.Table(Tables.CUTOFF_SAW_QUESTIONNAIRES);
            Delete.Table(Tables.CUTOFF_SAW_QUESTIONNAIRES_CUTOFF_SAW_QUESTIONS);

            #endregion
        }
    }
}
