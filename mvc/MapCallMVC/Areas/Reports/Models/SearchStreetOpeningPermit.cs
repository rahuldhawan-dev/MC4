using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchStreetOpeningPermit : IValidatableObject
    {
        #region Properties

        [DropDown, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required]
        public DateTime? CreatedAtStart { get; set; }

        [Required]
        public DateTime? CreatedAtEnd { get; set; }

        public IEnumerable<ReportItem> Results { get; set; }

        #endregion

        #region Exposed Methods

        public NameValueCollection ToSearchParams()
        {
            var ret = new NameValueCollection {
                ["CreatedAtStart"] = CreatedAtStart.ToString(),
                ["CreatedAtEnd"] = CreatedAtEnd.ToString()
            };

            return ret;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((CreatedAtEnd - CreatedAtStart).Value.TotalDays > 31)
            {
                yield return new ValidationResult("Date range can only span a maximum of 31 days.",
                    new[] { "CreatedAtStart", "CreatedAtEnd" });
            }

            //return Enumerable.Empty<ValidationResult>();
        }

        #endregion

        #region Nested Type: ReportItem

        public class ReportItem
        {
            #region Properties

            public string PermitId { get; set; }
            public string WorkOrderId { get; set; }
            public string OperatingCenter { get; set; }
            public string StreetAddress { get; set; }
            public string NearestCrossStreet { get; set; }

            [DisplayName("County/Town")]
            public string CountyTown { get; set; }

            public string CreatedAt { get; set; }
            public string PermitReceivedDate { get; set; }
            public string PaymentReceivedAt { get; set; }
            public string SubmittedAt { get; set; }
            public string TotalCharged { get; set; }
            public string PermitFee { get; set; }
            public string InspectionFee { get; set; }
            public string BondFee { get; set; }
            public string Reconciled { get; set; }
            public string WorkDescription { get; set; }
            public string AccountCharged { get; set; }
            public string AccountingType { get; set; }
            public string CanceledAt { get; set; }
            public string Refunded { get; set; }

            #endregion
        }

        #endregion
    }
}