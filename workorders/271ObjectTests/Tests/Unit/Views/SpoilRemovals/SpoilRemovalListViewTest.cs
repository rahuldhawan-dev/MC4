using System.Collections.Specialized;
using System.Web.UI.WebControls;
using LINQTo271.Views.SpoilRemovals;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace _271ObjectTests.Tests.Unit.Views.SpoilRemovals
{
    /// <summary>
    /// Summary description for SpoilRemovalListViewTest.
    /// </summary>
    [TestClass]
    public class SpoilRemovalListViewTest : EventFiringTestClass
    {
        #region Private Members

        private IPage _iPage;

        private IObjectDataSource _dataSource,
                                  _odsSpoilStorageLocation,
                                  _odsSpoilFinalProcessingLocation;
        private IDropDownList _ddlSpoilStorageLocation,
                              _ddlFinalDestination;
        private ITextBox _txtQuantity, _txtDateRemoved;
        private IGridViewRow _iFooterRow;
        private IGridView _listControl;
        private IViewState _viewState;
        
        private TestSpoilRemovalListView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _ddlSpoilStorageLocation)
                .DynamicMock(out _ddlFinalDestination)
                .DynamicMock(out _txtQuantity)
                .DynamicMock(out _txtDateRemoved)
                .DynamicMock(out _listControl)
                .DynamicMock(out _iFooterRow)
                .DynamicMock(out _viewState)
                .DynamicMock(out _dataSource)
                .DynamicMock(out _odsSpoilStorageLocation)
                .DynamicMock(out _odsSpoilFinalProcessingLocation)
                .DynamicMock(out _iPage);

            SetupResult.For(_listControl.IFooterRow).Return(_iFooterRow);

            _target = new TestSpoilRemovalListViewBuilder()
                .WithDDLSpoilStorageLocation(_ddlSpoilStorageLocation)
                .WithDDLFinalDestination(_ddlFinalDestination)
                .WithTXTQuantity(_txtQuantity)
                .WithTXTDateRemoved(_txtDateRemoved)
                .WithListControl(_listControl)
                .WithViewState(_viewState)
                .WithDataSource(_dataSource)
                .WithODSSpoilStorageLocation(_odsSpoilStorageLocation)
                .WithODSSpoilFinalProcessingLocation(_odsSpoilFinalProcessingLocation)
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
                    SpoilRemovalListView.EntityKeys.OPERATING_CENTER_ID,
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
                        SpoilRemovalListView.EntityKeys.
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
        public void TestRemovedFromReturnsValueFromSpoilStorageLocationDropDownList()
        {
            _target =
                new TestSpoilRemovalListViewBuilder().WithListControl(
                    _listControl);

            var expected = 1;
            using(_mocks.Record())
            {
                SetupResult
                    .For(
                    _iFooterRow.FindIControl<IDropDownList>(
                        SpoilRemovalListView.ControlIDs.REMOVED_FROM))
                    .Return(_ddlSpoilStorageLocation);
                SetupResult
                    .For(_ddlSpoilStorageLocation.GetSelectedValue())
                    .Return(expected);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.RemovedFrom);
            }
        }

        [TestMethod]
        public void TestFinalDestinationReturnsValueFromFinalDestinationDropDownList()
        {
            _target =
                new TestSpoilRemovalListViewBuilder().WithListControl(
                    _listControl);
            var expected = 1;
            using (_mocks.Record())
            {
                SetupResult
                    .For(
                    _iFooterRow.FindIControl<IDropDownList>(
                        SpoilRemovalListView.ControlIDs.FINAL_DESTINATION))
                    .Return(_ddlFinalDestination);
                SetupResult
                    .For(_ddlFinalDestination.GetSelectedValue())
                    .Return(expected);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.FinalDestination);
            }
        }
        
        [TestMethod]
        public void TestQuantityPropertyReturnsValueFromQuantityTextBox()
        {
            _target =
                new TestSpoilRemovalListViewBuilder().WithListControl(
                    _listControl);
            var expected = "some place";

            using (_mocks.Record())
            {
                SetupResult
                    .For(_iFooterRow.FindIControl<ITextBox>(
                        SpoilRemovalListView.ControlIDs.QUANTITY))
                    .Return(_txtQuantity);
                SetupResult
                    .For(_txtQuantity.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.Quantity);
            }
        }

        [TestMethod]
        public void TestDateRemovedPropertyReturnsValueFromDateRemovedTextBox()
        {
            _target =
                new TestSpoilRemovalListViewBuilder().WithListControl(
                    _listControl);
            var expected = "some place";

            using (_mocks.Record())
            {
                SetupResult
                    .For(_iFooterRow.FindIControl<ITextBox>(
                        SpoilRemovalListView.ControlIDs.DATE_REMOVED))
                    .Return(_txtDateRemoved);
                SetupResult
                    .For(_txtDateRemoved.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateRemoved);
            }
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestSetViewControlsVisibleDoesNothing()
        {
            _mocks.CreateMock(out _listControl);
            _target = new TestSpoilRemovalListViewBuilder()
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
            var expected = 14;

            using (_mocks.Record())
            {
                SetupResult
                    .For(_viewState.GetValue(
                        SpoilRemovalListView.EntityKeys.OPERATING_CENTER_ID))
                    .Return(expected);
                _dataSource.SetDefaultSelectParameterValue(
                    SpoilRemovalListView.EntityKeys.OPERATING_CENTER_ID,
                    expected.ToString());
                _odsSpoilStorageLocation.SetDefaultSelectParameterValue(
                    SpoilRemovalListView.EntityKeys.OPERATING_CENTER_ID,
                    expected.ToString());
                _odsSpoilFinalProcessingLocation.SetDefaultSelectParameterValue(
                    SpoilRemovalListView.EntityKeys.OPERATING_CENTER_ID,
                    expected.ToString());
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ListControl_DataBinding");
            }
        }
        
        [TestMethod]
        public void TestODSSpoilStorageLocationsInsertingSetsInputParameters()
        {
            int expectedRemovedFromID = 1,
                expectedFinalDestinationID = 3;
            string expectedDateRemoved = "10/10/2009", expectedQuantity = "1.2";

            var args =
                new ObjectDataSourceMethodEventArgs(new OrderedDictionary());

            using (_mocks.Record())
            {
                SetupResult.For(_ddlSpoilStorageLocation.GetSelectedValue()).
                    Return(expectedRemovedFromID);
                SetupResult.For(_ddlFinalDestination.GetSelectedValue()).Return(
                    expectedFinalDestinationID);
                SetupResult.For(_txtDateRemoved.Text).Return(expectedDateRemoved);
                SetupResult.For(_txtQuantity.Text).Return(expectedQuantity);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "odsSpoilRemovals_Inserting",
                    new object[] {
                        null, args
                    });

                Assert.AreEqual(expectedRemovedFromID,
                    args.InputParameters[
                        SpoilRemovalListView.EntityKeys.REMOVED_FROM]);
                Assert.AreEqual(expectedFinalDestinationID,
                    args.InputParameters[
                        SpoilRemovalListView.EntityKeys.FINAL_DESTINATION]);
                Assert.AreEqual(expectedDateRemoved,
                    args.InputParameters[
                        SpoilRemovalListView.EntityKeys.DATE_REMOVED]);
                Assert.AreEqual(expectedQuantity,
                    args.InputParameters[
                        SpoilRemovalListView.EntityKeys.QUANTITY]);
            }
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

    internal class TestSpoilRemovalListViewBuilder : TestDataBuilder<TestSpoilRemovalListView>
    {
        #region Private Members

        private IGridView _listControl;
        private IViewState _viewState;
        private IDropDownList _ddlSpoilStorageLocation,
                              _ddlFinalDestination;
        private ITextBox _txtQuantity, _txtDateRemoved;
        private IObjectDataSource _dataSource,
                                  _odsSpoilStorageLocation,
                                  _odsSpoilFinalProcessingLocation;
        private IPage _iPage;

        #endregion

        #region Exposed Methods

        public override TestSpoilRemovalListView Build()
        {
            var obj = new TestSpoilRemovalListView();
            if (_listControl != null)
                obj.SetListControl(_listControl);
            if (_viewState != null)
                obj.SetViewState(_viewState);
            if (_ddlSpoilStorageLocation!=null)
                obj.SetDDLSpoilStorageLocation(_ddlSpoilStorageLocation);
            if (_ddlFinalDestination!=null)
                obj.SetDDLFinalDestination(_ddlFinalDestination);
            if (_txtQuantity!=null)
                obj.SetTXTQuantity(_txtQuantity);
            if (_txtDateRemoved!=null)
                obj.SetTXTDateRemoved(_txtDateRemoved);
            if (_dataSource != null)
                obj.SetDataSource(_dataSource);
            if (_odsSpoilStorageLocation!=null)
                obj.SetODSpoilStorageLocation(_odsSpoilStorageLocation);
            if (_odsSpoilFinalProcessingLocation!=null)
                obj.SetODSSpoilFinalProcessingLocation(
                    _odsSpoilFinalProcessingLocation);
            if (_iPage != null)
                obj.SetPage(_iPage);
            return obj;
        }

        public TestSpoilRemovalListViewBuilder WithListControl(IGridView listControl)
        {
            _listControl = listControl;
            return this;
        }

        public TestSpoilRemovalListViewBuilder WithViewState(IViewState state)
        {
            _viewState = state;
            return this;
        }

        public TestSpoilRemovalListViewBuilder WithDDLSpoilStorageLocation(IDropDownList spoilStorageLocation)
        {
            _ddlSpoilStorageLocation = spoilStorageLocation;
            return this;
        }

        public TestSpoilRemovalListViewBuilder WithDDLFinalDestination(IDropDownList finalDestination)
        {
            _ddlFinalDestination = finalDestination;
            return this;
        }

        public TestSpoilRemovalListViewBuilder WithTXTQuantity(ITextBox quantity)
        {
            _txtQuantity = quantity;
            return this;
        }

        public TestSpoilRemovalListViewBuilder WithTXTDateRemoved(ITextBox dateRemoved)
        {
            _txtDateRemoved = dateRemoved;
            return this;
        }

        public TestSpoilRemovalListViewBuilder WithDataSource(IObjectDataSource locations)
        {
            _dataSource = locations;
            return this;
        }

        public TestSpoilRemovalListViewBuilder WithODSSpoilStorageLocation(IObjectDataSource odsSpoilStorageLocation)
        {
            _odsSpoilStorageLocation = odsSpoilStorageLocation;
            return this;
        }

        public TestSpoilRemovalListViewBuilder WithODSSpoilFinalProcessingLocation(IObjectDataSource odsSpoilFinalProcessingLocation)
        {
            _odsSpoilFinalProcessingLocation = odsSpoilFinalProcessingLocation;
            return this;
        }

        public TestSpoilRemovalListViewBuilder WithPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        #endregion
    }

    internal class TestSpoilRemovalListView : SpoilRemovalListView
    {
        #region Exposed Methods

        public void SetListControl(IGridView control)
        {
            gvSpoilRemovals = control;
        }

        public void SetViewState(IViewState viewState)
        {
            _iViewState = viewState;
        }

        public void SetDDLSpoilStorageLocation(IDropDownList spoilStorageLocation)
        {
            ddlSpoilStorageLocation = spoilStorageLocation;
        }

        public void SetDDLFinalDestination(IDropDownList finalDestination)
        {
            ddlSpoilFinalProcessingLocation = finalDestination;
        }

        public void SetTXTQuantity(ITextBox quantity)
        {
            txtQuantity = quantity;
        }

        public void SetTXTDateRemoved(ITextBox dateRemoved)
        {
            txtDateRemoved = dateRemoved;
        }
        
        public void SetDataSource(IObjectDataSource source)
        {
            odsSpoilRemovals = source;
        }

        public void SetODSpoilStorageLocation(IObjectDataSource ods)
        {
            odsSpoilStorageLocation = ods;
        }

        public void SetODSSpoilFinalProcessingLocation(IObjectDataSource ods)
        {
            odsSpoilFinalProcessingLocation = ods;
        }

        public void SetPage(IPage page)
        {
            _iPage = page;
        }

        #endregion
    }
}
