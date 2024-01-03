<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderSupervisorApprovalResourceView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.SupervisorApproval.WorkOrderSupervisorApprovalResourceView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderSupervisorApprovalListView" Src="~/Views/WorkOrders/SupervisorApproval/WorkOrderSupervisorApprovalListView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderSupervisorApprovalDetailView" Src="~/Views/WorkOrders/SupervisorApproval/WorkOrderSupervisorApprovalDetailView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderSupervisorApprovalSearchView" Src="~/Views/WorkOrders/SupervisorApproval/WorkOrderSupervisorApprovalSearchView.ascx" %>

<wo:WorkOrderSupervisorApprovalListView runat="server" ID="wolvWorkOrders" />
<wo:WorkOrderSupervisorApprovalSearchView runat="server" ID="wosvWorkOrders" />
<wo:WorkOrderSupervisorApprovalDetailView runat="server" ID="wodvWorkOrder" />
<div class="container">
    <mmsinc:MvpButton runat="server" ID="btnBackToList" Text="Back to List" OnClick="btnBackToList_Click" Visible="false" />
</div>