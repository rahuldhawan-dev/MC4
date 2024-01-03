using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Models.ViewModels.OperatorLicenses
{
    public class SearchOperatorLicense : SearchSet<OperatorLicense>
    {
        #region Properties

        [DropDown("", "OperatingCenter", "ByStateIdForHumanResourcesEmployeeLimited", DependsOn = "State"), EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("Employee", "e", "OperatingCenter.Id")]
        public int? OperatingCenter { get; set; }

        [View(OperatorLicense.DisplayNames.EMPLOYEE_STATUS), DropDown, EntityMap, EntityMustExist(typeof(EmployeeStatus)), SearchAlias("Employee", "e", "Status.Id", Required = true)]
        public int? Status { get; set; }

        [DropDown("Employee", "EmployeesByStateIdOrOperatingCenterIdAndStatusIdWithStatus", DependsOn = "State,OperatingCenter,Status", DependentsRequired = DependentRequirement.One, PromptText = "Please select a State above. "), EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(OperatorLicenseType))]
        public int? OperatorLicenseType { get; set; }

        [View("Licensed Operator Of Record")]
        [CheckBox]
        public bool? LicensedOperatorOfRecord { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("e.OperatingCenter", "oc", "State.Id")]
        public int? State { get; set; }

        [View(OperatorLicense.DisplayNames.LICENSE_LEVEL)]
        public string LicenseLevel { get; set; }

        [View(OperatorLicense.DisplayNames.LICENSE_SUB_LEVEL)]
        public string LicenseSubLevel { get; set; }

        public string LicenseNumber { get; set; }
        public DateRange ValidationDate { get; set; }
        public DateRange ExpirationDate { get; set; }

        [Search(CanMap = false)]
        public bool? Expired { get; set; }

        #endregion
    }
}
