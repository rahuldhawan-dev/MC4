<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContractorCrewAssignmentsResourceView.ascx.cs" Inherits="LINQTo271.Views.ContractorCrewAssignments.ContractorCrewAssignmentsResourceView" %>
<%@ Register TagPrefix="mmsinc" Assembly="MMSINC.Core" Namespace="MMSINC.Controls" %>
<%@ Register TagPrefix="wo" TagName="ContractorCrewAssignmentsSearchView" Src="~/Views/ContractorCrewAssignments/ContractorCrewAssignmentsSearchView.ascx" %>
<%@ Register TagPrefix="wo" TagName="ContractorCrewAssignmentsListView" Src="~/Views/ContractorCrewAssignments/ContractorCrewAssignmentsListView.ascx" %>

<%-- this div supports the two column layout used by the controls in this setup --%>
<div id="wrapper">
    <wo:ContractorCrewAssignmentsSearchView runat="server" ID="lvContractorCrewAssignmentsSearchView"/>
    <wo:ContractorCrewAssignmentsListView runat="server" ID="lvContractorCrewAssignmentsListView" />
</div>

<mmsinc:CssInclude ID="CssInclude1" runat="server" CssFileName="CrewAssignmentsMonthly.css" IncludesPath="~/Views/CrewAssignments/" />