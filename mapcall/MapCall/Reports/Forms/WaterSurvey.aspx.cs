using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace MapCall.Reports.Forms
{
    public partial class WaterSurvey : Page
    {
    #region Constants

        private const string REC_ID = "RecID";

    #endregion

    #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            string recID = Request.QueryString[REC_ID].ToString();
            if (recID != String.Empty)
            {
                ReportViewer1.Reset();
                ReportViewer1.Visible = false;
                ReportViewer1.LocalReport.DataSources.Clear();
                ObjectDataSource ods;
                ods = new ObjectDataSource(ObjectDataSource2.TypeName, "GetData");
                ods.SelectParameters.Add(new Parameter("RecID", TypeCode.Int32, recID));
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DSReports2_tblNJAWService", ods));

                ReportViewer1.LocalReport.ReportPath = "Reports\\Forms\\WaterSurvey.rdlc";
                ReportViewer1.LocalReport.EnableExternalImages = true;

                ReportParameter p = new ReportParameter("RecID", recID);
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p });
                ReportViewer1.LocalReport.Refresh();

                string DeviceInfo = "<DeviceInfo> <OutputFormat>PDF</OutputFormat>  <PageWidth>8.5in</PageWidth> <PageHeight>11in</PageHeight>  <MarginTop>0in</MarginTop> <MarginLeft>0in</MarginLeft>  <MarginRight>0in</MarginRight> <MarginBottom>0in</MarginBottom> </DeviceInfo>";
                string[] streams;
                string mimeType, encoding, fileNameExtension;
                Warning[] warnings;
                Byte[] renderedBytes;
                renderedBytes = ReportViewer1.LocalReport.Render("PDF", DeviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(renderedBytes);
                Response.End();
            }
        }

    #endregion
    }
}
