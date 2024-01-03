using System;
using System.Web.Mvc;
using AuthorizeNet;
using AuthorizeNet.APICore;
using AuthorizeNet.Utility.NotProvided;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class TrafficControlTicketControllerTest : MapCallMvcControllerTestBase<TrafficControlTicketController, TrafficControlTicket>
    {
        #region Private Members

        private Mock<INotificationService> _notifier;
        private Mock<IExtendedCustomerGateway> _gateway;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<IUserWithProfile>> _authServ;
        private User _user;
        private MerchantTotalFee _currentFee;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _gateway = new Mock<IExtendedCustomerGateway>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authServ = new Mock<IAuthenticationService<IUserWithProfile>>();
            _user = GetFactory<UserFactory>().Create(new { ProfileLastVerified = DateTime.Now.AddDays(-1) });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _container.Inject(_notifier.Object);
            _container.Inject(_gateway.Object);
            _container.Inject(_dateTimeProvider.Object);
            _container.Inject(_authServ.Object);

            // Needs to exist for Create tests
            _currentFee = GetEntityFactory<MerchantTotalFee>().Create(new { Fee = 0.021m, IsCurrent = true });
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = TrafficControlTicketController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/TrafficControlTicket/Search/", role);
                a.RequiresRole("~/FieldOperations/TrafficControlTicket/Show/", role);
                a.RequiresRole("~/FieldOperations/TrafficControlTicket/Index/", role);
                a.RequiresRole("~/FieldOperations/TrafficControlTicket/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/TrafficControlTicket/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/TrafficControlTicket/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/TrafficControlTicket/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/TrafficControlTicket/Destroy/", role, RoleActions.Delete);
                a.RequiresUserWithProfileAndRole("~/FieldOperations/TrafficControlTicket/ChoosePaymentMethod", role, RoleActions.Edit);
                a.RequiresUserWithProfileAndRole("~/FieldOperations/TrafficControlTicket/VerifyPaymentSummary", role, RoleActions.Edit);
                a.RequiresUserWithProfileAndRole("~/FieldOperations/TrafficControlTicket/ProcessPayment", role, RoleActions.Edit);
                a.RequiresUserWithProfileAndRole("~/FieldOperations/TrafficControlTicket/SuccessfulPayment", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/TrafficControlTicket/MarkAsSubmitted/", role, RoleActions.UserAdministrator);
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var entity = GetEntityFactory<TrafficControlTicket>().Create();
            InitializeControllerAndRequest("~/FieldOperations/TrafficControlTicket/Show" + entity.Id + ".frag");

            var result = (PartialViewResult)_target.Show(entity.Id);

            MvcAssert.IsViewNamed(result, "_ShowPopup");
            Assert.AreSame(entity, result.Model);
        }

        [TestMethod]
        public void TestShowResponseToPdf()
        {
            var ticket = GetEntityFactory<TrafficControlTicket>().Create();
            InitializeControllerAndRequest("~/FieldOperations/TrafficControlTicket/Show" + ticket.Id + ".pdf");

            var result = _target.Show(ticket.Id);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }

        [TestMethod]
        public void TestShowPdfErrorsWhenItDoesNotExist()
        {
            InitializeControllerAndRequest("~/FieldOperations/TrafficControlTicket/Show/666.pdf");

            var result = _target.Show(666) as HttpNotFoundResult;

            Assert.IsNotNull(result);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<TrafficControlTicket>().Create(new { SAPWorkOrderNumber = 0 });
            var entity1 = GetEntityFactory<TrafficControlTicket>().Create(new { SAPWorkOrderNumber = 0 });
            var search = new SearchTrafficControlTicket();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.SAPWorkOrderNumber, "SAPWorkOrderNumber");
                helper.AreEqual(entity1.SAPWorkOrderNumber, "SAPWorkOrderNumber", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<TrafficControlTicket>().Create();
            var expected = 0;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTrafficControlTicket, TrafficControlTicket>(eq, new
            {
                SAPWorkOrderNumber = expected
            }));

            Assert.AreEqual(expected, Session.Get<TrafficControlTicket>(eq.Id).SAPWorkOrderNumber);
        }

        #endregion

        #region Notifications

        [TestMethod]
        public void TestCreateSendsNotificationIfNotPaidForByNJAW()
        {
            //var fee1 = GetEntityFactory<MerchantTotalFee>().Create(new { Fee = 0.021m, IsCurrent = true });
            var ent = GetEntityFactory<TrafficControlTicket>().Create(new { MerchantTotalFee = _currentFee });
            var model = _viewModelFactory.Build<CreateTrafficControlTicket, TrafficControlTicket>(ent);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(TrafficControlTicketController.ROLE, resultArgs.Module);
            Assert.AreEqual(TrafficControlTicketController.NOTIFICATION_PURPOSE_NEW, resultArgs.Purpose);
        }

        [TestMethod]
        public void TestCreateDoesNotSendNotificationIfPaidForByNJAW()
        {
            // var fee1 = GetEntityFactory<MerchantTotalFee>().Create(new { Fee = 0.021m, IsCurrent = true });
            var ent = GetEntityFactory<TrafficControlTicket>().Create(new { MerchantTotalFee = _currentFee });
            var model = _viewModelFactory.BuildWithOverrides<CreateTrafficControlTicket, TrafficControlTicket>(ent, new { PaidByNJAW = true });

            NotifierArgs resultArgs = null;

            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never());

        }

        [TestMethod]
        public void TestUpdateNotificationIsNotSentIfConditionsHaveNotBeenMet()
        {
            var ent = GetEntityFactory<TrafficControlTicket>().Create();
            ent.BillingParty = GetEntityFactory<BillingParty>().Create();
            var model = _viewModelFactory.Build<EditTrafficControlTicket, TrafficControlTicket>(ent);

            ValidationAssert.ModelStateIsValid(model);
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Update(model);
            var entity = Repository.Find(model.Id);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
            Assert.IsFalse(entity.InvoiceValid);
            Assert.IsNull(resultArgs);
        }

        [TestMethod]
        public void TestUpdateDoesNotNotifyIfAlreadyUpdated()
        {
            var billingParty = GetEntityFactory<BillingParty>().Create(new { EstimatedHourlyRate = 125m });
            var ent = GetEntityFactory<TrafficControlTicket>().Create(new
            {
                BillingParty = billingParty,
                InvoiceAmount = 150m,
                InvoiceDate = DateTime.Now,
                InvoiceTotalHours = 1m,
                InvoiceNumber = "111-ABC"
            });
            var model = _viewModelFactory.Build<EditTrafficControlTicket, TrafficControlTicket>(ent);

            ValidationAssert.ModelStateIsValid(model);
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Update(model);
            var entity = Repository.Find(model.Id);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateDoesNotUpdateIfPaidByAmericanWater()
        {
            var billingParty = GetEntityFactory<BillingParty>().Create(new { EstimatedHourlyRate = 125m });
            var ent = GetEntityFactory<TrafficControlTicket>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditTrafficControlTicket, TrafficControlTicket>(ent, new
            {
                BillingParty = billingParty.Id,
                InvoiceAmount = 150m,
                InvoiceDate = DateTime.Now,
                InvoiceTotalHours = 1m,
                InvoiceNumber = "111-ABC",
                PaidByNJAW = true
            });

            var result = _target.Update(model);
            var entity = Repository.Find(model.Id);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateNotificationSentWhenTheyShouldBeSent()
        {
            var billingParty = GetEntityFactory<BillingParty>().Create(new { EstimatedHourlyRate = 125m });
            var ent = GetEntityFactory<TrafficControlTicket>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditTrafficControlTicket, TrafficControlTicket>(ent, new
            {
                BillingParty = billingParty.Id,
                InvoiceAmount = 150m,
                InvoiceDate = DateTime.Now,
                InvoiceTotalHours = 1m,
                InvoiceNumber = "111-ABC"
            });

            ValidationAssert.ModelStateIsValid(model);
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Update(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(TrafficControlTicketController.ROLE, resultArgs.Module);
            Assert.AreEqual(TrafficControlTicketController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
        }

        #endregion

        #region Processing

        #region Choose Payment

        [TestMethod]
        public void TestChoosePaymentIsHttpPostOnlyAndRequiresUserToHaveProfile()
        {
            MyAssert.MethodHasAttribute<HttpPostAttribute>(_target, "ChoosePaymentMethod", typeof(int))
                .MethodHasAttribute<UserMustHaveProfileAttribute>();
        }

        [TestMethod]
        public void TestChoosePaymentThrowsNotFoundIfNotFound()
        {
            Assert.IsNotNull(_target.ChoosePaymentMethod(666) as HttpNotFoundResult);
        }

        [TestMethod]
        public void TestChoosePaymentRedirectsToVerifyPaymentSummaryIfASingleProfileExsits()
        {
            var profileId = 555;
            var customer = new AuthorizeNet.Customer();
            customer.PaymentProfiles.Add(new PaymentProfile(new customerPaymentProfileMaskedType()) { ProfileID = "1231231" });
            var user = GetEntityFactory<User>().Create(new { CustomerProfileId = profileId });
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);
            _gateway.Setup(x => x.GetCustomer(profileId.ToString())).Returns(customer);
            var ticket = GetEntityFactory<TrafficControlTicket>().Create();

            var result = _target.ChoosePaymentMethod(ticket.Id) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("TrafficControlTicket", result.RouteValues["controller"]);
            Assert.AreEqual("VerifyPaymentSummary", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestChoosePaymentRedirectsToChoosePaymentIfUserHasMoultipleProfiles()
        {
            var profileId = 555;
            var customer = new AuthorizeNet.Customer();
            customer.PaymentProfiles.Add(new PaymentProfile(new customerPaymentProfileMaskedType()) { ProfileID = "1231231" });
            customer.PaymentProfiles.Add(new PaymentProfile(new customerPaymentProfileMaskedType()) { ProfileID = "2131231" });
            var user = GetEntityFactory<User>().Create(new { CustomerProfileId = profileId });
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);
            _gateway.Setup(x => x.GetCustomer(profileId.ToString())).Returns(customer);
            var ticket = GetEntityFactory<TrafficControlTicket>().Create();

            var result = _target.ChoosePaymentMethod(ticket.Id) as PartialViewResult;
            var resultModel = (ChoosePaymentMethod)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual(ticket.Id, resultModel.Id);
            Assert.AreEqual(customer.PaymentProfiles, resultModel.PaymentProfiles);
        }

        #endregion

        #region VerifyPayment

        [TestMethod]
        public void TestVerifyPaymentSummaryPassesAlongTempDataToVerifyPaymentSummary()
        {
            var profileId = 555;
            var customer = new AuthorizeNet.Customer();
            var selectedProfile = "1231231";
            customer.PaymentProfiles.Add(new PaymentProfile(new customerPaymentProfileMaskedType()) { ProfileID = selectedProfile });
            customer.PaymentProfiles.Add(new PaymentProfile(new customerPaymentProfileMaskedType()) { ProfileID = "1231232" });
            var fee = GetEntityFactory<MerchantTotalFee>().Create(new { Fee = 0.36m, IsCurrent = true });
            var ticket = GetEntityFactory<TrafficControlTicket>().Create(new { InvoiceAmount = 150m, MerchantTotalFee = fee });

            var user = GetEntityFactory<User>().Create(new { CustomerProfileId = profileId });
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);
            _gateway.Setup(x => x.GetCustomer(profileId.ToString())).Returns(customer);

            _target.TempData["ticket"] = new ChoosePaymentMethod { Id = ticket.Id, SelectedProfileId = selectedProfile };

            var result = _target.VerifyPaymentSummary() as PartialViewResult;
            var model = (VerifyPaymentSummary<TrafficControlTicket>)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual(ticket, model.Entity);
            Assert.AreEqual(customer.PaymentProfiles[0], model.SelectedPaymentProfile);
        }

        #endregion

        #region Process Payment

        [TestMethod]
        public void TestProcessPaymentVerifiesEntityProfileAndProcessesPayment()
        {
            var profileId = 555;
            var customer = new AuthorizeNet.Customer();
            var selectedProfile = "1231231";
            customer.PaymentProfiles.Add(new PaymentProfile(new customerPaymentProfileMaskedType()) { ProfileID = selectedProfile });
            var workOrder = GetEntityFactory<WorkOrder>().Create();
            var ticket = GetEntityFactory<TrafficControlTicket>().Create(new { InvoiceAmount = 150m, TotalCharged = 162m, ProcessingFee = 12m, WorkOrder = workOrder });
            var user = GetEntityFactory<User>().Create(new { CustomerProfileId = profileId });
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);
            _gateway.Setup(x => x.GetCustomer(profileId.ToString())).Returns(customer);

            Order order = null;
            var response = new Mock<IGatewayResponse>();
            response.Setup(x => x.Approved).Returns(true);
            _gateway.Setup(x => x.AuthorizeAndCapture(It.IsAny<Order>())).Returns(response.Object).Callback((Order o) =>
            {
                order = o;
            });

            var model = new VerifyPaymentSummary<TrafficControlTicket>
            {
                Id = ticket.Id,
                SelectedPaymentProfileId = selectedProfile
            };

            var result = _target.ProcessPayment(model) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("SuccessfulPayment", result.RouteValues["action"]);
            Assert.AreEqual(workOrder.Id.ToString(), order.PONumber);
        }

        [TestMethod]
        public void TestProcessPaymentDoesNotSetPONumberIfThereIsNotAWorkOrder()
        {
            var profileId = 555;
            var customer = new AuthorizeNet.Customer();
            var selectedProfile = "1231231";
            customer.PaymentProfiles.Add(new PaymentProfile(new customerPaymentProfileMaskedType()) { ProfileID = selectedProfile });
            var ticket = GetEntityFactory<TrafficControlTicket>().Create(new { InvoiceAmount = 150m, TotalCharged = 162m, ProcessingFee = 12m });
            Assert.IsNull(ticket.WorkOrder, "Sanity");
            var user = GetEntityFactory<User>().Create(new { CustomerProfileId = profileId });
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);
            _gateway.Setup(x => x.GetCustomer(profileId.ToString())).Returns(customer);

            Order order = null;
            var response = new Mock<IGatewayResponse>();
            response.Setup(x => x.Approved).Returns(true);
            _gateway.Setup(x => x.AuthorizeAndCapture(It.IsAny<Order>())).Returns(response.Object).Callback((Order o) =>
            {
                order = o;
            });

            var model = new VerifyPaymentSummary<TrafficControlTicket>
            {
                Id = ticket.Id,
                SelectedPaymentProfileId = selectedProfile
            };

            var result = _target.ProcessPayment(model) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("SuccessfulPayment", result.RouteValues["action"]);
            Assert.IsNull(order.PONumber);
        }

        [TestMethod]
        public void TestProcessPaymentVerifiesEntityProfileAndReturnsRejectedPaymentIfNotApproved()
        {
            var profileId = 555;
            var customer = new AuthorizeNet.Customer();
            var selectedProfile = "1231231";
            customer.PaymentProfiles.Add(new PaymentProfile(new customerPaymentProfileMaskedType()) { ProfileID = selectedProfile });
            var ticket = GetEntityFactory<TrafficControlTicket>().Create(new { InvoiceAmount = 150m, TotalCharged = 162m, ProcessingFee = 12m });
            var user = GetEntityFactory<User>().Create(new { CustomerProfileId = profileId });
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);
            _gateway.Setup(x => x.GetCustomer(profileId.ToString())).Returns(customer);

            var response = new Mock<IGatewayResponse>();
            response.Setup(x => x.Approved).Returns(false);
            _gateway.Setup(x => x.AuthorizeAndCapture(It.IsAny<Order>())).Returns(response.Object);

            var model = new VerifyPaymentSummary<TrafficControlTicket>
            {
                Id = ticket.Id,
                SelectedPaymentProfileId = selectedProfile
            };

            var result = _target.ProcessPayment(model) as PartialViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("~/Views/Authorize/_RejectedPayment.cshtml", result.ViewName);
        }

        #endregion

        #region Successful Payment

        [TestMethod]
        public void TestSuccessfulPaymentReturnsViewWithModel()
        {
            var ticket = GetEntityFactory<TrafficControlTicket>().Create(new { InvoiceAmount = 150m });

            var result = _target.SuccessfulPayment(ticket.Id) as PartialViewResult;
            var resultModel = (Payment<TrafficControlTicket>)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual("_SuccessfulPayment", result.ViewName);
            Assert.AreEqual(ticket.Id, resultModel.Entity.Id);
        }

        #endregion

        #region MarkAsSubmitted

        [TestMethod]
        public void TestMarkAsSubmittedThrowsNotFoundWhenNotFound()
        {
            var result = _target.MarkAsSubmitted(666) as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(String.Format(TrafficControlTicketController.NOT_FOUND, 666), result.StatusDescription);
        }

        [TestMethod]
        public void TestMarkAsSubmittedThrowsNotFoundForCanceledWhenCanceled()
        {
            var ticket = GetEntityFactory<TrafficControlTicket>().Create(new { CanceledAt = DateTime.Now });
            var result = _target.MarkAsSubmitted(ticket.Id) as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(TrafficControlTicketController.CANCELED, result.StatusDescription);
        }

        [TestMethod]
        public void TestMarkAsSubmittedThrowsNotFoundAlreadySubmittedWhenAlreadySubmitted()
        {
            var ticket = GetEntityFactory<TrafficControlTicket>().Create(new { SubmittedAt = DateTime.Now });
            var result = _target.MarkAsSubmitted(ticket.Id) as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(TrafficControlTicketController.ALREADY_SUBMITTED, result.StatusDescription);
        }

        [TestMethod]
        public void TestMarkAsSubmittedThrowsNotFoundChecksIncorrectWhenChecksIncorrect()
        {
            var ticket = GetEntityFactory<TrafficControlTicket>().Create(new { TotalCharged = 162m, ProcessingFee = 12m, InvoiceAmount = 150m });
            var check = GetEntityFactory<TrafficControlTicketCheck>().Create(new { TrafficControlTicket = ticket, Amount = 151m, CheckNumber = 4 });
            var result = _target.MarkAsSubmitted(ticket.Id) as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(TrafficControlTicketController.CHECKS_INCORRECT, result.StatusDescription);
        }

        [TestMethod]
        public void TestMarkAsSubmittedThrowsNotFoundTrackingNumberMissingWhenMissing()
        {
            var fee = new MerchantTotalFee { Fee = 0.36m };
            var mtotFee = (12m + 150m) * fee.Fee;
            var ticket = GetEntityFactory<TrafficControlTicket>().Create(new { TotalCharged = 162m + mtotFee, MTOTFee = mtotFee, ProcessingFee = 12m, InvoiceAmount = 150m, MerchantTotalFee = fee });
            var check = GetEntityFactory<TrafficControlTicketCheck>().Create(new { TrafficControlTicket = ticket, Amount = 150m, CheckNumber = 4 });
            Session.Clear();
            var result = _target.MarkAsSubmitted(ticket.Id) as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(TrafficControlTicketController.NO_TRACKING_NUMBER, result.StatusDescription);
        }

        [TestMethod]
        public void TestMarkAsSubmittedMarksAsSubmittedAndRedirectsToIndexOfPendingSubmittalsOhAndAlsoSendsNotification()
        {
            var now = DateTime.Now;
            var fee = new MerchantTotalFee { Fee = 0.36m };
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var mtotFee = (12m + 150m) * fee.Fee;
            var ticket = GetEntityFactory<TrafficControlTicket>().Create(new { TotalCharged = 162m + mtotFee, MTOTFee = mtotFee, ProcessingFee = 12m, InvoiceAmount = 150m, TrackingNumber = "1234411", SAPWorkOrderNumber = 12322, MerchantTotalFee = fee });
            var check = GetEntityFactory<TrafficControlTicketCheck>().Create(new { TrafficControlTicket = ticket, Amount = 150m, CheckNumber = 4 });
            Session.Clear();
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.MarkAsSubmitted(ticket.Id) as RedirectToRouteResult;

            ticket = Repository.Find(ticket.Id);

            Assert.AreEqual(now, ticket.SubmittedAt);
            Assert.AreEqual(_currentUser, ticket.SubmittedBy);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("TrafficControlTicket", result.RouteValues["Controller"]);
            Assert.AreEqual(TrafficControlTicketStatus.Indices.PENDING_SUBMITTAL, result.RouteValues["status"]);

            //notification
            Assert.AreSame(ticket, resultArgs.Data);
            Assert.AreEqual(TrafficControlTicketController.ROLE, resultArgs.Module);
            Assert.AreEqual(TrafficControlTicketController.MARKED_AS_SUBMITTED_PURPOSE, resultArgs.Purpose);
            Assert.AreEqual(String.Format(TrafficControlTicketController.MARKED_AS_SUBMITTED_SUBJECT, ticket.SAPWorkOrderNumber), resultArgs.Subject);
        }

        #endregion

        #endregion
    }
}
