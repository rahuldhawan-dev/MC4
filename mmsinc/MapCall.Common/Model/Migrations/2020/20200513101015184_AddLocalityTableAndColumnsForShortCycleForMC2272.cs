using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200513101015184), Tags("Production")]
    public class AddLocalityTableAndColumnsForShortCycleForMC2272 : Migration
    {
        public override void Up()
        {
            Create.Table("Localities")
                  .WithIdentityColumn()
                  .WithColumn("Code").AsAnsiString(4).NotNullable()
                  .WithColumn("Description").AsAnsiString(30).NotNullable();
            Alter.Table("ShortCycleWorkOrders")
                 .AddForeignKeyColumn("LocalityId", "Localities");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ShortCycleWorkOrders", "LocalityId", "Localities");
            Delete.Table("Localities");
        }
    }
}
