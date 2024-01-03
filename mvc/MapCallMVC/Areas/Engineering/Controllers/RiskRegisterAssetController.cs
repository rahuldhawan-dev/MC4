using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Engineering.Models.ViewModels.RiskRegisterAssets;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Engineering.Controllers
{
    public class RiskRegisterAssetController : ControllerBaseWithPersistence<IRiskRegisterAssetRepository, RiskRegisterAsset, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EngineeringRiskRegister;
        public const string RISK_REGISTER_CREATED = "Risk Register Created",
                            RISK_REGISTER_COMPLETED = "Risk Register Completed",
                            EMAIL_TEMPLATE_NAME = "Risk Register Notification";

        #endregion

        #region Constructors

        public RiskRegisterAssetController(ControllerBaseWithPersistenceArguments<IRiskRegisterAssetRepository, RiskRegisterAsset, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private void SendNotification(int id)
        {
            var entity = Repository.Find(id);
            entity.RecordUrl = GetUrlForModel(entity, "Show", "RiskRegister", "Engineering");
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs
            {
                OperatingCenterId = entity.OperatingCenter.Id,
                Module = ROLE,
                TemplateName = EMAIL_TEMPLATE_NAME,
                Purpose = entity.CompletionActualDate.HasValue ? RISK_REGISTER_COMPLETED : RISK_REGISTER_CREATED,
                Data = entity
            };
            notifier.Notify(args);
        }

        #endregion

        #region Public Methods

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchRiskRegisterAssetViewModel search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchRiskRegisterAssetViewModel viewModel)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(viewModel));
                formatter.Excel(() => {
                    var results = Repository.Search(viewModel);
                    return new ExcelResult(GetExportFilename())
                          .AddSheet(new List<RiskRegisterAssetExcelDisclaimerViewModel> { new RiskRegisterAssetExcelDisclaimerViewModel() }, new MMSINC.Utilities.Excel.ExcelExportSheetArgs { SheetName = "Disclaimer" })
                          .AddSheet(results, new MMSINC.Utilities.Excel.ExcelExportSheetArgs { SheetName = "Risk Register Asset" });
                });
                formatter.Map(() => {
                    viewModel.EnablePaging = false;
                    var results = Repository.Search(viewModel);
                    return _container.With((IEnumerable<IThingWithCoordinate>)results).GetInstance<MapResultWithCoordinates>();
                });
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
                x.Map(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound();
                    return _container.With((IEnumerable<IThingWithCoordinate>)new[] { model }).GetInstance<MapResultWithCoordinates>();
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateRiskRegisterAsset>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateRiskRegisterAsset viewModel)
        {
            return ActionHelper.DoCreate(viewModel, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendNotification(viewModel.Id);
                    return RedirectToAction("Show", new { id = viewModel.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditRiskRegisterAsset>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditRiskRegisterAsset viewModel)
        {
            return ActionHelper.DoUpdate(viewModel, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    SendNotification(viewModel.Id);
                    return RedirectToAction("Show", new { id = viewModel.Id });
                }
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        private string GetExportFilename()
        {
            Func<DateTime?, string> doWeirdDateFormat =
                (d) => {
                    if (!d.HasValue) { return string.Empty; }

                    var val = d.Value;
                    return $"{val.Month:D2}{val.Day:D2}{val:yy}{val.Hour}{val.Minute}{val.Second}{val.Millisecond}";
                };

            var start = doWeirdDateFormat(DateTime.Now);
            return $"RiskRegisterAsset_{start}";
        }

        #endregion
    }
}
