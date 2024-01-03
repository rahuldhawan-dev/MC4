using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.HR.dropdownlists
{
    public partial class ddlExpenseLineNumber : UserControl
    {
        public string SelectedValue
        {
            get { return ddl_ExpenseLineNumber.SelectedValue; }
            set { ddl_ExpenseLineNumber.SelectedValue = value; }
        }
        public int SelectedIndex
        {
            get { return ddl_ExpenseLineNumber.SelectedIndex; }
            set { ddl_ExpenseLineNumber.SelectedIndex = value; }
        }
        public string Width
        {
            get { return ddl_ExpenseLineNumber.Width.ToString(); }
            set { ddl_ExpenseLineNumber.Width = Unit.Parse(value); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}