<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderRestorationProcessingSearchView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.RestorationProcessing.WorkOrderRestorationProcessingSearchView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="wo" TagName="BaseWorkOrderSearch" Src="~/Controls/WorkOrders/BaseWorkOrderSearch.ascx" %>
<%@ Register TagPrefix="mmsinc" TagName="DateRange" Src="~/Common/DateRange.ascx" %>

<center>
    <table>
        <wo:BaseWorkOrderSearch runat="server" ID="baseSearch" />
        <tr>
            <td>Last Crew Assigned:</td>
            <td> 
                <mmsinc:MvpDropDownList ID="ddlCrew" runat="server" />
                <atk:CascadingDropDown runat="server" ID="cddCrews" TargetControlID="ddlCrew"
                    ParentControlID="baseSearch$ddlOperatingCenter" Category="Crew" EmptyText="None Found" EmptyValue=""
                    PromptText="--Select Here--" PromptValue=""
                    ServicePath="~/Views/Crews/CrewsServiceView.asmx" ServiceMethod="GetCrewsByOperatingCenterID"
                     />
            </td>
            <tr>
                <td>
                    <mmsinc:MvpDropDownList ID="ddlDateType" runat="server">
                        <asp:ListItem Value="DateReceived"></asp:ListItem>
                        <asp:ListItem Value="DateCompleted"></asp:ListItem>
                        <asp:ListItem Value="DateDocumentAttached"></asp:ListItem>
                        <asp:ListItem Value="InitialDate"></asp:ListItem>
                        <asp:ListItem Value="FinalDate"></asp:ListItem>
                    </mmsinc:MvpDropDownList>
                </td>
                <td>
                    <mmsinc:DateRange runat="server" ID="drDateToSearch" />
                </td>
            </tr>
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
