using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Modules.Maps
{
    // TODO: This page should really redirect to the MVC show fragment. This page is only used by RTO
    //       and it has to load in an iframe which is why it doesn't use the mvc fragment.
    public partial class valve_mvc : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlDataSource1.SelectParameters["RecID"].DefaultValue = Request.QueryString["RecID"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["toggle"] == "true")
            {
                var icon = Request.QueryString["icon"];
                string script = String.Format("window.parent.TheEsri.setCurrentMarkerIcon('{0}');", icon);
                ScriptManager.RegisterStartupScript(Page, typeof(string), "toggleScript", script, true);
            }
        }

        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            var hlValve = (HyperLink)MMSINC.Utility.GetFirstControlInstance(FormView1, "hlValve");
            var lblCritical = (Label)MMSINC.Utility.GetFirstControlInstance(FormView1, "lblCritical");

            if (FormView1.DataItem == null) return;
            var dataRowView = (DataRowView)FormView1.DataItem;
            
            if (dataRowView.Row["Critical"].ToString() == "True")
                lblCritical.Text = String.Format("<b>CRITICAL:</b><br> {0}", dataRowView.Row["CriticalNotes"]);

            if (hlValve != null)
            {
                hlValve.NavigateUrl = String.Format("~/modules/mvc/fieldoperations/valve/show/{0}",
                    dataRowView.Row["RecID"]);
                hlValve.Text = dataRowView.Row["ValveNumber"].ToString();
            }
        }
    }
}