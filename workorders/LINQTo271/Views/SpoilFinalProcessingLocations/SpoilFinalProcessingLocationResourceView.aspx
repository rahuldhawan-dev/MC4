<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="SpoilFinalProcessingLocationResourceView.aspx.cs" Inherits="LINQTo271.Views.SpoilFinalProcessingLocations.SpoilFinalProcessingLocationResourceViewPage" %>
<%@ Register src="SpoilFinalProcessingLocationResourceView.ascx" tagname="SpoilFinalProcessingLocationResourceView" tagprefix="wo" %>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Insert or Edit a Spoil FinalProcessing Location...
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:SpoilFinalProcessingLocationResourceView runat="server" />
</asp:Content>
