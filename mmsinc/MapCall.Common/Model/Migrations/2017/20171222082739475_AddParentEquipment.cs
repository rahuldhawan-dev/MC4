using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20171222082739475), Tags("Production")]
    public class AddParentEquipment : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment")
                 .AddForeignKeyColumn("ParentEquipmentId", "Equipment", "EquipmentID", nullable: true);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Equipment", "ParentEquipmentId", "Equipment", "EquipmentID");
        }
    }
}
