using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130212154437), Tags("Production")]
    public class UpdateTrainingModules : Migration
    {
        public override void Up()
        {
            Execute.Sql("UPDATE tblTrainingModules SET IsActive = 1");
        }

        public override void Down() { }
    }
}
