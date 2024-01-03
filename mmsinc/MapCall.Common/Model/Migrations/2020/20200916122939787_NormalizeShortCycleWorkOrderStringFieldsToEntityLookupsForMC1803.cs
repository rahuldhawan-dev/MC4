using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200902122939787), Tags("Production")]
    public class NormalizeShortCycleWorkOrderStringFieldsToEntityLookupsForMC1803 : Migration
    {
        public override void Up()
        {
            Alter.Column("Description").OnTable("ServiceInstallationPositions").AsString(50).Nullable();

            this.ExtractNonLookupTableLookup("ShortCycleAssignments", "Number", "ShortCycleAssignmentNumbers", 4,
                newColumnName: "AssignmentNumberId");

            Execute.Sql("drop STATISTICS [ShortCycleWorkOrders].[_dta_stat_30779317_15_1]");
            Execute.Sql("drop STATISTICS [ShortCycleWorkOrders].[_dta_stat_30779317_15_13_79_8_1_81_80]");
            Execute.Sql("drop STATISTICS [ShortCycleWorkOrders].[_dta_stat_30779317_8_79_13_1_81_80]");
            Execute.Sql("drop STATISTICS [ShortCycleWorkOrders].[_dta_stat_30779317_1_81_80_15_79]");
            Execute.Sql("drop STATISTICS [ShortCycleWorkOrders].[_dta_stat_30779317_1_81_80_13_79_15]");
            Delete.Index("ShortCycleWorkOrders_WorkOrder").OnTable("ShortCycleWorkOrders");

            this.NullEmptyStringValues("ShortCycleWorkOrders", "LiabilityIndicator");
            this.NullEmptyStringValues("ShortCycleWorkOrders", "ServiceFound");

            this.ExtractNonLookupTableLookup("ShortCycleWorkOrders", "ServiceType", "ShortCycleWorkOrderServiceTypes",
                2, newColumnName: "ServiceTypeId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrders", "Priority", "ShortCycleWorkOrderPriorities", 20,
                newColumnName: "PriorityId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrders", "WorkCenter", "ShortCycleWorkCenters", 8,
                newColumnName: "WorkCenterId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrders", "NormalDurationUnit",
                "ShortCycleWorkOrderDurationUnits", 3, newColumnName: "NormalDurationUnitId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrders", "LiabilityIndicator",
                "ShortCycleWorkOrderLiabilityIndicators", 80, newColumnName: "LiabilityIndicatorId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrders", "FixedChargeNoMeter",
                "ShortCycleWorkOrderFixedChargeNoMeters", 10, newColumnName: "FixedChargeNoMeterId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrders", "DunningLock",
                "ShortCycleWorkOrderDunningLockStatuses", 40, newColumnName: "DunningLockId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrders", "PremiseType", "ShortCycleWorkOrderPremiseTypes",
                40, newColumnName: "PremiseTypeId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrders", "LandlordAllocation",
                "ShortCycleWorkOrderLandlordAllocations", 30, newColumnName: "LandlordAllocationId");

            Alter.Table("ShortCycleWorkOrders").AddForeignKeyColumn("ServiceFoundId", "ServiceInstallationPositions");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrders SET ServiceFoundId = ServiceInstallationPositions.Id FROM ServiceInstallationPositions WHERE ServiceInstallationPositions.SAPCode = LEFT(ServiceFound, 3);");
            Delete.Column("ServiceFound").FromTable("ShortCycleWorkOrders");

            Alter.Table("ShortCycleWorkOrderPriorities").AddColumn("Level").AsInt32().Nullable();
            Execute.Sql("UPDATE ShortCycleWorkOrderPriorities SET Level = cast(substring(Description, 1, 1) as int)");
            Alter.Table("ShortCycleWorkOrderPriorities").AlterColumn("Level").AsInt32().NotNullable();

            Alter.Table("ShortCycleWorkOrders").AddForeignKeyColumn("PlanningPlantId", "PlanningPlants");
            Execute.Sql(
                "INSERT INTO PlanningPlants (Code, Description) select distinct scwo.PlanningPlant, 'GET MY REAL NAME' from ShortCycleWorkOrders scwo where scwo.PlanningPlant is not null and not exists (select 1 from PlanningPlants pp where pp.Code = scwo.PlanningPlant) order by 1");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrders SET PlanningPlantId = pp.Id FROM PlanningPlants pp WHERE pp.Code = ShortCycleWorkOrders.PlanningPlant;");
            Delete.Column("PlanningPlant").FromTable("ShortCycleWorkOrders");

            Alter.Table("ShortCycleWorkOrders")
                 .AlterColumn("BackReportingType").AsInt32().Nullable()
                 .AlterColumn("CompanyCode").AsInt32().Nullable()
                 .AlterColumn("OrderType").AsInt32().Nullable()
                 .AlterColumn("OperationId").AsInt32().Nullable()
                 .AlterColumn("PlannerGroup").AsInt32().Nullable()
                 .AlterColumn("OperationNumber").AsInt32().Nullable();

            Create.Table("ShortCycleReasonCodes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(35).Nullable()
                  .WithColumn("Code").AsFixedLengthString(4).NotNullable().Unique();

            Execute.Sql(
                @"INSERT INTO ShortCycleReasonCodes (Code) SELECT DISTINCT left(ReasonCode, 4) FROM ShortCycleWorkOrders WHERE ReasonCode is not null");
            Execute.Sql(
                @"UPDATE ShortCycleReasonCodes SET Description = RIGHT(ReasonCode, LEN(ReasonCode) - 5) FROM ShortCycleWorkOrders WHERE LEN(ReasonCode) > 5 AND left(ReasonCode, 4) = Code");

            Alter.Table("ShortCycleWorkOrders")
                 .AddForeignKeyColumn("ReasonCodeId", "ShortCycleReasonCodes");

            Execute.Sql(
                @"UPDATE ShortCycleWorkOrders SET ReasonCodeId = ShortCycleReasonCodes.Id FROM ShortCycleReasonCodes WHERE ReasonCode = Code + '-' + Description OR ReasonCode = Code + ' ' + Description OR ReasonCode = Code");

            Delete.Column("ReasonCode").FromTable("ShortCycleWorkOrders");

            Execute.Sql(@"CREATE NONCLUSTERED INDEX [ShortCycleWorkOrders_WorkOrder] ON [dbo].[ShortCycleWorkOrders]
(
	[WorkOrder] ASC
)
INCLUDE ( 	[Id],
	[SAPCommunicationStatusId],
	[FunctionalLocation],
	[NotificationNumber],
	[PriorityId],
	[BackReportingType],
	[CompanyCode],
	[Premise],
	[Status],
	[WBSElement],
	[PlanningPlantId],
	[MATCode],
	[MATCodeDescription],
	[OperationText],
	[FSRId],
	[FSRName],
	[CustomerNumber],
	[NextReplacementYear],
	[ServiceTypeId],
	[MeterSerialNumber],
	[ManufacturerSerialNumber],
	[IsCustomerEnrolledForEmail],
	[IsUpdate],
	[AssignmentStart],
	[AssignmentEnd],
	[OrderType],
	[OperationId],
	[WorkCenterId],
	[CustomerAccount],
	[NormalDuration],
	[NormalDurationUnitId],
	[PlannerGroup],
	[ActiveMQStatus],
	[PhoneAhead],
	[CustomerAtHome],
	[OperationNumber],
	[ModifiedBy],
	[TimeModified],
	[ReasonCodeId],
	[DueDateTime],
	[LiabilityIndicatorId],
	[AppointmentStart],
	[AppointmentEnd],
	[CrewSize],
	[ReceivedAt]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO");

            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_30779317_15_13_79_8_1_81_80] ON [dbo].[ShortCycleWorkOrders]([PlanningPlantId], [Status], [DoNotUnschedule], [PriorityId], [Id], [AssignmentStart], [LastDispatched])");
            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_30779317_8_79_13_1_81_80] ON [dbo].[ShortCycleWorkOrders]([PriorityId], [DoNotUnschedule], [Status], [Id], [AssignmentStart], [LastDispatched])");
            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_30779317_1_81_80_13_79_15] ON [dbo].[ShortCycleWorkOrders]([Id], [AssignmentStart], [LastDispatched], [Status], [DoNotUnschedule], [PlanningPlantId])");
            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_30779317_1_81_80_15_79] ON [dbo].[ShortCycleWorkOrders]([Id], [AssignmentStart], [LastDispatched], [PlanningPlantId], [DoNotUnschedule])");
            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_30779317_15_1] ON [dbo].[ShortCycleWorkOrders]([PlanningPlantId], [Id]);");
        }

        public override void Down()
        {
            Execute.Sql("drop STATISTICS [ShortCycleWorkOrders].[_dta_stat_30779317_15_1]");
            Execute.Sql("drop STATISTICS [ShortCycleWorkOrders].[_dta_stat_30779317_15_13_79_8_1_81_80]");
            Execute.Sql("drop STATISTICS [ShortCycleWorkOrders].[_dta_stat_30779317_8_79_13_1_81_80]");
            Execute.Sql("drop STATISTICS [ShortCycleWorkOrders].[_dta_stat_30779317_1_81_80_15_79]");
            Execute.Sql("drop STATISTICS [ShortCycleWorkOrders].[_dta_stat_30779317_1_81_80_13_79_15]");
            Delete.Index("ShortCycleWorkOrders_WorkOrder").OnTable("ShortCycleWorkOrders");

            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrders", "ServiceType", "ShortCycleWorkOrderServiceTypes",
                2, newColumnName: "ServiceTypeId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrders", "Priority", "ShortCycleWorkOrderPriorities", 25,
                newColumnName: "PriorityId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrders", "WorkCenter", "ShortCycleWorkCenters", 8,
                newColumnName: "WorkCenterId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrders", "NormalDurationUnit",
                "ShortCycleWorkOrderDurationUnits", 3, newColumnName: "NormalDurationUnitId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrders", "LiabilityIndicator",
                "ShortCycleWorkOrderLiabilityIndicators", 20, newColumnName: "LiabilityIndicatorId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrders", "FixedChargeNoMeter",
                "ShortCycleWorkOrderFixedChargeNoMeters", 10, newColumnName: "FixedChargeNoMeterId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrders", "DunningLock",
                "ShortCycleWorkOrderDunningLockStatuses", 20, newColumnName: "DunningLockId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrders", "PremiseType", "ShortCycleWorkOrderPremiseTypes",
                45, newColumnName: "PremiseTypeId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrders", "LandlordAllocation",
                "ShortCycleWorkOrderLandlordAllocations", 15, newColumnName: "LandlordAllocationId");

            Alter.Table("ShortCycleWorkOrders").AddColumn("ServiceFound").AsString(30).Nullable();
            Execute.Sql(
                "UPDATE ShortCycleWorkOrders SET ServiceFound = ServiceInstallationPositions.SAPCode FROM ServiceInstallationPositions WHERE ServiceInstallationPositions.Id = ServiceFoundId;");
            Delete.ForeignKeyColumn("ShortCycleWorkOrders", "ServiceFoundId", "ServiceInstallationPositions");

            Create.Column("ReasonCode")
                  .OnTable("ShortCycleWorkOrders").AsString(40).Nullable();

            Execute.Sql(
                @"UPDATE ShortCycleWorkOrders SET ReasonCode = Code + '-' + Description FROM ShortCycleReasonCodes WHERE ShortCycleReasonCodes.Id = ReasonCodeId AND Code IS NOT NULL AND Description IS NOT NULL");
            Execute.Sql(
                @"UPDATE ShortCycleWorkOrders SET ReasonCode = Code FROM ShortCycleReasonCodes WHERE ShortCycleReasonCodes.Id = ReasonCodeId AND Code IS NOT NULL AND Description IS NULL");

            Delete.ForeignKeyColumn("ShortCycleWorkOrders", "ReasonCodeId", "ShortCycleReasonCodes");
            Delete.Table("ShortCycleReasonCodes");

            Alter.Table("ShortCycleWorkOrders")
                 .AlterColumn("BackReportingType").AsString(2).Nullable()
                 .AlterColumn("CompanyCode").AsString(4).Nullable()
                 .AlterColumn("OrderType").AsString(4).Nullable()
                 .AlterColumn("OperationId").AsString(4).Nullable()
                 .AlterColumn("PlannerGroup").AsString(4).Nullable();

            Alter.Table("ShortCycleWorkOrders").AddColumn("PlanningPlant").AsString(4).Nullable();
            Execute.Sql(
                "UPDATE ShortCycleWorkOrders SET PlanningPlant = pp.Code FROM PlanningPlants pp WHERE pp.Id = ShortCycleWorkOrders.PlanningPlantId;");
            Delete.ForeignKeyColumn("ShortCycleWorkOrders", "PlanningPlantId", "PlanningPlants");

            Execute.Sql(@"CREATE NONCLUSTERED INDEX [ShortCycleWorkOrders_WorkOrder] ON [dbo].[ShortCycleWorkOrders]
(
	[WorkOrder] ASC
)
INCLUDE ( 	[Id],
	[SAPCommunicationStatusId],
	[FunctionalLocation],
	[NotificationNumber],
	[Priority],
	[BackReportingType],
	[CompanyCode],
	[Premise],
	[Status],
	[WBSElement],
	[PlanningPlant],
	[MATCode],
	[MATCodeDescription],
	[OperationText],
	[FSRId],
	[FSRName],
	[CustomerNumber],
	[NextReplacementYear],
	[ServiceType],
	[MeterSerialNumber],
	[ManufacturerSerialNumber],
	[IsCustomerEnrolledForEmail],
	[IsUpdate],
	[AssignmentStart],
	[AssignmentEnd],
	[OrderType],
	[OperationId],
	[WorkCenter],
	[CustomerAccount],
	[NormalDuration],
	[NormalDurationUnit],
	[PlannerGroup],
	[ActiveMQStatus],
	[PhoneAhead],
	[CustomerAtHome],
	[OperationNumber],
	[ModifiedBy],
	[TimeModified],
	[ReasonCode],
	[DueDateTime],
	[LiabilityIndicator],
	[AppointmentStart],
	[AppointmentEnd],
	[CrewSize],
	[ReceivedAt]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO");

            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_30779317_15_13_79_8_1_81_80] ON [dbo].[ShortCycleWorkOrders]([PlanningPlant], [Status], [DoNotUnschedule], [Priority], [Id], [AssignmentStart], [LastDispatched])");
            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_30779317_8_79_13_1_81_80] ON [dbo].[ShortCycleWorkOrders]([Priority], [DoNotUnschedule], [Status], [Id], [AssignmentStart], [LastDispatched])");
            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_30779317_1_81_80_13_79_15] ON [dbo].[ShortCycleWorkOrders]([Id], [AssignmentStart], [LastDispatched], [Status], [DoNotUnschedule], [PlanningPlant])");
            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_30779317_1_81_80_15_79] ON [dbo].[ShortCycleWorkOrders]([Id], [AssignmentStart], [LastDispatched], [PlanningPlant], [DoNotUnschedule])");
            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_30779317_15_1] ON [dbo].[ShortCycleWorkOrders]([PlanningPlant], [Id]);");

            this.ReplaceNonLookupTableLookup("ShortCycleAssignments", "Number", "ShortCycleAssignmentNumbers", 4,
                newColumnName: "AssignmentNumberId");
        }
    }
}
