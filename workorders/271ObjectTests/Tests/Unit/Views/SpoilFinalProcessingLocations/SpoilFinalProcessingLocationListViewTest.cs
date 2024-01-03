using System.Collections.Specialized;
using System.Web.UI.WebControls;
using LINQTo271.Views.SpoilFinalProcessingLocations;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace _271ObjectTests.Tests.Unit.Views.SpoilFinalProcessingLocations
{
    /// <summary>
    /// Summary description for SpoilFinalProcessingLocationListViewTest.
    /// </summary>
    [TestClass]
    public class SpoilFinalProcessingLocationListViewTest : EventFiringTestClass
    {
        #region Private Members

        private ITextBox _txtName;
        private IDropDownList _ddlTown, _ddlStreet;
        private IGridViewRow _iFooterRow;
        private IObjectDataSource _dataSource, _odsTowns;
        private IViewState _viewState;
        private IGridView _listControl;
        private IPage _iPage;

        private TestSpoilFinalProcessingLocationListView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _viewState)
                .DynamicMock(out _listControl)
                .DynamicMock(out _dataSource)
                .DynamicMock(out _odsTowns)
                .DynamicMock(out _iFooterRow)
                .DynamicMock(out _txtName)
                .DynamicMock(out _ddlTown)
                .DynamicMock(out _ddlStreet)
                .DynamicMock(out _iPage);

            SetupResult.For(_listControl.IFooterRow).Return(_iFooterRow);

            _target = new TestSpoilFinalProcessingLocationListViewBuilder()
                .WithViewState(_viewState)
                .WithListControl(_listControl)
                .WithDataSource(_dataSource)
                .WithODSTowns(_odsTowns)
                .WithTXTName(_txtName)
                .WithDDLTown(_ddlTown)
                .WithDDLStreet(_ddlStreet)
                .WithPage(_iPage);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestSettingOperatingCenterIDSetsValueInViewState()
        {
            var expected = 10;

            using (_mocks.Record())
            {
                _viewState.SetValue(
                    SpoilFinalProcessingLocationListView.EntityKeys.OPERATING_CENTER_ID,
                    expected);
            }

            using (_mocks.Playback())
            {
                _target.OperatingCenterID = expected;
            }
        }

        [TestMethod]
        public void TestGettingOperatingCenterIDRetrievesValueFromViewState()
        {
            var expected = 6;

            using (_mocks.Record())
            {
                SetupResult
                    .For(_viewState.GetValue(
                        SpoilFinalProcessingLocationListView.EntityKeys.
                            OPERATING_CENTER_ID))
                    .Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.OperatingCenterID);
            }
        }

        [TestMethod]
        public void TestListControlPropertyReturnsListControl()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_listControl, _target.ListControl);
        }

        [TestMethod]
        public void TestNamePropertyGetsNameTextBoxFromGridViewIfNull()
        {
            _target = new TestSpoilFinalProcessingLocationListViewBuilder()
                .WithListControl(_listControl);
            var expected = "New Location";

            using (_mocks.Record())
            {
                SetupResult
                    .For(_iFooterRow.FindIControl<ITextBox>(
                        SpoilFinalProcessingLocationListView.ControlIDs.NAME))
                    .Return(_txtName);
                SetupResult
                    .For(_txtName.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.Name);
            }
        }

        [TestMethod]
        public void TestTownIDPropertyReturnsValueFromTownDropDown()
        {
            _target = new TestSpoilFinalProcessingLocationListViewBuilder()
                .WithListControl(_listControl);
            var expected = 69;

            using (_mocks.Record())
            {
                SetupResult
                    .For(
                    _iFooterRow.FindIControl<IDropDownList>(
                        SpoilFinalProcessingLocationListView.ControlIDs.TOWN_ID))
                    .Return(_ddlTown);
                SetupResult
                    .For(_ddlTown.GetSelectedValue())
                    .Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.TownID);
            }
        }

        [TestMethod]
        public void TestStreetIDPropertyReturnsValueFromStreetDropDown()
        {
            _target = new TestSpoilFinalProcessingLocationListViewBuilder()
                .WithListControl(_listControl);
            var expected = 138;

            using (_mocks.Record())
            {
                SetupResult
                    .For(
                    _iFooterRow.FindIControl<IDropDownList>(
                        SpoilFinalProcessingLocationListView.ControlIDs.STREET_ID))
                    .Return(_ddlStreet);
                SetupResult
                    .For(_ddlStreet.GetSelectedValue())
                    .Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.StreetID);
            }
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestSetViewControlsVisibleDoesNothing()
        {
            _mocks.CreateMock(out _listControl);
            _target = new TestSpoilFinalProcessingLocationListViewBuilder()
                .WithListControl(_listControl);

            using (_mocks.Record())
            {

            }

            using (_mocks.Playback())
            {
                _target.SetViewControlsVisible(false);
                _target.SetViewControlsVisible(true);
            }
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestListControlDataBindingSetsOperatingCenterParameterOnDataSources()
        {
            var expected = 16;

            using (_mocks.Record())
            {
                SetupResult
                    .For(_viewState.GetValue(
                        SpoilFinalProcessingLocationListView.EntityKeys.OPERATING_CENTER_ID))
                    .Return(expected);
                _dataSource.SetDefaultSelectParameterValue(
                    SpoilFinalProcessingLocationListView.EntityKeys.OPERATING_CENTER_ID,
                    expected.ToString());
                _odsTowns.SetDefaultSelectParameterValue(
                    SpoilFinalProcessingLocationListView.EntityKeys.OPERATING_CENTER_ID,
                    expected.ToString());
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ListControl_DataBinding");
            }
        }

        [TestMethod]
        public void TestODSSpoilFinalProcessingLocationsInsertingSetsInputParameters()
        {
            int expectedOperatingCenterID = 20,
                expectedTownID = 100,
                expectedStreetID = 666;
            string expectedName = "Test Location";
            var args =
                new ObjectDataSourceMethodEventArgs(new OrderedDictionary());

            using (_mocks.Record())
            {
                SetupResult
                    .For(_viewState.GetValue(
                        SpoilFinalProcessingLocationListView.EntityKeys.
                            OPERATING_CENTER_ID))
                    .Return(expectedOperatingCenterID);
                SetupResult.For(_txtName.Text).Return(expectedName);
                SetupResult.For(_ddlTown.GetSelectedValue()).Return(expectedTownID);
                SetupResult
                    .For(_ddlStreet.GetSelectedValue()).Return(expectedStreetID);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "odsSpoilFinalProcessingLocations_Inserting",
                    new object[] {
                        null, args
                    });

                Assert.AreEqual(expectedOperatingCenterID,
                    args.InputParameters[
                    SpoilFinalProcessingLocationListView.EntityKeys.OPERATING_CENTER_ID]);
                Assert.AreEqual(expectedName,
                    args.InputParameters[
                        SpoilFinalProcessingLocationListView.EntityKeys.NAME]);
                Assert.AreEqual(expectedTownID,
                    args.InputParameters[
                        SpoilFinalProcessingLocationListView.EntityKeys.TOWN_ID]);
                Assert.AreEqual(expectedStreetID,
                    args.InputParameters[
                        SpoilFinalProcessingLocationListView.EntityKeys.STREET_ID]);
            }
        }

        [TestMethod]
        public void TestODSSpoilFinalProcessingLocationsUpdatingFixesStreetIDParameter()
        {
            _mocks.ReplayAll();

            var expected = "123";
            var args =
                new ObjectDataSourceMethodEventArgs(new OrderedDictionary());
            args.InputParameters[
                SpoilFinalProcessingLocationListView.EntityKeys.STREET_ID] = expected +
                                                                     ":::foobar";

            InvokeEventByName(_target, "odsSpoilFinalProcessingLocations_Updating",
                new object[] {
                    null, args
                });

            Assert.AreEqual(expected, args.InputParameters[SpoilFinalProcessingLocationListView.EntityKeys.STREET_ID]);
        }

        [TestMethod]
        public void TestLBInsertClickCallsInsertOnDataSourceIfPageIsValid()
        {
            using (_mocks.Record())
            {
                Expect.Call(_iPage.IsValid).Return(true);
                Expect.Call(_dataSource.Insert()).Return(1);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "lbInsert_Click");
            }
        }

        [TestMethod]
        public void TestLBInsertClickDoesNotCallInsertOnDataSourceIfPageIsNotValid()
        {
            using (_mocks.Record())
            {
                Expect.Call(_iPage.IsValid).Return(false);
                DoNotExpect.Call(_dataSource.Insert());
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "lbInsert_Click");
            }
        }

        #endregion
    }

    internal class TestSpoilFinalProcessingLocationListViewBuilder : TestDataBuilder<TestSpoilFinalProcessingLocationListView>
    {
        #region Private Members

        private IGridView _listControl;
        private IViewState _viewState;
        private IObjectDataSource _dataSource;
        private ITextBox _txtName;
        private IDropDownList _ddlTown, _ddlStreet;
        private IPage _iPage;
        private IObjectDataSource _odsTowns;

        #endregion

        #region Exposed Methods

        public override TestSpoilFinalProcessingLocationListView Build()
        {
            var obj = new TestSpoilFinalProcessingLocationListView();
            if (_listControl != null)
                obj.SetListControl(_listControl);
            if (_viewState != null)
                obj.SetViewState(_viewState);
            if (_dataSource != null)
                obj.SetDataSource(_dataSource);
            if (_txtName != null)
                obj.SetTXTName(_txtName);
            if (_ddlTown != null)
                obj.SetDDLTown(_ddlTown);
            if (_ddlStreet != null)
                obj.SetDDLStreet(_ddlStreet);
            if (_iPage != null)
                obj.SetPage(_iPage);
            if (_odsTowns != null)
                obj.SetODSTowns(_odsTowns);
            return obj;
        }

        public TestSpoilFinalProcessingLocationListViewBuilder WithListControl(IGridView control)
        {
            _listControl = control;
            return this;
        }

        public TestSpoilFinalProcessingLocationListViewBuilder WithViewState(IViewState state)
        {
            _viewState = state;
            return this;
        }

        public TestSpoilFinalProcessingLocationListViewBuilder WithDataSource(IObjectDataSource source)
        {
            _dataSource = source;
            return this;
        }

        public TestSpoilFinalProcessingLocationListViewBuilder WithTXTName(ITextBox name)
        {
            _txtName = name;
            return this;
        }

        public TestSpoilFinalProcessingLocationListViewBuilder WithDDLTown(IDropDownList town)
        {
            _ddlTown = town;
            return this;
        }

        public TestSpoilFinalProcessingLocationListViewBuilder WithDDLStreet(IDropDownList street)
        {
            _ddlStreet = street;
            return this;
        }

        public TestSpoilFinalProcessingLocationListViewBuilder WithPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        public TestSpoilFinalProcessingLocationListViewBuilder WithODSTowns(IObjectDataSource source)
        {
            _odsTowns = source;
            return this;
        }

        #endregion
    }

    internal class TestSpoilFinalProcessingLocationListView : SpoilFinalProcessingLocationListView
    {
        #region Exposed Methods

        public void SetListControl(IGridView control)
        {
            gvSpoilFinalProcessingLocations = control;
        }

        public void SetViewState(IViewState state)
        {
            _iViewState = state;
        }

        public void SetDataSource(IObjectDataSource source)
        {
            odsSpoilFinalProcessingLocations = source;
        }

        public void SetTXTName(ITextBox name)
        {
            txtName = name;
        }

        public void SetDDLTown(IDropDownList town)
        {
            ddlTown = town;
        }

        public void SetDDLStreet(IDropDownList street)
        {
            ddlStreet = street;
        }

        public void SetPage(IPage page)
        {
            _iPage = page;
        }

        public void SetODSTowns(IObjectDataSource source)
        {
            odsTowns = source;
        }

        #endregion
    }
}
