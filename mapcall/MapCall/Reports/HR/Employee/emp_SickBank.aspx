<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="emp_SickBank.aspx.cs" Inherits="MapCall.Modules.Reports.emp_SickBank" Title="Sick Bank Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Employee Sick Bank
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Use the form below to search for Employee Sick Bank records.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <table>
        <tr>
            <td style="vertical-align:top;">
                OpCode:
            </td>
            <td>
                <asp:ListBox runat="server" ID="ddlOpCode"
                    SelectionMode="Multiple"
                    Rows="5" 
                    DataSourceID="dsOpCode"
                    AppendDataBoundItems="true"
                    DataTextField="OpCode"
                    DataValueField="OpCode">
                </asp:ListBox>
            </td>
            <td style="vertical-align:top;" nowrap>
                <div>Status: </div>
                <div style="position:relative;top:8px;">Business Unit:</div>
            </td>
            <td style="vertical-align:top;">
                <asp:DropDownList runat="server" ID="ddlStatus">
                    <asp:ListItem Text="--Select Here--" Value="" />
                    <asp:ListItem Text="Active" Value="Active" />
                    <asp:ListItem Text="Inactive" Value="Inactive" />
                </asp:DropDownList>
                <br />
                <asp:DropDownList runat="server" ID="ddlBusinessUnit" 
                    AppendDataBoundItems="true" DataSourceID="dsBusinessUnit"
                    DataTextField="BusinessUnit"
                    DataValueField="BusinessUnitID">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </asp:DropDownList>
                <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" />   
            </td>
        </tr>
    </table>

    
    <asp:SqlDataSource runat="server" ID="dsOpCode"
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        SelectCommand="SELECT DISTINCT oc.OPeratingCenterCode as OpCode FROM tblEmployee e inner join operatingCenters oc on oc.OperatingCenterId = e.OperatingCenterID and isNull(e.OperatingCenterID,0) <> 0 order by 1" />

    <asp:SqlDataSource runat="server" ID="dsBusinessUnit"
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        SelectCommand="
            select 
	            bu.businessUnitID,
	            isNull(operatingCenterCode,'') + ' - ' + isNull(dep.description, '') + ' - ' + isNull(cast(bu.bu as varchar(20)),'') + ' - ' + isNull(bu.description,'') as businessunit
            from 
	            dbo.BusinessUnits bu
            left join Departments dep      on  dep.DepartmentID = bu.DepartmentID  
            left join OperatingCenters oc  on  oc.OperatingCenterID = bu.OperatingCenterID
            order by 
	            operatingCenterCode, dep.Description, bu.Description" />

    <asp:Panel runat="server" ID="pnlReportHolder" Width="100%" Height="480">
        <rsweb:ReportViewer ID="rvEmployeeSickBank" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="100%" Width="100%" Visible="false" >
            <LocalReport ReportPath="~\Reports\rptEmployeeSickBank.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="odsSickBank" Name="DSHumanResources_emp_SickBank" />   
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    </asp:Panel>
    <asp:ObjectDataSource ID="odsSickBank" 
         runat="server" SelectMethod="GetData" TypeName="MapCall.DSHumanResourcesTableAdapters.emp_SickBankTableAdapter">
        <SelectParameters>
            <asp:Parameter Name="OpCode" Type="String" />
            <asp:Parameter Name="status" Type="String" />
            <asp:Parameter Name="businessUnitID" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>