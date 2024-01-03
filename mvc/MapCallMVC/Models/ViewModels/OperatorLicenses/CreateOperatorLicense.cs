using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Models.ViewModels.OperatorLicenses
{
    public class CreateOperatorLicense : OperatorLicenseViewModel
    {
        #region Properties

        [DropDown, Required, EntityMustExist(typeof(State)), EntityMap]
        public int? State { get; set; }

        [EntityMap]
        [DropDown("", "OperatingCenter", "ByStateIdForHumanResourcesEmployeeLimited", DependsOn = "State"), EntityMustExist(typeof(OperatingCenter)), Required]
        public int? OperatingCenter { get; set; }

        [DropDown("Employee", "EmployeesByOperatingCenterIdAndStatusId", DependsOn = "OperatingCenter, Status", PromptText = "Please select an Operating Center and Status above. "), EntityMustExist(typeof(Employee)), Required, EntityMap]
        public int? Employee { get; set; }

        [View(OperatorLicense.DisplayNames.EMPLOYEE_STATUS), DropDown, Required, EntityMap(MapDirections.None), EntityMustExist(typeof(EmployeeStatus))]
        public int? Status { get; set; }

        #endregion

        #region Constructors

        public CreateOperatorLicense(IContainer container) : base(container) { }

        #endregion
    }
}
