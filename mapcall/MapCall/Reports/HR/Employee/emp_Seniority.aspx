<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="emp_Seniority.aspx.cs" Inherits="MapCall.Modules.HR.Reports.emp_Seniority" Title="Seniority" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    Local: 
    <asp:DropDownList runat="server" ID="ddlLocal" 
        DataSourceID="dsLocal" 
        AppendDataBoundItems="true"
        DataTextField="Name"
        DataValueField="Id"
        >
        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
    </asp:DropDownList>                    
    
    <asp:SqlDataSource runat="server" ID="dsLocal"
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        SelectCommand="select Name, Id from LocalBargainingUnits order by Name"
        > 
    </asp:SqlDataSource>
    <asp:Button runat="server" ID="btnView" OnClick="btnView_Click" Text="View" />
    <asp:Panel runat="server" ID="pnlReportHolder" Width="100%" Height="480">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="100%" Width="100%" Visible="false">
            <LocalReport  ReportPath="~\Reports\emp_Seniority.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DSHumanResources_emp_Seniority" />   
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    </asp:Panel>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="MapCall.DSHumanResourcesTableAdapters.emp_SeniorityTableAdapter">
        <SelectParameters>
            <asp:Parameter Name="LocalID" DefaultValue="1" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
