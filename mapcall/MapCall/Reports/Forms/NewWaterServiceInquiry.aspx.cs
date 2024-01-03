using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace MapCall.Reports.Forms
{
    public partial class NewWaterServiceInquiry : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var recID = Request.QueryString["RecID"];
            if (string.IsNullOrWhiteSpace(recID)) return;
            
            ReportViewer1.Reset();
            ReportViewer1.Visible = false;
            ReportViewer1.LocalReport.DataSources.Clear();
            ObjectDataSource ods;
            ods = new ObjectDataSource(ObjectDataSource2.TypeName, "GetData");
            ods.SelectParameters.Add(new Parameter("RecID", TypeCode.Int32, recID));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_RptFormNewWaterServiceInquiry", ods));

            ReportViewer1.LocalReport.ReportPath = "Reports\\Forms\\NewWaterServiceInquiry.rdlc";
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
            //Response.AddHeader("content-disposition"
            //Response.AddHeader("content-disposition", "attachment; filename=nwsi.pdf");
            Response.BinaryWrite(renderedBytes);
            Response.End();
        }
    }
}
