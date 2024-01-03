<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="SpoilsPerWorkOrder.aspx.cs" Inherits="LINQTo271.Views.Reports.SpoilsPerWorkOrder" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="mmsinc" Assembly="MMSINC.Core" Namespace="MMSINC.Controls" %>
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
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return validateSearch()" />
                </td>
            </tr>
        </table>

        <script type="text/javascript">
            function validateSearch() {
                var ddlOperatingCenter = getServerElementById('ddlOperatingCenter');
                if (!ddlOperatingCenter.val()) {
                    alert('Please choose an Operating Center.');
                    ddlOperatingCenter.focus();
                    return false;
                }
                return true;
            }
        </script>
    </mmsinc:MvpPanel>

    <mmsinc:MvpPanel runat="server" ID="pnlResults" Visible="false">
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Back to Search" OnClick="btnReturnToSearch_Click" />
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" /><br />
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" AutoGenerateColumns="false" AllowSorting="true"
            OnSorting="gvSearchResults_Sorting">
            <Columns>
                <asp:BoundField HeaderText="Work Order Number" DataField="WorkOrderID" SortExpression="WorkOrderID" />
                <asp:TemplateField HeaderText="Date Completed" SortExpression="WorkOrder.DateCompleted">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.DateCompleted") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Town" SortExpression="WorkOrder.Town.Name">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.Town") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Street Number" SortExpression="WorkOrder.StreetNumber">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.StreetNumber") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Street" SortExpression="WorkOrder.Street.FullStName">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.Street") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Work Description" SortExpression="WorkOrder.WorkDescription.Description">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.WorkDescription") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Quantity" DataField="Quantity" SortExpression="Quantity" />
                <asp:BoundField HeaderText="Storage Location" DataField="SpoilStorageLocation" SortExpression="SpoilStorageLocation.Description" />
                <%-- not sure about quantity/storage location yet --%>
                <asp:TemplateField HeaderText="Account Charged" SortExpression="WorkOrder.AccountCharged">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.AccountCharged") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Accounting Type" SortExpression="WorkOrder.WorkDescription.AccountingType.Description">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.WorkDescription.AccountingType") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Crew Assigned">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.CurrentlyAssignedCrew") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </mmsinc:MvpGridView>
    </mmsinc:MvpPanel>

    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
</asp:Content>
