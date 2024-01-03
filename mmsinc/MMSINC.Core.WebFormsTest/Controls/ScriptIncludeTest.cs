using System;
using System.Web.UI;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using MMSINC.Interface;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for ScriptIncludeTest.
    /// </summary>
    [TestClass]
    public class ScriptIncludeTest : EventFiringTestClass
    {
        #region Constants

        public const string SCRIPT_FILE_NAME = "foo.js";

        #endregion

        #region Private Members

        private IClientScriptManager _scriptManager;
        private IUserControl _mockParent;
        private string _scriptFileName = SCRIPT_FILE_NAME;
        private TestScriptInclude _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _mockParent)
               .DynamicMock(out _scriptManager);

            SetupResult.For(_mockParent.ClientScriptManager).Return(
                _scriptManager);

            _target = new TestScriptIncludeBuilder()
                     .WithMockParent(_mockParent)
                     .WithScriptFileName(_scriptFileName);
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
        public void TestClientScriptManagerComesFromIParent()
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
            _target = new TestScriptIncludeBuilder().WithIncludesPath(path);

            Assert.AreEqual(path, _target.IncludesPath);
        }

        [TestMethod]
        public void TestIncludesPathReturnsDefaultValueIfNotSet()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(ScriptInclude.BASE_INCLUDES_PATH, _target.IncludesPath);
        }

        #endregion

        #region ScriptFileName

        [TestMethod]
        public void TestScriptFileNameReturnsValueIfSet()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(_scriptFileName, _target.ScriptFileName);
        }

        [TestMethod]
        public void TestScriptFileNameReturnsNullIfNotSet()
        {
            _mocks.ReplayAll();

            _target = new TestScriptIncludeBuilder().WithScriptFileName(null);

            Assert.IsNull(_target.ScriptFileName);
        }

        #endregion

        #region ScriptKey

        [TestMethod]
        public void TestScriptKeyPropertyGeneratesFormattedScriptKeyIfNotSet()
        {
            _mocks.ReplayAll();

            Assert.AreEqual("foo_ScriptInclude", _target.ScriptKey);
        }

        #endregion

        #region ScriptFileUrl

        [TestMethod]
        public void TestScriptFileUrlPropertyReturnsConcatenatedUrlIfNotSet()
        {
            _mocks.ReplayAll();

            Assert.AreEqual("~/Includes/foo.js", _target.ScriptFileUrl);
        }

        #endregion

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageLoadRegistersClientScriptIfScriptUrlExists()
        {
            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _scriptManager.TryRegisterClassScriptInclude());
                SetupResult
                   .For(_scriptManager.ClientScriptExists(_target.ScriptFileUrl))
                   .Return(true);
                SetupResult
                   .For(_scriptManager.TryRegisterClientScriptInclude(
                        _target.ScriptKey, _target.ScriptFileUrl))
                   .Return(true);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadThrowsExceptionIfScriptUrlDoesNotExist()
        {
            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _scriptManager.TryRegisterClassScriptInclude());
                DoNotExpect.Call(
                    () => _scriptManager.TryRegisterClientScriptInclude(
                        _target.ScriptKey, _target.ScriptFileUrl));
                SetupResult
                   .For(_scriptManager.ClientScriptExists(_target.ScriptFileUrl))
                   .Return(false);
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws<ArgumentException>(
                    () => InvokeEventByName(_target, "Page_Load"));
            }
        }

        #endregion
    }

    internal class TestScriptIncludeBuilder : TestDataBuilder<TestScriptInclude>
    {
        #region Private Members

        private Control _realParent;
        private IUserControl _mockParent;
        private string _scriptFileName;
        private string _includesPath;

        #endregion

        #region Exposed Methods

        public override TestScriptInclude Build()
        {
            var obj = new TestScriptInclude();
            if (_mockParent != null)
                obj.SetMockParent(_mockParent);
            if (_realParent != null)
                obj.SetRealParent(_realParent);
            if (_scriptFileName != null)
                obj.ScriptFileName = _scriptFileName;
            if (_includesPath != null)
                obj.IncludesPath = _includesPath;
            return obj;
        }

        public TestScriptIncludeBuilder WithMockParent(IUserControl parent)
        {
            _mockParent = parent;
            return this;
        }

        public TestScriptIncludeBuilder WithRealParent(Control parent)
        {
            _realParent = parent;
            return this;
        }

        public TestScriptIncludeBuilder WithScriptFileName(string scriptFileName)
        {
            _scriptFileName = scriptFileName;
            return this;
        }

        public TestScriptIncludeBuilder WithIncludesPath(string path)
        {
            _includesPath = path;
            return this;
        }

        #endregion
    }

    internal class TestScriptInclude : ScriptInclude
    {
        #region Private Members

        private Control _realParent;

        #endregion

        #region Properties

        public override Control Parent
        {
            get { return _realParent ?? base.Parent; }
        }

        #endregion

        #region Exposed Methods

        public void SetMockParent(IUserControl parent)
        {
            _parent = parent;
        }

        public void SetRealParent(Control parent)
        {
            _realParent = parent;
        }

        public void SetScriptManager(IClientScriptManager scriptManager)
        {
            _iClientScript = scriptManager;
        }

        #endregion
    }
}
