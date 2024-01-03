<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" theme="bender" AutoEventWireup="true" CodeBehind="WQComplaints.aspx.cs" Inherits="MapCall.Reports.WaterQuality.WQComplaints" Title="Water Quality Complaints - Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Water Quality Complaints
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    For Year:
    <asp:DropDownList runat="server" ID="ddlYear" DataSourceID="dsYear" 
        AppendDataBoundItems="true" DataTextField="Year" DataValueField="Year" 
        ondatabound="ddlYear_DataBound" />
    <asp:SqlDataSource runat="server" ID="dsYear" 
        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="SELECT DISTINCT DatePart(Year, DateComplaintReceived) AS [Year] FROM WaterQualityComplaints WHERE DateComplaintReceived IS NOT NULL ORDER BY DatePart(Year, DateComplaintReceived)" />
    OpCode:
    <asp:DropDownList runat="server" ID="ddlOpCode" DataSourceID="dsOpCode" AppendDataBoundItems="true" DataTextField="OpCode" DataValueField="OpCode" />
    <asp:SqlDataSource runat="server" ID="dsOpCode"
        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="SELECT OperatingCenterCode AS [OpCode] FROM OperatingCenters ORDER BY 1" />
    <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" Text="View" />
    <asp:Panel runat="server" ID="pnlReportHolder" height="8.5in" width="11in">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" height="8.5in" width="11in" Visible="false">
            <LocalReport ReportPath="~\Reports\WaterQuality\rptWQComplaints.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DSReports2_rptWQComplaintsRollUp" />   
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    </asp:Panel>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
        TypeName="MapCall.DSReports2TableAdapters.rptWQComplaintsRollUpTableAdapter">
        <SelectParameters>
            <asp:Parameter Name="Year" />
            <asp:Parameter Name="OpCode" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
