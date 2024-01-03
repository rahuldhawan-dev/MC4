using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.HR.dropdownlists
{
    public partial class ddlFacility : UserControl
    {
        public string SelectedValue
        {
            get { return ddl_Facility.SelectedValue; }
            set { ddl_Facility.SelectedValue = value; }
        }
        public int SelectedIndex
        {
            get { return ddl_Facility.SelectedIndex; }
            set { ddl_Facility.SelectedIndex = value; }
        }
        public string Width
        {
            get { return ddl_Facility.Width.ToString(); }
            set { ddl_Facility.Width = Unit.Parse(value); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}