using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MMSINC.Page;

namespace MapCall.Reports
{
    public abstract class ReportPage : DataElementRolePage
    {
        #region Constants

        private const string RESULTS = "Total Records: {0}";

        #endregion
        
        #region Properties

        #region Abstract Properties

        /// <summary>
        ///  This must point to the main gridview of the page with the reports results.
        /// </summary>
        public abstract GridView GridView { get; }

        public abstract SqlDataSource DataSource { get; }
        
        public abstract Panel SearchPanel { get; }
        
        public abstract Panel ResultsPanel { get; }
        
        public abstract Label InformationLabel { get; }

        public abstract Label PermissionLabel { get; }

        #endregion
        
        #endregion

        #region Private Methods

        private void ApplyFilter()
        {
            if (String.IsNullOrEmpty(DataSource.FilterExpression) && !String.IsNullOrEmpty(Filter))
                DataSource.FilterExpression = Filter;
        }

        #endregion

        #region Events/Delegates

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!CanView)
            {
                SearchPanel.Visible = false;
                PermissionLabel.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module);
            }

            if (IsPostBack)
            {
                ApplyFilter();
            }
        }

        #endregion

        #region Exposed Methods

        // This needs to be overridden to get the excel export to work properly
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }

        public virtual string CreateFilter()
        {
            var sb = new StringBuilder();
            foreach (Control ctrl in SearchPanel.Controls)
            {
                if (ctrl is IDataField)
                    sb.Append(((IDataField)ctrl).FilterExpression());
            }      
            return (sb.Length > 0) ? sb.ToString().Substring(5) : String.Empty;
        }

        #endregion
        
        #region Event Handlers

        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            SearchPanel.Visible = true;
            ResultsPanel.Visible = false;
        }

        /// <summary>
        /// This will export the GridView to an excel file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnExport_Click(object sender, EventArgs e)
        {
            ApplyFilter();
            GridView.DataBind();
            Page.VerifyRenderingInServerForm(GridView);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Data.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            var sw = new StringWriter();
            var htmlwriter = new HtmlTextWriter(sw);
            GridView.RenderControl(htmlwriter);
            Response.Write(sw.ToString());
            Response.End();
        }

        protected virtual void btnSearch_Click(object sender, EventArgs e)
        {
            // This is replacing AND here, because the method is often overloaded 
            // in inheriting pages. 
            Filter = CreateFilter().TrimStart(" AND".ToCharArray());
            DataSource.FilterExpression = Filter;

            SearchPanel.Visible = false;
            ResultsPanel.Visible = true;
            GridView.DataBind();
            InformationLabel.Text = String.Format(RESULTS, GridView.Rows.Count);
        }

        #endregion
    }
}
