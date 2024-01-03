using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchBusinessUnit : SearchSet<BusinessUnit>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(Department))]
        public int? Department { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }
        
        // This can't filter by OperatingCenter. The EmployeeResponsible may have a 
        // different OperatingCenter than the BusinessUnit.
        [DropDown, EntityMap, EntityMustExist(typeof(Employee))]
        public int? EmployeeResponsible { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(BusinessUnitArea))]
        public int? Area { get; set; }

        #endregion
    }
}