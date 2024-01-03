<%@ Page Title="Inventory Usage" Language="C#" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="MaterialsUsed.aspx.cs" Inherits="LINQTo271.Views.Reports.MaterialsUsed1" %>
<%@ Register TagPrefix="mapcall" Namespace="MMSINC.Controls" Assembly="MMSINC.Core.WebForms" %>
<%@ Register TagPrefix="atk" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="mmsinc" TagName="DateRange" Src="~/Common/DateRange.ascx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Inventory Usage
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
        <table>
            <tr>
                <th>OpCode:</th>
                <td>
                    <mmsinc:MvpDropDownList runat="server" ID="ddlOperatingCenter" DataSourceID="odsOperatingCenters"
                        DataTextField="OpCntr" DataValueField="OperatingCenterID" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </mmsinc:MvpDropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvOperatingCenter" ControlToValidate="ddlOperatingCenter" 
                        Text="Required" InitialValue="" />
                </td>
            </tr>
            <tr>
                <th>Stock Location</th>
                <td>
                    <mmsinc:MvpDropDownList ID="ddlStockLocation" runat="server" />
                    <atk:CascadingDropDown runat="server" ID="cddStockLocation" TargetControlID="ddlStockLocation" ParentControlID="ddlOperatingCenter"
                        Category="StockLocation" EmptyText="None Found" EmptyValue="" PromptText="--Select Here--"
                        PromptValue="" ServicePath="~/Views/StockLocations/StockLocationServiceView.asmx" ServiceMethod="GetStockLocationsByOperatingCenterID" />
                </td>
            </tr>
             
            <tr>
                <th>Word Order Date Completed:</th>
                <td>
                    <mmsinc:DateRange runat="server" ID="drDateCompleted" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>
    </mmsinc:MvpPanel>
    
    <mmsinc:MvpPanel runat="server" ID="pnlResults" Visible="False">
        <mmsinc:MvpButton runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" AutoGenerateColumns="false" AllowSorting="true"
            OnDataBinding="gvSearchResults_DataBinding" OnSorting="gvSearchResults_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Operating Center" SortExpression="WorkOrder.OperatingCenter">
                    <ItemTemplate><%#Eval("WorkOrder.OperatingCenter")%></ItemTemplate>
                </asp:TemplateField>
                <mapcall:MvpBoundField DataField="StockLocation" HeaderText="Stock Location" SortExpression="StockLocation" />
                
                <asp:TemplateField HeaderText="Date Completed" SortExpression="WorkOrder.DateCompleted">
                    <ItemTemplate>
                        <%#Eval("WorkOrder.DateCompleted", "{0:d}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DocID" SortExpression="WorkOrder.MaterialsDocID">
                    <ItemTemplate>
                        <%#Eval("WorkOrder.MaterialsDocID") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Work Order Number" SortExpression="WorkOrderID">
                    <ItemTemplate>
                        <a href="../WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%# Eval("WorkOrderID") %>">
                            <%# Eval("WorkOrderID") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Work Description" SortExpression="WorkOrder.WorkDescription">
                    <ItemTemplate>
                        <%#Eval("WorkOrder.WorkDescription") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <mapcall:MvpBoundField DataField="PartNumber" HeaderText="Part Number" SortExpression="PartNumber" />
                <mapcall:MvpBoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <mapcall:MvpBoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                <asp:TemplateField HeaderText="SAP Work Order" SortExpression="WorkOrder.SAPWorkOrderNumber">
                    <ItemTemplate><%#Eval("WorkOrder.SAPWorkOrderNumber") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SAP Notification" SortExpression="WorkOrder.SAPNotificationNumber">
                    <ItemTemplate><%#Eval("WorkOrder.SAPNotificationNumber") %></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </mmsinc:MvpGridView>
    </mmsinc:MvpPanel>

    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />    
</asp:Content>
