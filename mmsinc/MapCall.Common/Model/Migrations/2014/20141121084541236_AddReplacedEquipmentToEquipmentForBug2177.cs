using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141121084541236), Tags("Production")]
    public class AddReplacedEquipmentToEquipmentForBug2177 : Migration
    {
        public const string TABLE_NAME = "Equipment";

        public override void Up()
        {
            Alter.Table(TABLE_NAME).AddForeignKeyColumn("ReplacedEquipmentId", TABLE_NAME, "EquipmentId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(TABLE_NAME, "ReplacedEquipmentId", TABLE_NAME);
        }
    }
}
