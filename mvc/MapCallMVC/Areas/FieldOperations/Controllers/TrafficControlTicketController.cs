using System;
using System.Linq;
using System.Web.Mvc;
using AuthorizeNet;
using AuthorizeNet.Utility.NotProvided;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class TrafficControlTicketController : ControllerBaseWithPaymentHandling<ITrafficControlTicketRepository, TrafficControlTicket, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const string NOTIFICATION_PURPOSE = "Traffic Control Ticket Invoice Entered",
            NOTIFICATION_PURPOSE_NEW = "Traffic Control Ticket Entered",
            NOTIFICATION_PURPOSE_PAYMENT = "Traffic Control Payment Submitted",
            MARKED_AS_SUBMITTED_PURPOSE = "Traffic Control Payment Submitted",
            MARKED_AS_SUBMITTED_SUBJECT = "Traffic Control Payment Submitted for SAP WO# {0}",
            NOT_FOUND = "Unable to locate the traffic control ticket with the requested id: {0}",
            CANCELED = "This ticket has been canceled.",
            NOT_PAID_FOR = "This ticket has not been paid for.",
            ALREADY_SUBMITTED = "This ticket has already been marked as submitted.",
            NO_TRACKING_NUMBER = "This ticket does not yet have a tracking number.",
            CHECKS_INCORRECT = "This ticket does not have the correct check information entered.",
            SUCCESSFUL_PAYMENT_MESSAGE = "Your invoice will now be processed and submitted.";
        
        #endregion

        #region Private Members

        private AuthorizeNet.Customer _currentCustomer;

        #endregion
        
        #region Properties

        public ICustomerGateway CustomerGateway
        {
            get { return _container.GetInstance<IExtendedCustomerGateway>(); }
        }

        public AuthorizeNet.Customer CurrentCustomer
        {
            get
            {
                return _currentCustomer ??
                       (_currentCustomer =
                        CustomerGateway.GetCustomer(AuthenticationService.CurrentUser.CustomerProfileId.ToString()));
            }
        }

        #endregion

        #region Private Methods

        private void SendCreationsMostBodaciousNotification(TrafficControlTicket entity)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "TrafficControlTicket", "FieldOperations");
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs
            {
                OperatingCenterId = (entity.OperatingCenter != null) ? entity.OperatingCenter.Id : 0,
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE_NEW,
                Data = entity
            };
            notifier.Notify(args);
        }

        private void SendUpdatesMostBodaciousNotification(TrafficControlTicket entity)
        {
            if (entity.InvoiceValid)
            {
                entity.RecordUrl = GetUrlForModel(entity, "Show", "TrafficControlTicket", "FieldOperations");
                var notifier = _container.GetInstance<INotificationService>();
                var args = new NotifierArgs {
                    OperatingCenterId = (entity.OperatingCenter != null) ? entity.OperatingCenter.Id : 0,
                    Module = ROLE,
                    Purpose = NOTIFICATION_PURPOSE,
                    Data = entity
                };
                notifier.Notify(args);
            }
        }

        private void SendMarkAsSubmittedsMostBodactionNotification(TrafficControlTicket entity)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs {
                OperatingCenterId = (entity.OperatingCenter != null) ? entity.OperatingCenter.Id : 0,
                Module = ROLE,
                Purpose = MARKED_AS_SUBMITTED_PURPOSE,
                Data = entity,
                Subject = String.Format(MARKED_AS_SUBMITTED_SUBJECT, entity.SAPWorkOrderNumber)
            };
            notifier.Notify(args);
        }

        #endregion
        
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddDropDownData<BillingParty>();
                    goto case ControllerAction.Search;
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<TrafficControlTicketStatus>("Status");
                    this.AddDropDownData<BillingParty>();
                    break;
            }
        }
        
        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchTrafficControlTicket>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true 
                }));
                x.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchTrafficControlTicket search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? workOrderId = null)
        {
            return ActionHelper.DoNew(new CreateTrafficControlTicket(_container, workOrderId));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateTrafficControlTicket model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (model.PaidByNJAW != true)
                    {
                        SendCreationsMostBodaciousNotification(Repository.Find(model.Id));
                    }
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTrafficControlTicket>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditTrafficControlTicket model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (!model.InvoiceAlreadyValid && !model.PaidByNJAW == true)
                    {
                        SendUpdatesMostBodaciousNotification(Repository.Find(model.Id));
                    }
                    return null;
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

        #region Constructors

        public TrafficControlTicketController(
            ControllerBaseWithPersistenceArguments<ITrafficControlTicketRepository, TrafficControlTicket, User> args)
            : base(args) {}

        #endregion

        #region Payment

        /// <summary>
        /// We want to use the Id here. We include the original invoice number in the description.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override string GetInvoiceNumber(TrafficControlTicket entity)
        {
            return entity.AuthorizeNetInvoiceNumber;

        }

        protected override string GetDescription(TrafficControlTicket entity)
        {
            return String.Format("Original Invoice Number: {0}, Accounting Code: {1}", entity.InvoiceNumber, entity.AccountingCode);
        }

        protected override string GetPONumber(TrafficControlTicket entity)
        {
            if (entity.WorkOrder != null)
            {
                return entity.WorkOrder.Id.ToString();
            }
            return base.GetPONumber(entity);
        }

        #region Step 1 - Choose Payment Method

        //TODO Bug 2642 - Test
        [HttpPost, RequiresSecureForm(false), RequiresRole(ROLE, RoleActions.Edit), UserMustHaveProfile]
        public ActionResult ChoosePaymentMethod(int id)
        {
            if (!Repository.Exists(id))
            {
                return HttpNotFound(String.Format(NOT_FOUND, id));
            }

            if (CurrentCustomer.PaymentProfiles.Count == 1)
            {
                TempData["ticket"] = new ChoosePaymentMethod {
                    Id = id,
                    SelectedProfileId = CurrentCustomer.PaymentProfiles[0].ProfileID
                };
                return RedirectToAction("VerifyPaymentSummary");
            }

            return PartialView("_ChoosePaymentMethod", new ChoosePaymentMethod {
                Id = id,
                PaymentProfiles = CurrentCustomer.PaymentProfiles
            });
        }

        #endregion

        #region Step 2 - Verify Payment

        //TODO Bug 2642 - Test
        [HttpGet, RequiresRole(ROLE, RoleActions.Edit), UserMustHaveProfile]
        public ActionResult VerifyPaymentSummary()
        {
            return VerifyPaymentSummary((ChoosePaymentMethod)TempData["ticket"]);
        }

        ////TODO Bug 2642 - Test
        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), UserMustHaveProfile]
        public ActionResult VerifyPaymentSummary(ChoosePaymentMethod ticket)
        {
            var entity = Repository.Find(ticket.Id);
            entity.CalculateFees();
            Repository.Save(entity);
            return VerifyEntityAndProfile(
                ticket.Id,
                ticket.SelectedProfileId,
                () => PartialView("_VerifyPaymentSummary", new VerifyPaymentSummary<TrafficControlTicket> {
                    Entity = entity,
                    SelectedPaymentProfile =
                        CurrentCustomer.PaymentProfiles.First(p => p.ProfileID == ticket.SelectedProfileId)
                }));
        }

        #endregion

        #region Step 3 Process Payment

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), UserMustHaveProfile]
        public ActionResult ProcessPayment(VerifyPaymentSummary<TrafficControlTicket> ticket)
        {
            return VerifyEntityAndProfile(
                ticket.Id, ticket.SelectedPaymentProfileId,
                () => DoProcessPayment(ticket));
        }
        
        #endregion

        #region Step 4 Successful Payment

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit), UserMustHaveProfile]
        public ActionResult SuccessfulPayment(int id)
        {
            var model = Repository.Find(id);
            return PartialView("_SuccessfulPayment", new Payment<TrafficControlTicket> { Entity = model });
        }

        #endregion

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator), RequiresSecureForm]
        public ActionResult MarkAsSubmitted(int id)
        {
            var ticket = Repository.Find(id);

            if (ticket == null)
                return HttpNotFound(String.Format(NOT_FOUND, id));
            if (ticket.IsCanceled)
                return HttpNotFound(CANCELED);
            if (ticket.SubmittedAt.HasValue)
                return HttpNotFound(ALREADY_SUBMITTED);
            if (!ticket.HasChecksForCorrectAmount)
                return HttpNotFound(CHECKS_INCORRECT);
            if (String.IsNullOrWhiteSpace(ticket.TrackingNumber))
                return HttpNotFound(NO_TRACKING_NUMBER);

            ticket.SubmittedAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            ticket.SubmittedBy = AuthenticationService.CurrentUser;
            Repository.Save(ticket);

            SendMarkAsSubmittedsMostBodactionNotification(ticket);

            return RedirectToAction("Index", "TrafficControlTicket",
                new {status = TrafficControlTicketStatus.Indices.PENDING_SUBMITTAL});
        }

        #endregion
    }
}