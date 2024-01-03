using System.Linq;
using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.Controllers;

namespace Contractors.Controllers
{
    public class StreetController : Data.DesignPatterns.Mvc.ControllerBase<IStreetRepository, Street>
    {
        public StreetController(ControllerBaseWithAuthenticationArguments<IStreetRepository, Street, ContractorUser> args) : base(args) {}

        [HttpGet]
        public ActionResult ByTownId(int townId)
        {
            return
                new CascadingActionResult(
                    Repository.GetByTownId(townId).OrderBy(x => x.FullStName),
                    "FullStName", "Id");
        }
    }
}
