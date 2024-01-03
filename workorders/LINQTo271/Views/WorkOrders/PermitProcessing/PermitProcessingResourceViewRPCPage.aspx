<%@ Page Title="" Language="C#" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="PermitProcessingResourceViewRPCPage.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.PermitProcessing.PermitProcessingResourceViewRPCPage" %>
<%@Register tagPrefix="wo" tagName="PermitProcessingResourceRPCView" src="PermitProcessingResourceViewRPC.ascx" %>
<asp:Content ID="Content4" ContentPlaceHolderID="cphInstructions" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <wo:PermitProcessingResourceRPCView runat="server" />
</asp:Content>
