using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190711092659401), Tags("Production")]
    public class AddLegacyIdToServicesForMC1046 : Migration
    {
        public struct StringLengths
        {
            public const int LEGACY_ID = 15;
        }

        public override void Up()
        {
            Alter.Table("Services").AddColumn("LegacyId").AsString(StringLengths.LEGACY_ID).Nullable();
        }

        public override void Down()
        {
            Delete.Column("LegacyId").FromTable("Services");
        }
    }
}
