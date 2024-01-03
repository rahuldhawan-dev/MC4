using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.ExternalStreetOpeningPermits
{
    public class NewExternalStreetOpeningPermitForm
    {
        /// <summary>
        /// This is the work order id, but named as such so that it's highly unlikely
        /// to ever conflict with the html form we receive from the Permits API.
        /// </summary>
        [Required, EntityMustExist(typeof(WorkOrder))]
        public int? NoApiConflictWorkOrderId { get; set; }

        /// <summary>
        /// Needed so the view can setup some javascript with the existing workorder values.
        /// </summary>
        public WorkOrder WorkOrder { get; set; }

        // FormId comes from two places.
        // 1. The NewFromPermitForm action has it as part of the querystring
        // 2. It's included directly in the rendered html from the permits API, so we 
        //    can make use of that in binding.
        [Required]
        public int? FormId { get; set; }

        /// <summary>
        /// The pre-rendered html from the Permits API.
        /// </summary>
        public string PermitFormHtml { get; set; }
    }
}