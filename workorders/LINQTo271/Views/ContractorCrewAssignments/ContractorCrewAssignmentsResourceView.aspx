<%@ Page Title="" Language="C#" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="ContractorCrewAssignmentsResourceView.aspx.cs" Inherits="LINQTo271.Views.ContractorCrewAssignments.ContractorCrewAssignmentsResourceViewPage" %>
<%@ Register TagPrefix="wo" TagName="ContractorCrewAssignmentsResourceView" Src="~/Views/ContractorCrewAssignments/ContractorCrewAssignmentsResourceView.ascx" %>
<asp:Content ID="Content4" ContentPlaceHolderID="cphInstructions" runat="server">
    Contractor Crew Assignments    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <wo:ContractorCrewAssignmentsResourceView runat="server" ID="carv" />
</asp:Content>
