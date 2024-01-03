using System.Web.Mvc;
using MMSINC.Controllers;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Controllers
{
    public class FileController : MMSINC.Controllers.ControllerBase
    {
        [RequiresSecureForm(false)]
        public ActionResult Create(UploadModel model)
        {
            // TODO: See if text/html is still needed for IE's shortcomings.
            const string RETURN_CONTENT_TYPE = "text/html";
            if (ModelState.IsValid)
            {
                var fileServ = _container.With(TempData).GetInstance<FileUploadService>();
                fileServ.StoreFileAndSetKey(model.FileUpload);
                return Json(new {
                    success = true,
                    key = model.FileUpload.Key,
                    fileName = model.FileUpload.FileName
                }, RETURN_CONTENT_TYPE);
            }

            return Json(new {success = false}, RETURN_CONTENT_TYPE);
        }

        // Wrapper model needed to work with the AjaxFileUploadModelBinder. 
        public class UploadModel
        {
            public AjaxFileUpload FileUpload { get; set; }
        }

        public FileController(ControllerBaseArguments args) : base(args) { }
    }
}
