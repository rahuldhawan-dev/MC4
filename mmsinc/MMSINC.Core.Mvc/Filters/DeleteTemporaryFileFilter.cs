using System.Web.Mvc;
using MMSINC.Results;

namespace MMSINC.Filters
{
    /// <summary>
    /// This filter deletes temporary files created by ExcelResult/PdfResult. These files can't
    /// be deleted properly from the result objects themselves and has to be done after they've
    /// been executed.
    /// </summary>
    public class DeleteTemporaryFileFilter : ActionFilterAttribute
    {
        // NOTE: Everything on google suggests doing this as opposed to in the result itself.
        //       Supposedly, OnResultExecuted is called after the response has been completely
        //       written to, meaning we should be able to delete the file without hitting a 
        //       file access bug. If this doesn't work, clearing the temporary files as a scheduled
        //       task will need to be done to prevent temp file garbage from filling up the drive.
        // DOUBLE NOTE: It's seemingly impossible to duplicate this error locally.
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var tempFileResult = filterContext.Result as ITemporaryFileResult;
            if (tempFileResult != null)
            {
                tempFileResult.DeleteTemporaryFiles();
            }
        }
    }
}
