using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230215233010466), Tags("Production")]
    public class MC5325_AddDefaultMaintenancePlanProductionWorkDescription : Migration
    {
        private const string DESCRIPTION = "Maintenance Plan";
        private const int ROUTINE_ORDER_TYPE = 5;

        public override void Up()
        {
            Execute.Sql($@"
                IF NOT EXISTS(SELECT * FROM ProductionWorkDescriptions WHERE Description = '{DESCRIPTION}' AND OrderTypeId = {ROUTINE_ORDER_TYPE})
                BEGIN
                    INSERT INTO ProductionWorkDescriptions([Description], ProductionSkillSetId, OrderTypeId) VALUES ('{DESCRIPTION}', NULL, {ROUTINE_ORDER_TYPE});
                END");
        }

        public override void Down() { }
    }
}
