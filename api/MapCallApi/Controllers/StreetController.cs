using System.Collections.Generic;
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
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using StructureMap.Query;

namespace MapCallApi.Controllers
{
    public class StreetController : ControllerBaseWithPersistence<IStreetRepository, Street, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesDataLookups;
        public const string TOWN_REQUIRED_ERROR = "Town parameter(int) is required.";

        #endregion
        
        #region Constructors

        public StreetController(ControllerBaseWithPersistenceArguments<IStreetRepository, Street, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchStreet searchSet)
        {
            if (searchSet.PageSize == 0)
            {
                searchSet.EnablePaging = false;
            }

            if (!ModelState.IsValid && ModelState.ContainsKey(nameof(searchSet.Town)))
            {
                return new JsonHttpStatusCodeResult(HttpStatusCode.BadRequest, TOWN_REQUIRED_ERROR);
            }
            
            return Json(_repository.SearchForApi(searchSet).Select(x => new { 
                x.Id, 
                x.FullStName
            }), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
