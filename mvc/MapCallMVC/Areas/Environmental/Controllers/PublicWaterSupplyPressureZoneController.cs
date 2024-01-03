using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyPressureZones;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class PublicWaterSupplyPressureZoneController : ControllerBaseWithPersistence<PublicWaterSupplyPressureZone, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral,
                                 EMAIL_NOTIFICATION_ROLE = RoleModules.EnvironmentalWaterSystems;

        public const string PRESSURE_ZONE_CREATED_EMAIL_SUBJECT = "New Pressure Zone created for {0} on {1}";

        public struct NotificationPurposes
        {
            public const string PRESSURE_ZONE_CREATED = "Public Water Supply Pressure Zone Created";
        }

        #endregion

        #region Constructors

        public PublicWaterSupplyPressureZoneController(ControllerBaseWithPersistenceArguments<IRepository<PublicWaterSupplyPressureZone>, PublicWaterSupplyPressureZone, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private void SendPressureZoneCreatedNotification(PublicWaterSupplyPressureZone model)
        {
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            var notifier = _container.GetInstance<INotificationService>();

            model.RecordUrl = GetUrlForModel(model, "Show", "PublicWaterSupplyPressureZone", "Environmental");
            model.PublicWaterSupply.RecordUrl = GetUrlForModel(model.PublicWaterSupply, "Show", "PublicWaterSupply");

            foreach (var opCenterPws in model.PublicWaterSupply.OperatingCenterPublicWaterSupplies)
            {
                var args = new NotifierArgs {
                    OperatingCenterId = opCenterPws.OperatingCenter.Id,
                    Module = EMAIL_NOTIFICATION_ROLE,
                    Purpose = NotificationPurposes.PRESSURE_ZONE_CREATED,
                    Data = model,
                    Subject = string.Format(PRESSURE_ZONE_CREATED_EMAIL_SUBJECT, model.PublicWaterSupply.Identifier, now)
                };
                notifier.Notify(args);
            }
        }

        #endregion

        #region Public Methods

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchPublicWaterSupplyPressureZoneViewModel viewModel)
        {
            return ActionHelper.DoSearch(viewModel);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchPublicWaterSupplyPressureZoneViewModel viewModel)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(viewModel));
                formatter.Excel(() => ActionHelper.DoExcel(viewModel));
            });
        }
        
        #endregion

        #region New/Create

        [HttpGet, RequiresAdmin]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<PublicWaterSupplyPressureZoneViewModel>());
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Create(PublicWaterSupplyPressureZoneViewModel viewModel)
        {
            return ActionHelper.DoCreate(viewModel, new ActionHelperDoCreateArgs
            {
                OnSuccess = () => {
                    var entity = Repository.Find(viewModel.Id);
                    SendPressureZoneCreatedNotification(entity);
                    return RedirectToAction("Show", new { id = viewModel.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresAdmin]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<PublicWaterSupplyPressureZoneViewModel>(id);
        }

        [HttpPost, RequiresAdmin]
        public ActionResult Update(PublicWaterSupplyPressureZoneViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Cascading Endpoints

        [HttpGet]
        public ActionResult ByPublicWaterSupply(int publicWaterSupplyId)
        {
            return new CascadingActionResult<PublicWaterSupplyPressureZone, PublicWaterSupplyPressureZoneDisplayItem>(Repository.Where(x => x.PublicWaterSupply.Id == publicWaterSupplyId));
        }

        #endregion

        #endregion
    }
}
