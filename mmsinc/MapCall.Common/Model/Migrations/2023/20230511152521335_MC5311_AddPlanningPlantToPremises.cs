using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230511152521335), Tags("Production")]
    public class MC5311_AddPlanningPlantToPremises : AutoReversingMigration
    {
        public override void Up()
        {
            Create.ForeignKeyColumn("Premises", "PlanningPlantId", "PlanningPlants");
        }
    }
}

