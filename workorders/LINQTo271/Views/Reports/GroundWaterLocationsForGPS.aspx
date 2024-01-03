<%@ Page Title="Ground Water Locations for GPS" Language="C#" Theme="WorkOrders" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="GroundWaterLocationsForGPS.aspx.cs" Inherits="LINQTo271.Views.Reports.GroundWaterLocationsForGPS" EnableEventValidation="false" %>
<%@ Register TagPrefix="mmsinc" TagName="DateRange" Src="~/Common/DateRange.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
        <table>
            <tr>
                <th>Operating Center:</th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlOpCode" DataSourceID="odsOperatingCenters"
                        DataTextField="FullDescription" DataValueField="OperatingCenterID" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Date Completed:</th>
                <td>
                    <mmsinc:DateRange runat="server" ID="drCompletedDate" />
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
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" OnDataBinding="gvSearchResults_DataBinding"
            AutoGenerateColumns="false" AllowSorting="true" OnSorting="gvSearchResults_Sorting" >
            <Columns>
                <asp:TemplateField HeaderText="Work Order Number" SortExpression="WorkOrderID">
                    <ItemTemplate>
                        <a href="../WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%# Eval("WorkOrderID") %>">
                            <%# Eval("WorkOrderID") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Longitude" DataField="Longitude" SortExpression="Longitude" />
                <asp:BoundField HeaderText="Latitude" DataField="Latitude" SortExpression="Latitude" />
                <asp:BoundField HeaderText="Street Address" DataField="StreetAddress" SortExpression="StreetAddress" />
                <asp:BoundField HeaderText="Cross Street" DataField="NearestCrossStreet" SortExpression="NearestCrossStreet.FullStName" />
                <asp:BoundField HeaderText="City" DataField="TownAddress" SortExpression="TownAddress" />
                <asp:BoundField HeaderText="Job Description" DataField="WorkDescription" SortExpression="WorkDescription.Description" />
                <asp:TemplateField HeaderText="Proximity">
                    <ItemTemplate>
                        1000
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </mmsinc:MvpGridView>
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Return to Search" OnClick="btnReturnToSearch_Click" />
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" />
    </mmsinc:MvpPanel>

    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
</asp:Content>
