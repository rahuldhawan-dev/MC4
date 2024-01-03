using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170505111700758), Tags("Production")]
    public class MakeProjectDescriptionOnRecurringProjectsATextFieldForBug3795 : Migration
    {
        public override void Up()
        {
            Alter.Table("RecurringProjects").AlterColumn("ProjectDescription").AsText();
        }

        public override void Down() { }
    }
}
