using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Contractors.Models.ViewModels
{
    public class SearchContractor : SearchSet<Contractor>
    {
        public SearchString Name { get; set; }

        [EntityMap, EntityMustExist(typeof(OperatingCenter)), DropDown, SearchAlias("OperatingCenters", "Id")]
        public int? OperatingCenter { get; set; }

        public int? Town { get; set; }

        public int? Street { get; set; }

        public SearchString HouseNumber { get; set; }

        public SearchString ApartmentNumber { get; set; }

        public bool? IsUnionShop { get; set; }

        [View(Contractor.Display.IS_BCP_PARTNER)]
        public bool? IsBcpPartner { get; set; }

        public bool? IsActive { get; set; }

        public bool? AWR { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorWorkCategoryType)), SearchAlias("WorkCategories", "Id")]
        public int? WorkCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorFunctionalAreaType)), SearchAlias("FunctionalAreas", "Id")]
        public int? FunctionalArea { get; set; }
    }
}