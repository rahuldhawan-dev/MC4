using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Models.ViewModels.OperatorLicenses
{
    public class SearchOperatorLicenseReport : SearchSet<OperatorLicense>, ISearchOperatorLicenseReport
    {
        #region Properties

        [DropDown("", "OperatingCenter", "ByStateIdForHumanResourcesEmployeeLimited", DependsOn = "State"), EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above. "), EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatorLicenseType))]
        public int? OperatorLicenseType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
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

        public bool? LicensedOperatorOfRecord { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EmployeeStatus)), SearchAlias("Employee", "e", "Status.Id")]
        public int? EmployeeStatus { get; set; }

        #endregion
    }
}
