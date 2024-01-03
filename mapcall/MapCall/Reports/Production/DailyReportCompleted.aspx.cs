using System;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace MapCall.Reports.Production
{
    public partial class DailyReportCompleted : Page
    {
        private string _startDate;
        private string _endDate;
        private string _opCode;
        private string _facilityID;
        private string _assignedTo;
        private string _WorkOrderAssignedContractorName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _startDate = Request.QueryString["startDate"];
                _endDate = Request.QueryString["endDate"];
                txtStartDate.Text = _startDate;
                txtEndDate.Text = _endDate;
            }
            else
            {
                _startDate = txtStartDate.Text;
                _endDate = txtEndDate.Text;
            }
            _opCode = Request.QueryString["opCode"];
            _facilityID = Request.QueryString["facilityID"];
            _assignedTo = Request.QueryString["assignedTo"];
            _WorkOrderAssignedContractorName = Request.QueryString["WorkOrderAssignedContractorName"];
            ExecReport();

        }
        private void ExecReport()
        {
            ObjectDataSource1.SelectParameters["startDate"].DefaultValue = _startDate;
            ObjectDataSource1.SelectParameters["endDate"].DefaultValue = _endDate;

            if (!String.IsNullOrEmpty(_opCode))
                ObjectDataSource1.FilterExpression += String.Format("AND opCode = '{0}'", _opCode);
            if (!String.IsNullOrEmpty(_facilityID))
                ObjectDataSource1.FilterExpression += String.Format("AND FacilityID = '{0}'", _facilityID);
            if (!String.IsNullOrEmpty(_assignedTo))
                ObjectDataSource1.FilterExpression += String.Format("AND assignedTo = '{0}'", _assignedTo);
            if (!String.IsNullOrEmpty(_WorkOrderAssignedContractorName))
                ObjectDataSource1.FilterExpression += String.Format("AND WorkOrderAssignedContractorName = '{0}'", _WorkOrderAssignedContractorName);
            if (ObjectDataSource1.FilterExpression.Length > 4)
                ObjectDataSource1.FilterExpression = ObjectDataSource1.FilterExpression.Substring(4);

            ReportViewer1.Reset();
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DSProduction_rptProd_DailyReportCompleted", "ObjectDataSource1"));
            ReportViewer1.LocalReport.ReportPath = @"Reports\Production\DailyReportCompleted.rdlc";

            ReportParameter p1 = new ReportParameter("startDate", _startDate);
            ReportParameter p2 = new ReportParameter("endDate", _endDate);
            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });
            ReportViewer1.LocalReport.Refresh();
            ReportViewer1.Visible = true;
        }
    }
}
