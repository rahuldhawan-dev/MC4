using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MMSINC.Data.NHibernate;
using MapCall.Common.Metadata;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class InterconnectionTestController : ControllerBaseWithPersistence<InterconnectionTest, User>
    {
        #region Constants

        public const RoleModules Role = RoleModules.ProductionInterconnections;

        #endregion

        #region Constructors

        public InterconnectionTestController(ControllerBaseWithPersistenceArguments<IRepository<InterconnectionTest>, InterconnectionTest, User> args) : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(Role)]
        public ActionResult Search(SearchInterconnectionTest search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(Role)]
        public ActionResult Index(SearchInterconnectionTest search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    return _container.With((IEnumerable<IThingWithCoordinate>)results).GetInstance<MapResultWithCoordinates>();
                });
            });
        }

        [HttpGet, RequiresRole(Role)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(Role, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateInterconnectionTest(_container));
        }

        [HttpPost, RequiresRole(Role, RoleActions.Add)]
        public ActionResult Create(CreateInterconnectionTest model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditInterconnectionTest>(id);
        }

        [HttpPost, RequiresRole(Role, RoleActions.Edit)]
        public ActionResult Update(EditInterconnectionTest model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(Role, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}