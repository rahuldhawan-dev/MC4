<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SOPProcessingSearchView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.SOPProcessing.SOPProcessingSearchView" %>
<%@ Register TagPrefix="wo" TagName="BaseWorkOrderSearch" Src="~/Controls/WorkOrders/BaseWorkOrderSearch.ascx" %>

<center>
    <table>
        <wo:BaseWorkOrderSearch runat="server" ID="baseSearch" />
        <asp:BoundField DataField="Priority" HeaderText="Job Priority" SortExpression="Priority.Description" />
        <tr>
            <td>Job Priority:</td>
            <td class="control">
                <mmsinc:MvpDropDownList runat="server" ID="ddlPriority" DataSourceID="odsPriority"
                                        DataTextField="Description" DataValueField="WorkOrderPriorityID" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>

            </td>
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
    
    <asp:ObjectDataSource runat="server" ID="odsPriority" TypeName="WorkOrders.Model.WorkOrderPriorityRepository"
                          SelectMethod="SelectAllAsList" />
</center>