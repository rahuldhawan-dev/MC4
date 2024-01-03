using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using MMSINC.ClassExtensions.IOrderedDictionaryExtensions;
using MMSINC.DataPages;

namespace MapCall.Controls.Data
{
    public partial class DataField : UserControl, IDataField
    {
        #region Fields

        protected CascadingDropDown cddCascade;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not changing
        /// the selected index of the DDL should force a PostBack,
        /// if the DataType is DropDownList.
        /// </summary>
        public bool AutoPostBack
        {
            get { return ddlDataField.AutoPostBack; }
            set { ddlDataField.AutoPostBack = value; }
        }

        /// <summary>
        /// Gets or sets the PromptText when using a CascadingDropDown.
        /// </summary>
        public string PromptText { get; set; }

        /// <summary>
        /// Gets or sets the PromptValue when using a CascadingDropDown.
        /// </summary>
        public string PromptValue { get; set; }

        /// <summary>
        /// Gets or sets the ParentControlID when using a CascadingDropDown.
        /// </summary>
        public string ParentControlID { get; set; }

        /// <summary>
        /// Gets or sets the Category when using a CascadingDropDown.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the EmptyText when using a CascadingDropDown.
        /// </summary>
        public string EmptyText { get; set; }

        /// <summary>
        /// Gets or sets the EmptyValue when using a CascadingDropDown.
        /// </summary>
        public string EmptyValue { get; set; }

        /// <summary>
        /// Gets or sets the LoadingText when using a CascadingDropDown.
        /// </summary>
        public string LoadingText { get; set; }

        /// <summary>
        /// Gets or sets the ServicePath when using a CascadingDropDown.
        /// </summary>
        public string ServicePath { get; set; }

        /// <summary>
        /// Gets or sets the ServiceMethod when using a CascadingDropDown.
        /// </summary>
        public string ServiceMethod { get; set; }

        /// <summary>
        /// Set to true if people should be able to enter * in a textbox
        /// and convert the sql to LIKE % % stuff. If no *'s are in the
        /// textbox value, then an exact match is performed.
        /// </summary>
        public bool AllowWildcardsAndExactMatches { get; set; }

        public string HeaderText
        {
            get { return lblHeaderText.Text; }
            set { lblHeaderText.Text = value; }
        }

        public bool ShowTime { get; set; }
        public DateTime StartDate
        {
            get { return dtDataField.StartDate.Value; }
            set { dtDataField.StartDate = value; }
        }
        public DateTime EndDate
        {
            get { return dtDataField.EndDate.Value; }
            set { dtDataField.EndDate = value; }
        }
        public string DataFieldName { get; set; }
        public string SelectCommand { get; set; }
        public string ConnectionString { get; set; }
        public DataTypes DataType { get; set; }
        public bool ListBoxAsStrings { get; set; }

        /// <summary>
        /// Wrapper property to retrieve the value of this control,
        /// no matter what its type is.
        /// </summary>
        public object CurrentValue
        {
            get { return GetCurrentValue(); }
        }

        public bool Required { get; set; }

        public int ListBoxRows
        {
            get { return lbDataField.Rows; }
            set { lbDataField.Rows = value; }
        }
        #endregion

        #region Event Handlers

        protected void Page_Init(object sender, EventArgs e)
        {
            switch (DataType)
            {
                case DataTypes.Date:
                    dtDataField.ShowTimePicker = ShowTime;
                    break;
                case DataTypes.CascadingDropDown:
                    cddCascade = new CascadingDropDown {
                        Enabled = false,
                        EnableViewState = false,
                        TargetControlID = "ddlDataField",
                        EmptyText = EmptyText ?? "None Found",
                        EmptyValue = EmptyValue ?? "",
                        PromptText = PromptText ?? "--Select Here--",
                        PromptValue = PromptValue ?? "",
                        ParentControlID = ParentControlID,
                        Category = Category,
                        LoadingText = LoadingText,
                        ServicePath = ServicePath,
                        ServiceMethod = ServiceMethod
                    };
                    ddlDataField.Parent.Controls.Add(cddCascade);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(SelectCommand) && string.IsNullOrWhiteSpace(ConnectionString))
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString;
            }

            dsDataField.SelectCommand = SelectCommand;
            dsDataField.ConnectionString = ConnectionString;
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            switch (DataType)
            {
                case DataTypes.Boolean:
                    chkDataField.Visible = chkDataField.EnableViewState = true;
                    break;
                case DataTypes.BooleanDropDown:
                    ddlBoolean.Visible = ddlBoolean.EnableViewState = true;
                    break;
                case DataTypes.Date:
                    dtDataField.Visible = dtDataField.EnableViewState = true;
                    dtDataField.ShowTimePicker = ShowTime;
                    break;
                case DataTypes.NumberRange:
                    numDataField.Visible = numDataField.EnableViewState = true;
                    break;
                case DataTypes.CascadingDropDown:
                    cddCascade.Enabled = cddCascade.EnableViewState = true;
                    goto case DataTypes.DropDownList;
                case DataTypes.DropDownList:
                    ddlDataField.Visible = ddlDataField.EnableViewState = true;
                    ddlRequired.Enabled = ddlRequired.Visible = Required;
                    if (DataType == DataTypes.DropDownList && ddlDataField.Items.Count == 0)
                        ddlDataField.DataBind();
                    break;
                case DataTypes.ListBox:
                    lbDataField.Visible = lbDataField.EnableViewState = true;
                    if (lbDataField.Items.Count == 0)
                        lbDataField.DataBind();
                    break;
                case DataTypes.Integer:
                    cvDataFieldInteger.Enabled = cvDataFieldInteger.Visible = true;
                    txtDataField.Visible = txtDataField.EnableViewState = true;
                    break;
                case DataTypes.Double:
                    cvDataFieldDouble.Enabled = cvDataFieldDouble.Visible = true;
                    txtDataField.Visible = txtDataField.EnableViewState = true;
                    break;
                default:
                    txtDataField.Visible = txtDataField.EnableViewState = true;
                    break;
            }
        }

        protected void ddlDataField_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("--Select Here--", ""));
        }

        #endregion

        #region Exposed Methods

        public void FilterExpression(IFilterBuilder builder)
        {
            switch (DataType)
            {
                case DataTypes.Integer:
                    if (!String.IsNullOrWhiteSpace(txtDataField.Text))
                    {
                        int val;
                        if (int.TryParse(txtDataField.Text, out val))
                        {
                            builder.AddExpression(new FilterBuilderExpression(DataFieldName, DbType.Int32, val));
                        }
                    }
                    break;
                case DataTypes.Double:
                    if (!String.IsNullOrWhiteSpace(txtDataField.Text))
                    {
                        double val;
                        if (double.TryParse(txtDataField.Text, out val))
                        {
                            builder.AddExpression(new FilterBuilderExpression(DataFieldName, DbType.Double, val));
                        }
                    }
                    break;
                case DataTypes.Boolean:
                    if (chkDataField.Checked)
                        builder.AddExpression(new FilterBuilderExpression(DataFieldName, DbType.Boolean, chkDataField.Checked));
                    break;
                case DataTypes.BooleanDropDown:
                    if (ddlBoolean.SelectedIndex > 0)
                    {
                        bool val;

                        if (ddlBoolean.SelectedValue == "0")
                        {
                            val = false;
                        }
                        else if (ddlBoolean.SelectedValue == "1")
                        {
                            val = true;
                        }
                        else
                        {
                            throw new NotSupportedException("Invalid BooleanDropDown value.");
                        }

                        builder.AddExpression(new FilterBuilderExpression(DataFieldName, DbType.Boolean, val)
                                                  {
                                                      CustomFilterExpression = string.Format("{0} = @{1}",
                                                        FilterBuilderParameter.GetFormattedQualifiedFieldName(DataFieldName),
                                                        FilterBuilderParameter.GetParameterizedFormattedName(DataFieldName)),
                                                      IgnoreIfThereAreNullParameters = true
                                                  });

                    }
                    break;
                case DataTypes.Date:
                    if (dtDataField.EndDate.HasValue)
                    {
                        dtDataField.FilterExpression(builder, DataFieldName);
                    }

                    break;
                case DataTypes.NumberRange:
                    if (numDataField.End.Length > 0)
                    {
                        // TODO: This needs to work properly with DbType. Nothing that uses it
                        //       differs between ints and floats/decimals. 
                        numDataField.FilterExpression(builder, DataFieldName);
                    }

                    break;
                case DataTypes.DropDownList:
                    if (!string.IsNullOrEmpty(ddlDataField.SelectedValue))
                    {
                        builder.AddExpression(new FilterBuilderExpression(DataFieldName, DbType.String,
                                                                          ddlDataField.SelectedValue));
                    }
                    break;
                case DataTypes.CascadingDropDown:
                    if (!string.IsNullOrWhiteSpace(cddCascade.SelectedValue))
                    {
                        var value = IOrderedDictionaryExtensions.CleanValue(ddlDataField.SelectedValue);
                        builder
                            .AddExpression(new FilterBuilderExpression(
                                DataFieldName, DbType.String, value));
                    }
                    break;
                case DataTypes.ListBox:
                    if (lbDataField.SelectedIndex > 0)
                    {
                        var exp = new FilterBuilderExpression();

                        var innerParams = new List<string>(lbDataField.Items.Count);

                        for (var i = 0; i < lbDataField.Items.Count; i++)
                        {
                            var cur = lbDataField.Items[i];
                            if (cur.Selected)
                            {
                                var p = new FilterBuilderParameter
                                            {
                                                Name = DataFieldName + i,
                                                Value = cur.Value,
                                                DbType = (ListBoxAsStrings ? DbType.String : DbType.Int32)
                                            };
                                exp.AddParameter(p);
                                innerParams.Add("@" + p.ParameterFormattedName);
                            }
                        }

                        if (innerParams.Any())
                        {
                            var parms = String.Join(", ", innerParams.ToArray());
                            var formattedLeftSide = FilterBuilderParameter.GetFormattedQualifiedFieldName(DataFieldName);
                            exp.CustomFilterExpression = string.Format("{0} in ({1})", formattedLeftSide, parms);
                            builder.AddExpression(exp);
                        }

                    }
                    break;
                default:   //DataTypes.String
                    if (!String.IsNullOrWhiteSpace(txtDataField.Text))
                    {
                        var exp = new FilterBuilderExpression();
                        var formattedLeftSide = FilterBuilderParameter.GetFormattedQualifiedFieldName(DataFieldName);
                        var formattedRightSide = FilterBuilderParameter.GetParameterizedFormattedName(DataFieldName);

                        var text = txtDataField.Text.Trim();

                        if (AllowWildcardsAndExactMatches)
                        {
                            if (text.Contains("*"))
                            {
                                text = text.Replace('*', '%');
                                exp.CustomFilterExpression = string.Format("{0} like @{1}", formattedLeftSide, formattedRightSide);
                            }

                        }
                        else
                        {
                            exp.CustomFilterExpression = string.Format("{0} like '%' + @{1} + '%'", formattedLeftSide, formattedRightSide);
                        }

                        exp.AddParameter(DataFieldName, DbType.String, text);
                        builder.AddExpression(exp);
                    }

                    break;
            }
        }


        public string FilterExpression()
        {
            var returnString = string.Empty;

            switch (DataType)
            {
                case DataTypes.Integer:
                    if (!string.IsNullOrEmpty(txtDataField.Text))
                    {
                        int val;
                        if (int.TryParse(txtDataField.Text, out val))
                        {
                            returnString = String.Format(" AND [{0}] = {1}", DataFieldName, val);
                        }
                    }
                    break;
                case DataTypes.Double:
                    if (!string.IsNullOrEmpty(txtDataField.Text))
                    {
                        double val;
                        if (double.TryParse(txtDataField.Text, out val))
                        {
                            returnString = String.Format(" AND [{0}] = {1}", DataFieldName, val);
                        }
                    }
                    break;
                case DataTypes.Boolean:
                    if (chkDataField.Checked)
                        returnString = String.Format(" AND [{0}] = 1", DataFieldName);
                    break;
                case DataTypes.BooleanDropDown:
                    if (ddlBoolean.SelectedIndex > 0)
                    {
                        returnString = String.Format(" AND isNull([{0}],0) = {1}",
                                                     DataFieldName,
                                                     ddlBoolean.SelectedValue);
                    }
                    break;
                case DataTypes.Date:
                    if (dtDataField.EndDate.HasValue)
                        returnString = dtDataField.FilterExpression(DataFieldName);
                    break;
                case DataTypes.NumberRange:
                    if (numDataField.End.Length > 0)
                        returnString = numDataField.FilterExpression(DataFieldName);
                    break;
                case DataTypes.DropDownList:
                    if (ddlDataField.SelectedIndex > 0)
                    {
                        returnString = String.Format(" AND [{0}] = '{1}'", DataFieldName, SanitizeKeepApostrophes(ddlDataField.SelectedValue));
                    }
                    break;
                case DataTypes.CascadingDropDown:
                    if (ddlDataField.SelectedValue != PromptValue)
                    {
                        var value = IOrderedDictionaryExtensions.CleanValue(ddlDataField.SelectedValue);
                        returnString = String.Format(" AND [{0}] = '{1}'", DataFieldName, SanitizeKeepApostrophes(value));
                    }
                    break;
                case DataTypes.ListBox:
                    if (lbDataField.SelectedIndex > 0)
                    {
                        var lbSelectedItems = new StringBuilder();
                        lbSelectedItems.AppendFormat(" AND [{0}] in (", DataFieldName);
                        foreach (ListItem li in lbDataField.Items)
                        {
                            if (li.Selected)
                            {
                                if (ListBoxAsStrings)
                                    lbSelectedItems.AppendFormat(", '{0}'", SanitizeKeepApostrophes(li.Value));
                                else
                                    lbSelectedItems.AppendFormat(", {0}", SanitizeKeepApostrophes(li.Value));
                            }
                        }
                        lbSelectedItems.Append(")");

                        lbSelectedItems.Replace("(, ", "(");
                        returnString = lbSelectedItems.ToString();
                    }
                    break;
                default:   //DataTypes.String
                    if (!string.IsNullOrEmpty(txtDataField.Text))
                    {
                        returnString = String.Format(" AND [{0}] like '%{1}%'", DataFieldName, SanitizeKeepApostrophes(txtDataField.Text));
                    }

                    break;
            }

            return returnString;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This is a hacky sql sanitize method for preventing atleast some injection problems, or any error that
        /// involves apostrophes.
        /// </summary>
        /// <param name="sqlValue"></param>
        /// <returns></returns>
        private static string SanitizeKeepApostrophes(string sqlValue)
        {
            if (string.IsNullOrEmpty(sqlValue))
            {
                return sqlValue;
            }

            return sqlValue.Replace("'", "''");
        }



        private object GetCurrentValue()
        {
            switch (DataType)
            {
                case DataTypes.Boolean:
                    return chkDataField.Checked;
                case DataTypes.BooleanDropDown:
                    return ddlBoolean.SelectedValue;
                case DataTypes.CascadingDropDown:
                case DataTypes.DropDownList:
                    return ddlDataField.SelectedValue;
                case DataTypes.ListBox:
                    return lbDataField.SelectedValue;
                case DataTypes.String:
                    return txtDataField.Text;
                // TODO:  IDK what to do about this.  There's no "value" when there's a range,
                // just a filter expression.  Maybe that's how this should work?
                //case DataTypes.Date:
                //    return dtDataField
                //case DataTypes.Number:
                //    return numDataField
                default:
                    throw (new InvalidOperationException(
                        string.Format("The CurrentValue property is not available for DataFields of DataType '{0}'.",
                        DataType.ToString())));
            }
        }

        #endregion
    }

    /// <summary>
    /// Enumeration value used to specify the type of field being considered.
    /// </summary>
    public enum DataTypes
    {
        String,
        Integer,
        Double,
        Boolean,
        Date,
        NumberRange,
        DropDownList,
        CascadingDropDown,
        ListBox,
        BooleanDropDown
    }

    public class CustomerSurveyDataField : DataField
    {
        public int QuestionID { get; set; }
    }
}