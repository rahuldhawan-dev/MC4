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
    /// Summary description for ClientIDRepositoryTest.
    /// </summary>
    [TestClass]
    public class ClientIDRepositoryTest : EventFiringTestClass
    {
        #region Private Members

        private IClientScriptManager _scriptManager;
        private TestClientIDRepository _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks.DynamicMock(out _scriptManager);

            _target = new TestClientIDRepositoryBuilder()
               .WithScriptManager(_scriptManager);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestPagePrerenderDoesNotLoadClassScriptUsingScriptManager()
        {
            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _scriptManager.TryRegisterClassScriptInclude());
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }
    }

    internal class TestClientIDRepositoryBuilder : TestDataBuilder<TestClientIDRepository>
    {
        #region Private Members

        private IClientScriptManager _scriptManager;

        #endregion

        #region Exposed Methods

        public override TestClientIDRepository Build()
        {
            var obj = new TestClientIDRepository();
            if (_scriptManager != null)
                obj.SetScriptManager(_scriptManager);
            return obj;
        }

        public TestClientIDRepositoryBuilder WithScriptManager(IClientScriptManager manager)
        {
            _scriptManager = manager;
            return this;
        }

        #endregion
    }

    internal class TestClientIDRepository : ClientIDRepository
    {
        #region Exposed Methods

        public void SetScriptManager(IClientScriptManager manager)
        {
            _iClientScript = manager;
        }

        #endregion
    }
}
