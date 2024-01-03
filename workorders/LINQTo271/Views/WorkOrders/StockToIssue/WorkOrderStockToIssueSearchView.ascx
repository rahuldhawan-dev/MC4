<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderStockToIssueSearchView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.StockToIssue.WorkOrderStockToIssueSearchView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="mmsinc" TagName="DateRange" Src="~/Common/DateRange.ascx" %>
<%@ Register TagPrefix="wo" TagName="BaseWorkOrderSearch" Src="~/Controls/WorkOrders/BaseWorkOrderSearch.ascx" %>

<center>
    <table>
        <wo:BaseWorkOrderSearch runat="server" ID="baseSearch" ShowAssetType="false" ShowDescriptionOfWork="false"
            ShowNearestCrossStreet="false" ShowStreet="false" ShowStreetNumber="false" ShowTownSection="false" />
        <tr>
            <td>Last Crew Assigned:</td>
            <td> 
                <mmsinc:MvpDropDownList ID="ddlCrew" runat="server" />
                <atk:CascadingDropDown runat="server" ID="cddCrews" TargetControlID="ddlCrew"
                    ParentControlID="baseSearch$ddlOperatingCenter" Category="Crew" EmptyText="None Found" EmptyValue=""
                    PromptText="--Select Here--" PromptValue=""
                    ServicePath="~/Views/Crews/CrewsServiceView.asmx" ServiceMethod="GetCrewsByOperatingCenterID"
                     />
<%--                <mmsinc:MvpDropDownList runat="server" ID="ddlCrew" DataSourceID="odsCrews" AppendDataBoundItems="true"
                    DataValueField="CrewID" DataTextField="Description">
                    <asp:ListItem Value="" Text="--Select Here--" />
                </mmsinc:MvpDropDownList>--%>
            </td>
        </tr>
        <tr>
            <td>Date Completed:</td>
            <td><mmsinc:DateRange runat="server" ID="drDateCompleted" /></td>
        </tr>
        <tr>
            <td>Materials Approved:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlMaterialsApproved" >
                    <asp:ListItem Selected="True">No</asp:ListItem>
                    <asp:ListItem>Yes</asp:ListItem>
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <center>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" />
                </center>
            </td>
        </tr>
    </table>
</center>

<asp:ObjectDataSource runat="server" ID="odsCrews" TypeName="WorkOrders.Model.CrewRepository"
        SelectMethod="SelectAllAsList" DataObjectTypeName="WorkOrders.Model.Crew" />
