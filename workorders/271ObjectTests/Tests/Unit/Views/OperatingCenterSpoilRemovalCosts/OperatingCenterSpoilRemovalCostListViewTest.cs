using LINQTo271.Views.OperatingCenterSpoilRemovalCosts;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.OperatingCenterSpoilRemovalCosts
{
    /// <summary>
    /// Summary description for OperatingCenterSpoilTypeListViewTest.
    /// </summary>
    [TestClass]
    public class OperatingCenterSpoilRemovalCostListViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListControl _listControl;
        private TestOperatingCenterSpoilRemovalCostListView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _listControl);

            _target = new TestOperatingCenterSpoilRemovalCostListViewBuilder()
                .WithListControl(_listControl);
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

    internal class TestOperatingCenterSpoilRemovalCostListViewBuilder : TestDataBuilder<TestOperatingCenterSpoilRemovalCostListView>
    {
        #region Private Members

        private IListControl _listControl;

        #endregion

        #region Exposed Methods

        public override TestOperatingCenterSpoilRemovalCostListView Build()
        {
            var obj = new TestOperatingCenterSpoilRemovalCostListView();
            if (_listControl != null)
                obj.SetListControl(_listControl);
            return obj;
        }

        public TestOperatingCenterSpoilRemovalCostListViewBuilder WithListControl(IListControl control)
        {
            _listControl = control;
            return this;
        }

        #endregion
    }

    internal class TestOperatingCenterSpoilRemovalCostListView : OperatingCenterSpoilRemovalCostListView
    {
        #region Exposed Methods

        public void SetListControl(IListControl control)
        {
            gvOperatingCenterSpoilRemovalCosts = control;
        }

        #endregion
    }
}
