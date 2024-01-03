using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.StreetOpeningPermits
{
    public class StreetOpeningPermitViewModel : ViewModel<StreetOpeningPermit>
    {
        #region Properties

        [Required, StringLength(StreetOpeningPermit.StringLengths.STREET_OPENING_PERMIT_NUMBER)]
        public string StreetOpeningPermitNumber { get; set; }

        [Required]
        public DateTime? DateRequested { get; set; }

        [CompareTo(
            nameof(DateRequested),
            ComparisonType.GreaterThanOrEqualTo,
            TypeCode.DateTime,
            ErrorMessage = "Must be after Date Requested",
            IgnoreNullValues = true)]
        public DateTime? DateIssued { get; set; }

        [CompareTo(
            nameof(DateIssued),
            ComparisonType.GreaterThanOrEqualTo,
            TypeCode.DateTime,
            ErrorMessage = "Must be after Date Issued",
            IgnoreNullValues = true)]
        public DateTime? ExpirationDate { get; set; }

        [Multiline]
        public string Notes { get; set; }

        #endregion

        #region Constructors

        public StreetOpeningPermitViewModel(IContainer container) : base(container) { }

        #endregion
    }
}
