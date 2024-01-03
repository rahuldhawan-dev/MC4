<%@ Page Title="" Language="C#" Theme="bender" MasterPageFile="~/WorkOrders.Master" AutoEventWireup="true" CodeBehind="RestorationAccountingCodeResourceView.aspx.cs" Inherits="LINQTo271.Views.RestorationAccountingCodes.RestorationAccountingCodeResourceViewPage" %>
<%@ Register src="RestorationAccountingCodeResourceView.ascx" tagName="RestorationAccountingCodeResourceView" tagPrefix="wo" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cphInstructions" runat="server">
    Edit Restoration Accounting Codes
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <wo:RestorationAccountingCodeResourceView runat="server" />
</asp:Content>
