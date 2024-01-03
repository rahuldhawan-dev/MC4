using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderSpoilsFormTest.
    /// </summary>
    [TestClass]
    public class WorkOrderSpoilsFormTest : EventFiringTestClass
    {
        #region Private Members

        private ParameterCollection _odsSpoilsParameters;
        private IObjectDataSource _dataSource,
                                  _odsOperatingCenterSpoilStorageLocations;
        private IDropDownList _ddlSpoilStorageLocation;
        private ITextBox _txtQuantity;
        private IGridViewRow _iFooterRow, _iEditRow;
        private IGridViewRowCollection _rowCollection;
        private IEnumerator<IGridViewRow> _rowCollectionEnum;
        private IGridView _gvSpoils;
        private IViewState _viewState;
        private IPage _iPage;
        private ILinkButton _lbEdit, _lbDelete;
        private IRepository<WorkOrder> _workOrderRepository;

        private TestWorkOrderSpoilsForm _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _dataSource)
                .DynamicMock(out _odsOperatingCenterSpoilStorageLocations)
                .DynamicMock(out _ddlSpoilStorageLocation)
                .DynamicMock(out _txtQuantity)
                .DynamicMock(out _gvSpoils)
                .DynamicMock(out _rowCollection)
                .DynamicMock(out _rowCollectionEnum)
                .DynamicMock(out _iFooterRow)
                .DynamicMock(out _iEditRow)
                .DynamicMock(out _viewState)
                .DynamicMock(out _iPage)
                .DynamicMock(out _lbEdit)
                .DynamicMock(out _lbDelete)
                .DynamicMock(out _workOrderRepository);

            _odsSpoilsParameters = new ParameterCollection();
            SetupResult.For(_dataSource.SelectParameters).Return(
                _odsSpoilsParameters);

            _target = new TestWorkOrderSpoilsFormBuilder()
                .WithDataSource(_dataSource)
                .WithGridView(_gvSpoils)
                .WithViewState(_viewState)
                .WithPage(_iPage)
                .WithWorkOrderRepository(_workOrderRepository)
                .WithODSSpoilStorageLocations(
                _odsOperatingCenterSpoilStorageLocations);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        private void SetupGridView()
        {
            const int editIndex = -1;

            SetupResult.For(_gvSpoils.IRows).Return(_rowCollection);
            SetupResult.For(_gvSpoils.EditIndex).Return(editIndex);
            SetupResult.For(_gvSpoils.IFooterRow).Return(_iFooterRow);
            SetupResult.For(_rowCollection[editIndex]).Return(_iEditRow);
            SetupResult.For(_rowCollection.GetEnumerator())
                .Return(_rowCollectionEnum);

            SetupResult
                .For(_iFooterRow.FindIControl<ITextBox>(
                    WorkOrderSpoilsForm.ControlIDs.QUANTITY))
                .Return(_txtQuantity);
            SetupResult
                .For(_iFooterRow.FindIControl<IDropDownList>(
                    WorkOrderSpoilsForm.ControlIDs.SPOIL_STORAGE_LOCATION))
                .Return(_ddlSpoilStorageLocation);

            SetupResult
                .For(
                _iEditRow.FindIControl<ILinkButton>(
                    WorkOrderSpoilsForm.ControlIDs.EDIT_LINK))
                .Return(_lbEdit);
            SetupResult
                .For(
                _iEditRow.FindIControl<ILinkButton>(
                    WorkOrderSpoilsForm.ControlIDs.DELETE_LINK))
                .Return(_lbDelete);

            var moveNextCalled = false;
            SetupResult.For(_rowCollectionEnum.MoveNext()).Do(
                (Func<bool>)delegate {
                    if (!moveNextCalled)
                    {
                        moveNextCalled = true;
                        return true;
                    }
                    return false;
                });
            SetupResult.For(_rowCollectionEnum.Current).Return(_iEditRow);
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestWorkOrderRepositoryPropertyReturnsMockedRepositoryInstance()
        {
            Assert.AreSame(_workOrderRepository, _target.WorkOrderRepository);
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestWorkOrderPropertyUsesMockedValueIfPresent()
        {
            var workOrder = new WorkOrder();
            _target.SetHiddenFieldValueByName("_workOrder", workOrder);
            Assert.AreSame(workOrder, _target.WorkOrder);
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestWorkOrderPropertyUsesWorkOrderRepositoryToGetWorkOrderValueByWorkOrderID()
        {
            var workOrderID = 1;
            var workorder = new WorkOrder {
                WorkOrderID = workOrderID
            };
            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(workOrderID);
                SetupResult.For(_workOrderRepository.Get(workOrderID))
                    .Return(workorder);
            }
            using (_mocks.Playback())
            {
                Assert.AreSame(workorder, _target.WorkOrder);
            }
        }

        [TestMethod]
        public void TestSettingWorkOrderIDSetsSelectParameterForDataSource()
        {
            var expectedID = 1;
            var param = new Parameter("WorkOrderID");
            _odsSpoilsParameters.Add(param);

            _mocks.ReplayAll();

            _target.WorkOrderID = expectedID;

            Assert.AreEqual(expectedID.ToString(), param.DefaultValue);
        }

        [TestMethod]
        public void TestQuantityPropertyReturnsEnteredDecimalValueOfQuantityControl()
        {
            var expected = (60.5).ToString();
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_txtQuantity.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.Quantity);
            }
        }

        [TestMethod]
        public void TestSpoilStorageLocationIDPropertyReturnsSelectedIntValueOfSpoilStorageLocationIDControl()
        {
            var expected = 15.ToString();
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlSpoilStorageLocation.SelectedValue).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.SpoilStorageLocationID);
            }
        }

        #endregion

        #region Event Handler Tests

        #region Page Events

        [TestMethod]
        public void TestPagePrerenderHidesInsertRowAndEditControlsWhenCurrentMvpModeIsReadOnly()
        {
            var workOrderID = 1;
            var workOrder = new WorkOrder {
                WorkOrderID = workOrderID,
                OperatingCenterID = 10
            };
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(workOrderID);
                SetupResult.For(_workOrderRepository.Get(workOrderID))
                    .Return(workOrder);
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.
                            CURRENT_MVP_MODE)).Return(DetailViewMode.ReadOnly);
                _iFooterRow.Visible = false;
                _lbEdit.Visible = false;
                _lbDelete.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        [TestMethod]
        public void TestPagePrerenderDoesNotHideInsertRowOrEditControlsWhenCurrentMvpModeIsNotReadOnly()
        {
            var workOrderID = 1;
            var workOrder = new WorkOrder {
                WorkOrderID = workOrderID,
                OperatingCenterID = 10
            };
            SetupGridView();

            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                if (mode == DetailViewMode.ReadOnly) continue;

                using (_mocks.Record())
                {
                    SetupResult.For(
                        _viewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                WORK_ORDER_ID))
                        .Return(workOrderID);
                    SetupResult.For(_workOrderRepository.Get(workOrderID))
                        .Return(workOrder);
                    SetupResult.For(
                        _viewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                CURRENT_MVP_MODE)).Return(mode);

                    DoNotExpect.Call(() => _iFooterRow.Visible = false);
                    DoNotExpect.Call(() => _lbEdit.Visible = false);
                    DoNotExpect.Call(() => _lbDelete.Visible = false);
                }

                using (_mocks.Playback())
                {
                    InvokeEventByName(_target, "Page_Prerender");
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
                SetupGridView();
            }

            _mocks.ReplayAll();
        }
        
        [TestMethod]
        public void TestPagePrerenderSetsOperatingCenterIDForObjectDataSources()
        {
            var workOrderID = 1;
            var workOrder = new WorkOrder {
                WorkOrderID = workOrderID,
                OperatingCenterID = 10
            };
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult
                    .For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(workOrderID);
                SetupResult
                    .For(_workOrderRepository.Get(workOrderID))
                    .Return(workOrder);
                _odsOperatingCenterSpoilStorageLocations
                    .SetDefaultSelectParameterValue(
                    WorkOrderSpoilsForm.SpoilsParameterNames.
                        OPERATING_CENTER_ID,
                    workOrder.OperatingCenterID.ToString());
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        #endregion

        #region Control Events

        [TestMethod]
        public void TestODSSpoilsInsertingSetsInputParameters()
        {
            int expectedWorkOrderID = 12;
            string expectedQuantity = "66.5",
                   expectedSpoilStorageLocationID = "2";
            var args =
                new ObjectDataSourceMethodEventArgs(new OrderedDictionary());
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(expectedWorkOrderID);
                SetupResult.For(_txtQuantity.Text).Return(expectedQuantity);
                SetupResult.For(_ddlSpoilStorageLocation.SelectedValue).Return(
                    expectedSpoilStorageLocationID);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "odsSpoils_Inserting",
                    new object[] {
                        null, args
                    });

                Assert.AreEqual(expectedWorkOrderID,
                    args.InputParameters[
                        WorkOrderSpoilsForm.SpoilsParameterNames.WORK_ORDER_ID]);
                Assert.AreEqual(expectedQuantity,
                    args.InputParameters[
                        WorkOrderSpoilsForm.SpoilsParameterNames.QUANTITY]);
                Assert.AreEqual(expectedSpoilStorageLocationID,
                    args.InputParameters[
                        WorkOrderSpoilsForm.SpoilsParameterNames.SPOIL_STORAGE_LOCATION_ID]);
            }
        }

        [TestMethod]
        public void TestLBInsertClickCallsInsertOnDataSourceWhenPageIsValid()
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
        public void TestLBInsertClickDoesNotCallInsertOnDataSourceWhenPageIsNotValid()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IsValid).Return(false);
                DoNotExpect.Call(() => _dataSource.Insert());
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "lbInsert_Click");
            }
        }

        #endregion

        #endregion
    }

    internal class TestWorkOrderSpoilsFormBuilder : TestDataBuilder<TestWorkOrderSpoilsForm>
    {
        #region Private Members

        private IObjectDataSource _dataSource,
                                  _odsSpoilStorageLocations;
        private IGridView _gvSpoils;
        private IViewState _viewState;
        private IPage _page;
        private IRepository<WorkOrder> _workOrderRepository;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderSpoilsForm Build()
        {
            var obj = new TestWorkOrderSpoilsForm();
            if (_dataSource != null)
                obj.SetODSSpoils(_dataSource);
            if (_odsSpoilStorageLocations != null)
                obj.SetODSSpoilStorageLocations(_odsSpoilStorageLocations);
            if (_gvSpoils != null)
                obj.SetGVSpoils(_gvSpoils);
            if (_viewState != null)
                obj.SetViewState(_viewState);
            if (_page != null)
                obj.SetPage(_page);
            if (_workOrderRepository != null)
                obj.SetRepository(_workOrderRepository);
            return obj;
        }

        public TestWorkOrderSpoilsFormBuilder WithDataSource(IObjectDataSource source)
        {
            _dataSource = source;
            return this;
        }

        public TestWorkOrderSpoilsFormBuilder WithGridView(IGridView spoils)
        {
            _gvSpoils = spoils;
            return this;
        }

        public TestWorkOrderSpoilsFormBuilder WithViewState(IViewState state)
        {
            _viewState = state;
            return this;
        }

        public TestWorkOrderSpoilsFormBuilder WithPage(IPage page)
        {
            _page = page;
            return this;
        }

        public TestWorkOrderSpoilsFormBuilder WithWorkOrderRepository(IRepository<WorkOrder> repository)
        {
            _workOrderRepository = repository;
            return this;
        }

        public TestWorkOrderSpoilsFormBuilder WithODSSpoilStorageLocations(IObjectDataSource locations)
        {
            _odsSpoilStorageLocations = locations;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderSpoilsForm : WorkOrderSpoilsForm
    {
        #region Exposed Methods

        public void SetODSSpoils(IObjectDataSource source)
        {
            odsSpoils = source;
        }

        public void SetGVSpoils(IGridView spoils)
        {
            gvSpoils = spoils;
        }

        public void SetViewState(IViewState state)
        {
            _iViewState = state;
        }

        public void SetPage(IPage page)
        {
            _iPage = page;
        }

        public void SetRepository(IRepository<WorkOrder> repository)
        {
            _workOrderRepository = repository;
        }

        public void SetODSSpoilStorageLocations(IObjectDataSource locations)
        {
            odsOperatingCenterSpoilStorageLocations = locations;
        }

        #endregion
    }
}
