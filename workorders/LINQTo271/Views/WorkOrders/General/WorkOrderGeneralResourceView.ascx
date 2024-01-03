<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderGeneralResourceView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.General.WorkOrderGeneralResourceView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderGeneralListView" Src="~/Views/WorkOrders/General/WorkOrderGeneralListView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderGeneralDetailView" Src="~/Views/WorkOrders/General/WorkOrderGeneralDetailView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderGeneralSearchView" Src="~/Views/WorkOrders/General/WorkOrderGeneralSearchView.ascx" %>

<wo:WorkOrderGeneralListView runat="server" ID="wolvWorkOrders" />
<wo:WorkOrderGeneralSearchView runat="server" ID="wosvWorkOrders" />
<wo:WorkOrderGeneralDetailView runat="server" ID="wodvWorkOrder" />
<div class="container">
    <mmsinc:MvpButton runat="server" ID="btnBackToList" Text="Back to List" OnClick="btnBackToList_Click" Visible="false" />
</div>