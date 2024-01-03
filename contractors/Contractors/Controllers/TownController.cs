using System.Linq;
using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.Controllers;

namespace Contractors.Controllers
{
    public class TownController : Data.DesignPatterns.Mvc.ControllerBase<ITownRepository, Town>
    {
        #region Constructor

        public TownController(ControllerBaseWithAuthenticationArguments<ITownRepository, Town, ContractorUser> args) : base(args) {}

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult ByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult(
                Repository.GetByOperatingCenterId(operatingCenterId).OrderBy(
                    x => x.ShortName), "ShortName", "Id");
        }

        [HttpGet]
        public ActionResult ByCountyId(int stateId)
        {
            return new CascadingActionResult(Repository.GetByCountyId(stateId), "ShortName", "Id");
        }

        #endregion
    }
}