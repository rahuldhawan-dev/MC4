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
    /// Summary description for WorkOrderMaterialsUsedFormTest.
    /// </summary>
    [TestClass]
    public class WorkOrderMaterialsUsedFormTest : EventFiringTestClass
    {
        #region Private Members

        private IPage _iPage;
        private IDropDownList _ddlPartNumber,
                              _ddlPartNumberEdit,
                              _ddlStockLocation,
                              _ddlStockLocationEdit;
        private ITextBox _txtNonStockDescription, _txtQuantity;
        private ILinkButton _lbEdit, _lbDelete;
        private IGridViewRow _iFooterRow, _iEditRow;
        private IGridViewRowCollection _rowCollection;
        private IEnumerator<IGridViewRow> _rowCollectionEnum;
        private IViewState _viewState;
        private IGridView _gvMaterialsUsed;

        private IObjectDataSource _odsMaterialsUsed,
                                  _odsActiveStockLocations,
                                  _odsAllStockLocations,
                                  _odsOperatingCenterStockedMaterials;
        private ParameterCollection _odsMaterialsUsedParameters;
        private IRepository<WorkOrder> _workOrderRepository;

        private TestWorkOrderMaterialsUsedForm _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _gvMaterialsUsed)
                .DynamicMock(out _odsMaterialsUsed)
                .DynamicMock(out _odsOperatingCenterStockedMaterials)
                .DynamicMock(out _odsActiveStockLocations)
                .DynamicMock(out _odsAllStockLocations)
                .DynamicMock(out _viewState)
                .DynamicMock(out _ddlPartNumber)
                .DynamicMock(out _ddlPartNumberEdit)
                .DynamicMock(out _ddlStockLocation)
                .DynamicMock(out _ddlStockLocationEdit)
                .DynamicMock(out _txtNonStockDescription)
                .DynamicMock(out _txtQuantity)
                .DynamicMock(out _rowCollection)
                .DynamicMock(out _iFooterRow)
                .DynamicMock(out _iEditRow)
                .DynamicMock(out _iPage)
                .DynamicMock(out _lbEdit)
                .DynamicMock(out _lbDelete)
                .DynamicMock(out _rowCollectionEnum)
                .DynamicMock(out _workOrderRepository);

            _odsMaterialsUsedParameters = new ParameterCollection();
            SetupResult.For(_odsMaterialsUsed.SelectParameters).Return(
                _odsMaterialsUsedParameters);

            _target = new TestWorkOrderMaterialsUsedFormBuilder()
                .WithGVMaterialsUsed(_gvMaterialsUsed)
                .WithODSMaterialsUsed(_odsMaterialsUsed)
                .WithODSOperatingCenterStockedMaterials(_odsOperatingCenterStockedMaterials)
                .WithODSActiveStockLocations(_odsActiveStockLocations)
                .WithODSAllStockLocations(_odsAllStockLocations)
                .WithWorkOrderRepository(_workOrderRepository)
                .WithViewState(_viewState)
                .WithIPage(_iPage);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        private void SetupGridView()
        {
            const int editIndex = -1;

            SetupResult.For(_gvMaterialsUsed.IRows).Return(_rowCollection);
            SetupResult.For(_gvMaterialsUsed.EditIndex).Return(editIndex);
            SetupResult.For(_gvMaterialsUsed.IFooterRow).Return(_iFooterRow);
            SetupResult.For(_rowCollection[editIndex]).Return(_iEditRow);
            SetupResult.For(_rowCollection.GetEnumerator())
                .Return(_rowCollectionEnum);

            SetupResult
                .For(_iFooterRow.FindIControl<IDropDownList>(
                    WorkOrderMaterialsUsedForm.ControlIDs.MATERIAL_ID))
                .Return(_ddlPartNumber);
            SetupResult
                .For(_iFooterRow.FindIControl<ITextBox>(
                    WorkOrderMaterialsUsedForm.ControlIDs.DESCRIPTION))
                .Return(_txtNonStockDescription);
            SetupResult
                .For(_iFooterRow.FindIControl<ITextBox>(
                    WorkOrderMaterialsUsedForm.ControlIDs.QUANTITY))
                .Return(_txtQuantity);
            SetupResult
                .For(_iFooterRow.FindIControl<IDropDownList>(
                    WorkOrderMaterialsUsedForm.ControlIDs.STOCK_LOCATION_ID))
                .Return(_ddlStockLocation);

            SetupResult
                .For(_iEditRow.FindIControl<IDropDownList>(
                    WorkOrderMaterialsUsedForm.ControlIDs.MATERIAL_ID_EDIT))
                .Return(_ddlPartNumberEdit);
            SetupResult
                .For(_iEditRow.FindIControl<IDropDownList>(
                    WorkOrderMaterialsUsedForm.ControlIDs.STOCK_LOCATION_ID_EDIT))
                .Return(_ddlStockLocationEdit);
            SetupResult
                .For(_iEditRow.FindIControl<ILinkButton>(
                    WorkOrderMaterialsUsedForm.ControlIDs.EDIT_LINK))
                .Return(_lbEdit);
            SetupResult
                .For(_iEditRow.FindIControl<ILinkButton>(
                    WorkOrderMaterialsUsedForm.ControlIDs.DELETE_LINK))
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
            _odsMaterialsUsedParameters.Add(param);

            _mocks.ReplayAll();

            _target.WorkOrderID = expectedID;

            Assert.AreEqual(expectedID.ToString(), param.DefaultValue);
        }

        [TestMethod]
        public void TestDescriptionPropertyReturnsStringValueOfDescriptionField()
        {
            var expected = "some string";
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_txtNonStockDescription.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.Description);
            }
        }

        [TestMethod]
        public void TestMaterialIDPropertyReturnsSelectedIntValueOfMaterialIDControl()
        {
            var expected = 12.ToString();
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumber.SelectedValue).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.MaterialID);
            }
        }

        [TestMethod]
        public void TestMaterialIDEditPropertyReturnsSelectedIntValueOfMaterialIDEditControl()
        {
            var expected = 21.ToString();
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumberEdit.SelectedValue).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.MaterialIDEdit);
            }
        }

        [TestMethod]
        public void TestQuantityPropertyReturnsStringValueOfQuantityField()
        {
            var expected = 25.ToString();
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
        public void TestStockLocationIDPropertyReturnsSelectedIntValueOfStockLocationIDControl()
        {
            SetupGridView();
            var expected = 13.ToString();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlStockLocation.SelectedValue).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.StockLocationID);
            }
        }

        [TestMethod]
        public void TestStockLocationIDEditPropertyReturnsSelectedIntValueOfStockLocationIDEditControl()
        {
            SetupGridView();
            var expected = 31.ToString();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlStockLocationEdit.SelectedValue).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.StockLocationIDEdit);
            }
        }

        [TestMethod]
        public void TestSettingCssClassSetsCssClassOfGridView()
        {
            var cssClass = "proletariat-box";

            using (_mocks.Record())
            {
                // from each, according to their ability
                _gvMaterialsUsed.CssClass = cssClass;
            }

            using (_mocks.Playback())
            {
                // to each, according to their need
                _target.TableCssClass = cssClass;
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
        public void TestPagePrerenderHidesInsertRowAndEditControlsWhenWorkOrderMaterialsAlreadyApproved()
        {
            var workOrderID = 1;
            var workOrder = new WorkOrder {
                WorkOrderID = workOrderID,
                OperatingCenterID = 10,
                MaterialsApprovedOn = DateTime.Now,
                MaterialsApprovedBy = new Employee()
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
                                                   CURRENT_MVP_MODE)).Return(DetailViewMode.Edit);
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
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(workOrderID);
                SetupResult.For(_workOrderRepository.Get(workOrderID))
                    .Return(workOrder);
                _odsActiveStockLocations.SetDefaultSelectParameterValue(
                    WorkOrderMaterialsUsedForm.ParameterNames.
                        OPERATING_CENTER_ID,
                    workOrder.OperatingCenterID.ToString());
                _odsAllStockLocations.SetDefaultSelectParameterValue(
                    WorkOrderMaterialsUsedForm.ParameterNames.
                        OPERATING_CENTER_ID,
                    workOrder.OperatingCenterID.ToString());
                _odsOperatingCenterStockedMaterials.SetDefaultSelectParameterValue(
                    WorkOrderMaterialsUsedForm.ParameterNames.
                        OPERATING_CENTER_ID,
                    workOrder.OperatingCenterID.ToString());
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        #endregion

        #region Validation Events

        [TestMethod]
        public void TestDDlPartNumberValidateSetsArgsValidWhenMaterialAndStockLocationChosen()
        {
            var expected = "not null or empty";
            var args = new ServerValidateEventArgs(null, false);
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumber.SelectedValue).Return(expected);
                SetupResult.For(_ddlStockLocation.SelectedValue).Return(expected);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlPartNumber_Validate",
                    new object[] {
                        null, args
                    });
                Assert.IsTrue(args.IsValid);
            }
        }

        [TestMethod]
        public void TestDDLPartNumberValidateSetsArgsValidWhenNoMaterialOrStockLocationChosen()
        {
            var args = new ServerValidateEventArgs(null, false);
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumber.SelectedValue).Return(null);
                SetupResult.For(_ddlStockLocation.SelectedValue).Return(null);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlPartNumber_Validate",
                    new object[] {
                        null, args
                    });
                Assert.IsTrue(args.IsValid);
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            SetupGridView();
            args = new ServerValidateEventArgs(null, false);

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumber.SelectedValue).Return(String.Empty);
                SetupResult.For(_ddlStockLocation.SelectedValue).Return(String.Empty);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlPartNumber_Validate",
                    new object[] {
                        null, args
                    });
                Assert.IsTrue(args.IsValid);
            }
        }

        [TestMethod]
        public void TestDDLPartNumberValidSetsArgsInValidWhenMaterialButNoStockLocationChosen()
        {
            var args = new ServerValidateEventArgs(null, true);
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumber.SelectedValue).Return("not empty or null");
                SetupResult.For(_ddlStockLocation.SelectedValue).Return(null);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlPartNumber_Validate",
                    new object[] {
                        null, args
                    });
                Assert.IsFalse(args.IsValid);
            }
        }

        [TestMethod]
        public void TestDDLPartNumberValidSetsArgsInValidWhenNoMaterialButStockLocationChosen()
        {
            var args = new ServerValidateEventArgs(null, true);
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumber.SelectedValue).Return(null);
                SetupResult.For(_ddlStockLocation.SelectedValue).Return("not empty or null");
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlPartNumber_Validate",
                    new object[] {
                        null, args
                    });
                Assert.IsFalse(args.IsValid);
            }
        }

        [TestMethod]
        public void TestDDlPartNumberEditValidateSetsArgsValidWhenMaterialAndStockLocationChosen()
        {
            var expected = "not null or empty";
            var args = new ServerValidateEventArgs(null, false);
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumberEdit.SelectedValue).Return(expected);
                SetupResult.For(_ddlStockLocationEdit.SelectedValue).Return(expected);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlPartNumberEdit_Validate",
                    new object[] {
                        null, args
                    });
                Assert.IsTrue(args.IsValid);
            }
        }

        [TestMethod]
        public void TestDDLPartNumberEditValidateSetsArgsValidWhenNoMaterialOrStockLocationChosen()
        {
            var args = new ServerValidateEventArgs(null, false);
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumberEdit.SelectedValue).Return(null);
                SetupResult.For(_ddlStockLocationEdit.SelectedValue).Return(null);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlPartNumberEdit_Validate",
                    new object[] {
                        null, args
                    });
                Assert.IsTrue(args.IsValid);
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            SetupGridView();
            args = new ServerValidateEventArgs(null, false);

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumberEdit.SelectedValue).Return(String.Empty);
                SetupResult.For(_ddlStockLocation.SelectedValue).Return(String.Empty);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlPartNumberEdit_Validate",
                    new object[] {
                        null, args
                    });
                Assert.IsTrue(args.IsValid);
            }
        }

        [TestMethod]
        public void TestDDLPartNumberEditValidSetsArgsInValidWhenMaterialButNoStockLocationChosen()
        {
            var args = new ServerValidateEventArgs(null, true);
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumberEdit.SelectedValue).Return("not empty or null");
                SetupResult.For(_ddlStockLocationEdit.SelectedValue).Return(null);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlPartNumberEdit_Validate",
                    new object[] {
                        null, args
                    });
                Assert.IsFalse(args.IsValid);
            }
        }

        [TestMethod]
        public void TestDDLPartNumberEditValidSetsArgsInValidWhenNoMaterialButStockLocationChosen()
        {
            var args = new ServerValidateEventArgs(null, true);
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlPartNumberEdit.SelectedValue).Return(null);
                SetupResult.For(_ddlStockLocationEdit.SelectedValue).Return("not empty or null");
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlPartNumberEdit_Validate",
                    new object[] {
                        null, args
                    });
                Assert.IsFalse(args.IsValid);
            }
        }

        #endregion

        #region Control Events

        [TestMethod]
        public void TestODSMaterialsUsedInsertingSetsInputParameters()
        {
            int expectedWorkOrderID = 12;
            string expectedMaterialID = "123",
                   expectedDescription = "description",
                   expectedQuantity = "2",
                   expectedStockLocationID = "456";
            var args =
                new ObjectDataSourceMethodEventArgs(new OrderedDictionary());
            SetupGridView();

            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(expectedWorkOrderID);
                SetupResult.For(_ddlPartNumber.SelectedValue).Return(
                    expectedMaterialID);
                SetupResult.For(_txtNonStockDescription.Text).Return(
                    expectedDescription);
                SetupResult.For(_txtQuantity.Text).Return(expectedQuantity);
                SetupResult.For(_ddlStockLocation.SelectedValue).Return(
                    expectedStockLocationID);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "odsMaterialsUsed_Inserting",
                    new object[] {
                        null, args
                    });

                Assert.AreEqual(expectedWorkOrderID,
                    args.InputParameters[
                        WorkOrderMaterialsUsedForm.MaterialsUsedParameterNames.
                            WORK_ORDER_ID]);
                Assert.AreEqual(expectedMaterialID,
                    args.InputParameters[
                        WorkOrderMaterialsUsedForm.MaterialsUsedParameterNames.
                            MATERIAL_ID]);
                Assert.AreEqual(expectedDescription,
                    args.InputParameters[
                        WorkOrderMaterialsUsedForm.MaterialsUsedParameterNames.
                            DESCRIPTION]);
                Assert.AreEqual(expectedQuantity,
                    args.InputParameters[
                        WorkOrderMaterialsUsedForm.MaterialsUsedParameterNames.
                            QUANTITY]);
                Assert.AreEqual(expectedStockLocationID,
                    args.InputParameters[
                        WorkOrderMaterialsUsedForm.MaterialsUsedParameterNames.
                            STOCK_LOCATION_ID]);
            }
        }

        [TestMethod]
        public void TestLBInsertClickCallsInsertOnDataSourceWhenPageIsValid()
        {
            using (_mocks.Record())
            {
                Expect.Call(_iPage.IsValid).Return(true);
                Expect.Call(_odsMaterialsUsed.Insert()).Return(1);
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
                DoNotExpect.Call(() => _odsMaterialsUsed.Insert());
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "lbInsert_Click");
            }
        }

        #endregion

        #endregion
    }

    internal class TestWorkOrderMaterialsUsedFormBuilder : TestDataBuilder<TestWorkOrderMaterialsUsedForm>
    {
        #region Private Members

        private IPage _iPage;
        private IGridView _gvMaterialsUsed;

        private IObjectDataSource _odsMaterialsUsed,
                                  _odsActiveStockLocations,
                                  _odsAllStockLocations,
                                  _odsOperatingCenterStockedMaterials;
        private IViewState _viewState;
        private IRepository<WorkOrder> _workOrderRepository;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderMaterialsUsedForm Build()
        {
            var obj = new TestWorkOrderMaterialsUsedForm();
            if (_gvMaterialsUsed != null)
                obj.SetGVMaterialsUsed(_gvMaterialsUsed);
            if (_workOrderRepository != null)
                obj.SetWorkOrderRepository(_workOrderRepository);
            if (_odsMaterialsUsed != null)
                obj.SetODSMaterialsUsed(_odsMaterialsUsed);
            if (_odsActiveStockLocations!= null)
                obj.SetODSActiveStockLocations(_odsActiveStockLocations);
            if (_odsAllStockLocations != null)
                obj.SetODSAllStockLocations(_odsAllStockLocations);
            if (_odsOperatingCenterStockedMaterials != null)
                obj.SetODSOperatingCenterStockedMaterials(
                    _odsOperatingCenterStockedMaterials);
            if (_viewState != null)
                obj.SetViewState(_viewState);
            if (_iPage != null)
                obj.SetIPage(_iPage);
            return obj;
        }

        public TestWorkOrderMaterialsUsedFormBuilder WithGVMaterialsUsed(IGridView view)
        {
            _gvMaterialsUsed = view;
            return this;
        }

        public TestWorkOrderMaterialsUsedFormBuilder WithODSMaterialsUsed(IObjectDataSource source)
        {
            _odsMaterialsUsed = source;
            return this;
        }

        public TestWorkOrderMaterialsUsedFormBuilder WithViewState(IViewState state)
        {
            _viewState = state;
            return this;
        }

        public TestWorkOrderMaterialsUsedFormBuilder WithIPage(IPage iPage)
        {
            _iPage = iPage;
            return this;
        }

        public TestWorkOrderMaterialsUsedFormBuilder WithODSOperatingCenterStockedMaterials(IObjectDataSource source)
        {
            _odsOperatingCenterStockedMaterials = source;
            return this;
        }

        public TestWorkOrderMaterialsUsedFormBuilder WithODSActiveStockLocations(IObjectDataSource source)
        {
            _odsActiveStockLocations = source;
            return this;
        }

        public TestWorkOrderMaterialsUsedFormBuilder WithODSAllStockLocations(IObjectDataSource source)
        {
            _odsAllStockLocations = source;
            return this;
        }

        public TestWorkOrderMaterialsUsedFormBuilder WithWorkOrderRepository(IRepository<WorkOrder> workOrderRepository)
        {
            _workOrderRepository = workOrderRepository;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderMaterialsUsedForm : WorkOrderMaterialsUsedForm
    {
        #region Exposed Methods

        public void SetGVMaterialsUsed(IGridView view)
        {
            gvMaterialsUsed = view;
        }

        public void SetODSMaterialsUsed(IObjectDataSource source)
        {
            odsMaterialsUsed = source;
        }

        public void SetViewState(IViewState state)
        {
            _iViewState = state;
        }

        public void SetIPage(IPage iPage)
        {
            _iPage = iPage;
        }

        public void SetWorkOrderRepository(IRepository<WorkOrder> repository)
        {
            _workOrderRepository = repository;
        }

        public void SetODSActiveStockLocations(IObjectDataSource source)
        {
            odsActiveStockLocations = source;
        }

        public void SetODSAllStockLocations(IObjectDataSource source)
        {
            odsAllStockLocations = source;
        }

        public void SetODSOperatingCenterStockedMaterials(IObjectDataSource source)
        {
            odsOperatingCenterStockedMaterials = source;
        }

        #endregion
    }
}
