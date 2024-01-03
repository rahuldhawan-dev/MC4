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

namespace MapCallApi.Controllers
{
    public class EmployeeController : ControllerBaseWithPersistence<IEmployeeRepository, Employee, User>
    {

        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesEmployee;
        public const int MAX_JSON_LENGTH = 20971520;

        #endregion

        #region Constructors

        public EmployeeController(ControllerBaseWithPersistenceArguments<IEmployeeRepository, Employee, User> args) : base(args) { }

        #endregion

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEmployee search)
        {
            search.EnablePaging = false;

            var employees = _repository.SearchForApi(search).Select(x => x.ToJson());

            return new JsonResult
            {
                MaxJsonLength = MAX_JSON_LENGTH,
                Data = employees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}