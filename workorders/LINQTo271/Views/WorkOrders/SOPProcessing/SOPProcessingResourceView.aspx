<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="SOPProcessingResourceView.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.SOPProcessing.SOPProcessingResourceViewPage" %>
<%@ Register TagPrefix="wo" TagName="SOPProcessingResourceView" Src="~/Views/WorkOrders/SOPProcessing/SOPProcessingResourceView.ascx" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphStyle">
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

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphInstructions">
    Street Opening Permits - Work Without Permit Attached
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <wo:SOPProcessingResourceView runat="server" ID="wofrvWorkOrders" />
</asp:Content>
