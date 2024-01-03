using System;
using System.Web.UI;

namespace MapCall.Modules.HR.Employee
{
    public partial class Schedule : Page
    {
        public string ScheduleTypeID
        {
            get
            {
                if (this.ViewState["ScheduleTypeID"] == null)
                    return String.Empty;
                else
                    return this.ViewState["ScheduleTypeID"].ToString();
            }
            set { this.ViewState["ScheduleTypeID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ScheduleTypeID = Request["ScheduleTypeID"];
            SqlDataSource1.SelectParameters[0].DefaultValue = ScheduleTypeID;
            GridView1.DataBind();
        }
    }
}
