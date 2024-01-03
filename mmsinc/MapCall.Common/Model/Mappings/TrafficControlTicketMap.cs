using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class TrafficControlTicketMap : ClassMap<TrafficControlTicket>
    {
        public TrafficControlTicketMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.WorkOrder).Nullable();
            References(x => x.Street).Not.Nullable();
            References(x => x.CrossStreet).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.Town).Column("MunicipalityId").Not.Nullable();
            References(x => x.ApprovedBy).Nullable();
            References(x => x.BillingParty).Nullable();
            References(x => x.MerchantTotalFee).Nullable();

            Map(x => x.SAPWorkOrderNumber).Not.Nullable();
            Map(x => x.StreetNumber).Not.Nullable().Length(20);
            Map(x => x.WorkStartDate).Not.Nullable();
            Map(x => x.WorkEndDate).Nullable();
            Map(x => x.TotalHours).Length(5).Precision(2).Not.Nullable();
            Map(x => x.NumberOfOfficers).Not.Nullable();
            Map(x => x.AccountingCode).Nullable().Length(12);
            Map(x => x.InvoiceNumber).Length(12);
            Map(x => x.InvoiceAmount).Precision(10);
            Map(x => x.InvoiceDate);
            Map(x => x.InvoiceTotalHours).Length(5).Precision(2).Nullable();
            Map(x => x.TrafficControlTicketNotes).Column("Notes").Nullable();
            Map(x => x.DateApproved);

            Map(x => x.PaymentReceivedAt).Nullable();
            Map(x => x.TotalCharged).Nullable();
            Map(x => x.PaymentTransactionId).Nullable();
            Map(x => x.PaymentAuthorizationCode).Nullable();
            Map(x => x.PaymentProfileId).Nullable();
            Map(x => x.ProcessingFee).Nullable();
            Map(x => x.MTOTFee).Nullable();
            Map(x => x.TrackingNumber).Nullable();
            Map(x => x.SubmittedAt).Nullable();
            Map(x => x.CanceledAt).Nullable();
            Map(x => x.PaidByNJAW).Not.Nullable();

            References(x => x.SubmittedBy, "SubmittedBy").Nullable();
            References(x => x.CanceledBy, "CanceledBy").Nullable();

            var statusFormula = "(SELECT CASE " +
                                "   WHEN (PaidByNJAW = 1) " +
                                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Cleared')" +
                                "   WHEN (CanceledAt is not null) " +
                                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Canceled')" +
                                "   WHEN (InvoiceDate is null) " +
                                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Open')" +
                                "   WHEN (InvoiceDate is not null AND PaymentReceivedAt is null) " +
                                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Awaiting Payment')" +
                                "   WHEN (PaymentReceivedAt is not null AND PaymentReceivedAt < DateAdd(day,-1, dbo.GetStartOfDay()) AND SubmittedAt is null) " +
                                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Pending Submittal')" +
                                "   WHEN (PaymentReceivedAt is not null AND SubmittedAt is null) " +
                                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Paid')" +
                                "   WHEN (SubmittedAt is not null AND EXISTS (SELECT 1 FROM TrafficControlTicketChecks tctc WHERE tctc.TrafficControlTicketId = Id AND tctc.Reconciled = 0))" +
                                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Submitted')" +
                                "   ELSE " +
                                "            (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Cleared')" +
                                "END)";
            var statusFormulaSqlite =
                "(SELECT CASE " +
                "   WHEN (PaidByNJAW = 1) " +
                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Cleared')" +
                "   WHEN (CanceledAt is not null) " +
                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Canceled')" +
                "   WHEN (InvoiceDate is null) " +
                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Open')" +
                "   WHEN (InvoiceDate is not null AND PaymentReceivedAt is null) " +
                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Awaiting Payment')" +
                "   WHEN (PaymentReceivedAt is not null AND PaymentReceivedAt < DateAddPLus('d',-1, dbo.GetStartOfDay()) AND SubmittedAt is null) " +
                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Pending Submittal')" +
                "   WHEN (PaymentReceivedAt is not null AND SubmittedAt is null) " +
                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Paid')" +
                "   WHEN (SubmittedAt is not null AND EXISTS (SELECT 1 FROM TrafficControlTicketChecks tctc WHERE tctc.TrafficControlTicketId = Id AND tctc.Reconciled = 0))" +
                "       THEN (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Submitted')" +
                "   ELSE " +
                "            (SELECT tcts.Id from TrafficControlTicketStatuses tcts where tcts.Description = 'Cleared')" +
                "END)";

            References(x => x.Status)
               .DbSpecificFormula(statusFormula, statusFormulaSqlite.Replace("dbo.", string.Empty));

            Map(x => x.HasInvoice)
               .Formula("CASE WHEN (InvoiceNumber IS NOT NULL) THEN 1 ELSE 0 END")
               .Not.Update()
               .Not.Insert();
            HasMany(x => x.TrafficControlDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.TrafficControlNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.TrafficcControlTicketChecks)
               .KeyColumn("TrafficControlTicketId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
