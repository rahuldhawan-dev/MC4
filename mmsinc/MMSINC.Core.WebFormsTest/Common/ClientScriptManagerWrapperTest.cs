using System;
using System.Web.UI;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using MMSINC.Common;

namespace MMSINC.Core.WebFormsTest.Common
{
    /// <summary>
    /// Summary description for ClientScriptManagerWrapperTest
    /// </summary>
    [TestClass]
    public class ClientScriptManagerWrapperTest : EventFiringTestClass
    {
        #region Private Members

        private ClientScriptManager _scriptManager;
        private IUserControl _control;
        private ClientScriptManagerWrapper _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _control);

            _target = new TestClientScriptManagerWrapperBuilder(_scriptManager,
                _control);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestClassNamePropertyReturnsNameOfControlClass()
        {
            var className = "TheClass";
            var path = "/path/to/" + className + ".ascx";
            var view = _mocks.DynamicMock<IView>();
            _target = new TestClientScriptManagerWrapperBuilder(_scriptManager,
                view);

            using (_mocks.Record())
            {
                SetupResult.For(view.AppRelativeVirtualPath).Return(path);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(className, _target.ClassName);
            }
        }

        // NOTE: Cannot test ClasSciptIsRegistered fully

        [TestMethod]
        public void TestClassScriptKeyPropertyReturnsClassNameFormattedAsScriptKey()
        {
            var className = "TheClass";
            var path = "/path/to/" + className + ".ascx";
            var view = _mocks.DynamicMock<IView>();
            var key = String.Format(
                ClientScriptManagerWrapper.SCRIPT_KEY_FORMAT, className);
            _target = new TestClientScriptManagerWrapperBuilder(_scriptManager,
                view);

            using (_mocks.Record())
            {
                SetupResult.For(view.AppRelativeVirtualPath).Return(path);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(key, _target.ClassScriptKey);
            }
        }

        [TestMethod]
        public void TestClassScriptUrlPropertyReturnsAppRelativeVirtualPathOfControlWithScriptFileExtension()
        {
            var path = "path/to/stuffs.as*x";

            using (_mocks.Record())
            {
                SetupResult.For(_control.AppRelativeVirtualPath).Return(path);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(
                    ClientScriptManagerWrapper.CONTROL_EXTENSION_RGX.Replace(path,
                        ClientScriptManagerWrapper.SCRIPT_FILE_EXTENSION),
                    _target.ClassScriptUrl);
            }
        }

        #endregion
    }

    internal class TestClientScriptManagerWrapperBuilder : TestDataBuilder<ClientScriptManagerWrapper>
    {
        #region Private Members

        private readonly ClientScriptManager _scriptManager;
        private readonly IUserControl _control;

        #endregion

        #region Constructors

        internal TestClientScriptManagerWrapperBuilder(ClientScriptManager scriptManager, IUserControl control)
        {
            _scriptManager = scriptManager;
            _control = control;
        }

        #endregion

        #region Exposed Methods

        public override ClientScriptManagerWrapper Build()
        {
            var obj = new TestClientScriptManagerWrapper(_scriptManager,
                _control);
            return obj;
        }

        #endregion
    }

    internal class TestClientScriptManagerWrapper : ClientScriptManagerWrapper
    {
        #region Constructors

        public TestClientScriptManagerWrapper(ClientScriptManager scriptManager, IUserControl control) : base(
            scriptManager, control) { }

        #endregion
    }
}
