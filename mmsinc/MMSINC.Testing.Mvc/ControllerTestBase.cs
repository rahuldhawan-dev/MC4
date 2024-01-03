using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;
using MMSINC.Testing.MSTest.IgnoreIf;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities.StructureMap;
using NHibernate.Criterion;
using StructureMap;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MMSINC.Testing
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TMvcApplication">This needs to be the MvcApplication type for the site project being tested. It is important for the MvcApplicationTester to function correctly.</typeparam>
    /// <typeparam name="TController"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TRepository"></typeparam>
    /// <remarks>
    ///
    /// SOME EDUCATION ABOUT THE AUTOMATIC ACTION TESTS THAT SHOULD PROBABLY BE IN THE WIKI SECTION UNDER "TESTING"
    ///
    /// Any test that inherits from ControllerTestBase will automatically include common unit tests for our basic CRUD actions.
    /// This means you do not have to write any particular tests for Create/Destroy/Edit/Index/Search/Show/Update unless they
    /// do something other than the basic ActionHelper calls.
    ///
    /// 1. If the controller does not have one of those actions, then the test will be skipped(it will pass, but it otherwise
    ///    will not run).
    /// 2. If the test fails for a good reason, then you should probably fix what's broken. This means potentially setting configuration
    ///    stuff on AutoTestOptions if it's just a testing bug and not an application bug.
    /// 3. If the test fails because the action is doing something unsupported(unusual model types, multiple parameters, not
    ///    using ActionHelper, etc), then override the test method and implement it yourself. If implementing
    ///    the test doesn't make sense, but you otherwise have written tests to cover what the action does, then you can make
    ///    the override a no-op.
    /// 4. Things we shouldn't be doing are not part of these tests. ex: throwing ModelValidationException
    /// 5. DON'T implement test overrides that violate the test name. ie if the test says it "Returns the Show view", don't implement
    ///    a test that returns a json view or a redirect. Yes I'm aware that the auto test options make this hypocritical.
    /// 6. WHEN OVERRIDING THE TESTS you must leave a comment explaining why.
    ///
    /// </remarks>
    public abstract class
        ControllerTestBase<TMvcApplication, TController, TEntity, TRepository> : InMemoryDatabaseTest<TEntity,
            TRepository>
        where TMvcApplication : MvcApplication, new()
        where TController : ControllerBase
        where TEntity : class, new()
        where TRepository : class, IRepository<TEntity>
    {
        #region Private Members

        protected TController _target;
        private Type _controllerClass;
        private string _resourceName;
        protected IViewModelFactory _viewModelFactory;
        private AutomatedTestOptions _autoTestOptions;

        #endregion

        #region Private fields related to automatic tests

        // All of this stuff is for caching things to reduce the amount of reflection
        // being done. The automatic tests generate *a lot* of tests.

        private static readonly Type _controllerType = typeof(TController);
        private static readonly string _controllerTypeFullName = _controllerType.FullName;

        private static readonly Dictionary<string, MethodInfo> _actionMethodsByName =
            new Dictionary<string, MethodInfo>();

        private static readonly string _thisControllerName = _controllerType.Name.Replace("Controller", string.Empty);
        private static readonly Type _intType = typeof(int);
        private static readonly Type _viewModelTEntityType = typeof(ViewModel<TEntity>);
        private static readonly Type _searchSetBaseType = typeof(ISearchSet);
        private static readonly Type _tentityType = typeof(TEntity);
        private static readonly Type _ienumerableTEntityType = typeof(IEnumerable<TEntity>);
        private static IEnumerable<string> _overrideMethodsMissingTestMethodAttribute;
        private static readonly Dictionary<string, Func<bool>> _testsCanRunByName;
        private readonly Type _thisTestType;
        private const string URL_REFERRER = "http://www.internet.com/";
        private bool _setAutomatedTestsOptionsBaseMethodRan = false;

        #endregion

        #region Properties

        // Don't lazy load Application. And don't put a public setter on this
        // unless you explicitly dispose the original instance. 
        public MvcApplicationTester<TMvcApplication> Application { get; private set; }
        public FakeMvcHttpHandler Request { get; set; }

        public Type ControllerClass
        {
            get { return _controllerClass ?? (_controllerClass = _controllerType); }
        }

        public string ResourceName
        {
            get { return _resourceName ?? (_resourceName = _thisControllerName); }
        }

        /// <summary>
        /// TestContext of the currently running test. Automatically set by test runner.
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Constructors

        static ControllerTestBase()
        {
            // Only cache the typical actions. There's maybe one spot where we need to actually
            // use an action that isn't the typical CRUD actions. Doing GetMethods().ToDictionary()
            // doesn't work here because of method overloads causing duplicate key errors.

            void AddMethod(string method)
            {
                _actionMethodsByName.Add(method, _controllerType.GetMethod(method));
            }

            AddMethod("Create");
            AddMethod("Destroy");
            AddMethod("Edit");
            AddMethod("Index");
            AddMethod("New");
            AddMethod("Search");
            AddMethod("Show");
            AddMethod("Update");

            // TODO: This needs to check for override tests somehow. The override would need to skip the CanRunTest check and return true.
            // DOUBLE TODO: Make sure all test methods are here. I think I added new ones.
            // TRIPLE TODO: Maybe make a single method that also adds these methods to the list of methods that
            //              need to be checked for TestMethod so they only need to be referenced in one spot.
            var testCanRun = new Dictionary<string, Func<bool>>();

            testCanRun.Add(nameof(TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving), CanRunTestForCreate);
            testCanRun.Add(nameof(TestCreateReturnsNewViewWithModelIfModelStateErrorsExist), CanRunTestForCreate);
            testCanRun.Add(nameof(TestCreateSavesNewRecordWhenModelStateIsValid), CanRunTestForCreate);

            testCanRun.Add(nameof(TestDestroyReturnsNotFoundIfRecordCanNotBeFound), CanRunTestForDestroy);
            testCanRun.Add(nameof(TestDestroyRedirectsToSearchPageWhenRecordIsSuccessfullyDestroyed),
                CanRunTestForDestroy);
            testCanRun.Add(nameof(TestDestroyActuallyDeletesTheRecordAndOnlyTheRecord), CanRunTestForDestroy);
            testCanRun.Add(nameof(TestDestroyRedirectsBackToShowPageOfAttemptedDeletedRecordIfThereAreModelStateErrors),
                CanRunTestForDestroy);

            testCanRun.Add(nameof(TestEditReturnsEditViewWithEditViewModel), CanRunTestForEdit);
            testCanRun.Add(nameof(TestEditReturns404IfMatchingRecordCanNotBeFound), CanRunTestForEdit);

            testCanRun.Add(nameof(TestIndexCanPerformSearchForAllSearchModelProperties), CanRunTestForIndex);
            testCanRun.Add(nameof(TestIndexRedirectsToSearchIfModelStateIsInvalid), CanRunTestForIndex);
            testCanRun.Add(nameof(TestIndexRedirectsToSearchIfThereAreZeroResults), CanRunTestForIndex);
            testCanRun.Add(nameof(TestIndexReturnsResults), CanRunTestForIndex);

            testCanRun.Add(nameof(TestNewReturnsNewViewWithNewViewModel),
                () => CanRunTestForNew() != NewTestType.CanNotTest);

            testCanRun.Add(nameof(TestSearchReturnsSearchViewWithModel),
                () => CanRunTestForSearch() != SearchTestType.CanNotTest);

            testCanRun.Add(nameof(TestShowReturnsNotFoundIfRecordCanNotBeFound), CanRunTestForShow);
            testCanRun.Add(nameof(TestShowReturnsShowViewWhenRecordIsFound), CanRunTestForShow);

            testCanRun.Add(nameof(TestUpdateSavesChangesWhenModelStateIsValid), CanRunTestForUpdate);
            testCanRun.Add(nameof(TestUpdateRedirectsToShowActionAfterSuccessfulSave), CanRunTestForUpdate);
            testCanRun.Add(nameof(TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors), CanRunTestForUpdate);
            testCanRun.Add(nameof(TestUpdateReturnsNotFoundIfRecordBeingUpdatedDoesNotExist), CanRunTestForUpdate);

            _testsCanRunByName = testCanRun;
        }

        protected ControllerTestBase()
        {
            _thisTestType = GetType();
        }

        #endregion

        #region Private Methods

        private static bool ShouldIgnoreControllerTest(ITestMethod testMethod)
        {
            return
                !CanRunTest(testMethod) ||
                (testMethod.TestMethodName.Contains("RedirectsToSearch") &&
                 GetActionMethodInfo("Search") == null);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IViewModelFactory>().Use<ViewModelFactory>();
        }

        protected virtual FakeMvcHttpHandler GetRequestHandler()
        {
            var actions = (from m in ControllerClass.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                           where m.ReturnType.IsOrIsSubclassOf<ActionResult>()
                           select m);
            var actionsWithNoParameters =
                (from m in actions where m.GetParameters().Count(p => p.IsOptional == false) == 0 select m).Union(
                    (from m in actions
                     where m.GetParameters().Count() == 1 && !m.GetParameters().First().ParameterType.IsValueType
                     select m));

            if (actionsWithNoParameters.Any())
            {
                return Application.CreateRequestHandler(String.Format("~/{0}/{1}", ResourceName,
                    actionsWithNoParameters.First().Name));
            }

            var actionsWithSingleIntParameter =
                (from m in actions
                 where m.GetParameters().Count() == 1 && m.GetParameters().First().ParameterType == _intType
                 select m);

            if (actionsWithSingleIntParameter.Any())
            {
                return Application.CreateRequestHandler(String.Format("~/{0}/{1}/0", ResourceName,
                    actionsWithSingleIntParameter.First().Name));
            }

            throw new NotImplementedException(
                String.Format(
                    "Could not find an action on controller {0} with either no parameters, or a single int or reference type parameter. You must override GetRequestHandler in this test class and return Application.CreateRequestHandler(string) with a valid url for {0}.",
                    ControllerClass.Name));
        }

        /// <summary>
        /// Resets the Request and target to a new instance based on the virtual path.
        /// ie ~/Controller/Action/Params
        /// </summary>
        /// <param name="virtualPath"></param>
        protected void InitializeControllerAndRequest(string virtualPath)
        {
            Request = Application.CreateRequestHandler(virtualPath);
            _target = Request.CreateAndInitializeController<TController>();
        }

        private static bool CanRunTest(string testMethod, Type testClassType)
        {
            if (_testsCanRunByName.TryGetValue(testMethod, out var canRunFn))
            {
                // Check if override for test name, if so return true.
                if (testClassType !=
                    typeof(ControllerTestBase<TMvcApplication, TController, TEntity, TRepository>))
                {
                    return true;
                }

                // else return canRunFn
                return canRunFn();
            }

            // Because this is going to run for every test, including the ones that aren't
            // automatic, we need to return the default.
            return true;
        }

        private static bool CanRunTest(ITestMethod testMethod)
        {
            return CanRunTest(testMethod.TestMethodName, testMethod.MethodInfo.DeclaringType);
        }

        private void TestOverrideTestMethodsHaveTestMethodAttributes()
        {
            // Only run this once so we're not wasting CPU doing this 20 times per test class.
            if (_overrideMethodsMissingTestMethodAttribute == null)
            {
                var methodsToTest = _testsCanRunByName.Keys.Concat(new[] {nameof(TestControllerAuthorization)});

                var thisTestType = GetType();
                _overrideMethodsMissingTestMethodAttribute = methodsToTest
                                                            .Where(x => !thisTestType
                                                                        .GetMethod(x)
                                                                        .GetCustomAttributes<TestMethodAttribute>(false)
                                                                        .Any()).Select(x => x).ToList();
            }

            if (_overrideMethodsMissingTestMethodAttribute.Any())
            {
                Assert.Fail(
                    $"{GetType().FullName}: The inherited {string.Join(",", _overrideMethodsMissingTestMethodAttribute)} method(s) need a TestMethod attribute or else it'll never run.");
            }
        }

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void ControllerTestBaseTestInitialize()
        {
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));

            _viewModelFactory = _container.GetInstance<IViewModelFactory>();

            TestOverrideTestMethodsHaveTestMethodAttributes();
            _autoTestOptions = new AutomatedTestOptions();
            SetAutomatedTestOptions(_autoTestOptions);
            if (!_setAutomatedTestsOptionsBaseMethodRan)
            {
                Assert.Fail($"When overriding {nameof(SetAutomatedTestOptions)} you *must* call the base method.");
            }

            Application = _container
               .GetInstance<MvcApplicationTester<TMvcApplication>>();
            Request = GetRequestHandler();
            _target = Request.CreateAndInitializeController<TController>();
        }
        
        /// <summary>
        /// Called during *every test* not just the automated tests. Use this to configure
        /// overrides for how the automatic controller tests work.
        ///
        /// Remember to always call the base method!
        /// </summary>
        /// <param name="options"></param>
        protected virtual void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            // Need to ensure this is called, otherwise test failures are kinda confusing.
            _setAutomatedTestsOptionsBaseMethodRan = true;
        }

        [TestCleanup]
        public void ControllerTestBaseTestCleanup()
        {
            Application?.Dispose();
        }

        #endregion

        #region Automatic testing

        private static MethodInfo GetActionMethodInfo(string action)
        {
            if (_actionMethodsByName.TryGetValue(action, out var method))
            {
                return method;
            }

            return _controllerType.GetMethod(action);
        }

        protected virtual IEntity CreateEntityForAutomatedTests(bool saveEntity = true)
        {
            // NOTE: If GetEntityFactory is failing due to non-null or transitive properties then it's because
            //       the factory is not setting required values. You may need to fix an existing factory or 
            //       create a new one.
            // NOTE: If CreateValidEntity() isn't creating a valid entity, the implementation might need to be
            //       using BuildWithConcreteDependencies() rather than Create(). This happens when dealing with
            //       entities that have unique constraints.
            // NOTE: Or, you might also need to use Create in GetValidEntity() instead of BuildWithConcreteDependencies().
            //       There's not much rhyme or reason to why one works and the other doesn't sometimes. I *think*
            //       BuildWithConcreteDependencies seems to fail when dealing with entities that don't have a factory class.
            // NOTE: This thing gets weird when dealing with creating an entity that adds a reference of itself
            //       to another entity, but then the created entity isn't actually used. Flush error.
            var entity = _autoTestOptions.CreateValidEntity?.Invoke() ??
                         (IEntity)GetEntityFactory<TEntity>().BuildWithConcreteDependencies();
            if (saveEntity)
            {
                Session.Save(entity);
            }

            return entity;
        }

        /// <summary>
        /// Attempts to return an entity by id and bypasses any repository filtering. Use this
        /// if you need to ensure the existence of an entity regardless of role/other filtering.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private TEntity TryQueryForEntityByIdWithoutRepositoryFiltering(int id)
        {
            return Session.QueryOver<TEntity>().Where(Restrictions.Eq("Id", id)).SingleOrDefault();
        }

        private string GetCurrentModelStateErrors()
        {
            var sb = new StringBuilder();
            foreach (var ms in _target.ModelState.ToDictionaryOfErrors())
            {
                sb.AppendLine($"{ms.Key}: {ms.Value}");
            }

            return sb.ToString();
        }

        private void InitializeRedirectToReferrer(bool flag)
        {
            if (flag)
            {
                Request.Request.Setup(x => x.UrlReferrer).Returns(new Uri(URL_REFERRER));
            }
        }

        private void ShouldMaybeBePartialView(ActionResult result, bool shouldBePartial, string action)
        {
            if (shouldBePartial)
            {
                MvcAssert.IsPartialView(result, $"{_controllerTypeFullName}.{action} did not return a PartialView.");
            }
            else
            {
                MvcAssert.IsNotPartialView(result,
                    $"{_controllerTypeFullName}.{action} returned an unexpected PartialView.");
            }
        }

        private void AssertInconclusiveForGhostTest()
        {
            Assert.Inconclusive("Test can not run because it doesn't support the given action.");
        }

        #region Create

        private static bool CanRunTestForCreate()
        {
            var createAction = GetActionMethodInfo("Create");
            if (createAction == null)
            {
                return false; // Nothing to test
            }

            var createParams = createAction.GetParameters();
            if (createParams.Length != 1)
            {
                Assert.Fail($"{_controllerTypeFullName}.Create does not have exactly one parameter.");
            }

            var viewModelType = createParams.Single().ParameterType;
            if (!_viewModelTEntityType.IsAssignableFrom(viewModelType))
            {
                Assert.Fail(
                    $"{_controllerTypeFullName}.Create's view model parameter does not inherit from {_viewModelTEntityType.FullName}>.");
            }

            return true;
        }

        private dynamic CreateValidViewModelForCreateAction()
        {
            var validEntity = (TEntity)CreateEntityForAutomatedTests(false);
            var viewModelType = GetActionMethodInfo("Create").GetParameters().Single().ParameterType;
            // TODO: This should probably be done with ViewModelFactory but atm it only works with generic parameters.
            // 1. Create a view model that should be valid for creation.
            // NOTE: viewModel must be dynamic in order to properly pass it to the Create method dynamically.
            dynamic viewModel = _container.GetInstance(viewModelType);
            viewModel.Map(validEntity);
            _autoTestOptions.InitializeCreateViewModel?.Invoke(viewModel);

            // This is a sanity check that we're definitely creating a valid view model
            // to pass to the Create action. If you're seeing this fail, you likely to do
            // one of the following things:
            // 1. If the property that's failing has a [Required] validator(*NOT* RequiredWhen)
            // check that the entity factory has a default value set for that property. This is only
            // relevant for create view models, because a required field on a create model should
            // mean that that's the new default state for all records. If adding a default to the
            // factory will break stuff or is not doable, go to the next step.
            // 2. If it's failing for any other reason, you need to override the automated 
            // test options for InitializeCreateViewModel to set the model values to a valid state.
            ValidationAssert.ModelStateIsValid(viewModel);
            return viewModel;
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            _autoTestOptions.InitializeTestForCreateAndUpdateTests();
            // NOTE: viewModel must be dynamic in order to properly pass it to the Create method dynamically.
            dynamic viewModel = CreateValidViewModelForCreateAction();
            var oldId = viewModel.Id;

            var existingRecordCount = Repository.GetAll().Count();
            InitializeRedirectToReferrer(_autoTestOptions.CreateRedirectsToReferrerOnSuccess);
            // 2. Pass it to create
            var result = (ActionResult)_target.AsDynamic().Create(viewModel);

            var newId = (int)viewModel.Id;
            Assert.AreNotEqual(oldId, newId,
                $"{_controllerTypeFullName}.Create did not set the view model's Id parameter to its new value after saving.");

            // Ensure that exactly one record is saved
            var newRecordCount = Repository.GetAll().Count();
            // If this is failing:
            //    - The factory's Build method may be adding the entity to a parent collection. When the session
            //      flushes, that entity ends up getting saved. In this case, you might need to set CreateValidEntity
            //      so that it uses Create() instead of BuildWithConcreteDependencies()
            Assert.AreEqual(existingRecordCount + 1, newRecordCount,
                $"{_controllerTypeFullName}.Create saved {(newRecordCount - existingRecordCount)} records rather than 1.");
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            _autoTestOptions.InitializeTestForCreateAndUpdateTests();
            // NOTE: viewModel must be dynamic in order to properly pass it to the Create method dynamically.
            dynamic viewModel = CreateValidViewModelForCreateAction();

            InitializeRedirectToReferrer(_autoTestOptions.CreateRedirectsToReferrerOnSuccess);
            // 2. Pass it to create
            var result = (ActionResult)_target.AsDynamic().Create(viewModel);

            if (!_target.ModelState.IsValid)
            {
                Assert.Fail(
                    $"{_controllerTypeFullName}.Create did not get a valid model. Errors: {GetCurrentModelStateErrors()}");
            }

            if (_autoTestOptions.CreateReturnsPartialShowViewOnSuccess)
            {
                MvcAssert.IsPartialView(result, $"{_controllerTypeFullName}.Create did not return a partial view.");

                var expectedModel = Repository.Find(viewModel.Id);
                MvcAssert.IsViewWithNameAndModel(result, _autoTestOptions.ExpectedShowViewName ?? "_Show",
                    expectedModel,
                    $"{_controllerTypeFullName}.Create did not return a expected view and/or expected model.");
            }
            else if (_autoTestOptions.CreateRedirectsToReferrerOnSuccess)
            {
                MvcAssert.RedirectsToUrl(result, URL_REFERRER + _autoTestOptions.CreateRedirectSuccessUrlFragment,
                    $"{_controllerTypeFullName}.Create did not redirect to expected url when successfully creating a record.");
            }
            else
            {
                // NOTE: Is this test failing because the id values don't match? Then it's probably
                // because the controller is redirecting to a different controller.
                var controllerName = _autoTestOptions.ExpectedCreateRedirectControllerName ?? _thisControllerName;
                var routeArgs = _autoTestOptions.CreateRedirectsToRouteOnSuccessArgs?.Invoke(viewModel) ??
                                new {action = "Show", controller = controllerName, id = viewModel.Id};
                // TODO: This should potentially test for area, but it's not supplied often. The area is assumed from the current request in MVC.
                MvcAssert.RedirectsToRoute(result, routeArgs,
                    $"{_controllerTypeFullName}.Create did not set redirect to the expected route.");
            }
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            _autoTestOptions.InitializeTestForCreateAndUpdateTests();
            // NOTE: viewModel must be dynamic in order to properly pass it to the Create method dynamically.
            dynamic viewModel = CreateValidViewModelForCreateAction();
            _target.ModelState.AddModelError("Auto test error", "for Create action");
            var initialRepoCount = Repository.GetAll().Count();
            // 2. Pass it to create
            InitializeRedirectToReferrer(_autoTestOptions.CreateRedirectsToReferrerOnError);
            var result = (ActionResult)_target.AsDynamic().Create(viewModel);

            if (_autoTestOptions.CreateRedirectsToReferrerOnError)
            {
                MvcAssert.RedirectsToUrl(result, URL_REFERRER + _autoTestOptions.CreateRedirectErrorUrlFragment,
                    $"{_controllerTypeFullName}.Create did not redirect to expected url when model state errors occurred.");
            }
            else
            {
                if (_autoTestOptions.CreateRedirectsToRouteOnErrorArgs != null)
                {
                    MvcAssert.RedirectsToRoute(result,
                        _autoTestOptions.CreateRedirectsToRouteOnErrorArgs.Invoke(viewModel),
                        $"{_controllerTypeFullName}.Create did not set redirect to the expected route.");
                }
                else
                {
                    var expectedViewName = _autoTestOptions.ExpectedNewViewName ?? "New";
                    MvcAssert.IsViewWithNameAndModel(result, expectedViewName, viewModel,
                        $"{_controllerTypeFullName}.Create did not return New view with expected model.");
                    Assert.AreEqual(initialRepoCount, Repository.GetAll().Count(),
                        $"{_controllerTypeFullName}.Create unexpectedly changed record count when it should not have.");
                }
            }
        }

        #endregion

        #region Destroy

        private static bool CanRunTestForDestroy()
        {
            var destroyAction = GetActionMethodInfo("Destroy");
            if (destroyAction == null)
            {
                return false; // Nothing to test.
            }

            var destroyParams = destroyAction.GetParameters();
            if (destroyParams.Length != 1)
            {
                // Show actions that have extra parameters shouldn't fail as long
                // as there's still an int id param.
                Assert.Fail(
                    $"{_controllerTypeFullName}.Destroy must have one parameter. The parameter must be an int or a {_viewModelTEntityType.FullName}>.");
            }
            else if (!destroyParams.Any())
            {
                Assert.Fail($"{_controllerTypeFullName}.Destroy does not have any parameters.");
            }

            return true;
        }

        private dynamic GenerateDestroyParam(int id)
        {
            // If action takes viewmodel, create view model, set Id, return viewmodel.
            var destroyParam = GetActionMethodInfo("Destroy").GetParameters().Single();
            if (destroyParam.ParameterType == _intType)
            {
                return id;
            }

            dynamic viewModel = _container.GetInstance(destroyParam.ParameterType);
            viewModel.Id = id;
            return viewModel;
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestDestroyReturnsNotFoundIfRecordCanNotBeFound()
        {
            // 1. See if the test can be ran
            // 2. The actual testing
            // The rest of this can just be done with dynamics
            var result = (ActionResult)_target.AsDynamic().Destroy(GenerateDestroyParam(-42484));
            MvcAssert.IsNotFound(result, null,
                $"{_controllerTypeFullName}.Destroy did not return the expected NotFoundResult.");
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestDestroyRedirectsToSearchPageWhenRecordIsSuccessfullyDestroyed()
        {
            // 1. See if the test can be ran
            // 2. The actual testing
            var entity = CreateEntityForAutomatedTests();

            // TODO: I'm not sure how I feel about this. We need to evict everything so we don't hit
            // errors when dealing with collection references(ex: deleting a MainCrossingInspection would
            // cause it to be-resaved if its MainCrossing.Inspections has been initialized). 
            // We don't normally have to worry about this in production, but it seems like something
            // we should be checking for.
            Session.Flush();
            Session.Clear();

            InitializeRedirectToReferrer(_autoTestOptions.DestroyRedirectsToReferrerOnSuccess);

            // Some redirects use values from before the entity was deleted. So we need to get those
            // before deleting them.
            var possibleRedirectArgs = _autoTestOptions.DestroyRedirectsToRouteOnSuccessArgs?.Invoke(entity.Id);
            // The rest of this can just be done with dynamics
            var result = (ActionResult)_target.AsDynamic().Destroy(GenerateDestroyParam(entity.Id));

            // NOTE: If this is failing due to a 404 redirect, you probably need to set CreateValidEntity.
            // This will happen because:
            //   - The controller's repository does extra filtering(ex WorkOrderPrePlanningController).

            if (_autoTestOptions.DestroyRedirectsToReferrerOnSuccess)
            {
                MvcAssert.RedirectsToUrl(result, URL_REFERRER + _autoTestOptions.DestroyRedirectSuccessUrlFragment,
                    $"{_controllerTypeFullName}.Destroy did not redirect to expected page when successfully deleting a record.");
            }
            else if (_autoTestOptions.DestroyReturnsHttpStatusCodeNoContentOnSuccess)
            {
                MvcAssert.IsStatusCode((int)HttpStatusCode.NoContent, result,
                    $"{_controllerTypeFullName}.Destroy did not return expected status code.");
            }
            else
            {
                var successRedirectAction = (GetActionMethodInfo("Search") != null) ? "Search" : "Index";
                var routeArgs = possibleRedirectArgs ??
                                new {action = successRedirectAction, controller = _thisControllerName};
                MvcAssert.RedirectsToRoute(result, routeArgs,
                    $"{_controllerTypeFullName}.Destroy did not redirect to the {successRedirectAction} action.");
            }
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestDestroyActuallyDeletesTheRecordAndOnlyTheRecord()
        {
            // 1. See if the test can be ran
            // 2. The actual testing
            var entityToDelete = CreateEntityForAutomatedTests();
            var entityThatStays = CreateEntityForAutomatedTests();

            // This might need to do the same session clearing as the other existing destroy test in this class

            Assert.AreNotSame(entityToDelete, entityThatStays,
                $"{_controllerTypeFullName}.Destroy failed sanity check. Entities created for this test must be different.");

            // TODO: I'm not sure how I feel about this. We need to evict everything so we don't hit
            // errors when dealing with collection references(ex: deleting a MainCrossingInspection would
            // cause it to be-resaved if its MainCrossing.Inspections has been initialized). 
            // We don't normally have to worry about this in production, but it seems like something
            // we should be checking for.
            Session.Flush();
            Session.Clear();

            // The rest of this can just be done with dynamics
            _target.AsDynamic().Destroy(GenerateDestroyParam(entityToDelete.Id));

            // NOTE: If this is failing to delete
            //  - It may be hidden due to ActionHelper.DoDestroy eating the exception.
            //  - For some reason, Repository.Find(entityToDelete.Id) is returning null.    
            Assert.IsNull(TryQueryForEntityByIdWithoutRepositoryFiltering(entityToDelete.Id),
                $"{_controllerTypeFullName}.Destroy failed to delete the expected entity.");
            Assert.IsNotNull(TryQueryForEntityByIdWithoutRepositoryFiltering(entityThatStays.Id),
                $"{_controllerTypeFullName}.Destroy somehow deleted the wrong entity.");
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestDestroyRedirectsBackToShowPageOfAttemptedDeletedRecordIfThereAreModelStateErrors()
        {
            // 1. See if the test can be ran
            // 2. The actual testing

            var entityToDelete = CreateEntityForAutomatedTests();
            _target.ModelState.AddModelError("Whoops!", "I'm in error!");
            InitializeRedirectToReferrer(_autoTestOptions.DestroyRedirectsToReferrerOnError);

            // The rest of this can just be done with dynamics
            dynamic viewModel = GenerateDestroyParam(entityToDelete.Id);
            var result = _target.AsDynamic().Destroy(viewModel);
            if (_autoTestOptions.DestroyRedirectsToReferrerOnError)
            {
                MvcAssert.RedirectsToUrl(result, URL_REFERRER + _autoTestOptions.DestroyRedirectErrorUrlFragment,
                    $"{_controllerTypeFullName}.Destroy did not redirect to expected page when failing to delete a record.");
            }
            else
            {
                var routeArgs = _autoTestOptions.DestroyRedirectsToRouteOnErrorArgs?.Invoke(viewModel) ?? new
                    {action = "Show", controller = _thisControllerName, id = entityToDelete.Id};
                MvcAssert.RedirectsToRoute(result, routeArgs,
                    $"{_controllerTypeFullName}.Destroy did not redirect to expected page when failing to delete a record.");
            }
        }

        #endregion

        #region Edit

        private static bool CanRunTestForEdit()
        {
            var newAction = GetActionMethodInfo("Edit");
            if (newAction == null)
            {
                return false; // Nothing to test.
            }

            var editParams = newAction.GetParameters();
            if (!editParams.Any())
            {
                Assert.Fail(
                    $"{_controllerTypeFullName}.Edit does not have any parameters. It should have a single integer id param.");
            }

            if (editParams.Length > 1)
            {
                if (editParams.First().ParameterType != _intType)
                {
                    Assert.Fail($"{_controllerTypeFullName}.Edit does not have its first parameter as an int.");
                }

                if (editParams.Count() - editParams.Count(x => x.IsOptional) > 1)
                {
                    Assert.Fail($"{_controllerTypeFullName}.Edit has more than one non-optional parameter.");
                }
            }

            return true;
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestEditReturnsEditViewWithEditViewModel()
        {
            var expectedViewName = _autoTestOptions.ExpectedEditViewName ?? "Edit";
            var existingValidEntity = CreateEntityForAutomatedTests();
            var result = (ActionResult)_target.AsDynamic().Edit(existingValidEntity.Id);
            MvcAssert.IsViewNamed(result, expectedViewName,
                $"{_controllerTypeFullName}.Edit did not return the expected ViewResult. If the expected view name is incorrect, this can be configured at the test level by setting ExpectedEditViewName.");

            var model = ((ViewResultBase)result).Model;

            // NOTE: If this fails with a 404:
            //   - The test might need to have AutomaticTestOptions.CreateValidEntity set
            //   - The TEntity for the controller test may be set to the wrong type.

            Assert.IsInstanceOfType(model, _viewModelTEntityType,
                $"{_controllerTypeFullName}.Edit did not return a model that inherits {_viewModelTEntityType.FullName}.");

            // Bypass this part of the test if there's no Update action, or the Update action has multiple parameters.
            // This should be a very very very very rare case, and that case should probably be because something is being
            // done wrong.
            if (_autoTestOptions.DoUpdateSingleViewModelParameterCheck)
            {
                var editModelType = GetActionMethodInfo("Update").GetParameters().Single().ParameterType;
                Assert.IsInstanceOfType(model, editModelType,
                    $"{_controllerTypeFullName}.Edit did not return the model type({editModelType.Name}) that the Update action expects.");
            }

            ShouldMaybeBePartialView(result, _autoTestOptions.EditReturnsPartialView, "Edit");
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestEditReturns404IfMatchingRecordCanNotBeFound()
        {
            var result = (ActionResult)_target.AsDynamic().Edit(0);
            MvcAssert.IsNotFound(result, null,
                $"{_controllerTypeFullName}.Edit did not return the expected NotFoundResult.");
        }

        #endregion

        #region Index

        private static bool CanRunTestForIndex()
        {
            var indexAction = GetActionMethodInfo("Index");

            if (indexAction == null)
            {
                return false; // Nothing to test.
            }

            var indexParams = indexAction.GetParameters();
            if (!indexParams.Any())
            {
                return false; // Nothing to test. There's no search model.
            }

            if (indexParams.Count() > 1)
            {
                // TODO: I don't think we do this anywhere. Could scan parameters for the proper type if necessary.
                // TODO: Tests failing here should also be looked at to see if their parameter can be moved to the view model.
                //       There's really no reason why it couldn't or shouldn't be on the view model.
                Assert.Fail($"{_controllerTypeFullName}.Index has more than one parameter.");
            }

            return true;
        }

        private Type GetSearchSetTypeFromSearchModelType(Type originalType)
        {
            if (!_searchSetBaseType.IsAssignableFrom(originalType))
            {
                // If this is failing, and the model is used with ActionHelper, then you need
                // to actually implement ISearchSet. I'm pretty sure it won't even compile if
                // you try to do that. If you can't use ISearchSet then you need to override the
                // search test and do it manually.
                Assert.Fail($"{GetType().FullName} failed: {originalType.FullName} does not implement ISearchSet.");
            }

            Type TryGetType(Type parentType)
            {
                var genericArgs = parentType.GetGenericArguments();
                if (genericArgs.Any())
                {
                    return genericArgs.Single(); // Should be only one parameter as of right now.
                }

                return TryGetType(parentType.BaseType);
            }

            return TryGetType(originalType);
        }

        private void ResultIsIndexView(ActionResult result)
        {
            var expectedViewName = _autoTestOptions.ExpectedIndexViewName ?? "Index";
            MvcAssert.IsViewNamed(result, expectedViewName,
                $"{_controllerTypeFullName}.Index returned incorrect view.");
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestIndexReturnsResults()
        {
            ActionResult result = null;

            var searchModelTester = GenerateSearchModelTesterForAction("Index");
            searchModelTester.SearchCallback = (searchModel) => { result = _target.AsDynamic().Index(searchModel); };

            var expectedEntity = CreateEntityForAutomatedTests();
            // NOTE: If this is failing because the results count == 0:
            //   1. you might need to set autoTestOptions.CreateValidEntity
            //   2. If that doesn't work, then you need to override this test method and test it yourself.
            //   3. It may fail if the search model has required properties that can't be initialized correctly. Implement the test yourself.
            //          ex: SearchSpoilStorageLocation requires OperatingCenter, but this test can't set that value.
            //   4. If GetEntityFactory<TEntity> returns a StaticListEntityLookupFactory then it's likely you need
            //      to set autoTestOptions.CreateValidEntity so that it uses a specific factory. 
            //          ex: WorkDescriptionControllerTest.
            //   5. Also if you're using a StaticListEntityLookupFactory, the override for CreateValidEntity
            //      almost certainly needs to have Session.Flush() called. The entity isn't otherwise being flushed
            //      automatically upon save, and querying for records isn't making NHibernate auto-flush it either.
            dynamic searchResult = searchModelTester.TestBlankSearchAndGetResult();
            Assert.IsInstanceOfType(searchResult.Results, _ienumerableTEntityType,
                $"{_controllerTypeFullName} failed at searching. This test can not be done automatically and must be overridden.");
            IEnumerable<TEntity> results = searchResult.Results;
            Assert.AreEqual(1, results.Count(),
                $"{_controllerTypeFullName} failed at searching. Expected one result, but got {results.Count()}.");
            Assert.AreSame(expectedEntity, results.Single(),
                $"{_controllerTypeFullName} failed at searching. Result was not the same as the expected entity.");

            // Good practice would probably dictate that this should be its own test.
            // However, it would literally be identical to the above bit of code as
            // it would require the exact same setup. If, for some reason, we start
            // requiring overrides for this, then we can split it out to its own test.
            // Otherwise it's just a waste of CPU cycles to run the index action in
            // another test.
            if (_autoTestOptions.IndexRedirectsToShowForSingleResult)
            {
                MvcAssert.RedirectsToRoute(result,
                    new {action = "Show", controller = _thisControllerName, id = expectedEntity.Id});
            }
            else
            {
                ResultIsIndexView(result);
            }
        }

        /// <summary>
        /// If you're overriding this method, then you need to ensure that you're testing
        /// every model property. You should not need to override this if you're doing
        /// normal ActionHelper.DoIndex things.
        /// </summary>
        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            RunActionCanPerformSearchWithSearchModel("Index",
                (searchModel) => { _target.AsDynamic().Index(searchModel); });
        }

        private static Type GetIndexModelType(string action = "Index") // Optional param literally needed for one test.
        {
            return GetActionMethodInfo(action).GetParameters().Single().ParameterType;
        }

        private ISearchModelTesterForSearchSet GenerateSearchModelTesterForAction(string action)
        {
            var searchModelType = GetIndexModelType(action);
            var searchModelTResultType = GetSearchSetTypeFromSearchModelType(searchModelType);
            var searchModelTesterEmptyType = typeof(SearchModelTesterForSearchSet<,,>);
            var searchModelTesterType =
                searchModelTesterEmptyType.MakeGenericType(searchModelType, searchModelTResultType, _tentityType);

            var searchModelTester =
                (ISearchModelTesterForSearchSet)_container.GetInstance(searchModelTesterType);

            _autoTestOptions.InitializeSearchTester?.Invoke(searchModelTester);

            return searchModelTester;
        }

        // This method exists solely for AuditLogEntryController because it has two
        // different index actions. 
        protected void RunActionCanPerformSearchWithSearchModel(string action, Action<dynamic> actionCallback)
        {
            var searchModelTester = GenerateSearchModelTesterForAction(action);

            // NOTE: This is slower than directly calling the Repository method, but it ensures
            // that this test is calling any override repository methods, along with any other
            // possible search configurations, without having to configure it for each test.
            searchModelTester.SearchCallback = actionCallback;

            try
            {
                searchModelTester.TestAllProperties();
            }
            catch (Exception e)
            {
                // Are you seeing this fail because of violating constraints? Then it's probably because you have
                // a search property for an entity reference, but the actual entity property is a string/int/something else.
                // You need to add the property manually to the searchModelTester.TestPropertyValues dictionary.
                Assert.Fail($"{_controllerTypeFullName}.Index failed at searching. {e}");
            }
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            ActionResult result = null;
            var searchModelTester = GenerateSearchModelTesterForAction("Index");
            searchModelTester.SearchCallback = (searchModel) => {
                _target.ModelState.AddModelError("Error", "ERRRROOOOR");
                result = _target.AsDynamic().Index(searchModel);
            };

            searchModelTester.TestBlankSearchAndGetResult();

            MvcAssert.RedirectsToRoute(result, new {action = "Search", controller = _thisControllerName},
                $"{_controllerTypeFullName}.Index did not redirect to Search when modelstate was invalid.");
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            ActionResult result = null;
            var searchModelTester = GenerateSearchModelTesterForAction("Index");
            searchModelTester.SearchCallback = (searchModel) => { result = _target.AsDynamic().Index(searchModel); };

            searchModelTester.TestBlankSearchAndGetResult();

            if (_autoTestOptions.IndexDisplaysViewWhenNoResults)
            {
                ResultIsIndexView(result);
            }
            else
            {
                MvcAssert.RedirectsToRoute(result, new {action = "Search", controller = _thisControllerName},
                    $"{_controllerTypeFullName}.Index did not redirect to Search when there are no results.");
            }
        }

        #endregion

        #region New

        private enum NewTestType
        {
            CanNotTest,
            NoParameter,
            SingleNullableParameter,
            SingleViewModelParameter
        }

        private static NewTestType CanRunTestForNew()
        {
            var newAction = GetActionMethodInfo("New");
            if (newAction == null)
            {
                return NewTestType.CanNotTest; // Nothing to test.
            }

            void EnsureCreateAction()
            {
                if (GetActionMethodInfo("Create") == null)
                {
                    Assert.Fail($"{_controllerTypeFullName}.New does not have a cooresponding Create action.");
                }
            }

            var newParams = newAction.GetParameters();
            if (newParams.Any())
            {
                if (newParams.All(x => x.IsOptional))
                {
                    EnsureCreateAction();
                    // Test should work if all the params are optional.
                    return NewTestType.NoParameter;
                }

                if (newParams.Count() > 1)
                {
                    Assert.Fail(
                        $"{_controllerTypeFullName}.New accepts more than one parameter. Test needs to be disabled and done manually.");
                }

                var maybeViewModelParam = newParams.Single();
                if (maybeViewModelParam.ParameterType.IsNullable())
                {
                    return NewTestType.SingleNullableParameter;
                }

                if (!_viewModelTEntityType.IsAssignableFrom(maybeViewModelParam.ParameterType))
                {
                    Assert.Fail(
                        $"{_controllerTypeFullName}.New accepts a non-optional parameter that does not inherit from {_viewModelTEntityType.FullName}>. Test needs to be disabled and done manually.");
                }
                else
                {
                    return NewTestType.SingleViewModelParameter;
                }
            }

            EnsureCreateAction();
            return NewTestType.NoParameter;
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestNewReturnsNewViewWithNewViewModel()
        {
            // TODO: Remove need for this second check
            var testType = CanRunTestForNew();
            if (testType == NewTestType.CanNotTest)
            {
                AssertInconclusiveForGhostTest();
            }

            var expectedViewName = _autoTestOptions.ExpectedNewViewName ?? "New";

            ActionResult result = null;

            switch (testType)
            {
                case NewTestType.CanNotTest:
                    return; // Do nothing, can't perform a test for this action.

                case NewTestType.NoParameter:
                case NewTestType.SingleNullableParameter:

                    result = (ActionResult)((testType == NewTestType.SingleNullableParameter)
                        ? _target.AsDynamic().New(null)
                        : _target.AsDynamic().New());
                    MvcAssert.IsViewNamed(result, expectedViewName,
                        $"{_controllerTypeFullName}.New did not return the expected ViewResult. If the expected view name is incorrect, this can be configured at the test level by setting ExpectedNewViewName.");

                    var model = ((ViewResultBase)result).Model;
                    Assert.IsNotNull(model, $"{_controllerTypeFullName}.New must have a model instance.");
                    Assert.IsInstanceOfType(model, _viewModelTEntityType,
                        $"{_controllerTypeFullName}.New did not return a model that inherits {_viewModelTEntityType.FullName}>.");

                    var newModelType = GetActionMethodInfo("Create").GetParameters().Single().ParameterType;
                    Assert.IsInstanceOfType(model, newModelType,
                        $"{_controllerTypeFullName}.New did not return the model type({newModelType.Name}) that the Create action expects.");

                    break;

                case NewTestType.SingleViewModelParameter:
                    var paramType = GetActionMethodInfo("New").GetParameters().Single().ParameterType;
                    // This must be set to dynamic or else calling New() with it fails.
                    // I think it's because the dynamic binding tries to call the method
                    // as if the model was cast to object rather than its actual type. The
                    // error it throws doesn't explicitly say what's wrong.
                    dynamic expectedModel = _container.GetInstance(paramType);
                    result = (ActionResult)_target.AsDynamic().New(expectedModel);
                    MvcAssert.IsViewWithNameAndModel(result, expectedViewName, expectedModel,
                        $"{_controllerTypeFullName}.New did not return the expected ViewResult or model.");

                    break;

                default:
                    throw new NotImplementedException();
            }

            ShouldMaybeBePartialView(result, _autoTestOptions.NewReturnsPartialView, "New");
        }

        #endregion

        #region Search

        private enum SearchTestType
        {
            CanNotTest,
            WithParam,
            WithoutParam
        }

        private static SearchTestType CanRunTestForSearch()
        {
            var searchAction = GetActionMethodInfo("Search");
            if (searchAction == null)
            {
                return SearchTestType.CanNotTest; // Nothing to test.
            }

            var indexAction = GetActionMethodInfo("Index");
            if (indexAction == null)
            {
                Assert.Fail($"{_controllerTypeFullName}.Search does not have a cooresponding Index action.");
            }

            if (!indexAction.GetParameters().Any())
            {
                Assert.Fail(
                    $"{_controllerTypeFullName}.Search does not have a cooresponding Index action that accepts any parameters.");
            }

            var searchParams = searchAction.GetParameters();
            if (!searchParams.Any())
            {
                return SearchTestType.WithoutParam;
            }

            if (searchParams.Count() > 1)
            {
                Assert.Fail($"{_controllerTypeFullName}.Search accepts more than one parameter.");
            }

            var searchParamModelType = searchParams.Single().ParameterType;
            var indexModelType = indexAction.GetParameters().Single().ParameterType;

            if (searchParamModelType != indexModelType)
            {
                Assert.Fail(
                    $"{_controllerTypeFullName}.Search accepts a different type({searchParamModelType.FullName}) than the Index action accepts({indexModelType.FullName}).");
            }

            return SearchTestType.WithParam;
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestSearchReturnsSearchViewWithModel()
        {
            // We have two variations on Search.
            // 1. No parameter - create our own search model
            // 2. Pass in search model - used with view.
            // These both need to function, or we go through and replace everything that does #1 with #2.

            switch (CanRunTestForSearch())
            {
                case SearchTestType.CanNotTest:
                    return; // Nothing to test

                case SearchTestType.WithoutParam:
                    var expectedModelType = GetActionMethodInfo("Index").GetParameters().Single().ParameterType;

                    var result = (ViewResult)_target.AsDynamic().Search();

                    Assert.IsInstanceOfType(result.Model, expectedModelType,
                        $"{_controllerTypeFullName}.Search did not return a view with model of type {expectedModelType.FullName}. Instead returned {result.Model?.GetType().FullName}");

                    MvcAssert.IsViewNamed(result, "Search",
                        $"{_controllerTypeFullName}.Search did not return view with expected view name.");
                    break;

                case SearchTestType.WithParam:
                    var searchModelType = GetActionMethodInfo("Search").GetParameters().Single().ParameterType;
                    dynamic expectedSearchModel = _container.GetInstance(searchModelType);

                    var result2 = (ViewResult)_target.AsDynamic().Search(expectedSearchModel);

                    MvcAssert.IsViewWithNameAndModel(result2, "Search", expectedSearchModel,
                        $"{_controllerTypeFullName}.Search did not return view with expected view name or model.");

                    break;
            }
        }

        #endregion

        #region Show

        private static bool CanRunTestForShow()
        {
            var showAction = GetActionMethodInfo("Show");
            if (showAction == null)
            {
                return false; // Nothing to test.
            }

            var showParams = showAction.GetParameters();
            if (showParams.Length >= 1 && showParams.First().ParameterType != _intType)
            {
                // Show actions that have extra parameters shouldn't fail as long
                // as there's still an int id param.
                Assert.Fail($"{_controllerTypeFullName}.Show does not have its first parameter as an int.");
            }
            else if (!showParams.Any())
            {
                Assert.Fail($"{_controllerTypeFullName}.Show does not have any parameters.");
            }

            return true;
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            // 1. See if the test can be ran
            // 2. The actual testing
            // The rest of this can just be done with dynamics
            var result = (ActionResult)_target.AsDynamic().Show(0);
            MvcAssert.IsNotFound(result, null,
                $"{_controllerTypeFullName}.Show did not return the expected NotFoundResult.");
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // 1. See if the test can be ran
            // 2. The actual testing
            var entity = CreateEntityForAutomatedTests();

            // The rest of this can just be done with dynamics
            var result = (ActionResult)_target.AsDynamic().Show(entity.Id);

            // NOTE: If this is failing because the view name is incorrect, you probably need to set ExpectedShowViewName. 
            // This will happen because:
            //   - The controller inherits from EntityLookupControllerBase. This is MapCallMVC specific.
            //   - The controller has ActionHelperViewVirtualPathFormat

            // NOTE: If this fails because the model is missing or unexpected
            //   - Make sure there's not a NotFound override that's still returning a view. This is common in Contractors for some reason.

            // NOTE: If this fails with a 404:
            //   - The test might need to have AutomaticTestOptions.CreateValidEntity set
            //   - The TEntity for the controller test may be set to the wrong type.
            var expectedViewName = _autoTestOptions.ExpectedShowViewName ?? "Show";
            MvcAssert.IsViewWithNameAndModel(result, expectedViewName, entity,
                $"{_controllerTypeFullName}.Show failed. If the expected view name is incorrect, this can be configured at the test level by setting ExpectedShowViewName.");
            ShouldMaybeBePartialView(result, _autoTestOptions.ShowReturnsPartialView, "Show");
        }

        #endregion

        #region Update

        private static bool CanRunTestForUpdate()
        {
            var updateAction = GetActionMethodInfo("Update");
            if (updateAction == null)
            {
                return false; // Nothing to test
            }

            var updateParams = updateAction.GetParameters();
            if (updateParams.Length != 1)
            {
                Assert.Fail($"{_controllerTypeFullName}.Update does not have exactly one parameter.");
            }

            var viewModelType = updateParams.Single().ParameterType;
            if (!_viewModelTEntityType.IsAssignableFrom(viewModelType))
            {
                Assert.Fail(
                    $"{_controllerTypeFullName}.Update's view model parameter does not inherit from {_viewModelTEntityType.FullName}>.");
            }

            return true;
        }

        private dynamic CreateValidViewModelForUpdateAction()
        {
            var validEntity = (TEntity)CreateEntityForAutomatedTests();
            var viewModelType = _autoTestOptions.UpdateViewModelType ??
                                GetActionMethodInfo("Update")
                                   .GetParameters()
                                    // This might need to be First instead of Single.
                                   .Single()
                                   .ParameterType;
            // TODO: This should probably be done with ViewModelFactory but atm it only works with generic parameters.
            // 1. Create a view model that should be valid for creation.
            // NOTE: viewModel must be dynamic in order to properly pass it to the Update method dynamically.
            dynamic viewModel = _container.GetInstance(viewModelType);
            viewModel.Map(validEntity);
            _autoTestOptions.InitializeUpdateViewModel?.Invoke(viewModel);

            // This is a sanity check that we're definitely creating a valid view model
            // to pass to the Update action. If you're seeing this fail, you likely to 
            // override the InitialUpdateViewModel test option.
            ValidationAssert.ModelStateIsValid(viewModel);
            return viewModel;
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            // There needs to be two assertions here
            // 1. That the changes were actually *saved*
            //    - This could be done by evicting the entity and querying for it again to make sure changes were persisted.
            // 2. That a change to a view model property was copied to the entity.

            // NOTE: viewModel must be dynamic in order to properly pass it to the Update method dynamically.
            //dynamic viewModel = CreateValidViewModelForUpdateAction();
            //var id = viewModel.Id;

            //// 2. Pass it to update
            //InitializeRedirectToReferrer(_autoTestOptions.UpdateRedirectsToReferrerOnSuccess);
            //var result = (ActionResult)_target.AsDynamic().Update(viewModel);

            // Preferrably, we should be able to test this without mocking anything.
            Assert.Inconclusive("Figure out how to automatically test that changes have been persisted.");
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            _autoTestOptions.InitializeTestForCreateAndUpdateTests();
            // NOTE: viewModel must be dynamic in order to properly pass it to the Update method dynamically.
            dynamic viewModel = CreateValidViewModelForUpdateAction();
            var id = viewModel.Id;

            // 2. Pass it to update
            InitializeRedirectToReferrer(_autoTestOptions.UpdateRedirectsToReferrerOnSuccess);
            var result = (ActionResult)_target.AsDynamic().Update(viewModel);

            if (_autoTestOptions.UpdateRedirectsToReferrerOnSuccess)
            {
                MvcAssert.RedirectsToUrl(result, URL_REFERRER + _autoTestOptions.UpdateRedirectSuccessUrlFragment,
                    $"{_controllerTypeFullName}.Update did not redirect to expected url after successfully updating record.");
            }
            else if (_autoTestOptions.UpdateReturnsPartialShowViewOnSuccess)
            {
                MvcAssert.IsPartialView(result, $"{_controllerTypeFullName}.Update did not return a partial view.");

                var expectedModel = Repository.Find(viewModel.Id);
                MvcAssert.IsViewWithNameAndModel(result, _autoTestOptions.ExpectedShowViewName ?? "_Show",
                    expectedModel,
                    $"{_controllerTypeFullName}.Update did not return a expected view and/or expected model.");
            }
            else
            {
                // NOTE: Is this test failing because the id values don't match? Then it's probably
                // because the controller is redirecting to a different controller.
                var controller = _autoTestOptions.ExpectedUpdateRedirectControllerName ?? _thisControllerName;
                // TODO: This should potentially test for area, but it's not supplied often. The area is assumed from the current request in MVC.
                var routeArgs = _autoTestOptions.UpdateRedirectsToRouteOnSuccessArgs?.Invoke(viewModel) ??
                                new {action = "Show", controller = controller, id = id};
                MvcAssert.RedirectsToRoute(result, routeArgs,
                    $"{_controllerTypeFullName}.Update did not set redirect to the expected route.");
            }
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestUpdateReturnsEditViewWithModelWhenThereAreModelStateErrors()
        {
            _autoTestOptions.InitializeTestForCreateAndUpdateTests();
            // NOTE: viewModel must be dynamic in order to properly pass it to the Create method dynamically.
            dynamic viewModel = CreateValidViewModelForUpdateAction();

            _target.ModelState.AddModelError("Auto test error", "for Update action");

            InitializeRedirectToReferrer(_autoTestOptions.UpdateRedirectsToReferrerOnError);
            // 2. Pass it to create
            var result = (ActionResult)_target.AsDynamic().Update(viewModel);

            if (_autoTestOptions.UpdateRedirectsToReferrerOnError)
            {
                MvcAssert.RedirectsToUrl(result, URL_REFERRER + _autoTestOptions.UpdateRedirectErrorUrlFragment,
                    $"{_controllerTypeFullName}.Update did not redirect to expected url after experiencing modelstate errors.");
            }
            else
            {
                if (_autoTestOptions.UpdateRedirectsToRouteOnErrorArgs != null)
                {
                    MvcAssert.RedirectsToRoute(result,
                        _autoTestOptions.UpdateRedirectsToRouteOnErrorArgs.Invoke(viewModel),
                        $"{_controllerTypeFullName}.Update did not set redirect to the expected route.");
                }
                else
                {
                    var expectedViewName = _autoTestOptions.ExpectedEditViewName ?? "Edit";
                    MvcAssert.IsViewWithNameAndModel(result, expectedViewName, viewModel,
                        $"{_controllerTypeFullName}.Update did not return Edit view with expected model.");
                }
            }
        }

        [TestMethodWithIgnoreIfSupport, IgnoreIf(nameof(ShouldIgnoreControllerTest))]
        public virtual void TestUpdateReturnsNotFoundIfRecordBeingUpdatedDoesNotExist()
        {
            _autoTestOptions.InitializeTestForCreateAndUpdateTests();
            dynamic updateModel = CreateValidViewModelForUpdateAction();
            updateModel.Id = 0; // Need to reset the Id to something that doesn't exist.
            var result = (ActionResult)_target.AsDynamic().Update(updateModel);
            MvcAssert.IsNotFound(result, null,
                $"{_controllerTypeFullName}.Update did not return the expected NotFoundResult.");
        }

        #endregion

        #endregion

        #region Required Tests

        public abstract void TestControllerAuthorization();

        /// <summary>
        /// Asserts that an action requires SecureForm. This is only needed if
        /// you have an action that explicitly requires secure forms(ie it has
        /// RequiresSecureForm(true)). 
        /// </summary>
        /// <param name="virtualRequestPath"></param>
        public void RequiresSecureForm(string virtualRequestPath)
        {
            InitializeControllerAndRequest(virtualRequestPath);
            Assert.IsTrue(FormBuilder.RequiresSecureForm(Request.RouteContext));
        }

        /// <summary>
        /// Asserts that an action does not require SecureForm. This is only needed
        /// it you have an action that explicitly does not require secure forms(ie it has
        /// RequiresSecureForm(false)).
        /// </summary>
        /// <param name="virtualRequestPath"></param>
        public void DoesNotRequireSecureForm(string virtualRequestPath)
        {
            InitializeControllerAndRequest(virtualRequestPath);
            Assert.IsFalse(FormBuilder.RequiresSecureForm(Request.RouteContext));
        }

        #endregion

        /// <summary>
        /// Configuration class for determining if automated tests should be ran. All properties
        /// default to true. Set properties to false to disable these tests from running.
        ///
        /// NOTE: If you disable a test, there must be a similar test written in the test class.
        /// If you find yourself copy/pasting tests from elsewhere that have these flags set to
        /// false, ask yourself if you should be doing that. There may be a better way to write
        /// your action.
        ///
        /// NOTE 2: You DO NOT need to set any test to false for actions that do not exist on
        /// a given controller. Those tests will be skipped regardless.
        /// </summary>
        public class AutomatedTestOptions
        {
            #region All tests

            /// <summary>
            /// Set this if you need to use a specific entity that won't return a 404. This
            /// function should always create a *new* entity that the current user can access.
            /// Do not return a cached instance.
            /// Multiple tests use this and a single test may need to create multiple entities.
            /// This is used for creating initial view models for Create and Update actions as well.
            /// </summary>
            public Func<IEntity> CreateValidEntity { get; set; }

            #endregion

            /// <summary>
            /// Performs extra initialization steps prior to the Create and Update tests
            /// that run model validation. You only need to override this if the test
            /// you're using is failing validation due to user access problems.
            /// </summary>
            public Action InitializeTestForCreateAndUpdateTests { get; set; }

            #region Create tests

            public bool CreateRedirectsToReferrerOnError { get; set; }
            public string CreateRedirectErrorUrlFragment { get; set; }
            public bool CreateRedirectsToReferrerOnSuccess { get; set; }
            public string CreateRedirectSuccessUrlFragment { get; set; }
            public Func<dynamic, object> CreateRedirectsToRouteOnErrorArgs { get; set; }
            public Func<dynamic, object> CreateRedirectsToRouteOnSuccessArgs { get; set; }

            /// <summary>
            /// This mainly exists for Contractors due to the use of ajaxtables all over the place.
            /// </summary>
            public bool CreateReturnsPartialShowViewOnSuccess { get; set; }

            /// <summary>
            /// This property only exists for EntityLookupControllerBase. That controller
            /// requires a generic parameter to be used, and that generic param is what
            /// defines the controller name.
            /// Do not use this otherwise.
            /// </summary>
            public string ExpectedCreateRedirectControllerName { get; set; }

            /// <summary>
            /// Optional action for initializing properties on the CREATE view model.
            /// Setting values may be necessary to prevent 
            /// </summary>
            public Action<object> InitializeCreateViewModel { get; set; }

            #endregion

            #region Destroy tests

            public bool DestroyRedirectsToReferrerOnError { get; set; }
            public string DestroyRedirectErrorUrlFragment { get; set; }
            public bool DestroyRedirectsToReferrerOnSuccess { get; set; }
            public string DestroyRedirectSuccessUrlFragment { get; set; }
            public Func<dynamic, object> DestroyRedirectsToRouteOnErrorArgs { get; set; }
            public Func<dynamic, object> DestroyRedirectsToRouteOnSuccessArgs { get; set; }
            public bool DestroyReturnsHttpStatusCodeNoContentOnSuccess { get; set; }

            #endregion

            #region Edit tests

            /// <summary>
            /// This needs to be set on rare occasions where the view name is not exactly "Edit".
            /// ex: using ActionHelperViewVirtualPathFormat
            /// </summary>
            public string ExpectedEditViewName { get; set; }

            public bool EditReturnsPartialView { get; set; }

            #endregion

            #region Index tests

            /// <summary>
            /// This needs to be set on rare occasions where the view name is not exactly "Index".
            /// ex: using ActionHelperViewVirtualPathFormat
            /// </summary>
            public string ExpectedIndexViewName { get; set; }

            public bool IndexRedirectsToShowForSingleResult { get; set; }
            public bool IndexDisplaysViewWhenNoResults { get; set; }

            #endregion

            #region New tests

            /// <summary>
            /// This needs to be set on rare occasions where the view name is not exactly "New".
            /// ex: using ActionHelperViewVirtualPathFormat
            /// </summary>
            public string ExpectedNewViewName { get; set; }

            public bool NewReturnsPartialView { get; set; }

            #endregion

            #region Search tests

            /// <summary>
            /// For the occasional test that needs to configure the search tester. This should be
            /// preferred to overriding the actual search test.
            ///
            /// DON'T CALL TestAllProperties IN THIS!
            /// </summary>
            public Action<ISearchModelTesterForSearchSet> InitializeSearchTester { get; set; }

            #endregion

            #region Show tests

            /// <summary>
            /// This needs to be set on rare occasions where the view name is not exactly "Show".
            /// ex: using ActionHelperViewVirtualPathFormat
            /// </summary>
            public string ExpectedShowViewName { get; set; }

            public bool ShowReturnsPartialView { get; set; }

            #endregion

            #region Update tests

            public bool UpdateRedirectsToReferrerOnError { get; set; }
            public string UpdateRedirectErrorUrlFragment { get; set; }
            public bool UpdateRedirectsToReferrerOnSuccess { get; set; }
            public string UpdateRedirectSuccessUrlFragment { get; set; }
            public Func<dynamic, object> UpdateRedirectsToRouteOnErrorArgs { get; set; }
            public Func<dynamic, object> UpdateRedirectsToRouteOnSuccessArgs { get; set; }

            /// <summary>
            /// This mainly exists for Contractors due to the use of ajaxtables all over the place.
            /// </summary>
            public bool UpdateReturnsPartialShowViewOnSuccess { get; set; }

            /// <summary>
            /// The edit and update tests both check the Update action for
            /// a single parameter type. Set this to false if the Update action
            /// has multiple parameters.
            /// </summary>
            public bool DoUpdateSingleViewModelParameterCheck { get; set; } = true;

            /// <summary>
            /// This property only exists for EntityLookupControllerBase. That controller
            /// requires a generic parameter to be used, and that generic param is what
            /// defines the controller name.
            /// Do not use this otherwise.
            /// </summary>
            public string ExpectedUpdateRedirectControllerName { get; set; }

            /// <summary>
            /// Optional action for initializing properties on the UPDATE view model.
            /// Setting values may be necessary to prevent 
            /// </summary>
            public Action<object> InitializeUpdateViewModel { get; set; }
            
            /// <summary>
            /// Optional <see cref="Type"/> to override the default behavior of deriving the type from the
            /// parameter to the Update action.
            /// </summary>
            public Type UpdateViewModelType { get; set; }

            #endregion
        }
    }

    /// <typeparam name="TMvcApplication">This needs to be the MvcApplication type for the site project being tested. It is important for the MvcApplicationTester to function correctly.</typeparam>
    public abstract class ControllerTestBase<TMvcApplication, TController, TEntity> :
        ControllerTestBase<TMvcApplication, TController, TEntity, RepositoryBase<TEntity>>
        where TMvcApplication : MvcApplication, new()
        where TController : ControllerBase
        where TEntity : class, new() { }
}
