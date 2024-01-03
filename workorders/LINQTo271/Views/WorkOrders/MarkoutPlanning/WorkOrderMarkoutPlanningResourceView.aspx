<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="WorkOrderMarkoutPlanningResourceView.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.MarkoutPlanning.WorkOrderMarkoutPlanningResourceViewPage" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutPlanningResourceView" Src="~/Views/WorkOrders/MarkoutPlanning/WorkOrderMarkoutPlanningResourceView.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Markout Planning
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:WorkOrderMarkoutPlanningResourceView runat="server" ID="womprv" />
</asp:Content>
