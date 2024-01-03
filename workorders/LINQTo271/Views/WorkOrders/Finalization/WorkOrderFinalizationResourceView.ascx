<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderFinalizationResourceView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Finalization.WorkOrderFinalizationResourceView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderFinalizationListView" Src="~/Views/WorkOrders/Finalization/WorkOrderFinalizationListView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderFinalizationDetailView" Src="~/Views/WorkOrders/Finalization/WorkOrderFinalizationDetailView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderFinalizationSearchView" Src="~/Views/WorkOrders/Finalization/WorkOrderFinalizationSearchView.ascx" %>

<wo:WorkOrderFinalizationListView runat="server" ID="wolvWorkOrders" />
<wo:WorkOrderFinalizationSearchView runat="server" ID="wosvWorkOrders" />
<wo:WorkOrderFinalizationDetailView runat="server" ID="wodvWorkOrder" />
<div class="container">
    <mmsinc:MvpButton runat="server" ID="btnBackToList" Text="Back to List" OnClick="btnBackToList_Click" Visible="false" />
</div>