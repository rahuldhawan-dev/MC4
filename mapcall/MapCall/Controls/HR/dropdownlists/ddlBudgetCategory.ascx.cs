using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.HR.dropdownlists
{
    public partial class ddlBudgetCategory : UserControl
    {
        public string SelectedValue
        {
            get { return ddl_LookupType.SelectedValue; }
            set { ddl_LookupType.SelectedValue = value; }
        }
        public int SelectedIndex
        {
            get { return ddl_LookupType.SelectedIndex; }
            set { ddl_LookupType.SelectedIndex = value; }
        }
        public string Width
        {
            get { return ddl_LookupType.Width.ToString(); }
            set { ddl_LookupType.Width = Unit.Parse(value); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}