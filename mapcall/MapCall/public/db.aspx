<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="db.aspx.cs" Inherits="MapCall.public1.db" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DetailsView runat="server" ID="dvTest" DataSourceID="dsTest"></asp:DetailsView>
        <asp:SqlDataSource runat="server" ID="dsTest" 
            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            SelectCommand="select 1 from tblPermissions where username = 'mcadmin'"/>
    </div>
    </form>
</body>
</html>
