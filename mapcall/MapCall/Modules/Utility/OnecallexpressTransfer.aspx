<%@ Page Title="" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="OnecallexpressTransfer.aspx.cs" Inherits="MapCall.Modules.Utility.OnecallexpressTransfer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadTagScripts" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeadTag" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
One Call EXPRESS
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphInstructions" runat="server">
    Your account does not have access to One Call EXPRESS
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
<script type="text/javascript">
    alert('Your account is not linked to a One Call EXPRESS account. This window will close and you will be brought back to Mapcall.net');
    window.close();
</script>
</asp:Content>
