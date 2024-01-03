<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderMarkoutPlanningResourceView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.MarkoutPlanning.WorkOrderMarkoutPlanningResourceView" %>
<%@ Register src="WorkOrderMarkoutPlanningListView.ascx" tagname="WorkOrderMarkoutPlanningListView" tagprefix="wo" %>
<%@ Register src="WorkOrderMarkoutPlanningSearchView.ascx" tagname="WorkOrderMarkoutPlanningSearchView" tagprefix="wo" %>

<wo:WorkOrderMarkoutPlanningSearchView ID="wosvWorkOrders" 
    runat="server" />
<wo:WorkOrderMarkoutPlanningListView ID="wolvWorkOrders" 
    runat="server" />
<div class="container">
    <mmsinc:mvpbutton runat="server" id="btnBackToList" text="Back to List" onclick="btnBackToList_Click" visible="false" />
</div>
