using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class NPDESRegulatorInspectionController : ControllerBaseWithPersistence<INpdesRegulatorInspectionRepository, NpdesRegulatorInspection, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public NPDESRegulatorInspectionController(
            ControllerBaseWithPersistenceArguments<INpdesRegulatorInspectionRepository, NpdesRegulatorInspection, User> args)
            : base(args) { }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchNpdesRegulatorInspection>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchNpdesRegulatorInspection search)
        {
#if DEBUG
            if (MMSINC.MvcApplication.IsInTestMode && MMSINC.MvcApplication.RegressionTestFlags.Contains("document page size 5"))
            {
                search.PageSize = 5;
            }
#endif
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(x => new {
                        x.Id,
                        x.SewerOpening,
                        x.SewerOpening.OperatingCenter,
                        x.SewerOpening.Town,
                        x.InspectedBy,
                        x.SewerOpening.SewerOpeningType,
                        x.SewerOpening.Status,
                        x.SewerOpening.NpdesPermitNumber,
                        x.SewerOpening.Street,
                        x.SewerOpening.LocationDescription,
                        x.SewerOpening.OutfallNumber,
                        x.ArrivalDateTime,
                        x.DepartureDateTime,
                        x.NpdesRegulatorInspectionType,
                        x.HasInfiltration,
                        x.WeatherCondition,
                        x.OutfallCondition,
                        x.GateStatusAnswerType,
                        x.BlockCondition,
                        x.IsDischargePresent,
                        x.DischargeWeatherRelatedType,
                        x.RainfallEstimate,
                        x.BodyOfWater,
                        x.DischargeFlow,
                        x.DischargeCause,
                        x.DischargeDuration,
                        x.IsPlumePresent,
                        x.IsErosionPresent,
                        x.IsSolidFloatPresent,
                        x.IsAdditionalEquipmentNeeded,
                        x.HasSamplesBeenTaken,
                        x.SampleLocation,
                        x.HasFlowMeterMaintenanceBeenPerformed,
                        x.HasDownloadedFlowMeterData,
                        x.HasCalibratedFlowMeter,
                        x.HasRemovedFlowMeter,
                        x.HasFlowMeterBeenMaintainedOther,
                        x.Remarks,
                        x.SewerOpening.WasteWaterSystem,
                        x.SewerOpening.State,
                        x.SewerOpening.TownSection,
                        x.SewerOpening.StreetNumber,
                        CrossStreet = x.SewerOpening.IntersectingStreet
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region New/Create

        [SkipRoleOperatingCenterCheck]
        [HttpGet, ActionBarVisible(false), RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int id)
        {
            var opening = _container.GetInstance<IRepository<SewerOpening>>().Find(id);
            if (opening == null)
            {
                return HttpNotFound("NPDES Regulator not found");
            }

            var model = new CreateNpdesRegulatorInspection(_container) {
                SewerOpening = opening.Id
            };
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateNpdesRegulatorInspection model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    var action = model.IsMapPopup ? "Edit" : "Show";
                    if (model.IsMapPopup)
                    {
                        DisplaySuccessMessage("Successfully saved!");
                    }
                   
                    return RedirectToAction(action, new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditNpdesRegulatorInspection>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditNpdesRegulatorInspection model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var action = model.IsMapPopup ? "Edit" : "Show";
                    if (model.IsMapPopup)
                    {
                        DisplaySuccessMessage("Successfully updated!");
                    }

                    return RedirectToAction(action, new { id = model.Id });
                }
            });
        }

        #endregion
    }
}