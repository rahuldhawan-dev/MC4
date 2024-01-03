using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200915145545498), Tags("Production")]
    public class MC2550AddFacilityAreaAndSubAreas : Migration
    {
        #region Constants

        public const int MAX_DESCRIPTION_LENGTH = 30;

        #endregion

        #region Exposed Methods

        public override void Up()
        {
            this.CreateLookupTableWithValues("FacilityAreas", "Lab", "Chemical", "Electrical", "Pumping", "Filter",
                "Generator", "Basin", "Operational", "Piping", "Vault", "Intake", "Reservoir", "Clarifier");

            Create.Table("FacilitySubAreas")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(MAX_DESCRIPTION_LENGTH).NotNullable()
                  .WithForeignKeyColumn("AreaId", "FacilityAreas", nullable: false);

            Insert.IntoTable("FacilitySubAreas")
                  .Row(new {Description = "Bacteriological", AreaId = 1})
                  .Row(new {Description = "Wet", AreaId = 1})
                  .Row(new {Description = "Control Room", AreaId = 1})
                  .Row(new {Description = "Sodium Hypochlorite", AreaId = 2})
                  .Row(new {Description = "Lime", AreaId = 2})
                  .Row(new {Description = "Chlorine", AreaId = 2})
                  .Row(new {Description = "Sodium", AreaId = 2})
                  .Row(new {Description = "Fluoride", AreaId = 2})
                  .Row(new {Description = "Zinc", AreaId = 2})
                  .Row(new {Description = "PAC", AreaId = 2})
                  .Row(new {Description = "Ferric Chloride", AreaId = 2})
                  .Row(new {Description = "Potassium Permanganate", AreaId = 2})
                  .Row(new {Description = "Ozone", AreaId = 2})
                  .Row(new {Description = "Pacl", AreaId = 2})
                  .Row(new {Description = "Carbon", AreaId = 2})
                  .Row(new {Description = "Ammonia", AreaId = 2})
                  .Row(new {Description = "Zinc Orthophosphate", AreaId = 2})
                  .Row(new {Description = "Main", AreaId = 3})
                  .Row(new {Description = "North", AreaId = 3})
                  .Row(new {Description = "South", AreaId = 3})
                  .Row(new {Description = "Process", AreaId = 3})
                  .Row(new {Description = "High Service", AreaId = 4})
                  .Row(new {Description = "Low Service", AreaId = 4})
                  .Row(new {Description = "Vacuum", AreaId = 4})
                  .Row(new {Description = "Raw Water", AreaId = 4})
                  .Row(new {Description = "Low Lift", AreaId = 4})
                  .Row(new {Description = "Pump Room", AreaId = 4})
                  .Row(new {Description = "Filter Gallery", AreaId = 5})
                  .Row(new {Description = "Emergency Generator", AreaId = 6})
                  .Row(new {Description = "Mixing", AreaId = 7})
                  .Row(new {Description = "Equalization", AreaId = 7})
                  .Row(new {Description = "Sedimentation", AreaId = 7})
                  .Row(new {Description = "Emergency Generator", AreaId = 7})
                  .Row(new {Description = "Admin", AreaId = 8})
                  .Row(new {Description = "Control Room", AreaId = 8})
                  .Row(new {Description = "Operators Booth", AreaId = 8})
                  .Row(new {Description = "Training Room", AreaId = 8})
                  .Row(new {Description = "Filter Pipe Gallery", AreaId = 9})
                  .Row(new {Description = "Pipe Gallery", AreaId = 9})
                  .Row(new {Description = "Flow Meter", AreaId = 10})
                  .Row(new {Description = "Raw Water", AreaId = 11})
                  .Row(new {Description = "Raw Water", AreaId = 12});

            Create.Table("FacilitiesFacilityAreas")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId", nullable: false)
                  .WithForeignKeyColumn("FacilityAreaId", "FacilityAreas", nullable: false)
                  .WithForeignKeyColumn("FacilitySubAreaId", "FacilitySubAreas");
        }

        public override void Down()
        {
            Delete.Table("FacilitiesFacilityAreas");
            Delete.Table("FacilitySubAreas");
            Delete.Table("FacilityAreas");
        }

        #endregion
    }
}
