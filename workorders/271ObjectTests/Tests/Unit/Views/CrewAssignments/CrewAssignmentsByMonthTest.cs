using System;
using System.Collections.Generic;
using LINQTo271.Views.CrewAssignments;
using MMSINC.Controls.BetterCalendar;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.CrewAssignments
{
    /// <summary>
    /// Summary description for CrewAssignmentsByMonthTest.
    /// </summary>
    [TestClass]
    public class CrewAssignmentsByMonthTest : EventFiringTestClass
    {
        #region Private Members

        private ICalendar _calendar;
        private TestCrewAssignmentsByMonth _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();


            _mocks
                .DynamicMock(out _calendar);

            _target = new TestCrewAssignmentsByMonthBuilder()
                .WithCalendar(_calendar);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
            _target.Dispose();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestCalendarControlIsInjectable()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_calendar, _target.GetPropertyValueByName("Calendar"));
        }

        [TestMethod]
        public void TestCrewIsInjectable()
        {
            _mocks.ReplayAll();

            var crew = new Crew();
            _target = new TestCrewAssignmentsByMonthBuilder()
                .WithCrew(crew);

            Assert.AreSame(crew, _target.Crew);
        }

        [TestMethod]
        public void TestSelectedDateReturnsSelectedDateFromCalendar()
        {
            var now = DateTime.Now;

            using (_mocks.Record())
            {
                SetupResult.For(_calendar.SelectedDate).Return(now);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(now, _target.SelectedDate);
            }
        }

        [TestMethod]
        public void TestCrewAssignmentsIsInjectable()
        {
            _mocks.ReplayAll();

            var assignments = new List<CrewAssignment>();
            _target = new TestCrewAssignmentsByMonthBuilder()
                .WithCrewAssignments(assignments);

            Assert.AreSame(assignments, _target.CrewAssignments);
        }

        [TestMethod]
        public void TestCrewAssignmentsReturnsCrewAssignmentsFromCrewIfNoValueSet()
        {
            _mocks.ReplayAll();

            var crew = new Crew();
            _target = new TestCrewAssignmentsByMonthBuilder()
                .WithCrew(crew);

            Assert.AreSame(crew.CrewAssignments, _target.CrewAssignments);
        }

        [TestMethod]
        public void TestBeginningOfMonthReturnsBeginningOfMonthFromDate()
        {
            _mocks.ReplayAll();

            var date = new DateTime(2006, 6, 6); // \m/
            var beginningOfMonth = new DateTime(2006, 6, 1);
            _target = new TestCrewAssignmentsByMonthBuilder()
                .WithDate(date);

            Assert.AreEqual(beginningOfMonth, _target.BeginningOfMonth);
        }

        [TestMethod]
        public void TestEndOfMonthReturnsEndOfMonthFromDate()
        {
            _mocks.ReplayAll();

            var date = new DateTime(2006, 6, 6); // \m/
            var endOfMonth = new DateTime(2006, 6, 30);
            _target = new TestCrewAssignmentsByMonthBuilder()
                .WithDate(date);

            Assert.AreEqual(endOfMonth, _target.EndOfMonth);
        }

        [TestMethod]
        public void TestAllThoseAnnoyingCalendarItemStylesInOneFellSwoop()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(CrewAssignmentsByMonth.ScheduleColors.ZERO,
                _target.Zero.BackColor);
            Assert.AreEqual(CrewAssignmentsByMonth.ScheduleColors.FIRST_HALF,
                _target.FirstHalf.BackColor);
            Assert.AreEqual(CrewAssignmentsByMonth.ScheduleColors.SECOND_HALF,
                _target.SecondHalf.BackColor);
            Assert.AreEqual(CrewAssignmentsByMonth.ScheduleColors.ONE_HUNDRED,
                _target.OneHundred.BackColor);
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageLoadWiresUpSelectedDatesChangedEventOnCalendar()
        {
            using (_mocks.Record())
            {
                _calendar.SelectedDatesChanged += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Load");
            }
        }

        [TestMethod]
        public void TestCalendarDatesChangedEventFiresSelectedDateChangedEvent()
        {
            var now = DateTime.Now;
            _target = new TestCrewAssignmentsByMonthBuilder()
                .WithCalendar(_calendar)
                .WithSelectedDateChangedHandler((sender, e) => {
                    _called = true;
                    Assert.AreSame(_target, sender);
                    Assert.AreEqual(now, e.Date);
                });

            using (_mocks.Record())
            {
                SetupResult.For(_calendar.SelectedDate).Return(now);
            }
            
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Calendar_SelectedDatesChanged");
                Assert.IsTrue(_called);
            }
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestDataBindSetsCrewAndDateAndSetsStylesForCurrentMonth()
        {
            _mocks.ReplayAll();

            var crew = new Crew();
            var date = DateTime.Now;

            _target.DataBind(crew, date);

            Assert.AreSame(crew, _target.Crew);
            Assert.AreEqual(date, _target.Date);
            Assert.IsTrue(_target.SetStylesForMonthCalled);
        }

        #endregion
    }

    internal class TestCrewAssignmentsByMonthBuilder : TestDataBuilder<TestCrewAssignmentsByMonth>
    {
        #region Private Members

        private ICalendar _calendar;
        private DateTime? _date;
        private IEnumerable<CrewAssignment> _crewAssignments;
        private Crew _crew;
        private EventHandler<DateTimeEventArgs> _selectedDateChanged;

        #endregion

        #region Private Members

        private void Ctrl_OnDispose(TestCrewAssignmentsByMonth ctrl)
        {
            if (_selectedDateChanged != null)
                ctrl.SelectedDateChanged -= _selectedDateChanged;
        }

        #endregion

        #region Exposed Methods

        public override TestCrewAssignmentsByMonth Build()
        {
            var obj = new TestCrewAssignmentsByMonth();
            if (_calendar != null)
                obj.SetCalendar(_calendar);
            if (_date != null)
                obj.SetDate(_date.Value);
            if (_crewAssignments != null)
                obj.SetCrewAssignments(_crewAssignments);
            if (_crew != null)
                obj.SetCrew(_crew);
            if (_selectedDateChanged != null)
                obj.SelectedDateChanged += _selectedDateChanged;
            obj._onDispose += Ctrl_OnDispose;
            return obj;
        }

        public TestCrewAssignmentsByMonthBuilder WithCalendar(ICalendar calendar)
        {
            _calendar = calendar;
            return this;
        }

        public TestCrewAssignmentsByMonthBuilder WithDate(DateTime date)
        {
            _date = date;
            return this;
        }

        public TestCrewAssignmentsByMonthBuilder WithCrewAssignments(IEnumerable<CrewAssignment> assignments)
        {
            _crewAssignments = assignments;
            return this;
        }

        public TestCrewAssignmentsByMonthBuilder WithCrew(Crew crew)
        {
            _crew = crew;
            return this;
        }

        public TestCrewAssignmentsByMonthBuilder WithSelectedDateChangedHandler(EventHandler<DateTimeEventArgs> handler)
        {
            _selectedDateChanged = handler;
            return this;
        }

        #endregion
    }

    internal class TestCrewAssignmentsByMonth : CrewAssignmentsByMonth
    {
        #region Properties

        public bool SetStylesForMonthCalled { get; protected set; }

        #endregion

        #region Delegates

        internal delegate void OnDisposeHandler(TestCrewAssignmentsByMonth ctrl);

        #endregion

        #region Events

        internal OnDisposeHandler _onDispose;

        #endregion

        #region Private Methods

        protected override void SetStylesForMonth()
        {
            SetStylesForMonthCalled = true;
        }

        #endregion

        #region Exposed Methods

        public void SetCalendar(ICalendar calendar)
        {
            _calendar = calendar;
        }

        public void SetDate(DateTime date)
        {
            Date = date;
        }

        public void SetCrewAssignments(IEnumerable<CrewAssignment> assignments)
        {
            _crewAssignments = assignments;
        }

        public void SetCrew(Crew crew)
        {
            Crew = crew;
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_onDispose != null)
                _onDispose(this);
        }

        #endregion
    }
}
