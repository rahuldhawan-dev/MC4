using System.Reflection;
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
    /// Summary description for MvpPageTest.
    /// </summary>
    [TestClass]
    public class MvpPageTest : EventFiringTestClass
    {
        #region Private Members

        private IPage _iPage;
        private IUser _iUser;
        private IRequest _iRequest;
        private IServer _iServer;
        private IClientScriptManager _iClientScript;
        private TestMvpPage _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _iPage)
               .DynamicMock(out _iRequest)
               .DynamicMock(out _iServer)
               .DynamicMock(out _iClientScript)
               .DynamicMock(out _iUser);

            _target = new TestMvpPageBuilder()
                     .WithIPage(_iPage)
                     .WithIRequest(_iRequest)
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
        public void TestIPageReturnsWrappedInstanceOfSelfWhenNothingMocked()
        {
            _target = new TestMvpPageBuilder();

            var wrapper = _target.IPage;
            var pi = wrapper.GetType().GetField("_innerPage",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.AreSame(_target, pi.GetValue(wrapper));

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
        public void TestIRequestReturnsIRequestFromIPageWhenNothingMocked()
        {
            _target = new TestMvpPageBuilder()
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
        public void TestIsMvpPostBackValueIsInjectable()
        {
            _target = new TestMvpPageBuilder()
               .WithIsMvpPostBack(true);

            Assert.IsTrue(_target.IsMvpPostBack);

            _target = new TestMvpPageBuilder()
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
        public void TestIServerReturnsMockedIServerObject()
        {
            Assert.AreSame(_iServer, _target.IServer);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestIServerReturnsWrappedInstanceOfServerWhenNothingMocked()
        {
            _mocks.ReplayAll();

            // TODO: find a sound way to test this
            Assert.Inconclusive("Test not yet written.");
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
            _target = new TestMvpPageBuilder()
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

            _target = new TestMvpPageBuilder()
               .WithIClientScript(_iClientScript);
        }

        #endregion

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageLoadAttemptsToLoadClientScriptIfScriptManagerIsNotNull()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_iClientScript.TryRegisterClassScriptInclude())
                           .Return(false);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        #endregion
    }

    internal class TestMvpPageBuilder : TestDataBuilder<TestMvpPage>
    {
        #region Private Members

        private IPage _iPage;
        private IUser _iUser;
        private IRequest _iRequest;
        private IServer _iServer;
        private bool? _isMvpPostBack;
        private IClientScriptManager _iClientScript;

        #endregion

        #region Exposed Methods

        public override TestMvpPage Build()
        {
            var obj = new TestMvpPage();
            if (_iPage != null)
                obj.SetIPage(_iPage);
            if (_iRequest != null)
                obj.SetIRequest(_iRequest);
            if (_isMvpPostBack != null)
                obj.SetIsMvpPostBack(_isMvpPostBack.Value);
            if (_iServer != null)
                obj.SetIServer(_iServer);
            if (_iClientScript != null)
                obj.SetIClientScript(_iClientScript);
            if (_iUser != null)
                obj.SetIUser(_iUser);
            return obj;
        }

        public TestMvpPageBuilder WithIPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        public TestMvpPageBuilder WithIRequest(IRequest request)
        {
            _iRequest = request;
            return this;
        }

        public TestMvpPageBuilder WithIsMvpPostBack(bool b)
        {
            _isMvpPostBack = b;
            return this;
        }

        public TestMvpPageBuilder WithIServer(IServer server)
        {
            _iServer = server;
            return this;
        }

        public TestMvpPageBuilder WithIClientScript(IClientScriptManager manager)
        {
            _iClientScript = manager;
            return this;
        }

        public TestMvpPageBuilder WithIUser(IUser user)
        {
            _iUser = user;
            return this;
        }

        #endregion
    }

    internal class TestMvpPage : MvpPage
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

        public void SetIServer(IServer server)
        {
            _iServer = server;
        }

        public void SetIsMvpPostBack(bool b)
        {
            _isMvpPostBack = b;
        }

        public void SetIClientScript(IClientScriptManager manager)
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
