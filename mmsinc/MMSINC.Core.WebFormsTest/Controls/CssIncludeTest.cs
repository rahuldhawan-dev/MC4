using System;
using System.Reflection;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for CssIncludeTest.
    /// </summary>
    [TestClass]
    public class CssIncludeTest : EventFiringTestClass
    {
        #region Constants

        private const string CSS_FILE_NAME = "mySteez.css";

        #endregion

        #region Private Members

        private IClientScriptManager _scriptManager;
        private IUserControl _mockParent;
        private string _cssFileName = CSS_FILE_NAME;
        private TestCssInclude _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _mockParent)
               .DynamicMock(out _scriptManager);

            SetupResult.For(_mockParent.ClientScriptManager)
                       .Return(_scriptManager);

            _target = new TestCssIncludeBuilder()
                     .WithMockParent(_mockParent)
                     .WithCssFileName(_cssFileName);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        #region IParent

        [TestMethod]
        public void TestClientScriptManagerReturnsClientScriptManagerFromParent()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_mockParent.ClientScriptManager, _target.ClientScriptManager);
        }

        #endregion

        #region IncludesPath

        [TestMethod]
        public void TestIncludesPathReturnsValueIfSet()
        {
            _mocks.ReplayAll();

            var path = "path/to/stuff/";
            _target = new TestCssIncludeBuilder().WithIncludesPath(path);

            Assert.AreEqual(path, _target.IncludesPath);
        }

        [TestMethod]
        public void TestIncludesPathReturnsDefaultValueIfNotSet()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(ScriptInclude.BASE_INCLUDES_PATH, _target.IncludesPath);
        }

        #endregion

        #region CssFileUrl

        [TestMethod]
        public void TestScriptFileUrlPropertyReturnsConcatenatedUrlIfNotSet()
        {
            _mocks.ReplayAll();

            Assert.AreEqual("~/Includes/mySteez.css", _target.CssFileUrl);
        }

        #endregion

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestCreateChildControlsRegistersCssIncludeIfFileExists()
        {
            using (_mocks.Record())
            {
                SetupResult
                   .For(_scriptManager.CssFileExists(_target.CssFileUrl))
                   .Return(true);
                SetupResult
                   .For(_scriptManager.TryRegisterCssInclude(_target.CssFileUrl))
                   .Return(true);
            }

            using (_mocks.Playback())
            {
                _target.GetType()
                       .GetMethod("CreateChildControls",
                            BindingFlags.NonPublic | BindingFlags.Instance)
                       .Invoke(_target, null);
            }
        }

        [TestMethod]
        public void TestCreateChildControlsThrowsExceptionIfFileDoesNotExist()
        {
            using (_mocks.Record())
            {
                SetupResult
                   .For(_scriptManager.ClientScriptExists(_target.CssFileUrl))
                   .Return(false);
                DoNotExpect.Call(
                    () =>
                        _scriptManager.TryRegisterCssInclude(_target.CssFileUrl));
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws<ArgumentException>(
                    () =>
                        _target.GetType()
                               .GetMethod("CreateChildControls",
                                    BindingFlags.NonPublic | BindingFlags.Instance)
                               .Invoke(_target, null));
            }
        }

        #endregion
    }

    internal class TestCssIncludeBuilder : TestDataBuilder<TestCssInclude>
    {
        #region Private Members

        private IUserControl _mockParent;
        private string _includesPath;
        private string _cssFileName;

        #endregion

        #region Exposed Methods

        public override TestCssInclude Build()
        {
            var obj = new TestCssInclude();
            if (_mockParent != null)
                obj.SetMockParent(_mockParent);
            if (_includesPath != null)
                obj.IncludesPath = _includesPath;
            if (_cssFileName != null)
                obj.CssFileName = _cssFileName;
            return obj;
        }

        public TestCssIncludeBuilder WithMockParent(IUserControl parent)
        {
            _mockParent = parent;
            return this;
        }

        public TestCssIncludeBuilder WithIncludesPath(string path)
        {
            _includesPath = path;
            return this;
        }

        public TestCssIncludeBuilder WithCssFileName(string name)
        {
            _cssFileName = name;
            return this;
        }

        #endregion
    }

    internal class TestCssInclude : CssInclude
    {
        #region Exposed Methods

        public void SetMockParent(IUserControl control)
        {
            _parent = control;
        }

        #endregion
    }
}
