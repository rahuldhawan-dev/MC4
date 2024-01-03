<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RestorationProductCodeResourceView.ascx.cs" Inherits="LINQTo271.Views.RestorationProductCodes.RestorationProductCodeResourceView" %>
<%@ Register src="RestorationProductCodeListView.ascx" tagname="RestorationProductCodeListView" tagprefix="wo" %>
<%@ Register Src="RestorationProductCodeSearchView.ascx" TagName="RestorationProductCodeSearchView" TagPrefix="wo" %>

<wo:RestorationProductCodeSearchView ID="slSearchView" runat="server" />
<wo:RestorationProductCodeListView ID="slListView" runat="server" />
