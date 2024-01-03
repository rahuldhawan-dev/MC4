using System;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using StructureMap;

namespace MapCallMVC.Controllers
{
    public class PublicWaterSupplyController : ControllerBaseWithPersistence<IPublicWaterSupplyRepository, PublicWaterSupply, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalWaterSystems;
        public const string NOTIFICATION_PURPOSE1 = "PWSID Created";
        public const string NOTIFICATION_PURPOSE2 = "PWSID Status Change";

        #endregion

        #region Private Methods

        //public int _statusBeforeEdit;//temp to hold status; if status changes on update, it will send notification
        internal void SendPWSIDNotification(IContainer container, PublicWaterSupply entity, string thisPurpose)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "PublicWaterSupply");
            var notifier = container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = 0, //there are multiple operating centers, in the notify method if operating center is 0 it will look it up by Module and Purpose
                Module = ROLE,
                Purpose = thisPurpose,
                Data = entity
            };
            notifier.Notify(args);
        }

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            // Show has an OperatingCenters tab
            this.AddOperatingCenterDropDownData();

            this.AddDropDownData<PlanningPlant>("PlanningPlant", x => x.Id, x => x.Display);

            if (!new[] { ControllerAction.Search, ControllerAction.Show }.Contains(action))
            {
                // we only want to have to query this once
                var employees = ((IQueryable<EmployeeDisplayItem>)_container.GetInstance<IRepository<Employee>>()
                                                                            .GetActiveEmployeesForSelect()
                                                                            .SelectDynamic<Employee, EmployeeDisplayItem>().Result).ToList();

                this.AddDropDownData("ExecutiveDirector", employees, x => x.Id, x => x.Display);
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchPublicWaterSupply>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchPublicWaterSupply search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(x => new {
                        x.Id,
                        x.State,
                        x.System,
                        PWSID = x.Identifier,
                        OperatingCenters = string.Join(", ", x.OperatingCenterPublicWaterSupplies.Select(y => y.OperatingCenter.ToString())),
                        x.Status,
                        x.Ownership,
                        x.DateOfOwnership,
                        x.ConsentOrderEndDate,
                        x.NewSystemInitialSafetyAssessmentCompleted,
                        x.DateSafetyAssessmentActionItemsCompleted,
                        x.NewSystemInitialWQEnvAssessmentCompleted,
                        x.DateWQEnvAssessmentActionItemsCompleted,
                        x.Type,
                        x.HasConsentOrder,
                        x.WaterSampleComplianceFormForTheCurrentMonth
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
            return ActionHelper.DoNew(new CreatePublicWaterSupply(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreatePublicWaterSupply model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {      
                OnSuccess = () => {                        
                    SendPWSIDNotification(_container, Repository.Find(model.Id), NOTIFICATION_PURPOSE1);
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        { 
            return ActionHelper.DoEdit<EditPublicWaterSupply>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditPublicWaterSupply model)
        {
            bool sendStatusNotification = false;
            var entity = Repository.Find(model.Id);

            if (model.Status > 0 && entity?.Status?.Id > 0)
            {
                if (model.Status != entity.Status.Id)
                {
                    sendStatusNotification = true;
                }
            }

            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (sendStatusNotification)
                    {
                        SendPWSIDNotification(_container, Repository.Find(model.Id), NOTIFICATION_PURPOSE2);
                    }

                    return null;
                }
            });
        }

        #endregion

        #region ActiveByOperatingCenterId(s)

        [HttpGet]
        public ActionResult ActiveByOperatingCenterId(params int[] operatingCenterIds) =>
            new CascadingActionResult<PublicWaterSupply, PublicWaterSupplyDisplayItem>(
                Repository.GetActiveByOperatingCenterId(operatingCenterIds));

        #endregion

        #region ActiveOrPendingByOperatingCenterId(s)

        [HttpGet]
        public ActionResult ActiveOrPendingByOperatingCenterId(params int[] operatingCenterIds) =>
            new CascadingActionResult<PublicWaterSupply, PublicWaterSupplyDisplayItem>(
                Repository.GetActiveOrPendingByOperatingCenterId(operatingCenterIds));

        #endregion

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(params int[] operatingCenterIds) =>
            new CascadingActionResult<PublicWaterSupply, PublicWaterSupplyDisplayItem>(
                Repository.GetByOperatingCenterId(operatingCenterIds));

        #endregion

        #region ByOperatingCenterIdsAndAWOwned

        [HttpGet]
        public ActionResult ByOperatingCenterIdsAndAWOwned(int[] id) =>
            new CascadingActionResult(Repository.GetAWOwnedByOperatingCenterIds(id), "Description", "Id");

        #endregion

        #region ByPartialPWSIDMatch

        [HttpGet]
        public ActionResult ByPartialPWSIDMatch(string partial)
        {
            var results = Repository.FindByPartialIdMatch(partial);
            return new AutoCompleteResult(results, "Identifier", "Identifier");
        }

        #endregion

        #region ByStateId

        [HttpGet]
        public ActionResult ByStateId(params int[] stateIds) =>
            new CascadingActionResult<PublicWaterSupply, PublicWaterSupplyDisplayItem>(
                Repository.GetByStateId(stateIds));

        #endregion
        
        #region ByOperatingCenterOrState

        [HttpGet]
        public ActionResult ByOperatingCenterOrState(int[] states, int[] operatingCenters)
        {
            IQueryable<PublicWaterSupply> ret;

            if (operatingCenters != null && operatingCenters.Any())
            {
                ret = Repository.Where(pws =>
                    pws.OperatingCenterPublicWaterSupplies.Any(ocpws =>
                        operatingCenters.Contains(ocpws.OperatingCenter.Id)));
            }
            else if (states != null && states.Any())
            {
                ret = Repository.Where(pws => states.Contains(pws.State.Id));
            }
            else
            {
                ret = Array.Empty<PublicWaterSupply>().AsQueryable();
            }

            return new CascadingActionResult<PublicWaterSupply, PublicWaterSupplyDisplayItem>(ret);
        }
        
        #endregion

        #region GetSystemNameByOperatingCenter

        [HttpGet]
        public ActionResult GetSystemNameByOperatingCenter(int operatingCenterId) =>
            new CascadingActionResult<PublicWaterSupply, PublicWaterSupplyDisplayItemForNearMiss>(Repository
               .GetByOperatingCenterId(operatingCenterId));

        #endregion

        #region GetByStateOrOperatingCenter

        [HttpGet]
        public ActionResult ActiveByStateIdOrOperatingCenterId(int[] stateIds, int[] operatingCenterIds) =>
            new CascadingActionResult<PublicWaterSupply, PublicWaterSupplyDisplayItem>(
                Repository.GetActiveByStateIdOrOperatingCenterId(stateIds, operatingCenterIds));

        #endregion

        #region Constructors

        public PublicWaterSupplyController(ControllerBaseWithPersistenceArguments<IPublicWaterSupplyRepository, PublicWaterSupply, User> args) : base(args) { }

        #endregion
    }
}
