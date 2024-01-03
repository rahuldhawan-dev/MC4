using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Modules.Maps
{
    // TODO: This page should really redirect to the MVC show fragment. This page is only used by RTO
    //       and it has to load in an iframe which is why it doesn't use the mvc fragment.
    
    public partial class hydrant_mvc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //lblHydNum.Text = Request.QueryString["hydNum"];
            SqlDataSource1.SelectParameters["RecID"].DefaultValue = Request.QueryString["RecID"];
            if (Request.QueryString["toggle"] == "true")
            {
                var icon = Request.QueryString["icon"];
                string script = String.Format("window.parent.TheEsri.setCurrentMarkerIcon('{0}');", icon);
                ScriptManager.RegisterStartupScript(Page, typeof(string), "toggleScript", script, true);
            }
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            var hlHydrant = (HyperLink)MMSINC.Utility.GetFirstControlInstance(FormView1, "hlHydrant");
            var lblCritical = (Label)MMSINC.Utility.GetFirstControlInstance(FormView1, "lblCritical");

            var dataRowView = (DataRowView)FormView1.DataItem;

            if (dataRowView == null) return;

            if (dataRowView.Row["Critical"].ToString() == "True")
                lblCritical.Text = String.Format("<b>CRITICAL:</b> {0}", dataRowView.Row["CriticalNotes"]);
            if (hlHydrant != null)
            {
                hlHydrant.NavigateUrl = String.Format("~/modules/mvc/fieldoperations/hydrant/show/{0}",
                    dataRowView.Row["RecID"]);
                hlHydrant.Text = dataRowView.Row["HydrantNumber"].ToString();
            }
        }
    }
}