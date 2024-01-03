using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MMSINC.Core.MvcTest.Controllers
{
    [TestClass]
    public class ControllerBaseTest
    {
        #region Private Members

        private Mock<IRepository<TestUser>> _userRepo;
        private TestController1 _target1;

        private IContainer _container;
        //private RequestContext _requestContext;
        //private UrlHelper _urlHelper;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void ControllerBaseTestInitialize()
        {
            _container = new Container(e => { e.For<IViewModelFactory>().Use<ViewModelFactory>(); });
            _container.Inject(new Mock<ISession>().Object);
            _container.Inject((_userRepo = new Mock<IRepository<TestUser>>()).Object);

            _target1 = _container.GetInstance<TestController1>();
        }

        #endregion

        #region Repository

        [TestMethod]
        public void TestShouldProvideRepositoryBaseWithSingleTypeArgument()
        {
            var repoMock = new Mock<IRepository<TestUser>>();
            _container.Inject(repoMock.Object);
            var target = _container.GetInstance<TestController>();

            Assert.AreSame(repoMock.Object, target.Repository);
        }

        [TestMethod]
        public void TestShouldProvideRepositoryOfSpecifiedTypeWithTwoTypeArguments()
        {
            var target = _container.GetInstance<TestController>();

            Assert.AreSame(_userRepo.Object, target.Repository);
        }

        [TestMethod]
        public void TestShouldCacheProvidedRepository0()
        {
            var repoMock = new Mock<IRepository<TestUser>>();
            _container.Inject(repoMock.Object);
            var target = _container.GetInstance<TestController>();

            Assert.AreSame(repoMock.Object, target.Repository);
            Assert.AreSame(repoMock.Object, target.Repository);
        }

        [TestMethod]
        public void TestShouldCacheProvidedRepository1()
        {
            var repository = _container.With(new Mock<ISession>().Object).GetInstance<TestRepository>();
            _container.Inject(repository);
            var target = _container.GetInstance<TestController1>();

            Assert.AreSame(repository, target.Repository);
            Assert.AreSame(repository, target.Repository);
        }

        // TODO: who is this test, and what does it do?
        //[TestMethod]
        //public void TestNotShouldProvideRepositoryBaseWithSingleTypeArgumentWhenTypeArgumentIsTypeArgument()
        //{
        //    MyAssert.Throws<TypeParameterException>(() => {
        //        var target = new BadTestController();
        //        var repoMock = new Mock<IRepository<TestUser>>();
        //        _container.Container.Inject(repoMock.Object);

        //        // This should throw the exception
        //        Assert.AreSame(repoMock.Object, target.Repository);
        //    });
        //}

        #endregion

        #region DisplayErrorMessage(string message)

        [TestMethod]
        public void TestDisplayErrorMessageSetsTempDataWithErrorMessageKey()
        {
            var theMessage = "this is the message";

            _target1.CallDisplayErrorMessage(theMessage);

            var result = ((List<string>)_target1.TempData[ControllerBase.ERROR_MESSAGE_KEY]).Single();

            Assert.AreEqual(theMessage, result);
        }

        #endregion

        #region DisplayModelStateErrors(string message)

        [TestMethod]
        public void TestDisplayModelStateErrorsDisplaysAllModelStateErrors()
        {
            var errors = new Dictionary<string, string> {
                {"foo", "foo error"},
                {"bar", "bar error"},
                {"baz", "baz error"}
            };
            errors.Each(e => _target1.ModelState.AddModelError(e.Key, e.Value));

            _target1.CallDisplayModelStateErrors();

            var displayErrors = (List<string>)_target1.TempData[ControllerBase.ERROR_MESSAGE_KEY];

            Assert.IsTrue(displayErrors.Contains("foo error"));
            Assert.IsTrue(displayErrors.Contains("bar error"));
            Assert.IsTrue(displayErrors.Contains("baz error"));
        }

        [TestMethod]
        public void TestDisplayModelStateErrorsDoesNothingIfNoModelStateErrorsExist()
        {
            _target1.CallDisplayModelStateErrors();

            Assert.IsNull(_target1.TempData[ControllerBase.ERROR_MESSAGE_KEY]);
        }

        #endregion

        [TestMethod]
        public void TestGetUrlForModelReturnsUrlForModel()
        {
            using (var app = new FakeMvcApplicationTester(_container))
            {
                var model = new TestModelWithId {Id = 5};
                var request = app.CreateRequestHandler();
                var urlHelper = new UrlHelper(request.RequestContext);
                _target1 = request.CreateAndInitializeController<TestController1>();
                _target1.Url = urlHelper;

                var result = _target1.GetUrlForModel(model, "Show", "TestController1");

                Assert.AreEqual("http://localhost/TestController1/Show/5", result);
            }
        }

        [TestMethod]
        public void TestRedirectToRouteWithTabSelectedRedirectsToUrlForModelWithFragmentId()
        {
            using (var app = new FakeMvcApplicationTester(_container))
            {
                var model = new TestModelWithId {Id = 5};
                var request = app.CreateRequestHandler();
                var urlHelper = new UrlHelper(request.RequestContext);
                _target1 = request.CreateAndInitializeController<TestController1>();
                _target1.Url = urlHelper;

                var result =
                    _target1.RedirectToRouteWithTabSelected(
                        new {Id = 5, Action = "Show", Controller = "TestController1", Area = ""},
                        "#foo") as RedirectResult;

                Assert.AreEqual("/TestController1/Show/5#foo", result.Url);

                // in case they forget to include a #
                result = _target1.RedirectToRouteWithTabSelected(
                    new {Id = 5, Action = "Show", Controller = "TestController1", Area = ""}, "foo") as RedirectResult;

                Assert.AreEqual("/TestController1/Show/5#foo", result.Url);
            }
        }

        [TestMethod]
        public void TestGetMapUrlForModelReturnsUrlForModelMap()
        {
            using (var app = new FakeMvcApplicationTester(_container))
            {
                var model = new TestModelWithId {Id = 5};
                var request = app.CreateRequestHandler();
                var urlHelper = new UrlHelper(request.RequestContext);
                _target1 = request.CreateAndInitializeController<TestController1>();
                _target1.Url = urlHelper;

                var result = _target1.GetMapUrlForModel(model, "Show", "TestController1");

                Assert.AreEqual(
                    "http://localhost/Modules/mvc/Map?ControllerName=TestController1&ActionName=Show&AreaName=&Search%5Bid%5D=5",
                    result);
            }
        }
    }

    public class TestController : MMSINC.Controllers.ControllerBase<TestUser>
    {
        public TestController(ControllerBaseArguments<IRepository<TestUser>, TestUser> args) : base(args) { }
    }

    public class TestController1 : MMSINC.Controllers.ControllerBase<TestRepository, TestUser>
    {
        #region Exposed Methods

        public TestController1(ControllerBaseArguments<TestRepository, TestUser> args) : base(args) { }

        public void CallDisplayErrorMessage(string message)
        {
            DisplayErrorMessage(message);
        }

        public void CallDisplayModelStateErrors()
        {
            DisplayModelStateErrors();
        }

        #endregion
    }

    public class TestRepository : RepositoryBase<TestUser>
    {
        #region Private Members

        private ICriteria _criteriaObj;
        private AbstractCriterion _idMatchesCriterion;
        private IQueryable<TestUser> _linq;

        #endregion

        #region Properties

        public override ICriteria Criteria => _criteriaObj ?? base.Criteria;

        public override IQueryable<TestUser> Linq => _linq ?? base.Linq;

        #endregion

        #region Constructors

        public TestRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IQueryable<Object> GetLinqProperty()
        {
            return Linq;
        }

        public void SetLinqProperty(IQueryable<TestUser> linq)
        {
            _linq = linq;
        }

        public ICriteria GetCriteriaObj()
        {
            return Criteria;
        }

        public void SetCriteriaObj(ICriteria criteria)
        {
            _criteriaObj = criteria;
        }

        public void SetIdMatchesCriterion(AbstractCriterion criterion)
        {
            _idMatchesCriterion = criterion;
        }

        #endregion

        #region Private Methods

        protected override AbstractCriterion GetIdEqCriterion(int id)
        {
            return _idMatchesCriterion ?? base.GetIdEqCriterion(id);
        }

        #endregion
    }

    public class TestModelWithId
    {
        public int Id { get; set; }
    }
}
