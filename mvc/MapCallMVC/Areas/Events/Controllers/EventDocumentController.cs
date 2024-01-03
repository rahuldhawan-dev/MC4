using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Events.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;


namespace MapCallMVC.Areas.Events.Controllers
{
    public class EventDocumentController : ControllerBaseWithPersistence<EventDocument, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EventsEvents;

        #endregion

        #region Constructors

        public EventDocumentController(ControllerBaseWithPersistenceArguments<IRepository<EventDocument>, EventDocument, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        { 
            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add);
                    break;

                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    break;

                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    break;
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchEventDocument search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchEventDocument search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create
        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateEventDocument>());
        }
        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateEventDocument model)
        {
            return ActionHelper.DoCreate(model);
        }
        #endregion

        #region Edit/Update
        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEventDocument>(id);
        }
        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEventDocument model)
        {
            return ActionHelper.DoUpdate(model);
        }
        #endregion

        #region Delete/Destroy
        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #endregion
    }
}