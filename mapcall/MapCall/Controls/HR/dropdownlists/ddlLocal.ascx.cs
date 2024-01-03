using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.HR.dropdownlists
{
    public partial class ddlLocal : UserControl
    {
        public string SelectedValue
        {
            get { return ddl_Local.SelectedValue; }
            set { ddl_Local.SelectedValue = value; }
        }
        public int SelectedIndex
        {
            get { return ddl_Local.SelectedIndex; }
            set { ddl_Local.SelectedIndex = value; }
        }
        public string Width
        {
            get { return ddl_Local.Width.ToString(); }
            set { ddl_Local.Width = Unit.Parse(value); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}