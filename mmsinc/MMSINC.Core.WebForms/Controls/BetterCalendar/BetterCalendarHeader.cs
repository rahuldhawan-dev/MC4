using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls.BetterCalendar
{
    public class BetterCalendarHeader : Table
    {
        #region Fields

        private static readonly object DateChangeRequestEventKey = new object();
        private TableCell _dateLabelCell;

        #endregion

        #region Properties

        public DateTime VisibleDate { get; set; }

        #endregion

        #region Constructors

        public BetterCalendarHeader()
        {
            CssClass = "header";
        }

        #endregion

        #region Private Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitRow();
        }

        private void InitRow()
        {
            var row = new TableRow();
            var rowCells = row.Cells;
            rowCells.Add(CreateCell(CreateLastYearButton()));
            rowCells.Add(CreateCell(CreateLastMonthButton()));
            _dateLabelCell = new TableCell();
            _dateLabelCell.CssClass = "dateLabel";
            rowCells.Add(_dateLabelCell);
            rowCells.Add(CreateCell(CreateNextMonthButton()));
            rowCells.Add(CreateCell(CreateNextYearButton()));
            Rows.Add(row);
        }

        protected static LinkButton CreateButton(string text, string toolTip, EventHandler handler)
        {
            var b = new LinkButton {
                Text = text,
                ToolTip = toolTip
            };
            b.Click += handler;
            return b;
        }

        protected LinkButton CreateLastYearButton()
        {
            return CreateButton("<<", "Go to the previous year", LastYearButtonClicked);
        }

        protected LinkButton CreateLastMonthButton()
        {
            return CreateButton("<", "Go to the previous month", LastMonthButtonClicked);
        }

        protected LinkButton CreateNextMonthButton()
        {
            return CreateButton(">", "Go to the next month", NextMonthButtonClicked);
        }

        protected LinkButton CreateNextYearButton()
        {
            return CreateButton(">>", "Go to the next year", NextYearButtonClicked);
        }

        protected static TableCell CreateCell(Control c)
        {
            var cell = new TableCell();
            cell.Controls.Add(c);
            return cell;
        }

        protected void OnDateChangeRequest(BetterCalendarHeaderDateChangeType changeType, int diff)
        {
            var handler = (EventHandler<BetterCalendarHeaderDateChangedArgs>)Events[DateChangeRequestEventKey];
            if (handler != null)
            {
                handler(this, new BetterCalendarHeaderDateChangedArgs(changeType, diff));
            }
        }

        private static string GetMonthName(DateTime d)
        {
            return DateTimeFormatInfo.CurrentInfo.GetMonthName(d.Month);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            var month = GetMonthName(VisibleDate);
            var year = VisibleDate.Year;

            var label = string.Format("{0} {1}", month, year);
            _dateLabelCell.Text = label;
        }

        #endregion

        #region Events

        public event EventHandler<BetterCalendarHeaderDateChangedArgs> DateChangeRequest
        {
            add { Events.AddHandler(DateChangeRequestEventKey, value); }
            remove { Events.RemoveHandler(DateChangeRequestEventKey, value); }
        }

        #endregion

        #region Event Handlers

        private void LastYearButtonClicked(object sender, EventArgs e)
        {
            OnDateChangeRequest(BetterCalendarHeaderDateChangeType.Year, -1);
        }

        private void LastMonthButtonClicked(object sender, EventArgs e)
        {
            OnDateChangeRequest(BetterCalendarHeaderDateChangeType.Month, -1);
        }

        private void NextYearButtonClicked(object sender, EventArgs e)
        {
            OnDateChangeRequest(BetterCalendarHeaderDateChangeType.Year, 1);
        }

        private void NextMonthButtonClicked(object sender, EventArgs e)
        {
            OnDateChangeRequest(BetterCalendarHeaderDateChangeType.Month, 1);
        }

        #endregion
    }
}
