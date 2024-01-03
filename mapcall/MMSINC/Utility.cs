using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC
{
    public static class Utility
    {
        #region Exposed Static Methods

        // NOTE: This should be used sparingly. Iterating through child controls on some databound controls
        //       will force databinding to occur! -Ross 6/29/2011

        public static Control GetFirstControlInstance(Control control, string controlID)
        {
            if (control.ID == controlID)
            {
                return control;
            }
            foreach (Control c in control.Controls)
            {
                if (c.ID == null && c.Controls.Count == 0)
                {
                    // 2008-04-23 by Jason Duncan
                    // no need to consider controls with no id
                    // and no children
                    continue;
                }
                Control t = GetFirstControlInstance(c, controlID);
                if (t != null)
                {
                    return t;
                }
            }
            return null;
        }
        public static Control GetFirstControlInstanceIn(Control control, string controlID, string parentID)
        {
            if (control.ID == controlID && control.Parent.ID == parentID)
            {
                return control;
            }
            foreach (Control c in control.Controls)
            {
                Control t = GetFirstControlInstanceIn(c, controlID, parentID);
                if (t != null)
                {
                    return t;
                }
            }
            return null;
        }


        public static void ExportToExcel(Page Page, SqlDataSource sds)
        {
            Page.Response.Clear();
            Page.Response.AddHeader("content-disposition", "attachment;filename=Data.xls");
            Page.Response.Charset = "";
            Page.Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlwriter = new HtmlTextWriter(sw);
            DataGrid dg = new DataGrid();
            dg.DataSource = sds;
            dg.DataBind();
            dg.RenderControl(htmlwriter);
            Page.Response.Write(sw.ToString());
            Page.Response.End();
        }

        #endregion
    }


}