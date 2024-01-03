using System;
using System.Text;
using MMSINC.Page;
using MapCall.Common.ClassExtensions;
using MapCall.Controls.Data;

namespace MapCall.Modules.Utility
{
    public partial class Timeout : DataElementPage
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.IsAdmin())
            {
                Response.Write("Access Denied [Administrator]");
                Response.End();
            }
        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            foreach (var ctrl in pnlSearch.Controls)
            {
                if (ctrl is DataField)
                    sb.Append(((DataField)ctrl).FilterExpression());
            }

            SqlDataSource1.FilterExpression = (sb.Length > 0) ? sb.ToString().Substring(5) : String.Empty;
            Filter = SqlDataSource1.FilterExpression;

            base.btnSearch_Click(sender, e);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataElement1.DataElementID = Int32.Parse(GridView1.SelectedDataKey[0].ToString());
            DataElement1.DataBind();

            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
        }
    }
}
