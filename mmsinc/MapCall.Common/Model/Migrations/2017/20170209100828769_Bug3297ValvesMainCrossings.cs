using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170209100828769), Tags("Production")]
    public class Bug3297ValvesMainCrossings : Migration
    {
        public override void Up()
        {
            Create.Column("ControlsCrossing").OnTable("Valves")
                  .AsBoolean().NotNullable().WithDefaultValue(false);

            Create.Table("MainCrossingsValves")
                  .WithColumn("ValveId").AsInt32().NotNullable()
                  .ForeignKey("FK_MainCrossingsValves_Valves_ValveId", "Valves", "Id")
                  .WithColumn("MainCrossingId").AsInt32().NotNullable()
                  .ForeignKey("FK_MainCrossingsValves_MainCrossings_MainCrossingId", "MainCrossings", "MainCrossingId");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_MainCrossingsValves_Valves_ValveId").OnTable("MainCrossingsValves");
            Delete.ForeignKey("FK_MainCrossingsValves_MainCrossings_MainCrossingId").OnTable("MainCrossingsValves");
            Delete.Table("MainCrossingsValves");
            Delete.Column("ControlsCrossing").FromTable("Valves");
        }
    }
}
