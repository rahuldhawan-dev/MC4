<%@ Page Language="C#" MasterPageFile="~/WorkOrders.Master" Theme="bender" AutoEventWireup="true" CodeBehind="WorkOrderPlanningResourceView.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.Planning.WorkOrderPlanningResourceViewPage" EnableEventValidation="false" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderPlanningResourceView" Src="~/Views/WorkOrders/Planning/WorkOrderPlanningResourceView.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Planning
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphHeadTagScripts">
    <%-- TODO: These are hacked in here --%>
    <mmsinc:ScriptInclude runat="server" ScriptFileName="jqmodal.js" />
    <mmsinc:ScriptInclude runat="server" ScriptFileName="LatLonPicker.js" />
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

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:WorkOrderPlanningResourceView runat="server" ID="woprv" />
</asp:Content>
