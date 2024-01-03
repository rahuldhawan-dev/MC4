using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services
{
    public class SearchServicePremiseNumberServiceNumber
    {
        #region Properties

        // These properties are named weirdly because they would otherwise
        // conflict with the properties/fields on BaseValveImageViewModel
        // when they're both on the same view. OperatingCenter messes up
        // the Town cascades and ValveNumber messes up autofilling in 
        // the ValveNumber textbox.
     
        public string ServiceNumberSearch { get; set; }

        [Required]
        public string PremiseNumberSearch { get; set; }

        #endregion
    }
}
