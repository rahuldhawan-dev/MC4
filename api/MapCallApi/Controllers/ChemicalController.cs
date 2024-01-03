using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC.Controllers;

namespace MapCallApi.Controllers
{
    public class ChemicalController : ControllerBaseWithPersistence<IChemicalRepository, Chemical, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Constructors

        public ChemicalController(ControllerBaseWithPersistenceArguments<IChemicalRepository, Chemical, User> args) : base(args) { }

        #endregion
        
        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchChemical searchSet)
        {
            searchSet.EnablePaging = false;

            return Json(_repository.SearchForApi(searchSet)
                                   .Select(x => x.ChemicalToJson()), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
