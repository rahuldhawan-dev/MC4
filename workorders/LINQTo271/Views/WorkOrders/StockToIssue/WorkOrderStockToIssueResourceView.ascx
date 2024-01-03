<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderStockToIssueResourceView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.StockToIssue.WorkOrderStockToIssueResourceView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderStockToIssueListView" Src="~/Views/WorkOrders/StockToIssue/WorkOrderStockToIssueListView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderStockToIssueDetailView" Src="~/Views/WorkOrders/StockToIssue/WorkOrderStockToIssueDetailView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderStockToIssueSearchView" Src="~/Views/WorkOrders/StockToIssue/WorkOrderStockToIssueSearchView.ascx" %>

<wo:WorkOrderStockToIssueListView runat="server" ID="wolvWorkOrders" />
<wo:WorkOrderStockToIssueSearchView runat="server" ID="wosvWorkOrders" />
<wo:WorkOrderStockToIssueDetailView runat="server" ID="wodvWorkOrder" />
<div class="container">
    <mmsinc:MvpButton runat="server" ID="btnBackToList" Text="Back to List" OnClick="btnBackToList_Click" Visible="false" />
</div>