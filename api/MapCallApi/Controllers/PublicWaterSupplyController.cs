using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC.Controllers;
using Newtonsoft.Json;

namespace MapCallApi.Controllers
{
    public class PublicWaterSupplyController : ControllerBaseWithPersistence<IPublicWaterSupplyRepository, PublicWaterSupply, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Constructors

        public PublicWaterSupplyController(ControllerBaseWithPersistenceArguments<IPublicWaterSupplyRepository, PublicWaterSupply, User> args) : base(args) { }

        #endregion

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchPublicWaterSupply search)
        {
            search.EnablePaging = false;
            var publicWaterSupplies = _repository.Search(search)
                                                 .Select(x => x.PublicWaterSupplyToJson());

            return Json(publicWaterSupplies, JsonRequestBehavior.AllowGet);
        }
    }
}