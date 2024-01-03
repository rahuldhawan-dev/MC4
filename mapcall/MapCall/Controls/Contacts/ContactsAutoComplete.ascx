<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactsAutoComplete.ascx.cs" Inherits="MapCall.Controls.Contacts.ContactsAutoComplete" %>
<div class="autoComplete" id="<%= ClientID %>">
    <asp:HiddenField ID="hidAC" runat="server" />
    <div class="selected">
        Selected: <span class="label"><%= SelectedContactName %> </span> 
    </div>
    <div class="search">
        <asp:TextBox ID="tbText" runat="server" CssClass="autocomplete" AutoCompleteType="Disabled" />
        <asp:RequiredFieldValidator ID="rfvCac" runat="server" Display="Dynamic" 
            ControlToValidate="tbText" 
            ErrorMessage="Required" />
    </div>
</div>