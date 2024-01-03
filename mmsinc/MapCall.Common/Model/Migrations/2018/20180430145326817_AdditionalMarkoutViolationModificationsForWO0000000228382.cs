using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180430145326817), Tags("Production")]
    public class AdditionalMarkoutViolationModificationsForWO0000000228382 : Migration
    {
        public override void Up()
        {
            Alter.Table("MarkoutViolations").AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MarkoutViolations", "CoordinateId", "Coordinates", "CoordinateId");
        }
    }
}
