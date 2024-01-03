using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Metadata;

namespace Contractors.Controllers
{
    public class RestorationMethodController : Data.DesignPatterns.Mvc.ControllerBase<IRestorationMethodRepository, RestorationMethod>
    {   
        public RestorationMethodController(ControllerBaseWithAuthenticationArguments<IRestorationMethodRepository, RestorationMethod, ContractorUser> args) : base(args) {}

        [HttpGet, NoCache]
        public ActionResult ByRestorationTypeID(int restorationTypeID)
        {
            return new CascadingActionResult(Repository.GetByRestorationTypeID(restorationTypeID), "Description", "Id");
        }
    }
}
