using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161010213311773), Tags("Production")]
    public class AddCoordinateToEquipmentForBug3004 : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment").AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID").Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Equipment", "CoordinateId", "Coordinates", "CoordinateID");
        }
    }
}
