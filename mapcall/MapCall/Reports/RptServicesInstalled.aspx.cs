using System;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace MapCall.Reports
{
    public partial class RptServicesInstalled : Page
    {
        protected void Button1_Click(object sender, EventArgs e)
        {
            ObjectDataSource2.SelectParameters["year"].DefaultValue = ddlYear.SelectedItem.Text;

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_getRptServicesInstalled2", "ObjectDataSource2"));
            ReportViewer1.LocalReport.ReportPath = "Reports\\RptServicesInstalled2.rdlc";

            var p = new ReportParameter("year", ddlYear.SelectedItem.Text);
            ReportViewer1.LocalReport.SetParameters(new [] { p });
            ReportViewer1.LocalReport.Refresh();
            ReportViewer1.Visible = true;
        }
    }
}
