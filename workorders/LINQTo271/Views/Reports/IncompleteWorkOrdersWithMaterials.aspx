<%@ Page Title="Incomplete Orders with Materials" Theme="bender" Language="C#" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="IncompleteWorkOrdersWithMaterials.aspx.cs" Inherits="LINQTo271.Views.Reports.IncompleteWorkOrdersWithMaterials" EnableEventValidation="false" %>
<%@ Register TagPrefix="mapcall" Namespace="MMSINC.Controls" Assembly="MMSINC.Core.WebForms" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
        <table>
            <tr>
                <th>Operating Center:</th>
                <td>
                    <mmsinc:MvpDropDownList runat="server" ID="ddlOperatingCenter" DataSourceID="odsOperatingCenters"
                        DataTextField="FullDescription" DataValueField="OperatingCenterID" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </mmsinc:MvpDropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvOperatingCenter" ControlToValidate="ddlOperatingCenter" 
                        Text="Required" InitialValue="" />
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
        <mmsinc:MvpButton runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" AutoGenerateColumns="false" AllowSorting="true"
            OnDataBinding="gvSearchResults_DataBinding" OnSorting="gvSearchResults_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Work Order Number" SortExpression="WorkOrderID">
                    <ItemTemplate>
                        <a href="../WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%# Eval("WorkOrderID") %>">
                            <%# Eval("WorkOrderID") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Town" DataField="Town" SortExpression="Town.Name" />
                <asp:TemplateField HeaderText="Address">
                    <ItemTemplate>
                        <%# Eval("StreetNumber") %> <%# Eval("Street") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Apartment Addtl">
                    <ItemTemplate>
                        <%# Eval("ApartmentAddtl") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description&nbsp;of&nbsp;Job<br/>(Hover for Notes)"
                    SortExpression="WorkDescription.Description">
                    <ItemTemplate>
                        <asp:Label runat="server" Title='<%# Eval("Notes") ?? "No Notes Entered" %>' Text='<%# Eval("WorkDescription") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Asset Type" DataField="AssetType" SortExpression="AssetType.Description" />
                <asp:BoundField HeaderText="AssetID or Premise #" DataField="AssetID" SortExpression="AssetID" />
                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" />
                <asp:BoundField DataField="CreatedOn" HeaderText="Date Created" SortExpression="CreatedOn" />
                <asp:TemplateField HeaderText="SAP Work Order" SortExpression="SAPWorkOrderNumber">
                    <ItemTemplate><%#Eval("SAPWorkOrderNumber") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SAP Notification" SortExpression="SAPNotificationNumber">
                    <ItemTemplate><%#Eval("SAPNotificationNumber") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Crew Assignments">
                    <ItemTemplate>
                        <mmsinc:MvpGridView runat="server" ID="lvCrewAssignments"
                            DataSource='<%# Eval("CrewAssignments") %>'
                            AutoGenerateColumns="false" ShowEmptyTable="false" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderText="Crew" DataField="Crew" />
                                <asp:BoundField HeaderText="DateStarted" DataField="DateStarted" />
                                <asp:BoundField HeaderText="DateEnded" DataField="DateEnded" />
                            </Columns>
                        </mmsinc:MvpGridView>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Materials Installed">                                        
                    <ItemTemplate>
                        <mmsinc:MvpGridView runat="server" ID="lvMaterialsInstalled" 
                            DataSource='<%# Eval("MaterialsUseds") %>' 
                            AutoGenerateColumns="false" ShowEmptyTable="false" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderText="PartNumber" DataField="PartNumber" />
                                <asp:BoundField HeaderText="Description" DataField="Description" />                                
                                <asp:BoundField HeaderText="NonStockDescription" DataField="NonStockDescription" />
                                <asp:BoundField HeaderText="Quantity" DataField="Quantity" />
                            </Columns>
                        </mmsinc:MvpGridView>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </mmsinc:MvpGridView>
        
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Return to Search" OnClick="btnReturnToSearch_Click" />
        
    </mmsinc:MvpPanel>

    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />    
</asp:Content>
