<%@ Page Language="C#" MasterPageFile="~/MapCall.Master" AutoEventWireup="true" CodeBehind="WaterSurvey.aspx.cs" Inherits="MapCall.Reports.Forms.WaterSurvey" Title="Water Survey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt"
        height="11.25in" width="8.75in">
        <LocalReport EnableExternalImages="True" 
            ReportPath="Reports\Forms\WaterSurvey.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DSReports2_tblNJAWService" />
            </DataSources>
        </LocalReport>
    </rsweb:reportviewer>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetData"
        TypeName="MapCall.DSReports2TableAdapters.tblNJAWServiceTableAdapter"
        OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:Parameter Name="RecID" Type="Int32" Size="4" DefaultValue="1" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
