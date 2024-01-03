using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Contractors.Models.ViewModels
{
    public class ContractorViewModel : ViewModel<Contractor>
    {
        #region Constructors

        public ContractorViewModel(IContainer container) : base(container) { }

        #endregion

        [Required, StringLength(Contractor.StringLengths.NAME)]
        public string Name { get; set; }

        [StringLength(Contractor.StringLengths.VENDOR_ID)]
        public string VendorId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("Town", "ByStateId", DependsOn = nameof(State), PromptText = "Please select a state above.", Area = ""), 
         EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [DropDown("Street", "GetActiveByTownId", DependsOn = nameof(Town), PromptText = "Please select a town", Area = ""),
         EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }

        [StringLength(Contractor.StringLengths.HOUSE_NUMBER)]
        public string HouseNumber { get; set; }

        [StringLength(Contractor.StringLengths.APARTMENT_NUMBER)]
        public string ApartmentNumber { get; set; }

        [StringLength(Contractor.StringLengths.ZIP)]
        public string Zip { get; set; }

        [StringLength(Contractor.StringLengths.PHONE)]
        public string Phone { get; set; }

        [CheckBox]
        public bool? IsUnionShop { get; set; }

        [CheckBox]
        public bool? IsActive { get; set; }

        [CheckBox]
        public bool? IsBcpPartner { get; set; }

        [CheckBox, View(Description = "This controls all access for this contractor to the web site.")]
        public bool? ContractorsAccess { get; set; }

        [CheckBox]
        public bool? AWR { get; set; }

        [EntityMap, CheckBoxList, EntityMustExist(typeof(OperatingCenter)), 
         View(Description = "The operating centers this contractor belongs to.", DisplayName = Contractor.Display.OPERATING_CENTERS)]
        public int[] OperatingCenters { get; set; }

        [EntityMap, CheckBoxList, EntityMustExist(typeof(OperatingCenter)), 
         View(Description = "The framework operating Centers this contractor belongs to.", DisplayName = Contractor.Display.OPERATING_CENTERS)]
        public int[] FrameworkOperatingCenters { get; set; }

        [EntityMap, CheckBoxList, EntityMustExist(typeof(ContractorFunctionalAreaType)), View(Contractor.Display.FUNCTIONAL_AREAS)]
        public int[] FunctionalAreas { get; set; }

        [EntityMap, CheckBoxList, EntityMustExist(typeof(ContractorWorkCategoryType)), View(Contractor.Display.WORK_CATEGORIES)]
        public int[] WorkCategories { get; set; }
    }

    public class CreateContractor : ContractorViewModel
    {
        #region Constructors

        public CreateContractor(IContainer container) : base(container) { }

        #endregion
        
        #region Mapping

        public override Contractor MapToEntity(Contractor entity)
        {
            base.MapToEntity(entity);

            entity.CreatedBy =
                _container.GetInstance<IAuthenticationService<User>>().CurrentUser.UserName;

            return entity;
        }

        #endregion
    }

    public class EditContractor : ContractorViewModel
    {
        public EditContractor(IContainer container) : base(container) { }
    }
}