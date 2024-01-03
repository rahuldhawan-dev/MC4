using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141002100915872), Tags("Production")]
    public class AddCourseNumberToTrainingModuleForBug2010 : Migration
    {
        public const string TABLE_NAME = "tblTrainingModules",
                            COURSE_NUMBER = "AmericanWaterCourseNumber";

        public struct StringLengths
        {
            public const int COURSE_NUMBER = 50;
        }

        public override void Up()
        {
            Alter.Table(TABLE_NAME)
                 .AddColumn(COURSE_NUMBER)
                 .AsAnsiString(StringLengths.COURSE_NUMBER)
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column(COURSE_NUMBER).FromTable(TABLE_NAME);
        }
    }
}
