using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130801093435), Tags("Production")]
    public class MakeDistrictsNullableOnProjectsRP : Migration
    {
        public override void Up()
        {
            Alter.Column("District").OnTable("RPProjects").AsInt32().Nullable();
        }

        public override void Down()
        {
            // no need to set it back to not nullable
        }
    }
}
