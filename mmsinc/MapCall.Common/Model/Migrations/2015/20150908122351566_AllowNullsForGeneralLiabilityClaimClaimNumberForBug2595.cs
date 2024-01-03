using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150908122351566), Tags("Production")]
    public class AllowNullsForGeneralLiabilityClaimClaimNumberForBug2595 : Migration
    {
        public override void Up()
        {
            Alter.Table("GeneralLiabilityClaims").AlterColumn("ClaimNumber")
                 .AsAnsiString(AddTableForGeneralLiabilityFormForBug1983.StringLengths.CLAIM_NUMBER)
                 .Nullable();
        }

        public override void Down()
        {
            //Alter.Table("GeneralLiabilityClaims").AlterColumn("ClaimNumber")
            //    .AsAnsiString(AddTableForGeneralLiabilityFormForBug1983.StringLengths.CLAIM_NUMBER)
            //    .NotNullable();
        }
    }
}
