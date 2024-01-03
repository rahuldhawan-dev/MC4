<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SOPProcessingListView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.SOPProcessing.SOPProcessingListView" %>

<div style="font-size: smaller">* Orders in light green are completed</div>
<asp:Label runat="server" ID="lblCount" ></asp:Label>
<mmsinc:MvpGridView runat="server" ID="gvWorkOrders" AutoGenerateColumns="false"
    OnSelectedIndexChanged="ListControl_SelectedIndexChanged" AutoGenerateSelectButton="true"
    DataKeyNames="WorkOrderID" AllowSorting="true" OnSorting="ListControl_Sorting" OnRowDataBound="gvWorkOrders_RowDataBound">
    <Columns>
        <asp:BoundField DataField="WorkOrderID" HeaderText="Order Number" SortExpression="WorkOrderID" />
        <asp:BoundField DataField="StreetNumber" HeaderText="Street Number" SortExpression="StreetNumber" />
        <asp:BoundField DataField="Street" HeaderText="Street" SortExpression="Street.StreetName" />
        <asp:BoundField DataField="NearestCrossStreet" HeaderText="Nearest Cross Street" SortExpression="NearestCrossStreet.StreetName" />
        <asp:BoundField DataField="Town" HeaderText="Town" SortExpression="Town.Name" />
        <asp:BoundField DataField="TownSection" HeaderText="Town Section" SortExpression="TownSection.Name" />
        <asp:TemplateField SortExpression="WorkDescription.Description" HeaderText="Description&nbsp;of&nbsp;Job<br/>(Hover for Notes)">        
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Title='<%# Eval("Notes") ?? "No Notes Entered" %>' Text='<%# Eval("WorkDescription") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CurrentlyAssignedCrew" HeaderText="Last Assigned To" SortExpression="CurrentlyAssignedCrew" />
        <asp:BoundField DataField="Priority" HeaderText="Job Priority" SortExpression="Priority.Description"/>
    </Columns>
</mmsinc:MvpGridView>
