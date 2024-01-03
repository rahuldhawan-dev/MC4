using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230222145359721), Tags("Production")]
    public class MC5338_AddClaimTypeAndWitnessStatementFieldsToGeneralLiabilityClaims : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("GeneralLiabilityClaimTypes", "Preventable", "Non-Preventable");
            Alter.Table("GeneralLiabilityClaims")
                 .AddForeignKeyColumn("GeneralLiabilityClaimTypeId", "GeneralLiabilityClaimTypes")
                 .AddColumn("WitnessStatement").AsBoolean().Nullable();
        }
        public override void Down()
        {
            Delete.ForeignKeyColumn("GeneralLiabilityClaims", "GeneralLiabilityClaimTypeId", "GeneralLiabilityClaimTypes");
            Delete.Table("GeneralLiabilityClaimTypes");
            Delete.Column("WitnessStatement").FromTable("GeneralLiabilityClaims");
        }
    }
}

