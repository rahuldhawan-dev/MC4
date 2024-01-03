<%@ Page Title="Spoil Removal" Language="C#" MasterPageFile="~/WorkOrders.Master" Theme="bender" AutoEventWireup="true" CodeBehind="SpoilRemovalResourceView.aspx.cs" Inherits="LINQTo271.Views.SpoilRemovals.SpoilRemovalResourceViewPage" %>
<%@ Register src="SpoilRemovalResourceView.ascx" tagname="SpoilRemovalResourceView" tagprefix="wo" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphInstructions">
    Insert or Edit a Spoil Removal
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphMain">
    <wo:SpoilRemovalResourceView runat="server" />
</asp:Content>