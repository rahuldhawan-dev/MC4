<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="SOPProcessingResourceView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.SOPProcessing.SOPProcessingResourceView" %>
<%@ Register TagPrefix="wo" TagName="SOPProcessingListView" Src="~/Views/WorkOrders/SOPProcessing/SOPProcessingListView.ascx" %>
<%@ Register TagPrefix="wo" TagName="SOPProcessingDetailView" Src="~/Views/WorkOrders/SOPProcessing/SOPProcessingDetailView.ascx" %>
<%@ Register TagPrefix="wo" TagName="SOPProcessingSearchView" Src="~/Views/WorkOrders/SOPProcessing/SOPProcessingSearchView.ascx" %>

<wo:SOPProcessingListView runat="server" ID="wolvWorkOrders" />
<wo:SOPProcessingSearchView runat="server" ID="wosvWorkOrders" />
<wo:SOPProcessingDetailView runat="server" ID="wodvWorkOrder" />
<div class="container">
    <mmsinc:MvpButton runat="server" ID="btnBackToList" Text="Back to List" OnClick="btnBackToList_Click" Visible="false" CausesValidation="false"/>
</div>