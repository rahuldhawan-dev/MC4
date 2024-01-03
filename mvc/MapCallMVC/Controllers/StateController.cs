using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class StateController : ControllerBaseWithPersistence<IRepository<State>, State, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesDataLookups;
        public const string NOT_FOUND = "Not found";

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index()
        {
            return ActionHelper.DoIndexWithResults(Repository.GetAll().ToList());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresAdmin]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditState>(id);
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Update(EditState model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int opCenterId)
        {
            var states = new List<State>();
            var opc = _container.GetInstance<IOperatingCenterRepository>().Find(opCenterId);
            if (opc != null && opc.State != null)
            {
                states.Add(opc.State);
            }

            return new CascadingActionResult(states, "Description", "Id")
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        public StateController(ControllerBaseWithPersistenceArguments<IRepository<State>, State, User> args) : base(args) {}
    }
}
