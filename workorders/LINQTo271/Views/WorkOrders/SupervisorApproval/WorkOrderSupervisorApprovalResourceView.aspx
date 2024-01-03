<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="WorkOrderSupervisorApprovalResourceView.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.SupervisorApproval.WorkOrderSupervisorApprovalResourceViewPage" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderSupervisorApprovalResourceView" Src="~/Views/WorkOrders/SupervisorApproval/WorkOrderSupervisorApprovalResourceView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInputListView" Src="~/Views/WorkOrders/Input/WorkOrderInputListView.ascx" %>


<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Supervisor Approval
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:WorkOrderSupervisorApprovalResourceView runat="server" ID="wofrvWorkOrders" />
    <wo:WorkOrderInputListView runat="server" ID="wolvWorkOrder" /> 
</asp:Content>
