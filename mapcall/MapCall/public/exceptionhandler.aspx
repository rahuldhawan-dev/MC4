<%@ Page Language="C#" Title="Error" MasterPageFile="~/Public.master" Theme="bender" AutoEventWireup="true" CodeBehind="exceptionhandler.aspx.cs" Inherits="MapCall.public1.exceptionhandler" %>

<asp:Content ContentPlaceHolderID="cphHeading" runat="server">
    Error
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
<div class="form">
 An error has occurred in the application. <br /><br />
            A mapcall administrator has been notified. <br /><br />
            <asp:HyperLink ID="HyperLink1" runat="server" Text="Click Here" NavigateUrl="~/Modules/HR/home.aspx" /> to return to the application.
</div>
</asp:Content>

