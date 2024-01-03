<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComboBox.ascx.cs" Inherits="LINQTo271.ComboBox.ComboBox" %>

<span style="<%= Style %>">
    <asp:HiddenField runat="server" ID="hidValue" />
    <asp:HiddenField runat="server" ID="hidText" />
    <input type="text" id="<%= TextClientID %>" autocomplete="off"
        onblur="return ComboBox.TextBox.blur(event, $(this), $('#<%= lbOptions.ClientID %>'))"
        onchange="return ComboBox.TextBox.change($(this), $('#<%= hidText.ClientID %>'))"
        onkeyup="return ComboBox.TextBox.keyUp(event, $(this), $('#<%= lbOptions.ClientID %>'))"
        onkeydown="return ComboBox.TextBox.keyDown(event, $('#<%= lbOptions.ClientID %>'), $('#<%= hidValue.ClientID %>'), $('#<%= hidText.ClientID %>'), $(this))" />
    <span style="<%= ArrowWrapperStyle %>">
        <img id="<%= ImgClientID %>" src="/includes/ddlArrowNormal.png" alt=""
            onmousedown="return ComboBox.Button.mouseDown($(this))"
            onmouseup="return ComboBox.Button.mouseUp($(this))"
            onclick="return ComboBox.Button.click($('#<%= lbOptions.ClientID %>'))" />
    </span><br />
    <asp:ListBox runat="server" ID="lbOptions" Rows="5" AppendDataBoundItems="false" />
</span>
