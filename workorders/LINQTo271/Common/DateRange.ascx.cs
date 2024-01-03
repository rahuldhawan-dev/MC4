using System;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Controls;
using WorkOrders.Library.Controls;

namespace LINQTo271.Common
{
    public partial class DateRange : WorkOrdersMvpUserControl, IDateRange
    {
        #region Constants

        public struct Operators
        {
            #pragma warning disable 169
            public const string EQUALS = "=",
                                GREATER_THAN_OR_EQUAL_TO = ">=",
                                GREATER_THAN = ">",
                                LESS_THAN_OR_EQUAL_TO = "<=",
                                LESS_THAN = "<",
                                BETWEEN = "BETWEEN";
            #pragma warning restore 169
        }

        private const string DDLSEARCHOP_ONCHANGE =
            "ddlSearchOp_Change(this, $('#{0}'))";

        #endregion

        #region Private Members

        protected DateRangeDefault _default = DateRangeDefault.None;

        #endregion

        #region Properties

        public string SelectedOperator
        {
            get { return ddlSearchOp.SelectedValue ?? string.Empty; }
        }

        protected bool ProvideRange
        {
            get { return ddlSearchOp.SelectedValue == Operators.BETWEEN; }
        }

        public bool HasStartDate
        {
            get
            {
                DateTime date;
                return DateTime.TryParse(ccStartDate.Text, out date);
            }
        }

        public bool HasEndDate
        {
            get
            {
                DateTime date;
                return DateTime.TryParse(ccEndDate.Text, out date);
            }
        }


        /// <summary>
        /// The start date, if the operator selection is a binary range, else
        /// null.
        /// </summary>
        public DateTime? StartDate
        {
            get { return ProvideRange && HasStartDate ? (DateTime?)DateTime.Parse(ccStartDate.Text) : null; }
        }

        /// <summary>
        /// The end date, if the operator selection is a binary range, else
        /// null.
        /// </summary>
        public DateTime? EndDate
        {
            get { return ProvideRange && HasEndDate ? (DateTime?)DateTime.Parse(ccEndDate.Text) : null; }
        }

        /// <summary>
        /// The date entered, if the operator selection is not a binary range,
        /// else null.
        /// </summary>
        public DateTime? Date
        {
            get
            {
                return ProvideRange || !HasEndDate
                           ? null : (DateTime?)DateTime.Parse(ccEndDate.Text);
            }
        }

        public DateRangeDefault Default
        {
            get { return _default; }
            set { _default = value; }
        }

        #endregion

        #region Private Methods

        private void WireClientSideEvents()
        {
            ddlSearchOp.Attributes.Add("onclick",
                String.Format(DDLSEARCHOP_ONCHANGE, tdStartDate.ClientID));
        }

        private void SetDefault()
        {
            switch (Default)
            {
                case DateRangeDefault.None:
                    return;
                case DateRangeDefault.LastBusinessWeek:
                    var lastWeek = DateTime.Now.AddWeeks(-1);
                    ccStartDate.Text =
                        lastWeek.GetDayFromWeek(DayOfWeek.Monday).ToString("d");
                    tdStartDate.Style.Clear();
                    ccEndDate.Text =
                        lastWeek.GetDayFromWeek(DayOfWeek.Saturday).ToString("d");
                    ddlSearchOp.SelectedValue = "BETWEEN";
                    break;
            }
        }

        #endregion

        #region Event Handlers

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetDefault();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WireClientSideEvents();
        }

        #endregion
    }

    public interface IDateRange : IControl
    {
        #region Properties

        DateTime? Date { get; }
        DateTime? StartDate { get; }
        DateTime? EndDate { get; }
        string SelectedOperator { get; }
        bool HasStartDate { get; }
        bool HasEndDate { get; }

        #endregion
    }

    public enum DateRangeDefault
    {
        None, LastBusinessWeek
    }
}
