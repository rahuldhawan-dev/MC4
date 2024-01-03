<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="WorkOrderFinalizationResourceRPCPage.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.Finalization.WorkOrderFinalizationResourceRPCPage" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderFinalizationResourceRPCView" Src="~/Views/WorkOrders/Finalization/WorkOrderFinalizationResourceRPCView.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphStyle">
    <style type="text/css">
        table.border
        {
            border-collapse: collapse;
        }
        table.border tbody tr td, table.border thead tr th
        {
            border: 1px solid black;
        }
        td.bold
        {
            font-weight: bold;
        }
        #tblNewMarkout tr td table tr td
        {
            border: none;
        }
        #contentDiv > table, #divMain, #divContent
        {
            width: 100%;
        }
        #mapWrapper, iframe.map
        {
            width: 100%;
            height: 400px;
        }
        .ui-widget-overlay, .ui-dialog {
            z-index: 2;
        }
        .ui-dialog .ui-dialog-titlebar-close span {top: -1px;left: -1px;}
</style>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Finalization
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:WorkOrderFinalizationResourceRPCView runat="server" />
</asp:Content>
