<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="WorkOrderRestorationProcessingResourceView.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.RestorationProcessing.WorkOrderRestorationProcessingResourceViewPage" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderRestorationProcessingResourceView" Src="~/Views/WorkOrders/RestorationProcessing/WorkOrderRestorationProcessingResourceView.ascx" %>

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
    RestorationProcessing  
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:WorkOrderRestorationProcessingResourceView runat="server" ID="wofrvWorkOrders" />
</asp:Content>
