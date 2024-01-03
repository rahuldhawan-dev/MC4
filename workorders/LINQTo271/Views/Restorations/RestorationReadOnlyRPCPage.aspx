<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestorationReadOnlyRPCPage.aspx.cs" Inherits="LINQTo271.Views.Restorations.RestorationReadOnlyRPCPage" %>
<%@ Register TagPrefix="wo" TagName="RestorationReadOnlyRPCView" Src="~/Views/Restorations/RestorationReadOnlyRPCView.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" />
    <div class="container_12">
        <wo:RestorationReadOnlyRPCView runat="server" />
    </div>
    <mmsinc:ScriptInclude runat="server" IncludesPath="~/resources/scripts/" ScriptFileName="jquery.js" />
    <mmsinc:CssInclude runat="server" CssFileName="960.css" />
    <mmsinc:CssInclude runat="server" CssFileName="reset.css" />
    <mmsinc:CssInclude runat="server" CssFileName="text.css" />
    <mmsinc:CssInclude runat="server" IncludesPath="~/Views/Restorations/" CssFileName="RestorationReadOnlyRPCPage.css" />
    </form>
</body>
</html>
