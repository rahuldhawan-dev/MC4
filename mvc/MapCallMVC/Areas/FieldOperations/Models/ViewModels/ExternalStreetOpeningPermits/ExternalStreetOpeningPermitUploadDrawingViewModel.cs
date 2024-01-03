using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.ExternalStreetOpeningPermits
{
    public class ExternalStreetOpeningPermitUploadDrawingViewModel
    {
        #region Properties

        [Required, EntityMustExist(typeof(StreetOpeningPermit))]
        public int? StreetOpeningPermit { get; set; }

        [FileUpload(OnComplete = "PermitDrawingUpload.onComplete")]
        public AjaxFileUpload FileUpload { get; set; }

        #endregion
    }
}