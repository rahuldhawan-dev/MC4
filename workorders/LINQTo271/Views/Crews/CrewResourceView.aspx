<%@ Page Language="C#" MasterPageFile="~/WorkOrders.Master" Theme="bender" AutoEventWireup="true" CodeBehind="CrewResourceView.aspx.cs" Inherits="LINQTo271.Views.Crews.CrewResourceViewPage" %>
<%@ Register TagPrefix="wo" TagName="CrewResourceView" Src="~/Views/Crews/CrewResourceView.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Crew Management
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:CrewResourceView runat="server" ID="crv" />
</asp:Content>
