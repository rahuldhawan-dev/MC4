<%@ Page Title="Incomplete Orders" Theme="bender" Language="C#" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="IncompleteWorkOrders.aspx.cs" Inherits="LINQTo271.Views.Reports.IncompleteWorkOrders" %>

<asp:Content ContentPlaceHolderID="cphInstructions" runat="server">
    Use this form to display incomplete Work Orders
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">

    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
        <table>
            <tr>
                <th>Work Description:</th>
                <td>
                    <mmsinc:MvpDropDownList runat="server" ID="ddlWorkDescription" DataSourceID="odsWorkDescriptions"
                        DataTextField="Description" DataValueField="WorkDescriptionID" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </mmsinc:MvpDropDownList>
                    <asp:RequiredFieldValidator runat="server" id="rfvWorkDescription"
                        InitialValue="" ControlToValidate="ddlWorkDescription" 
                        Text="Required" />
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
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Back to Search" OnClick="btnReturnToSearch_Click" /> <br />
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" />
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" 
            AutoGenerateColumns="false" AllowSorting="false"
            OnDataBinding="gvSearchResults_DataBinding"
            OnRowDataBound="gvSearchResults_RowDataBound" 
            OnSorting="gvSearchResults_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Work Order Number">
                    <ItemTemplate>
                        <a href="../WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%# Eval("WorkOrderID") %>">
                            <%# Eval("WorkOrderID") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Operating Center" SortExpression="OperatingCenter">
                    <ItemTemplate><%# Eval("OperatingCenter") %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Street Number" DataField="StreetNumber" SortExpression="StreetNumber" />
                <asp:BoundField HeaderText="Street" DataField="Street" SortExpression="Street" />
                <asp:TemplateField HeaderText="Apartment Addtl" SortExpression="ApartmentAddtl">
                    <ItemTemplate><%# Eval("ApartmentAddtl") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Town" SortExpression="Town">
                    <ItemTemplate><%# Eval("Town") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Work Description" SortExpression="WorkDescription">
                    <ItemTemplate><%# Eval("WorkDescription") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Asset Type" SortExpression="AssetType.Description">
                    <ItemTemplate><%# Eval("AssetType") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Asset ID" SortExpression="AssetID">
                    <ItemTemplate><%# Eval("AssetID") %></ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Latitude" SortExpression="Latitude">
                    <ItemTemplate><%# Eval("Latitude")%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Longitude" SortExpression="Longitude">
                    <ItemTemplate><%# Eval("Longitude") %></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Date Received" DataFormatString="{0:d}"
                    SortExpression="DateReceived"
                    DataField="DateReceived" />  
                <asp:TemplateField HeaderText="Crew" SortExpression="CurrentlyAssignedCrew.Description">
                    <ItemTemplate>
                        <%# Eval("CurrentlyAssignedCrew.Description") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Contractor" SortExpression="AssignedContractor.Name">
                    <ItemTemplate>
                        <%# Eval("AssignedContractor") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Purpose" DataField="DrivenBy" SortExpression="DrivenBy" />
                <asp:BoundField HeaderText="Priority" DataField="Priority" SortExpression="Priority" />
            </Columns>
        </mmsinc:MvpGridView>
    </mmsinc:MvpPanel>
    
    <asp:ObjectDataSource runat="server" ID="odsWorkDescriptions" TypeName="WorkOrders.Model.WorkDescriptionRepository"
        SelectMethod="SelectAllSorted" />

</asp:Content>
