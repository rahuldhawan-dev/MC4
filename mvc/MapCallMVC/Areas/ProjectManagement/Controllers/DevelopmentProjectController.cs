using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.ProjectManagement.Controllers
{
    public class DevelopmentProjectController : ControllerBaseWithPersistence<DevelopmentProject, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesProjects;
        public const string NOTIFICATION_PURPOSE = "Development Project Created";

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action != ControllerAction.Show)
            {
                if (action == ControllerAction.New)
                {
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, extraFilterP: x => x.IsActive);
                }
                else
                {
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                }
                this.AddDropDownData<DevelopmentProjectCategory>("Category");
                this.AddDynamicDropDownData<PublicWaterSupply, PublicWaterSupplyDisplayItem>();
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchDevelopmentProject search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchDevelopmentProject search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateDevelopmentProject(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateDevelopmentProject model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendCreationNotification(model);
                    return null; // defer to default
                }
            });
        }

        private void SendCreationNotification(CreateDevelopmentProject project)
        {
            var entity = Repository.Find(project.Id);
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = entity.OperatingCenter.Id,
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = entity
            };

            notifier.Notify(args);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditDevelopmentProject>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditDevelopmentProject model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public DevelopmentProjectController(ControllerBaseWithPersistenceArguments<IRepository<DevelopmentProject>, DevelopmentProject, User> args) : base(args) { }
    }
}