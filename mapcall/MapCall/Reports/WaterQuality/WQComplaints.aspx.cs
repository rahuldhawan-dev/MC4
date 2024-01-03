using System;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace MapCall.Reports.WaterQuality
{
    public partial class WQComplaints : Page
    {
        #region Constants

        private struct ReportParameterNames
        {
            public const string YEAR = "Year";
            public const string OPCODE = "OpCode";
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            ObjectDataSource1.SelectParameters[ReportParameterNames.YEAR].DefaultValue = ddlYear.SelectedValue;
            ObjectDataSource1.SelectParameters[ReportParameterNames.OPCODE].DefaultValue = ddlOpCode.SelectedValue;

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DSReports2_rptWQComplaintsRollUp", "ObjectDataSource1"));
            ReportViewer1.LocalReport.ReportPath = "Reports\\WaterQuality\\rptWQComplaintsRollUp.rdlc";

            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] {
                new ReportParameter(ReportParameterNames.YEAR, ddlYear.SelectedValue),
                new ReportParameter(ReportParameterNames.OPCODE, ddlOpCode.SelectedValue)
            });
            ReportViewer1.LocalReport.Refresh();
            ReportViewer1.Visible = true;
        }

        protected void ddlYear_DataBound(object sender, EventArgs e)
        {
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        #endregion
    }
}
