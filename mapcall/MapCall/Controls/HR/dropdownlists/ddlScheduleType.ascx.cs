using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Controls.HR.dropdownlists
{
    public partial class ddlScheduleType : UserControl
    {
        public string SelectedValue
        {
            get { return ddl_ScheduleType.SelectedValue; }
            set { ddl_ScheduleType.SelectedValue = value; }
        }
        public int SelectedIndex
        {
            get { return ddl_ScheduleType.SelectedIndex; }
            set { ddl_ScheduleType.SelectedIndex = value; }
        }
        public string Width
        {
            get { return ddl_ScheduleType.Width.ToString(); }
            set { ddl_ScheduleType.Width = Unit.Parse(value); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}