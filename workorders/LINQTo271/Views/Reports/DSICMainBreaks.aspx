<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" EnableEventValidation="false" CodeBehind="DSICMainBreaks.aspx.cs" Inherits="LINQTo271.Views.Reports.DSICMainBreaks" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="mmsinc" TagName="DateRange" Src="~/Common/DateRange.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
        <style>
            /* fix the z-index of the first date picker */
            td#tdDateReceived table { z-index: 1000 !important; }
        </style>

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
                <th>Date Received:</th>
                <td id="tdDateReceived"><mmsinc:DateRange runat="server" ID="drDateReceived" /></td>
            </tr>
            <tr>
                <th>Date Completed:</th>
                <td><mmsinc:DateRange runat="server" ID="drDateCompleted" /></td>
            </tr>
            <tr>
                <th>Town:</th>
                <td>
                    <mmsinc:MvpDropDownList runat="server" ID="ddlTown" />
                    <atk:CascadingDropDown runat="server" ID="cddTowns" TargetControlID="ddlTown" ParentControlID="ddlOpCode"
                        Category="Town" EmptyText="None Found" EmptyValue="" PromptText="--Select Here--"
                        PromptValue="" ServicePath="~/Views/Towns/TownsServiceView.asmx" ServiceMethod="GetTownsByOperatingCenterID"
                        SelectedValue='<%# Bind("TownID") %>' BehaviorID="cddTowns" />
                </td>
            </tr>
            <tr>
                <th>Material:</th>
                <td>
                    <mmsinc:MvpDropDownList runat="server" ID="ddlMainBreakMaterial" DataSourceID="odsMainBreakMaterials"
                        DataTextField="Description" DataValueField="MainBreakMaterialID" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </mmsinc:MvpDropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click"
                        OnClientClick="return validateSearch()" />
                </td>
            </tr>
        </table>
        <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Model.OperatingCenterRepository"
            SelectMethod="SelectAll271OperatingCenters" />
        <asp:ObjectDataSource runat="server" ID="odsMainBreakMaterials" TypeName="WorkOrders.Model.MainBreakMaterialRepository"
            SelectMethod="SelectAllAsList" />            
    </mmsinc:MvpPanel>

    <mmsinc:MvpPanel runat="server" ID="pnlResults" Visible="false">
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Back to Search" OnClick="btnReturnToSearch_Click" />
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" />
        <br />

        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" AutoGenerateColumns="false" OnDataBinding="gvSearchResults_DataBinding">
            <Columns>
                <asp:TemplateField HeaderText="Operating Center">
                    <ItemTemplate><asp:Label runat="server" Text='<%# Eval("WorkOrder.OperatingCenter") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Work Order Number">
                    <ItemTemplate>
                        <a href="../WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%# Eval("WorkOrderID") %>">
                            <%# Eval("WorkOrderID") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Work Description">
                    <ItemTemplate><asp:Label runat="server" Text='<%# Eval("WorkOrder.WorkDescription") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date Received">
                    <ItemTemplate><asp:Label runat="server" Text='<%# Eval("WorkOrder.DateReceived") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date Completed">
                    <ItemTemplate><asp:Label runat="server" Text='<%# Eval("WorkOrder.DateCompleted") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Street Address">
                    <ItemTemplate><asp:Label runat="server" Text='<%# Eval("WorkOrder.StreetAddress") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Town">
                    <ItemTemplate><asp:Label runat="server" Text='<%# Eval("WorkOrder.Town") %>' /></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ServiceSize" HeaderText="Main Size" SortExpression="ServiceSize" />
                <asp:BoundField DataField="Depth" HeaderText="Depth" SortExpression="Depth" />
                <asp:BoundField DataField="MainBreakMaterial" HeaderText="Material" SortExpression="MainBreakMaterial" />
                <asp:BoundField DataField="MainFailureType" HeaderText="Failure Type" SortExpression="MainFailureType" />
                <asp:BoundField DataField="MainBreakSoilCondition" HeaderText="Soil Condition" SortExpression="MainBreakSoilCondition" />
                <asp:TemplateField HeaderText="Customers Affected" >
                    <ItemTemplate><asp:Label runat="server" Text='<%# Eval("WorkOrder.CustomerImpactRange") %>' /></ItemTemplate>                
                </asp:TemplateField>
                <asp:BoundField DataField="ShutdownTime" HeaderText="Shutdown Time" SortExpression="Shutdown Time" />
                <asp:TemplateField HeaderText="Alert Issued?">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.AlertIssued") == null ? "" : ((bool)Eval("WorkOrder.AlertIssued")) ? "Yes" : "No" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Latitude" >
                    <ItemTemplate><asp:Label runat="server" Text='<%# Eval("WorkOrder.Latitude") %>' /></ItemTemplate>                
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Longitude" >
                    <ItemTemplate><asp:Label runat="server" Text='<%# Eval("WorkOrder.Longitude") %>' /></ItemTemplate>                
                </asp:TemplateField>
                <asp:CheckBoxField DataField="BoilAlertIssued" HeaderText="Boil Alert Issued"/>
            </Columns>
        </mmsinc:MvpGridView>
    </mmsinc:MvpPanel>
</asp:Content>
