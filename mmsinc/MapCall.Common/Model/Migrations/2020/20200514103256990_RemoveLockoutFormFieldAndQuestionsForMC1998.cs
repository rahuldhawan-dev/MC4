using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200514103256990), Tags("Production")]
    public class RemoveLockoutFormFieldAndQuestionsForMC1998 : Migration
    {
        public override void Up()
        {
            Delete.Column("EquipmentIdOther").FromTable("LockoutForms");

            // Disable these questions
            Execute.Sql("UPDATE LockoutFormQuestions SET IsActive = 0 WHERE CategoryId = 4");
        }

        public override void Down()
        {
            Alter.Table("LockoutForms").AddColumn("EquipmentIdOther").AsAnsiString(50).Nullable();
            Execute.Sql("UPDATE LockoutFormQuestions SET IsActive = 1 WHERE CategoryId = 4");
        }
    }
}
