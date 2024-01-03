using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace MapCall.Reports.Forms
{
    public partial class WaterServiceInquiryAndSurvey : Page
    {
    #region Constants

        private struct QueryStringKeys
        {
            public const string REC_ID = "RecID";
        }

    #endregion

    #region Private Members

        private int? m_iRecID;

    #endregion

    #region Properties

        protected int RecID
        {
            get
            {
                if (m_iRecID == null)
                {
                    object obj = Request.QueryString[QueryStringKeys.REC_ID];
                    m_iRecID = (obj == null) ? -1 : Int32.Parse(obj.ToString());
                }
                return (int)m_iRecID;
            }
        }

    #endregion

    #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] {
                new ReportParameter(QueryStringKeys.REC_ID, RecID.ToString())
            });

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

        protected void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            ObjectDataSource2.SelectParameters.Clear();
            ObjectDataSource3.SelectParameters.Clear();
            for (int x = 0; x < e.Parameters.Count; x++)
            {
                Parameter p = new Parameter();
                p.Name = e.Parameters[x].Name;
                p.DefaultValue = e.Parameters[x].Values[0].ToString();
                ObjectDataSource2.SelectParameters.Add(p);
                ObjectDataSource3.SelectParameters.Add(p);
            }
            e.DataSources.Add(new ReportDataSource("DataSet1_RptFormNewWaterServiceInquiry", "ObjectDataSource2"));
            e.DataSources.Add(new ReportDataSource("DSReports2_tblNJAWService", "ObjectDataSource3"));
        }

    #endregion
    }
}
