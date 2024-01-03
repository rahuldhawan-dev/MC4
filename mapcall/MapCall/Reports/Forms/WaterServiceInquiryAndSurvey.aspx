<%@ Page Language="C#" MasterPageFile="~/MapCall.Master" AutoEventWireup="true" CodeBehind="WaterServiceInquiryAndSurvey.aspx.cs" Inherits="MapCall.Reports.Forms.WaterServiceInquiryAndSurvey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt"
        height="11.25in" width="8.75in">
<LocalReport EnableExternalImages="True" ReportPath="Reports\Forms\WaterServiceInquiryAndSurvey.rdlc" OnSubreportProcessing="LocalReport_SubreportProcessing">
    <DataSources>
        <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DataSet1_RptFormNewWaterServiceInquiry" />
        <rsweb:ReportDataSource DataSourceId="ObjectDataSource3" Name="DSReports2_tblNJAWService" />
    </DataSources>
</LocalReport>
</rsweb:reportviewer>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetData"
        TypeName="MapCall.DataSet1TableAdapters.RptFormNewWaterServiceInquiryTableAdapter">
        <SelectParameters>
            <asp:Parameter Name="RecID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="GetData"
        TypeName="MapCall.DSReports2TableAdapters.tblNJAWServiceTableAdapter">
        <SelectParameters>
            <asp:Parameter Name="RecID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
