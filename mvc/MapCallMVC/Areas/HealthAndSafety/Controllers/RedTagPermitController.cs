using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class RedTagPermitController : ControllerBaseWithPersistence<IRedTagPermitRepository, RedTagPermit, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        public const string CREATE_NOTIFICATION = "Red Tag Permit Out Of Service";

        public const string EDIT_NOTIFICATION = "Red Tag Permit In Service";

        #endregion

        #region Constructors

        public RedTagPermitController(ControllerBaseWithPersistenceArguments<IRedTagPermitRepository, RedTagPermit, User> args) : base(args) {}

        #endregion

        #region Private Methods

        private void SendCreateNotification(CreateRedTagPermitViewModel model)
        {
            var templateModel = Repository.Find(model.Id);

            var newModel = new RedTagPermitNotification {
                ProductionWorkOrderRecordUrl = GetUrlForModel(templateModel.ProductionWorkOrder, "Show", "ProductionWorkOrder", "Production"),
                RecordUrl = GetUrlForModel(templateModel, "Show", "RedTagPermit", "HealthAndSafety"),
                RedTagPermit = templateModel
            };

            var notifier = _container.GetInstance<INotificationService>();

            var args = new NotifierArgs {
                OperatingCenterId = templateModel.OperatingCenter.Id,
                Module = ROLE,
                Purpose = CREATE_NOTIFICATION,
                Data = newModel
            };

            notifier.Notify(args);
        }

        private void SendUpdateNotification(RedTagPermitViewModel model)
        {
            var templateModel = Repository.Find(model.Id);

            var newModel = new RedTagPermitNotification {
                ProductionWorkOrderRecordUrl = GetUrlForModel(templateModel.ProductionWorkOrder, "Show", "ProductionWorkOrder", "Production"),
                RecordUrl = GetUrlForModel(templateModel, "Show", "RedTagPermit", "HealthAndSafety"),
                RedTagPermit = templateModel
            };

            var notifier = _container.GetInstance<INotificationService>();

            var args = new NotifierArgs {
                OperatingCenterId = templateModel.OperatingCenter.Id,
                Module = ROLE,
                Purpose = EDIT_NOTIFICATION,
                Data = newModel
            };

            notifier.Notify(args);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, 
         RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchRedTagPermitViewModel viewModel)
        {
            return ActionHelper.DoSearch(viewModel);
        }

        [HttpGet, 
         RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchRedTagPermitViewModel viewModel)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(viewModel));
                formatter.Excel(() => {
                    viewModel.EnablePaging = false;
                    return ActionHelper.DoExcel(viewModel);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, 
         ActionBarVisible(false),
         RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(NewRedTagPermitViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound("One or more of the provided ids could not be found.");
            }

            var createViewModel = ViewModelFactory.BuildWithOverrides<CreateRedTagPermitViewModel>(new {
                viewModel.ProductionWorkOrder,
                viewModel.OperatingCenter,
                viewModel.Equipment
            });

            return ActionHelper.DoNew(createViewModel);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateRedTagPermitViewModel viewModel)
        {
            return ActionHelper.DoCreate(viewModel, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendCreateNotification(viewModel);
                    return null;
                },
                OnError = () => {
                    viewModel.SetDefaults();
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, 
         RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditRedTagPermitViewModel>(id);
        }

        [HttpPost, 
         RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditRedTagPermitViewModel viewModel)
        {
            return ActionHelper.DoUpdate(viewModel, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var entity = _repository.Find(viewModel.Id);

                    if (entity.EquipmentRestoredOn.HasValue)
                    {
                        SendUpdateNotification(viewModel);
                    }

                    return null;
                }
            });
        }

        #endregion
    }
}
