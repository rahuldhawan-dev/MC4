using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Testing;
using Moq;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SapNotificationControllerTest : MapCallMvcControllerTestBase<SapNotificationController, WorkOrder>
    {
        #region Private Members

        private Mock<ISAPNotificationRepository> _sapNotificationRepository;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _sapNotificationRepository = new Mock<ISAPNotificationRepository>();

            _container.Inject(_sapNotificationRepository.Object);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = SapNotificationController.ROLE;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/SapNotification/Search", role);
                a.RequiresRole("~/FieldOperations/SapNotification/Index", role);
                a.RequiresRole("~/FieldOperations/SapNotification/Update", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/SapNotification/Show", role);
            });
        }

        #region Tests

        #region Index/Search

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // overridden because not ISearchSet
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because not ISearchSet

            // params don't matter, not testing that atm.
            var search = new SearchSapNotification
            {
                PlanningPlant = new[] { "D201" },
                DateCreatedTo = DateTime.Now,
                DateCreatedFrom = DateTime.Now,
                NotificationType = new[] { "20" }
            };
            var results = new SAPNotificationCollection();
            results.Items.Add(new SAPNotification { SAPNotificationNumber = "123321", SAPErrorCode = "Success Not an error" });
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<MapCall.Common.Model.ViewModels.SearchSapNotification>())).Returns(results);

            var result = _target.Index(search) as ViewResult;
            var resultModel = (IEnumerable<SAPNotification>)result.Model;

            MvcAssert.IsViewNamed(result, "Index");
            Assert.IsNotNull(resultModel);
            Assert.AreEqual(1, resultModel.ToList().Count);
            Assert.AreEqual(results.Items[0].SAPNotificationNumber, resultModel.First().SAPNotificationNumber);
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            // override because not ISearchSet
            var search = new SearchSapNotification();
            _target.ModelState.AddModelError("Uh ohs", "Uh ohs");

            var result = _target.Index(search) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Search", result.RouteValues["action"]);
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public void TestIndexRedirectsBackToSearchIfOneResultWithCrappySapErrorMessage()
        {
            var search = new SearchSapNotification {
                PlanningPlant = new[] { "D201" },
                DateCreatedTo = DateTime.Now,
                DateCreatedFrom = DateTime.Now,
                NotificationType = new[] { "20" }
            };
            var results = new SAPNotificationCollection();
            var sapNotification = new SAPNotification {SAPNotificationNumber = "123321", SAPErrorCode = "An Error"};
            results.Items.Add(sapNotification);
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<MapCall.Common.Model.ViewModels.SearchSapNotification>())).Returns(results);

            var result = _target.Index(search) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Search", result.RouteValues["action"]);
            _target.AssertTempDataContainsMessage(sapNotification.SAPErrorCode, SapNotificationController.ERROR_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestIndexRedirectsToSearchWithConcatenatedErrorMessage()
        {
            var tempDataErrorList = new List<string>();
            _target.TempData[SapNotificationController.ERROR_MESSAGE_KEY] = tempDataErrorList;

            var otherError = "Other error";
            tempDataErrorList.Add(otherError);

            var search = new SearchSapNotification
            {
                PlanningPlant = new[] { "D201" },
                DateCreatedTo = DateTime.Now,
                DateCreatedFrom = DateTime.Now,
                NotificationType = new[] { "20" }
            };
            var results = new SAPNotificationCollection();
            var sapNotification = new SAPNotification { SAPNotificationNumber = "123321", SAPErrorCode = "An Error" };
            results.Items.Add(sapNotification);
            _sapNotificationRepository.Setup(x => x.Search(It.IsAny<MapCall.Common.Model.ViewModels.SearchSapNotification>())).Returns(results);

            var result = _target.Index(search) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Search", result.RouteValues["action"]);

            Assert.IsTrue(tempDataErrorList.Contains(otherError));
            Assert.IsTrue(tempDataErrorList.Contains(sapNotification.SAPErrorCode));
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            // override because of lack of ViewModel<T>
            // noop because it redirects to Search.
        }

        [TestMethod]
        public override void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            Assert.Inconclusive("I don't do validation checks");
        }

        [TestMethod]
        public override void TestUpdateReturnsNotFoundIfRecordBeingUpdatedDoesNotExist()
        {
            Assert.Inconclusive("Am I supposed to return a 404? Becaue I don't");
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            // noop: Handled by other tests
        }

        [TestMethod]
        public void TestUpdateCallsSapRepoSaveAndSavesSuccessfullyWhenCancelled()
        {
            var model = new EditSapNotification(_container) { Cancel = "Yes", Complete = "", SAPNotificationNumber = "123", Remarks = "123211" };
            var notification = new SAPNotificationStatus() { SAPMessage = SapNotificationController.SUCCESS_MESSAGE_CANCELLED};
            _sapNotificationRepository.Setup(x => x.Save(It.Is<SAPNotificationStatus>(args => 
                args.Cancel == model.Cancel && 
                args.Complete == model.Complete && 
                args.Remarks == model.Remarks && 
                args.SAPNotificationNo == model.SAPNotificationNumber
            ))).Returns(notification);
            
            var result = _target.Update(model) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            _target.AssertTempDataContainsMessage(notification.SAPMessage, ControllerBase.SUCCESS_MESSAGE_KEY);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod]
        public void TestUpdateCallsSapRepoSaveAndSavesSuccessfullyWhenCompleted()
        {
            var model = new EditSapNotification(_container) { Cancel = "", Complete = "Yes", SAPNotificationNumber = "123", Remarks = "123211" };
            var notification = new SAPNotificationStatus { SAPMessage = SapNotificationController.SUCCESS_MESSAGE_COMPLETED };
            _sapNotificationRepository.Setup(x => x.Save(It.Is<SAPNotificationStatus>(args =>
                args.Cancel == model.Cancel &&
                args.Complete == model.Complete &&
                args.Remarks == model.Remarks &&
                args.SAPNotificationNo == model.SAPNotificationNumber
            ))).Returns(notification);

            var result = _target.Update(model) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            _target.AssertTempDataContainsMessage(notification.SAPMessage, ControllerBase.SUCCESS_MESSAGE_KEY);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }
        
        [TestMethod]
        public void TestUpdateCallsSapRepoSaveAndErrorsErroneously()
        {
            var model = new EditSapNotification(_container) { Cancel = "", Complete = "Yes", SAPNotificationNumber = "123", Remarks = "123211" };
            var notification = new SAPNotificationStatus { SAPMessage = "Something bad has happened." };
            _sapNotificationRepository.Setup(x => x.Save(It.Is<SAPNotificationStatus>(args =>
                args.Cancel == model.Cancel &&
                args.Complete == model.Complete &&
                args.Remarks == model.Remarks &&
                args.SAPNotificationNo == model.SAPNotificationNumber
            ))).Returns(notification);

            var result = _target.Update(model) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            _target.AssertTempDataContainsMessage(notification.SAPMessage, ControllerBase.ERROR_MESSAGE_KEY);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        #endregion

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // override needed for SAP repo stuff
            var expected = "these are going to be the notification notes";
            var expectedappendedmessage = " Locality : 5811 LocalityDescription : West Hempstead";
            var model = new CreateSapNotificationWorkOrder(_container) { SAPNotificationNumber = "123" };
            var searchResult = new SAPNotification() { NotificationLongText = expected, SAPErrorCode = "Successful", Locality = "5811", LocalityDescription = "West Hempstead" };
            var searchResults = new SAPNotificationCollection();
            searchResults.Items.Add(searchResult);
            _sapNotificationRepository.Setup(x => x.SearchWorkOrder(
                            It.Is<SAPNotification>(args => args.CreateWorkOrderNotificationNumber == model.SAPNotificationNumber)))
                .Returns(searchResults);

            var result = _target.Show(model) as ViewResult;
            var resultModel = result.Model as CreateSapNotificationWorkOrder;
            
            Assert.IsNotNull(result);
            Assert.IsNotNull(resultModel);
            Assert.AreEqual(expected + expectedappendedmessage, _target.TempData[SapNotificationController.TEMP_DATA_CREATE_WORK_ORDER_NOTES]);
            MvcAssert.IsViewNamed(result, "Show");
        }

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            // noop: doesn't do this
        }

        #endregion

        #region SetLookupData

        [TestMethod]
        public void TestSetLookupDataForSearchSetsLookupDataProperly()
        {
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, SAPWorkOrdersEnabled = true });
            var opCntr2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false, SAPWorkOrdersEnabled = true });
            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = SapNotificationController.ROLE }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = opCntr,
                User = _currentUser
            });
            var roleThatShouldNotAppear = GetFactory<RoleFactory>().Create(new
            {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = SapNotificationController.ROLE }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                OperatingCenter = opCntr2,
                User = _currentUser
            });

            var sapNotificationTypes = GetEntityFactory<SAPNotificationType>().CreateList(4);
            var sapPriorities = GetEntityFactory<SAPWorkOrderPriority>().CreateList(2);
            var sapPurposes = GetEntityFactory<SAPWorkOrderPurpose>().CreateList(3);
            var sapPlanningPlants = GetEntityFactory<PlanningPlant>().CreateList(5, new { OperatingCenter = opCntr });
            var invalidSapPlanningPlants = GetEntityFactory<PlanningPlant>().CreateList(5, new { OperatingCenter = opCntr2 });

            _target.SetLookupData(ControllerAction.Search);
            var notificationTypes = (IEnumerable<SelectListItem>)_target.ViewData["NotificationType"];
            var priorities = (IEnumerable<SelectListItem>)_target.ViewData["Priority"];
            var codes = (IEnumerable<SelectListItem>)_target.ViewData["Code"];
            var planningPlants = (IEnumerable<SelectListItem>)_target.ViewData["PlanningPlant"];

            Assert.AreEqual(sapNotificationTypes.Count, notificationTypes.Count());
            Assert.AreEqual(sapPriorities.Count, priorities.Count());
            Assert.AreEqual(sapPurposes.Count, codes.Count());
            Assert.AreEqual(sapPlanningPlants.Count, planningPlants.Count());
        }

        #endregion

        #endregion
    }
}