using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210112111101844), Tags("Production")]
    public class MC2487CreateTankInspectionTable : Migration
    {
        public override void Up()
        {
            //Lookup tables
            this.CreateLookupTableWithValues("TankStructureTypes", "GroundTank/Standpipe", "Reservoir/Clearwell", "Elevated");
            this.CreateLookupTableWithValues("TankInspectionTypes", 50, "Routine/Standard", "Inspection Site Observation", "Comprehensive", "Warranty");
            this.CreateLookupTableWithValues("TankInspectionQuestionGroups", 100, "Security and Vandalism", "General Site Info", "Foundation", "Exterior Condition", "Exterior Roof ", "Overflow Piping, Vaults, Accessory Bldgs. and Valves");

            //General Tab
            Create.Table("TankInspections")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders", "Id").NotNullable()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID").NotNullable()
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentId").NotNullable()
                  .WithForeignKeyColumn("TankObservedById", "tblEmployee", "tblEmployeeId").NotNullable()
                  .WithForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies", "Id").Nullable()
                  .WithForeignKeyColumn("PublicWaterSupplyPressureZoneId", "PublicWaterSupplyPressureZones", "Id").Nullable()
                  .WithColumn("TankCapacity").AsDecimal().NotNullable() 
                  .WithColumn("TankAddress").AsAnsiString(int.MaxValue).Nullable()
                  .WithForeignKeyColumn("TownId", "Towns", "TownId").NotNullable()
                  .WithColumn("City").AsAnsiString(int.MaxValue).Nullable()
                  .WithForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateId").NotNullable()
                  .WithColumn("ZipCode").AsAnsiString(12).Nullable()
                  .WithColumn("ObservationDate").AsDateTime().Nullable()
                  .WithColumn("LastObserved").AsDateTime().Nullable()
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId").NotNullable()
                  .WithForeignKeyColumn("TankStructureTypeId", "TankStructureTypes").Nullable()
                  .WithForeignKeyColumn("TankInspectionTypeId", "TankInspectionTypes").Nullable();

            //Links Question Group Lookup With Question SubGroup Lookup table
            Create.Table("TankInspectionQuestionTypes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(255).NotNullable()
                  .WithForeignKeyColumn("GroupId", "TankInspectionQuestionGroups", nullable: false);

            Insert.IntoTable("TankInspectionQuestionTypes")
                  .Row(new { Description = "Signs of trespassing or vandalism", GroupId = 1 })
                  .Row(new { Description = "Fence secure with gate secure and locked", GroupId = 1 })
                  .Row(new { Description = "Ladder and vandal deterrent secured and locked", GroupId = 1 })
                  .Row(new { Description = "Hatches are sealed and locked", GroupId = 1 })
                  .Row(new { Description = "Lighting working and adequate", GroupId = 1 })
                  .Row(new { Description = "Signage installed and legible", GroupId = 1 })
                  .Row(new { Description = "Buildings on site, doors, and windows secured and locked", GroupId = 1 })
                   //group2
                  .Row(new { Description = "Debris (limbs, brush, fallen trees, animal carcasses, etc.) around site", GroupId = 2 })
                  .Row(new { Description = "Areas of standing water around tank? Site drainage affecting tank, vault, etc.?", GroupId = 2 })
                  .Row(new { Description = "Areas of erosion, animal burrowing, etc.", GroupId = 2 })
                  .Row(new { Description = "Grounds maintained (grass cut, no large overgrowth along fence line, tank bottom, etc.)", GroupId = 2 })
                   //group 3
                  .Row(new { Description = "Signs of settlement", GroupId = 3 })
                  .Row(new { Description = "Signs of crackling, shifting or spalling (grouting or patching necessary?)", GroupId = 3 })
                  .Row(new { Description = "All nuts, bolts, or other appurtenances at the foundation intact, tight, and rust free", GroupId = 3 })
                  .Row(new { Description = "Extensive exposed aggregate or reinforcement", GroupId = 3 })
                   //group 4
                  .Row(new { Description = "Any active leaks", GroupId = 4 })
                  .Row(new { Description = "Visible cracks, holes, buckles, pitting or dents anywhere on tank", GroupId = 4 })
                  .Row(new { Description = "Any rust spots, streaks, or stains; any vegetative growth", GroupId = 4 })
                  .Row(new { Description = "Coating showing any blisters, chips, or cracks", GroupId = 4 })
                  .Row(new { Description = "Ladders in good shape with rungs intact; vandal deterrent installed and locked", GroupId = 4 })
                  .Row(new { Description = "Other damage to the structure", GroupId = 4 })
                  .Row(new { Description = "Loose cables or wires (if applicable)", GroupId = 4 })
                   //group 5
                  .Row(new { Description = "Holes, openings, pitting, depressions, dents, or ponding water", GroupId = 5 })
                  .Row(new { Description = "Coating showing any blisters, chips, or cracks", GroupId = 5 })
                  .Row(new { Description = "Rust spots or stains", GroupId = 5 })
                  .Row(new { Description = "Overhanging tree limbs", GroupId = 5 })
                  .Row(new { Description = "Antenna damage", GroupId = 5 })
                  .Row(new { Description = "Other damage to the structure", GroupId = 5 })
                  .Row(new { Description = "Condition of manholes", GroupId = 5 })
                  .Row(new { Description = "Condition of vents and vent screens; vents have no visible obstructions", GroupId = 5 })
                  .Row(new { Description = "Condition of balcony, platforms, railings, ladders, etc.", GroupId = 5 })
                   //group 6
                  .Row(new { Description = "Condition of underground chamber or vault", GroupId = 6 })
                  .Row(new { Description = "Manhole hatches and lids closed and locked", GroupId = 6 })
                  .Row(new { Description = "Condition of any buildings on site", GroupId = 6 })
                  .Row(new { Description = "Overflow pipe secured with proper screening", GroupId = 6 })
                  .Row(new { Description = "Condition of overflow screen, Tideflex check valve, hinged flap, etc.", GroupId = 6 })
                  .Row(new { Description = "Valves on tank site accessible and exercised (especially tank shut off valve)", GroupId = 6 });
            Create.Table("TankInspectionQuestions")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("TankInspectionId", "TankInspections", nullable: false)
                  .WithForeignKeyColumn("TankInspectionQuestionTypeId", "TankInspectionQuestionTypes", nullable: false)
                  .WithColumn("ObservationAndComments").AsAnsiString(int.MaxValue).Nullable()
                  .WithColumn("RepairsNeeded").AsBoolean().Nullable()
                  .WithColumn("CorrectiveWoDateCreated").AsDateTime().Nullable()
                  .WithColumn("CorrectiveWoDateCompleted").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("TankInspectionQuestions", "TankInspectionId", "TankInspections", "Id");
            Delete.ForeignKeyColumn("TankInspectionQuestions", "TankInspectionQuestionTypeId", "TankInspectionQuestionTypes", "Id");
            Delete.ForeignKeyColumn("TankInspectionQuestionTypes", "GroupId", "TankInspectionQuestionGroups", "Id");
            Delete.ForeignKeyColumn("TankInspections", "ProductionWorkOrderId", "ProductionWorkOrders", "Id");
            Delete.ForeignKeyColumn("TankInspections", "OperatingCenterId", "OperatingCenters", "OperatingCenterID");
            Delete.ForeignKeyColumn("TankInspections", "EquipmentId", "Equipment", "EquipmentId");
            Delete.ForeignKeyColumn("TankInspections", "TankObservedById", "tblEmployee", "tblEmployeeId");
            Delete.ForeignKeyColumn("TankInspections", "TownId", "Towns", "TownId");
            Delete.ForeignKeyColumn("TankInspections", "CoordinateId", "Coordinates", "CoordinateId");
            Delete.ForeignKeyColumn("TankInspections", "FacilityId", "tblFacilities", "RecordId");
            Delete.ForeignKeyColumn("TankInspections", "TankStructureTypeId", "TankStructureTypes", "Id");
            Delete.ForeignKeyColumn("TankInspections", "TankInspectionTypeId", "TankInspectionTypes", "Id");
            Delete.ForeignKeyColumn("TankInspections", "PublicWaterSupplyPressureZoneId", "PublicWaterSupplyPressureZones", "Id");
            Delete.Table("TankInspections");
            Delete.Table("TankStructureTypes");
            Delete.Table("TankInspectionTypes");
            Delete.Table("TankInspectionQuestionGroups");
            Delete.Table("TankInspectionQuestionTypes");
            Delete.Table("TankInspectionQuestions");
        }
    }
}