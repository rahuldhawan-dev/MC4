<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderPlanningListView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Planning.WorkOrderPlanningListView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderTableLegend" Src="~/Controls/WorkOrders/WorkOrderTableLegend.ascx" %>

<wo:WorkOrderTableLegend runat="server"></wo:WorkOrderTableLegend>
<asp:Label runat="server" ID="lblCount" ></asp:Label>
<mmsinc:MvpGridView runat="server" ID="gvWorkOrders" ShowHeader="true"
    AutoGenerateColumns="false" OnSelectedIndexChanged="ListControl_SelectedIndexChanged"
    AutoGenerateSelectButton="true" DataKeyNames="WorkOrderID" AllowSorting="true" OnSorting="ListControl_Sorting" OnRowDataBound="gvWorkOrders_RowDataBound">
    <Columns>        
        <asp:BoundField DataField="WorkOrderID" HeaderText="Order Number" SortExpression="WorkOrderID" />
        <asp:BoundField DataField="MarkoutExpirationDate" HeaderText="Markout Expiration Date" SortExpression="MarkoutExpirationDate" DataFormatString="{0:d}" />
        <asp:BoundField DataField="Town" HeaderText="Town" SortExpression="Town.Name" />
        <asp:BoundField DataField="TownSection" HeaderText="Town Section" SortExpression="TownSection.Name" />
        <asp:BoundField DataField="StreetNumber" HeaderText="Street Number" SortExpression="StreetNumber" />        
        <asp:BoundField DataField="Street" HeaderText="Street" SortExpression="Street.StreetName" />
        <asp:BoundField DataField="NearestCrossStreet" HeaderText="Nearest Cross Street" SortExpression="NearestCrossStreet.StreetName" />
        <asp:BoundField DataField="AssetType" HeaderText="Asset Type" SortExpression="AssetType.Description" />
        <asp:BoundField DataField="DrivenBy" HeaderText="Purpose" SortExpression="DrivenBy.Description" />
        <%-- TODO: The Asset class currently won't sort, it's not IComparable --%>
        <asp:BoundField DataField="AssetID" HeaderText="Asset ID" />
        <asp:TemplateField SortExpression="WorkDescription.Description" HeaderText="Description&nbsp;of&nbsp;Job<br/>(Hover for Notes)">        
            <ItemTemplate>
                <asp:Label runat="server" Title='<%# Eval("Notes") ?? "No Notes Entered" %>' Text='<%# Eval("WorkDescription") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:BoundField DataField="MarkoutRequired" HeaderText="Markout Required" SortExpression="MarkoutRequired" />
        <asp:BoundField DataField="Priority" HeaderText="Job Priority" SortExpression="Priority.Description" />
    </Columns>
</mmsinc:MvpGridView>