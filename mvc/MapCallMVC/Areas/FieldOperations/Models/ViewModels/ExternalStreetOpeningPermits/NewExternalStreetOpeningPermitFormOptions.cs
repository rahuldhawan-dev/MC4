using MapCall.Common.Model.Entities;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.ExternalStreetOpeningPermits
{
    public class NewExternalStreetOpeningPermitFormOptions
    {
        #region Properties

        // NOTE: This entire view model is *only* for display purposes.
        // The view model will never be used for postbacks.

        public WorkOrder WorkOrder { get; set; }

        public int? PermitsApiStateFormId { get; set; }
        public int? PermitsApiCountyFormId { get; set; }
        public int? PermitsApiMunicipalityFormId { get; set; }

        /// <summary>
        /// Set to true if we're completely unable to connect to the api.
        /// </summary>
        public bool UnableToConnectToApi { get; set; }

        #endregion
    }
}