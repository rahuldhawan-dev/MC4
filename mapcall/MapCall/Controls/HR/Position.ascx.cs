using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;

namespace MapCall.Controls.HR
{
    public partial class Position : UserControl
    {
        public bool AllowEdit, AllowDelete;
        public int positionID;
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
            if (positionID != 0)
            {
                SqlDataSource1.SelectParameters[0].DefaultValue = positionID.ToString();
            }
            var btnDelete = Utility.GetFirstControlInstance(DetailsView1, "btnDelete");
            if (btnDelete != null)
                btnDelete.Visible = AllowDelete;
            var btnEdit = Utility.GetFirstControlInstance(DetailsView1, "btnEdit");
            if (btnEdit != null)
                btnEdit.Visible = AllowEdit;
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
            SqlDataSource1.SelectParameters[0].DefaultValue = ((IDbDataParameter)e.Command.Parameters["@PositionID"]).Value.ToString();
            Audit.Insert(
                (int)AuditCategory.DataInsert,
                Page.User.Identity.Name,
                String.Format("Added PositionID:{0}", ((IDbDataParameter)e.Command.Parameters["@PositionID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
            if (!String.IsNullOrEmpty(SqlDataSource1.SelectParameters[0].DefaultValue))
            {
                this.positionID = Int32.Parse(SqlDataSource1.SelectParameters[0].DefaultValue);
                OnItemInserted(e);
            }
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            DetailsView1.DataBind();
        }
        protected void SqlDataSource1_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            lblResults.Text = "Record Deleted";
            Audit.Insert(
                (int)AuditCategory.DataDelete,
                Page.User.Identity.Name,
                String.Format("Deleted PositionID:{0}", ((IDbDataParameter)e.Command.Parameters["@PositionID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }
        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            lblResults.Text = "Record Updated";
            Audit.Insert(
                (int)AuditCategory.DataUpdate,
                Page.User.Identity.Name,
                String.Format("Updated PositionID:{0}", ((IDbDataParameter)e.Command.Parameters["@PositionID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }
    }
}