<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderGeneralListView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.General.WorkOrderGeneralListView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderTableLegend" Src="~/Controls/WorkOrders/WorkOrderTableLegend.ascx" %>

<wo:WorkOrderTableLegend runat="server"></wo:WorkOrderTableLegend>
<asp:Label runat="server" ID="lblCount" ></asp:Label>
<mmsinc:MvpGridView runat="server" ID="gvWorkOrders" AutoGenerateColumns="false"
    OnSelectedIndexChanged="ListControl_SelectedIndexChanged" AutoGenerateSelectButton="false"
    DataKeyNames="WorkOrderID" AllowSorting="true" OnSorting="ListControl_Sorting" OnRowDataBound="gvWorkOrders_RowDataBound">
    <Columns>
        <asp:HyperLinkField HeaderText="" Text="Select" DataNavigateUrlFields="WorkOrderId" DataNavigateUrlFormatString="WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg={0}" />
        <asp:HyperLinkField HeaderText="" Text="Print" DataNavigateUrlFields="WorkOrderID" DataNavigateUrlFormatString="../ReadOnly/WorkOrderReadOnlyRPCPage.aspx?cmd=view&arg={0}" Target="_blank" />
        <asp:BoundField DataField="WorkOrderID" HeaderText="Order Number" SortExpression="WorkOrderID" />
        <asp:BoundField HeaderText="SAP Notification #" DataField="SAPNotificationNumber" SortExpression="SAPNotificationNumber"/>
        <asp:BoundField HeaderText="SAP Work Order #" DataField="SAPWorkOrderNumber" SortExpression="SAPWorkOrderNumber"/>
        <asp:BoundField DataField="StreetNumber" HeaderText="Street Number" SortExpression="StreetNumber" />
        <asp:BoundField DataField="Street" HeaderText="Street" SortExpression="Street.StreetName" />
        <asp:BoundField DataField="ApartmentAddtl" HeaderText="Apartment Addtl" SortExpression="ApartmentAddtl" />
        <asp:BoundField DataField="NearestCrossStreet" HeaderText="Nearest Cross Street" SortExpression="NearestCrossStreet.StreetName" />
        <asp:BoundField DataField="Town" HeaderText="Town" SortExpression="Town.Name" />
        <asp:BoundField DataField="TownSection" HeaderText="Town Section" SortExpression="TownSection.Name" />
        <asp:BoundField DataField="AssetType" HeaderText="Asset Type" SortExpression="AssetType.Description" />
        <asp:BoundField DataField="AssetID" HeaderText="Asset ID" SortExpression="AssetID" />
        <asp:TemplateField SortExpression="WorkDescription.Description" HeaderText="Description&nbsp;of&nbsp;Job<br/>(Hover for Notes)">        
            <ItemTemplate>
                <asp:Label runat="server" Title='<%# Eval("Notes") ?? "No Notes Entered" %>' Text='<%# Eval("WorkDescription") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy.FullName" />
        <asp:BoundField DataField="DrivenBy" HeaderText="Purpose" SortExpression="DrivenBy.Description" />
        <asp:BoundField DataField="CurrentlyAssignedCrew" HeaderText="Last Assigned To" SortExpression="CurrentlyAssignedCrew" />
        <asp:BoundField DataField="LastAssignedDate" HeaderText="Last Assigned Date" SortExpression="LastAssignedDate" DataFormatString="{0:d}"/>
        <asp:BoundField DataField="DateReceived" HeaderText="Date Received" SortExpression="DateReceived" DataFormatString="{0:d}"/>
        <asp:BoundField DataField="DateCompleted" HeaderText="Date Completed" SortExpression="DateCompleted" DataFormatString="{0:d}"/>
        <asp:TemplateField SortExpression="AssignedContractor" HeaderText="AssignedContractor">        
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# string.Concat(Eval("AssignedContractor"), Eval("AssignedToContractorOn")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</mmsinc:MvpGridView>
