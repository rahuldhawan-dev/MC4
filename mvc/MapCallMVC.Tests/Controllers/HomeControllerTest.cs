using System.Reflection;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Common;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest 
    {
        #region Fields

        private HomeController _target;
        private Mock<IAuthenticationRepository<User>> _authRepo;
        private Mock<IAuthenticationService<User>> _authServ;
        private FakeMvcHttpHandler _request;
        private IContainer _container;

        private readonly PropertyInfo _isProductionProp = typeof(HttpApplicationBase).GetProperty("IsProduction", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static),
                                      _isStagingProp = typeof(HttpApplicationBase).GetProperty("IsStaging", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        private bool _previousIsProductionValue,
                     _previousIsStagingValue;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _request = new FakeMvcHttpHandler(_container);
            _previousIsProductionValue = (bool)_isProductionProp.GetValue(null, new object[] { });
            _previousIsStagingValue = (bool)_isStagingProp.GetValue(null, new object[] { });

            _authRepo = new Mock<IAuthenticationRepository<User>>();
            _authServ = new Mock<IAuthenticationService<User>>();
            _container.Inject(_authRepo.Object);
            _container.Inject(_authServ.Object);
            _target = _request.CreateAndInitializeController<HomeController>();
        }

        [TestCleanup]
        public void TestCleanup()
        {

            // Need to ensure we set HttpApplicationBase's properties back to what we started with
            // since they're static and all.
            _isProductionProp.SetValue(null, _previousIsProductionValue, new object[] { });
            _isStagingProp.SetValue(null, _previousIsStagingValue, new object[] { });
        }

        #endregion

        #region Tests

        #region Index

        [TestMethod]
        public void TestIndexReturnsIndexView()
        {
            MvcAssert.IsViewNamed(_target.Index(), "Index");
        }

        [TestMethod]
        public void TestEntityLookupsReturnsEntityLookupsView()
        {
            MvcAssert.IsViewNamed(_target.EntityLookups(), "EntityLookups");
        }

        //[TestMethod]
        //public void TestIndexRendersStuff()
        //{
        //   using (var app = new MapCallMvcApplicationTester())
        //   {
        //       var razorEngine = new TestableRazorViewEngine();
        //       app.ViewEngines.Clear();
        //       app.ViewEngines.Insert(0, razorEngine);
        //       var request = app.CreateRequestHandler("~/Home/Index");
        //       request.Process();
        //   }
        //}
        

        #endregion

        #region Impersonate

        [TestMethod]
        public void TestImpersonateReturnsNotFoundInProductionMode()
        {
            _request.Request.Setup(x => x.IsLocal).Returns(true);
            _isProductionProp.SetValue(null, true, new object[] { });
            _isStagingProp.SetValue(null, false, new object[] { });
            MvcAssert.IsNotFound(_target.Impersonate("bluh"));
        }

        [TestMethod]
        public void TestImpersonateReturnsNotFoundInStagingMode()
        {
            _request.Request.Setup(x => x.IsLocal).Returns(true);
            _isProductionProp.SetValue(null, false, new object[] { });
            _isStagingProp.SetValue(null, true, new object[] { });
            MvcAssert.IsNotFound(_target.Impersonate("bluh"));
        }

        [TestMethod]
        public void TestImpersonateReturnsNotFoundIfRequestIsNotLocal()
        {
            _request.Request.Setup(x => x.IsLocal).Returns(false);
            _isProductionProp.SetValue(null, false, new object[] { });
            _isStagingProp.SetValue(null, false, new object[] { });

            MvcAssert.IsNotFound(_target.Impersonate("bluh"));
        }
        
        [TestMethod]
        public void TestImpersonateSignsInAsGivenUserInDevMode()
        {
            _request.Request.Setup(x => x.IsLocal).Returns(true);
            _isProductionProp.SetValue(null, false, new object[] { });
            _isStagingProp.SetValue(null, false, new object[] { });

            var user = new User { UserName = "a user", HasAccess = true };
            user.SetPropertyValueByName("Id", 42);
            _authRepo.Setup(x => x.GetUser("a user")).Returns(user).Verifiable();
            _authServ.Setup(x => x.SignIn(42, false));
          
            _target.Impersonate("a user");
    
            _authRepo.VerifyAll();
            _authServ.VerifyAll();
        }

        [TestMethod]
        public void TestImpersonateErrorsWhenAUserDoesntExist()
        {
            _request.Request.Setup(x => x.IsLocal).Returns(true);
            _isProductionProp.SetValue(null, false, new object[] { });
            _isStagingProp.SetValue(null, false, new object[] { });
            var result = _target.Impersonate("a user");
            MvcAssert.IsNotFound(result);
        }

        #endregion

        #endregion
    }
}
