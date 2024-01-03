<%@ Page Title="Work Orders Created/Completed" Language="C#" Theme="bender" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="WorkOrdersCreatedBy.aspx.cs" Inherits="LINQTo271.Views.Reports.WorkOrdersCreatedBy" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
        <mmsinc:MvpLabel runat="server" ID="lblError" ForeColor="Red" />
        <table>
            <tr>
                <th>Start Date:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtDateStart" autocomplete="off" />
                    <atk:CalendarExtender runat="server" TargetControlID="txtDateStart" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvDateStart" 
                    ControlToValidate="txtDateStart" Text="Required" />
                </td>
            </tr>
            <tr>
                <th>End Date:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtDateEnd" autocomplete="off" />
                    <atk:CalendarExtender runat="server" TargetControlID="txtDateEnd" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvDateEnd" 
                    ControlToValidate="txtDateEnd" Text="Required" />
                </td>
            </tr>
            <tr>
                <th>Operating Center:</th>
                <td>
                    <mmsinc:MvpDropDownList runat="server" ID="ddlOperatingCenter" 
                        DataSourceID="odsOperatingCenters" DataTextField="FullDescription"
                        DataValueField="OperatingCenterID" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </mmsinc:MvpDropDownList>
                </td>
            </tr>
            <tr>
                <th>Employee:</th>
                <td>
                    <mmsinc:MvpDropDownList ID="ddlEmployee" runat="server" />
                    <atk:CascadingDropDown runat="server" ID="cddRequestingEmployee" TargetControlID="ddlEmployee"
                        ParentControlID="ddlOperatingCenter" Category="Employee" EmptyText="Select an Employee" EmptyValue=""
                        PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Employees...]"
                        ServicePath="~/Views/Employees/EmployeesServiceView.asmx" ServiceMethod="GetEmployeesByOperatingCenterID"
                        SelectedValue='<%# Bind("RequestingEmployeeID") %>' />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>
    </mmsinc:MvpPanel>
    
    <mmsinc:MvpPanel runat="server" ID="pnlResults" Visible="false">
        <mmsinc:MvpButton runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" OnSorting="gvSearchResults_Sorting"
            OnDataBinding="gvSearchResults_DataBinding" OnRowDataBound="gvSearchResults_RowDataBound" 
            AutoGenerateColumns="false" AllowSorting="false">
            <Columns>
                <mmsinc:MvpBoundField HeaderText="OperatingCenter" DataField="OperatingCenter" />
                <mmsinc:MvpBoundField HeaderText="Employee" DataField="Employee" />
                <mmsinc:MvpBoundField HeaderText="Orders Created" DataField="Created" />
                <mmsinc:MvpBoundField HeaderText="Orders Completed" DataField="Completed" />
            </Columns>
        </mmsinc:MvpGridView>

        <mmsinc:MvpObjectDataSource runat="server" ID="odsEmployees" TypeName="WorkOrders.Model.WorkOrderRepository"
            DataObjectTypeName="WorkOrders.Model.WorkOrder" SelectMethod="GetCreatedByCreatedOnDateCompletedWorkOrderCountsByEmployeeAndDateRange">
        </mmsinc:MvpObjectDataSource>
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Return to Search" OnClick="btnReturnToSearch_Click" />
    </mmsinc:MvpPanel>
    
    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
</asp:Content>
