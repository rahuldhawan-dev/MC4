using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class ViolationCertificateViewModel : ViewModel<ViolationCertificate>
    {
        #region Properties

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? CertificateDate { get; set; }
        [Multiline]
        public string Comments { get; set; }

        #endregion

        #region Constructors

        public ViolationCertificateViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateViolationCertificate : ViolationCertificateViewModel
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter)), Required]
        public int? OperatingCenter { get; set; }

        [DropDown("Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Please select an Operating Center above."), EntityMustExist(typeof(Employee)),
         Required, EntityMap]
        public int? Employee { get; set; }

        #endregion

        #region Constructors

        public CreateViolationCertificate(IContainer container) : base(container) {}

        #endregion
    }

    public class EditViolationCertificate : ViolationCertificateViewModel
    {
        #region Constructors

        public EditViolationCertificate(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchViolationCertificate : SearchSet<ViolationCertificate>
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Please select an Operating Center above."),
         EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }

        public DateRange CertificateDate { get; set; }
        public bool? Expired { get; set; }
        [DropDown, SearchAlias("Employee", "e", "Status.Id")]
        public int? EmployeeStatus { get; set; }

        #endregion
    }
}
