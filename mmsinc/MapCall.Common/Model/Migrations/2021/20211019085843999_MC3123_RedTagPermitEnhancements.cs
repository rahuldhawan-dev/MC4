using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211019085843999), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC3123_RedTagPermitEnhancements : Migration
    {
        private const int RED_TAG_PERMIT_PREREQUISITE_ID = 7;

        public override void Up()
        {
            // 01: Add lookup table for protection types
            this.CreateLookupTableWithValues("RedTagPermitProtectionTypes",
                "Sprinkler System",
                "Fire Pump",
                "Special Protection",
                "Other");

            // 02: Create Main table
            Create.Table("RedTagPermits")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders", "Id", false)
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentID", false)
                  .WithForeignKeyColumn("PersonResponsibleId", "tblEmployee", "tblEmployeeID", false)
                  .WithForeignKeyColumn("AuthorizedById", "tblEmployee", "tblEmployeeId", false)
                  .WithForeignKeyColumn("ProtectionTypeId", "RedTagPermitProtectionTypes", "Id", false)
                  .WithColumn("AdditionalInformationForProtectionType").AsAnsiString(255).Nullable()
                  .WithColumn("AreaProtected").AsAnsiString(255).NotNullable()
                  .WithColumn("ReasonForImpairment").AsAnsiString(255).NotNullable()
                  .WithColumn("NumberOfTurnsToClose").AsInt32().NotNullable()
                  .WithColumn("FireProtectionEquipmentOperator").AsAnsiString(255).NotNullable()
                  .WithColumn("EquipmentImpairedOn").AsDateTime().NotNullable()
                  .WithColumn("EquipmentRestoredOn").AsDateTime().Nullable()
                  .WithColumn("CreatedOn").AsDateTime().NotNullable()
                  .WithColumn("EmergencyOrganizationNotified").AsBoolean().Nullable()
                  .WithColumn("PublicFireDepartmentNotified").AsBoolean().Nullable()
                  .WithColumn("HazardousOperationsStopped").AsBoolean().Nullable()
                  .WithColumn("HotWorkProhibited").AsBoolean().Nullable()
                  .WithColumn("SmokingProhibited").AsBoolean().Nullable()
                  .WithColumn("ContinuousWorkAuthorized").AsBoolean().Nullable()
                  .WithColumn("OngoingPatrolOfArea").AsBoolean().Nullable()
                  .WithColumn("HydrantConnectedToSprinkler").AsBoolean().Nullable()
                  .WithColumn("PipePlugsOnHand").AsBoolean().Nullable()
                  .WithColumn("FireHoseLaidOut").AsBoolean().Nullable()
                  .WithColumn("HasOtherPrecaution").AsBoolean().Nullable()
                  .WithColumn("OtherPrecautionDescription").AsAnsiString(255).Nullable();

            // 03: Add support for production work orders
            Alter.Table("ProductionWorkOrders")
                 .AddColumn("NeedsRedTagPermitAuthorization").AsBoolean().Nullable()
                 .AddColumn("NeedsRedTagPermitAuthorizedOn").AsDateTime().Nullable()
                 .AddForeignKeyColumn("NeedsRedTagPermitAuthorizedById", "tblEmployee", "tblEmployeeId");

            // 04: Add a new prerequisite type for Red Tag Permit
            this.AddLookupValueWithId("ProductionPrerequisites", RED_TAG_PERMIT_PREREQUISITE_ID, "Red Tag Permit");

            Alter.Table("SAPEquipmentTypes")
                 .AddColumn("IsEligibleForRedTagPermit")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(false);

            // 05: Assign which equipment types are eligible:
            //     FIRE-AL (Fire Alarm) and FIRE-SUP (Fire Suppression) are considered eligible for red tag permits
            Execute.Sql(@"
                update SAPEquipmentTypes 
                   set IsEligibleForRedTagPermit = 1 
                 where Id in (157, 159)");

            // 06: Add a red tag permit prerequisite for each existing equipment that is eligible 
            Execute.Sql($@"
                insert into EquipmentProductionPrerequisites (EquipmentId, ProductionPrerequisiteId)
                select e.EquipmentId, {RED_TAG_PERMIT_PREREQUISITE_ID}
                  from Equipment e
                 where e.SAPEquipmentTypeId 
                    in (157, 159)");

            // 07: Add a red tag prerequisite for each existing production work order that is eligible - 
            Execute.Sql($@"
                insert into ProductionWorkOrdersProductionPrerequisites (
                       ProductionWorkOrderId
                     , ProductionPrerequisiteId
                     , SatisfiedOn
                     , LinkedDocumentId
                     , SkipRequirement
                     , SkipRequirementComments)
                select pwo.Id, {RED_TAG_PERMIT_PREREQUISITE_ID}, null, null, 0, null
                  from ProductionWorkOrders pwo
                  join ProductionWorkOrdersEquipment pwoe
                    on pwo.Id = pwoe.ProductionWorkOrderId
                   and pwoe.IsParent = 1
                 where pwo.DateCompleted is null
                   and pwo.DateCancelled is null
                   and pwoe.ProductionWorkOrderId in 
                        (
                        select e.EquipmentId
                          from Equipment e
                         where e.SAPEquipmentTypeId in (157, 159)
                        )");

            // 08: add in some notification types/purposes

            this.AddNotificationType("Production", "Production Work Management", "Red Tag Permit Out Of Service");
            this.AddNotificationType("Production", "Production Work Management", "Red Tag Permit In Service");
        }

        public override void Down()
        {
            // 08: Remove some notification types/purposes
            this.RemoveNotificationPurpose("Production", "Production Work Management", "Red Tag Permit Out Of Service");
            this.RemoveNotificationPurpose("Production", "Production Work Management", "Red Tag Permit In Service");

            // 07: Remove red tag prerequisites for each existing production work orders that were eligible
            Execute.Sql($"delete from ProductionWorkOrdersProductionPrerequisites where ProductionPrerequisiteId = {RED_TAG_PERMIT_PREREQUISITE_ID}");

            // 06: Remove red tag permit prerequisite for each existing equipment that was eligible 
            Execute.Sql($"delete from EquipmentProductionPrerequisites where ProductionPrerequisiteId = {RED_TAG_PERMIT_PREREQUISITE_ID}");

            // 05: Remove which equipment types are eligible:
            Delete.Column("IsEligibleForRedTagPermit").FromTable("SAPEquipmentTypes");

            // 04: Remove the new prerequisite type for Red Tag Permit
            Execute.Sql($"DELETE FROM ProductionPrerequisites WHERE Id = {RED_TAG_PERMIT_PREREQUISITE_ID};");

            // 03: Remove support for production work orders
            Delete.ForeignKeyColumn("ProductionWorkOrders", "NeedsRedTagPermitAuthorizedById", "tblEmployee", "tblEmployeeId");
            Delete.Column("NeedsRedTagPermitAuthorizedOn").FromTable("ProductionWorkOrders");
            Delete.Column("NeedsRedTagPermitAuthorization").FromTable("ProductionWorkOrders");

            // 02: Remove Main table
            Delete.Table("RedTagPermits");

            // 01: Remove lookup table for protection types
            Delete.Table("RedTagPermitProtectionTypes");
        }
    }
}

