using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.Controllers;

namespace Contractors.Controllers
{
    public class WorkDescriptionController : Data.DesignPatterns.Mvc.ControllerBase<IWorkDescriptionRepository, WorkDescription>
    { 
        public WorkDescriptionController(ControllerBaseWithAuthenticationArguments<IWorkDescriptionRepository, WorkDescription, ContractorUser> args) : base(args) {}

        // This is saying "Cache this on the client only for about 5 minutes, vary it based on this query string value"
        [HttpGet]
        public ActionResult ByAssetTypeId(int assetTypeId)
        {
            return new CascadingActionResult(Repository.GetByAssetTypeId(assetTypeId), "Description", "Id");
        }
    }
}
