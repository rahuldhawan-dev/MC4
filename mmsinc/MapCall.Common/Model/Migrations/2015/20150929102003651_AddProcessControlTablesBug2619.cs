using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150929102003651), Tags("Production")]
    public class AddProcessControlTablesBug2619 : Migration
    {
        public override void Up()
        {
            // The ProcessStages table is being repurposed and being removed from trainingmodules.
            Delete.ForeignKey("FK_tblTrainingModules_ProcessStages_ProcessStage").OnTable("tblTrainingModules");
            Delete.Column("ProcessStage").FromTable("tblTrainingModules");
            Delete.Column("ProcessStageSequence").FromTable("tblTrainingModules");

            Delete.FromTable("ProcessStages").AllRows();

            this.EnableIdentityInsert("ProcessStages");
            Insert.IntoTable("ProcessStages")
                  .Row(new {ProcessStageID = 1, Description = "Source of Supply"})
                  .Row(new {ProcessStageID = 2, Description = "Water Treatment"})
                  .Row(new {ProcessStageID = 3, Description = "Water Distribution"});
            this.DisableIdentityInsert("ProcessStages");

            Create.Table("Processes")
                  .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn("Description").AsString(50).NotNullable()
                  .WithColumn("Sequence").AsDecimal(4, 2).NotNullable()
                  .WithColumn("ProcessStageId").AsInt32().NotNullable()
                  .ForeignKey("FK_Processes_ProcessStages_ProcessStageId", "ProcessStages", "ProcessStageId");

            Insert.IntoTable("Processes")
                  .Row(new {ProcessStageId = 1, Description = "Ground Water Pumping", Sequence = 1.01m})
                  .Row(new {ProcessStageId = 1, Description = "Surface Water Pumping", Sequence = 1.02m})
                  .Row(new {ProcessStageId = 1, Description = "Reservoir Storage", Sequence = 1.03m})
                  .Row(new {ProcessStageId = 2, Description = "Pre-Treatment", Sequence = 2.01m})
                  .Row(new {ProcessStageId = 2, Description = "Filtration", Sequence = 2.02m})
                  .Row(new {ProcessStageId = 2, Description = "Post-Treatment", Sequence = 2.03m})
                  .Row(new {ProcessStageId = 3, Description = "High Service Pumping", Sequence = 3.01m})
                  .Row(new {ProcessStageId = 3, Description = "Elevated Water Storage", Sequence = 3.02m})
                  .Row(new {ProcessStageId = 3, Description = "Ground Water Storage", Sequence = 3.03m})
                  .Row(new {ProcessStageId = 3, Description = "Ground Water Storage (Pumping)", Sequence = 3.04m})
                  .Row(new {ProcessStageId = 3, Description = "Pressure Reducing", Sequence = 3.05m})
                  .Row(new {ProcessStageId = 3, Description = "Gradient Transfer Pumping", Sequence = 3.06m})
                  .Row(new {ProcessStageId = 3, Description = "Interconnection", Sequence = 3.07m});

            Create.Table("FacilityProcesses")
                  .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn("FacilityId").AsInt32().NotNullable()
                  .ForeignKey("FK_FacilityProcesses_tblFacilities_FacilityId", "tblFacilities", "RecordId")
                  .WithColumn("ProcessId").AsInt32().NotNullable()
                  .ForeignKey("FK_FacilityProcesses_Processes_ProcessId", "Processes", "Id");

            Create.Table("FacilityProcessStepApplications")
                  .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn("Description").AsString(50).NotNullable().Unique();

            Insert.IntoTable("FacilityProcessStepApplications").Row(new {Description = "Flow"});
            Insert.IntoTable("FacilityProcessStepApplications").Row(new {Description = "Pressure"});

            Create.Table("FacilityProcessSteps")
                  .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn("EquipmentId").AsInt32().Nullable()
                  .ForeignKey("FK_FacilityProcessSteps_Equipment_EquipmentId", "Equipment", "EquipmentId")
                  .WithColumn("FacilityProcessId").AsInt32().NotNullable()
                  .ForeignKey("FK_FacilityProcessSteps_FacilityProcesses_FacilityProcessId", "FacilityProcesses", "Id")
                  .WithColumn("SignalId").AsInt32().Nullable() // This is supposed to be an FK to....?
                  .WithColumn("FacilityProcessStepApplicationId").AsInt32().NotNullable()
                  .ForeignKey(
                       "FK_FacilityProcessSteps_FacilityProcessStepApplications_FacilityProcessStepApplicationId",
                       "FacilityProcessStepApplications", "Id")
                  .WithColumn("UnitOfMeasureId").AsInt32().NotNullable()
                  .ForeignKey("FK_FacilityProcessSteps_UnitsOfMeasure_UnitOfMeasureId", "UnitsOfMeasure",
                       "UnitOfMeasureId")
                  .WithColumn("ElevationInFeet").AsInt32().NotNullable()
                  .WithColumn("NormalRange").AsDecimal(18, 6).NotNullable()
                  .WithColumn("Description").AsString(50).NotNullable()
                  .WithColumn("StepNumber").AsInt32().NotNullable();

            // Need data type for Processes.
            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('Processes', 'Processes')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Processes', @dataTypeId)");

            // Also need data type for FacilityProcesses
            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('FacilityProcesses', 'FacilityProcesses')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('FacilityProcesses', @dataTypeId)");
        }

        public override void Down()
        {
            this.RemoveDataType("Processes");
            this.RemoveDataType("FacilityProcesses");

            Delete.ForeignKey("FK_FacilityProcessSteps_UnitsOfMeasure_UnitOfMeasureId").OnTable("FacilityProcessSteps");
            Delete.ForeignKey(
                       "FK_FacilityProcessSteps_FacilityProcessStepApplications_FacilityProcessStepApplicationId")
                  .OnTable("FacilityProcessSteps");
            Delete.ForeignKey("FK_FacilityProcessSteps_Equipment_EquipmentId").OnTable("FacilityProcessSteps");
            Delete.ForeignKey("FK_FacilityProcessSteps_FacilityProcesses_FacilityProcessId")
                  .OnTable("FacilityProcessSteps");
            Delete.Table("FacilityProcessSteps");

            Delete.Table("FacilityProcessStepApplications");

            Delete.ForeignKey("FK_FacilityProcesses_tblFacilities_FacilityId").OnTable("FacilityProcesses");
            Delete.ForeignKey("FK_FacilityProcesses_Processes_ProcessId").OnTable("FacilityProcesses");
            Delete.Table("FacilityProcesses");

            Delete.ForeignKey("FK_Processes_ProcessStages_ProcessStageId").OnTable("Processes");
            Delete.Table("Processes");

            // Set back the old ProcessStages values.
            Delete.FromTable("ProcessStages").AllRows();

            this.EnableIdentityInsert("ProcessStages");
            Insert.IntoTable("ProcessStages")
                  .Row(new {ProcessStageID = 496, Description = "1-Source of Supply"})
                  .Row(new {ProcessStageID = 497, Description = "2-Process Recycle"})
                  .Row(new {ProcessStageID = 498, Description = "3-Influent"})
                  .Row(new {ProcessStageID = 499, Description = "4-Pre-Treatment"})
                  .Row(new {ProcessStageID = 500, Description = "5-Filtration"})
                  .Row(new {ProcessStageID = 501, Description = "6-Residuals Processing"})
                  .Row(new {ProcessStageID = 502, Description = "7-Post-Treatment"})
                  .Row(new {ProcessStageID = 503, Description = "8-Distribution System"});
            this.DisableIdentityInsert("ProcessStages");

            Alter.Table("tblTrainingModules")
                 .AddColumn("ProcessStage").AsInt32().Nullable()
                 .ForeignKey("FK_tblTrainingModules_ProcessStages_ProcessStage", "ProcessStages", "ProcessStageId")
                 .AddColumn("ProcessStageSequence").AsInt32().Nullable();
        }
    }
}
