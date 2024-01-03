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
    public class ChemicalStorageController : ControllerBaseWithPersistence<IRepository<ChemicalStorage>, ChemicalStorage, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Constructors

        public ChemicalStorageController(ControllerBaseWithPersistenceArguments<IRepository<ChemicalStorage>, ChemicalStorage, User> args) : base(args) { }

        #endregion

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchChemicalStorage search)
        {
            var chemicalStorage = Repository.Search(search).Select(x => x.GetChemicalStorageJson());

            return Json(chemicalStorage, JsonRequestBehavior.AllowGet);
        }
    }
}