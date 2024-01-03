using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallApi.Models;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using Newtonsoft.Json;

namespace MapCallApi.Controllers
{
    public class WasteWaterSystemController : ControllerBaseWithPersistence<IRepository<WasteWaterSystem>, WasteWaterSystem, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalWasteWaterSystems;

        #endregion

        #region Constructors

        public WasteWaterSystemController(ControllerBaseWithPersistenceArguments<IRepository<WasteWaterSystem>, WasteWaterSystem, User> args) : base(args) { }

        #endregion

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWasteWaterSystem search)
        {
            search.EnablePaging = false;

            var wasteWaterSystems = _repository.Search(search).Select(x => x.ToJson());

            return Json(wasteWaterSystems, JsonRequestBehavior.AllowGet);
        }
    }
}