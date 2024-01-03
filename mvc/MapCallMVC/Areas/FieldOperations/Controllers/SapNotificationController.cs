using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    [DisplayName("SAP Notifications")]
    public class SapNotificationController : ControllerBaseWithPersistence<WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesSAPNotifications;
        public const string BILLING_PARTY_NOT_FOUND = "Notifications not found.",
            SUCCESS_MESSAGE_COMPLETED = "Notification Completed Successfully",
            SUCCESS_MESSAGE_CANCELLED = "Notification Cancelled Successfully";

        public const string TEMP_DATA_CREATE_WORK_ORDER_NOTES = "CreateWorkOrderNotes";
        public const string TEMP_DATA_CREATE_WORK_ORDER_SPECIAL_INSTRUCTIONS = "CreateWorkOrderSpecialInstructions";

        #endregion

        #region Constructors

        //public SapNotificationController(ControllerBaseWithPersistenceArguments<IRepository<SAPNotification>, SAPNotification, User> args) : base(args) {}
        public SapNotificationController(ControllerBaseWithPersistenceArguments<IRepository<WorkOrder>, WorkOrder, User> args) : base(args) { }
        
        #endregion

        #region Private Methods 

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                var usersOperatingCentersForRole = this.GetUserOperatingCentersFn(ROLE).Invoke(_container.GetInstance<IOperatingCenterRepository>()).ToList().Select(x => x.Id).ToArray();

                // WO0000000213827 asked for the PlanningPlant dropdown to be sorted by OperatingCenter
                // NOTE: PlanningPlant.OperatingCenter is nullable at the time of making this, even though none of them in the database have null OperatingCenterID values.
                var planningPlants = _container.GetInstance<IRepository<PlanningPlant>>().Where(z =>
                    !z.Code.StartsWith("P") &&
                    usersOperatingCentersForRole.Contains(z.OperatingCenter.Id) &&
                    z.OperatingCenter.SAPEnabled &&
                    z.OperatingCenter.SAPWorkOrdersEnabled).OrderBy(x =>
                    x.OperatingCenter == null
                        ? ""
                        : x.OperatingCenter.OperatingCenterCode).ToList();
                this.AddDropDownData(planningPlants, y => y.Code, z => z.ToString());
                this.AddDropDownData<SAPNotificationType>("NotificationType", x => x.GetAllSorted(), x => x.Id, x => x.Description);
                this.AddDropDownData<SAPWorkOrderPriority>("Priority", x => x.GetAllSorted(), x => x.Id, x => x.Description);
                this.AddDropDownData<SAPWorkOrderPurpose>("Code", x => x.GetAllSorted(), x => x.Code, x => x.Description);
            }
        }

        #endregion

        #region Properties

        private ISAPNotificationRepository _sapNotificationRepository;

        public ISAPNotificationRepository SAPNotificationRepository
        {
            get
            {
                return _sapNotificationRepository ?? (_sapNotificationRepository = _container.GetInstance<ISAPNotificationRepository>());
            }
            set { _sapNotificationRepository = value; }
        }

        #endregion

        #region Exposed Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            SetLookupData(ControllerAction.Search);
            return View(new SearchSapNotification());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchSapNotification search)
        {
            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();
                return DoRedirectionToAction("Search", search);
            }

            if (!string.IsNullOrWhiteSpace(search.SAPNotificationNumber))
                return RedirectToAction("Show",
                    new CreateSapNotificationWorkOrder(_container) {SAPNotificationNumber = search.SAPNotificationNumber});

            var results = SAPNotificationRepository.Search(search.ToSearchSapNotification());

            switch (results.Result)
            {
                case SAPNotificationCollectionResult.Success:
                    return View("Index", results.OrderBy(x => x.City).ThenBy(x => x.Street1));

                default: // For Error and any additional enum values that may get added
                    DisplayErrorMessage(results.First().SAPErrorCode);
                    return DoRedirectionToAction("Search", search);
            }

            // They embed SAPErrorCode in the first result (annoying)
            // If we have one result and SAPErrorCode isn't Success, we have a real error
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)] 
        public ActionResult Update(EditSapNotification model)
        {
            // EditSapNotification is a proper ViewModel<SAPNotification> yet it isn't being used
            // for mapping for some reason? -Ross 12/27/2017
            var sapStat = new SAPNotificationStatus {
                SAPNotificationNo = model.SAPNotificationNumber,
                Cancel = model.Cancel,
                Complete = model.Complete,
                UserName = AuthenticationService?.CurrentUser?.FullName
            };

            if (!string.IsNullOrWhiteSpace(model.ReadOnlyNotes))
            {
                sapStat.Remarks = model.ReadOnlyNotes + Environment.NewLine + model.Remarks;
            }
            else
            {
                sapStat.Remarks = model.Remarks;
            }

            var result = SAPNotificationRepository.Save(sapStat);

            if (!string.IsNullOrWhiteSpace(result.SAPMessage) &&
                    result.SAPMessage != SUCCESS_MESSAGE_COMPLETED &&
                    result.SAPMessage != SUCCESS_MESSAGE_CANCELLED)
            {
                DisplayErrorMessage(result.SAPMessage);
            }
            else
            {
                DisplaySuccessMessage(result.SAPMessage);
            }

            // WaterQualityComplaints use this to redirect back to the Show record this post is coming from.
            if (!string.IsNullOrWhiteSpace(model.RedirectUrl))
            {
                return Redirect(model.RedirectUrl);
            }

            return DoRedirectionToAction("Index", model.IndexSearch?.ToRouteValues());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(CreateSapNotificationWorkOrder model)
        {
            // we're going to query the webservice again with all the search values
            // because we don't have a way to query by SAPNotificationNumber.
            //Repository.Search()
            var notification =
                SAPNotificationRepository.SearchWorkOrder(new SAPNotification {
                    CreateWorkOrderNotificationNumber = model.SAPNotificationNumber
                });

            switch (notification.Result)
            {
                case SAPNotificationCollectionResult.Success:
                    model = ViewModelFactory.Build<CreateSapNotificationWorkOrder, SAPNotification>(notification.FirstOrDefault());
                    TempData[TEMP_DATA_CREATE_WORK_ORDER_NOTES] = model.NotificationLongText + " Locality : " + model.Locality + " LocalityDescription : " + model.LocalityDescription;
                    TempData[TEMP_DATA_CREATE_WORK_ORDER_SPECIAL_INSTRUCTIONS] = model.SpecialInstructions;
                    //model = new CreateSapNotificationWorkOrder(new RouteValueDictionary(notification));
                    //var rvd = new RouteValueDictionary(model.ToCreateWorkOrder());
                    //rvd.Add("area", "FieldOperations");

                    // If there are open work orders, lets go to a show page, otherwise lets create an order
                    //return model.OpenWorkOrderCount > 0  ? DoView("Show", model) : DoRedirectionToAction("New", "WorkOrder", rvd);
                    return this.RespondTo(formatter => {
                        formatter.View(() => DoView("Show", model));
                        formatter.Fragment(() => DoView("_ShowPopup", model, partial: false));
                    });

                default:
                    DisplayErrorMessage(notification.FirstOrDefault().SAPErrorCode);
                    return RedirectToAction("Search");
            }
        }

        #endregion
    }
}