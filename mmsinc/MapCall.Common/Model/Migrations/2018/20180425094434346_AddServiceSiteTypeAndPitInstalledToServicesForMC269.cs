using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180425094434346), Tags("Production")]
    public class AddServiceSideTypeAndPitInstalledToServicesForMC269 : Migration
    {
        public override void Up()
        {
            Create.LookupTable("ServiceSideTypes", 15);
            Alter.Table("Services")
                 .AddColumn("PitInstalled").AsBoolean().Nullable()
                 .AddForeignKeyColumn("ServiceSideTypeId", "ServiceSideTypes");

            Insert.IntoTable("ServiceSideTypes")
                  .Rows(new {Description = "Short Side"},
                       new {Description = "Long Side"});
        }

        public override void Down()
        {
            Delete.Column("PitInstalled").FromTable("Services");
            Delete.Column("ServiceSideTypeId").FromTable("Services");
            Delete.Table("ServiceSideTypes");
        }
    }
}
