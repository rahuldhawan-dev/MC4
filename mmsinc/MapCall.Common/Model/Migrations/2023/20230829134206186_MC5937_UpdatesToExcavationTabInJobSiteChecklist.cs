using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230829134206186), Tags("Production")]
    public class MC5937_UpdatesToExcavationTabInJobSiteChecklist : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SoilConditionsWithinExcavationTypes", 80, "Not able or can barely penetrate soil",
                "Able to penetrate the soil to the back of the thumbnail with moderate pressure", "Easily penetrate the soil with light pressure");
            this.CreateLookupTableWithValues("SoilCompositionExcavationTypes", "Solid Rock",
                "Clay (sticks together in large clumps)", "Granulated (sand, gravel, fine particles, etc)");

            Alter.Table("JobSiteCheckLists")
                 .AddColumn("SpotterAssigned").AsBoolean().Nullable()
                 .AddColumn("IsManufacturerDataOnSiteForShoringOrShieldingEquipment").AsBoolean().Nullable()
                 .AddColumn("IsTheExcavationGuardedFromAccidentalEntry").AsBoolean().Nullable()
                 .AddForeignKeyColumn("SoilCompositionExcavationTypeId", "SoilCompositionExcavationTypes")
                 .AddForeignKeyColumn("SoilConditionsWithinExcavationTypeId", "SoilConditionsWithinExcavationTypes")
                 .AddColumn("AreThereAnyVisualSignsOfPotentialSoilCollapse").AsBoolean().Nullable()
                 .AddColumn("IsTheExcavationSubjectToVibration").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("JobSiteCheckLists", "SoilConditionsWithinExcavationTypeId", "SoilConditionsWithinExcavationTypes");
            Delete.Table("SoilConditionsWithinExcavationTypes");
            Delete.ForeignKeyColumn("JobSiteCheckLists", "SoilCompositionExcavationTypeId", "SoilCompositionExcavationTypes");
            Delete.Table("SoilCompositionExcavationTypes");
            Delete.Column("SpotterAssigned").FromTable("JobSiteCheckLists");
            Delete.Column("IsManufacturerDataOnSiteForShoringOrShieldingEquipment").FromTable("JobSiteCheckLists");
            Delete.Column("IsTheExcavationGuardedFromAccidentalEntry").FromTable("JobSiteCheckLists");
            Delete.Column("AreThereAnyVisualSignsOfPotentialSoilCollapse").FromTable("JobSiteCheckLists");
            Delete.Column("IsTheExcavationSubjectToVibration").FromTable("JobSiteCheckLists");
        }
    }
}

