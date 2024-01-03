using MMSINC.Data;
using MapCall.Common.Model.Entities;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;

namespace MapCallMVC.Areas.HumanResources.Models.ViewModels
{
    public class SearchEmployeeHeadCount : SearchSet<EmployeeHeadCount>
    {
        [SearchAlias("BusinessUnit.OperatingCenter", "State.Id", Required = true)]
        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [SearchAlias("BusinessUnit", "OperatingCenter.Id", Required = true)]
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "BusinessUnit", "FindByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        [EntityMustExist(typeof(BusinessUnit)), EntityMap]
        public int? BusinessUnit { get; set; }
        
        [SearchAlias("BusinessUnit", "Department.Id")]
        [DropDown, EntityMap, EntityMustExist(typeof(Department))]
        public int? Department { get; set; }

        // This property can't be named "Area" because its value gets messed
        // up by the MVC route rather than the user selected value. 
        [SearchAlias("BusinessUnit", "Area.Id")]
        [DropDown, EntityMap, EntityMustExist(typeof(BusinessUnitArea))]
        public int? AreaName { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EmployeeHeadCountCategory))]
        public int? Category { get; set; }

        public int? Year { get; set; }
    }
}