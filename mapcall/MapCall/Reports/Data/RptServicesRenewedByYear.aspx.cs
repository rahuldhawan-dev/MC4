using System;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace MapCall.Reports.Data
{
    public partial class RptServicesRenewedByYear : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            ObjectDataSource1.SelectParameters["startYear"].DefaultValue = txtStartYear.Text;
            ObjectDataSource1.SelectParameters["endYear"].DefaultValue = txtEndYear.Text;
            ObjectDataSource1.SelectParameters["opCntr"].DefaultValue = ddlOpCntr.SelectedValue;

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_RptServicesRenewedByYear", "ObjectDataSource1"));
            ReportViewer1.LocalReport.ReportPath = @"Reports\Data\RptServicesRenewedByYear.rdlc";

            ReportParameter p1 = new ReportParameter("startYear", txtStartYear.Text);
            ReportParameter p2 = new ReportParameter("endYear", txtEndYear.Text);
            ReportParameter p3 = new ReportParameter("opCntr", ddlOpCntr.SelectedValue);
            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
            ReportViewer1.LocalReport.Refresh();
            ReportViewer1.Visible = true;
        }
    }
}
