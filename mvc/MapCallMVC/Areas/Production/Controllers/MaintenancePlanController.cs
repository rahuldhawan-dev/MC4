using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class MaintenancePlanController : ControllerBaseWithPersistence<IRepository<MaintenancePlan>, MaintenancePlan, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionPlannedWork;
        
        public enum ForecastDaysEnum
        {
            OneWeek = 7,
            OneMonth = 31,
            OneQuarter = 91,
            OneHalfYear = 182,
            ThreeQuarter = 273,
            OneYear = 365,
            PlusMonth = 400
        }

        #endregion

        #region Private Members 

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public MaintenancePlanController(
            ControllerBaseWithPersistenceArguments<IRepository<MaintenancePlan>, MaintenancePlan, User> args,
            IDateTimeProvider dateTimeProvider) : base(args)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchMaintenancePlan search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchMaintenancePlan search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(e => new Dictionary<string, object> {
                        {nameof(e.Id), e.Id},
                        {nameof(e.PlanNumber), e.PlanNumber},
                        {nameof(e.State), e.State},
                        {nameof(e.OperatingCenter), e.OperatingCenter},
                        {MaintenancePlan.DisplayNames.PLANNING_PLANT, e.PlanningPlant},
                        {nameof(e.Facility), e.Facility},
                        {MaintenancePlan.DisplayNames.PLAN_NAME, e.Name},
                        {MaintenancePlan.DisplayNames.PLAN_TYPE, e.PlanType},
                        {MaintenancePlan.DisplayNames.PRODUCTION_WORK_ORDER_FREQUENCY, e.ProductionWorkOrderFrequency},
                        {nameof(e.LocalTaskDescription), e.LocalTaskDescription},
                        {MaintenancePlan.DisplayNames.HAS_COMPLIANCE_REQUIREMENT, e.HasComplianceRequirement},
                        {nameof(e.IsActive), e.IsActive},
                        {nameof(e.LastWorkOrderCompleted), e.LastWorkOrderCompleted},
                        {nameof(e.NextWorkOrderDueDate), e.NextWorkOrderDueDate},
                        {nameof(e.TaskDetailsSummary), e.TaskDetailsSummary},
                        {nameof(e.TaskGroupCategory), e.TaskGroupCategory},
                        {MaintenancePlan.DisplayNames.TASK_GROUP, e.TaskGroup},
                        {nameof(e.TaskDescription), e.TaskDescription},
                        {MaintenancePlan.DisplayNames.RESOURCES, e.Resources},
                        {nameof(e.EstimatedHours), e.EstimatedHours},
                        {MaintenancePlan.DisplayNames.CONTRACTOR_COST, e.ContractorCost},
                        {MaintenancePlan.DisplayNames.HAS_COMPLETION_REQUIREMENT, e.HasACompletionRequirement},
                        {MaintenancePlan.DisplayNames.COMPANY_REQUIREMENT, e.HasCompanyRequirement},
                        {MaintenancePlan.DisplayNames.OSHA_REQUIREMENT, e.HasOshaRequirement},
                        {MaintenancePlan.DisplayNames.PSM_REQUIREMENT, e.HasPsmRequirement},
                        {MaintenancePlan.DisplayNames.REGULATORY_REQUIREMENT, e.HasRegulatoryRequirement},
                        {MaintenancePlan.DisplayNames.OTHER_COMPLIANCE, e.HasOtherCompliance},
                        {MaintenancePlan.DisplayNames.OTHER_COMPLIANCE_REASON, e.OtherComplianceReason},
                        {nameof(e.Start), e.Start},
                        {nameof(e.TaskDetails), e.TaskDetails},
                        {nameof(e.AdditionalTaskDetails), e.AdditionalTaskDetails}
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateMaintenancePlan>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateMaintenancePlan model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Copy

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Copy(int id)
        {
            var maintenancePlan = Repository.Find(id);
            if (maintenancePlan == null)
            {
                return DoHttpNotFound($"Could not find a maintenance plan with id {id}");
            }

            var copy = ViewModelFactory.BuildWithOverrides<CopyMaintenancePlan, MaintenancePlan>(maintenancePlan, new {
                Id = 0
            });

            return ActionHelper.DoCreate(copy, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    return RedirectToAction("Edit", new { id = copy.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditMaintenancePlan>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditMaintenancePlan model)
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

        #region Child Elements

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddScheduledAssignments(CreateScheduledAssignments model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs { 
                OnSuccess = () => RedirectToShowPageForecastTab(model.Id),
                OnError = () => RedirectToShowPageForecastTab(model.Id)
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveScheduledAssignments(RemoveScheduledAssignments model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToShowPageForecastTab(model.Id),
                OnError = () => RedirectToShowPageForecastTab(model.Id)
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddEquipmentMaintenancePlan(AddEquipmentMaintenancePlan model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveEquipmentMaintenancePlan(RemoveEquipmentMaintenancePlan model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    return RedirectToReferrerOr("Show", "Equipment", new { id = model.Equipment.FirstOrDefault() }, "#MaintenancePlansTab");
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveAllEquipmentMaintenancePlan(RemoveAllEquipmentMaintenancePlan model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpGet]
        public ActionResult ByFacilityIdAndEquipmentTypeId(int facilityId, int equipmentTypeId)
        {
            var equipmentType = _container.GetInstance<IRepository<EquipmentType>>().Find(equipmentTypeId);
            var query = Repository.Where(x => x.Facility.Id == facilityId && x.EquipmentTypes.Contains(equipmentType));

            return new CascadingActionResult<MaintenancePlan, MaintenancePlanDisplayItem>(query, "Display");
        }

        [NoCache]
        [HttpGet]
        [RequiresRole(ROLE)]
        public ActionResult Forecast(int maintenancePlanId)
        {
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();
            var today = _dateTimeProvider.GetCurrentDate();
            var maintenancePlan = Repository.Find(maintenancePlanId);

            var forecastWorkOrders = GetForecastWorkOrders(maintenancePlan, today);

            return PartialView("_ForecastResults", new MaintenancePlanForecastResults {
                MaintenancePlan = maintenancePlan.Id,
                OperatingCenter = maintenancePlan.OperatingCenter.Id, 
                ForecastWorkOrders = forecastWorkOrders
            });
        }

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.New:
                case ControllerAction.Search:
                    //EquipmentTypes property only needs to be populated here for search screen, for Edit and New , it will be cascading
                    if (action == ControllerAction.Search)
                    {
                        this.AddDropDownData<EquipmentType>("EquipmentTypes", f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    }
                    this.AddDropDownData<ProductionWorkOrderFrequency>(x => x.GetAllSorted(y => y.SortOrder), x => x.Id,
                        x => x.Name);
                    break;
                case ControllerAction.Show:
                    this.AddDropDownData<ProductionWorkOrderFrequency>(x => x.GetAllSorted(y => y.SortOrder), x => x.Id,
                        x => x.Name);
                    this.AddEnumDropDownData<ForecastDaysEnum>("ForecastDays");
                    break;
            }
        }

        private ActionResult RedirectToShowPageForecastTab(int id)
        {
            return RedirectToReferrerOr("Show", "MaintenancePlan", new { id = id }, "#SchedulingForecastTab");
        }

        private static IEnumerable<ForecastWorkOrder> GetForecastWorkOrders(MaintenancePlan maintenancePlan, DateTime today)
        {
            return maintenancePlan.ProductionWorkOrderFrequency
                                  .GetForecastDates(today)
                                  .Select(x => new ForecastWorkOrder {
                                       PlannedDate = x.ToShortDateString(),
                                       LocalTaskDescription = maintenancePlan.LocalTaskDescription,
                                       Resources = maintenancePlan.Resources.ToString(),
                                       EstimatedHours = maintenancePlan.EstimatedHours.ToString(),
                                       SkillSet = maintenancePlan.SkillSet != null ? maintenancePlan.SkillSet.ToString() : string.Empty,
                                       Assignments = maintenancePlan.ScheduledAssignments.Where(assignment => assignment.ScheduledDate.Date == x.Date)
                                   })
                                  .ToList(); // We only want to enumerate GetForecastDates() once, since it uses logic to generate the values. The UI can have a simple list to pass around
        }

        #endregion
    }
}