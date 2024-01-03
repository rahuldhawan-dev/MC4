<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderFinalizationListView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Finalization.WorkOrderFinalizationListView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderTableLegend" Src="~/Controls/WorkOrders/WorkOrderTableLegend.ascx" %>

<wo:WorkOrderTableLegend runat="server"></wo:WorkOrderTableLegend>

<asp:Label runat="server" ID="lblCount" ></asp:Label>
<mmsinc:MvpGridView runat="server" ID="gvWorkOrders" AutoGenerateColumns="false"
    OnSelectedIndexChanged="ListControl_SelectedIndexChanged" AutoGenerateSelectButton="false"
    DataKeyNames="WorkOrderID" AllowSorting="true" OnSorting="ListControl_Sorting" OnRowDataBound="gvWorkOrders_RowDataBound">
    <Columns>
        <asp:HyperLinkField HeaderText="" Text="Select" DataNavigateUrlFields="WorkOrderId" DataNavigateUrlFormatString="WorkOrderFinalizationResourceRPCPage.aspx?cmd=update&arg={0}" />
        <asp:BoundField DataField="WorkOrderID" HeaderText="Order Number" SortExpression="WorkOrderID" />
        <asp:BoundField DataField="StreetNumber" HeaderText="Street Number" SortExpression="StreetNumber" />
        <asp:BoundField DataField="Street" HeaderText="Street" SortExpression="Street.StreetName" />
        <asp:BoundField DataField="NearestCrossStreet" HeaderText="Nearest Cross Street" SortExpression="NearestCrossStreet.StreetName" />
        <asp:BoundField DataField="Town" HeaderText="Town" SortExpression="Town.Name" />
        <asp:BoundField DataField="TownSection" HeaderText="Town Section" SortExpression="TownSection.Name" />
        <asp:TemplateField SortExpression="WorkDescription.Description" HeaderText="Description of Job<br/>(Hover for Notes)">        
            <ItemTemplate >
                <asp:Label runat="server" Title='<%# Eval("Notes") ?? "No Notes Entered" %>' Text='<%# Eval("WorkDescription") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>        
        <asp:BoundField DataField="CurrentlyAssignedCrew" HeaderText="Last Assigned To" SortExpression="CurrentlyAssignedCrew" />
        <asp:BoundField DataField="DrivenBy" HeaderText="Purpose" SortExpression="DrivenBy.Description" />
    </Columns>
</mmsinc:MvpGridView>
