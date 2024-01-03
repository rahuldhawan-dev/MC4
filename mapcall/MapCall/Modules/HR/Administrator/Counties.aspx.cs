using System;
using System.Configuration;
using System.Text;
using MMSINC;
using MMSINC.Page;
using MapCall.Controls.Data;

namespace MapCall.Modules.HR.Administrator
{
    public partial class Counties : DataElementPage
    {
        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Object ctrl in pnlSearch.Controls)
                if (ctrl is DataField)
                    sb.Append(((DataField)ctrl).FilterExpression());

            if (sb.Length > 0)
            {
                SqlDataSource1.FilterExpression = sb.ToString().Substring(5);
                this.Filter = SqlDataSource1.FilterExpression;
            }
            else
            {
                SqlDataSource1.FilterExpression = String.Empty;
                this.Filter = String.Empty;
            }

            base.btnSearch_Click(sender, e);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataElement1.DataElementID = Int32.Parse(GridView1.SelectedDataKey[0].ToString());
            DataElement1.DataBind();
            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed {0} ID:{1}", DataElement1.DataElementName, DataElement1.DataElementID),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );

            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
        }
    }
}
