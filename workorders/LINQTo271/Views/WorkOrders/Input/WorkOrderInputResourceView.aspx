<%@ Page Language="C#" MasterPageFile="~/WorkOrders.Master" Theme="bender" AutoEventWireup="true" CodeBehind="WorkOrderInputResourceView.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.Input.WorkOrderInputResourceViewPage" EnableEventValidation="false" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInputResourceView" Src="~/Views/WorkOrders/Input/WorkOrderInputResourceView.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Input
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:WorkOrderInputResourceView runat="server" ID="woirvParaDesWorkOrdersOI" />
</asp:Content>
