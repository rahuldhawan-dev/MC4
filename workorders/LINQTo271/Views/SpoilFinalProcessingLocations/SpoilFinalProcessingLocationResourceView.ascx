<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpoilFinalProcessingLocationResourceView.ascx.cs" Inherits="LINQTo271.Views.SpoilFinalProcessingLocations.SpoilFinalProcessingLocationResourceView" %>
<%@ Register src="SpoilFinalProcessingLocationListView.ascx" tagname="SpoilFinalProcessingLocationListView" tagprefix="wo" %>
<%@ Register src="SpoilFinalProcessingLocationSearchView.ascx" tagname="SpoilFinalProcessingLocationSearchView" tagprefix="wo" %>

<wo:SpoilFinalProcessingLocationSearchView ID="sslSearchView" runat="server" />
<wo:SpoilFinalProcessingLocationListView ID="sslListView" runat="server" />
