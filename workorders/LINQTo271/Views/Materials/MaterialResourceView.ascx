<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaterialResourceView.ascx.cs" Inherits="LINQTo271.Views.Materials.MaterialsResourceView" %>
<%@ Register src="MaterialListView.ascx" tagname="MaterialListView" tagprefix="wo" %>
<%@ Register Src="MaterialSearchView.ascx" TagName="MaterialSearchView" TagPrefix="wo" %>

<wo:MaterialSearchView ID="slSearchView" runat="server" />
<wo:MaterialListView ID="slListView" runat="server" />
