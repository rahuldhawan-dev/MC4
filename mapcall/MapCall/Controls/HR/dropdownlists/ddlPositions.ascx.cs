using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.HR.dropdownlists
{
    public partial class ddlPositions : UserControl
    {
        public string SelectedValue
        {
            get { return ddl_Positions.SelectedValue; }
            set { ddl_Positions.SelectedValue = value; }
        }
        public int SelectedIndex
        {
            get { return ddl_Positions.SelectedIndex; }
            set { ddl_Positions.SelectedIndex = value; }
        }
        public string Width
        {
            get { return ddl_Positions.Width.ToString(); }
            set { ddl_Positions.Width = Unit.Parse(value); }
        }
        public bool Required
        {
            set { rfv_ddl_Positions.Enabled = value; }
            get { return rfv_ddl_Positions.Enabled; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}