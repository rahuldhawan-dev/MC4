<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="WorkOrderGeneralResourceView.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.General.WorkOrderGeneralResourceViewPage" EnableEventValidation="false" ValidateRequest="false" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderGeneralResourceView" Src="~/Views/WorkOrders/General/WorkOrderGeneralResourceView.ascx" %>

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
    <a href="https://sproutvideo.com/videos/7c9ad9bc1b12e1c0f4">General Searching</a>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:WorkOrderGeneralResourceView runat="server" ID="wogrvWorkOrders" />
</asp:Content>
