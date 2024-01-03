using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class SecureFormValueProviderFactoryTest
    {
        #region Fields

        private FakeMvcApplicationTester _app;
        private FakeMvcHttpHandler _request;
        private SecureFormValueProviderFactory<SecureFormToken, SecureFormDynamicValue> _target;
        private Mock<ITokenRepository<SecureFormToken, SecureFormDynamicValue>> _repo;
        private ControllerContext _controllerContext;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _app = new FakeMvcApplicationTester(_container);

            var controller = new FakeCrudController();
            _app.ControllerFactory.RegisterController(controller);
            _request = _app.CreateRequestHandler("~/FakeCrud/Create/");
            _controllerContext = _request.CreateControllerContext(controller);
            _target = _container.GetInstance<SecureFormValueProviderFactory<SecureFormToken, SecureFormDynamicValue>>();

            _repo = new Mock<ITokenRepository<SecureFormToken, SecureFormDynamicValue>>();
            _container.Inject(_repo.Object);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _app.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetValueProviderReturnsProviderWithTokenWhenAValidTokenExistsForTheRequest()
        {
            var token = new SecureFormToken();
            var guid = Guid.NewGuid();
            _repo.Setup(x => x.FindByToken(guid)).Returns(token);
            _request.RequestForm[FormBuilder.SECURE_FORM_HIDDEN_FIELD_NAME] = guid.ToString();

            var result =
                (SecureFormValueProvider<SecureFormToken, SecureFormDynamicValue>)_target.GetValueProvider(
                    _controllerContext);
            Assert.IsNotNull(result);
            Assert.AreSame(token, result.Token);
        }

        [TestMethod]
        public void TestGetValueProviderReturnsNullWhenAnInvalidTokenExistsForTheRequest()
        {
            _request.RequestForm[FormBuilder.SECURE_FORM_HIDDEN_FIELD_NAME] = "";

            var result =
                (SecureFormValueProvider<SecureFormToken, SecureFormDynamicValue>)_target.GetValueProvider(
                    _controllerContext);
            Assert.IsNull(result);
        }

        #endregion
    }
}
