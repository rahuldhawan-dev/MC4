using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Library;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Validation;
using MapCallImporter.ViewModels;
using MMSINC.Utilities.ObjectMapping;
using IUnitOfWork = MMSINC.Data.V2.IUnitOfWork;

namespace MapCallImporter.Models.Import
{
    public class ServiceExcelRecord : ExcelRecordBase<Service, MyCreateService, ServiceExcelRecord>
    {
        #region Properties

        public object Id { get; set; }
        [AutoMap(SecondaryPropertyName = "Agreement")]
        public bool AssociatedDepositAgreement { get; set; }
        public decimal? AmountReceived { get; set; }
        [AutoMap(SecondaryPropertyName = "ApartmentNumber")]
        public string AptBldg { get; set; }
        public string Block { get; set; }
        public bool BureauOfSafeDrinkingWaterPermitRequired { get; set; }
        public bool CleanedCoordinates { get; set; }
        [AutoMap(SecondaryPropertyName = "ContactDate")]
        public DateTime? InitialContactDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DateClosed { get; set; }
        [AutoMap(SecondaryPropertyName = "DateInstalled")]
        public DateTime? InstalledDate { get; set; }
        public DateTime? DateIssuedToField { get; set; }
        public int? DepthMainFeet { get; set; }
        public int? DepthMainInches { get; set; }
        public bool DeveloperServicesDriven { get; set; }
        public string Development { get; set; }
        public string GeoEFunctionalLocation { get; set; }
        public decimal? InstallationCost { get; set; }
        public DateTime? InstallationInvoiceDate { get; set; }
        public string InstallationInvoiceNumber { get; set; }
        public bool? IsActive { get; set; }
        public string JobNotes { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool LeadAndCopperCommunicationProvided { get; set; }
        public decimal? LengthOfService { get; set; }
        public string Lot { get; set; }
        public bool MeterSettingRequirement { get; set; }
        public string Name { get; set; }
        [AutoMap(SecondaryPropertyName = "ObjectId")]
        public int? ObjectID { get; set; }
        public DateTime? OriginalInstallationDate { get; set; }
        public int? NSINumber { get; set; }
        public string ParentTaskNumber { get; set; }
        public string PaymentReferenceNumber { get; set; }
        public DateTime? PermitExpirationDate { get; set; }
        public string PermitNumber { get; set; }
        public DateTime? PermitReceivedDate { get; set; }
        public DateTime? PermitSentDate { get; set; }
        public string PhoneNumber { get; set; }
        public string PremiseNumber { get; set; }
        public string Installation { get; set; }
        public string PurchaseOrderNumber { get; set; }
        [AutoMap(SecondaryPropertyName ="RetiredAccountNumber")]
        public string ServiceAccount { get; set; }
        public DateTime? RetiredDate { get; set; }
        [AutoMap(SecondaryPropertyName = "RetireMeterSet")]
        public string MeterSetAccount { get; set; }
        public decimal? RoadOpeningFee { get; set; }
        public long? SAPWorkOrderNumber { get; set; }
        public long? SAPNotificationNumber { get; set; }
        [AutoMap(SecondaryPropertyName = "ServiceInstallationFee")]
        public decimal? AdditionalServiceInstallationFee { get; set; }
        public long? ServiceNumber { get; set; }
        public string StreetNumber { get; set; }
        public string TapOrderNotes { get; set; }
        [AutoMap(SecondaryPropertyName = "TaskNumber1")]
        public string WBS { get; set; }
        public string TaskNumber2 { get; set; }
        public string Zip { get; set; }
        public int? ImageActionID { get; set; }
        public int? LengthOfCustomerSideSLReplaced { get; set; }
        public decimal? CustomerSideSLReplacementCost { get; set; }
        public DateTime? CustomerSideReplacementDate { get; set; }
        public DateTime? DateCreditProcessed { get; set; }
        public string DeviceLocation { get; set; }
        public bool DeviceLocationUnavailable { get; set; }
        [AutoMap(SecondaryPropertyName = "SAPErrorCode")]
        public string SAPStatus { get; set; }
        public bool? PitInstalled { get; set; }
        public string WorkIssuedTo { get; set; }
        public object CoordinateID { get; set; }
        public string CrossStreet { get; set; }
        public object Initiator { get; set; }
        public string SizeofMain { get; set; }
        public string TypeofMain { get; set; }
        public object RenewalOf { get; set; }
        public string MeterSettingSize { get; set; }
        public string OperatingCenter { get; set; }
        public string PermitType { get; set; }
        public string PreviousServiceMaterial { get; set; }
        public string PreviousServiceSize { get; set; }
        public string PurposeofInstallation { get; set; }
        public string CategoryofService { get; set; }
        public string CustomerSideMaterial { get; set; }
        public string ServiceMaterial { get; set; }
        public string Priority { get; set; }
        public string SizeofService { get; set; }
        public string CustomerSideSize { get; set; }
        public object ServiceType { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string StreetMaterial { get; set; }
        public string Town { get; set; }
        public string TownSection { get; set; }
        public string ServiceSideType { get; set; }
        public string CustomerSideSLReplacement { get; set; }
        public string FlushingofCustomerPlumbing { get; set; }
        public string CustomerSideSLReplacedBy { get; set; }
        public string CustomerSideSLReplacementContractor { get; set; }
        public string CustomerSideReplacementWBSNumber { get; set; }
        public object JustStreetAddress { get; set; }
        public string StreetAddress { get; set; }
        public object Description { get; set; }
        public object TotalFee { get; set; }
        public object DepthMain { get; set; }
        public object StatusMessage { get; set; }
        public object ServiceState { get; set; }
        public object Month { get; set; }
        public object Year { get; set; }
        public object YearRetired { get; set; }
        public object Installed { get; set; }
        public object IssuedToField { get; set; }
        public object Invoiced { get; set; }
        public bool Contacted { get; set; }
        public object HasTapImages { get; set; }
        public DateTime? CustomerSideSLWarrantyExpiration { get; set; }

        #endregion

        #region Exposed Methods

        protected override MyCreateService MapExtra(MyCreateService viewModel, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Service> helper)
        {
            viewModel = base.MapExtra(viewModel, uow, index, helper);

            viewModel.OperatingCenter =
                CommonModelMethods.FindOperatingCenter(OperatingCenter, nameof(OperatingCenter), uow, index, helper);
            viewModel.ServiceCategory = StringToEntityLookup<ServiceCategory>(uow, index, helper,
                nameof(CategoryofService),
                CategoryofService);
            viewModel.State =
                StringToEntity<State>(uow, index, helper, nameof(State), State, s => s.Abbreviation == State);
            viewModel.Town = CommonModelMethods.LookupTown(viewModel.OperatingCenter, OperatingCenter, Town,
                nameof(Town), uow, index, helper);
            viewModel.Street =
                CommonModelMethods.LookupStreet(viewModel.Town, Town, Street, nameof(Street), uow, index, helper);
            viewModel.CrossStreet = CommonModelMethods.LookupStreet(viewModel.Town, Town, CrossStreet,
                nameof(CrossStreet), uow, index, helper);
            viewModel.CustomerSideMaterial =
                StringToEntityLookup<ServiceMaterial>(uow, index, helper, nameof(CustomerSideMaterial),
                    CustomerSideMaterial);
            viewModel.CustomerSideReplacementWBSNumber = StringToEntityLookup<WBSNumber>(uow, index, helper,
                nameof(CustomerSideReplacementWBSNumber), CustomerSideReplacementWBSNumber);
            viewModel.CustomerSideSize = StringToEntity<ServiceSize>(uow, index, helper, nameof(CustomerSideSize),
                CustomerSideSize, x => x.ServiceSizeDescription == CustomerSideSize);
            viewModel.CustomerSideSLReplacedBy = StringToEntityLookup<CustomerSideSLReplacer>(uow, index, helper,
                nameof(CustomerSideSLReplacedBy),
                CustomerSideSLReplacedBy);
            viewModel.CustomerSideSLReplacement = StringToEntityLookup<CustomerSideSLReplacementOfferStatus>(uow, index,
                helper,
                nameof(CustomerSideSLReplacement), CustomerSideSLReplacement);
            viewModel.CustomerSideSLReplacementContractor =
                StringToEntity<Contractor>(uow, index, helper, nameof(CustomerSideSLReplacementContractor),
                    CustomerSideSLReplacementContractor, c => c.Name == CustomerSideSLReplacementContractor);
            viewModel.CustomerSideMaterial =
                StringToEntityLookup<ServiceMaterial>(uow, index, helper, nameof(CustomerSideMaterial),
                    CustomerSideMaterial);
            viewModel.ServiceMaterial =
                StringToEntityLookup<ServiceMaterial>(uow, index, helper, nameof(ServiceMaterial), ServiceMaterial);
            viewModel.MainSize = StringToEntity<ServiceSize>(uow, index, helper, nameof(SizeofMain), SizeofMain,
                x => x.ServiceSizeDescription == SizeofMain);
            viewModel.MeterSettingSize = StringToEntity<ServiceSize>(uow, index, helper, nameof(MeterSettingSize),
                MeterSettingSize, x => x.ServiceSizeDescription == MeterSettingSize);
            viewModel.PreviousServiceMaterial = StringToEntityLookup<ServiceMaterial>(uow, index, helper,
                nameof(PreviousServiceMaterial), PreviousServiceMaterial);
            viewModel.PreviousServiceSize =
                StringToEntity<ServiceSize>(uow, index, helper, nameof(PreviousServiceSize), PreviousServiceSize,
                    x => x.ServiceSizeDescription == PreviousServiceSize);
            viewModel.ServiceInstallationPurpose = StringToEntityLookup<ServiceInstallationPurpose>(uow, index, helper,
                nameof(PurposeofInstallation),
                PurposeofInstallation);
            viewModel.ServicePriority =
                StringToEntityLookup<ServicePriority>(uow, index, helper, nameof(Priority), Priority);
            viewModel.ServiceSize = StringToEntity<ServiceSize>(uow, index, helper, nameof(SizeofService),
                SizeofService, x => x.ServiceSizeDescription == SizeofService);
            viewModel.WorkIssuedTo = LookupWorkIssuedTo(viewModel.OperatingCenter, uow, index, helper);
            viewModel.FlushingOfCustomerPlumbing = StringToEntityLookup<FlushingOfCustomerPlumbingInstructions>(uow,
                index, helper,
                nameof(FlushingofCustomerPlumbing), FlushingofCustomerPlumbing);
            viewModel.PermitType = StringToEntityLookup<PermitType>(uow, index, helper, nameof(PermitType), PermitType);
            viewModel.ServiceSideType =
                StringToEntityLookup<ServiceSideType>(uow, index, helper, nameof(ServiceSideType), ServiceSideType);
            viewModel.StreetMaterial =
                StringToEntityLookup<StreetMaterial>(uow, index, helper, nameof(StreetMaterial), StreetMaterial);
            viewModel.MainType = StringToEntityLookup<MainType>(uow, index, helper, nameof(TypeofMain), TypeofMain);
            viewModel.TownSection = LookupTownSection(viewModel.Town, uow, index, helper);

            return viewModel;
        }

        private int? LookupWorkIssuedTo(int? operatingCenter, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Service> helper)
        {
            if (string.IsNullOrWhiteSpace(WorkIssuedTo))
            {
                return null;
            }

            if (operatingCenter == null)
            {
                helper.AddFailure($"Row {index}: Cannot map a service restoration contractor ({nameof(WorkIssuedTo)}) without an operating center.");
                return null;
            }

            var found = uow.Where<ServiceRestorationContractor>(src =>
                                src.Contractor == WorkIssuedTo && src.OperatingCenter.Id == operatingCenter.Value)
                           .Select(src => src.Id)
                           .ToList();

            if (!found.Any())
            {
                helper.AddFailure($"Row {index}: Cannot find service restoration contractor ({nameof(WorkIssuedTo)}) '{WorkIssuedTo}' within {nameof(OperatingCenter)} '{OperatingCenter}'.");
                return null;
            }
            if (found.Count > 1)
            {
                helper.AddFailure($"Row {index}: Found more than one service restoration contractor ({nameof(WorkIssuedTo)}) '{WorkIssuedTo}' within {nameof(OperatingCenter)} '{OperatingCenter}'.");
                return null;
            }

            return found.Single();
        }

        private int? LookupTownSection(int? town, IUnitOfWork uow, int index, ExcelRecordItemHelperBase<Service> helper)
        {
            if (string.IsNullOrWhiteSpace(TownSection))
            {
                return null;
            }

            if (town == null)
            {
                helper.AddFailure($"Row {index}: Cannot map a town section without town.");
                return null;
            }

            var found = uow.Where<TownSection>(ts => ts.Town.Id == town.Value && ts.Name == TownSection)
                           .Select(ts => ts.Id)
                           .ToList();

            if (!found.Any())
            {
                helper.AddFailure($"Row {index}: Cannot find {nameof(TownSection)} '{TownSection}' within {nameof(Town)} '{Town}'.");
                return null;
            }

            return found.Single();
        }

        public override Service MapAndValidate(IUnitOfWork uow, int index, ExcelRecordItemValidationHelper<Service, MyCreateService, ServiceExcelRecord> helper)
        {
            return MapToEntity(uow, index, helper);
        }

        #endregion
    }
}
