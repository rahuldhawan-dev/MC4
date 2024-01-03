using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210407114701801), Tags("Production")]
    public class MC2487MakeEquipmentNullable : Migration
    {
        public override void Up()
        {
            Alter.Column("EquipmentId").OnTable("TankInspections").AsInt32().Nullable();
        }

        public override void Down()
        {
            Alter.Column("EquipmentId").OnTable("TankInspections").AsInt32().NotNullable();
        }
    }
}