<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrewResourceView.ascx.cs" Inherits="LINQTo271.Views.Crews.CrewResourceView" %>
<%@ Register TagPrefix="wo" TagName="CrewDetailView" Src="~/Views/Crews/CrewDetailView.ascx" %>
<%@ Register TagPrefix="wo" TagName="CrewsListView" Src="~/Views/Crews/CrewListView.ascx" %>

<wo:CrewsListView runat="server" ID="clvCrews" />
<wo:CrewDetailView runat="server" ID="cdvCrew" />
<div class="container">
    <mmsinc:MvpButton runat="server" ID="btnBackToList" Text="Back to List" OnClick="btnBackToList_Click" />
</div>