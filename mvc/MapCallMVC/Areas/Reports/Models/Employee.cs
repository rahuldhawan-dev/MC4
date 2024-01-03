using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchCommercialDriversLicenseCompliance : SearchSet<Employee>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }
        [DropDown]
        public int? Status { get; set; }
        [DropDown, View("Program Status")]
        public int? CommercialDriversLicenseProgramStatus { get; set; }
        public bool? IsCDLCompliant { get; set; }

        #endregion
    }

    public class SearchCommercialDriversLicenseDue : SearchSet<Employee>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }
        [DropDown]
        public int? Status { get; set; }
        [DropDown, DisplayName("Program Status")]
        public int? CommercialDriversLicenseProgramStatus { get; set; }
        [DropDown]
        public int? PositionGroup { get; set; }
        [View("Employee ID")]
        public string EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        
        #endregion
    }

    public class SearchCommercialDriversLicenseMedicalCertificatesDue : SearchCommercialDriversLicenseDue
    {
        public void Initialize(IDateTimeProvider dateTimeProvider)
        {
            MedicalCertificateExpirationDate = new DateRange {
                End = dateTimeProvider.GetCurrentDate(),
                Operator = RangeOperator.LessThan
            };
            Status = EmployeeStatus.Indices.ACTIVE;
            CommercialDriversLicenseProgramStatus =
                MapCall.Common.Model.Entities.CommercialDriversLicenseProgramStatus.Indices.IN_PROGRAM;
        }

        public DateRange MedicalCertificateExpirationDate { get; set; }
    }

    public class SearchCommercialDriversLicenseViolationCertificatesDue : SearchCommercialDriversLicenseDue
    {
        public void Initialize(IDateTimeProvider dateTimeProvider)
        {
            ViolationCertificateExpirationDate = new DateRange {
                End = dateTimeProvider.GetCurrentDate(),
                Operator = RangeOperator.LessThan
            };
            Status = EmployeeStatus.Indices.ACTIVE;
            CommercialDriversLicenseProgramStatus =
                MapCall.Common.Model.Entities.CommercialDriversLicenseProgramStatus.Indices.IN_PROGRAM;
        }

        public DateRange ViolationCertificateExpirationDate { get; set; }
    }

    public class SearchCommercialDriversLicensesDue : SearchCommercialDriversLicenseDue
    {
        public void Initialize(IDateTimeProvider dateTimeProvider)
        {
            DriversLicenseRenewalDate = new DateRange {
                End = dateTimeProvider.GetCurrentDate(),
                Operator = RangeOperator.LessThan
            };
            Status = EmployeeStatus.Indices.ACTIVE;
        }
        public DateRange DriversLicenseRenewalDate { get; set; }
    }

    public class SearchCommercialDriversLicenseAbstract : SearchSet<Employee>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }
        [DropDown]
        public int? Status { get; set; }
        [DropDown, DisplayName("Program Status")]
        public int? CommercialDriversLicenseProgramStatus { get; set; }
        [View("Employee ID")]
        public string EmployeeId { get; set; }
        public string LastName { get; set; }

        public bool? IsCDLCompliant { get; set; }
        public DateRange DriversLicenseIssuedDate { get; set; }
        
        #endregion
    }
}