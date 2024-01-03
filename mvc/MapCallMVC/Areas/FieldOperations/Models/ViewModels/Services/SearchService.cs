using System.ComponentModel;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services
{
    public class SearchService : SearchSet<Service>
    {
        #region Properties

        //Service Number:
        public long? ServiceNumber { get; set; }
        public SearchString LegacyId { get; set; }
        //Premise Number:
        public SearchString PremiseNumber { get; set; }
        //Operating Center:
        [DropDown("", "OperatingCenter", "ByStateIdForFieldServicesAssets", DependsOn = "State", DependentsRequired = DependentRequirement.None)]
        public int? OperatingCenter { get; set; }

        //Town:
        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }

        //Street Name:
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? Street { get; set; }

        //Cross Street:
        [DropDown("", "Street", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? CrossStreet { get; set; }

        //Town Section:
        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        public int? TownSection { get; set; }

        [MultiSelect]
        public int[] ServicePriority { get; set; }

        //Customer Name:
        [View("Customer Name")]
        public SearchString Name { get; set; }

        //Phone Number:
        public SearchString PhoneNumber { get; set; }
        //Street Number:
        public SearchString StreetNumber { get; set; }
        //Apt/Bldg:
        [View("Apartment Addtl")]
        public SearchString ApartmentNumber { get; set; }
        //Development:
        public SearchString Development { get; set; }
        //Lot:
        public SearchString Lot { get; set; }
        //Block:
        public SearchString Block { get; set; }
        //Category of Service:
        [MultiSelect, DisplayName("Category of Service")]
        public int[] ServiceCategory { get; set; }

        //WBS #:
        [View("WBS #")]
        public SearchString TaskNumber1 { get; set; }
        [DisplayName(Service.DisplayNames.LEAD_SERVICE_REPLACEMENT_WBS)]
        public SearchString LeadServiceReplacementWbs { get; set; }
        [DisplayName(Service.DisplayNames.LEAD_SERVICE_RETIREMENT_WBS)]
        public SearchString LeadServiceRetirementWbs { get; set; }

        //Service Size:\
        [DropDown, View(Service.DisplayNames.SERVICE_SIZE)]
        public int? ServiceSize { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSize))]
        public int? PreviousServiceCustomerSize { get; set; }

        //Installed Date:
        [View("Installed Date")]
        public DateRange DateInstalled { get; set; }

        public DateRange UpdatedAt { get; set; }
        public DateRange ContactDate { get; set; }
        public DateRange InstallationInvoiceDate { get; set; }

        [DropDown]
        public int? ServiceInstallationPurpose { get; set; }

        public bool? DeveloperServicesDriven { get; set; }
        public bool? Installed { get; set; }
        public bool? IssuedToField { get; set; }
        public bool? IsActive { get; set; }
        public bool? Invoiced { get; set; }
        public bool? Contacted { get; set; }
        public bool? HasTapImages { get; set; }

        [DropDown("", "ServiceRestorationContractor", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? WorkIssuedTo { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial)), View(Service.DisplayNames.SERVICE_MATERIAL)]
        public int? ServiceMaterial { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial)), View(Service.DisplayNames.PREVIOUS_SERVICE_MATERIAL)]
        public int? PreviousServiceMaterial { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? CustomerSideMaterial { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public int? PreviousServiceCustomerMaterial { get; set; }

        [View(DisplayName="SAP Status")]
        public string SAPErrorCode { get; set; }

        [DropDown, SearchAlias("Town", "State.Id")]
        public int? State { get; set; }

        [View("Re-grounding Premise")]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceRegroundingPremiseType))]
        public int? ServiceRegroundingPremiseType { get; set; }

        [View("Subfloor Type")]
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceSubfloorCondition))]
        public int? SubfloorCondition { get; set; }

        [DropDown("", "User", "FieldServicesAssetsUsersByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Select an operating center"), EntityMap, EntityMustExist(typeof(User))]
        public int? ProjectManager { get; set; }

        public IntRange YearOfHomeConstruction { get; set; }

        public bool? CompanyOwned { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PremiseUnavailableReason))]
        [View(DisplayName = Service.DisplayNames.PREMISE_NUMBER_UNAVAILABLE_REASON)]
        public int? PremiseUnavailableReason { get; set; }

        #endregion
    }
}