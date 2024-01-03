<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestorationRPCPage.aspx.cs" Inherits="LINQTo271.Views.Restorations.RestorationRPCPage" %>
<%@ Register TagPrefix="wo" TagName="RestorationResourceRPCView" Src="~/Views/Restorations/RestorationResourceRPCView.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Restoration</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" />
    <wo:RestorationResourceRPCView runat="server" />
    <mmsinc:ScriptInclude runat="server" IncludesPath="~/resources/scripts/" ScriptFileName="jquery.js" />
    <mmsinc:ScriptInclude runat="server" IncludesPath="~/resources/scripts/" ScriptFileName="utilities.js" />
    <mmsinc:CssInclude runat="server" IncludesPath="~/Views/Restorations/" CssFileName="RestorationRPCPage.css" />
    </form>
</body>
</html>
