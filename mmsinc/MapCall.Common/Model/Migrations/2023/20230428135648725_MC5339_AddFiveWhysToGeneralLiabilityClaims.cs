using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230428135648725), Tags("Production")]
    public class MC5339_AddFiveWhysToGeneralLiabilityClaims : Migration
    {
        public override void Up()
        {
            Alter.Table("GeneralLiabilityClaims")
                 .AddColumn("Why1").AsString(255).Nullable();
            Alter.Table("GeneralLiabilityClaims")
                 .AddColumn("Why2").AsString(255).Nullable();
            Alter.Table("GeneralLiabilityClaims")
                 .AddColumn("Why3").AsString(255).Nullable();
            Alter.Table("GeneralLiabilityClaims")
                 .AddColumn("Why4").AsString(255).Nullable();
            Alter.Table("GeneralLiabilityClaims")
                 .AddColumn("Why5").AsString(255).Nullable();
            // They want all existing incident records to be set to "False" for this.
            Create.Column("FiveWhysCompleted").OnTable("GeneralLiabilityClaims").AsBoolean().NotNullable().WithDefaultValue(false);
            Alter.Table("GeneralLiabilityClaims")
                 .AddColumn("DateSubmitted").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Why1").FromTable("GeneralLiabilityClaims");
            Delete.Column("Why2").FromTable("GeneralLiabilityClaims");
            Delete.Column("Why3").FromTable("GeneralLiabilityClaims");
            Delete.Column("Why4").FromTable("GeneralLiabilityClaims");
            Delete.Column("Why5").FromTable("GeneralLiabilityClaims");
            Delete.Column("FiveWhysCompleted").FromTable("GeneralLiabilityClaims");
            Delete.Column("DateSubmitted").FromTable("GeneralLiabilityClaims");
        }
    }
}

