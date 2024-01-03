using System;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace MapCall.Reports.Data
{
    public partial class RptStreets : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportViewer1.Visible = true;
            ReportViewer1.Reset();
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.EnableHyperlinks = true;
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_rptStreets", "ObjectDataSource1"));
            ReportViewer1.LocalReport.ReportPath = @"Reports\Data\RptStreets.rdlc";
            ReportViewer1.LocalReport.Refresh();
        }
    }
}
