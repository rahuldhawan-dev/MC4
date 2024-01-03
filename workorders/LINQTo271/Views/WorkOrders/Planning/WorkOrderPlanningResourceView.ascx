<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderPlanningResourceView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Planning.WorkOrderPlanningResourceView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrdersSearchView" Src="~/Views/WorkOrders/Planning/WorkOrderPlanningSearchView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderDetailView" Src="~/Views/WorkOrders/Planning/WorkOrderPlanningDetailView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrdersListView" Src="~/Views/WorkOrders/Planning/WorkOrderPlanningListView.ascx" %>

<wo:WorkOrdersSearchView runat="server" ID="wosvWorkOrders" />
<wo:WorkOrdersListView runat="server" ID="wolvWorkOrders" />
<wo:WorkOrderDetailView runat="server" ID="wodvWorkOrder" />
<div class="container">
    <mmsinc:MvpButton runat="server" ID="btnBackToList" Text="Back to List" OnClick="btnBackToList_Click" Visible="false" />
</div>