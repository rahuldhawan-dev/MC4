<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderInputResourceView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Input.WorkOrderInputResourceView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInputDetailView" Src="~/Views/WorkOrders/Input/WorkOrderInputDetailView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInputListView" Src="~/Views/WorkOrders/Input/WorkOrderInputListView.ascx" %>

<wo:WorkOrderInputDetailView runat="server" ID="wodvWorkOrder" />
<wo:WorkOrderInputListView runat="server" ID="wolvWorkOrder" />
