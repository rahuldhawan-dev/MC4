using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class MedicalCertificateViewModel : ViewModel<MedicalCertificate>
    {
        #region Properties

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? CertificationDate { get; set; }

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public DateTime? ExpirationDate { get; set; }

        [Multiline]
        public string Comments { get; set; }

        #endregion

        #region Constructors

        public MedicalCertificateViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateMedicalCertificate : MedicalCertificateViewModel
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

        public CreateMedicalCertificate(IContainer container) : base(container) {}

        #endregion
    }

    public class EditMedicalCertificate : MedicalCertificateViewModel
    {
        #region Constructors

        public EditMedicalCertificate(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchMedicalCertificate : SearchSet<MedicalCertificate>
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Please select an Operating Center above."),
         EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }

        public DateRange CertificationDate { get; set; }
        public DateRange ExpirationDate { get; set; }
        public bool? Expired { get; set; }
        [DropDown, SearchAlias("Employee", "e", "Status.Id")]
        public int? EmployeeStatus { get; set; }

        #endregion
    }
}