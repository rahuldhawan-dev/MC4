<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="emp_Licenses.aspx.cs" Inherits="MapCall.Modules.Reports.emp_Licenses" Title="Employee Licenses Report" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    OpCode:
    <asp:DropDownList runat="server" ID="ddlOpCode"
        DataSourceID="dsOpCode"
        AppendDataBoundItems="true"
        DataTextField="OpCode"
        DataValueField="OpCode">
        <asp:ListItem Text="--Select Here--" />
    </asp:DropDownList>
    <asp:SqlDataSource runat="server" ID="dsOpCode"
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        SelectCommand="SELECT DISTINCT oc.OPeratingCenterCode as OpCode FROM tblEmployee e inner join operatingCenters oc on oc.OperatingCenterId = e.OperatingCenterID and isNull(e.OperatingCenterID,0) <> 0 order by 1" />
    <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" />
    <asp:Panel runat="server" ID="pnlReportHolder" Width="100%" Height="480">
        <rsweb:ReportViewer ID="rvEmployeeLicenses" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="100%" Width="100%" Visible="false">
            <LocalReport ReportPath="~\Reports\rptEmployeeLicenses.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="odsLicenses" Name="DSHumanResources_emp_SickBank" />   
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    </asp:Panel>
    <asp:ObjectDataSource ID="odsLicenses" runat="server" SelectMethod="GetData" TypeName="MapCall.DSHumanResourcesTableAdapters.emp_LicensesTableAdapter">
        <SelectParameters>
            <asp:Parameter Name="OpCode" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>