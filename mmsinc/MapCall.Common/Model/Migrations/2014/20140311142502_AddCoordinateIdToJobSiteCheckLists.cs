using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140311142502), Tags("Production")]
    public class AddCoordinateIdToJobSiteCheckLists : Migration
    {
        public override void Up()
        {
            Alter.Table("JobSiteCheckLists")
                 .AddColumn("CoordinateId")
                 .AsInt32()
                 .Nullable()
                 .ForeignKey("FK_JobSiteCheckLists_Coordinates_CoordinateId", "Coordinates", "CoordinateID");

            // This is a non-nullable field but we need to give the existing rows some coordinate data.
            // This is happening before there's ever real data in the database so it doesn't matter.
            // Live rollout of this won't have data.

            Execute.Sql("UPDATE [JobSiteCheckLists] SET CoordinateId = (select top 1 CoordinateId from Coordinates)");

            Alter.Column("CoordinateId")
                 .OnTable("JobSiteCheckLists")
                 .AsInt32()
                 .NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_JobSiteCheckLists_Coordinates_CoordinateId").OnTable("JobSiteCheckLists");
            Delete.Column("CoordinateId").FromTable("JobSiteCheckLists");
        }
    }
}
