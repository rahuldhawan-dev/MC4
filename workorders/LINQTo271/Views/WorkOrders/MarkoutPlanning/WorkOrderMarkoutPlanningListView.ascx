<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderMarkoutPlanningListView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.MarkoutPlanning.WorkOrderMarkoutPlanningListView" %>
<%@ Import Namespace="MapCall.Common.Utility" %>
<%@ Import Namespace="WorkOrders.Library" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<asp:Label runat="server" ID="lblCount"></asp:Label>

<style>
.markout_type 
{
  width: auto;
}        
</style>

<mmsinc:MvpGridView runat="server" ID="gvWorkOrders" AutoGenerateColumns="false" DataKeyNames="WorkOrderID"
    AllowSorting="true" OnSorting="ListControl_Sorting" OnSelectedIndexChanged="ListControl_SelectedIndexChanged"
    OnRowCommand="gvWorkOrders_RowCommand">
    <Columns>
        <asp:TemplateField HeaderText="Order Number" SortExpression="WorkOrderID">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblOrderNumber" Text='<%# Eval("WorkOrderID") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Created By" SortExpression="CreatedBy.FullName">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblCreatedBy" Text='<%#String.Format("<a href=\"mailto:{1}\">{0} - {1}</a>", Eval("CreatedBy"), Eval("CreatedBy.Email"))%>'/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="SAP Notification #" DataField="SAPNotificationNumber" SortExpression="SAPNotificationNumber"/>
        <asp:BoundField HeaderText="SAP Work Order #" DataField="SAPWorkOrderNumber" SortExpression="SAPWorkOrderNumber"/>
        <asp:BoundField DataField="DateReceived" HeaderText="Date Received" SortExpression="DateReceived"
            DataFormatString="{0:d}" />
        <asp:BoundField DataField="StreetNumber" HeaderText="Street Number" SortExpression="StreetNumber" />
        <asp:BoundField DataField="Street" HeaderText="Street" SortExpression="Street.StreetName" />
        <asp:BoundField DataField="NearestCrossStreet" HeaderText="Nearest Cross Street"
            SortExpression="NearestCrossStreet.StreetName" />
        <asp:BoundField DataField="Town" HeaderText="Town" SortExpression="Town.Name" />
        <asp:BoundField DataField="TownSection" HeaderText="Town Section" SortExpression="TownSection.Name" />
        <asp:BoundField DataField="AssetType" HeaderText="Asset Type" SortExpression="AssetType.Description" />
        <%-- TODO: The Asset class currently won't sort, it's not IComparable --%>
        <asp:BoundField DataField="AssetID" HeaderText="Asset ID" />
        <asp:TemplateField HeaderText="Description of Job" SortExpression="WorkDescription.Description">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblWorkDescription" Title='<%# Eval("Notes") ?? "No Notes Entered" %>'
                    Text='<%# Eval("WorkDescription") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority.Description" />
        <asp:TemplateField HeaderText="Date Markout Needed">
            <ItemTemplate>
                <asp:TextBox runat="server" ID="txtDateNeeded" CssClass="date_needed" Width="80px"
                     Text='<%# Eval("MarkoutToBeCalled") == null ? "" : WorkOrdersWorkDayEngine.GetRoutineReadyDate((DateTime)Eval("MarkoutToBeCalled")).ToString("d") %>' autocomplete="off" />
                <atk:CalendarExtender runat="server" ID="ceDateNeeded" TargetControlID="txtDateNeeded" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date Markout to be Called">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("MarkoutToBeCalled") == null ? "unset" : ((DateTime)Eval("MarkoutToBeCalled")).ToString("d") %>' CssClass="date_to_be_called" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Markout Type">
            <ItemTemplate>
                <asp:DropDownList runat="server" ID="ddlMarkoutType" DataSourceID="odsMarkoutTypes" SelectedValue='<%# Eval("MarkoutTypeNeededID") %>'
                    AppendDataBoundItems="true" DataTextField="Description" DataValueField="MarkoutTypeID" CssClass="markout_type">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </asp:DropDownList>
                <asp:TextBox runat="server" ID="txtMarkoutNote" Text='<%# Eval("RequiredMarkoutNote") %>' style='<%# Eval("RequiredMarkoutNote") == null ? "display: none" : "" %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbSave" Text="Save" CommandName="save" CssClass="save_link" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</mmsinc:MvpGridView>

<asp:ObjectDataSource runat="server" ID="odsMarkoutTypes" SelectMethod="SelectAllSorted"
    TypeName="WorkOrders.Model.MarkoutTypeRepository" />
