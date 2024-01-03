<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="RestorationTypeCostResourceView.aspx.cs" Inherits="LINQTo271.Views.RestorationTypeCosts.RestorationTypeCostResourceViewPage" %>
<%@ Register TagPrefix="wo" TagName="RestorationTypeCostResourceView" Src="~/Views/RestorationTypeCosts/RestorationTypeCostResourceView.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Restoration Type Cost Estimates
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <wo:RestorationTypeCostResourceView runat="server" />
</asp:Content>
