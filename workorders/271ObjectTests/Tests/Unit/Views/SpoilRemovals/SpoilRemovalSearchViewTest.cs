using System.Web.UI.WebControls;
using LINQTo271.Views.SpoilRemovals;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace _271ObjectTests.Tests.Unit.Views.SpoilRemovals
{
    /// <summary>
    /// Summary description for SpoilRemovalSearchViewTest.
    /// </summary>
    [TestClass]
    public class SpoilRemovalSearchViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestSpoilRemovalSearchView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SpoilRemovalSearchViewTestInitialize()
        {
            _target = new TestSpoilRemovalSearchViewBuilder();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestOperatingCenterIDBindsDdlOperatingCenterIfItemsCountEqualsZero()
        {
            var expected = "1";
            var ddlOperatingCenter = _mocks.DynamicMock<IDropDownList>();
            _target.SetHiddenFieldValueByName("ddlOperatingCenter",
                ddlOperatingCenter);
            var items = new ListItemCollection { };
            using (_mocks.Record())
            {
                SetupResult.For(ddlOperatingCenter.Items).Return(items);
                SetupResult.For(ddlOperatingCenter.SelectedValue).Return(
                    expected);
            }
            using (_mocks.Playback())
            {
                var result = _target.OperatingCenterID;
                Assert.AreEqual(result, int.Parse(expected));
            }
        }

        #endregion
    }

    internal class TestSpoilRemovalSearchViewBuilder : TestDataBuilder<TestSpoilRemovalSearchView>
    {
        #region Exposed Methods

        public override TestSpoilRemovalSearchView Build()
        {
            var obj = new TestSpoilRemovalSearchView();
            return obj;
        }

        #endregion
    }

    internal class TestSpoilRemovalSearchView : SpoilRemovalSearchView
    {
    }
}
