using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Utility.Notifications;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class RoadwayImprovementNotificationController : ControllerBaseWithPersistence<RoadwayImprovementNotification, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesProjects;

        #endregion

        #region Private Methods

        private void SendNotification(RoadwayImprovementNotification model)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs
            {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE,
                Purpose = "Roadway Improvement Notification Created",
                Data = model
            };
            notifier.Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            if (action == ControllerAction.New)
            {
                this.AddOperatingCenterDropDownData(X => X.IsActive);
            }
            else
            {
                this.AddOperatingCenterDropDownData();
            }

            this.AddDropDownData<RoadwayImprovementNotificationStatus>();
            this.AddDropDownData<RoadwayImprovementNotificationEntity>();
            this.AddDropDownData<RoadwayImprovementNotificationPreconAction>();

            if (action == ControllerAction.Show)
            {
                this.AddDropDownData<MainType>();
                this.AddDropDownData<MainSize>();
                this.AddDropDownData<RoadwayImprovementNotificationStreetStatus>();
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchRoadwayImprovementNotification search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    IsPartial = true, 
                    ViewName = "_ShowPopup"
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchRoadwayImprovementNotification search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateRoadwayImprovementNotification(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateRoadwayImprovementNotification model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendNotification(Repository.Find(model.Id));
                    return null; // defer to default
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditRoadwayImprovementNotification>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditRoadwayImprovementNotification model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #region Children

        #region AddRoadwayImprovementNotificationStreet

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddRoadwayImprovementNotificationStreet(AddRoadwayImprovementNotificationStreet model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region RemoveRoadwayImprovementNotificationStreet

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveRoadwayImprovementNotificationStreet(RemoveRoadwayImprovementNotificationStreet model)
        {
            return ActionHelper.DoUpdate(model);
        }
        
        #endregion

        #endregion

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

		#region Constructors

        public RoadwayImprovementNotificationController(ControllerBaseWithPersistenceArguments<IRepository<RoadwayImprovementNotification>, RoadwayImprovementNotification, User> args) : base(args) {}

		#endregion
    }
}