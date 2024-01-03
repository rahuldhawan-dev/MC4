using LINQTo271.Views.RestorationTypeCosts;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.RestorationTypeCosts
{
    /// <summary>
    /// Summary description for RestorationTypeCostListViewTestTest
    /// </summary>
    [TestClass]
    public class RestorationTypeCostListViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListControl _listControl;
        private TestRestorationTypeCostListView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks.DynamicMock(out _listControl);
            _target = new TestRestorationTypeCostListViewBuilder()
                .WithListControl(_listControl);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestListControlPropertyReturnsListControl()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_listControl, _target.ListControl);
        }

        [TestMethod]
        public void TestSetViewControlsVisibleDoesNothing()
        {
            using (_mocks.Record())
            {
            }

            using (_mocks.Playback())
            {
                _target.SetViewControlsVisible(false);
                _target.SetViewControlsVisible(true);
            }
        }
    }

    internal class TestRestorationTypeCostListViewBuilder : TestDataBuilder<TestRestorationTypeCostListView>
    {
        #region Private Members

        private IListControl _listControl;

        #endregion

        #region Exposed Methods

        public override TestRestorationTypeCostListView Build()
        {
            var obj = new TestRestorationTypeCostListView();
            if (_listControl != null)
                obj.SetListControl(_listControl);
            return obj;
        }

        public TestRestorationTypeCostListViewBuilder WithListControl(IListControl control)
        {
            _listControl = control;
            return this;
        }

        #endregion
    }

    internal class TestRestorationTypeCostListView : RestorationTypeCostListView
    {
        #region Exposed Methods

        public void SetListControl(IListControl control)
        {
            gvRestorationTypeCosts = control;
        }

        #endregion
    }
}
