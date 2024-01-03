using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200723113157589), Tags("Production")]
    public class MC1888AddSection5ToConfinedSpaceForms : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("ConfinedSpaceFormMethodsOfCommunication",
                "Voice",
                "Radio",
                "Other");

            this.CreateLookupTableWithValues("ConfinedSpaceFormHazardTypes",
                "Oxygen deficient or enriched atmosphere",
                "Flammable/combustible vapors or gas",
                "Airborne combustible dusts",
                "Other toxic gases or vapors",
                "Mechanical hazard",
                "Chemical hazard",
                "Electrical hazard",
                "Entrapment or engulfment hazard",
                "Other (specify)");

            Create.Table("ConfinedSpaceFormHazards")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ConfinedSpaceFormId", "ConfinedSpaceForms").NotNullable()
                  .WithForeignKeyColumn("ConfinedSpaceFormHazardTypeId", "ConfinedSpaceFormHazardTypes").NotNullable()
                  .WithColumn("Notes").AsString(255).NotNullable();

            Alter.Table("ConfinedSpaceForms")
                 .AddColumn("PermitBeginsAt").AsDateTime().Nullable()
                 .AddColumn("PermitEndsAt").AsDateTime().Nullable()
                 .AddForeignKeyColumn("EntrantEmployeeId", "tblEmployee", "tblEmployeeId").Nullable()
                 .AddForeignKeyColumn("AttendantEmployeeId", "tblEmployee", "tblEmployeeId").Nullable()
                 .AddForeignKeyColumn("EntrySupervisorEmployeeId", "tblEmployee", "tblEmployeeId").Nullable()

                  // Required safety equipment
                 .AddColumn("HasWarningSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasAccessSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasLightingSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasVentilationSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasGFCISafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasOtherSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasHeadSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasEyeSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasRespiratorySafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasHandSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasFallSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasFootSafetyEquipment").AsBoolean().Nullable()
                 .AddColumn("HasOtherSafetyEquipmentNotes").AsString(255).Nullable()

                  // Method of communication
                 .AddForeignKeyColumn("ConfinedSpaceFormMethodOfCommunicationId",
                      "ConfinedSpaceFormMethodsOfCommunication").Nullable()
                 .AddColumn("MethodOfCommunicationOtherNotes").AsString(255).Nullable()

                  // Hot work permit
                 .AddColumn("IsHotWorkPermitRequired").AsBoolean().Nullable()
                 .AddColumn("IsFireWatchRequired").AsBoolean().Nullable()

                  // Method of rescue of entrants
                 .AddColumn("HasRetrievalSystem").AsBoolean().Nullable()
                 .AddColumn("HasContractRescueService").AsBoolean().Nullable()
                 .AddColumn("EmergencyResponseAgency").AsString(255).Nullable()
                 .AddColumn("EmergencyResponseContact").AsString(50).Nullable()

                  // Authorization to begin entry operation
                 .AddColumn("BeginEntryAuthorizedAt").AsDateTime().Nullable()
                 .AddForeignKeyColumn("BeginEntryAuthorizedByEmployeeId", "tblEmployee", "tblEmployeeId").Nullable()

                  // Cancellation of permit
                 .AddColumn("PermitCancelledAt").AsDateTime().Nullable()
                 .AddForeignKeyColumn("PermitCancelledByEmployeeId", "tblEmployee", "tblEmployeeId").Nullable();
        }

        public override void Down()
        {
            // Delete foreignkey columns
            this.DeleteForeignKeyColumn("ConfinedSpaceForms", "EntrantEmployeeId", "tblEmployee");
            this.DeleteForeignKeyColumn("ConfinedSpaceForms", "AttendantEmployeeId", "tblEmployee");
            this.DeleteForeignKeyColumn("ConfinedSpaceForms", "EntrySupervisorEmployeeId", "tblEmployee");
            this.DeleteForeignKeyColumn("ConfinedSpaceForms", "ConfinedSpaceFormMethodOfCommunicationId",
                "ConfinedSpaceFormMethodsOfCommunication");
            this.DeleteForeignKeyColumn("ConfinedSpaceForms", "BeginEntryAuthorizedByEmployeeId", "tblEmployee");
            this.DeleteForeignKeyColumn("ConfinedSpaceForms", "PermitCancelledByEmployeeId", "tblEmployee");

            Delete.Column("PermitBeginsAt").FromTable("ConfinedSpaceForms");
            Delete.Column("PermitEndsAt").FromTable("ConfinedSpaceForms");
            Delete.Column("HasWarningSafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasAccessSafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasLightingSafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasVentilationSafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasGFCISafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasOtherSafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasHeadSafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasEyeSafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasRespiratorySafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasHandSafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasFallSafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasFootSafetyEquipment").FromTable("ConfinedSpaceForms");
            Delete.Column("HasOtherSafetyEquipmentNotes").FromTable("ConfinedSpaceForms");
            Delete.Column("MethodOfCommunicationOtherNotes").FromTable("ConfinedSpaceForms");
            Delete.Column("IsHotWorkPermitRequired").FromTable("ConfinedSpaceForms");
            Delete.Column("IsFireWatchRequired").FromTable("ConfinedSpaceForms");
            Delete.Column("HasRetrievalSystem").FromTable("ConfinedSpaceForms");
            Delete.Column("HasContractRescueService").FromTable("ConfinedSpaceForms");
            Delete.Column("EmergencyResponseAgency").FromTable("ConfinedSpaceForms");
            Delete.Column("EmergencyResponseContact").FromTable("ConfinedSpaceForms");
            Delete.Column("BeginEntryAuthorizedAt").FromTable("ConfinedSpaceForms");
            Delete.Column("PermitCancelledAt").FromTable("ConfinedSpaceForms");

            Delete.Table("ConfinedSpaceFormHazards");
            Delete.Table("ConfinedSpaceFormHazardTypes");
            Delete.Table("ConfinedSpaceFormMethodsOfCommunication");
        }
    }
}
