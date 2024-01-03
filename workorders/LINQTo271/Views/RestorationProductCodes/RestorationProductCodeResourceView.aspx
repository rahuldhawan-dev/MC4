<%@ Page Language="C#"  MasterPageFile="~/WorkOrders.Master" Theme="bender" AutoEventWireup="true" CodeBehind="RestorationProductCodeResourceView.aspx.cs" Inherits="LINQTo271.Views.RestorationProductCodes.RestorationProductCodeResourceViewPage" %>
<%@ Register src="RestorationProductCodeResourceView.ascx" tagName="RestorationProductCodeResourceView" tagPrefix="wo" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cphInstructions" runat="server">
    Edit Restoration Product Codes
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <wo:RestorationProductCodeResourceView runat="server" />
</asp:Content>
