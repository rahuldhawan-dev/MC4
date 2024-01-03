<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PremiseNumberSearch.ascx.cs" Inherits="MapCall.Controls.Data.PremiseNumberSearch" %>

<%--ClientID is required for some jQuery stuff--%>
<div class="premNumSearch" id='<%= this.ClientID %>'>
    <asp:HiddenField runat="server" ID="hidPremiseID" EnableViewState="false" />
    <asp:HiddenField runat="server" ID="hidPremiseNumber" EnableViewState="false" />
    <div>
        <label for="<%= this.txtPremiseNumber.ClientID %>">Search</label> 
        <asp:TextBox runat="server" 
                     ID="txtPremiseNumber" 
                     Text='<%#Eval("PremiseNumber") %>' 
                     AutoComplete="Off" 
                     MaxLength="10" 
                     EnableViewState="false" />
    </div>
    <div>
        <asp:ListBox runat="server" ID="listPremiseNumber" CssClass="lBox" EnableViewState="false" />
    </div>
    <div>
        Selected Premise:<br />
        <asp:Label runat="server" ID="lblPremiseNumber" CssClass="pNum" EnableViewState="false" />
        [<a runat="server" ID="lnkRemoveSelected" href="#">remove</a>]
        <asp:Label runat="server" ID="rfvPremiseID" ForeColor="Red" EnableViewState="false" />
    </div>
</div>