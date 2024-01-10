using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderMap : ClassMap<WorkOrder>
    {
        #region Constants

        public const string TABLE_NAME = nameof(WorkOrder) + "s";

        public const string
            SQL_PENDING_ASSIGNMENT =
                "(CASE WHEN EXISTS (SELECT 1 FROM CrewAssignments ca where ca.WorkOrderID = WorkOrderID and ca.AssignedFor >= CAST(GETDATE() AS Date)) THEN 1 ELSE 0 END)";

        #endregion

        #region Constructors

        public WorkOrderMap()
        {
            Id(x => x.Id, "WorkOrderID");

            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.CustomerName);
            Map(x => x.DateCompleted);
            Map(x => x.Latitude).Nullable();
            Map(x => x.Longitude).Nullable();
            Map(x => x.AlertID);
            Map(x => x.DateReceived);
            Map(x => x.Notes).Length(int.MaxValue).CustomSqlType("text");
            Map(x => x.SpecialInstructions).Length(int.MaxValue).CustomSqlType("text");
            Map(x => x.RequiredMarkoutNote);
            Map(x => x.PhoneNumber);
            Map(x => x.PremiseNumber).Nullable();
            Map(x => x.SecondaryPhoneNumber);
            Map(x => x.ServiceNumber).Nullable();
            Map(x => x.StreetNumber).Nullable();
            Map(x => x.StreetOpeningPermitRequired);
            Map(x => x.TrafficControlRequired);
            Map(x => x.ZipCode).Nullable();
            Map(x => x.ApprovedOn);
            Map(x => x.MaterialsApprovedOn);
            Map(x => x.MaterialsDocID);
            Map(x => x.DateStarted);
            Map(x => x.DatePrinted);
            Map(x => x.DateReportSent);
            Map(x => x.BackhoeOperator);
            Map(x => x.ExcavationDate);
            Map(x => x.DateCompletedPC);
            Map(x => x.LostWater);
            Map(x => x.NumberOfOfficersRequired);
            Map(x => x.OldWorkOrderNumber);
            Map(x => x.CustomerAccountNumber);
            Map(x => x.AccountCharged).Length(WorkOrder.StringLengths.ACCOUNT_CHARGED);
            Map(x => x.InvoiceNumber);
            Map(x => x.BusinessUnit).Length(WorkOrder.StringLengths.BUSINESS_UNIT);
            Map(x => x.DistanceFromCrossStreet);
            Map(x => x.OfficeAssignedOn);
            Map(x => x.SignificantTrafficImpact);
            Map(x => x.MarkoutToBeCalled);
            Map(x => x.SAPNotificationNumber);
            Map(x => x.SAPWorkOrderNumber);
            Map(x => x.RequiresInvoice);
            Map(x => x.CancelledAt);
            Map(x => x.MeterSerialNumber).Nullable();
            Map(x => x.DeviceLocation).Nullable();
            Map(x => x.Installation).Nullable();
            Map(x => x.SAPEquipmentNumber).Nullable();
            Map(x => x.UpdatedMobileGIS).Nullable();
            Map(x => x.DoorNoticeLeftDate).Nullable();
            Map(x => x.AssignedToContractorOn).Nullable();
            Map(x => x.ApartmentAddtl).Length(WorkOrder.StringLengths.APARTMENT_ADDTL).Nullable();
            Map(x => x.IsConfinedSpaceFormRequired).Not.Nullable();
            Map(x => x.AlertIssued);
            Map(x => x.DigitalAsBuiltRequired).Not.Nullable();
            Map(x => x.DigitalAsBuiltCompleted).Nullable();
            Map(x => x.InitialServiceLineFlushTime).Nullable();
            Map(x => x.InitialFlushTimeEnteredAt).Nullable();
            Map(x => x.HasPitcherFilterBeenProvidedToCustomer).Nullable();
            Map(x => x.DatePitcherFilterDeliveredToCustomer).Nullable();
            Map(x => x.PitcherFilterCustomerDeliveryOtherMethod).Nullable();
            Map(x => x.DateCustomerProvidedAWStateLeadInformation).Nullable();
            Map(x => x.PlannedCompletionDate).Nullable();
            Map(x => x.IsThisAMultiTenantFacility).Nullable();
            Map(x => x.NumberOfPitcherFiltersDelivered).Nullable();
            Map(x => x.DescribeWhichUnits).Nullable();

            References(x => x.WorkOrderCancellationReason).Nullable();
            References(x => x.SAPWorkOrderStep, "SapWorkOrderStepId").Nullable();
            References(x => x.ApprovedBy).Nullable();
            References(x => x.OfficeAssignment).Nullable();
            References(x => x.Town).Not.Nullable();
            References(x => x.Street).Nullable();
            References(x => x.WorkDescription);
            References(x => x.AssetType);
            References(x => x.CreatedBy, "CreatorID").Not.Nullable();
            References(x => x.CompletedBy).Nullable();
            References(x => x.CancelledBy).Nullable();
            References(x => x.Hydrant);
            References(x => x.MarkoutRequirement);
            References(x => x.NearestCrossStreet);
            References(x => x.OperatingCenter);
            References(x => x.Priority).Not.Nullable();
            References(x => x.Purpose).Not.Nullable();
            References(x => x.RequestedBy, "RequesterID").Not.Nullable();
            References(x => x.RequestingEmployee);
            References(x => x.EstimatedCustomerImpact, "CustomerImpactRangeId"); // Duplicate of CustomerImpactRange.
            References(x => x.AnticipatedRepairTime, "RepairTimeRangeId"); // Duplicate of RepairTimeRange
            References(x => x.TownSection).Nullable();
            References(x => x.SewerOpening).Nullable();
            References(x => x.StormWaterAsset, "StormCatchID").Nullable();
            References(x => x.Service).Nullable();
            References(x => x.MainCrossing).Nullable();
            References(x => x.Valve).Nullable();
            References(x => x.Equipment).Nullable();
            References(x => x.AssignedContractor).Nullable();
            References(x => x.PlantMaintenanceActivityTypeOverride).Nullable();
            References(x => x.AcousticMonitoringType).Nullable();
            References(x => x.MaterialsApprovedBy).Nullable();
            References(x => x.PreviousServiceLineMaterial);
            References(x => x.PreviousServiceLineSize);
            References(x => x.CustomerServiceLineMaterial);
            References(x => x.CustomerServiceLineSize);
            References(x => x.CompanyServiceLineMaterial);
            References(x => x.CompanyServiceLineSize);
            References(x => x.OriginalOrderNumber, "OriginalOrderNumber").Nullable();
            References(x => x.FlushingNoticeType).Nullable();
            References(x => x.EchoshoreLeakAlert).Nullable();
            References(x => x.SmartCoverAlert).Nullable();
            References(x => x.InitialFlushTimeEnteredBy).Nullable();
            References(x => x.PitcherFilterCustomerDeliveryMethod).Nullable();
            References(x => x.MarkoutTypeNeeded).Nullable();
            References(x => x.Premise, "PremiseId").Nullable();
            References(x => x.MeterLocation).Nullable();

            Map(x => x.MonthCompleted)
               .Formula("CASE WHEN (DateCompleted is not null) THEN month(DateCompleted) ELSE NULL END")
               .ReadOnly();
            Map(x => x.YearCompleted)
               .Formula("CASE WHEN (DateCompleted is not null) THEN year(DateCompleted) ELSE NULL END")
               .ReadOnly();
            Map(x => x.Completed)
               .Formula("CASE WHEN (DateCompleted is not null) THEN 1 ELSE 0 END")
               .Not.Update()
               .Not.Insert();
            Map(x => x.HasInvoice)
               .Formula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM WorkOrderInvoices woi where woi.WorkOrderID = WorkOrderID) THEN 1 ELSE 0 END)");
            Map(x => x.HasSAPErrorCode)
               .DbSpecificFormula(
                    "CASE WHEN (SAPErrorCode is not null and CharIndex('SUCCESS', UPPER(SAPErrorCode)) = 0) THEN 1 ELSE 0 END");
            Map(x => x.HasMaterialsUsed)
               .DbSpecificFormula(
                    "CASE WHEN EXISTS (SELECT 1 FROM MaterialsUsed MU WHERE MU.WOrkOrderID = WorkOrderID) THEN 1 ELSE 0 END");
            Map(x => x.SAPErrorCode).Nullable();
            Map(x => x.MaterialPlanningCompletedOn).Nullable();
            Map(x => x.MaterialPostingDate).Nullable();
            Map(x => x.DateRejected).Nullable();
            Map(x => x.ORCOMServiceOrderNumber);
            Map(x => x.HasJobSiteCheckLists)
               .Formula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM JobSiteCheckLists jscl WHERE jscl.MapCallWorkOrderId = WorkOrderId  and jscl.HasExcavation is not null) THEN 1 ELSE 0 END)");
            Map(x => x.HasPreJobSafetyBriefs)
               .Formula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM JobSiteCheckLists jscl WHERE jscl.MapCallWorkOrderId = WorkOrderId  and jscl.AnyPotentialOverheadHazards is not null) THEN 1 ELSE 0 END)");
            Map(x => x.StreetOpeningPermitRequested)
               .Formula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM StreetOpeningPermits sop WHERE sop.WorkOrderID = WorkOrderId) THEN 1 ELSE 0 END)");
            Map(x => x.StreetOpeningPermitIssued)
               .Formula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM StreetOpeningPermits sop WHERE sop.WorkOrderID = WorkOrderId  and sop.DateIssued is not null) THEN 1 ELSE 0 END)");
            Map(x => x.HasPendingAssignments).DbSpecificFormula(SQL_PENDING_ASSIGNMENT, SQL_PENDING_ASSIGNMENT.ToSqlite());

            HasMany(x => x.CrewAssignments).KeyColumn("WorkOrderID").Cascade.All();
            HasMany(x => x.MainBreaks).KeyColumn("WorkOrderID");
            HasMany(x => x.Markouts).KeyColumn("WorkOrderID");
            HasMany(x => x.MaterialsUsed).KeyColumn("WorkOrderID");
            HasMany(x => x.ServiceInstallations).KeyColumn("WorkOrderId");
            HasMany(x => x.StreetOpeningPermits).KeyColumn("WorkOrderId");
            HasMany(x => x.Requisitions).KeyColumn("WorkOrderID").Cascade.None();
            HasMany(x => x.Restorations).KeyColumn("WorkOrderID");
            HasMany(x => x.Spoils).KeyColumn("WorkOrderID");
            HasMany(x => x.JobSiteCheckLists).KeyColumn("MapCallWorkOrderId");
            HasMany(x => x.BelowGroundHazards).KeyColumn("WorkOrderId");
            HasMany(x => x.WorkOrdersScheduleOfValues).KeyColumn("WorkOrderID").Cascade.All();
            HasMany(x => x.TrafficControlTickets).KeyColumn("WorkOrderId").Inverse().Cascade.All();
            HasMany(x => x.Invoices).KeyColumn("WorkOrderId").Inverse().Cascade.All();
            HasMany(x => x.SewerOverflows).KeyColumn("WorkOrderId").Inverse().Cascade.All();
            HasMany(x => x.MarkoutDamages).KeyColumn("WorkOrderId").Inverse().Cascade.All();
            HasMany(x => x.MarkoutViolations).KeyColumn("WorkOrderId").Inverse().Cascade.All();
            HasMany(x => x.WorkOrderDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.JobObservations).KeyColumn("WorkOrderId").Inverse().Cascade.All();

            HasOne(x => x.CurrentMarkout);
            HasOne(x => x.CurrentAssignment);
            HasOne(x => x.AssetId);
        }

        #endregion
    }
}
