using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230120083035021), Tags("Production")]
    public class MC3894EnvironmentalPermitsAddPermitNameField : Migration
    {
        public struct SQL
        {
            public const string POPULATE_COLUMN = "Update EnvironmentalPermits Set PermitName = PermitCrossReferenceNumber";
        } 
        public override void Up()
        {
            Create.Column("PermitName").OnTable("EnvironmentalPermits").AsAnsiString(50).Nullable();
            Execute.Sql(SQL.POPULATE_COLUMN);
        }

        public override void Down()
        {
            Delete.Column("PermitName").FromTable("EnvironmentPermits");
        }
    }
}

