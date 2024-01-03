using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Controls.BetterCalendar;
using WorkOrders.Library.Controls;
using WorkOrders.Model;
using WorkOrders.Views.CrewAssignments;

namespace LINQTo271.Views.CrewAssignments
{
    /// <summary>
    /// Heh, crewass bm.
    /// </summary>
    public partial class CrewAssignmentsByMonth : WorkOrdersMvpUserControl, ICrewAssignmentsByMonth
    {
        #region Constants

        public struct ScheduleColors
        {
            public static readonly Color ZERO = Color.White,
                                         FIRST_HALF = Color.Green,
                                         SECOND_HALF = Color.Yellow,
                                         ONE_HUNDRED = Color.Red;
        }

        #endregion

        #region Control Declarations

        protected ICalendar _calendar;

        #endregion

        #region Private Members

        private DateTime? _beginningOfMonth,
                          _endOfMonth;
        private Style _zero, _firstHalf, _secondHalf, _oneHundred;
        protected Crew _crew;
        protected IEnumerable<CrewAssignment> _crewAssignments;
        private DateTime _date;

        #endregion

        #region Properties

        protected ICalendar Calendar
        {
            get
            {
                if (_calendar == null)
                    _calendar = wcCrewAssignments;
                return _calendar;
            }
        }

        public DateTime SelectedDate
        {
            get { return Calendar.SelectedDate; }
            set { Calendar.SelectedDate = value; }
        }

        public DateTime VisibleDate
        {
            get { return Calendar.VisibleDate; }
            set { Calendar.VisibleDate = value; }
        }

        public Crew Crew
        {
            get { return _crew; }
            protected set { _crew = value; }
        }

        public IEnumerable<CrewAssignment> CrewAssignments
        {
            get
            {
                if (_crewAssignments == null)
                    _crewAssignments = Crew.CrewAssignments;
                return _crewAssignments;
            }
        }

        public DateTime Date
        {
            get { return _date; }
            protected set { _date = value; }
        }

        public DateTime BeginningOfMonth
        {
            get
            {
                if (_beginningOfMonth == null)
                    _beginningOfMonth = Date.GetBeginningOfMonth();
                return _beginningOfMonth.Value;
            }
        }

        public DateTime EndOfMonth
        {
            get
            {
                if (_endOfMonth == null)
                    _endOfMonth = Date.GetEndOfMonth();
                return _endOfMonth.Value;
            }
        }

        public Style Zero
        {
            get
            {
                if (_zero == null)
                    _zero = new Style() {
                        BackColor = ScheduleColors.ZERO
                    };
                return _zero;
            }
        }

        public Style FirstHalf
        {
            get
            {
                if (_firstHalf == null)
                    _firstHalf = new Style() {
                        BackColor =
                            ScheduleColors.FIRST_HALF
                    };
                return _firstHalf;
            }
        }

        public Style SecondHalf
        {
            get
            {
                if (_secondHalf == null)
                    _secondHalf = new Style() {
                        BackColor = ScheduleColors.SECOND_HALF
                    };
                return _secondHalf;
            }
        }

        public Style OneHundred
        {
            get
            {
                if (_oneHundred == null)
                    _oneHundred = new Style() {
                        BackColor = ScheduleColors.ONE_HUNDRED
                    };
                return _oneHundred;
            }
        }

        #endregion

        #region Private Methods

        // TODO: Test these
        // maybe the untestablility means this should be bumped out to another class?

        // virtual for testing.
        protected virtual void SetStylesForMonth()
        {
            var curDate = BeginningOfMonth;
            while (curDate <= EndOfMonth)
            {
                wcCrewAssignments.CustomDates[curDate] = GetStyleForDate(curDate);
                curDate = curDate.GetNextDay();
            }
        }

        protected Style GetStyleForDate(DateTime date)
        {
            var timeToComplete = GetCrewTimeToCompleteAssignmentsByDate(date);

            var percentage =
                GetCrewAvailabilityFromTimeToCompleteAssignments(timeToComplete);

            return GetStyleForPercentage(percentage);
        }

        protected decimal GetCrewTimeToCompleteAssignmentsByDate(DateTime date)
        {
            return CrewAssignments.GetByDate(date).GetTimeToComplete();
        }

        protected decimal GetCrewAvailabilityFromTimeToCompleteAssignments(decimal timeToComplete)
        {
            var percentage = 0m;

            if (timeToComplete != 0 && Crew.Availability != 0)
                percentage = (timeToComplete / Crew.Availability) * 100;
            return percentage;
        }

        protected Style GetStyleForPercentage(decimal percentage)
        {
            if (percentage == 0m)
                return Zero;
            if (percentage < 50m)
                return FirstHalf;
            if (percentage < 100m)
                return SecondHalf;
            return OneHundred;
        }

        #endregion

        #region Events

        public event EventHandler<DateTimeEventArgs> SelectedDateChanged;

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            Calendar.SelectedDatesChanged +=
                Calendar_SelectedDatesChanged;
        }

        protected void Calendar_SelectedDatesChanged(Object sender, EventArgs e)
        {
            if (SelectedDateChanged != null)
                SelectedDateChanged(this,
                    new DateTimeEventArgs(Calendar.SelectedDate));
        }

        #endregion

        #region Exposed Methods

        public void DataBind(Crew crew, DateTime date)
        {
            Crew = crew;
            Date = date;

            SetStylesForMonth();
        }

        #endregion
    }

    public class DateTimeEventArgs : EventArgs
    {
        #region Private Members

        private readonly DateTime _date;

        #endregion

        #region Properties

        public DateTime Date { get { return _date; } }

        #endregion

        #region Constructors

        public DateTimeEventArgs(DateTime date)
        {
            _date = date;
        }

        #endregion
    }
}