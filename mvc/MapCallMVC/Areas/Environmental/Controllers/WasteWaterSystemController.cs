using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Environmental.Models.ViewModels.WasteWaterSystems;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    [DisplayName(PAGE_TITLE)]
    public class
        WasteWaterSystemController : ControllerBaseWithPersistence<IRepository<WasteWaterSystem>, WasteWaterSystem,
            User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalWasteWaterSystems;

        public const string WASTE_WATER_SYSTEM_CREATED = "Wastewater System Created",
                            WASTE_WATER_SYSTEM_UPDATED = "Wastewater System Updated",
                            PAGE_TITLE = "Wastewater System";

        #endregion

        #region Constructors

        public WasteWaterSystemController(
            ControllerBaseWithPersistenceArguments<IRepository<WasteWaterSystem>, WasteWaterSystem, User> args) :
            base(args) { }

        #endregion

        #region Private Methods

        private void SendCreationsMostBodaciousNotification(WasteWaterSystem model, string notificationPurpose)
        {
            model.RecordUrl = GetUrlForModel(model, "Show", "WasteWaterSystem", "Environmental");
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE,
                Purpose = notificationPurpose,
                Data = model
            };

            notifier.Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            this.AddDropDownData<PlanningPlant>("PlanningPlant", x => x.Id, x => x.Display);
            
            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownData(x => x.IsActive);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        [RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchWasteWaterSystem>();
        }

        [HttpGet]
        [RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet]
        [RequiresRole(ROLE)]
        public ActionResult Index(SearchWasteWaterSystem search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    return this.Excel(Repository.Search(search)
                                                .Select(x => new {
                                                     x.Id,
                                                     x.State,
                                                     x.OperatingCenter,
                                                     x.WasteWaterSystemId,
                                                     x.WasteWaterSystemName,
                                                     x.PermitNumber,
                                                     x.Status,
                                                     x.Ownership,
                                                     x.LicensedOperatorStatus,
                                                     x.CurrentLicensedContractor,
                                                     x.Type,
                                                     x.SubType,
                                                     x.DateOfOwnership,
                                                     x.DateOfResponsibility,
                                                     x.IsCombinedSewerSystem,
                                                     x.HasConsentOrder,
                                                     x.ConsentOrderStartDate,
                                                     x.ConsentOrderEndDate,
                                                     x.NewSystemInitialSafetyAssessmentCompleted,
                                                     x.DateSafetyAssessmentActionItemsCompleted,
                                                     x.NewSystemInitialWQEnvAssessmentCompleted,
                                                     x.DateWQEnvAssessmentActionItemsCompleted,
                                                 }));
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new WasteWaterSystemViewModel(_container));
        }

        [HttpPost]
        [RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(WasteWaterSystemViewModel model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendCreationsMostBodaciousNotification(Repository.Find(model.Id), WASTE_WATER_SYSTEM_CREATED);
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<WasteWaterSystemViewModel>(id);
        }

        [HttpPost]
        [RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(WasteWaterSystemViewModel model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    SendCreationsMostBodaciousNotification(entity, WASTE_WATER_SYSTEM_UPDATED);

                    return DoRedirectionToAction("Show", new {id = model.Id});
                }
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        [RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Cascading Endpoints

        [HttpGet]
        public ActionResult ByOperatingCenter(int operatingCenter)
        {
            var data = Repository.Where(x => x.OperatingCenter.Id == operatingCenter);
            return new CascadingActionResult<WasteWaterSystem, WasteWaterSystemDisplayItem>(data.OrderBy(x =>
                x.WasteWaterSystemName)) {SortItemsByTextField = false};
        }

        [HttpGet]
        public ActionResult ActiveByOperatingCenter(params int[] operatingCenterIds)
        {
            var query = Repository.Where(x => x.Status.Id == WasteWaterSystemStatus.Indices.ACTIVE);

            if (operatingCenterIds != null && operatingCenterIds.Any())
            {
                query = query.Where(x => operatingCenterIds.Contains(x.OperatingCenter.Id));
            }

            return new CascadingActionResult<WasteWaterSystem, WasteWaterSystemDisplayItem>(query.OrderBy(x => x.WasteWaterSystemName));
        }

        [HttpGet]
        public ActionResult GetSystemNameByOperatingCenter(int operatingCenterId)
        {
            var data = Repository.Where(x => x.OperatingCenter.Id == operatingCenterId);
            return new CascadingActionResult<WasteWaterSystem, WasteWaterSystemDisplayItem>(data.OrderBy(x =>
                x.WasteWaterSystemName));
        }

        [HttpGet]
        public ActionResult ByOperatingCenterAndTown(int? operatingCenter, int? town)
        {
            var data = Repository.Where(s =>
                s.OperatingCenter.Id == operatingCenter && (town == null || s.Towns.Any(t => t.Id == town)));
            return new CascadingActionResult<WasteWaterSystem, WasteWaterSystemDisplayItem>(data.OrderBy(s =>
                s.WasteWaterSystemName)) {SortItemsByTextField = false};
        }

        [HttpGet]
        public ActionResult ByOperatingCenters(int[] operatingCenters)
        {
            var data = operatingCenters == null || !operatingCenters.Any()
                ? Repository.GetAll()
                : Repository.Where(s => operatingCenters.Contains(s.OperatingCenter.Id));
            return new CascadingActionResult<WasteWaterSystem, WasteWaterSystemDisplayItem>(data.OrderBy(s =>
                s.WasteWaterSystemName)) {SortItemsByTextField = false};
        }

        [HttpGet]
        public ActionResult ActiveByStateOrOperatingCenter(int[] stateIds, int[] operatingCenterIds)
        {
            var query = Repository.Where(x => x.Status.Id == WasteWaterSystemStatus.Indices.ACTIVE);
            
            if (stateIds != null && stateIds.Any())
            {
                query = query.Where(x => stateIds.Contains(x.OperatingCenter.State.Id));
            }

            if (operatingCenterIds != null && operatingCenterIds.Any())
            {
                query = query.Where(x => operatingCenterIds.Contains(x.OperatingCenter.Id));
            }

            return new CascadingActionResult<WasteWaterSystem, WasteWaterSystemDisplayItem>(query.OrderBy(x => x.WasteWaterSystemName));
        }

        #endregion
    }
}
