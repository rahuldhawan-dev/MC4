using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.Core.WebFormsTest.Common
{
    /// <summary>
    /// Summary description for MvpUserControlTest.
    /// </summary>
    [TestClass]
    public class MvpUserControlTest : EventFiringTestClass
    {
        #region Private Members

        private IPage _iPage;
        private IUser _iUser;
        private IRequest _iRequest;
        private IResponse _iResponse;
        private IServer _iServer;
        private IClientScriptManager _iClientScript;
        private TestMvpUserControl _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _iPage)
               .DynamicMock(out _iResponse)
               .DynamicMock(out _iRequest)
               .DynamicMock(out _iServer)
               .DynamicMock(out _iClientScript)
               .DynamicMock(out _iUser);

            _target = new TestMvpUserControlBuilder()
                     .WithIPage(_iPage)
                     .WithIRequest(_iRequest)
                     .WithIResponse(_iResponse)
                     .WithIServer(_iServer)
                     .WithIClientScript(_iClientScript)
                     .WithIUser(_iUser);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        #region IPage

        [TestMethod]
        public void TestIPageReturnsMockedIPageInstance()
        {
            Assert.AreSame(_iPage, _target.IPage);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestIPageIsNullWhenNothingMockedAndNotRunningOnWebServer()
        {
            _target = new TestMvpUserControlBuilder();

            Assert.IsNull(_target.IPage);

            _mocks.ReplayAll();
        }

        #endregion

        #region IResponse

        [TestMethod]
        public void TestIResponseReturnsMockedIResponseInstance()
        {
            Assert.AreSame(_iResponse, _target.IResponse);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestIResponseReturnsIResponseFromIPageWhenIResponseNotMocked()
        {
            _target = new TestMvpUserControlBuilder()
               .WithIPage(_iPage);

            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IResponse).Return(_iResponse);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(_iResponse, _target.IResponse);
            }
        }

        [TestMethod]
        public void TestIResponseReturnsNullWhenNothingMockedAndNotRunningOnWebServer()
        {
            _target = new TestMvpUserControlBuilder();

            Assert.IsNull(_target.IResponse);

            _mocks.ReplayAll();
        }

        #endregion

        #region IRequest

        [TestMethod]
        public void TestIRequestReturnsMockedIRequestObject()
        {
            Assert.AreSame(_iRequest, _target.IRequest);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestIRequestReturnsIRequestFromIPageWhenIRequestNotMocked()
        {
            _target = new TestMvpUserControlBuilder()
               .WithIPage(_iPage);

            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IRequest).Return(_iRequest);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(_iRequest, _target.IRequest);
            }
        }

        [TestMethod]
        public void TestIRequestReturnsNullWhenNothingMockedAndNotRunningOnWebServer()
        {
            _target = new TestMvpUserControlBuilder();

            Assert.IsNull(_target.IRequest);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestIsMvpPostBackValueIsInjectable()
        {
            _target = new TestMvpUserControlBuilder()
               .WithIsMvpPostBack(true);

            Assert.IsTrue(_target.IsMvpPostBack);

            _target = new TestMvpUserControlBuilder()
               .WithIsMvpPostBack(false);

            Assert.IsFalse(_target.IsMvpPostBack);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRelativeUrlReturnsRelativeUrlFromIRequestObject()
        {
            var expected = "some url";
            using (_mocks.Record())
            {
                SetupResult.For(_iRequest.RelativeUrl).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.RelativeUrl);
            }
        }

        #endregion

        #region IServer

        [TestMethod]
        public void TestIServerReturnsMockedIServerInstance()
        {
            Assert.AreSame(_iServer, _target.IServer);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestIServerReturnsIServerFromIPageWhenIServerNotMocked()
        {
            _target = new TestMvpUserControlBuilder()
               .WithIPage(_iPage);

            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IServer).Return(_iServer);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(_iServer, _target.IServer);
            }
        }

        [TestMethod]
        public void TestIServerReturnsNullWhenNothingMockedAndNotRunningOnWebServer()
        {
            _target = new TestMvpUserControlBuilder();

            _mocks.ReplayAll();

            Assert.IsNull(_target.IServer);
        }

        #endregion

        #region IUser

        [TestMethod]
        public void TestIUserReturnsMockedIUserInstance()
        {
            Assert.AreSame(_iUser, _target.IUser);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestIUserReturnsWrappedInstanceFromIPageWhenNothingMocked()
        {
            _target = new TestMvpUserControlBuilder()
               .WithIPage(_iPage);

            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IUser).Return(_iUser);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(_iUser, _target.IUser);
            }
        }

        #endregion

        #region Client Script Manager

        [TestMethod]
        public void TestClientScriptManagerReturnsMockedIClientScriptManagerInstance()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_iClientScript, _target.ClientScriptManager);
        }

        [TestMethod]
        public void TestClientScriptManagerReturnsNewClientScriptManagerWrapperWhenClientScriptManagerNotMocked()
        {
            _mocks.ReplayAll();

            _target = new TestMvpUserControlBuilder()
               .WithIClientScript(_iClientScript);
        }

        [TestMethod]
        public void TestClientScriptMangerReturnsNullWhenNothingMockedAndNotRunningOnWebServer()
        {
            _mocks.ReplayAll();

            _target = new TestMvpUserControlBuilder();

            Assert.IsNull(_target.ClientScriptManager);
        }

        #endregion

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPagePrerenderAttemptsToLoadClientScriptWhenIsVisible()
        {
            _target.Visible = true;

            using (_mocks.Record())
            {
                SetupResult.For(_iClientScript.TryRegisterClassScriptInclude())
                           .Return(false);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        [TestMethod]
        public void TestPagePrerenderDoesNotAttemptToLoadClientScriptWhenIsNotVisible()
        {
            _target.Visible = false;

            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _iClientScript.TryRegisterClassScriptInclude());
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        #endregion
    }

    internal class TestMvpUserControlBuilder : TestDataBuilder<TestMvpUserControl>
    {
        #region Private Members

        private IPage _iPage;
        private IUser _iUser;
        private IRequest _iRequest;
        private IResponse _iResponse;
        private IServer _iServer;
        private bool? _isMvpPostBack;
        private IClientScriptManager _iClientScript;

        #endregion

        #region Exposed Methods

        public override TestMvpUserControl Build()
        {
            var obj = new TestMvpUserControl();
            if (_iPage != null)
                obj.SetIPage(_iPage);
            if (_iRequest != null)
                obj.SetIRequest(_iRequest);
            if (_iResponse != null)
                obj.SetIResponse(_iResponse);
            if (_isMvpPostBack != null)
                obj.SetIsMvpPostBack(_isMvpPostBack.Value);
            if (_iServer != null)
                obj.SetIServer(_iServer);
            if (_iClientScript != null)
                obj.SetClientScript(_iClientScript);
            if (_iUser != null)
                obj.SetIUser(_iUser);
            return obj;
        }

        public TestMvpUserControlBuilder WithIPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        public TestMvpUserControlBuilder WithIRequest(IRequest request)
        {
            _iRequest = request;
            return this;
        }

        public TestMvpUserControlBuilder WithIResponse(IResponse response)
        {
            _iResponse = response;
            return this;
        }

        public TestMvpUserControlBuilder WithIServer(IServer server)
        {
            _iServer = server;
            return this;
        }

        public TestMvpUserControlBuilder WithIsMvpPostBack(bool b)
        {
            _isMvpPostBack = b;
            return this;
        }

        public TestMvpUserControlBuilder WithIClientScript(IClientScriptManager manager)
        {
            _iClientScript = manager;
            return this;
        }

        public TestMvpUserControlBuilder WithIUser(IUser user)
        {
            _iUser = user;
            return this;
        }

        #endregion
    }

    internal class TestMvpUserControl : MvpUserControl
    {
        #region Exposed Methods

        public void SetIPage(IPage page)
        {
            _iPage = page;
        }

        public void SetIRequest(IRequest request)
        {
            _iRequest = request;
        }

        public void SetIResponse(IResponse response)
        {
            _iResponse = response;
        }

        public void SetIServer(IServer server)
        {
            _iServer = server;
        }

        public void SetIsMvpPostBack(bool b)
        {
            _isMvpPostBack = b;
        }

        public void SetClientScript(IClientScriptManager manager)
        {
            _iClientScript = manager;
        }

        public void SetIUser(IUser user)
        {
            _iUser = user;
        }

        #endregion
    }
}
