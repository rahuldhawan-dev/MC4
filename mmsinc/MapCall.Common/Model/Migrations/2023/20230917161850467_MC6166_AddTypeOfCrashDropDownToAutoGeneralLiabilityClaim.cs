using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230917161850467), Tags("Production")]
    public class Mc6166AddTypeOfCrashDropDownToAutoGeneralLiabilityClaim : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("CrashTypes", "Rear-end",
                "Sideswipe", "Frontal", "Side", "Other");
            Alter.Table("GeneralLiabilityClaims")
                 .AddForeignKeyColumn("CrashTypeId", "CrashTypes")
                 .AddColumn("OtherTypeOfCrash").AsString(255).Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("GeneralLiabilityClaims", "CrashTypeId", "CrashTypes");
            Delete.Table("CrashTypes");
            Delete.Column("OtherTypeOfCrash").FromTable("GeneralLiabilityClaims");
        }
    }
}

