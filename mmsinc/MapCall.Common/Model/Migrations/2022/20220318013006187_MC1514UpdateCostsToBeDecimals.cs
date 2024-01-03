using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220318013006187), Tags("Production")]
    public class MC1514UpdateCostsToBeDecimals : Migration
    {
        public override void Up()
        {
            Alter.Column("InterimMitigationMeasuresTakenEstimatedCosts")
                 .OnTable("HighRiskAssets")
                 .AsDecimal(19, 2)
                 .Nullable();

            Alter.Column("FinalMitigationMeasuresTakenEstimatedCosts")
                 .OnTable("HighRiskAssets")
                 .AsDecimal(19, 2)
                 .Nullable();
        }

        public override void Down()
        {
            Alter.Column("InterimMitigationMeasuresTakenEstimatedCosts")
                 .OnTable("HighRiskAssets")
                 .AsCurrency()
                 .Nullable();

            Alter.Column("FinalMitigationMeasuresTakenEstimatedCosts")
                 .OnTable("HighRiskAssets")
                 .AsCurrency()
                 .Nullable();
        }
    }
}

