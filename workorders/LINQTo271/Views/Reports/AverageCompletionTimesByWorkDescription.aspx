<%@ Page Title="Average Completion Times by Work Description" Language="C#" Theme="bender" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="AverageCompletionTimesByWorkDescription.aspx.cs" Inherits="LINQTo271.Views.Reports.AverageCompletionTimesByWorkDescription" %>
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
                        <asp:ListItem Text="--Select Here--" Value="0" />
                    </mmsinc:MvpDropDownList>
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
                <mmsinc:MvpBoundField HeaderText="Operating Center" DataField="OpCntr" />
                <mmsinc:MvpBoundField HeaderText="Work Description" DataField="WorkDescription" />
                <mmsinc:MvpBoundField HeaderText="Average Life of Order (hrs)" DataFormatString="{0:0.00}" DataField="Completion" />
                <mmsinc:MvpBoundField HeaderText="Average Man Hours To Complete (hrs)" DataFormatString="{0:0.00}" DataField="ManHours" />
                <mmsinc:MvpBoundField HeaderText="Average time to Approve (hrs)" DataFormatString="{0:0.00}" DataField="Approval" />
                <mmsinc:MvpBoundField HeaderText="Average time to issue stock (hrs)" DataFormatString="{0:0.00}" DataField="StockApproval" />
                <mmsinc:MvpBoundField HeaderText="Order Count" DataField="Count" />
                <mmsinc:MvpBoundField HeaderText="Average Crew Size" DataFormatString="{0:0.00}" DataField="CrewAverage" />
            </Columns>
        </mmsinc:MvpGridView>

        <mmsinc:MvpObjectDataSource runat="server" ID="odsEmployees" TypeName="WorkOrders.Model.CrewAssignmentRepository"
            DataObjectTypeName="WorkOrders.Model.WorkOrder" SelectMethod="GetWorkOrderTimeAverages">
        </mmsinc:MvpObjectDataSource>
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Return to Search" OnClick="btnReturnToSearch_Click" />
    </mmsinc:MvpPanel>
    
    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
</asp:Content>
