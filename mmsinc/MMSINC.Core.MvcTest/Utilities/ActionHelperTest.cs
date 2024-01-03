using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Data;
using MMSINC.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using NHibernate.Exceptions;
using StructureMap;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MMSINC.Core.MvcTest.Utilities
{
    [TestClass]
    public class ActionHelperTest : InMemoryDatabaseTest<TestUser>
    {
        #region Private Members

        private ActionHelper<TestControllerWithPersistenceWithRepository, IRepository<TestUser>, TestUser, TestUser>
            _target;

        private TestControllerWithPersistenceWithRepository _controller;
        private IRepository<TestUser> _testUserRepository;
        private TestControllerWithViewPathFormat _controllerWithViewPathFormat;

        private ActionHelper<TestControllerWithViewPathFormat, IRepository<TestUser>, TestUser, TestUser>
            _viewPathTarget;

        #endregion

        #region Setup/TearDown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IViewModelFactory>().Use<ViewModelFactory>();
            e.For<IDisplayItemService>().Use<DisplayItemService>();
            e.For<IRepository<TestUser>>().Use<TestUserRepository>();
            e.For<IAuthenticationService<TestUser>>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeTarget();
        }

        private void InitializeForVirtualPathFormatTest()
        {
            _controllerWithViewPathFormat = _container.GetInstance<TestControllerWithViewPathFormat>();
            _viewPathTarget =
                new ActionHelper
                    <TestControllerWithViewPathFormat, IRepository<TestUser>, TestUser, TestUser>
                    (_container, _controllerWithViewPathFormat);
        }

        private void InitializeTarget()
        {
            _controller = _container.GetInstance<TestControllerWithPersistenceWithRepository>();
            _target =
                new ActionHelper<
                    TestControllerWithPersistenceWithRepository, IRepository<TestUser>, TestUser, TestUser>(
                    _container,
                    _controller);
        }

        #endregion

        #region Controller

        [TestMethod]
        public void TestControllerReturnsController()
        {
            Assert.AreSame(_controller, _target.Controller);
        }

        #endregion

        #region DoNew

        [TestMethod]
        public void TestDoNewCallsSetDefaultsOnViewModel()
        {
            var model = new TestUserViewModel(_container);

            Assert.IsFalse(model.SetDefaultsCalled);

            var result = (ViewResult)_target.DoNew(model);

            Assert.IsTrue(model.SetDefaultsCalled);
        }

        [TestMethod]
        public void TestDoNewCallsSetLookupDataAndReturnsNewView()
        {
            var model = new TestUserViewModel(_container);

            var result = (ViewResult)_target.DoNew(model);

            Assert.AreEqual("New", result.ViewName);
            Assert.AreEqual(model, result.Model);
            Assert.IsTrue(_controller.SetLookupDataCalled);
        }

        [TestMethod]
        public void TestDoNewReturnsViewWithFormattedPathIfRequired()
        {
            InitializeForVirtualPathFormatTest();
            var model = new TestUserViewModel(_container);
            MvcAssert.IsViewNamed(_viewPathTarget.DoNew(model), "~/Views/SomePath/New.cshtml");
        }

        [TestMethod]
        public void TestDoNewUsesViewNameArgWhenSet()
        {
            var model = new TestUserViewModel(_container);
            var args = new ActionHelperDoNewArgs();
            args.ViewName = "Blorgh";

            var result = (ViewResult)_target.DoNew(model, args);
            MvcAssert.IsViewNamed(result, "Blorgh");
        }

        [TestMethod]
        public void TestDoNewReturnsPartialResultWhenIsPartialIsTrue()
        {
            var model = new TestUserViewModel(_container);
            var args = new ActionHelperDoNewArgs();
            args.IsPartial = true;

            var result = _target.DoNew(model, args);
            MvcAssert.IsPartialView(result);
        }

        [TestMethod]
        public void TestDoNewReturnsPartialViewWithDefaultPartialViewNameWhenIsPartialIsTrueAndNoVieWNameIsSet()
        {
            var model = new TestUserViewModel(_container);
            var args = new ActionHelperDoNewArgs();
            args.IsPartial = true;

            var result = _target.DoNew(model, args);
            MvcAssert.IsViewNamed(result, "_New");
        }

        #endregion

        #region DoCreate

        [TestMethod]
        public void TestDoCreateCorrectRedirectActionReturnsSuccess()
        {
            var user = new TestUserFactory(_container).Build();
            var userViewModel = new TestUserViewModel(_container, user) {Id = 666};

            var result = _target.DoCreate(userViewModel);
            Assert.AreEqual("Show", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual(userViewModel.Id, ((RedirectToRouteResult)result).RouteValues["id"]);
            Assert.AreNotEqual(0, userViewModel.Id);
        }

        [TestMethod]
        public void TestDoCreateDisplaysModelStateErrorsAndReturnsNewViewIfInvalidModel()
        {
            var user = new TestUserFactory(_container).Build();
            var userViewModel = new TestUserViewModel(_container, user);
            _target.Controller.ModelState.AddModelError("foo", "bar");

            var result = _target.DoCreate(userViewModel);
            MvcAssert.IsViewNamed(result, "New");
        }

        [TestMethod]
        public void TestDoCreateUsesReturnsOnSuccessArgIfArgHasValueAndDoesNotReturnNull()
        {
            var user = new TestUserFactory(_container).Build();
            var userViewModel = new TestUserViewModel(_container, user) {Id = 666};
            var expected = new EmptyResult();

            var result = _target.DoCreate(userViewModel, new ActionHelperDoCreateArgs {
                OnSuccess = () => expected
            });

            Assert.AreSame(expected, result);

            result = _target.DoCreate(userViewModel, new ActionHelperDoCreateArgs {
                OnSuccess = () => null
            });

            Assert.AreNotSame(expected, result);
            Assert.IsNotNull(result,
                "The OnSuccess arg returning null means ActionHelper should return the default success ActionResult.");
        }

        [TestMethod]
        public void TestDoCreateSavesNewEntity()
        {
            var user = new TestUserFactory(_container).Build();
            var userViewModel = new TestUserViewModel(_container, user) {Email = "some@email.com"};

            _target.DoCreate(userViewModel);

            var result = Session.Query<TestUser>().Single(x => x.Email == "some@email.com");
            Assert.AreEqual("some@email.com", result.Email);
        }

        [TestMethod]
        public void TestDoCreateUsesTheEntityArgToSaveWhenItIsNotNull()
        {
            var user = new TestUserFactory(_container).Build();
            var userViewModel = new TestUserViewModel(_container, user) {Email = "some@email.com"};
            var expected = new TestUser();

            _target.DoCreate(userViewModel, new ActionHelperDoCreateArgs {
                Entity = expected
            });

            Assert.AreEqual("some@email.com", expected.Email,
                "The supplied entity object should have been used for creation.");
            Assert.AreNotEqual(0, expected.Id, "Make sure this has an id now");
        }

        #endregion

        #region DoEdit

        [TestMethod]
        public void TestDoEditCallsMap()
        {
            var entity = new TestUserFactory(_container).Create();
            var result = (ViewResult)_target.DoEdit<TestUserViewModel>(entity.Id);
            var model = (TestUserViewModel)result.Model;

            Assert.IsTrue(model.MapCalled);
        }

        [TestMethod]
        public void TestDoEditCallsSetDefaultsOnViewModelBEFOREMapIsCalled()
        {
            var entity = new TestUserFactory(_container).Create();
            var result = (ViewResult)_target.DoEdit<TestUserViewModel>(entity.Id);
            var model = (TestUserViewModel)result.Model;

            Assert.IsTrue(model.SetDefaultsCalledBeforeMap);
        }

        [TestMethod]
        public void TestDoEditGetsEditModelReturnsEditView()
        {
            var entity = new TestUserFactory(_container).Create();

            var result = (ViewResult)_target.DoEdit<TestUserViewModel>(entity.Id);

            Assert.AreEqual("Edit", result.ViewName);
            Assert.IsInstanceOfType(result.Model, typeof(TestUserViewModel));
        }

        [TestMethod]
        public void TestDoEditGetsEditModelReturnsPartialEditViewWhenIsPartialIsTrue()
        {
            var entity = new TestUserFactory(_container).Create();

            var result = (ViewResultBase)_target.DoEdit(entity.Id,
                new ActionHelperDoEditArgs<TestUser, TestUserViewModel> {IsPartial = true});

            Assert.AreEqual("_Edit", result.ViewName);
            MvcAssert.IsPartialView(result);
            Assert.IsInstanceOfType(result.Model, typeof(TestUserViewModel));
        }

        [TestMethod]
        public void TestDoEditReturnsFormattedViewPathIfRequired()
        {
            var entity = new TestUserFactory(_container).Create();

            InitializeForVirtualPathFormatTest();
            MvcAssert.IsViewNamed(_viewPathTarget.DoEdit<TestUserViewModel>(entity.Id), "~/Views/SomePath/Edit.cshtml");
        }

        [TestMethod]
        public void TestDoEditReturnsNotFoundWhenNotFound()
        {
            var expected = "Not Found";

            var result = (HttpNotFoundResult)_target.DoEdit(808,
                new ActionHelperDoEditArgs<TestUser, TestUserViewModel> {NotFound = expected});

            Assert.AreEqual(expected, result.StatusDescription);
        }

        [TestMethod]
        public void TestDoEditReturnsValueReturnedFromGetEntityOverrideWhenSet()
        {
            var entity = new TestUserFactory(_container).Create();
            var expected = new TestUser();

            var result = (ViewResult)_target.DoEdit(entity.Id, new ActionHelperDoEditArgs<TestUser, TestUserViewModel> {
                GetEntityOverride = () => expected
            });

            Assert.AreEqual("Edit", result.ViewName);
            Assert.IsInstanceOfType(result.Model, typeof(TestUserViewModel));
        }

        [TestMethod]
        public void TestDoEditReturnsNotFoundIfGetEntityOverrideReturnsNull()
        {
            var entity = new TestUserFactory(_container).Create();

            var result = _target.DoEdit(entity.Id, new ActionHelperDoEditArgs<TestUser, TestUserViewModel> {
                GetEntityOverride = () => null,
                NotFound = "Some not found message"
            });

            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void TestDoEditDoesEditWhenInitializeViewModelIsSet()
        {
            var entity = new TestUserFactory(_container).Create(new {Email = "Stuff"});
            var model = new TestUserViewModel(_container, entity) {
                Email = "This is a test user emailed"
            };
            var result = (ViewResult)_target.DoEdit(entity.Id, new ActionHelperDoEditArgs<TestUser, TestUserViewModel> {
                InitializeViewModel = (x) => { x.Email = entity.Email; }
            });
            var editModel = (TestUserViewModel)result.Model;

            Assert.AreEqual("Edit", result.ViewName);
            Assert.IsInstanceOfType(result.Model, typeof(TestUserViewModel));
            Assert.AreEqual(entity.Email, editModel.Email);
            Assert.AreNotEqual(model.Email, editModel.Email);
        }

        [TestMethod]
        public void TestDoEditDoesEditNormallyWhenInitializeViewModelIsNull()
        {
            var entity = new TestUserFactory(_container).Create();
            var model = new TestUserViewModel(_container, entity) {
                Email = "This is a test user email"
            };
            var result = (ViewResult)_target.DoEdit(entity.Id, new ActionHelperDoEditArgs<TestUser, TestUserViewModel> {
                InitializeViewModel = null
            });
            var editModel = (TestUserViewModel)result.Model;

            Assert.AreEqual("Edit", result.ViewName);
            Assert.IsInstanceOfType(result.Model, typeof(TestUserViewModel));
            Assert.AreEqual(entity.Email, editModel.Email);
        }

        #endregion

        #region DoUpdate

        [TestMethod]
        public void TestDoUpdateRendersEditViewWithModelAndModelErrorsIfModelStateIsNotValid()
        {
            var user = new TestUserFactory(_container).Create();
            var userViewModel = new TestUserViewModel(_container, user);
            _target.Controller.ModelState.AddModelError("foo", "bar");

            var result = (ViewResult)_target.DoUpdate(userViewModel);
            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(userViewModel.Id, ((TestUserViewModel)result.Model).Id);
            Assert.IsTrue(((List<string>)_target.Controller.TempData[ControllerBase.ERROR_MESSAGE_KEY])
               .Contains("bar"));
        }

        [TestMethod]
        public void TestDoUpdateRendersFormattedEditViewPathIfRequired()
        {
            InitializeForVirtualPathFormatTest();
            var user = new TestUserFactory(_container).Create();
            var userViewModel = new TestUserViewModel(_container, user);
            _viewPathTarget.Controller.ModelState.AddModelError("foo", "bar");
            MvcAssert.IsViewNamed(_viewPathTarget.DoUpdate(userViewModel), "~/Views/SomePath/Edit.cshtml");
        }

        [TestMethod]
        public void TestDoUpdateReturnsHttpNotFoundWhenNotFound()
        {
            var expected = "not found";
            var user = new TestUser();
            var userViewModel = new TestUserViewModel(_container, user) {Id = 666};

            var result =
                (HttpNotFoundResult)_target.DoUpdate(userViewModel, new ActionHelperDoUpdateArgs {NotFound = expected});

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.StatusDescription);
        }

        [TestMethod]
        public void TestDoUpdateReturnsOnNotFoundResultFromArgs()
        {
            ActionResult expectedNotFoundResult = new ViewResult();
            Func<ActionResult> onNotFound = () => { return expectedNotFoundResult; };
            var userViewModel = new TestUserViewModel(_container) {Id = 666};

            var result = _target.DoUpdate(userViewModel, new ActionHelperDoUpdateArgs {OnNotFound = onNotFound});
            Assert.AreSame(expectedNotFoundResult, result);
        }

        [TestMethod]
        public void TestDoUpdateUsesReturnsOnSuccessArgIfArgHasValueAndDoesNotReturnNull()
        {
            var user = new TestUserFactory(_container).Build();
            var userViewModel = new TestUserViewModel(_container, user) {Id = 666};
            var expected = new EmptyResult();

            var result = _target.DoUpdate(userViewModel, new ActionHelperDoUpdateArgs {
                OnSuccess = () => expected,
                GetEntityOverride = () => user
            });

            Assert.AreSame(expected, result);

            result = _target.DoUpdate(userViewModel, new ActionHelperDoUpdateArgs {
                OnSuccess = () => null,
                GetEntityOverride = () => user
            });

            Assert.AreNotSame(expected, result);
            Assert.IsNotNull(result,
                "The OnSuccess arg returning null means ActionHelper should return the default success ActionResult.");
        }

        [TestMethod]
        public void TestDoUpdateUsesGetEntityOverrideWhenitIsSet()
        {
            var expectedEmail = "this@isanemail.com";
            var userOne = GetFactory<TestUserFactory>().Create(new {Email = "something@email.com"});
            var userTwo = GetFactory<TestUserFactory>().Create(new {Email = "another@email.com"});

            var model = new TestUserViewModelWithEmail(_container);
            model.Id = userOne.Id;
            model.Email = expectedEmail;

            // First assert that it uses the base method for finding the original entity and updates it.
            _target.DoUpdate(model);
            Assert.AreEqual(expectedEmail, userOne.Email);

            // Next assert that it updates whatever entity is returned by the GetEntityOverride.
            model.Id = 0; // This Id should not matter at all.
            _target.DoUpdate(model, new ActionHelperDoUpdateArgs {
                GetEntityOverride = () => userTwo
            });
            Assert.AreEqual(expectedEmail, userTwo.Email);
        }

        [TestMethod]
        public void TestDoUpdateReturnsNotFoundIfGetEntityOverrideFuncReturnsNull()
        {
            var result = _target.DoUpdate(new TestUserViewModel(_container), new ActionHelperDoUpdateArgs {
                NotFound = "Not Found",
                GetEntityOverride = () => null
            });
            MvcAssert.IsNotFound(result);
        }

        #endregion

        #region DoIndex

        [TestMethod]
        public void TestDoIndexReturnsDoRedirectToActionWhenCountZero()
        {
            var search = new TestSearchSet();

            var result = _target.DoIndex(search) as RedirectToRouteResult;

            MyAssert.IsInstanceOfType<RedirectToRouteResult>(result);

            var expected =
                ActionHelper<TestControllerWithPersistenceWithRepository, IRepository<TestUser>, TestUser, TestUser>
                   .NO_RESULTS;
            var resultError = ((List<string>)_controller.TempData[ControllerBase.ERROR_MESSAGE_KEY]).Single();
            Assert.AreEqual(expected, resultError);
        }

        [TestMethod]
        public void TestDoIndexReturnsRecordsAndShowsIndexWhenRedirectSingleItemToShowViewFalse()
        {
            var user = new TestUserFactory(_container).Create();
            var search = new TestSearchSet {Id = user.Id};

            var result = (ViewResult)_target.DoIndex(search,
                new ActionHelperDoIndexArgs {RedirectSingleItemToShowView = false});
            var resultModel = (TestSearchSet)result.Model;

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Results.Count());
        }

        [TestMethod]
        public void TestDoIndexReturnsSearchWithErrorsWhenModelStateErrorsExist()
        {
            var expected = "Expected Error String";
            var search = new TestSearchSet();
            _target.Controller.ModelState.AddModelError("Id", expected);

            var result = _target.DoIndex(search) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Search", result.RouteValues["Action"]);
            Assert.AreEqual(expected, ((List<string>)_controller.TempData[ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestDoIndexReturnsShowWhenCountOneAndRedirectSingleItemToShowViewIsTrue()
        {
            var user = new TestUserFactory(_container).Create();
            var search = new TestSearchSet {Id = user.Id};

            var result = _target.DoIndex(search, new ActionHelperDoIndexArgs {
                RedirectSingleItemToShowView = true
            }) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["Action"]);
            Assert.AreEqual(user.Id, result.RouteValues["id"]);
        }

        [TestMethod]
        public void TestDoIndexReturnsViewWithRecordsWhenCountGreaterThanOne()
        {
            var search = new TestSearchSet();
            var user = new TestUserFactory(_container).CreateList(2);

            var result = (ViewResult)_target.DoIndex(search);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestDoIndexReturnsOnSuccessArg()
        {
            var expectedResult = new EmptyResult();
            var search = new TestSearchSet();
            var user = new TestUserFactory(_container).CreateList(2);

            var result = _target.DoIndex(search, new ActionHelperDoIndexArgs {
                OnSuccess = () => expectedResult
            });

            Assert.AreSame(expectedResult, result);
        }

        [TestMethod]
        public void TestDoIndexWithSearchReturnsFormattedIndexViewPathIfRequired()
        {
            InitializeForVirtualPathFormatTest();
            var user = new TestUserFactory(_container).Create();
            var search = new TestSearchSet {Id = user.Id};

            var result = (ViewResult)_viewPathTarget.DoIndex(search);
            MvcAssert.IsViewWithNameAndModel(result, "~/Views/SomePath/Index.cshtml", search);
        }

        [TestMethod]
        public void TestDoIndexCallsSearchMethodActionInPlaceOfRepositorySearchCallIfActionIsNotNull()
        {
            var mockRepo = new Mock<IRepository<TestUser>>();
            mockRepo.Setup(x => x.Search(It.IsAny<TestSearchSet>()))
                    .Throws(new Exception("This should not be getting called."));
            _container.Inject(mockRepo.Object);
            InitializeTarget();

            var overrideSearchMethodCalled = false;
            var search = new TestSearchSet();
            var result = _target.DoIndex(search, new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => { overrideSearchMethodCalled = true; }
            });

            Assert.IsTrue(overrideSearchMethodCalled);
        }

        [TestMethod]
        public void TestDoIndexRedirectsBackToSearchWithErrorMessageIfResultsHigherThanMaxResults()
        {
            var max = 2;
            var search = new TestSearchSet();
            var users = new TestUserFactory(_container).CreateList(max + 1);

            var result =
                (RedirectToRouteResult)_target.DoIndex(search,
                    new ActionHelperDoIndexArgs {MaxResults = max});

            var resultError = ((List<string>)_controller.TempData[ControllerBase.ERROR_MESSAGE_KEY]).Single();
            Assert.AreEqual(
                $"The query you have entered will bring back more than {max} results. Please refine your search.",
                resultError);
        }

        #endregion

        #region DoExcel

        [TestMethod]
        public void TestDoExcelSetsEnablePagingToFalse()
        {
            var search = new TestSearchSet();
            search.EnablePaging = true;

            var result = (ExcelResult)_target.DoExcel(search);

            Assert.IsFalse(search.EnablePaging, "Paging is unnecessary for excel files.");
        }

        [TestMethod]
        public void TestDoExcelReturnsEmptyWorksheetWhenThereAreZeroResults()
        {
            var search = new TestSearchSet();

            var result = (ExcelResult)_target.DoExcel(search);
            using (var helper = new ExcelResultTester(_container, result, true))
            {
                Assert.AreEqual(0, helper.GetRowCount("Sheet1"));
            }
        }

        [TestMethod]
        public void TestDoExcelReturnsSearchWithErrorsWhenModelStateErrorsExist()
        {
            var expected = "Expected Error String";
            var search = new TestSearchSet();
            _target.Controller.ModelState.AddModelError("Id", expected);

            var result = _target.DoExcel(search) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Search", result.RouteValues["Action"]);
            Assert.AreEqual(expected, ((List<string>)_controller.TempData[ControllerBase.ERROR_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestDoExcelReturnsExcelResults()
        {
            var search = new TestSearchSet();
            var users = new TestUserFactory(_container).CreateList(2);

            var result = (ExcelResult)_target.DoExcel(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                Assert.AreEqual(2, helper.GetRowCount("Sheet1"));
                helper.AreEqual(users[0].UniqueName, "Foo",
                    0); // UniqueName has a display name attribute for "Foo" for whatever reason.
                helper.AreEqual(users[1].UniqueName, "Foo", 1);
            }
        }

        [TestMethod]
        public void TestDoExcelReturnsOnSuccessArg()
        {
            var expectedResult = new EmptyResult();
            var search = new TestSearchSet();
            var user = new TestUserFactory(_container).CreateList(2);

            var result = _target.DoExcel(search, new ActionHelperDoIndexArgs {
                OnSuccess = () => expectedResult
            });

            Assert.AreSame(expectedResult, result);
        }

        [TestMethod]
        public void TestDoExcelCallsSearchMethodActionInPlaceOfRepositorySearchCallIfActionIsNotNull()
        {
            var mockRepo = new Mock<IRepository<TestUser>>();
            mockRepo.Setup(x => x.Search(It.IsAny<TestSearchSet>()))
                    .Throws(new Exception("This should not be getting called."));
            _container.Inject(mockRepo.Object);
            InitializeTarget();

            var overrideSearchMethodCalled = false;
            var search = new TestSearchSet();
            var result = _target.DoExcel(search, new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => { overrideSearchMethodCalled = true; }
            });

            Assert.IsTrue(overrideSearchMethodCalled);
        }

        [TestMethod]
        public void TestDoExcelRedirectsBackToSearchWithErrorMessageIfResultsHigherThanMaxResults()
        {
            var max = 2;
            var search = new TestSearchSet();
            var users = new TestUserFactory(_container).CreateList(max + 1);

            var result =
                (RedirectToRouteResult)_target.DoExcel(search,
                    new ActionHelperDoIndexArgs {MaxResults = max});

            var resultError = ((List<string>)_controller.TempData[ControllerBase.ERROR_MESSAGE_KEY]).Single();
            Assert.AreEqual(
                $"The query you have entered will bring back more than {max} results. Please refine your search.",
                resultError);
        }

        [TestMethod]
        public void TestDoExcelReturnsResultsWithOnlyExportableProperties()
        {
            var search = new TestSearchSet();
            var user = new TestUserFactory(_container).Create();
            var exportableProperties = new List<string> {"UniqueName"};
            search.ExportableProperties = exportableProperties;

            var result = (ExcelResult)_target.DoExcel(search);
            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.ContainsColumn("Foo"); // UniqueName becomes Foo
                helper.DoesNotContainColumn("Email");
            }
        }

        [TestMethod]
        public void TestDoExcelReturnsResultsWithAllColumnsWhenExportablePropertiesIsNull()
        {
            var search = new TestSearchSet();
            var user = new TestUserFactory(_container).Create();
            search.ExportableProperties = null;

            var result = (ExcelResult)_target.DoExcel(search);
            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.ContainsColumn("Foo"); // UniqueName becomes Foo
                helper.ContainsColumn("Email");
            }
        }

        [TestMethod]
        public void TestDoExcelReturnsResultsWithExportablePropertiesThatDoNotOverrideDoesNotExport()
        {
            var search = new TestSearchSet();
            var user = new TestUserFactory(_container).Create();
            var exportableProperties = new List<string> {"UniqueName", "AdministratorId"};
            search.ExportableProperties = exportableProperties;

            var result = (ExcelResult)_target.DoExcel(search);
            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.ContainsColumn("Foo"); // UniqueName becomes Foo
                helper.DoesNotContainColumn("AdministratorId"); // This is marked with DoesNotExport
            }
        }

        [TestMethod]
        public void TestDoExcelSetsAutofitOnResultFromArgs()
        {
            var search = new TestSearchSet();
            var users = new TestUserFactory(_container).CreateList(2);

            var result = (ExcelResult)_target.DoExcel(search, new ActionHelperDoIndexArgs {
                AutofitForExcel = false
            });

            Assert.IsFalse(result.Autofit);

            result = (ExcelResult)_target.DoExcel(search, new ActionHelperDoIndexArgs {
                AutofitForExcel = true
            });

            Assert.IsTrue(result.Autofit);
        }

        #endregion

        #region DoShow

        [TestMethod]
        public void Test_DoShow_FormatsCustomViewName_IfRequiredAndCustomViewNameIsNotAlreadyAVirtualPath()
        {
            InitializeForVirtualPathFormatTest();
            var entity = new TestUserFactory(_container).Create();
            MvcAssert.IsViewNamed(_viewPathTarget.DoShow(entity.Id, new ActionHelperDoShowArgs {
                ViewName = "~/Virtual"
            }), "~/Virtual");
            MvcAssert.IsViewNamed(_viewPathTarget.DoShow(entity.Id, new ActionHelperDoShowArgs {
                ViewName = "NotVirtual"
            }), "~/Views/SomePath/NotVirtual.cshtml");
        }

        [TestMethod]
        public void Test_DoShow_ReturnsCustomViewName()
        {
            var expected = "ShowThisInstead";
            var entity = new TestUserFactory(_container).Create();

            var result = (ViewResult)_target.DoShow(entity.Id, new ActionHelperDoShowArgs {ViewName = expected});

            Assert.AreEqual(expected, result.ViewName);
            Assert.AreEqual(entity, result.Model);
        }

        [TestMethod]
        public void Test_DoShow_ReturnsPartialResult_WhenIsPartialIsTrue()
        {
            var entity = new TestUserFactory(_container).Create();
            var result = _target.DoShow(entity.Id, new ActionHelperDoShowArgs {IsPartial = true});
            MvcAssert.IsPartialView(result);
        }

        [TestMethod]
        public void Test_DoShow_ReturnsPartialResultWithDefaultPartialName_WhenIsPartialIsTrueAndViewNameIsNotSet()
        {
            var entity = new TestUserFactory(_container).Create();
            var result = _target.DoShow(entity.Id, new ActionHelperDoShowArgs {IsPartial = true, ViewName = null});
            MvcAssert.IsViewNamed(result, "_Show");
        }

        [TestMethod]
        public void Test_DoShow_ReturnsNotFound_WhenNotFound()
        {
            var expected = "Not Found";

            var result = (HttpNotFoundResult)_target.DoShow(666, new ActionHelperDoShowArgs {NotFound = expected});

            Assert.AreEqual(expected, result.StatusDescription);
        }

        [TestMethod]
        public void Test_DoShow_ReturnsResultFromOnNotFound_WhenItExists()
        {
            ActionResult expected = new ViewResult();
            Func<ActionResult> onNotFound = () => expected;

            var result = _target.DoShow(666, new ActionHelperDoShowArgs {OnNotFound = onNotFound});

            Assert.AreSame(expected, result);
        }

        [TestMethod]
        public void Test_DoShow_ReturnsDefaultResult_WhenOnNotFoundExistsButReturnsNull()
        {
            Func<ActionResult> onNotFound = () => { return null; };

            var result = _target.DoShow(666,
                new ActionHelperDoShowArgs {OnNotFound = onNotFound, NotFound = string.Empty});
            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void Test_DoShow_ReturnsPartialView()
        {
            var entity = new TestUserFactory(_container).Create();

            var result = _target.DoShow(entity.Id, new ActionHelperDoShowArgs {
                ViewName = "_SomePartialView",
                IsPartial = true
            });

            MvcAssert.IsPartialView(result);
            MvcAssert.IsViewNamed(result, "_SomePartialView");
        }

        [TestMethod]
        public void Test_DoShow_ReturnsPartialView_WithFormattedPathIfRequired()
        {
            InitializeForVirtualPathFormatTest();
            var entity = new TestUserFactory(_container).Create();
            var result = _viewPathTarget.DoShow(entity.Id, new ActionHelperDoShowArgs {
                ViewName = "_Show",
                IsPartial = true
            });
            MvcAssert.IsPartialView(result);
            MvcAssert.IsViewNamed(result, "~/Views/SomePath/_Show.cshtml");
        }

        [TestMethod]
        public void Test_DoShow_ReturnsShowView_WithFormattedPathIfRequired()
        {
            InitializeForVirtualPathFormatTest();
            var entity = new TestUserFactory(_container).Create();
            MvcAssert.IsViewNamed(_viewPathTarget.DoShow(entity.Id), "~/Views/SomePath/Show.cshtml");
        }

        [TestMethod]
        public void Test_DoShow_ReturnsViewWithRecord()
        {
            var entity = new TestUserFactory(_container).Create();

            var result = (ViewResult)_target.DoShow(entity.Id);

            Assert.AreEqual("Show", result.ViewName);
            Assert.AreEqual(entity, result.Model);
        }

        [TestMethod]
        public void Test_DoShow_WithEmptyArgs_ReturnsViewWithRecord()
        {
            var entity = new TestUserFactory(_container).Create();

            var result = (ViewResult)_target.DoShow(entity.Id, new ActionHelperDoShowArgs());

            Assert.AreEqual("Show", result.ViewName);
            Assert.AreEqual(entity, result.Model);
        }

        [TestMethod]
        public void TestDoShowCallsOnModelFoundActionIfActionExistsAndModelIsFound()
        {
            var entity = new TestUserFactory(_container).Create();
            var onModelFoundWasCalled = false;
            Action<TestUser> shouldBeCalled = (x) => { onModelFoundWasCalled = true; };

            _target.DoShow(entity.Id, new ActionHelperDoShowArgs {NotFound = string.Empty},
                onModelFound: shouldBeCalled);
            Assert.IsTrue(onModelFoundWasCalled);

            onModelFoundWasCalled = false;
            _target.DoShow(0, new ActionHelperDoShowArgs {NotFound = string.Empty}, onModelFound: shouldBeCalled);
            Assert.IsFalse(onModelFoundWasCalled);
        }

        [TestMethod]
        public void TestDoShowReturnsValueReturnedFromGetEntityOverrideWhenSet()
        {
            var entity = new TestUserFactory(_container).Create();
            var expected = new TestUser();

            var result = (ViewResult)_target.DoShow(entity.Id, new ActionHelperDoShowArgs {
                GetEntityOverride = () => expected
            });

            Assert.AreEqual("Show", result.ViewName);
            Assert.AreSame(expected, result.Model);
        }

        [TestMethod]
        public void TestDoShowReturnsNotFoundIfGetEntityOverrideReturnsNull()
        {
            var entity = new TestUserFactory(_container).Create();

            var result = _target.DoShow(entity.Id, new ActionHelperDoShowArgs {
                GetEntityOverride = () => null,
                NotFound = "Some not found message"
            });

            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void Test_DoShow_ReturnsOnSuccess_IfSetAndSuccessful()
        {
            var entity = new TestUserFactory(_container).Create();
            var onSuccessCalled = false;
            var expectedResult = new EmptyResult();

            Func<TestUser, ActionResult> shouldBeCalled = _ => {
                onSuccessCalled = true;
                return expectedResult;
            };

            var result = _target.DoShow(entity.Id, new ActionHelperDoShowArgs<TestUser> {
                OnSuccess = shouldBeCalled
            });

            Assert.IsTrue(onSuccessCalled);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Test_DoShow_ReturnsViewWithRecord_IfOnSuccessSetButReturnsNull()
        {
            var entity = new TestUserFactory(_container).Create();
            Func<TestUser, ActionResult> shouldBeCalled = _ => null;

            var result = (ViewResult)_target.DoShow(entity.Id, new ActionHelperDoShowArgs<TestUser> {
                OnSuccess = shouldBeCalled
            });

            Assert.AreEqual("Show", result.ViewName);
            Assert.AreEqual(entity, result.Model);
        }

        #endregion

        #region DoSearch

        [TestMethod]
        public void TestDoSearchReturnsSearchViewWithModelInstanceOfNoModelIsSupplied()
        {
            var result = (ViewResultBase)_target.DoSearch<TestSearchSet>();
            Assert.IsTrue(_controller.SetLookupDataCalled);
            Assert.AreEqual(ControllerAction.Search, _controller.ControllerActionUsedForLookupData.Value);
            MvcAssert.IsViewNamed(result, "Search");
            Assert.IsInstanceOfType(result.Model, typeof(TestSearchSet));
        }

        [TestMethod]
        public void TestDoSearchReturnsSearchViewWithModelThatIsSupplied()
        {
            var expectedModel = new TestSearchSet();
            var result = (ViewResultBase)_target.DoSearch(expectedModel);
            Assert.IsTrue(_controller.SetLookupDataCalled);
            Assert.AreEqual(ControllerAction.Search, _controller.ControllerActionUsedForLookupData.Value);
            MvcAssert.IsViewWithNameAndModel(result, "Search", expectedModel);
        }

        #endregion

        #region DoDestroy

        [TestMethod]
        public void TestDoDestroyDeletesTheGivenRecord()
        {
            var entity = new TestUserFactory(_container).Create();
            var entityThatShouldStillExist = new TestUserFactory(_container).Create();

            var result = _target.DoDestroy(entity.Id,
                new ActionHelperDoDestroyArgs {OnSuccessRedirectAction = string.Empty});

            Session.Clear();

            Assert.IsFalse(Session.Query<TestUser>().Any(x => x.Id == entity.Id));
            Assert.IsNotNull(Session.Query<TestUser>().Single(x => x.Id == entityThatShouldStillExist.Id));
        }

        [TestMethod]
        public void TestDoDestroyReturnsOnSuccessResultFromArgsWhenNotNull()
        {
            var entity = new TestUserFactory(_container).Create();
            var expectedResult = new EmptyResult();
            Func<ActionResult> onSuccess = () => expectedResult;

            var result = _target.DoDestroy(entity.Id,
                new ActionHelperDoDestroyArgs {OnSuccessRedirectAction = string.Empty, OnSuccess = onSuccess});

            Assert.AreSame(expectedResult, result);

            entity = new TestUserFactory(_container).Create();
            // When null, it should return the default result.
            onSuccess = () => null;

            result = _target.DoDestroy(entity.Id,
                new ActionHelperDoDestroyArgs {OnSuccessRedirectAction = string.Empty, OnSuccess = onSuccess});
            Assert.AreNotSame(expectedResult, result);
            Assert.IsNotNull(result, "A null result should not be passed back.");
        }

        [TestMethod]
        public void TestDoDestroyReturnsShowForSqlErrorsWithShowNotification()
        {
            var entity = new TestUserFactory(_container).Create();
            var group = new TestGroupFactory(_container).Create(new {Administrator = entity});

            var result = _target.DoDestroy(entity.Id);

            Assert.AreEqual("Show", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual(entity.Id, ((RedirectToRouteResult)result).RouteValues["id"]);
            Assert.AreEqual(
                ActionHelper<TestControllerWithPersistenceWithRepository, IRepository<TestUser>, TestUser, TestUser>
                   .SQL_ERROR, ((List<string>)_target.Controller.TempData[ControllerBase.ERROR_MESSAGE_KEY])[0]);
        }

        [TestMethod]
        public void TestDoDestroyDisplaysValidationErrorsWhenModelStateIsInvalid()
        {
            var entity = new TestUserFactory(_container).Create();
            _target.Controller.ModelState.AddModelError("foo", "bar");

            var result = _target.DoDestroy(entity.Id);

            Assert.IsTrue(((List<string>)_target.Controller.TempData[ControllerBase.ERROR_MESSAGE_KEY])
               .Contains("bar"));
        }

        [TestMethod]
        public void TestDoDestroyRedirectsToShowWhenModelStateIsInvalidIfThereIsNoOnErrorOverride()
        {
            var entity = new TestUserFactory(_container).Create();
            _target.Controller.ModelState.AddModelError("foo", "bar");

            var result = _target.DoDestroy(entity.Id);

            Assert.AreEqual("Show", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual(entity.Id, ((RedirectToRouteResult)result).RouteValues["id"]);
        }

        [TestMethod]
        public void TestDoDestroyRedirectsToShowWhenModelStateIsInvalidIfTheOnErrorOverrideReturnsNull()
        {
            var entity = new TestUserFactory(_container).Create();
            _target.Controller.ModelState.AddModelError("foo", "bar");

            var result = _target.DoDestroy(entity.Id, new ActionHelperDoDestroyArgs {
                OnError = () => null
            });

            Assert.AreEqual("Show", ((RedirectToRouteResult)result).RouteValues["action"]);
            Assert.AreEqual(entity.Id, ((RedirectToRouteResult)result).RouteValues["id"]);
        }

        [TestMethod]
        public void TestDoDestroyReturnsWhateverOnErrorReturnsWhenModelStateIsInvalid()
        {
            var entity = new TestUserFactory(_container).Create();
            _target.Controller.ModelState.AddModelError("foo", "bar");
            var expectedResult = new EmptyResult();

            var result = _target.DoDestroy(entity.Id, new ActionHelperDoDestroyArgs {
                OnError = () => expectedResult
            });

            Assert.AreSame(expectedResult, result);
        }

        [TestMethod]
        public void TestGetSqlError()
        {
            ////The DELETE statement conflicted with the REFERENCE constraint "FK_tblNJAWValves_Streets_StName". The conflict occurred in database "mapcalldev", table "dbo.Valves", column 'StreetId'.\r\nThe statement has been terminated.
            /// 
            var toMatch =
                "The DELETE statement conflicted with the REFERENCE constraint \"FK_tblNJAWValves_Streets_StName\". The conflict occurred in database \"mapcalldev\", table \"dbo.Valves\", column 'StreetId'.\r\nThe statement has been terminated.";

            var result = _target.GetSqlError(new GenericADOException(string.Empty, new Exception(toMatch)));
            Assert.AreEqual(
                "This record is linked to the dbo.Valves table and cannot be deleted as a result. Column: StreetId, Constraint FK_tblNJAWValves_Streets_StName",
                result);

            result = _target.GetSqlError(new GenericADOException(toMatch, null));
            Assert.AreEqual(
                "This record is linked to the dbo.Valves table and cannot be deleted as a result. Column: StreetId, Constraint FK_tblNJAWValves_Streets_StName",
                result);

            result = _target.GetSqlError(new GenericADOException("some other message", null));
            Assert.AreEqual(
                ActionHelper<TestControllerWithPersistenceWithRepository, IRepository<TestUser>, TestUser, TestUser>
                   .SQL_ERROR, result);
        }

        #endregion

        #region Test classes

        private class
            TestControllerWithPersistenceWithRepository : ControllerBaseWithPersistence<IRepository<TestUser>, TestUser,
                TestUser>
        {
            #region Properties

            public bool SetLookupDataCalled { get; set; }
            public ControllerAction? ControllerActionUsedForLookupData { get; set; }

            #endregion

            #region Constructors

            public TestControllerWithPersistenceWithRepository(
                ControllerBaseWithPersistenceArguments<IRepository<TestUser>, TestUser, TestUser> args) : base(args) { }

            #endregion

            #region Exposed Methods

            public override void SetLookupData(ControllerAction action)
            {
                SetLookupDataCalled = true;
                ControllerActionUsedForLookupData = action;
            }

            #endregion
        }

        [ActionHelperViewVirtualPathFormat("~/Views/SomePath/{0}.cshtml")]
        private class TestControllerWithViewPathFormat : TestControllerWithPersistenceWithRepository
        {
            #region Constructors

            public TestControllerWithViewPathFormat(
                ControllerBaseWithPersistenceArguments<IRepository<TestUser>, TestUser, TestUser> args) : base(args) { }

            #endregion
        }

        private class TestUserViewModel : ViewModel<TestUser>
        {
            #region Properties

            public bool SetDefaultsCalled { get; set; }
            public bool SetDefaultsCalledBeforeMap { get; set; }
            public bool MapCalled { get; set; }

            public string Email { get; set; }

            #endregion

            #region Constructors

            public TestUserViewModel(IContainer container, TestUser entity) : this(container)
            {
                if (entity != null)
                {
                    Map(entity);
                }
            }

            public TestUserViewModel(IContainer container) : base(container) { }

            #endregion

            #region Exposed Methods

            public override void SetDefaults()
            {
                SetDefaultsCalled = true;
            }

            public override void Map(TestUser entity)
            {
                base.Map(entity);
                SetDefaultsCalledBeforeMap = SetDefaultsCalled;
                MapCalled = true;
            }

            #endregion
        }

        private class TestUserViewModelWithEmail : TestUserViewModel
        {
            #region Properties

            public string Email { get; set; }

            #endregion

            public TestUserViewModelWithEmail(IContainer container, TestUser entity) : base(container, entity) { }
            public TestUserViewModelWithEmail(IContainer container) : base(container) { }
        }

        private class TestSearchSet : SearchSet<TestUser>
        {
            #region Properties

            public virtual int? Id { get; set; }
            public virtual string Email { get; set; }
            public virtual TestGroup MainGroup { get; set; }
            public virtual DateRange DateAdded { get; set; }

            #endregion
        }

        #endregion
    }
}
