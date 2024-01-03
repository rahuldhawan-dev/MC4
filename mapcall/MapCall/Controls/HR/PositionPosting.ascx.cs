using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;

namespace MapCall.Controls.HR
{
    public partial class PositionPosting : UserControl
    {
        public bool AllowEdit, AllowDelete;
        public int positionPostingID;
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
            if (positionPostingID != 0)
            {
                SqlDataSource1.SelectParameters[0].DefaultValue = positionPostingID.ToString();
            }
            if (DetailsView1.CurrentMode != DetailsViewMode.ReadOnly)
            {
                GenerateClientScripts();
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
            SqlDataSource1.SelectParameters[0].DefaultValue = ((IDbDataParameter)e.Command.Parameters["@Position_Posting_ID"]).Value.ToString();
            Audit.Insert(
                (int)AuditCategory.DataInsert,
                Page.User.Identity.Name,
                String.Format("Added Position_Posting_ID:{0}", ((IDbDataParameter)e.Command.Parameters["@Position_Posting_ID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
            this.positionPostingID = Int32.Parse(SqlDataSource1.SelectParameters[0].DefaultValue);
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
                String.Format("Deleted Position_Posting_ID:{0}", ((IDbDataParameter)e.Command.Parameters["@Position_Posting_ID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }
        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            lblResults.Text = "Record Updated";
            Audit.Insert(
                (int)AuditCategory.DataUpdate,
                Page.User.Identity.Name,
                String.Format("Updated Position_Posting_ID:{0}", ((IDbDataParameter)e.Command.Parameters["@Position_Posting_ID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }
        private void GenerateClientScripts()
        {
            StringBuilder sb = new StringBuilder();
            CheckBox chkInternal_Posting = (CheckBox)Utility.GetFirstControlInstance(DetailsView1, "chkInternal_Posting");
            CheckBox chkExternal_Recruitment = (CheckBox)Utility.GetFirstControlInstance(DetailsView1, "chkExternal_Recruitment");
            CheckBox chkInterviewing = (CheckBox)Utility.GetFirstControlInstance(DetailsView1, "chkInterviewing");
            CheckBox chkCandidate_Selected = (CheckBox)Utility.GetFirstControlInstance(DetailsView1, "chkCandidate_Selected");
            TextBox dvTxtEffective_Date_0 = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtEffective_Date_0");
            var LinkButton1 = (LinkButton)Utility.GetFirstControlInstance(DetailsView1, "LinkButton1");

            if (dvTxtEffective_Date_0 == null)
                dvTxtEffective_Date_0 = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtEffective_Date_-1");

            if (dvTxtEffective_Date_0 != null)
            {
                dvTxtEffective_Date_0.Attributes.Add("onchange",
                    String.Format("PosPos_CheckEffective('{0}', '{1}', '{2}', '{3}', '{4}');", dvTxtEffective_Date_0.ClientID, chkInternal_Posting.ClientID, chkExternal_Recruitment.ClientID, chkInterviewing.ClientID, chkCandidate_Selected.ClientID)
                );
                sb.Append(String.Format("PosPos_CheckEffective('{0}', '{1}', '{2}', '{3}', '{4}');", dvTxtEffective_Date_0.ClientID, chkInternal_Posting.ClientID, chkExternal_Recruitment.ClientID, chkInterviewing.ClientID, chkCandidate_Selected.ClientID));

                // This was wired to this onclick which is either the insert or update button click now
                // instead of the onsubmit for the form. This fixes the other parts of the page breaking,
                // when leaving the detail panel.
                // TODO: Redo with jquery if possible.
                if (LinkButton1 != null)
                    LinkButton1.Attributes.Add("onclick",
                        String.Format("PosPos_CheckEffective('{0}', '{1}', '{2}', '{3}', '{4}');", dvTxtEffective_Date_0.ClientID, chkInternal_Posting.ClientID, chkExternal_Recruitment.ClientID, chkInterviewing.ClientID, chkCandidate_Selected.ClientID)
                    );
            }


            chkInternal_Posting.Attributes.Add("onclick", 
                String.Format("document.getElementById('{0}').checked=false;document.getElementById('{1}').checked=false;",
                    chkInterviewing.ClientID, chkCandidate_Selected.ClientID)
                );
            chkExternal_Recruitment.Attributes.Add("onclick",
                String.Format("document.getElementById('{0}').checked=false;document.getElementById('{1}').checked=false;",
                    chkInterviewing.ClientID, chkCandidate_Selected.ClientID)
                );
            chkInterviewing.Attributes.Add("onclick",
                String.Format("document.getElementById('{0}').checked=false;document.getElementById('{1}').checked=false;document.getElementById('{2}').checked=false;",
                    chkInternal_Posting.ClientID, chkExternal_Recruitment.ClientID, chkCandidate_Selected.ClientID)
                );
            chkCandidate_Selected.Attributes.Add("onclick",
                String.Format("document.getElementById('{0}').checked=false;document.getElementById('{1}').checked=false;document.getElementById('{2}').checked=false;",
                    chkInternal_Posting.ClientID, chkExternal_Recruitment.ClientID, chkInterviewing.ClientID)
                );   
            Page.ClientScript.RegisterStartupScript(typeof(string), "ValidationScript",sb.ToString(), true); 
        }
    }
}