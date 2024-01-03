using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Common;

namespace MMSINC.Controls.BetterCalendar
{
    public class BetterCalendar : MvpUserControl, ICalendar, IPostBackEventHandler
    {
        #region Constants

        public struct CssClasses
        {
            public const string CALENDAR = "cal",
                                TODAY_DAY = "today",
                                SELECTED_DAY = "selected",
                                OTHER_DAY = "outOfRange",
                                MAIN_DEFAULT = "betterCalendar";
        }

        public const string SELECTED_DATE_CLICKED_EVENT_NAME = "SelectedDateClicked";

        #endregion

        #region Fields

        private static readonly object SelectedDatesChangedEventKey = new object();

        private string _cssClass = CssClasses.MAIN_DEFAULT;

        // Don't call directly, call the property getter. 
        private Calendar _internalCalendar;
        private BetterCalendarHeader _header;
        private readonly IDictionary<DateTime, Style> _customDates = new Dictionary<DateTime, Style>();

        #endregion

        #region Properties

        public string CssClass
        {
            get
            {
                if (_cssClass == null)
                {
                    return string.Empty;
                }

                return _cssClass;
            }
            set { _cssClass = value; }
        }

        public IDictionary<DateTime, Style> CustomDates
        {
            get { return _customDates; }
        }

        private BetterCalendarHeader Header
        {
            get
            {
                if (_header == null)
                {
                    var h = new BetterCalendarHeader();
                    InitHeader(h);
                    _header = h;
                }

                return _header;
            }
        }

        protected Calendar InternalCalendar
        {
            get
            {
                if (_internalCalendar == null)
                {
                    var c = new Calendar();
                    InitCalendar(c);
                    _internalCalendar = c;
                }

                return _internalCalendar;
            }
        }

        public DateTime SelectedDate
        {
            get { return InternalCalendar.SelectedDate; }
            set { InternalCalendar.SelectedDate = value; }
        }

        public DateTime VisibleDate
        {
            get { return InternalCalendar.VisibleDate; }
            set { InternalCalendar.VisibleDate = value; }
        }

        #endregion

        #region Private Methods

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitChildControls();
        }

        /// <summary>
        /// Sets the internal calendar to the necessary default values.
        /// </summary>
        /// <param name="cal"></param>
        protected void InitCalendar(Calendar cal)
        {
            if (cal == null)
            {
                throw new ArgumentNullException("cal");
            }

            // These need to be set explicitly because the properties
            // will default to DateTime.MinValue, though internally
            // they get set to DateTime.Today whenever they're DateTime.MinValue.
            // I really don't know what MS was thinking there. 
            cal.SelectedDate = DateTime.Today;
            cal.VisibleDate = DateTime.Today;
            cal.ShowTitle = false;
            cal.CssClass = CssClasses.CALENDAR;
            cal.SelectionChanged += InternalCalendarSelectionChanged;
            cal.DayRender += InternalCalendarDayRender;
            cal.TodayDayStyle.CssClass = CssClasses.TODAY_DAY;
            cal.SelectedDayStyle.CssClass = CssClasses.SELECTED_DAY;
            cal.OtherMonthDayStyle.CssClass = CssClasses.OTHER_DAY;
            Controls.Add(cal);
        }

        protected void InitHeader(BetterCalendarHeader header)
        {
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            header.DateChangeRequest += HeaderDateChangeRequest;
        }

        protected virtual void InitChildControls()
        {
            Controls.Add(Header);
            Controls.Add(InternalCalendar);
        }

        #endregion

        #region Rendering

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Header.VisibleDate = VisibleDate;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            var css = CssClass;
            if (!string.IsNullOrWhiteSpace(css))
            {
                writer.AddAttribute("class", css);
            }

            writer.RenderBeginTag("div");
            base.Render(writer);
            writer.RenderEndTag();
        }

        #endregion

        #endregion

        #region Events

        // Doing this to match how ASP control events are done by the core ASP library. 
        public event EventHandler SelectedDatesChanged
        {
            add { Events.AddHandler(SelectedDatesChangedEventKey, value); }
            remove { Events.RemoveHandler(SelectedDatesChangedEventKey, value); }
        }

        #endregion

        #region Event Handlers

        private void HeaderDateChangeRequest(object sender, BetterCalendarHeaderDateChangedArgs e)
        {
            switch (e.ChangeType)
            {
                case BetterCalendarHeaderDateChangeType.Month:
                    VisibleDate = VisibleDate.AddMonths(e.Difference);
                    break;
                case BetterCalendarHeaderDateChangeType.Year:
                    VisibleDate = VisibleDate.AddYears(e.Difference);
                    break;
            }
        }

        // Wrapping this for our own event so we send the proper sender object. 
        private void InternalCalendarSelectionChanged(object sender, EventArgs e)
        {
            var handler = (EventHandler)Events[SelectedDatesChangedEventKey];
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void InternalCalendarDayRender(object sender, DayRenderEventArgs e)
        {
            Style s;
            if (CustomDates.TryGetValue(e.Day.Date, out s))
            {
                e.Cell.ApplyStyle(s);
            }

            if (e.Day.Date == SelectedDate)
            {
                WireSelectedDayClickEvent(e.Cell);
            }
        }

        private void WireSelectedDayClickEvent(TableCell cell)
        {
            LiteralControl lit = null;
            LinkButton lb = null;
            foreach (var control in cell.Controls)
            {
                lit = control as LiteralControl;
                if (lit == null)
                {
                    continue;
                }

                lb = new LinkButton {
                    Text = lit.Text,
                    ToolTip = SelectedDate.ToString("MMMM dd"),
                    OnClientClick =
                        ClientScriptManager.GetPostBackEventReference(this,
                            SELECTED_DATE_CLICKED_EVENT_NAME)
                };
                lb.Click += InternalCalendarSelectionChanged;
            }

            if (lit == null)
            {
                return;
            }

            var index = cell.Controls.IndexOf(lit);
            cell.Controls.Remove(lit);
            cell.Controls.AddAt(index, lb);
        }

        #endregion

        #region Exposed Methods

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == SELECTED_DATE_CLICKED_EVENT_NAME)
            {
                InternalCalendarSelectionChanged(this, EventArgs.Empty);
            }
        }

        #endregion
    }

    public interface ICalendar
    {
        #region Properties

        DateTime SelectedDate { get; set; }
        DateTime VisibleDate { get; set; }

        #endregion

        #region Events

        event EventHandler SelectedDatesChanged;

        #endregion
    }
}
