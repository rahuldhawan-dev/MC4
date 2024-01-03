using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AuthorizeNet;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TrafficControlTicket : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes,
        IThingWithCoordinate, IEntityWithPayment
    {
        #region Constants

        public const decimal PROCESSING_FEE = 12m;

        #endregion

        #region Fields

        [NonSerialized] private IRepository<TrafficControlTicket> _repository;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate.Icon;

        [Required]
        public virtual int SAPWorkOrderNumber { get; set; }

        [Required, StringLength(20)]
        public virtual string StreetNumber { get; set; }

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime WorkStartDate { get; set; }

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? WorkEndDate { get; set; }

        [Required, DisplayName("Total Combined Hours")]
        public virtual decimal TotalHours { get; set; }

        [Required, DisplayName("Number of Officers or Employees")]
        public virtual int NumberOfOfficers { get; set; }

        [StringLength(CreateTrafficControlTicketsForBug2341.StringLengths.TrafficControlTickets.ACCOUNTING_CODE)]
        public virtual string AccountingCode { get; set; }

        [StringLength(CreateTrafficControlTicketsForBug2341.StringLengths.TrafficControlTickets.INVOICE_NUMBER)]
        public virtual string InvoiceNumber { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? InvoiceAmount { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? InvoiceDate { get; set; }

        public virtual decimal? InvoiceTotalHours { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? DateApproved { get; set; }

        public virtual bool HasInvoice { get; protected set; }

        [DisplayName("Notes")]
        public virtual string TrafficControlTicketNotes { get; set; }

        public virtual bool PaidByNJAW { get; set; }

        #region Payment/Invoices

        public virtual DateTime? PaymentReceivedAt { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? TotalCharged { get; set; }

        public virtual string PaymentTransactionId { get; set; }
        public virtual string PaymentAuthorizationCode { get; set; }
        public virtual string PaymentProfileId { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? ProcessingFee { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? MTOTFee { get; set; }

        public virtual string TrackingNumber { get; set; }
        public virtual DateTime? SubmittedAt { get; set; }
        public virtual DateTime? CanceledAt { get; set; }

        public virtual User SubmittedBy { get; set; }
        public virtual User CanceledBy { get; set; }

        #endregion

        #endregion

        #region References

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public virtual Street Street { get; set; }
        public virtual Street CrossStreet { get; set; }
        public virtual Town Town { get; set; }
        public virtual User ApprovedBy { get; set; }
        public virtual BillingParty BillingParty { get; set; }
        public virtual MerchantTotalFee MerchantTotalFee { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual IList<TrafficControlTicketCheck> TrafficcControlTicketChecks { get; set; }
        public virtual IList<TrafficControlDocument> TrafficControlDocuments { get; set; }
        public virtual IList<TrafficControlNote> TrafficControlNotes { get; set; }

        #endregion

        #region Logical Properties

        [DoesNotExport]
        public virtual string TableName => nameof(TrafficControlTicket) + "s";

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return TrafficControlDocuments.Map(td => (IDocumentLink)td); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return TrafficControlNotes.Map(td => (INoteLink)td); }
        }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal EstimatedInvoiceAmount =>
            //total hours - estimated hourly rate
            (BillingParty != null && BillingParty.EstimatedHourlyRate.HasValue)
                ? BillingParty.EstimatedHourlyRate.Value * TotalHours
                : 0;

        public virtual bool InvoiceValid =>
            (InvoiceAmount.HasValue
             && InvoiceDate.HasValue
             && InvoiceTotalHours.HasValue
             && !String.IsNullOrWhiteSpace(InvoiceNumber));

        [DisplayFormat(DataFormatString = CommonStringFormats.PERCENTAGE)]
        public virtual decimal? InvoicePercentageError
        {
            get
            {
                if (InvoiceAmount.HasValue && EstimatedInvoiceAmount > 0)
                {
                    return Math.Abs(EstimatedInvoiceAmount - InvoiceAmount.Value) / EstimatedInvoiceAmount;
                }

                return null;
            }
        }

        public virtual TrafficControlTicketStatus Status { get; set; }

        /// <summary>
        /// Returns true if a user has marked this traffic control request as canceled. 
        /// </summary>
        public virtual bool IsCanceled => CanceledAt.HasValue;

        /// <summary>
        /// Returns true if a user has paid for this traffic control request
        /// </summary>
        public virtual bool IsPaidFor => PaymentReceivedAt.HasValue || PaidByNJAW;

        public virtual bool HasChecksForCorrectAmount
        {
            get
            {
                var checkSum = TrafficcControlTicketChecks.Sum(x => x.Amount);
                var ret = checkSum == TotalCharged - ProcessingFee - MTOTFee;
                return ret;
            }
        }

        public virtual bool CanBeSubmitted => IsPaidFor && HasChecksForCorrectAmount && !SubmittedAt.HasValue &&
                                              !String.IsNullOrWhiteSpace(TrackingNumber);

        #region Related Tickets

        public virtual IList<TrafficControlTicket> RelatedWorkOrderTickets
        {
            get
            {
                return
                    _repository.Where(x => WorkOrder != null
                                           && x.WorkOrder != null
                                           && x.WorkOrder == WorkOrder
                                           && Id != x.Id).ToList();
            }
        }

        public virtual IList<TrafficControlTicket> RelatedSAPWorkOrderTickets
        {
            get
            {
                return
                    _repository.Where(x => SAPWorkOrderNumber != 0
                                           && x.SAPWorkOrderNumber == SAPWorkOrderNumber
                                           && Id != x.Id).ToList();
            }
        }

        public virtual IList<TrafficControlTicket> RelatedTownStreetTickets
        {
            get
            {
                return
                    _repository.Where(x => Town == x.Town
                                           && Street == x.Street
                                           && Id != x.Id).ToList();
            }
        }

        public virtual string AuthorizeNetInvoiceNumber => String.Format("{0,8}", Id.ToString("D8"));

        public virtual bool Payable => (InvoiceValid && !IsCanceled && !IsPaidFor && !SubmittedAt.HasValue &&
                                        !String.IsNullOrWhiteSpace(AccountingCode));

        #endregion

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IRepository<TrafficControlTicket> Repository
        {
            set => _repository = value;
        }

        #endregion

        #endregion

        #region Constructors

        public TrafficControlTicket()
        {
            TrafficControlDocuments = new List<TrafficControlDocument>();
            TrafficControlNotes = new List<TrafficControlNote>();
            TrafficcControlTicketChecks = new List<TrafficControlTicketCheck>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion

        public virtual void CalculateFees()
        {
            ProcessingFee = PROCESSING_FEE;
            MTOTFee = (ProcessingFee.Value + InvoiceAmount.Value) * MerchantTotalFee.Fee;
            TotalCharged = ProcessingFee.Value + InvoiceAmount.Value + MTOTFee;
        }
    }

    /// <summary>
    /// Represents a traffic control ticket and the payment profile (credit card) that the
    /// current user has chosen to use to pay to submit the traffic control ticket.
    /// </summary>
    [Serializable]
    public class VerifyPaymentSummaryTrafficControlTicket : IEntity
    {
        #region Properties

        public int Id { get; set; }
        public TrafficControlTicket TrafficControlTicket { get; set; }
        public string SelectedPaymentProfileId { get; set; }
        public PaymentProfile SelectedPaymentProfile { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents a traffic control ticket and the response from Authorize.Net after the
    /// system has tried to authorize payment for it.
    /// </summary>
    [Serializable]
    public class PaymentTrafficControlTicket
    {
        #region Properties

        public TrafficControlTicket TrafficControlTicket { get; set; }
        public IGatewayResponse Response { get; set; }

        #endregion
    }
}
