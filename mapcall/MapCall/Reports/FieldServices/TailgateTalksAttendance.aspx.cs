using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MMSINC.Page;
using MapCall.Controls.Data;

namespace MapCall.Reports.FieldServices
{
    public partial class TailgateTalksAttendance : DataElementPage
    {
        #region Event Handlers

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // utter crap.
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dsAttendees =
                    (SqlDataSource)e.Row.FindControl("dsAttendees");
                dsAttendees.SelectParameters[0].DefaultValue =
                    GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
            }
        }
        
        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (Object ctrl in pnlSearch.Controls)
                if (ctrl is DataField)
                    sb.Append(((IDataField)ctrl).FilterExpression());

            if (sb.Length > 0)
            {
                SqlDataSource1.FilterExpression = sb.ToString().Substring(5);
                Filter = SqlDataSource1.FilterExpression;
                hidFilter.Value = Filter;
            }
            else
            {
                SqlDataSource1.FilterExpression = String.Empty;
                Filter = String.Empty;
                hidFilter.Value = Filter;
            }

            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}", GridView1.Rows.Count);
        }

        protected override void btnExport_Click(object sender, EventArgs e)
        {
            Page.VerifyRenderingInServerForm(GridView1);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Data.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            var sw = new StringWriter();
            var htmlwriter = new HtmlTextWriter(sw);
            GridView1.RenderControl(htmlwriter);
            Response.Write(sw.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        #endregion
    }
}
