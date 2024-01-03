using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.ErrorHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using StructureMap;

namespace MMSINC.CoreTest.Utilities.ErrorHandling
{
    /// <summary>
    /// Summary description for ErrorModuleTest.
    /// </summary>
    [TestClass]
    public class ErrorModuleTest : EventFiringTestClass
    {
        #region Private Members

        private TestErrorModule _target;
        private IHttpApplicationWrapper _application;
        private IErrorEmailer _errorEmailer;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void ErrorModuleTestInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _application)
               .DynamicMock(out _errorEmailer);

            _target = new TestErrorModuleBuilder()
                     .WithHttpApplication(_application)
                     .WithErrorEmailer(_errorEmailer)
                     .Build();
        }

        [TestCleanup]
        public void ErrorModuleTestCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestConstructorCreatesHttpApplicationWrapperAndErrorEmailer()
        {
            // Need this empty Record statement so that the TestCleanup doesn't
            // fail when it calls VerifyAll.
            using (_mocks.Record()) { }

            // Also to get this test to pass without blowing up due to the rhino mocks
            // being used for the other tests, we need to make new instances and inject
            // them into the container.
            var app = new HttpApplicationWrapper(_container);
            var emailer = new ErrorEmailer();

            _container.Inject(app);
            _container.Inject(emailer);

            var target = new TestErrorModule();
            Assert.AreSame(app, target.HttpApplication);
            Assert.AreSame(emailer, target.ErrorEmailer);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestInitCreatesHttpApplicationWrapperAndAttachesToErrorEvent()
        {
            var context = _mocks.DynamicMock<HttpApplication>();

            using (_mocks.Record())
            {
                _application.Application = context;

                _application.Error += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.Init(context);
            }
        }

        [TestMethod]
        public void TestDisposeCallsDisposeOnApplication()
        {
            using (_mocks.Record())
            {
                _application.Dispose();
            }

            using (_mocks.Playback())
            {
                _target.Dispose();
            }
        }

        [TestMethod]
        public void TestApplication_ErrorOnlyEmailsErrorIfServerIsNotLocalhost()
        {
            var context = _mocks.CreateMock<IHttpContext>();
            var request = _mocks.CreateMock<IRequest>();
            var serverVariables = new NameValueCollection {
                {ErrorModule.SERVER_NAME_KEY, "www.somesite.com"}
            };

            using (_mocks.Record())
            {
                SetupResult.For(_application.CurrentContext).Return(context);
                SetupResult.For(context.Request).Return(request);
                SetupResult.For(request.ServerVariables).Return(serverVariables);
                _errorEmailer.SendEmail(context);
            }

            using (_mocks.Playback())
            {
                _target.CallApplication_Error(null, null);
            }

            _mocks.BackToRecordAll();

            serverVariables = new NameValueCollection {
                {ErrorModule.SERVER_NAME_KEY, ErrorModule.LOCALHOST}
            };

            using (_mocks.Record())
            {
                SetupResult.For(_application.CurrentContext).Return(context);
                SetupResult.For(context.Request).Return(request);
                SetupResult.For(request.ServerVariables).Return(serverVariables);
                DoNotExpect.Call(() => _errorEmailer.SendEmail(context));
            }

            using (_mocks.Playback())
            {
                _target.CallApplication_Error(null, null);
            }
        }
    }

    internal class TestErrorModuleBuilder : TestDataBuilder<TestErrorModule>
    {
        #region Private Members

        private IHttpApplicationWrapper _application;
        private IErrorEmailer _errorEmailer;

        #endregion

        #region Exposed Methods

        public override TestErrorModule Build()
        {
            var obj = new TestErrorModule();
            if (_application != null)
                obj.HttpApplication = _application;
            if (_errorEmailer != null)
                obj.ErrorEmailer = _errorEmailer;
            return obj;
        }

        public TestErrorModuleBuilder WithHttpApplication(IHttpApplicationWrapper application)
        {
            _application = application;
            return this;
        }

        public TestErrorModuleBuilder WithErrorEmailer(IErrorEmailer errorEmailer)
        {
            _errorEmailer = errorEmailer;
            return this;
        }

        #endregion
    }

    internal class TestErrorModule : ErrorModule
    {
        #region Properties

        public IHttpApplicationWrapper HttpApplication
        {
            get { return _httpApplication; }
            set { _httpApplication = value; }
        }

        public IErrorEmailer ErrorEmailer
        {
            get { return _errorEmailer; }
            set { _errorEmailer = value; }
        }

        #endregion

        #region Exposed Methods

        public void CallApplication_Error(Object sender, EventArgs e)
        {
            Application_Error(sender, e);
        }

        #endregion
    }
}
