using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Validation;
using MapCall.Common.Metadata;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.ServiceLineProtection.Models.ViewModels
{
    public class ServiceLineProtectionInvestigationViewModel : ViewModel<ServiceLineProtectionInvestigation>
    {
        #region Properties

        [Required, StringLength(ServiceLineProtectionInvestigation.StringLengths.CUSTOMER_NAME)]
        public string CustomerName { get; set; }

        [Required, StringLength(ServiceLineProtectionInvestigation.StringLengths.STREET_NUMBER)]
        public string StreetNumber { get; set; }

       
        [StringLength(ServiceLineProtectionInvestigation.StringLengths.CUSTOMER_ADDRESS)]
        public string CustomerAddress2 { get; set; }

        [Required, DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Town))]
        [DropDown("", "Town", "ByStateIdWithCounty", DependsOn = "State", PromptText = "Select a state above")]
        public int? CustomerCity { get; set; }

        [StringLength(ServiceLineProtectionInvestigation.StringLengths.CUSTOMER_ZIP)]
        [Required]
        public string CustomerZip { get; set; }

        [StringLength(ServiceLineProtectionInvestigation.StringLengths.CUSTOMER_PHONE)]
        public string CustomerPhone { get; set; }

        [StringLength(ServiceLineProtectionInvestigation.StringLengths.ACCOUNT_NUMBER)]
        public string AccountNumber { get; set; }

        [StringLength(ServiceLineProtectionInvestigation.StringLengths.PREMISE_NUMBER)]
        [Required]
        public string PremiseNumber { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [DropDown("", "OperatingCenter", "ByTownId", DependsOn = "CustomerCity", PromptText = "Select a town above.")]
        public int? OperatingCenter { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        [DropDown]
        public int? CustomerServiceMaterial { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(ServiceSize))]
        [DropDown]
        public int? CustomerServiceSize { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(ServiceLineProtectionWorkType))]
        [DropDown]
        public int? WorkType { get; set; }

        [Required,Coordinate(IconSet = IconSets.SingleDefaultIcon, AddressCallback = "ServiceLineProtectionInvestigationForm.getAddress"), EntityMap]
        public int? Coordinate { get; set; }

        [Multiline,StringLength(ServiceLineProtectionInvestigation.StringLengths.NOTES)]
        public string TheNotes { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Contractor)), DropDown]
        public int? Contractor { get; set; }

        #endregion

        #region Constructors

        public ServiceLineProtectionInvestigationViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateServiceLineProtectionInvestigation : ServiceLineProtectionInvestigationViewModel
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "CustomerCity", PromptText = "Please select a customer city above")]
        public int? Street { get; set; }
        
        #endregion

        #region Constructors

        public CreateServiceLineProtectionInvestigation(IContainer container) : base(container) {}

        #endregion
	}

    public class EditServiceLineProtectionInvestigation : ServiceLineProtectionInvestigationViewModel
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(Street))]
        [DropDown("", "Street", "ByTownId", DependsOn = "CustomerCity", PromptText = "Please select a customer city above")]
        public int? Street { get; set; }

        [EntityMap, EntityMustExist(typeof(Service))]
        [DropDown("FieldOperations", "Service", "ByStreetId", DependsOn = "Street", PromptText = "Please select a street above")]
        public int? Service { get; set; }
        
        [Required, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        [DropDown]
        public int? CompanyServiceMaterial { get; set; }
        [EntityMap, EntityMustExist(typeof(ServiceSize))]
        [DropDown]
        public int? CompanyServiceSize { get; set; }
        [Required]
        public DateTime? DateInstalled { get; set; }

        //TODO: Automatically populated if linked service is installed, maybe just make it a formula field?
        public DateTime? RenewalCompleted { get; set; }

        #endregion

        #region Constructors

		public EditServiceLineProtectionInvestigation(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void Map(ServiceLineProtectionInvestigation entity)
        {
            base.Map(entity);
            State = entity.OperatingCenter?.State?.Id;
        }

        #endregion
    }

    public class SearchServiceLineProtectionInvestigation : SearchSet<ServiceLineProtectionInvestigation>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter")]
        public int? CustomerCity { get; set; }

        public string CustomerName { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ServiceLineProtectionInvestigationStatus))]
        public int? Status { get; set; }

        [DropDown]
        public int? Contractor { get; set; }

        #endregion
	}
}