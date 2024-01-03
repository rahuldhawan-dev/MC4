using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Admin.Models.ViewModels.RoleGroups;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Admin.Controllers
{
    public class RoleGroupController : ControllerBaseWithPersistence<RoleGroup, User>
    {
        #region Consts

        public const string ROLE_GROUP_VIEWDATA_KEY = "RoleGroupViewData";

        #endregion

        #region Constructor

        public RoleGroupController(ControllerBaseWithPersistenceArguments<IRepository<RoleGroup>, RoleGroup, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                case ControllerAction.Show:
                    // This is all json serialized for the frontend, hence the lowercase names.
                    var roleTableData = new Dictionary<string, object>();
                    roleTableData["applications"] = _container.GetInstance<IRepository<Application>>().Linq
                                                              .Select(x => new { id = x.Id, name = x.Name }).ToList();
                    roleTableData["modules"] = _container.GetInstance<IRepository<Module>>().Linq
                                                         .Select(x => new { id = x.Id, name = x.Name, applicationId = x.Application.Id }).ToList();
                    roleTableData["actions"] = _container.GetInstance<IRepository<RoleAction>>().Linq
                                                         .Select(x => new { id = x.Id, name = x.Name }).ToList();
                    roleTableData["operatingCenters"] = _container.GetInstance<IRepository<OperatingCenter>>().Linq
                                                                  .Select(x => new { id = x.Id, name = x.OperatingCenterCode, stateId = x.State.Id });
                    roleTableData["states"] = _container.GetInstance<IRepository<State>>().Linq
                                                        .Select(x => new { id = x.Id, name = x.Abbreviation }).ToList();

                    ViewData[ROLE_GROUP_VIEWDATA_KEY] = roleTableData;
                    break;
            }

            base.SetLookupData(action);
        }

        #endregion

        #region Show/Index/Search

        [HttpGet, RequiresUserAdmin]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchRoleGroup>();
        }

        [HttpGet, RequiresUserAdmin]
        public ActionResult Index(SearchRoleGroup search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet, RequiresUserAdmin]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresAdmin]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<RoleGroupViewModel>());
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Create(RoleGroupViewModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresAdmin]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<RoleGroupViewModel>(id);
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Update(RoleGroupViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Destroy

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}