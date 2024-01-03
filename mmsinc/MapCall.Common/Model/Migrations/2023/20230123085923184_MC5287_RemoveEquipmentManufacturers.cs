using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230123085923184), Tags("Production")]
    public class MC5287_RemoveEquipmentManufacturers : Migration
    {
        public override void Up()
        {
            Delete.ForeignKeyColumn("Equipment", "ManufacturerID", "EquipmentManufacturers");
            Delete.Table("EquipmentManufacturers");
        }

        public override void Down() { }
    }
}
