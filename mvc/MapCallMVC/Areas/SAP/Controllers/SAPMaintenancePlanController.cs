using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.SAP.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using StructureMap.Query;
using SAPManualCall = MapCallMVC.Areas.SAP.Models.ViewModels.SAPManualCall;

namespace MapCallMVC.Areas.SAP.Controllers
{
    public class SAPMaintenancePlanController : ControllerBaseWithPersistence<WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        #endregion

        #region Private Members

        private ISAPMaintenancePlanLookupRepository _sapMaintenancePlanLookupRepository;

        #endregion

        #region Properties

        public ISAPMaintenancePlanLookupRepository SAPMaintenancePlanLookupRepository
        {
            get
            {
                return _sapMaintenancePlanLookupRepository ?? (_sapMaintenancePlanLookupRepository =
                           _container.GetInstance<ISAPMaintenancePlanLookupRepository>());
            }
            set { _sapMaintenancePlanLookupRepository = value; }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(SearchSAPMaintenancePlan search)
        {
            var planningPlants = _container.GetInstance<IRepository<PlanningPlant>>();

            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();
                return DoRedirectionToAction("Search", search);
            }
            
            var results =
                (IEnumerable<SAPMaintenancePlanLookup>)SAPMaintenancePlanLookupRepository.Search(
                    search.ToSearchSAPMaintenancePlan(planningPlants));

            if (results.Any() && results.Count() == 1 && results.First() != null &&
                !results.First().SAPErrorCode.StartsWith("Success"))
            {
                DisplayErrorMessage(results.First().SAPErrorCode);
                return DoRedirectionToAction("Search", search);
            }

            var result = results.First();
            result.SapSchedulingList =
                result.SapSchedulingList?.OrderByDescending(x =>
                    x.PlanDate == null ? (DateTime?)null : DateTime.Parse(x.PlanDate));
            ViewData["MaintenancePackage"] = results.First().SapRecordCycleList?.Select(x => new SelectListItem { Value = x.Cycle, Text = x.CycleText });
            return View("Show", results.First());

        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            SetLookupData(ControllerAction.Search);
            return View(new SearchSAPMaintenancePlan());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchSAPMaintenancePlan search)
        {
            var planningPlants = _container.GetInstance<IRepository<PlanningPlant>>();

            return this.RespondTo((formatter) => {
                formatter.View(() => {
                    if (!ModelState.IsValid)
                    {
                        DisplayModelStateErrors();
                        return DoRedirectionToAction("Search", search);
                    }
                    
                    var results =
                        (IEnumerable<SAPMaintenancePlanLookup>)SAPMaintenancePlanLookupRepository.Search(
                            search.ToSearchSAPMaintenancePlan(planningPlants));

                if (results.Any() && results.Count() == 1 && results.First() != null &&
                        !results.First().SAPErrorCode.StartsWith("Success"))
                    {
                        DisplayErrorMessage(results.First().SAPErrorCode);
                        return DoRedirectionToAction("Search", search);
                    }
                    return View("Index", results);
                });

                formatter.Fragment(() => {
                    var results =
                        (IEnumerable<SAPMaintenancePlanLookup>)SAPMaintenancePlanLookupRepository.Search(
                            search.ToSearchSAPMaintenancePlan(planningPlants));
                    ViewData["Equipment"] = search.Equipment;
                    ViewData["MapCallEquipmentId"] = search.MapCallEquipmentId;
                    ViewData["FunctionalLocation"] = search.FunctionalLocation;
                    ViewData["ShowAddToMaintenancePlan"] = search.ShowAddToMaintenancePlan;
                    ViewData["ShowRemoveFromMaintenancePlan"] = search.ShowRemoveFromMaintenancePlan;

                    return PartialView("_Index", results);
                });
            });
        }

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;
            }
        }

        #endregion

        #region Edit/Update

        [HttpPost, RequiresRole(ROLE)]
        public ActionResult AddEquipmentToMaintenancePlan(AddEquipmentToMaintenancePlan model)
        {
            var update = new SAPMaintenancePlanUpdate { MaintenancePlan = model.MaintenancePlan };
            update.SapAddRemoveItem.Add(new SAPAddRemoveItem {
                Equipment = model.Equipment,
                MaintenancePlan = model.MaintenancePlan,
                FunctionalLocation = model.FunctionalLocation,
                Item = model.MaintenanceItem,
                Action = "ADD"
            });
            var result = SAPMaintenancePlanLookupRepository.Save(update).First();
            if (!result.SAPErrorCode.ToUpper().Contains("Success"))
                DisplayErrorMessage(result.SAPErrorCode);

            return DoRedirectionToAction("Show", "Equipment", new {area = "", id = model.MapCallEquipmentId});
        }

        #endregion

        #region Constructors

        public SAPMaintenancePlanController(ControllerBaseWithPersistenceArguments<IRepository<WorkOrder>, WorkOrder, User> args) : base(args) { }

        #endregion

        [HttpPost, RequiresRole(ROLE)]
        public ActionResult RemoveEquipmentFromMaintenancePlan(RemoveEquipmentFromMaintenancePlan model)
        {
            var update = new SAPMaintenancePlanUpdate { MaintenancePlan = model.MaintenancePlan };
            var functionalLocation = _container.GetInstance<IEquipmentRepository>().Find(model.MapCallEquipmentId).FunctionalLocation;
            update.SapAddRemoveItem.Add(new SAPAddRemoveItem
            {
                Equipment = model.Equipment,
                MaintenancePlan = model.MaintenancePlan,
                FunctionalLocation = functionalLocation,
                Action = "REMOVE",
                Item = model.MaintenanceItem
            });
            var result = SAPMaintenancePlanLookupRepository.Save(update).First();
            if (!result.SAPErrorCode.ToUpper().Contains("SUCCESS"))
                DisplayErrorMessage(result.SAPErrorCode);

            return DoRedirectionToAction("Show", "Equipment", new { area = "", id = model.MapCallEquipmentId });
        }

        [HttpPost, RequiresRole(ROLE)]
        public ActionResult SapFixDate(EditSapFixCall model)
        {
            var update = new SAPMaintenancePlanUpdate {MaintenancePlan = model.MaintenancePlan};
            update.SapFixCall = new SAPFixCall {
                MaintenancePlan = model.MaintenancePlan,
                CallNumber = model.CallNumber,
                PlanDate = model.CorrectedPlanDate
            };
            var result = SAPMaintenancePlanLookupRepository.Save(update).First();
            if (!result.SAPErrorCode.ToUpper().Contains("SUCCESS"))
                DisplayErrorMessage(result.SAPErrorCode);
            else
                DisplaySuccessMessage(result.SAPErrorCode);
            return DoRedirectionToAction("Show", "SAPMaintenancePlan", new {area = "SAP", model.MaintenancePlan});

        }

        [HttpPost, RequiresRole(ROLE)]
        public ActionResult SapManualCall(SAPManualCall model)
        {
            var update = new SAPMaintenancePlanUpdate { MaintenancePlan = model.MaintenancePlan };
            update.SapManualCall = new MapCall.SAP.Model.Entities.SAPManualCall
            {
                MaintenancePackage = model.MaintenancePackage,
                MaintenancePlan = model.MaintenancePlan,
                ManualCallDate = model.ManualCallDate
            };
            var result = SAPMaintenancePlanLookupRepository.Save(update).First();
            if (!result.SAPErrorCode.ToUpper().Contains("SUCCESS"))
                DisplayErrorMessage(result.SAPErrorCode);
            return DoRedirectionToAction("Show", "SAPMaintenancePlan", new { area = "SAP", model.MaintenancePlan });
        }

        [HttpPost, RequiresRole(ROLE)]
        public ActionResult SapSkipCall(SAPSkipCall model)
        {
            var update = new SAPMaintenancePlanUpdate {MaintenancePlan = model.MaintenancePlan};
            update.SapSkipCall= new SAPSkipCall {
                MaintenancePlan = model.MaintenancePlan,
                CallNumber = model.CallNumber
            };
            var result = SAPMaintenancePlanLookupRepository.Save(update).First();
            if (!result.SAPErrorCode.ToUpper().Contains("SUCCESS"))
                DisplayErrorMessage(result.SAPErrorCode);
            else
                DisplaySuccessMessage(result.SAPErrorCode);

            return DoRedirectionToAction("Show", "SAPMaintenancePlan", new {area = "SAP", model.MaintenancePlan});
        }
    }
}
