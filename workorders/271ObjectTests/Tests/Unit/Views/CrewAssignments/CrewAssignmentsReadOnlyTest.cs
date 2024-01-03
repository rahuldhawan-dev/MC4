using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using _271ObjectTests.Tests.Unit.Model;
using _271ObjectTests.Tests.Unit.Views.WorkOrders.Scheduling;
using LINQTo271.Views.CrewAssignments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Controls;
using MMSINC.DesignPatterns.Mvp.Interface;
using MMSINC.Testing;
using MMSINC.Testing.TestExtensions;
using Rhino.Mocks;
using WorkOrders;
using WorkOrders.Model;
using WorkOrders.Presenters.CrewAssignments;
using WorkOrders.Views.CrewAssignments;

namespace _271ObjectTests.Tests.Unit.Views.CrewAssignments
{
    /// <summary>
    /// Summary description for CrewAssignmentsReadOnlyTest.
    /// </summary>
    [TestClass]
    public class CrewAssignmentsReadOnlyTest : EventFiringTestClass
    {
        #region Private Members

        private IResponse _iResponse;
        private IListControl _listControl;
        private ICrewAssignmentsByMonth _cabmCrewAssignments;
        private ICrewAssignmentsReadOnlyPresenter _presenter;
        private IDropDownList _ddlCrew;
        private ITextBox _ccDate;
        private CrewAssignmentsReadOnly _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _ddlCrew)
                .DynamicMock(out _ccDate)
                .DynamicMock(out _presenter)
                .DynamicMock(out _cabmCrewAssignments)
                .DynamicMock(out _listControl)
                .DynamicMock(out _iResponse);

            _target = new TestCrewAssignmentsReadOnlyBuilder()
                .WithDDLCrew(_ddlCrew)
                .WithCCDate(_ccDate)
                .WithPresenter(_presenter)
                .WithCalendar(_cabmCrewAssignments)
                .WithListControl(_listControl)
                .WithIResponse(_iResponse);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestCrewIDPropertyReturnsSelectedValueOfCrewDropDown()
        {
            var expected = 138;

            using (_mocks.Record())
            {
                SetupResult.For(_ddlCrew.GetSelectedValue()).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.CrewID);
            }
        }

        [TestMethod]
        public void TestDatePropertyReturnsDateValueFromTextBoxIfSet()
        {
            var expected = DateTime.Today.AddDays(-1);

            using (_mocks.Record())
            {
                SetupResult.For(_ccDate.TryGetDateTimeValue()).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.Date);
            }
        }

        [TestMethod]
        public void TestDatePropertyReturnsCurrentDateIfTextBoxValueNotSet()
        {
            var expected = DateTime.Today;

            using (_mocks.Record())
            {
                SetupResult.For(_ccDate.TryGetDateTimeValue()).Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.Date);
            }
        }

        [TestMethod]
        public void TestSettingDatePropertySetsFormattedStringValueOfTextBox()
        {
            var expected =
                DateTime.Today.ToString(CrewAssignmentsReadOnly.DATE_FORMAT);

            using (_mocks.Record())
            {
                _ccDate.Text = expected;
            }

            using (_mocks.Playback())
            {
                _target.Date = DateTime.Today;
            }
        }

        [TestMethod]
        public void TestPresenterPropertyGetsPresenterInstanceFromIocContainer()
        {
            _target = new TestCrewAssignmentsReadOnlyBuilder();
            WorkOrdersContainer.Instance.AddImplementorMethodFor
                <ICrewAssignmentsReadOnlyPresenter>(() => {
                    _called = true;
                    return _presenter;
                });

            Assert.AreSame(_presenter, _target.Presenter);
            Assert.IsTrue(_called);

            WorkOrdersContainer.Instance.RemoveImplementorMethodFor
                <ICrewAssignmentsReadOnlyPresenter>();

            _mocks.ReplayAll();
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestDisplayStartDateRendersDateInStartDateLabelInRow()
        {
            _mocks.ReplayAll();

            var row = new MockControlDictionary();
            var lbStart = new LinkButton {
                ID = "lbStart",
                Visible = true
            };
            var lblStartDate = new Label {
                ID = "lblStartDate",
                Visible = false
            };
            row.AddControl(lbStart);
            row.AddControl(lblStartDate);
            var date = DateTime.Today;
            var dateStr = date.ToString();

            _target.InvokeInstanceMethod("DisplayStartDate", new object[] {
                row, date
            });

            Assert.IsFalse(lbStart.Visible);
            Assert.IsTrue(lblStartDate.Visible);
            Assert.AreEqual(dateStr, lblStartDate.Text);
        }

        [TestMethod]
        public void TestDisplayEndDateRendersDateInEndDateLabelInRow()
        {
            _mocks.ReplayAll();

            var row = new MockControlDictionary();
            var lbEnd = new LinkButton {
                ID = "lbEnd",
                Visible = true
            };
            var lblEndDate = new Label {
                ID = "lblEndDate",
                Visible = false
            };
            row.AddControl(lbEnd);
            row.AddControl(lblEndDate);
            var date = DateTime.Today;
            var dateStr = date.ToString();

            _target.InvokeInstanceMethod("DisplayEndDate", new object[] {
                row, date
            });

            Assert.IsFalse(lbEnd.Visible);
            Assert.IsTrue(lblEndDate.Visible);
            Assert.AreEqual(dateStr, lblEndDate.Text);
        }

        [TestMethod]
        public void TestDisplayNotApplicableHidesEndDateLabelAndShowsNotApplicableLabelInRow()
        {
            _mocks.ReplayAll();

            var row = new MockControlDictionary();
            var lbEnd = new LinkButton {
                ID = "lbEnd",
                Visible = true
            };
            var lblNotApplicable = new Label {
                ID = "lblNotApplicable",
                Visible = false
            };
            row.AddControl(lbEnd);
            row.AddControl(lblNotApplicable);

            _target.InvokeInstanceMethod("DisplayNotApplicable", new object[] {
                row
            });

            Assert.IsFalse(lbEnd.Visible);
            Assert.IsTrue(lblNotApplicable.Visible);
        }

        [TestMethod]
        public void TestDataBindCrewFiresCalendarDataBind()
        {
            var crew = new Crew();
            var date = DateTime.Now;

            using (_mocks.Record())
            {
                SetupResult.For(_ccDate.TryGetDateTimeValue()).Return(date);
                _cabmCrewAssignments.DataBind(crew, date);
            }

            using (_mocks.Playback())
            {
                _target.DataBindCrew(crew);
            }
        }

        [TestMethod]
        public void TestDataBindFiresListControlDataBind()
        {
            using (_mocks.Record())
            {
                _listControl.DataBind();
            }

            using (_mocks.Playback())
            {
                _target.DataBind();
            }
        }

        [TestMethod]
        public void TestRedirectCallsRedirectOnResponse()
        {
            var expected = "Some URL";

            using (_mocks.Record())
            {
                _iResponse.Redirect(expected);
            }

            using (_mocks.Playback())
            {
                _target.Redirect(expected);
            }
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageLoadSetsTextBoxDateValueIfNoValueSet()
        {
            var expected =
                DateTime.Today.ToString(CrewAssignmentsReadOnly.DATE_FORMAT);

            using (_mocks.Record())
            {
                _ccDate.Text = expected;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadDoesNotSetTextBoxDateValueIfAlreadySet()
        {
            var date =
                DateTime.Today.AddDays(-1).ToString(
                    CrewAssignmentsReadOnly.DATE_FORMAT);

            using (_mocks.Record())
            {
                SetupResult.For(_ccDate.Text).Return(date);

                DoNotExpect.Call(() => _ccDate.Text = null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestPageLoadFiresPresenterOnViewLoaded()
        {
            using (_mocks.Record())
            {
                _presenter.OnViewLoaded();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestCalendarSelectedDateChangedSetsValueOfDateTextBox()
        {
            var date = DateTime.Today;
            var dateStr = date.ToString(CrewAssignmentsReadOnly.DATE_FORMAT);
            var args = new DateTimeEventArgs(date);

            using (_mocks.Record())
            {
                _ccDate.Text = dateStr;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target,
                    "cabmCrewAssignments_SelectedDateChanged", new object[] {
                        null, args
                    });
            }
        }

        [TestMethod]
        public void TestGridViewRowDataBoundDoesNotThrowException()
        {
            _mocks.ReplayAll();

            var assignment = new CrewAssignment();
            var row = new TestGridViewRow(assignment);
            var args = new GridViewRowEventArgs(row);
            Action invokeDataBound = () => InvokeEventByName(_target,
                                               "gvCrewAssignments_RowDataBound",
                                               new object[] {
                                                   null, args
                                               });

            foreach (DataControlRowType type in Enum.GetValues(typeof(DataControlRowType)))
            {
                if (type == DataControlRowType.DataRow) continue;

                row.DataItem = assignment;
                row.RowType = type;

                MyAssert.DoesNotThrow(invokeDataBound);

                row.DataItem = null;

                MyAssert.DoesNotThrow(invokeDataBound);
            }
        }

        [TestMethod]
        public void TestGridViewRowDataBoundDisplaysStartDateIfSet()
        {
            _mocks.ReplayAll();

            var dateStarted = DateTime.Today;
            CrewAssignment assignment = new TestCrewAssignmentBuilder()
                .WithDateStarted(dateStarted);
            var row = new TestGridViewRow(assignment, DataControlRowType.DataRow);
            var args = new GridViewRowEventArgs(row);
            bool displayStartDateCalled = false,
                 displayEndDateCalled = false,
                 displayNotApplicableCalled = false;


            _target = new TestCrewAssignmentsReadOnlyBuilder()
                .WithDisplayStartDateFn((ctrl, date) => {
                    displayStartDateCalled = true;
                    Assert.AreSame(row, ctrl);
                    Assert.AreEqual(dateStarted, date);
                })
                .WithDisplayEndDateFn(
                (ctrl, date) => displayEndDateCalled = true)
                .WithDisplayNotApplicableFn(
                ctrl => displayNotApplicableCalled = true);

            InvokeEventByName(_target,
                "gvCrewAssignments_RowDataBound",
                new object[] {
                    null, args
                });

            Assert.IsTrue(displayStartDateCalled);
            Assert.IsFalse(displayEndDateCalled);
            Assert.IsFalse(displayNotApplicableCalled);
        }

        [TestMethod]
        public void TestGridViewRowDataBoundDisplaysStartAndEndDateIfBothSet()
        {
            _mocks.ReplayAll();

            var dateEnded = DateTime.Today;
            var dateStarted = dateEnded.AddDays(-1);
            CrewAssignment assignment = new TestCrewAssignmentBuilder()
                .WithDateStarted(dateStarted)
                .WithDateEnded(dateEnded);
            var row = new TestGridViewRow(assignment, DataControlRowType.DataRow);
            var args = new GridViewRowEventArgs(row);
            bool displayStartDateCalled = false,
                 displayEndDateCalled = false,
                 displayNotApplicableCalled = false;


            _target = new TestCrewAssignmentsReadOnlyBuilder()
                .WithDisplayStartDateFn((ctrl, date) => {
                    displayStartDateCalled = true;
                    Assert.AreSame(row, ctrl);
                    Assert.AreEqual(dateStarted, date);
                })
                .WithDisplayEndDateFn(
                (ctrl, date) => {
                    displayEndDateCalled = true;
                    Assert.AreSame(row, ctrl);
                    Assert.AreEqual(dateEnded, date);
                })
                .WithDisplayNotApplicableFn(
                ctrl => displayNotApplicableCalled = true);

            InvokeEventByName(_target,
                "gvCrewAssignments_RowDataBound",
                new object[] {
                    null, args
                });

            Assert.IsTrue(displayStartDateCalled);
            Assert.IsTrue(displayEndDateCalled);
            Assert.IsFalse(displayNotApplicableCalled);
        }

        [TestMethod]
        public void TestGridViewRowDataBoundDisplaysNotApplicableIfStartAndEndDatesNotSet()
        {
            _mocks.ReplayAll();

            CrewAssignment assignment = new TestCrewAssignmentBuilder();
            var row = new TestGridViewRow(assignment, DataControlRowType.DataRow);
            var args = new GridViewRowEventArgs(row);
            bool displayStartDateCalled = false,
                 displayEndDateCalled = false,
                 displayNotApplicableCalled = false;


            _target = new TestCrewAssignmentsReadOnlyBuilder()
                .WithDisplayStartDateFn(
                (ctrl, date) => displayStartDateCalled = true)
                .WithDisplayEndDateFn(
                (ctrl, date) => displayEndDateCalled = true)
                .WithDisplayNotApplicableFn(
                ctrl => {
                    displayNotApplicableCalled = true;
                    Assert.AreSame(ctrl, row);
                });

            InvokeEventByName(_target,
                "gvCrewAssignments_RowDataBound",
                new object[] {
                    null, args
                });

            Assert.IsFalse(displayStartDateCalled);
            Assert.IsFalse(displayEndDateCalled);
            Assert.IsTrue(displayNotApplicableCalled);
        }

        [TestMethod]
        public void TestGVCrewAssignmentsRowCommandFiresAssignmentCommandEvent()
        {
            var crewAssignmentID = 12;
            var commandName = "Start";
            var command = CrewAssignmentCommandEventArgs.Commands.Start;
            var args = new MockGridViewCommandEventArgs(commandName,
                crewAssignmentID.ToString());
            CrewAssignmentCommandEventHandler handler = (sender, e) => {
                _called = true;
                
                Assert.AreEqual(crewAssignmentID, e.CrewAssignmentID);
                Assert.AreEqual(command, e.Command);
                MyAssert.AreClose(DateTime.Now, e.Date);
            };

            using (_target = new TestCrewAssignmentsReadOnlyBuilder()
                                 .WithAssignmentCommandEventHandler(handler))
            {
                InvokeEventByName(_target, "gvCrewAssignments_RowCommand",
                    new object[] {
                        null, args
                    });

                Assert.IsTrue(_called);
            }

            _mocks.ReplayAll();
        }

        #endregion
    }

    internal class TestCrewAssignmentsReadOnlyBuilder : TestDataBuilder<TestCrewAssignmentsReadOnly>
    {
        #region Private Members

        private Action<Control, DateTime> _displayStartDateFn, _displayEndDateFn;
        private Action<Control> _displayNotApplicableFn;
        private IDropDownList _ddlCrew;
        private ITextBox _ccDate;
        private ICrewAssignmentsReadOnlyPresenter _presenter;
        private ICrewAssignmentsByMonth _cabmCrewAssignments;
        private CrewAssignmentCommandEventHandler _onAssignmentCommand;
        private IListControl _listControl;
        private IResponse _iResponse;

        #endregion

        #region Private Methods

        private void View_OnDispose(TestCrewAssignmentsReadOnly view)
        {
            if (_onAssignmentCommand != null)
                view.AssignmentCommand -= _onAssignmentCommand;
        }

        #endregion

        #region Exposed Methods

        public override TestCrewAssignmentsReadOnly Build()
        {
            var obj = new TestCrewAssignmentsReadOnly();
            if (_ddlCrew != null)
                obj.SetDDLCrew(_ddlCrew);
            if (_ccDate != null)
                obj.SetCCDate(_ccDate);
            if (_presenter != null)
                obj.SetPresenter(_presenter);
            if (_cabmCrewAssignments != null)
                obj.SetCalendar(_cabmCrewAssignments);
            if (_displayStartDateFn != null)
                obj.SetDisplayStartDateFn(_displayStartDateFn);
            if (_displayEndDateFn != null)
                obj.SetDisplayEndDateFn(_displayEndDateFn);
            if (_displayNotApplicableFn != null)
                obj.SetDisplayNotApplicableFn(_displayNotApplicableFn);
            if (_listControl != null)
                obj.SetListControl(_listControl);
            if (_iResponse != null)
                obj.SetIResponse(_iResponse);
            if (_onAssignmentCommand != null)
                obj.AssignmentCommand += _onAssignmentCommand;
            obj._onDispose += View_OnDispose;
            return obj;
        }

        public TestCrewAssignmentsReadOnlyBuilder WithDDLCrew(IDropDownList ddlCrew)
        {
            _ddlCrew = ddlCrew;
            return this;
        }

        public TestCrewAssignmentsReadOnlyBuilder WithCCDate(ITextBox ccDate)
        {
            _ccDate = ccDate;
            return this;
        }

        public TestCrewAssignmentsReadOnlyBuilder WithDisplayStartDateFn(Action<Control, DateTime> fn)
        {
            _displayStartDateFn = fn;
            return this;
        }

        public TestCrewAssignmentsReadOnlyBuilder WithDisplayEndDateFn(Action<Control, DateTime> fn)
        {
            _displayEndDateFn = fn;
            return this;
        }

        public TestCrewAssignmentsReadOnlyBuilder WithDisplayNotApplicableFn(Action<Control> fn)
        {
            _displayNotApplicableFn = fn;
            return this;
        }

        public TestCrewAssignmentsReadOnlyBuilder WithPresenter(ICrewAssignmentsReadOnlyPresenter presenter)
        {
            _presenter = presenter;
            return this;
        }

        public TestCrewAssignmentsReadOnlyBuilder WithCalendar(ICrewAssignmentsByMonth calendar)
        {
            _cabmCrewAssignments = calendar;
            return this;
        }

        public TestCrewAssignmentsReadOnlyBuilder WithAssignmentCommandEventHandler(CrewAssignmentCommandEventHandler onAssignmentCommand)
        {
            _onAssignmentCommand = onAssignmentCommand;
            return this;
        }

        public TestCrewAssignmentsReadOnlyBuilder WithListControl(IListControl listControl)
        {
            _listControl = listControl;
            return this;
        }

        public TestCrewAssignmentsReadOnlyBuilder WithIResponse(IResponse page)
        {
            _iResponse = page;
            return this;
        }

        #endregion
    }

    internal class TestCrewAssignmentsReadOnly : CrewAssignmentsReadOnly
    {
        #region Private Members

        private Action<Control, DateTime> _displayStartDateFn, _displayEndDateFn;
        private Action<Control> _displayNotApplicableFn;

        #endregion

        #region Overridden Methods

        protected override void DisplayStartDate(Control row, DateTime date)
        {
            if (_displayStartDateFn != null)
                _displayStartDateFn(row, date);
            else
                base.DisplayStartDate(row, date);
        }

        protected override void DisplayEndDate(Control row, DateTime date)
        {
            if (_displayEndDateFn != null)
                _displayEndDateFn(row, date);
            else
                base.DisplayEndDate(row, date);
        }

        protected override void DisplayNotApplicable(Control row)
        {
            if (_displayNotApplicableFn != null)
                _displayNotApplicableFn(row);
            else
                base.DisplayNotApplicable(row);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_onDispose != null)
                _onDispose(this);
        }

        #endregion

        #region Delegates

        internal delegate void OnDisposeHandler(TestCrewAssignmentsReadOnly view);

        #endregion

        #region Events

        internal OnDisposeHandler _onDispose;

        #endregion

        #region Exposed Methods

        public void SetDDLCrew(IDropDownList ddl)
        {
            ddlCrew = ddl;
        }

        public void SetCCDate(ITextBox txt)
        {
            ccDate = txt;
        }

        public void SetPresenter(ICrewAssignmentsReadOnlyPresenter presenter)
        {
            _presenter = presenter;
        }

        public void SetDisplayStartDateFn(Action<Control, DateTime> fn)
        {
            _displayStartDateFn = fn;
        }

        public void SetDisplayEndDateFn(Action<Control, DateTime> fn)
        {
            _displayEndDateFn = fn;
        }

        public void SetDisplayNotApplicableFn(Action<Control> fn)
        {
            _displayNotApplicableFn = fn;
        }

        public void SetCalendar(ICrewAssignmentsByMonth assignments)
        {
            cabmCrewAssignments = assignments;
        }

        public void SetListControl(IListControl listControl)
        {
            gvCrewAssignments = listControl;
        }

        public void SetIResponse(IResponse response)
        {
            _iResponse = response;
        }

        #endregion
    }

    internal class MockControlDictionary : Control
    {
        #region Private Methods

        private readonly Hashtable _controls;

        #endregion

        #region Constructors

        internal MockControlDictionary()
        {
            _controls = new Hashtable();
        }

        #endregion

        #region Exposed Methods

        public void AddControl(Control ctrl)
        {
            AddControl(ctrl.ID, ctrl);
        }

        public void AddControl(string id, Control ctrl)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("Cannot add a control with no ID.");
            if (ctrl == null)
                throw new ArgumentNullException("Cannot add a null control to the collection.");

            _controls.Add(id, ctrl);
        }

        public void RemoveControl(string id)
        {
            _controls.Remove(id);
        }

        public override Control FindControl(string id)
        {
            var ret = _controls[id] as Control;
            return ret ?? base.FindControl(id);
        }

        #endregion
    }

    internal class MockGridViewCommandEventArgs : GridViewCommandEventArgs
    {
        #region Constructors

        public MockGridViewCommandEventArgs(string commandName, object argument)
            : base(null, new CommandEventArgs(commandName, argument))
        {
        }

        #endregion
    }
}
