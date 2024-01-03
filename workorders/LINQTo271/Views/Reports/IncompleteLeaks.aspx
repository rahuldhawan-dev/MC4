<%@ Page Title="Incomplete Orders - Leaks" Language="C#" Theme="bender" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="IncompleteLeaks.aspx.cs" Inherits="LINQTo271.Views.Reports.IncompleteLeaks" %>
<%@ Register TagPrefix="mmsinc" Namespace="MMSINC.Controls" Assembly="MMSINC.Core.WebForms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphInstructions" runat="server">
    Use this form to display incomplete Work Orders regarding leaks.
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
        <mmsinc:MvpLabel runat="server" ID="lblError" ForeColor="Red" />
        <table>
            <tr>
                <th>Operating Center:</th>
                <td>
                    <mmsinc:MvpDropDownList runat="server" ID="ddlOperatingCenter" DataSourceID="odsOperatingCenters"
                        DataTextField="FullDescription" DataValueField="OperatingCenterID" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
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
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Back to Search" OnClick="btnReturnToSearch_Click" />
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" /> <br />
        
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" AutoGenerateColumns="false" AllowSorting="false"
            OnDataBinding="gvSearchResults_DataBinding">
            <Columns>
                <asp:HyperLinkField HeaderText="Order Number" DataTextField="WorkOrderID" DataNavigateUrlFields="WorkOrderID" DataNavigateUrlFormatString="../WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg={0}" Target="_blank" />                
                <asp:BoundField HeaderText="Town" DataField="Town" SortExpression="Town" />
                <asp:TemplateField HeaderText="Address">
                    <ItemTemplate>
                        <%# Eval("StreetNumber") %> <%# Eval("Street") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description of Job<br/>(Hover for Notes)"
                    SortExpression="TownID">
                    <ItemTemplate>
                        <asp:Label runat="server" Title='<%# Eval("Notes") ?? "No Notes Entered" %>' Text='<%# Eval("WorkDescription") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DateReceived" HeaderText="Date Received" SortExpression="DateReceived" DataFormatString="{0:d}"/>
                <asp:BoundField DataField="Priority" HeaderText="Job Priority" SortExpression="Priority.Description" />
                <asp:BoundField DataField="DrivenBy" HeaderText="Purpose" SortExpression="DrivenBy.Description" />
                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy.Description" />
            </Columns>
        </mmsinc:MvpGridView>
    </mmsinc:MvpPanel>

    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
</asp:Content>
