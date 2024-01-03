<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="OperatingCenterSpoilRemovalCostResourceView.aspx.cs" Inherits="LINQTo271.Views.OperatingCenterSpoilRemovalCosts.OperatingCenterSpoilRemovalCostResourceViewPage" %>
<%@ Register TagPrefix="wo" TagName="OperatingCenterSpoilRemovalCostResourceView" Src="~/Views/OperatingCenterSpoilRemovalCosts/OperatingCenterSpoilRemovalCostResourceView.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Spoil Type Disposal Costs
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:OperatingCenterSpoilRemovalCostResourceView runat="server" />
</asp:Content>