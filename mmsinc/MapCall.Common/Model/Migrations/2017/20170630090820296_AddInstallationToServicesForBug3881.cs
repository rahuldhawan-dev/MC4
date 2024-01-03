using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170630090820296), Tags("Production")]
    public class AddInstallationToServicesForBug3881 : Migration
    {
        public override void Up()
        {
            Alter.Table("Services").AddColumn("Installation").AsAnsiString(10).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Installation").FromTable("Services");
        }
    }
}
