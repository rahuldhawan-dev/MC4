using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;

namespace MapCall.Controls.HR
{
    public partial class SystemDelivery : UserControl
    {
        public int SystemDeliveryID;
        public event EventHandler ItemInserted;
        protected void OnItemInserted(EventArgs e)
        {
            if (ItemInserted != null)
            {
                ItemInserted(this, e);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblResults.Text = "";
        }
        protected void DetailsView1_DataBound(object sender, EventArgs e)
        {
            if (SystemDeliveryID != 0)
            {
                SqlDataSource1.SelectParameters[0].DefaultValue = SystemDeliveryID.ToString();
            }
        }
        public void ChangeMode(DetailsViewMode dvm)
        {
            if (dvm == DetailsViewMode.Insert)
            {
                DetailsView1.ChangeMode(DetailsViewMode.Insert);
            }
        }
        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            SqlDataSource1.SelectParameters[0].DefaultValue = ((IDbDataParameter)e.Command.Parameters["@SysDelID"]).Value.ToString();
            Audit.Insert(
                (int)AuditCategory.DataInsert,
                Page.User.Identity.Name,
                String.Format("Added SystemDeliveryID:{0}", ((IDbDataParameter)e.Command.Parameters["@SysDelID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
            this.SystemDeliveryID = Int32.Parse(SqlDataSource1.SelectParameters[0].DefaultValue);
            OnItemInserted(e);
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            DetailsView1.DataBind();
        }
        protected void SqlDataSource1_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            lblResults.Text = "Record Deleted";
            Audit.Insert(
                (int)AuditCategory.DataDelete,
                Page.User.Identity.Name,
                String.Format("Deleted SystemDeliveryID:{0}", ((IDbDataParameter)e.Command.Parameters["@sysDelID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }
        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            lblResults.Text = "Record Updated";
            Audit.Insert(
                (int)AuditCategory.DataUpdate,
                Page.User.Identity.Name,
                String.Format("Updated SystemDeliveryID:{0}", ((IDbDataParameter)e.Command.Parameters["@sysDelID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }
    }
}