<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContractorCrewAssignmentsSearchView.ascx.cs" Inherits="LINQTo271.Views.ContractorCrewAssignments.ContractorCrewAssignmentsSearchView" %>
<%@ Register TagPrefix="wo" TagName="CrewAssignmentsByMonth" Src="~/Views/CrewAssignments/CrewAssignmentsByMonth.ascx" %>
<%@ Register TagPrefix="wo" TagName="CrewAssignmentsMonthLegend" Src="~/Views/CrewAssignments/CrewAssignmentsMonthLegend.ascx" %>
<%@ Register Assembly="MMSINC.Core" Namespace="MMSINC.Controls" TagPrefix="mmsinc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<%-- this div supports the two column layout used by the controls in this setup --%>
<div id="left-column">
    <table>
        <tr>
            <td class="label">Contractor:</td>
            <td class="field">
                <mmsinc:MvpDropDownList runat="server" ID="ddlContractor" DataSourceID="odsContractors" 
                    AppendDataBoundItems="true"
                    DataValueField="ContractorID" DataTextField="Name">
                    <asp:ListItem Value="" Text="--Select Here--" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td class="label">Crew:</td>
            <td class="field">
                <mmsinc:MvpDropDownList runat="server" ID="ddlCrew" />
                <atk:CascadingDropDown runat="server" ID="cddCrew" TargetControlID="ddlCrew"
                    BehaviorID="cddCrew"
                    ParentControlID="ddlContractor" Category="Crew" EmptyText="Select a Contractor" EmptyValue=""
                    PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Crews...]"
                    ServicePath="~/Views/Crews/CrewsServiceView.asmx" ServiceMethod="GetCrewsByContractor" />
            </td>
        </tr>
        <tr>
            <td class="label">Date:</td>
            <td class="field">
                <mmsinc:mvptextbox runat="server" id="ccDate" autopostback="true" OnTextChanged="ccDate_TextChanged" autocomplete="off" />
                <atk:calendarextender runat="server" id="ceDate" targetcontrolid="ccDate" />
            </td>
        </tr>
    </table>
    <wo:CrewAssignmentsMonthLegend ID="CrewAssignmentsMonthLegend1" runat="server" />
</div>
<div id="right-column">
    <wo:CrewAssignmentsByMonth runat="server" ID="cabmCrewAssignments" OnSelectedDateChanged="cabmCrewAssignments_SelectedDateChanged" />
    <br />
</div>

<asp:ObjectDataSource runat="server" ID="odsCrews" TypeName="WorkOrders.Model.CrewRepository"
    SelectMethod="SelectAllSorted" DataObjectTypeName="WorkOrders.Model.Crew" />
<asp:ObjectDataSource runat="server" ID="odsContractors" TypeName="WorkOrders.Model.ContractorRepository"
    SelectMethod="SelectAllSorted" DataObjectTypeName="WorkOrders.Model.Contractor" />
