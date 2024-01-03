using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180111103946835), Tags("Production")]
    public class WO200633LegacyIdForHydrantsAndValves : Migration
    {
        public struct StringLengths
        {
            public const int LEGACY_ID = 15;
        }

        public override void Up()
        {
            Create.Column("LegacyId").OnTable("Hydrants").AsCustom($"varchar({StringLengths.LEGACY_ID})").Nullable();
            Create.Column("LegacyId").OnTable("Valves").AsCustom($"varchar({StringLengths.LEGACY_ID})").Nullable();

            Execute.Sql("update Hydrants set LegacyId = MapPage");
            Execute.Sql("update Valves set LegacyId = MapPage");
        }

        public override void Down()
        {
            Delete.Column("LegacyId").FromTable("Hydrants");
            Delete.Column("LegacyId").FromTable("Valves");
        }
    }
}
