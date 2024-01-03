<%@ Page Title="" Language="C#" MasterPageFile="~/WorkOrders.Master" Theme="bender" AutoEventWireup="true" CodeBehind="MaterialResourceView.aspx.cs" Inherits="LINQTo271.Views.Materials.MaterialResourceViewPage" %>
<%@ Register src="MaterialResourceView.ascx" tagName="MaterialResourceView" tagPrefix="wo" %>

<asp:Content ContentPlaceHolderID="cphInstructions" runat="server">
    Edit Part #s and/or Activate Materials
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <wo:MaterialResourceView runat="server"/>    
</asp:Content>
