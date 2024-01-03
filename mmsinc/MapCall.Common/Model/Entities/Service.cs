using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations._2019;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.ChangeTracking;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Service
        : IEntityLookup,
            IThingWithNotes,
            IThingWithDocuments,
            IAsset,
            ISAPEntity,
            IThingWithCoordinate,
            IThingWithState,
            IThingWithShadow,
            IThingWithTownAndOperatingCenter,
            IEntityWithCreationTimeTracking,
            IThingWithSyncing
    {
        #region Constants

        public struct FormatStrings
        {
            public const string ADDRESS = "{0} {1} {2}, {3} {4}";
        }

        public struct Range
        {
            public const long SAP_RANGE_MIN = 10000000,
                              SAP_RANGE_MAX = 9999999999;
        }

        public struct DisplayNames
        {
            public const string
                ALT_PHONE_NUMBER = "Alt Phone Number",
                APT_NUM = "Apt/Bldg",
                APT_ADDTL = "Apartment Addtl",
                AGREEMENT = "Associated Deposit Agreement",
                BACKFLOW_DEVICE = "Backflow Prevention Device",
                CONTACT_DATE = "Initial Contact Date",
                CUSTOMER_SIDE_SL_REPLACEMENT = "Customer Side SL Replacement",
                CUSTOMER_SIDE_SL_REPLACED_BY = "Customer Side SL Replaced By",
                CUSTOMER_SIDE_SL_REPLACEMENT_CONTRACTOR = "Customer Side SL Replacement Contractor",
                DATE = "Date",
                DATE_INSTALLED = "Installed Date",
                FAX_NUMBER = "Fax Number",
                FLUSHING_CUSTOMER_PLUMBING = "Flushing of Customer Plumbing",
                GEO_LOCATION = "Geo.E Functional Location",
                LEAD_SERVICE_REPLACEMENT_WBS = "Lead Service Replacement WBS #",
                LEAD_SERVICE_RETIREMENT_WBS = "Lead Service Retirement WBS #",
                MAIN_SIZE = "Size of Main",
                METER_SET_ACCOUNT = "Meter Set Account",
                NSI_NUMBER = "NSI 42 Notification Number",
                OBJECT_ID = "ObjectID",
                PREMISE_NUMBER_UNAVAILABLE_REASON = "Premise Number Unavailable Reason",
                PREVIOUS_SERVICE_MATERIAL = "Previous Service Company Material",
                PREVIOUS_SERVICE_SIZE = "Previous Service Company Size",
                SERVICE_INSTALLATION_FEE = "Additional Service Installation Fee",
                SERVICE_ACCOUNT = "Service Account",
                SAP_ERROR = "SAP Status",
                SERVICE_COUNT = "Number of Services",
                STATUS = "Status",
                SERVICE_INSTALLATION_PURPOSE = "Purpose of Installation",
                SERVICE_CATEGORY = "Category of Service",
                SERVICE_MATERIAL = "Service Company Material",
                SERVICE_PRIORITY = "Priority",
                SERVICE_SIZE = "Service Company Size",
                TYPE_OF_MAIN = "Type of Main",
                WBS = "WBS #";
        }

        public struct StringLengths
        {
            public const int
                // If this is changed ApartmentAddtl in Work Order needs to be changed to match due to
                // mapping
                APARTMENT_NUMBER = 30, 
                BLOCK = 9,
                BUSINESS_PARTNER = 10,
                DEVELOPMENT = 30,
                EMAIL = 50,
                FAX = 12,
                INSTALLATION = 10,
                INSTALLATION_INVOICE_NUMBER = 20,
                LOT = 9,
                MAIL_PHONE_NUMBER = 12,
                MAIL_STATE = 2,
                MAIL_STREET_NAME = 30,
                MAIL_STREET_NUMBER = 10,
                MAIL_TOWN = 30,
                MAIL_ZIP = 10,
                NAME = 40,
                PARENT_TASK_NUMBER = 10,
                PAYMENT_REFERENCE_NUMBER = 15,
                PERMIT_NUMBER = 20,
                PHONE_NUMBER = 12,
                PREMISE_NUMBER = 10,
                PURCHASE_ORDER_NUMBER = 20,
                RETIRED_ACCOUNT_NUMBER = 10,
                RETIRE_METER_SET = 10,
                SERVICE_NUMBER = 10,
                STREET_NUMBER = 10,
                TASK_NUMBER_1 = 18,
                TASK_NUMBER_2 = 10,
                WBS = 25,
                WORK_ISSUED_TO = 30,
                ZIP = 10,
                LEGACY_ID = AddLegacyIdToServicesForMC1046.StringLengths.LEGACY_ID;
        }

        public struct StatusMessages
        {
            public const string
                INSTALLED = "The service was installed on {0:d}. ",
                ISSUED =
                    "The work was issued to the field on {0:d} but the service has not yet been " +
                    "installed. ",
                PERMIT_RECEIVED =
                    "The street opening permit was returned on {0:d} but has not yet been issued to the " +
                    "field. ",
                PERMIT_PENDING =
                    "The street opening permit applied for on {0:d} but has not yet been returned. ",
                APPLICATION_NOT_APPROVED =
                    "The New Service Application was received on {0:d} but has not yet been approved. ",
                APPLICATION_NOT_RETURNED =
                    "The New Service Application was sent on {0:d} but has not yet been returned. ",
                APPLICATION_NOT_SENT =
                    "The Initial Contact was on {0:d}. The Application has not yet been sent. ";
        }

        #endregion

        #region Fields

        [NonSerialized] private IServiceRepository _serviceRepository;
        [NonSerialized] private ITapImageRepository _tapImageRepository;
        [NonSerialized] private MMSINC.Data.NHibernate.IRepository<WorkOrder> _workOrderRepository;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        public virtual ServiceOfferedAgreementType OfferedAgreement { get; set; }

        [DisplayName(DisplayNames.AGREEMENT), BoolFormat("Yes", "No")]
        public virtual bool? Agreement { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? AmountReceived { get; set; }

        [DisplayName(DisplayNames.APT_NUM)]
        public virtual string ApartmentNumber { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ApplicationApprovedOn { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ApplicationReceivedOn { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ApplicationSentOn { get; set; }

        public virtual string Block { get; set; }

        [BoolFormat("Yes", "No")]
        public virtual bool? BureauOfSafeDrinkingWaterPermitRequired { get; set; }
        public virtual string BusinessPartner { get; set; }
        public virtual bool CleanedCoordinates { get; set; }

        [DisplayName(DisplayNames.CONTACT_DATE), DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ContactDate { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateClosed { get; set; }

        [DisplayName(DisplayNames.DATE_INSTALLED)]
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateInstalled { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateIssuedToField { get; set; }

        public virtual int? DepthMainFeet { get; set; }
        public virtual int? DepthMainInches { get; set; }

        [BoolFormat("Yes", "No")]
        public virtual bool? DeveloperServicesDriven { get; set; }
        public virtual string Development { get; set; }
        public virtual string Email { get; set; }

        [DisplayName(DisplayNames.FAX_NUMBER)]
        public virtual string Fax { get; set; }

        [DisplayName(DisplayNames.GEO_LOCATION)]
        public virtual string GeoEFunctionalLocation { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? InactiveDate { get; set; }

        [DisplayName("Date"), DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? InspectionDate { get; set; }

        public virtual decimal? InstallationCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? InstallationInvoiceDate { get; set; }

        public virtual string InstallationInvoiceNumber { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string JobNotes { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual bool? LeadAndCopperCommunicationProvided { get; set; }
        public virtual decimal? LengthOfService { get; set; }
        public virtual string Lot { get; set; }

        [DisplayName(DisplayNames.ALT_PHONE_NUMBER)]
        public virtual string MailPhoneNumber { get; set; }

        public virtual string MailState { get; set; }
        public virtual string MailStreetName { get; set; }
        public virtual string MailStreetNumber { get; set; }
        public virtual string MailTown { get; set; }
        public virtual string MailZip { get; set; }

        [BoolFormat("Yes", "No")]
        public virtual bool? MeterSettingRequirement { get; set; }
        public virtual string Name { get; set; }

        [DisplayName(DisplayNames.OBJECT_ID)]
        public virtual int? ObjectId { get; set; }

        [View(FormatStyle.Date, Description = "The date the offered agreement was signed or declined.")]
        public virtual DateTime? OfferedAgreementDate { get; set; }
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? OriginalInstallationDate { get; set; }

        [Multiline]
        public virtual string OtherPoint { get; set; }
        [View(DisplayNames.NSI_NUMBER)]
        public virtual int? NSINumber { get; set; }
        public virtual string ParentTaskNumber { get; set; }
        public virtual string PaymentReferenceNumber { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? PermitExpirationDate { get; set; }

        public virtual string PermitNumber { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? PermitReceivedDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? PermitSentDate { get; set; }

        public virtual string PhoneNumber { get; set; }
        public virtual string PremiseNumber { get; set; }

        // mc-244 asked for this to be a user rather than an employee
        // because they want to filter the users by their role's op center
        public virtual User ProjectManager { get; set; }
        public virtual string Installation { get; set; }
        public virtual string PurchaseOrderNumber { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? QuestionaireSentDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? QuestionaireReceivedDate { get; set; }

        [DisplayName(DisplayNames.SERVICE_ACCOUNT)]
        public virtual string RetiredAccountNumber { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? RetiredDate { get; set; }

        [DisplayName(DisplayNames.METER_SET_ACCOUNT)]
        public virtual string RetireMeterSet { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? RoadOpeningFee { get; set; }

        public virtual long? SAPWorkOrderNumber { get; set; }
        public virtual long? SAPNotificationNumber { get; set; }

        public virtual int? ServiceDwellingTypeQuantity { get; set; }

        [DisplayName(DisplayNames.SERVICE_INSTALLATION_FEE),
         DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? ServiceInstallationFee { get; set; }

        public virtual long? ServiceNumber { get; set; }

        [View("Re-grounding Premise")]
        public virtual ServiceRegroundingPremiseType ServiceRegroundingPremiseType { get; set; }

        public virtual string StreetNumber { get; set; }
        [View("Subfloor Type")]
        public virtual ServiceSubfloorCondition SubfloorCondition { get; set; }
        public virtual string TapOrderNotes { get; set; }

        [DisplayName(DisplayNames.WBS)]
        public virtual string TaskNumber1 { get; set; }

        public virtual string TaskNumber2 { get; set; }

        public virtual ServiceTerminationPoint TerminationPoint { get; set; }

        [View(Description = "Enter decade if exact year is unknown.")]
        public virtual int? YearOfHomeConstruction { get; set; }
        [DisplayName(DisplayNames.LEAD_SERVICE_REPLACEMENT_WBS)]
        public virtual string LeadServiceReplacementWbs { get; set; }
        [DisplayName(DisplayNames.LEAD_SERVICE_RETIREMENT_WBS)]
        public virtual string LeadServiceRetirementWbs { get; set; }
        public virtual string Zip { get; set; }

        public virtual int? ImageActionID { get; set; }

        public virtual int? LengthOfCustomerSideSLReplaced { get; set; }
        public virtual decimal? CustomerSideSLReplacementCost { get; set; }
        public virtual DateTime? CustomerSideReplacementDate { get; set; }
        public virtual DateTime? DateCreditProcessed { get; set; }
        public virtual string PlaceHolderNotes { get; set; }
        public virtual string DeviceLocation { get; set; }
        public virtual bool? DeviceLocationUnavailable { get; set; }

        [DisplayName(DisplayNames.SAP_ERROR)]
        public virtual string SAPErrorCode { get; set; }

        [BoolFormat("Yes", "No")]
        public virtual bool? PitInstalled { get; set; }
        public virtual string LegacyId { get; set; }
        public virtual string RecordUrl { get; set; }
        public virtual string RecordUrlMap { get; set; }
        public virtual bool NeedsToSync { get; set; }
        public virtual DateTime? LastSyncedAt { get; set; }

        #endregion

        #region Associations

        public virtual ServiceRestorationContractor WorkIssuedTo { get; set; }

        [DisplayName(DisplayNames.BACKFLOW_DEVICE)]
        public virtual BackflowDevice BackflowDevice { get; set; }

        [DoesNotExport]
        public virtual Coordinate Coordinate { get; set; }

        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;
        public virtual Street CrossStreet { get; set; }
        public virtual User Initiator { get; set; }

        [DisplayName(DisplayNames.STATUS)]
        public virtual ServiceStatus ServiceStatus { get; set; }

        [DisplayName(DisplayNames.MAIN_SIZE)]
        public virtual ServiceSize MainSize { get; set; }

        [DisplayName(DisplayNames.TYPE_OF_MAIN)]
        public virtual MainType MainType { get; set; }

        public virtual Service RenewalOf { get; set; }

        // Used to be: TapSize
        public virtual ServiceSize MeterSettingSize { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual PermitType PermitType { get; set; }
        public virtual PremiseType PremiseType { get; set; }
        [View(DisplayNames.PREVIOUS_SERVICE_MATERIAL)]
        public virtual ServiceMaterial PreviousServiceMaterial { get; set; }
        public virtual ServiceMaterial PreviousServiceCustomerMaterial { get; set; }
        [View(DisplayNames.PREVIOUS_SERVICE_SIZE)]
        public virtual ServiceSize PreviousServiceSize { get; set; }
        public virtual ServiceSize PreviousServiceCustomerSize { get; set; }

        [DisplayName(DisplayNames.SERVICE_INSTALLATION_PURPOSE)]
        public virtual ServiceInstallationPurpose ServiceInstallationPurpose { get; set; }

        [DisplayName(DisplayNames.SERVICE_CATEGORY)]
        public virtual ServiceCategory ServiceCategory { get; set; }

        public virtual ServiceDwellingType ServiceDwellingType { get; set; }
        public virtual ServiceMaterial CustomerSideMaterial { get; set; }
        [View(DisplayNames.SERVICE_MATERIAL)]
        public virtual ServiceMaterial ServiceMaterial { get; set; }

        [DisplayName(DisplayNames.SERVICE_PRIORITY)]
        public virtual ServicePriority ServicePriority { get; set; }

        [DisplayName(DisplayNames.SERVICE_SIZE)]
        public virtual ServiceSize ServiceSize { get; set; }

        //column is here so that it appears here in the excel file.

        [BoolFormat("Yes", "No")]
        public virtual bool? CompanyOwned { get; set; }
        public virtual ServiceSize CustomerSideSize { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual State State { get; set; }
        public virtual Street Street { get; set; }
        public virtual StreetMaterial StreetMaterial { get; set; }
        public virtual Town Town { get; set; }
        public virtual TownSection TownSection { get; set; }
        public virtual ServiceSideType ServiceSideType { get; set; }

        [DisplayName(DisplayNames.CUSTOMER_SIDE_SL_REPLACEMENT)]
        public virtual CustomerSideSLReplacementOfferStatus CustomerSideSLReplacement { get; set; }

        [DisplayName(DisplayNames.FLUSHING_CUSTOMER_PLUMBING)]
        public virtual FlushingOfCustomerPlumbingInstructions FlushingOfCustomerPlumbing { get; set; }

        [DisplayName(DisplayNames.CUSTOMER_SIDE_SL_REPLACED_BY)]
        public virtual CustomerSideSLReplacer CustomerSideSLReplacedBy { get; set; }

        [DisplayName(DisplayNames.CUSTOMER_SIDE_SL_REPLACEMENT_CONTRACTOR)]
        public virtual Contractor CustomerSideSLReplacementContractor { get; set; }

        public virtual WBSNumber CustomerSideReplacementWBSNumber { get; set; }

        public virtual bool? PremiseNumberUnavailable { get; set; }
        [View(DisplayNames.PREMISE_NUMBER_UNAVAILABLE_REASON)]
        public virtual PremiseUnavailableReason PremiseUnavailableReason { get; set; }

        // As of this writing (2022-08-24) there are more Service records which have a PremiseNumber than
        // have a Premise linked via PremiseId.  I could not find any Service records that had a
        // PremiseNumber which did not match the PremiseNumber on the Premise that it's linked to via
        // PremiseId.  Thus, it made much more sense to just exclude Premise from export since it's only
        // ever having .ToString() called on it which only returns PremiseNumber anyway; this avoids an
        // n+1 query issue.
        [DoesNotExport]
        public virtual Premise Premise { get; set; }

        public virtual User UpdatedBy { get; set; }

        public virtual DateTime? WarrantyExpirationDate
        {
            get
            {
                if (!CustomerSideReplacementDate.HasValue)
                {
                    return null;
                }

                return CustomerSideReplacementDate.Value.AddYears(1);
            }
        }

        public virtual IList<ServicePremiseContact> PremiseContacts { get; set; } =
            new List<ServicePremiseContact>();
        public virtual IList<ServiceFlush> Flushes { get; set; } = new List<ServiceFlush>();
        public virtual IList<TapImage> TapImages { get; set; } = new List<TapImage>();
        public virtual IList<ServiceRestoration> Restorations { get; set; }

        public virtual IList<ServiceLineProtectionInvestigation> ServiceLineProtectionInvestigations
        {
            get; set;
        }

        /// <summary>
        /// This is more like related services with the same operating center
        /// service number, and premise number, but they call it Renewals
        /// </summary>
        public virtual IEnumerable<Service> Renewals
        {
            get
            {
                return _serviceRepository
                   .Where(s => s.PremiseNumber == PremiseNumber
                               && s.OperatingCenter == OperatingCenter
                               && s.Id != Id);
            }
        }

        public virtual IList<ServiceServiceInstallationMaterial> ServiceInstallationMaterials { get; set; }
        public virtual IList<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();

        #endregion

        #region Logical Properties

        [DoesNotExport] // This slows down exporting significantly. 
        public virtual WorkOrder InstallationWorkOrder => WorkOrders.FirstOrDefault(x =>
            WorkDescription.GetMainBreakWorkDescriptions().Contains(x.WorkDescription.Id));

        [DoesNotExport] // This is used in Service History tab of a premise record. 
        public virtual WorkOrder MostRecentWorkOrder =>
            WorkOrders.OrderByDescending(x => x.Id).FirstOrDefault();

        /// <summary>
        /// By using this the view is able to select only the data that is required for the work orders
        /// tab view on the view. Without it we create a very ineffient query that can bring the sqlserver
        /// to a halt.
        /// This should be moved to a partial in the future. If you're changing this, move it to a partial
        /// note: it's used in the contractors view as well
        /// </summary>
        [DoesNotExport]
        public virtual IEnumerable<WorkOrder> WorkOrdersForDisplay => _workOrderRepository.GetByServiceId(Id);

        public virtual string JustStreetAddress =>
            $"{StreetNumber} {(string.IsNullOrWhiteSpace(ApartmentNumber) ? string.Empty : " Apt ")} " +
            $"{Street}, {Town}";

        public virtual string StreetAddress { get; set; }

        public virtual string Description
        {
            get
            {
                const string format = "[Id] {0} [Service] {1}, [Premise] {2}, [Service Type] {3}";
                return string.Format(format, Id, ServiceNumber, PremiseNumber, ServiceType);
            }
        }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? TotalFee => RoadOpeningFee + ServiceInstallationFee;

        public virtual string DepthMain
        {
            get
            {
                var sb = new StringBuilder();

                if (DepthMainFeet.HasValue)
                {
                    sb.Append(DepthMainFeet.Value + "'");
                }

                if (DepthMainInches.HasValue)
                {
                    sb.Append((DepthMainFeet.HasValue ? " " : "") + DepthMainInches.Value + "\"");
                }

                return sb.ToString();
            }
        }

        // This will probably turn into some type of logical status field so it can be searched.
        public virtual string StatusMessage
        {
            get
            {
                if (DateInstalled.HasValue)
                    return string.Format(StatusMessages.INSTALLED, DateInstalled);
                if (!DateInstalled.HasValue && DateIssuedToField.HasValue)
                    return string.Format(StatusMessages.ISSUED, DateIssuedToField);
                if (PermitSentDate.HasValue && PermitReceivedDate.HasValue)
                    return string.Format(StatusMessages.PERMIT_RECEIVED, PermitReceivedDate);
                if (PermitSentDate.HasValue && !PermitReceivedDate.HasValue)
                    return string.Format(StatusMessages.PERMIT_PENDING, PermitSentDate);
                if (ApplicationReceivedOn.HasValue && !ApplicationApprovedOn.HasValue)
                    return string.Format(StatusMessages.APPLICATION_NOT_APPROVED, ApplicationReceivedOn);
                if (ApplicationSentOn.HasValue && !ApplicationReceivedOn.HasValue)
                    return string.Format(StatusMessages.APPLICATION_NOT_RETURNED, ApplicationSentOn);
                if (!ApplicationSentOn.HasValue && !PermitSentDate.HasValue)
                    return string.Format(StatusMessages.APPLICATION_NOT_SENT, ContactDate);
                return string.Empty;
            }
        }

        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate.Icon;

        [DoesNotExport]
        public virtual string Identifier => $"p#:{PremiseNumber}, s#:{ServiceNumber}";

        [DoesNotExport]
        public virtual ServiceSAPErrorCodeType SAPErrorCodeType
        {
            get
            {
                if (SAPErrorCode == null)
                {
                    return ServiceSAPErrorCodeType.Unknown;
                }

                return SAPErrorCode
                   .Contains("Invalid Device Location", StringComparison.InvariantCultureIgnoreCase)
                    ? ServiceSAPErrorCodeType.InvalidDeviceLocation
                    : ServiceSAPErrorCodeType.Unknown;
            }
        }

        #region Docs/Notes

        #region Documents

        public virtual IList<ServiceDocument> ServiceDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return ServiceDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return ServiceDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<ServiceNote> ServiceNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return ServiceNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return ServiceNotes.Map(n => n.Note); }
        }

        #endregion

        [DoesNotExport]
        public virtual string TableName => nameof(Service) + "s";

        #endregion

        public virtual State ServiceState => Town.State;

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IServiceRepository ServiceRepository
        {
            set => _serviceRepository = value;
        }

        [SetterProperty]
        public virtual ITapImageRepository TapImageRepository
        {
            set => _tapImageRepository = value;
        }

        [SetterProperty]
        public virtual MMSINC.Data.NHibernate.IRepository<WorkOrder> WorkOrderRepository
        {
            set => _workOrderRepository = value;
        }

        #endregion

        #region Formula Fields

        public virtual int Month { get; set; }
        public virtual int Year { get; set; }
        public virtual int YearRetired { get; set; }
        public virtual bool Installed { get; set; }
        public virtual bool IssuedToField { get; set; }
        public virtual bool Invoiced { get; set; }
        public virtual bool Contacted { get; set; }
        public virtual bool HasTapImages { get; set; }
        public virtual DateTime? CustomerSideSLWarrantyExpiration { get; set; }

        public virtual IList<TapImage> RelatedTapImages
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PremiseNumber) ||
                    (int.TryParse(PremiseNumber, out int p) && p == 0))
                {
                    return new List<TapImage>();
                }

                return _tapImageRepository
                      .Where(x =>
                           x.OperatingCenter == OperatingCenter
                           && x.Town == Town
                           && x.PremiseNumber == PremiseNumber
                           && ((x.Service == null) || (x.Service != null && x.Service.Id != Id))
                       ).ToList();
            }
        }

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }

    public enum ServiceSAPErrorCodeType
    {
        Unknown = 0, // Default value 
        InvalidDeviceLocation
    }

    [Serializable]
    public class AggregatedService //: ViewModel<Service>
    {
        #region Properties

        //display
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }

        [DisplayName("Category of Service")]
        public virtual ServiceCategory ServiceCategory { get; set; }

        public virtual ServiceSize ServiceSize { get; set; }
        public virtual ServiceSize PreviousServiceSize { get; set; }

        //search
        [DisplayFormat(DataFormatString = "{0:yyyy}")]
        public virtual DateTime? DateInstalled { get; set; }

        // Aggregates
        public virtual decimal TotalFootage { get; set; }

        [DisplayName(Service.DisplayNames.SERVICE_ACCOUNT)]
        public virtual int ServiceCount { get; set; }

        public IEnumerable<Service> Services { get; set; }

        #endregion

        #region Constructors

        public AggregatedService()
        {
            Services = new List<Service>();
        }

        #endregion
    }

    [Serializable]
    public class ServiceStatus : EntityLookup
    {
        public struct Indices
        {
            public const int INSPECTED_AND_SITE_READY = 1, SITE_NOT_READY = 2;
        }
    }

    [Serializable]
    public class StreetMaterial : EntityLookup { }

    [Serializable]
    public class BackflowDevice : EntityLookup { }

    [Serializable]
    public class ServicePriority : EntityLookup
    {
        public struct Indices
        {
            public const int EMERGENCY = 1, RUSH_THREE_DAY = 2, ROUTINE = 3;
        }
    }

    [Serializable]
    public class ServiceInstallationPurpose : EntityLookup
    {
        public struct Indices
        {
            public const int
                STANDARD_RENEWAL = 1,
                MAKE_EXTERIOR_SETTING = 2,
                NEW_SERVICE = 3,
                MAIN_REPLACEMENT = 4,
                SERVICE_LINE_LEAK = 5,
                MEASUREMENT_ONLY = 6,
                RETIREMENT_ONLY = 7,
                MAIN_EXTENSION = 8,
                CUSTOMER_REQUEST = 9,
                MATERIAL_VERIFICATION = 12;
        }

        public virtual string CodeGroup => "N-D-PUR1";
        public virtual string SAPCode { get; set; }
    }

    [Serializable]
    public class ServiceDwellingType : EntityLookup
    {
        #region Properties

        public virtual int WaterGPD { get; set; }
        public virtual int SewerGPD { get; set; }

        #endregion
    }

    [Serializable]
    public class CustomerSideSLReplacementOfferStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int NO = 1, ACCEPTED = 2, REJECTED = 3;
        }
    }

    [Serializable]
    public class FlushingOfCustomerPlumbingInstructions : ReadOnlyEntityLookup { }

    [Serializable]
    public class CustomerSideSLReplacer : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int CONTRACTOR = 2;
        }
    }

    public class ServiceNotification
    {
        public Service Service { get; set; }
        public string UserEmail { get; set; }
    }

    [Serializable]
    public class ServiceSideType : ReadOnlyEntityLookup { }

    #region Reports

    #region BPU Report for Services

    public interface ISearchBPUReportForServices : ISearchSet<BPUReportForServiceReportItem>
    {
        int? OperatingCenter { get; set; }
        int Year { get; set; }
    }

    public class BPUReportForServiceReportItem
    {
        public OperatingCenter OperatingCenter { get; set; }
        public int Year { get; set; }
        public ServiceSize InstalledSize { get; set; }
        public ServiceMaterial InstalledType { get; set; }
        public int InstalledNew { get; set; }
        public int Replaced { get; set; }
    }

    #endregion

    #region Monthly Services Installed By Category

    public interface ISearchMonthlyServicesInstalledByCategory : ISearchSet<MonthlyServicesInstalledByCategoryViewModel>
    {
        int[] OperatingCenter { get; set; }
        int Year { get; set; }
    }

    public class MonthlyServicesInstalledByCategoryViewModel
    {
        public OperatingCenter OperatingCenter { get; set; }
        public ServiceCategory ServiceCategory { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Total { get; set; }
    }

    public class MonthlyServicesInstalledByCategoryReportViewModel : MonthlyReportViewModel
    {
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
    }

    #endregion

    #region Services Retired

    public interface ISearchServicesRetired : ISearchSet<ServicesRetiredReportItem>
    {
        int? OperatingCenter { get; set; }
        int? Town { get; set; }
        int? YearRetired { get; set; }
    }

    public class ServicesRetiredReportItem
    {
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }
        public virtual int YearRetired { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }

        public virtual ServiceMaterial PreviousServiceMaterial { get; set; }

        //public virtual ServiceSize PreviousServiceSize { get; set; }
        public virtual decimal PreviousServiceSize { get; set; }

        [DisplayName("Previous Service Size")]
        public virtual string PreviousServiceSizeDescription { get; set; }

        public virtual int Total { get; set; }
    }

    #endregion

    #region Services Renewed

    public interface ISearchServicesRenewedSummary : ISearchSet<ServicesRenewedSummaryReportItem>
    {
        int? OperatingCenter { get; set; }
        IntRange Year { get; set; }
    }

    public class ServicesRenewedSummaryReportItem
    {
        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual string Town { get; set; }

        // Installed
        public virtual int Year { get; set; }

        [DisplayName("Total Renewed")]
        public virtual int Total { get; set; }
    }

    #endregion

    #region ServicesCompletedByCategory

    public interface ISearchServicesCompletedByCategory : ISearchSet<ServicesCompletedByCategoryReportItem>
    {
        int? OperatingCenter { get; set; }
        DateRange DateInstalled { get; set; }
        bool? DeveloperServicesDriven { get; set; }
    }

    public class ServicesCompletedByCategoryReportItem
    {
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
        public virtual int Total { get; set; }
    }

    #endregion

    #region TDPendingServicesKPI

    public interface ISearchTDPendingServicesKPI : ISearchSet<TDPendingServicesKPIReportItem>
    {
        int? OperatingCenter { get; set; }
    }

    public class TDPendingServicesKPIReportItem
    {
        public const string SECTION_SERVICE = "Service", SECTION_CONTRACTOR = "Contractor";

        public struct Category
        {
            public const string
                WATER_SERVICE_RENEWAL_PENDING_PERMITS = "Water Service Renewals - Pending Permits",
                WATER_SERVICE_ISSUED_TO_FIELD = "Water Service Renewals - Issued To Field",
                NEW_WATER_SERVICES_APPROVED_APPLICATION = "New Water Services - Approved Applications",
                NEW_WATER_SERVICES_PERMITS_PENDING = "New Water Services - Permits Pending",
                NEW_WATER_SERVICES_ISSUED_TO_FIELD = "New Water Services - Issued To Field",
                NEW_WATER_SERVICES_SITE_NOT_READY = "New Water Services - Site Not Ready",
                SEWER_SERVICES = "Sewer";
        }

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string Section { get; set; }
        public virtual string ServicesContractor { get; set; }
        public virtual int Total { get; set; }
    }

    #endregion

    #endregion
}
