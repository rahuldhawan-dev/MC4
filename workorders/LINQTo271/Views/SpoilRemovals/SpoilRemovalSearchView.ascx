<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpoilRemovalSearchView.ascx.cs" Inherits="LINQTo271.Views.SpoilRemovals.SpoilRemovalSearchView" %>

<table>
    <tr>
        <td>Operating Center:</td>
        <td>
            <mmsinc:MvpDropDownList runat="server" ID="ddlOperatingCenter" DataSourceID="odsOperatingCenters"
                DataTextField="FullDescription" DataValueField="OperatingCenterID" AppendDataBoundItems="true" AutoPostBack="true" />
        </td>
    </tr>
</table>
<br />
<asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
    SelectMethod="SelectUserOperatingCenters" />
