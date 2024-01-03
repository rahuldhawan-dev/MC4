using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210621111401700), Tags("Production")]
    public class MC2487ChangeQuestion : Migration
    {
        public override void Up()
        {
            void UpdateTankInspectionQuestions(string originalQuestion, string newQuestion)
            {
                Execute.Sql(
                    $@"update TankInspectionQuestionTypes set Description = '{newQuestion}' where Description like '{originalQuestion}' ");
            }

            UpdateTankInspectionQuestions("Condition of manholes", "Are the condition of the manholes in acceptable condition?");
        }

        public override void Down()
        {
            void UpdateTankInspectionQuestions(string originalQuestion, string newQuestion)
            {
                Execute.Sql(
                    $@"update TankInspectionQuestionTypes set Description = '{originalQuestion}' where Description like '{newQuestion}' ");
            }

            UpdateTankInspectionQuestions("Condition of manholes", "Are the condition of the manholes in acceptable condition?");
        }
    }
}