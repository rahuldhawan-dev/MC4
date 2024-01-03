using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131108094653), Tags("Production")]
    public class ChangeFireDistrictPremiseNumberToVarchar : Migration
    {
        public const int PREMISE_NUMBER_LENGTH = 11;

        public override void Up()
        {
            Alter.Column("PremiseNumber").OnTable("FireDistrict").AsAnsiString(PREMISE_NUMBER_LENGTH).Nullable();
        }

        public override void Down()
        {
            // no need to convert back to int
        }
    }
}
