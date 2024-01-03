using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Model.Migrations._2016;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211221175447735), Tags("Production")]
    public class MC4059AddingFacilityAreaInspectionRatingToApcInspectionItems : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("FacilityInspectionAreaTypes", "ENTIRE FACILITY", "GROUNDS", "LAB", "GARAGE",
                "ADMIN AREA", "SHOP AREA", "CHEMICAL FEED/STORAGE AREA", "TREATMENT AREA", "OTHER");

            this.CreateLookupTableWithValues("FacilityInspectionRatingTypes", "SATISFACTORY", "ACTION REQUIRED");

            Alter.Table("APCInspectionItems")
                 .AddColumn("DateInspected").AsDateTime().Nullable()
                 .AddColumn("Score").AsInt32().Nullable()
                 .AddColumn("Percentage").AsAnsiString().Nullable()
                 .AddForeignKeyColumn("FacilityInspectionAreaTypeId", "FacilityInspectionAreaTypes")
                 .AddForeignKeyColumn("FacilityInspectionRatingTypeId", "FacilityInspectionRatingTypes");
        }

        public override void Down()
        {
            Delete.Column("DateInspected").FromTable("APCInspectionItems");
            Delete.Column("Score").FromTable("APCInspectionItems");
            Delete.Column("Percentage").FromTable("APCInspectionItems");
            Delete.ForeignKeyColumn("APCInspectionItems", "FacilityInspectionAreaTypeId", "FacilityInspectionAreaTypes");
            Delete.ForeignKeyColumn("APCInspectionItems", "FacilityInspectionRatingTypeId", "FacilityInspectionRatingTypes");

            Delete.Table("FacilityInspectionAreaTypes");
            Delete.Table("FacilityInspectionRatingTypes");
        }
    }
}

