using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210510094717662), Tags("Production")]
    public class MC2546RenameWellTestingFormToWellTest : Migration
    {
        public override void Up()
        {
            Rename.Table("WellTestingFormGradeTypes").To("WellTestGradeTypes");
            Rename.Table("WellTestingForms").To("WellTests");
        }

        public override void Down()
        {
            Rename.Table("WellTestGradeTypes").To("WellTestingFormGradeTypes");
            Rename.Table("WellTests").To("WellTestingForms");
        }
    }
}

