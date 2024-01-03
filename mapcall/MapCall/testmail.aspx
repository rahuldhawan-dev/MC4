<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testmail.aspx.cs" Inherits="MapCall.testmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Enter Email: 
        <asp:TextBox runat="server" ID="txtEmail"></asp:TextBox>
        
        <br />
        <asp:Button runat="server" ID="btnGo" OnClick="btnGo_Click" text="Send"/>
    </div>
    </form>
</body>
</html>
