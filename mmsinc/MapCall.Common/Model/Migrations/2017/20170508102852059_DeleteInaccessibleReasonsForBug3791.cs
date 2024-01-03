using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170508102852059), Tags("Production")]
    public class DeleteInaccessibleReasonsForBug3791 : Migration
    {
        public override void Up()
        {
            Delete.ForeignKeyColumn(UpdateValvesForBug2224.TableNames.VALVE_INSPECTIONS_NEW, "InaccessibleId",
                UpdateValvesForBug2224.TableNames.INACCESSIBLE_REASONS);

            Delete.Table(UpdateValvesForBug2224.TableNames.INACCESSIBLE_REASONS);
        }

        public override void Down()
        {
            Create.LookupTable(UpdateValvesForBug2224.TableNames.INACCESSIBLE_REASONS);

            Alter.Table(UpdateValvesForBug2224.TableNames.VALVE_INSPECTIONS_NEW)
                 .AddForeignKeyColumn("InaccessibleId", UpdateValvesForBug2224.TableNames.INACCESSIBLE_REASONS);
        }
    }
}
