<%@ Page Language="C#" Title="Operations" Theme="bender" AutoEventWireup="true" CodeBehind="login.aspx.cs"
    Inherits="MapCall.login" MasterPageFile="~/Public.master" %>

<asp:Content ContentPlaceHolderID="cphHeading" runat="server">
    Operations
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
    <form runat="server">
        <div class="content" style="margin-bottom:6px; text-align:center;">
            <div runat="server" id="divNoAccess" class="error" style="margin-bottom:6px;">
                This MapCall account is currently disabled.
            </div>
        </div>
        <div class="content" runat="server" id="divLogin">
            <span class="error">
                Your Okta username is not currently associated with an existing MapCall user. Please inform your site
                administrator.
            </span>
       
            <asp:Label runat="server" ID="LoginErrorDetails" CssClass="error" EnableViewState="false" />
        </div>
        <div class="content" runat="server" id="divAuthError">
            <span class="error">
                An error occured when attempting to authenticate your Okta login.
            </span>
       
            <asp:Label runat="server" ID="AuthErrorDetails" CssClass="error" EnableViewState="false" />
        </div>
    </form>
   
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphFooterLeft" EnableViewState="false">

</asp:Content>



