<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpoilRemovalResourceView.ascx.cs" Inherits="LINQTo271.Views.SpoilRemovals.SpoilRemovalResourceView" %>
<%@ Register src="SpoilRemovalListView.ascx" tagname="SpoilRemovalListView" tagprefix="wo" %>
<%@ Register Src="SpoilRemovalSearchView.ascx" TagName="SpoilRemovalSearchView" TagPrefix="wo" %>

<wo:SpoilRemovalSearchView ID="srSearchView" runat="server" />
<wo:SpoilRemovalListView ID="srListView" runat="server" />

