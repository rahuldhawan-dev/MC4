<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RestorationAccountingCodeResourceView.ascx.cs" Inherits="LINQTo271.Views.RestorationAccountingCodes.RestorationAccountingCodeResourceView" %>
<%@ Register src="RestorationAccountingCodeListView.ascx" tagname="RestorationAccountingCodeListView" tagprefix="wo" %>
<%@ Register Src="RestorationAccountingCodeSearchView.ascx" TagName="RestorationAccountingCodeSearchView" TagPrefix="wo" %>

<wo:RestorationAccountingCodeSearchView ID="slSearchView" runat="server" />
<wo:RestorationAccountingCodeListView ID="slListView" runat="server" />
