<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderFinalizationSearchView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Finalization.WorkOrderFinalizationSearchView" %>
<%@ Register TagPrefix="wo" TagName="BaseWorkOrderSearch" Src="~/Controls/WorkOrders/BaseWorkOrderSearch.ascx" %>

<center>
    <table>
        <wo:BaseWorkOrderSearch runat="server" ID="baseSearch" />
        <tr>
            <td>Purpose:</td>
            <td>
                <mmsinc:MvpListBox runat="server" ID="lstDrivenBy" DataSourceID="odsWorkOrderPurposes"
                    SelectionMode="Multiple"
                    DataTextField="Description" DataValueField="WorkOrderPurposeID" Width="350px"/>
                <asp:ObjectDataSource runat="server" ID="odsWorkOrderPurposes" TypeName="WorkOrders.Model.WorkOrderPurposeRepository"
                    SelectMethod="SelectAllAsList" />
            </td>
        </tr>
        <tr>
            <td>WBS Charged:</td>
            <td><mmsinc:MvpTextBox runat="server" ID="txtWBSCharged"/></td>
        </tr>
        <tr>
            <td colspan="2">
                <center>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" />
                </center>
            </td>
        </tr>
    </table>
</center>