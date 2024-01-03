using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Utility.Notifications;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using System.ComponentModel;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    [DisplayName(PAGE_TITLE)]
    public class WasteWaterSystemBasinController : ControllerBaseWithPersistence<WasteWaterSystemBasin, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral,
                                 EMAIL_NOTIFICATION_ROLE = RoleModules.EnvironmentalWasteWaterSystems;

        public const string BASIN_CREATED_EMAIL_SUBJECT = "New Basin Created for {0} on {1}",
                            PAGE_TITLE = "Wastewater System Basins";

        public struct NotificationPurposes
        {
            public const string BASIN_CREATED = "Wastewater System Basin Created";
        }

        #endregion

        #region Constructors

        public WasteWaterSystemBasinController(
            ControllerBaseWithPersistenceArguments<IRepository<WasteWaterSystemBasin>, WasteWaterSystemBasin, User>
                args) : base(args) { }

        #endregion

        #region Private Methods

        private void SendWasteWaterSystemBasinCreatedNotification(WasteWaterSystemBasin model)
        {
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            var notifier = _container.GetInstance<INotificationService>();

            model.RecordUrl = GetUrlForModel(model, "Show", "WasteWaterSystemBasin", "Environmental");
            model.WasteWaterSystem.RecordUrl = GetUrlForModel(model.WasteWaterSystem, "Show", "WasteWaterSystem", "Environmental");

            var args = new NotifierArgs {
                OperatingCenterId = model.WasteWaterSystem.OperatingCenter.Id,
                Module = EMAIL_NOTIFICATION_ROLE,
                Purpose = NotificationPurposes.BASIN_CREATED,
                Data = model,
                Subject = string.Format(BASIN_CREATED_EMAIL_SUBJECT, model.WasteWaterSystem.WasteWaterSystemName, now)
            };
            notifier.Notify(args);
        }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                case ControllerAction.Show:
                    this.AddDynamicDropDownData<WasteWaterSystem, WasteWaterSystemDisplayItem>(dataGetter: d => d.GetAllSorted(x => x.WasteWaterSystemName));
                    break;
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchWasteWaterSystemBasin search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWasteWaterSystemBasin search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresAdmin]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateWasteWaterSystemBasin>());
        }

        [HttpPost]
        [RequiresAdmin]
        public ActionResult Create(CreateWasteWaterSystemBasin model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs
            {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    SendWasteWaterSystemBasinCreatedNotification(entity);
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresAdmin]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditWasteWaterSystemBasin>(id);
        }

        [HttpPost]
        [RequiresAdmin]
        public ActionResult Update(EditWasteWaterSystemBasin model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete

        [HttpDelete]
        [RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Cascading Endpoints

        [HttpGet]
        public ActionResult ByWasteWaterSystem(int wasteWaterSystemId)
        {
            return new CascadingActionResult<WasteWaterSystemBasin, WasteWaterSystemBasinDisplayItem>(Repository.Where(x => x.WasteWaterSystem.Id == wasteWaterSystemId));
        }

        #endregion

        #endregion
    }
}