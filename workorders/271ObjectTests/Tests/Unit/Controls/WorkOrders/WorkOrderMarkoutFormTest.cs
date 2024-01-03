using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Utilities.StructureMap;
using Moq;
using Rhino.Mocks;
using StructureMap;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using Markout = WorkOrders.Model.Markout;
using MarkoutRequirementRepository = WorkOrders.Model.MarkoutRequirementRepository;
using OperatingCenter = WorkOrders.Model.OperatingCenter;
using WorkOrder = WorkOrders.Model.WorkOrder;
using MarkoutType = WorkOrders.Model.MarkoutType;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderMarkoutFormTest
    /// </summary>
    /// TODO: Code Review Something is up with this test class. It does not need a detail control
    /// other than for the update panel and its markoutError label.
    [TestClass]
    public class WorkOrderMarkoutFormTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailControl _detailControl;
        private ILabel _lblMarkoutError, _lblTypeNeeded;
        private IGridView _gvMarkouts;
        private ITextBox _txtMarkoutNumber,
                         _ccDateOfRequest, 
                         _ccMarkoutExpirationDate,
                         _ccMarkoutReadyDate,
                         _txtNote;
        private IDropDownList _ddlMarkoutType;
        private ILinkButton _editLink, _deleteLink;

        private IViewState _viewState;
        
        private IObjectDataSource _odsMarkouts;
        private IUpdatePanel _upMarkouts;

        private IGridViewRow _iFooterRow, _iEditRow;
        private IGridViewRowCollection _rowCollection;
        private IEnumerator<IGridViewRow> _rowCollectionEnum;

        private IRepository<WorkOrder> _workOrderRepository;

        private TestWorkOrderMarkoutForm _target;
        private ParameterCollection _odsMarkoutParameters;

        private ISecurityService _securityService;

        private Mock<IAuditLogEntryRepository> _auditLogEntryRepository;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _container = new Container();
            _container.Inject((_auditLogEntryRepository = new Mock<IAuditLogEntryRepository>()).Object);
            MockUsing(_mocks.DynamicMock);
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        private void MockUsing(Func<Type, object[], object> mock)
        {
            _mocks
                .DynamicMock(out _detailControl)
                .DynamicMock(out _lblMarkoutError)
                .DynamicMock(out _lblTypeNeeded)
                .DynamicMock(out _upMarkouts)
                .DynamicMock(out _txtMarkoutNumber)
                .DynamicMock(out _ddlMarkoutType)
                .DynamicMock(out _ccDateOfRequest)
                .DynamicMock(out _ccMarkoutExpirationDate)
                .DynamicMock(out _ccMarkoutReadyDate)
                .DynamicMock(out _gvMarkouts)
                .DynamicMock(out _iFooterRow)
                .DynamicMock(out _iEditRow)
                .DynamicMock(out _rowCollection)
                .DynamicMock(out _viewState)
                .DynamicMock(out _odsMarkouts)
                .DynamicMock(out _editLink)
                .DynamicMock(out _deleteLink)
                .DynamicMock(out _rowCollectionEnum)
                .DynamicMock(out _txtNote)
                .DynamicMock(out _workOrderRepository)
                .DynamicMock(out _securityService);

            _odsMarkoutParameters = new ParameterCollection();
            SetupResult.For(_odsMarkouts.SelectParameters).Return(
                _odsMarkoutParameters);

            _target = new TestWorkOrderMarkoutFormBuilder()
                .WithODSMarkouts(_odsMarkouts)
                .WithUpdatePanel(_upMarkouts)
                .WithViewState(_viewState)
                .WithGridView(_gvMarkouts)
                .WithWorkOrderRepository(_workOrderRepository)
                .WithSecurityService(_securityService);
            
            SetupResult.For(_detailControl.FindIControl<IUpdatePanel>(WorkOrderMarkoutForm.ControlIDs.UPDATE_PANEL)).Return(_upMarkouts);
            SetupResult.For(_upMarkouts.FindIControl<ILabel>(WorkOrderMarkoutForm.ControlIDs.MARKOUT_ERROR_LABEL)).Return(_lblMarkoutError);
            SetupResult.For(_upMarkouts.FindIControl<ILabel>(WorkOrderMarkoutForm.ControlIDs.MARKOUT_TYPE_NEEDED_LABEL)).Return(_lblTypeNeeded);
            
            SetupGridView();
        }

        private void SetupGridView()
        {
            const int editIndex = -1;
            SetupResult.For(_rowCollection[editIndex]).Return(_iEditRow);
            SetupResult.For(_gvMarkouts.IFooterRow).Return(_iFooterRow);
            SetupResult.For(_gvMarkouts.IRows).Return(_rowCollection);
            SetupResult.For(_rowCollection.GetEnumerator()).Return(_rowCollectionEnum);

            SetupResult.For(_iEditRow.FindIControl<ILinkButton>(WorkOrderMarkoutForm.ControlIDs.ROW_DELETE_LINKBUTTON)).Return(_deleteLink);
            SetupResult.For(_iEditRow.FindIControl<ILinkButton>(WorkOrderMarkoutForm.ControlIDs.ROW_EDIT_LINKBUTTON)).Return(_editLink);
            
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderMarkoutForm.ControlIDs.MARKOUT_NUMBER_TEXTBOX)).Return(_txtMarkoutNumber);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderMarkoutForm.ControlIDs.DATE_OF_REQUEST_TEXTBOX)).Return(_ccDateOfRequest);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderMarkoutForm.ControlIDs.DATE_EXPIRATION)).Return(_ccMarkoutExpirationDate);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderMarkoutForm.ControlIDs.DATE_READY)).Return(_ccMarkoutReadyDate);
            SetupResult.For(_iFooterRow.FindIControl<ITextBox>(WorkOrderMarkoutForm.ControlIDs.NOTE_TEXTBOX)).Return(_txtNote);

            SetupResult.For(_iFooterRow.FindIControl<IDropDownList>(WorkOrderMarkoutForm.ControlIDs.MARKOUT_TYPE_DROPDOWNLIST)).Return(_ddlMarkoutType);
            
            var moveNextCalled = false;
            SetupResult.For(_rowCollectionEnum.MoveNext()).Do((Func<bool>)delegate
            {
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
        public void TestMarkoutsDataSourceInsertingCommandSetsSomeNecessaryValuesOnEventArgs()
        {
            var expectedWorkOrderID = 1;
            var expectedMarkoutNumber = 2.ToString();
            var expectedMarkoutTypeID = 20.ToString();
            var expectedDateOfRequest = DateTime.Now.ToString();
            var expectedNote = "foo";
            var expectedCreatorID = 666;

            SetupResult.For(_viewState.GetValue(WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID)).Return(expectedWorkOrderID);
            SetupResult.For(_txtMarkoutNumber.Text).Return(expectedMarkoutNumber);
            SetupResult.For(_ddlMarkoutType.SelectedValue).Return(expectedMarkoutTypeID);
            SetupResult.For(_ccDateOfRequest.Text).Return(expectedDateOfRequest);
            SetupResult.For(_ccMarkoutExpirationDate.Text).Return(expectedDateOfRequest);
            SetupResult.For(_ccMarkoutReadyDate.Text).Return(expectedDateOfRequest);
            SetupResult.For(_txtNote.Text).Return(expectedNote);
            SetupResult.For(_securityService.GetEmployeeID()).Return(expectedCreatorID);

            var args = new ObjectDataSourceMethodEventArgs(new OrderedDictionary());

            args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.WORK_ORDER_ID
                ] = null;
            args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.MARKOUT_NUMBER
                ] = null;
            args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.MARKOUT_TYPE_ID
                ] = null;
            args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.DATE_OF_REQUEST
                ] = null;
            args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.NOTE
                ] = null;
            args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.CREATOR_ID
                ] = null;

            _mocks.ReplayAll();

            InvokeEventByName(_target, "odsMarkouts_Inserting", new object[] {
                null, args
            });

            Assert.AreEqual(expectedWorkOrderID, args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.WORK_ORDER_ID
                ]);
            Assert.AreEqual(expectedMarkoutNumber, args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.MARKOUT_NUMBER
                ]);
            Assert.AreEqual(expectedMarkoutTypeID, args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.MARKOUT_TYPE_ID
                ]);
            Assert.AreEqual(expectedDateOfRequest, args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.DATE_OF_REQUEST
                ]);
            Assert.AreEqual(expectedNote, args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.NOTE
                ]);
            Assert.AreEqual(expectedCreatorID, args.InputParameters[
                WorkOrderMarkoutForm.MarkoutsParameterNames.CREATOR_ID
                ]);
        }

        [TestMethod]
        public void TestDateOfRequestPropertyReturnsStringValueOfDateIssuedField()
        {
            var expected = DateTime.Now.ToString();

            using (_mocks.Record())
            {
                SetupResult.For(_ccDateOfRequest.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.DateOfRequest);
            }
        }

        [TestMethod]
        public void TestReadyDatePropertyReturnsDateTimeStringValue()
        {
            var expected = DateTime.Now.ToString();

            using (_mocks.Record())
            {
                SetupResult.For(_ccMarkoutReadyDate.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.ReadyDate);
            }
        }

        [TestMethod]
        public void TestExpirationDatePropertyReturnsDateTimeStringValue()
        {
            var expected = DateTime.Now.ToString();

            using (_mocks.Record())
            {
                SetupResult.For(_ccMarkoutExpirationDate.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.ExpirationDate);
            }
        }

        [TestMethod]
        public void TestMarkoutNumberPropertyReturnsStringValueOfField()
        {
            var expected = "whatever";
            
            using (_mocks.Record())
            {
                SetupResult.For(_txtMarkoutNumber.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.MarkoutNumber);
            }
        }

        [TestMethod]
        public void TestMarkoutTypeIDPropertyReturnsSelectedValueOfField()
        {
            var expected = 10.ToString();

            using (_mocks.Record())
            {
                SetupResult.For(_ddlMarkoutType.SelectedValue).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.MarkoutTypeID);
            }
        }

        [TestMethod]
        public void TestMarkoutErrorPropertyReturnsLabel()
        {
            _mocks.ReplayAll();
            Assert.AreEqual(_lblMarkoutError, _target.MarkoutError);
        }

        [TestMethod]
        public void TestSettingWorkOrderIDSetsSelectParameterForDataSource()
        {
            var expectedID = 1;
            var param = new Parameter("WorkOrderID");
            var order = new WorkOrder();

            _odsMarkoutParameters.Add(param);

            using (_mocks.Record())
            {
                SetupResult.For(_workOrderRepository.Get(expectedID)).Return(order);
            }
            using (_mocks.Playback())
            {
                _target.WorkOrderID = expectedID;
                Assert.AreEqual(expectedID.ToString(), param.DefaultValue);
            }
        }

        [TestMethod]
        public void TestSettingWorkOrderIDDoesNotSetMarkoutTypeNeededWhenNull()
        {
            var expectedID = 1;
            var param = new Parameter("WorkOrderID");
            var order = new WorkOrder();

            _odsMarkoutParameters.Add(param);

            using (_mocks.Record())
            {
                SetupResult.For(_workOrderRepository.Get(expectedID)).Return(order);
            }
            using (_mocks.Playback())
            {
                _target.WorkOrderID = expectedID;
                Assert.AreEqual(expectedID.ToString(), param.DefaultValue);
                Assert.IsNull(_target.MarkoutTypeNeeded.Text);
            }
        }

        [TestMethod]
        public void TestSettingWorkOrderIDSetsMarkoutTypeNeededToDescription()
        {
            var expectedID = 1;
            var description = "bar";
            var notes = "notes";
            var param = new Parameter("WorkOrderID");
            var order = new WorkOrder
            {
                MarkoutTypeNeeded = new global::WorkOrders.Model.MarkoutType
                {
                    Description = description
                },
                RequiredMarkoutNote = notes
            };

            _odsMarkoutParameters.Add(param);

            using (_mocks.Record())
            {
                SetupResult.For(_workOrderRepository.Get(expectedID)).Return(order);
                _lblTypeNeeded.Text = String.Format(WorkOrderMarkoutForm.MARKOUT_TYPE_LABEL_FORMAT_STRING,
                    description, notes);
            }
            using (_mocks.Playback())
            {
                _target.WorkOrderID = expectedID;
                Assert.AreEqual(expectedID.ToString(), param.DefaultValue);
            }
        }

        #endregion

        #region Event Handlers

        [TestMethod]
        public void TestPagePrerenderHidesInsertRowWhenCurrentMvpModeIsReadOnly()
        {
            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE))
                    .Return(DetailViewMode.ReadOnly);

                _iFooterRow.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        [TestMethod]
        public void TestPagePrerenderHidesEditControlsWhenCurrentMvpModeIsReadOnly()
        {
            _target.MarkoutRequirementID =
                MarkoutRequirementRepository.Indices.ROUTINE;
            using (_mocks.Record())
            {
                SetupResult.For(
                    _viewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE))
                    .Return(DetailViewMode.ReadOnly);

                _editLink.Visible = false;
                _deleteLink.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        [TestMethod]
        public void TestPagePrerenderHidesInsertRowWhenMarkoutRequirementIsNone()
        {
            _target.MarkoutRequirementID = MarkoutRequirementRepository.Indices.NONE;
            //It needs to do this when its not ReadOnly
            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                if (mode == DetailViewMode.ReadOnly) continue;

                using (_mocks.Record())
                {
                    SetupResult.For(
                        _viewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                CURRENT_MVP_MODE)).Return(mode);

                    _iFooterRow.Visible = false;
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
        public void TestPagePrerenderDoesNotHideInsertRowWhenCurrentMvpModeIsNotReadOnly()
        {
            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                if (mode == DetailViewMode.ReadOnly) continue;

                using (_mocks.Record())
                {
                    SetupResult.For(
                        _viewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                CURRENT_MVP_MODE)).Return(mode);

                    _iFooterRow.Visible = true;
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

        #endregion

        #region Control Events

        [TestMethod]
        public void TestLBInsertClickCallsInsertOnDataSource()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_odsMarkouts.Insert()).Return(1);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "lbInsert_Click");
            }
        }

        // TODO: Code Review
        [TestMethod]
        public void TestLBCancelClickDoesNothing()
        {
            _odsMarkouts = _mocks.CreateMock<IObjectDataSource>();
            _upMarkouts = _mocks.CreateMock<IUpdatePanel>();
            _viewState = _mocks.CreateMock<IViewState>();
            _gvMarkouts = _mocks.CreateMock<IGridView>();

            _target = new TestWorkOrderMarkoutFormBuilder()
                .WithODSMarkouts(_odsMarkouts)
                .WithUpdatePanel(_upMarkouts)
                .WithViewState(_viewState)
                .WithGridView(_gvMarkouts);

            using (_mocks.Record())
            {
                
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target,"lbCancel_Click");
            }
        }

        #endregion

        [TestMethod]
        public void TestInsertedLogsInsert()
        {
            var operatingCenter = new OperatingCenter {  MarkoutsEditable = true };
            var order = new WorkOrder { OperatingCenter = operatingCenter, WorkOrderID = 12};
            var markout = new Markout {
                WorkOrder = order,
                MarkoutID = 123,
                DateOfRequest = DateTime.Now,
                ReadyDate = DateTime.Now.AddDays(1),
                ExpirationDate = DateTime.Now.AddDays(2)
            };
            var ds = new MvpObjectDataSource();
            ds.SelectParameters.Add(new Parameter("WorkOrderID"));
            _target.SetODSMarkouts(ds);
            SetupResult.For(_viewState.GetValue(WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID)).Return(order.WorkOrderID);


            using (_mocks.Record())
            {
                SetupResult.For(_workOrderRepository.Get(order.WorkOrderID)).Return(order);
            }

            using (_mocks.Playback())
            {
                _target.WorkOrderID = order.WorkOrderID;
                InvokeEventByName(_target, "odsMarkouts_Inserted");
            }
            _auditLogEntryRepository.Verify(x => x.Save(It.Is<AuditLogEntry>(ale => 
                ale.EntityName == "Markout" 
                && ale.AuditEntryType == "INSERT"
                && ale.EntityId == markout.MarkoutID)), Times.Exactly(3));

        }

        [TestMethod]
        public void TestUpdatingLogsUpdate()
        {
            var operatingCenter = new OperatingCenter { MarkoutsEditable = true };
            var order = new WorkOrder { OperatingCenter = operatingCenter, WorkOrderID = 12 };
            var markout = new Markout
            {
                WorkOrder = order,
                MarkoutID = 123,
                DateOfRequest = DateTime.Now,
                ReadyDate = DateTime.Now.AddDays(1),
                ExpirationDate = DateTime.Now.AddDays(2)
            };
            var ds = new MvpObjectDataSource();
            ds.SelectParameters.Add(new Parameter("WorkOrderID"));
            _target.SetODSMarkouts(ds);
            SetupResult.For(_viewState.GetValue(WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID)).Return(order.WorkOrderID);
            var parameters = new OrderedDictionary();
            parameters.Add("MarkoutTypeID", markout.MarkoutTypeID);
            parameters.Add("DateOfRequest", markout.DateOfRequest);
            parameters.Add("ReadyDate", markout.ReadyDate);
            parameters.Add("ExpirationDate", markout.ExpirationDate);
            parameters.Add("MarkoutNumber", markout.MarkoutNumber);
            parameters.Add("MarkoutID", markout.MarkoutID);
            parameters.Add("Note", markout.Note);
            var args = new ObjectDataSourceMethodEventArgs(parameters);

            using (_mocks.Record())
            {
                SetupResult.For(_workOrderRepository.Get(order.WorkOrderID)).Return(order);
            }

            using (_mocks.Playback())
            {
                _target.WorkOrderID = order.WorkOrderID;
                InvokeEventByName(_target, "odsMarkouts_OnUpdating", null, args);
            }
            _auditLogEntryRepository.Verify(x => x.Save(It.Is<AuditLogEntry>(ale =>
                ale.EntityName == "Markout"
                && ale.AuditEntryType == "UPDATE"
                && ale.EntityId == markout.MarkoutID)), Times.Exactly(3));

        }
    }

    internal class TestWorkOrderMarkoutFormBuilder : TestDataBuilder<TestWorkOrderMarkoutForm>
    {
        private IViewState _viewState;
        private IUpdatePanel _upMarkouts;
        private IGridView _gvMarkouts;
        private IObjectDataSource _odsMarkouts;

        private ITextBox _txtMarkoutNumber,
                         _ccDateOfRequest;
        private IDropDownList _ddlMarkoutType;
        private IRepository<WorkOrder> _workOrderRepository;
        private ISecurityService _securityService;

        public override TestWorkOrderMarkoutForm Build()
        {
            var obj = new TestWorkOrderMarkoutForm();
            if (_odsMarkouts != null)
                obj.SetODSMarkouts(_odsMarkouts);
            if (_txtMarkoutNumber != null)
                obj.SetTXTMarkoutNumber(_txtMarkoutNumber);
            if (_ddlMarkoutType != null)
                obj.SetDDLMarkoutType(_ddlMarkoutType);
            if (_ccDateOfRequest != null)
                obj.SetCCDateOfRequest(_ccDateOfRequest);
            if (_upMarkouts != null)
                obj.SetUpdatePanel(_upMarkouts);
            if (_gvMarkouts != null)
                obj.SetGridView(_gvMarkouts);
            if (_viewState != null)
                obj.SetViewState(_viewState);
            if (_workOrderRepository != null)
                obj.SetWorkOrderRepository(_workOrderRepository);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestWorkOrderMarkoutFormBuilder WithODSMarkouts(IObjectDataSource markouts)
        {
            _odsMarkouts = markouts;
            return this;
        }

        public TestWorkOrderMarkoutFormBuilder WithTXTMarkoutNumber(ITextBox number)
        {
            _txtMarkoutNumber = number;
            return this;
        }

        public TestWorkOrderMarkoutFormBuilder WithDDLMarkoutType(IDropDownList markoutType)
        {
            _ddlMarkoutType = markoutType;
            return this;
        }

        public TestWorkOrderMarkoutFormBuilder WithCCDateOfRequest(ITextBox request)
        {
            _ccDateOfRequest = request;
            return this;
        }

        public TestWorkOrderMarkoutFormBuilder WithViewState(IViewState state)
        {
            _viewState = state;
            return this;
        }

        public TestWorkOrderMarkoutFormBuilder WithUpdatePanel(IUpdatePanel panel)
        {
            _upMarkouts = panel;
            return this;
        }

        public TestWorkOrderMarkoutFormBuilder WithGridView(IGridView gv)
        {
            _gvMarkouts = gv;
            return this;
        }

        public TestWorkOrderMarkoutFormBuilder WithWorkOrderRepository(IRepository<WorkOrder> workOrderRepository)
        {
            _workOrderRepository = workOrderRepository;
            return this;
        }

        public TestWorkOrderMarkoutFormBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

    }

    internal class TestWorkOrderMarkoutForm : WorkOrderMarkoutForm
    {
        public void SetODSMarkouts(IObjectDataSource markouts)
        {
            odsMarkouts = markouts;
        }

        public void SetTXTMarkoutNumber(ITextBox number)
        {
            _txtMarkoutNumber = number;
        }

        public void SetDDLMarkoutType(IDropDownList markoutType)
        {
            _ddlMarkoutType = markoutType;
        }

        public void SetCCDateOfRequest(ITextBox request)
        {
            _ccDateOfRequest = request;
        }

        public void SetUpdatePanel(IUpdatePanel panel)
        {
            upMarkouts = panel;
        }

        public void SetGridView(IGridView gridView)
        {
            gvMarkouts = gridView;
        }

        public void SetViewState(IViewState state)
        {
            _iViewState = state;
        }

        public void SetWorkOrderRepository(IRepository<WorkOrder> workOrderRepository)
        {
            _workOrderRepository = workOrderRepository;
        }

        public void SetSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
        }
    }
}
