using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210506142001700), Tags("Production")]
    public class Mc2487Remove3FieldsAndChangeQuestions : Migration
    {
        public override void Up()
        {
            Delete.ForeignKeyColumn("TankInspections", "TankStructureTypeId", "TankStructureTypes", "Id");
            Delete.ForeignKeyColumn("TankInspections", "PublicWaterSupplyPressureZoneId", "PublicWaterSupplyPressureZones", "Id");

            Delete.Table("TankStructureTypes");
            
            void UpdateTankInspectionQuestions(string originalQuestion, string newQuestion)
            {
                Execute.Sql(
                    $@"update TankInspectionQuestionTypes set Description = '{newQuestion}' where Description like '{originalQuestion}' ");
            }

            UpdateTankInspectionQuestions("Signage installed and legible", "Incorrect or No Signage installed and/or not legible");
            UpdateTankInspectionQuestions("Lighting working and adequate", "Inadequate lighting or lighting not working properly");
            UpdateTankInspectionQuestions("Hatches are sealed and locked", "For storage tank access hatches, are all hatches and lids closed and locked and or bolted securely");
            UpdateTankInspectionQuestions("Condition of balcony, platforms, railings, ladders, etc.", "Are all balcony or platforms, if any, in satisfactory shape (if N/A, please select yes)");
            UpdateTankInspectionQuestions("Condition of underground chamber or vault", "Is the condition of chamber or undergrounds vaults satisfactory");
            UpdateTankInspectionQuestions("Condition of any buildings on site", "Are all buildings on site, if applicable in satisfactory condition");
            UpdateTankInspectionQuestions("Condition of overflow screen, Tideflex check valve, hinged flap, etc.", "Is the condition of all overflow screens, Tideflex check valves, hinge flaps, etc. satisfactory");

            Insert.IntoTable("TankInspectionQuestionTypes")
                  .Row(new {Description = "Is the exterior roof in a satisfactory working condition (i.e. no visible problems)", GroupId = 1});

            Alter.Table("TankInspectionQuestions").AddColumn("TankInspectionAnswer").AsBoolean().Nullable();
        }

        public override void Down()
        {
            this.CreateLookupTableWithValues("TankStructureTypes", "GroundTank/Standpipe", "Reservoir/Clearwell", "Elevated");

            Alter.Table("TankInspections")
                 .AddForeignKeyColumn("TankStructureTypeId", "TankStructureTypes").Nullable()
                 .AddForeignKeyColumn("PublicWaterSupplyPressureZoneId", "PublicWaterSupplyPressureZones", "Id").Nullable();

            void UpdateTankInspectionQuestions(string newQuestion, string originalQuestion)
            {
                Execute.Sql(
                    $@"update TankInspectionQuestionTypes set Description = '{newQuestion}' where Description like '{originalQuestion}' ");
            }

            UpdateTankInspectionQuestions("Signage installed and legible", "Incorrect or No Signage installed and/or not legible");
            UpdateTankInspectionQuestions("Lighting working and adequate", "Inadequate lighting or lighting not working properly");
            UpdateTankInspectionQuestions("Hatches are sealed and locked", "For storage tank access hatches, are all hatches and lids closed and locked and or bolted securely");
            UpdateTankInspectionQuestions("Condition of balcony, platforms, railings, ladders, etc.", "Are all balcony or platforms, if any, in satisfactory shape (if N/A, please select yes)");
            UpdateTankInspectionQuestions("Condition of underground chamber or vault", "Is the condition of chamber or undergrounds vaults satisfactory");
            UpdateTankInspectionQuestions("Condition of any buildings on site", "Are all buildings on site, if applicable in satisfactory condition");
            UpdateTankInspectionQuestions("Condition of overflow screen, Tideflex check valve, hinged flap, etc.", "Is the condition of all overflow screens, Tideflex check valves, hinge flaps, etc. satisfactory");

            Delete.Column("TankInspectionAnswer").FromTable("TankInspectionQuestions");
        }
    }
}