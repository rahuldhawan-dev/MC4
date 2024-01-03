<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderRestorationProcessingResourceView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.RestorationProcessing.WorkOrderRestorationProcessingResourceView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderRestorationProcessingListView" Src="~/Views/WorkOrders/RestorationProcessing/WorkOrderRestorationProcessingListView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderRestorationProcessingDetailView" Src="~/Views/WorkOrders/RestorationProcessing/WorkOrderRestorationProcessingDetailView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderRestorationProcessingSearchView" Src="~/Views/WorkOrders/RestorationProcessing/WorkOrderRestorationProcessingSearchView.ascx" %>

<wo:WorkOrderRestorationProcessingListView runat="server" ID="wolvWorkOrders" />
<wo:WorkOrderRestorationProcessingSearchView runat="server" ID="wosvWorkOrders" />
<wo:WorkOrderRestorationProcessingDetailView runat="server" ID="wodvWorkOrder" />
<div class="container">
<mmsinc:MvpButton runat="server" ID="btnBackToList" Text="Back to List" OnClick="btnBackToList_Click" Visible="false" />
</div>