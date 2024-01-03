using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Models.ViewModels.OperatorLicenses
{
    public class OperatorLicenseViewModel : ViewModel<OperatorLicense>
    {
        #region Properties

        [DropDown, Required, EntityMustExist(typeof(OperatorLicenseType)), EntityMap]
        public int? OperatorLicenseType { get; set; }

        [Required, StringLength(OperatorLicense.StringLengths.LICENSE_LEVEL)]
        public string LicenseLevel { get; set; }

        [StringLength(OperatorLicense.StringLengths.LICENSE_SUB_LEVEL)]
        public string LicenseSubLevel { get; set; }

        [Required, StringLength(OperatorLicense.StringLengths.LICENSE_NUMBER)]
        public string LicenseNumber { get; set; }

        [Required]
        public DateTime? ValidationDate { get; set; }

        [Required]
        public DateTime? ExpirationDate { get; set; }

        [View("Licensed Operator Of Record")]
        [CheckBox]
        public bool? LicensedOperatorOfRecord { get; set; }

        #endregion

        #region Constructors

        public OperatorLicenseViewModel(IContainer container) : base(container) { }

        #endregion
    }
}
