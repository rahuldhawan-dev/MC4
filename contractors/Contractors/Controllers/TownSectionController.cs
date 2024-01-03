using System.Linq;
using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.Controllers;

namespace Contractors.Controllers
{
    public class TownSectionController : Data.DesignPatterns.Mvc.ControllerBase<ITownSectionRepository, TownSection>
    {
        public TownSectionController(ControllerBaseWithAuthenticationArguments<ITownSectionRepository, TownSection, ContractorUser> args) : base(args) {}

        // GET: /TownSection/
        [HttpGet]
        public ActionResult ByTownId(int townId)
        {
            return
                new CascadingActionResult(
                    Repository.GetByTownId(townId).OrderBy(x => x.Name), "Name",
                    "Id");
        }
    }
}
