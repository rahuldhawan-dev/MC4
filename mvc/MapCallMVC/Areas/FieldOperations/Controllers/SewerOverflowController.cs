using System;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerOverflows;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class SewerOverflowController : ControllerBaseWithPersistence<ISewerOverflowRepository, SewerOverflow, User>
    {
        private readonly IWorkOrderRepository _workOrderRepository;

        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        public const string NOTIFICATION_PURPOSE = "Sewer Overflow",
                            NOTIFICATION_SUBJECT = "Sewer Overflow DEP Notification : {0}";

        #endregion

        #region Private Methods

        private void SendCreationsMostBodaciousNotification(SewerOverflow model)
        {
            model.RecordUrl = GetUrlForModel(model, "Show", "SewerOverflow", "FieldOperations");

            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                Subject = String.Format(NOTIFICATION_SUBJECT, model.Id),
                OperatingCenterId = model.OperatingCenter.Id,
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
            this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
            this.AddDropDownData<SewerOverflowType>("OverflowType",
                x => x.GetAllSorted(y => y.Id),
                x => x.Id, 
                x => x.Description);
        }
        
        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchSewerOverflow search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchSewerOverflow search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    // TODO: This should ideally be built into the BoolFormat attribute, or at least a reusable helper method somewhere
                    var toYesNo = new Func<bool?, string>(b => {
                        if (b == null)
                        {
                            return string.Empty;
                        }

                        return b.Value ? "Yes" : "No";
                    });
                    
                    search.EnablePaging = false;
                    var results = Repository
                                 .Search(search)
                                 .Select(x => new {
                                      x.Id,
                                      OperatingCenter = x.OperatingCenter?.OperatingCenterCode,
                                      x.WasteWaterSystem,
                                      x.IncidentDate,
                                      x.Town,
                                      x.StreetNumber,
                                      x.Street,
                                      x.CrossStreet,
                                      x.Coordinate,
                                      x.WorkOrder,
                                      x.OverflowType,
                                      x.WeatherType,
                                      x.GallonsOverflowedEstimated,
                                      x.DischargeLocation,
                                      x.BodyOfWater,
                                      x.DischargeLocationOther,
                                      HowManyGallonsFlowedIntoBodyOfWater = x.GallonsFlowedIntoBodyOfWater,
                                      x.SewerClearingMethod,
                                      x.AreaCleanedUpTo,
                                      x.SewageRecoveredGallons,
                                      OverflowOnCustomerSide = toYesNo(x.OverflowCustomers),
                                      x.OverflowCause,
                                      x.ZoneType,
                                      x.LocationOfStoppage,
                                      x.TalkedTo,
                                      x.EnforcingAgencyCaseNumber,
                                      x.CallReceived,
                                      x.CrewArrivedOnSite,
                                      x.SewageContained,
                                      x.StoppageCleared,
                                      x.WorkCompleted,
                                      x.CreatedBy,
                                      x.CreatedAt,
                                  }).ToList();
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? workOrderId = null)
        {
            var model = new CreateSewerOverflow(_container);

            if (workOrderId.HasValue)
            {
                PopulateFromWorkOrder(workOrderId.Value, model);
            }

            return ActionHelper.DoNew(model);
        }

        private void PopulateFromWorkOrder(int workOrderId, CreateSewerOverflow model)
        {
            var wo = _workOrderRepository.Find(workOrderId);

            if (wo != null)
            {
                model.State = wo.State?.Id;
                model.WorkOrder = wo.Id;
                model.OperatingCenter = wo.OperatingCenter.Id;
                model.Town = wo.Town.Id;
                model.Street = wo.Street.Id;
                model.StreetNumber = wo.StreetNumber;
                model.CrossStreet = wo.NearestCrossStreet.Id;
                model.IncidentDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                if (wo.Coordinate != null)
                {
                    model.Coordinate = GetCoordinate(wo.Coordinate);
                }

                if (wo.Town != null && wo.Town.WasteWaterSystems.Count == 1)
                {
                    model.WasteWaterSystem = wo.Town.WasteWaterSystems.First().Id;
                }
            }
        }

        private int GetCoordinate(Coordinate woCoordinate)
        {
            var coordinate =
                _container.GetInstance<IRepository<Coordinate>>()
                          .Save(new Coordinate {
                               Latitude = woCoordinate.Latitude,
                               Longitude = woCoordinate.Longitude,
                               Icon = woCoordinate.Icon
                           });

            return coordinate.Id;
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateSewerOverflow model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (!string.IsNullOrWhiteSpace(model.EnforcingAgencyCaseNumber))
                    {
                        SendCreationsMostBodaciousNotification(Repository.Find(model.Id));
                    }

                    return RedirectToAction("Show", new {id = model.Id});
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditSewerOverflow>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditSewerOverflow viewModel)
        {
            return ActionHelper.DoUpdate(viewModel);
        }

        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Cascades

        [HttpGet]
        public ActionResult ByStreetId(int streetId)
        {
            return new CascadingActionResult(Repository.FindByStreetId(streetId), "Id", "Id") {
                SortItemsByTextField = false
            };
        }

        #endregion

        #region Constructors

        public SewerOverflowController(
            ControllerBaseWithPersistenceArguments<ISewerOverflowRepository, SewerOverflow, User> args,
            IWorkOrderRepository workOrderRepository) : base(args)
        {
            _workOrderRepository = workOrderRepository;
        }

        #endregion
    }
}
