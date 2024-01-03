using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;

namespace MapCall.Controls.HR
{
    public partial class PositionHistory : UserControl
    {
        public bool AllowEdit, AllowDelete;
        public int positionHistoryID;
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
            if (positionHistoryID != 0)
            {
                SqlDataSource1.SelectParameters[0].DefaultValue = positionHistoryID.ToString();
            }
            var btnDelete = Utility.GetFirstControlInstance(DetailsView1, "btnDelete");
            if (btnDelete != null)
                btnDelete.Visible = AllowDelete;
            var btnEdit = Utility.GetFirstControlInstance(DetailsView1, "btnEdit");
            if (btnEdit != null)
                btnEdit.Visible = AllowEdit;

            var lblPosition_Category = (Label)Utility.GetFirstControlInstance(DetailsView1, "lblPosition_Category");
            var lblOpCode = (Label)Utility.GetFirstControlInstance(DetailsView1, "lblOpCode");
            var lbOpCode = (ListBox)Utility.GetFirstControlInstance(DetailsView1, "lbOpCode");
            var gvOpCode = (GridView)Utility.GetFirstControlInstance(DetailsView1, "gvOpCode");
            
            switch (DetailsView1.CurrentMode)
            {
                case DetailsViewMode.Edit:
                    if (lblPosition_Category != null && lblPosition_Category.Text != "Bargaining Unit" && lbOpCode != null)
                    {
                        lbOpCode.Visible = true;
                        lblOpCode.Visible = false;
                    }
                    if (lbOpCode != null)
                    {
                        var dv = (DataView)dsOpCodeSelected.Select(DataSourceSelectArguments.Empty);
                        foreach (ListItem li in lbOpCode.Items)
                        {
                            foreach (DataRow datarow in dv.Table.Rows)
                            {
                                if (li.Text == datarow[0].ToString())
                                    li.Selected = true;
                            }
                        }
                    }
                    break;
                case DetailsViewMode.Insert:
                    break;
                case DetailsViewMode.ReadOnly:
                    if (lblPosition_Category != null && lblPosition_Category.Text != "Bargaining Unit")
                    {
                        gvOpCode.Visible = true;
                        lblOpCode.Visible = false;
                    }
                    break;
                default:
                    break;
            }
        }
        public void ChangeMode(DetailsViewMode dvm)
        {
            if (dvm == DetailsViewMode.Insert)
            {
                DetailsView1.ChangeMode(DetailsViewMode.Insert);
            }
        }
        public string ScheduleURL(string x)
        {
            return string.Format("schedule.aspx?scheduleTypeID={0}", x);
        }
        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            SqlDataSource1.SelectParameters[0].DefaultValue = ((IDbDataParameter)e.Command.Parameters["@Position_History_ID"]).Value.ToString();
            Audit.Insert(
                (int)AuditCategory.DataInsert,
                Page.User.Identity.Name,
                String.Format("Added Position_History_ID:{0}", ((IDbDataParameter)e.Command.Parameters["@Position_History_ID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
            this.positionHistoryID = Int32.Parse(SqlDataSource1.SelectParameters[0].DefaultValue);

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
                String.Format("Deleted Position_History_ID:{0}", ((IDbDataParameter)e.Command.Parameters["@Position_History_ID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }
        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            positionHistoryID = Int32.Parse(((IDbDataParameter)e.Command.Parameters["@Position_History_ID"]).Value.ToString());
            lblResults.Text = "Record Updated";
            Audit.Insert(
                (int)AuditCategory.DataUpdate,
                Page.User.Identity.Name,
                String.Format("Updated Position_History_ID:{0}", ((IDbDataParameter)e.Command.Parameters["@Position_History_ID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );

            //OpCode 
            var lbOpCode = (ListBox)Utility.GetFirstControlInstance(DetailsView1, "lbOpCode");
            var sb = new StringBuilder();
            if (lbOpCode != null)
            {
                foreach (ListItem li in lbOpCode.Items)
                {
                    if (li.Selected)
                        sb.AppendFormat("IF NOT EXISTS (select * from tblPosition_History_Opcode where Position_History_ID = {0} and OpCode = '{1}') BEGIN INSERT INTO tblPosition_History_Opcode(Position_History_ID, opCode) Values({0}, '{1}') END;", this.positionHistoryID, li.Value);
                    else
                        sb.AppendFormat("DELETE tblPosition_History_Opcode where Position_History_ID = {0} and OpCode = '{1}';", this.positionHistoryID, li.Value);
                }
            }

            if (sb.Length > 0)
            {
                using (var cmd = new SqlCommand())
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ToString()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sb.ToString();
                    cmd.Connection = conn;
                    conn.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        lblResults.ForeColor = Color.Red;
                        lblResults.Text += ex.Message;
                        throw;
                    }
                }
            }
        }
     }
}