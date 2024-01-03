using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;

namespace MapCall.Controls.Data
{
    public partial class number : UserControl
    {
        #region Fields

        private static readonly Dictionary<OperatorTypes, string> _operatorLookup;

        #endregion

        #region Properties

        /// <summary>
        /// Set this to true if the control IDs should be set to a more guaranteed unique name than
        /// just txtStart/txtEnd. This is false by default for backwards compatability.
        /// </summary>
        public bool UseBetterControlNamingScheme { get; set; }

        public string Start
        {
            get
            {
                return txtStart.Text;
            }
        }
        public string End
        {
            get
            {
                return txtEnd.Text;
            }
        }
        public string Operator
        {
            get { return ddlParam.SelectedValue; }
        }

        public OperatorTypes SelectedOperatorType
        {
            get
            {
                var selected = this.ddlParam.SelectedItem.Text;
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
                ddlParam.SelectedIndex = Int32.Parse(value);
            }
        }

        public bool HasValidEndValue
        {
            get { return IsValidDouble(End); }
        }

        public bool HasValidStartValue
        {
            get { return IsValidDouble(Start); }
        }

        #endregion

        #region Constructors

        static number()
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

        private static bool IsValidDouble(string val)
        {
            double tester;
            return double.TryParse(val, out tester);
        }

        private ListItem GetListItemByOperator(OperatorTypes opType)
        {
            var opStr = _operatorLookup[opType];
            return (from ListItem li in ddlParam.Items
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ddlParam.Attributes.Add("onChange", string.Format("ControlChange('{0}', '{1}');", tdStart.ClientID, ddlParam.ClientID));
            ScriptManager.RegisterStartupScript(this, typeof(string), this.UniqueID + "Script", string.Format("if (document.getElementById('{0}')!=null)\n\tControlChange('{0}', '{1}');\n", tdStart.ClientID, ddlParam.ClientID), true);
        }

        private void EnsureControlIds()
        {
            if (!UseBetterControlNamingScheme) { return; }

            if (string.IsNullOrWhiteSpace(this.ID))
            {
                throw new NullReferenceException(
                    "Number control requires an ID when UserBetterControlNamingScheme is set to true");
            }

            tdStart.ID = this.ID + "CellStart";
            txtStart.ID = this.ID + "Start";
            ddlParam.ID = this.ID + "Param";
            txtEnd.ID = this.ID + "End";
            cvEnd.ID = this.ID + "cvEnd";
            cvEnd.ControlToValidate = txtEnd.ID;
            CompareValidator1.ID = this.ID + "cvStart";
            CompareValidator1.ControlToValidate = txtStart.ID;

        }

        #endregion

        #region Exposed Methods

        public string FilterExpression(string fieldName)
        {
            if (Operator != "BETWEEN")
            {
                return String.Format(" AND [{0}] {1} '{2}'", fieldName, Operator, End);
            }
            return String.Format(" AND [{0}] >= '{1}' AND [{0}] <= '{2}'", fieldName, Start, End);
        }

        public void FilterExpression(IFilterBuilder builder, string dataFieldName)
        {
            if (!HasValidEndValue) { return; }

            var exp = new FilterBuilderExpression();
            var isBetween = Operator.Equals("BETWEEN", StringComparison.OrdinalIgnoreCase);

            if (!isBetween)
            {

                exp.CustomFilterExpression = String.Format("[{0}] {1} @{0}", dataFieldName, Operator);
                exp.AddParameter(dataFieldName, DbType.Double, Double.Parse(End));
            }
            else
            {
                if (!HasValidStartValue) { return; }

                var startParam = new FilterBuilderParameter
                                     {
                                         Name = dataFieldName + "Start",
                                         Value = Start,
                                         DbType = DbType.Double
                                     };
                var endParam = new FilterBuilderParameter
                                   {
                                       Name = dataFieldName + "End",
                                       Value = End,
                                       DbType = DbType.Double
                                   };
                exp.CustomFilterExpression = String.Format("[{0}] >= @{1} AND [{0}] <= @{2}",
                                           dataFieldName, startParam.Name, endParam.Name);
                exp.AddParameter(startParam);
                exp.AddParameter(endParam);
            }

            builder.AddExpression(exp);
        }

        #endregion
    }
}