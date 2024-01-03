using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160405135654455), Tags("Production")]
    public class AddInServiceDateToRPLargeServicesForBug2863 : Migration
    {
        public override void Up()
        {
            Alter.Table("LargeServiceProjects").AddColumn("InServiceDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("InServiceDate").FromTable("LargeServiceProjects");
        }
    }
}
