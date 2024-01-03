using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151026133501952), Tags("Production")]
    public class AddInspectionTypeToMainCrossingsForBug2684 : Migration
    {
        public const string TABLE_NAME = "MainCrossingInspectionTypes";

        public override void Up()
        {
            Create.LookupTable(TABLE_NAME);

            Alter.Table("MainCrossings")
                 .AddForeignKeyColumn("InspectionTypeId", TABLE_NAME);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MainCrossings", "InspectionTypeId", TABLE_NAME);

            Delete.Table(TABLE_NAME);
        }
    }
}
