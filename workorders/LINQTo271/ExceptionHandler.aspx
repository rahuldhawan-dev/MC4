<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExceptionHandler.aspx.cs" Inherits="LINQTo271.ExceptionHandler" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Error</title>
    <link rel="Stylesheet" href="~/Includes/reset.css" />
    <link rel="Stylesheet" href="~/Includes/960.css" />
    <link rel="Stylesheet" href="/Includes/Login.css" />
</head>
<body>
<div class="container_12">
    <form id="form1" runat="server">
        <div class="grid_3 headerlogo">
            <img src="/images/logo_front_sm.gif" />
        </div>
        <div class="grid_9 headertext">
            Error Notification
        </div>
        <div class="clear"></div>
        
        <div class="grid_12 blueline">&nbsp;</div>
        <div class="clear"></div>
         
        <div class="grid_12">&nbsp;</div>
        <div class="clear"></div>         

        <div class="grid_12" style="text-align:left;">
            An error has occurred in the application. <br /><br />
            A mapcall administrator has been notified. <br /><br />
            <asp:HyperLink runat="server" Text="Click Here" NavigateUrl="Views/WorkOrders/Input/WorkOrderInputResourceView.aspx" /> to return to the application.

        </div>
        <div class="clear"></div>         

        </div>
    </form>
</div>
</body>
</html>
