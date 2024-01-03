using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200520105419300), Tags("Production")]
    public class AddShortCycleDistrictTableAndColumnsForShortCycleForMC2319 : Migration
    {
        public override void Up()
        {
            Create.Table("ShortCycleDistricts")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(40).NotNullable();
            Alter.Table("ShortCycleWorkOrders")
                 .AddForeignKeyColumn("DistrictId", "ShortCycleDistricts");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ShortCycleWorkOrders", "DistrictId", "ShortCycleDistricts");
            Delete.Table("ShortCycleDistricts");
        }
    }
}
