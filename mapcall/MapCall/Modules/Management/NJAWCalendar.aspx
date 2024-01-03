<%@ Page Title="NJAW Calendar" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="NJAWCalendar.aspx.cs" Inherits="MapCall.Modules.Management.NJAWCalendar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
NJAW Calendar
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
<iframe src="http://www.google.com/calendar/embed?src=steve.tambini%40amwater.com&ctz=America/New_York"
    style="border: 0" width="800" height="600" frameborder="0" scrolling="no"></iframe>
</asp:Content>
