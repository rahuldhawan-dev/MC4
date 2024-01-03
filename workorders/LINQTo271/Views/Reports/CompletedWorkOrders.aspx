<%@ Page Title="Completed Orders" Language="C#" Theme="bender" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="CompletedWorkOrders.aspx.cs" Inherits="LINQTo271.Views.Reports.CompletedWorkOrders" %>
<%@ Import Namespace="System.Linq"%>
<%@ Import Namespace="WorkOrders.Model"%>
<%@ Import Namespace="WorkOrders.Model" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="mmsinc" TagName="DateRange" Src="~/Common/DateRange.ascx" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
        <mmsinc:MvpLabel runat="server" ID="lblError" ForeColor="Red" />
        <table>
            <tr>
                <th>Date Completed:</th>
                <td>
                    <mmsinc:DateRange runat="server" ID="drCompletedDate" />
                </td>
            </tr>
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
                <th>Town:</th>
                <td>
                    <mmsinc:MvpDropDownList ID="ddlTown" runat="server" />
                    <atk:CascadingDropDown runat="server" ID="cddTowns" TargetControlID="ddlTown" ParentControlID="ddlOperatingCenter"
                        Category="Town" EmptyText="None Found" EmptyValue="" PromptText="--Select Here--"
                        PromptValue="" ServicePath="~/Views/Towns/TownsServiceView.asmx" ServiceMethod="GetTownsByOperatingCenterID" />
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
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Back to Search" OnClick="btnReturnToSearch_Click" /> <br />
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" AutoGenerateColumns="false" AllowSorting="true"
            OnDataBinding="gvSearchResults_DataBinding" OnSorting="gvSearchResults_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Work Order Number">
                    <ItemTemplate>
                        <a href="../WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%# Eval("WorkOrderID") %>">
                            <%# Eval("WorkOrderID") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Town" DataField="Town" SortExpression="Town.Name" />
                <asp:BoundField HeaderText="SAP WO" DataField="SAPWorkOrderNumber" SortExpression="SAPWorkOrderNumber" />
                <asp:BoundField HeaderText="SAP Notif" DataField="SAPNotificationNumber" SortExpression="SAPNotificationNumber" />
                <asp:TemplateField HeaderText="Address">
                    <ItemTemplate>
                        <%# Eval("StreetNumber") %> <%# Eval("Street") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description&nbsp;of&nbsp;Job<br/>(Hover for Notes)"
                    SortExpression="TownID">
                    <ItemTemplate>
                        <asp:Label runat="server" Title='<%# Eval("Notes") ?? "No Notes Entered" %>' Text='<%# Eval("WorkDescription") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AssetType" HeaderText="Asset Type" SortExpression="AssetType.Description" />
                <asp:BoundField DataField="AssetID" HeaderText="Asset ID" SortExpression="AssetID" />
                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                <asp:BoundField DataField="CreatedOn" HeaderText="Date Created" SortExpression="CreatedOn" />
                <asp:BoundField DataField="DateReceived" HeaderText="Date Received" SortExpression="DateReceived" />
                <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" SortExpression="RequestedBy" />
                <asp:BoundField DataField="RequestingEmployee" HeaderText="Requesting Employee" NullDisplayText="n/a" />
                <asp:BoundField DataField="CompletedBy" HeaderText="Completed By" NullDisplayText="n/a" />
                <asp:BoundField DataField="DateCompleted" HeaderText="Date Completed" SortExpression="DateCompleted" NullDisplayText="n/a" />
                <asp:TemplateField HeaderText="Order Process Time">
                    <ItemTemplate>
                        <%# Eval("ProcessTime") == null ? "n/a" : String.Format("{0} days, {1}:{2}", Eval("ProcessTime.Days"), Eval("ProcessTime.Hours"), Eval("ProcessTime.Minutes")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ApprovedBy" HeaderText="Approved By" SortExpression="ApprovedBy.FullName" NullDisplayText="n/a" />
                <asp:BoundField DataField="ApprovedOn" HeaderText="Date Approved" SortExpression="ApprovedOn"
                    NullDisplayText="n/a" />
                <asp:TemplateField HeaderText="Supervisor Process Time">
                    <ItemTemplate>
                        <%# Eval("SupervisorProcessTime") == null ? "n/a" : String.Format("{0} days, {1}:{2}", Eval("SupervisorProcessTime.Days"), Eval("SupervisorProcessTime.Hours"), Eval("SupervisorProcessTime.Minutes")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Materials">
                    <ItemTemplate>
                        <%# ((IEnumerable<MaterialsUsed>)Eval("MaterialsUseds")).Any() ? "Yes" : "No" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MaterialsApprovedBy" HeaderText="Stock Approved By" SortExpression="MaterialsApprovedBy" NullDisplayText="n/a" />
                <asp:BoundField DataField="MaterialsApprovedOn" HeaderText="Stock Approval Date"
                    SortExpression="MaterialsApprovedOn" NullDisplayText="n/a" />
                <asp:TemplateField HeaderText="Stock Process Time">
                    <ItemTemplate>
                        <%# Eval("StockProcessTime") == null ? "n/a" : String.Format("{0} days, {1}:{2}", Eval("StockProcessTime.Days"), Eval("StockProcessTime.Hours"), Eval("StockProcessTime.Minutes")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Crew" SortExpression="CurrentlyAssignedCrew.Description">
                    <ItemTemplate>
                        <%# Eval("CurrentlyAssignedCrew.Description") %>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:BoundField HeaderText="Account Charged" DataField="AccountCharged" SortExpression="AccountCharged" />
                <asp:TemplateField HeaderText="Accounting Type" SortExpression="WorkDescription.AccountingType">
                    <ItemTemplate><%# Eval("WorkDescription.AccountingType")%></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Notes" DataField="Notes" />
            </Columns>
        </mmsinc:MvpGridView>
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" />
    </mmsinc:MvpPanel>

    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
</asp:Content>
