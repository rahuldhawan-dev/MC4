using System.Linq;
using System.Net;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC;
using MMSINC.Controllers;

namespace MapCallApi.Controllers
{
    public class FireDistrictController : ControllerBaseWithPersistence<IFireDistrictRepository, FireDistrict, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesDataLookups;
        public const string TOWN_REQUIRED_ERROR = "Town parameter(int) is required.";

        #endregion

        #region Constructors

        public FireDistrictController(ControllerBaseWithPersistenceArguments<IFireDistrictRepository, FireDistrict, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchFireDistrict searchSet)
        {
            if (!ModelState.IsValid && ModelState.ContainsKey(nameof(searchSet.Town)))
            {
                return new JsonHttpStatusCodeResult(HttpStatusCode.BadRequest, TOWN_REQUIRED_ERROR);
            }

            return Json(_repository.GetByTownId(searchSet.Town.Value).Select(x => new {
                x.Id, x.DistrictName
            }), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
