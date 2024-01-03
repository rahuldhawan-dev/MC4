using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class BappTeamIdeaController : ControllerBaseWithPersistence<BappTeamIdea, User>
    {
        #region Constants

        // TODO: change this to the newly created role
        public const RoleModules ROLE = RoleModules.BAPPTeamSharingGeneral;
        public const string NOTIFICATION_PURPOSE = "BAPP Team Idea";

        #endregion

        #region Private Methods

        private void SendCreationsMostBodaciousNotification(BappTeamIdea model)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs
            {
                OperatingCenterId = model.BappTeam.OperatingCenter.Id,
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = model
            };

            notifier.Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddDropDownData<OperatingCenter, IOperatingCenterRepository>(r => r.GetAllWithBappTeams());
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("Contact");
                    this.AddDropDownData<SafetyImplementationCategory>();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchBappTeamIdea search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchBappTeamIdea search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateBappTeamIdea(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateBappTeamIdea model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs
            {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    SendCreationsMostBodaciousNotification(entity);
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditBappTeamIdea>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditBappTeamIdea model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public BappTeamIdeaController(ControllerBaseWithPersistenceArguments<IRepository<BappTeamIdea>, BappTeamIdea, User> args) : base(args) {}
    }
}
