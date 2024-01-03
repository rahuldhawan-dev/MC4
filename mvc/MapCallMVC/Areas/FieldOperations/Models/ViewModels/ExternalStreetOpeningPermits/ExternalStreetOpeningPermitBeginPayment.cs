using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.ExternalStreetOpeningPermits
{
    public class ExternalStreetOpeningPermitBeginPayment
    {
        [Required, EntityMustExist(typeof(StreetOpeningPermit))]
        public int? Id { get; set; }

        /// <summary>
        /// The pre-rendered html from the Permits API.
        /// </summary>
        public string PaymentFormHtml { get; set; }
    }
}