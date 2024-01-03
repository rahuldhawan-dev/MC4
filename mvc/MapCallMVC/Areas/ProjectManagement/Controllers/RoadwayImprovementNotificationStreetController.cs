using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    [DisplayName("Roadway Improvement Notification Street Report")]
    public class RoadwayImprovementNotificationStreetController : ControllerBaseWithPersistence<IRepository<RoadwayImprovementNotificationStreet>, RoadwayImprovementNotificationStreet, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesProjects;

        #endregion

        #region Constructor

        public RoadwayImprovementNotificationStreetController(ControllerBaseWithPersistenceArguments<IRepository<RoadwayImprovementNotificationStreet>, RoadwayImprovementNotificationStreet, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        [NonAction]
        public override void SetLookupData(ControllerAction action)
        {
            this.AddDropDownData<MainSize>();
            this.AddDropDownData<MainType>();
            this.AddDropDownData<RoadwayImprovementNotificationStreetStatus>();

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData();
            }
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditRoadwayImprovementNotificationStreet>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditRoadwayImprovementNotificationStreet model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchRoadwayImprovementNotificationStreet search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }
        
        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchRoadwayImprovementNotificationStreet search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion
    }
}