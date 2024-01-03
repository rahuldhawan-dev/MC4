<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="WorkOrderFinalizationResourceView.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.Finalization.WorkOrderFinalizationResourceViewPage" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderFinalizationResourceView" Src="~/Views/WorkOrders/Finalization/WorkOrderFinalizationResourceView.ascx" %>

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
    </style>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Finalization
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:WorkOrderFinalizationResourceView runat="server" ID="wofrvWorkOrders" />
</asp:Content>
