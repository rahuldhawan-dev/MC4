<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="TrafficControlRequirement.aspx.cs" Inherits="LINQTo271.Views.Reports.TrafficControlRequirement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="mmsinc" TagName="DateRange" Src="~/Common/DateRange.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
    <asp:Label runat="server" ID="lblError" style="color: red" />
    <table>
        <tr>
            <th>Operating Center:</th>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlOpCode" DataSourceID="odsOperatingCenters"
                    DataTextField="FullDescription" DataValueField="OperatingCenterID" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <th>Town:</th>
            <td>
                <mmsinc:MvpDropDownList ID="ddlTown" runat="server" />
                <atk:CascadingDropDown runat="server" ID="cddTowns" TargetControlID="ddlTown" ParentControlID="ddlOpCode"
                    Category="Town" EmptyText="None Found" EmptyValue="" PromptText="--Select Here--"
                    PromptValue="" ServicePath="~/Views/Towns/TownsServiceView.asmx" ServiceMethod="GetTownsByOperatingCenterID" />
            </td>
        </tr>
        <tr>
            <th>Date Completed:</th>
            <td>
                <mmsinc:DateRange runat="server" ID="drCompletedDate" />
            </td>
        </tr>
        <tr>
            <th>Traffic Control Required:</th>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlTrafficControl">
                    <asp:ListItem Text="" Value="" />
                    <asp:ListItem Text="yes" Value="true" />
                    <asp:ListItem Text="no" Value="false" />
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
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export" OnClick="btnExport_Click" />
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Back to Search" OnClick="btnReturnToSearch_Click" /> <br />
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" AutoGenerateColumns="false" AllowSorting="true"
            OnSorting="gvSearchResults_Sorting">
            <Columns>
                <asp:HyperLinkField HeaderText="Work Order Number" DataTextField="WorkOrderID" DataNavigateUrlFields="WorkOrderID" DataNavigateUrlFormatString="../WorkOrders/ReadOnly/WorkOrderReadOnlyRPCPage.aspx?cmd=view&arg={0}" Target="_blank" SortExpression="WorkOrderID" />
                <asp:BoundField HeaderText="Date Completed" DataField="DateCompleted" SortExpression="DateCompleted" />
                <asp:BoundField HeaderText="Town" DataField="Town" SortExpression="Town.Name" />
                <asp:BoundField HeaderText="Street Number" DataField="StreetNumber" SortExpression="StreetNumber" />
                <asp:BoundField HeaderText="Street" DataField="Street" SortExpression="Street.FullStName " />
                <asp:BoundField HeaderText="Nearest Cross Street" DataField="NearestCrossStreet" SortExpression="NearestCrossStreet.FullStName" />
                <asp:TemplateField HeaderText="Work Description" SortExpression="WorkDescription.Description">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkDescription") %>' ToolTip='<%# Eval("Notes") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Account Charged" DataField="AccountCharged" SortExpression="AccountCharged" />
                <asp:TemplateField HeaderText="Accounting Type" SortExpression="WorkDescription.AccountingType.Description">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkDescription.AccountingType") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Crew Assigned" DataField="CurrentlyAssignedCrew" SortExpression="CurrentlyAssignedCrew.Description" />
                <asp:TemplateField HeaderText="Traffic Control Required?" SortExpression="TrafficControlRequired">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# ((bool)Eval("TrafficControlRequired") ? "yes" : "no") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Number of Officers Required" DataField="NumberOfOfficersRequired" SortExpression="NumberOfOfficersRequired" />
            </Columns>
        </mmsinc:MvpGridView>
        
    </mmsinc:MvpPanel>

    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
</asp:Content>
