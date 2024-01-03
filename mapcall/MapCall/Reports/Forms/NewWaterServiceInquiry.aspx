<%@ Page Language="C#" MasterPageFile="~/MapCall.Master" AutoEventWireup="true" CodeBehind="NewWaterServiceInquiry.aspx.cs" Inherits="MapCall.Reports.Forms.NewWaterServiceInquiry" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt" height="11.25in" width="8.75in">
        <LocalReport EnableExternalImages="True" ReportPath="Reports\Forms\NewWaterServiceInquiry.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DataSet1_RptFormNewWaterServiceInquiry" />
            </DataSources>
        </LocalReport>
    </rsweb:reportviewer>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetData"
        TypeName="MapCall.DataSet1TableAdapters.RptFormNewWaterServiceInquiryTableAdapter">
        <SelectParameters>
            <asp:Parameter Name="RecID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
