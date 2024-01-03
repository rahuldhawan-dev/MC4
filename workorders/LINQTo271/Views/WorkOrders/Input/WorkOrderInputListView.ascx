<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderInputListView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Input.WorkOrderInputListView" %>

<asp:HiddenField runat="server" ID="hidHistoryOperatingCenterID" />
<asp:HiddenField runat="server" ID="hidHistoryAssetTypeID"  />
<%-- asp:HiddenFields don't do postback.  this gets made hidden on page load --%>
<asp:TextBox runat="server" ID="hidHistoryAssetID" AutoPostBack="true" OnTextChanged="hidAssetID_TextChanged" />
<mmsinc:MvpUpdatePanel runat="server" ID="upWorkOrderHistory" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hidAssetHasOpenWorkOrders" Value='<%# HasOpenOrders %>' />
        <mmsinc:MvpGridView ID="gvWorkOrders" runat="server" AutoGenerateSelectButton="true"
            AllowSorting="true" AllowPaging="true" PageSize="5" AutoGenerateColumns="false"
            DataSourceID="odsWorkOrders" DataKeyNames="WorkOrderID" OnSelectedIndexChanged="ListControl_SelectedIndexChanged"
            OnClientSelect="javascript:return WorkOrderInputListView.gvWorkOrders_Select(this);" OnRowDataBound="ListControl_RowDataBound" 
            ShowEmptyTable="false">
            <Columns>
                <mmsinc:DynamicBoundField DataField="WorkOrderID" HeaderText="Order Number: " SortExpression="WorkOrderID" />

                <asp:TemplateField HeaderText="Operating Center: ">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("OperatingCenter.OpCntr") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <mmsinc:DynamicBoundField DataField="Town" HeaderText="Town: " SortExpression="Town" />
                <mmsinc:DynamicBoundField DataField="StreetNumber" HeaderText="Street Number: " SortExpression="StreetNumber" />                
                <mmsinc:DynamicBoundField DataField="Street" HeaderText="Street: " SortExpression="Street" />
                <mmsinc:DynamicBoundField DataField="NearestCrossStreet" HeaderText="Nearest Cross Street: " SortExpression="NearestCrossStreet" />
                <asp:TemplateField SortExpression="WorkDescription.Description" HeaderText="Description&nbsp;of&nbsp;Work:<br/>(Hover for Notes)">        
                    <ItemTemplate>
                        <asp:Label runat="server" Title='<%# Eval("Notes") ?? "No Notes Entered" %>' Text='<%# Eval("WorkDescription") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <mmsinc:DynamicBoundField DataField="Priority" HeaderText="Priority: " SortExpression="Priority" />
                <mmsinc:DynamicBoundField DataField="DateReceived" HeaderText="Date Received: " SortExpression="DateReceived" />
                <mmsinc:DynamicBoundField DataField="DateCompleted" HeaderText="Date Completed: " SortExpression="DateCompleted" />
            </Columns>
        </mmsinc:MvpGridView>
        <asp:ObjectDataSource ID="odsWorkOrders" runat="server" SelectMethod="GetWorkOrdersByOperatingCenterAndAsset"
            TypeName="WorkOrders.Model.WorkOrderRepository" SortParameterName="sortExpression">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidHistoryOperatingCenterID" Name="operatingCenterID"
                    PropertyName="Value" Type="Int32" />
                <asp:ControlParameter ControlID="hidHistoryAssetTypeID" Name="assetTypeID" PropertyName="Value"
                    Type="Int32" />
                <asp:ControlParameter ControlID="hidHistoryAssetID" Name="assetKey" PropertyName="Text"
                    Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="hidHistoryAssetID" EventName="TextChanged" />
        <asp:PostBackTrigger ControlID="gvWorkOrders" />
    </Triggers>
</mmsinc:MvpUpdatePanel>

<asp:UpdateProgress runat="server" ID="upgWorkOrderHistory" AssociatedUpdatePanelID="upWorkOrderHistory">
    <ProgressTemplate>
        Loading historical Work Orders for chosen asset...
    </ProgressTemplate>
</asp:UpdateProgress>
