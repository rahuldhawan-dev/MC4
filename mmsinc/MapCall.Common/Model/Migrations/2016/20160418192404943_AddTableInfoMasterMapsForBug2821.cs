using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160418192404943), Tags("Production")]
    public class AddTableInfoMasterMapsForBug2821 : Migration
    {
        public override void Up()
        {
            Create.Table("InfoMasterMaps")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID")
                  .WithColumn("MapId").AsFixedLengthString(32).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("InfoMasterMaps");
        }
    }
}
