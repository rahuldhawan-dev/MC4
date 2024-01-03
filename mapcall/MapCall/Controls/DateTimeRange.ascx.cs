using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MapCall.Common.Controls;

namespace MapCall.Controls
{
    public enum OperatorTypes
    {
        Equals = 0,
        GreaterThanOrEqual = 1,
        GreaterThan = 2,
        LessThan = 3,
        LessThanOrEqualTo = 4,
        Between = 5
    }

    public partial class DateTimeRange : UserControl
    {
        #region Structs

        public struct PARAMETER_SUFFIXES
        {
            public const string START_DATE = "StartDate";
            public const string END_DATE = "EndDate";
        }

        #endregion

        #region Fields

        private static readonly Dictionary<OperatorTypes, string> _operatorLookup;

        #endregion

        #region Properties

        /// <summary>
        /// Set this to true if the control IDs should be set to a more guaranteed unique name than
        /// just txtStart/txtEnd. This is false by default for backwards compatability.
        /// </summary>
        public bool UseBetterControlNamingScheme { get; set; }

        public bool ShowTimePicker { get; set; }
        public DateTime? StartDate
        {
            get
            {
                return dtpStart.SelectedDate;
            }
            set
            {
                dtpStart.SelectedDate = value;
            }
        }
        public DateTime? EndDate
        {
            get
            {
                return dtpEnd.SelectedDate;
            }
            set
            {
                dtpEnd.SelectedDate = value;
            }
        }

        public string Operator
        {
            get { return ddlDateInstalledParam.SelectedValue; }
        }

        public OperatorTypes SelectedOperatorType
        {
            get
            {
                var selected = this.ddlDateInstalledParam.SelectedItem.Text;
                return (from what in _operatorLookup
                        where what.Value == selected
                        select what.Key).Single();
            }
            set
            {
                GetListItemByOperator(value).Selected = true;
            }
        }


        public string SelectedIndex
        {
            set
            {
                ddlDateInstalledParam.SelectedIndex = Int32.Parse(value);
            }
        }

        #endregion

        #region Constructors

        static DateTimeRange()
        {
            var lookup = new Dictionary<OperatorTypes, string>();
            lookup[OperatorTypes.Equals] = "=";
            lookup[OperatorTypes.GreaterThanOrEqual] = ">=";
            lookup[OperatorTypes.GreaterThan] = ">";
            lookup[OperatorTypes.LessThan] = "<";
            lookup[OperatorTypes.LessThanOrEqualTo] = "<=";
            lookup[OperatorTypes.Between] = "BETWEEN";
            _operatorLookup = lookup;
        }

        #endregion


        #region Private Methods

        private ListItem GetListItemByOperator(OperatorTypes opType)
        {
            var opStr = _operatorLookup[opType];
            return (from ListItem li in ddlDateInstalledParam.Items
                    where li.Text == opStr
                    select li).Single();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);


            // The control ids need to be set at this point, any later
            // and the changing of the ID will mess up how the controls
            // get their postback data.
            EnsureControlIds();
        }

        private void InitializeDateTimePicker(DateTimePicker picker)
        {
            picker.ShowTimePicker = ShowTimePicker;
            picker.ShowCalendarButton = true;
            picker.ShowMonthChangeDropDown = true;
            picker.ShowYearChangeDropDown = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitializeDateTimePicker(dtpStart);
            InitializeDateTimePicker(dtpEnd);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            const string dateControlChange =
                "function dateControlChange(start, op) {s = document.getElementById(start);o = document.getElementById(op);s.style.display = (o.selectedIndex == 5 ? \"\" : \"none\");}";
            Page.ClientScript.RegisterStartupScript(this.GetType(),"dateControlChange", dateControlChange, true);

            ddlDateInstalledParam.Attributes.Add("onChange", string.Format("dateControlChange('{0}', '{1}');", tdDateInstalledStart.ClientID, ddlDateInstalledParam.ClientID));
            ScriptManager.RegisterStartupScript(this, typeof(string), ClientID + "Script", string.Format("if (document.getElementById('{0}')!=null)\n\tdateControlChange('{0}', '{1}');\n", tdDateInstalledStart.ClientID, ddlDateInstalledParam.ClientID), true);
        }

        private void EnsureControlIds()
        {
            if (!UseBetterControlNamingScheme) { return; }

            var id = ID;
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NullReferenceException(
                    "datetime control requires an ID when UserBetterControlNamingScheme is set to true");
            }

            tdDateInstalledStart.ID = id + "CellStart";
            dtpStart.ID = id + "Start";
            ddlDateInstalledParam.ID = id + "Param";
            dtpEnd.ID = id + "End";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method automatically generates the SQL needed for the filter expression.
        /// </summary>
        /// <param name="fieldName">This needs to be the field name you are searching on.</param>
        /// <returns></returns>
        public string FilterExpression(string fieldName)
        {
            if (this.Operator != "BETWEEN")
            {
                return String.Format(" AND [{0}] {1} '{2}'", fieldName, this.Operator, this.EndDate);
            }
            else
            {
                return String.Format(" AND [{0}] >= '{1}' AND [{0}] < '{2}'", fieldName, this.StartDate, this.EndDate.Value.AddDays(1).ToString());
            }
        }

        public void FilterExpression(IFilterBuilder builder, string dataFieldName)
        {
            var selectedEndDate = EndDate;

            if (!selectedEndDate.HasValue)
            {
                // Nothing to see here!
                return;
            }

            // Note: Sql2000 doesn't support DbType.Date. 

            var exp = new FilterBuilderExpression();
            var op = SelectedOperatorType;
            var isBetween = (op == OperatorTypes.Between);
            var isEquals = (op == OperatorTypes.Equals);
            var paramName = FilterBuilderParameter.GetParameterizedFormattedName(dataFieldName);

            if (!isBetween && !isEquals)
            {
                exp.CustomFilterExpression = String.Format("{0} {1} @{2}",
                    FilterBuilderParameter.GetFormattedQualifiedFieldName(dataFieldName),
                    _operatorLookup[op],
                    paramName);
                exp.AddParameter(paramName, DbType.DateTime, selectedEndDate);
            }
            else
            {
                DateTime begin, end;

                // The AddDays(1) is because of the < EndDate query. 

                if (isEquals)
                {
                    begin = selectedEndDate.Value.Date;
                    end = begin.AddDays(1);
                }
                else if (isBetween)
                {
                    begin = StartDate.Value;
                    end = selectedEndDate.Value.AddDays(1);
                }
                else
                // ReSharper disable HeuristicUnreachableCode
                // Won't compile otherwise.
                {
                    throw new NotSupportedException();
                }
                // ReSharper restore HeuristicUnreachableCode

                var beginParam = new FilterBuilderParameter()
                {
                    Name = paramName + PARAMETER_SUFFIXES.START_DATE,
                    Value = begin,
                    DbType = DbType.DateTime
                };

                var endParam = new FilterBuilderParameter()
                {
                    Name = paramName + PARAMETER_SUFFIXES.END_DATE,
                    Value = end,
                    DbType = DbType.DateTime
                };

                exp.CustomFilterExpression = String.Format("{0} >= @{1} AND {0} < @{2}",
                                                FilterBuilderParameter.GetFormattedQualifiedFieldName(dataFieldName),
                                                beginParam.Name,
                                                endParam.Name);

                exp.AddParameter(beginParam);
                exp.AddParameter(endParam);

            }

            builder.AddExpression(exp);
        }

        #endregion

    }
}