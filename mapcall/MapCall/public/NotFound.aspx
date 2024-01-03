<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Public.master" Theme="bender" CodeBehind="NotFound.aspx.cs" Inherits="MapCall.public1.NotFound" %>

<asp:Content ContentPlaceHolderID="cphHeading" runat="server">
    Error
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
    <style>
        #errWrapper { padding: 15px; }

        #errWrapper .error-section {
            margin-bottom: 2em;
        }

        #errWrapper .error-section-title {
            font-size: 20px;
        }

        #errWrapper .error-section-message {
            padding-left: 20px;
        }
        #errWrapper .error-section-message span {
            display: block;
            line-height: 2em;
        }
    </style>

<div class="form">
<div id="errWrapper">
    <div id="errMessage">
        <div class="error-section">
            <div class="error-section-title">Error</div>
            <div class="error-section-message">The requested resource could not be found.</div>
        </div>
        <div class="error-section">
            <div class="error-section-title">Possible causes</div>
            <div class="error-section-message">
                <span>1. You may have been given an address to a resource that no longer exists or has been moved to another location.</span>
                <span>2. If you have entered in the address manually, double check that you have entered the address correctly.</span>
                <span>3. If you are trying to access this page through a bookmark then you may need to update your bookmark to point at the correct address.</span>
                <span>4. You may not have the correct security access to acccess this resource. You can check with your site administrator to be sure.</span>
            </div>
        </div>
    </div>
</div>

            <asp:HyperLink ID="HyperLink1" runat="server" Text="Click Here" NavigateUrl="~/Modules/HR/home.aspx" /> to return to the application.
</div>
</asp:Content>

