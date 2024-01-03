<%@ Page Language="C#" AutoEventWireup="true" Theme="bender" CodeBehind="WorkOrderInputRPCView.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.Input.WorkOrderInputRPCPage" EnableEventValidation="false" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInputResourceRPCView" Src="~/Views/WorkOrders/Input/WorkOrderInputResourceRPCView.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link type="text/css" href="<%#ResolveUrl("~/resources/bender/bender.css")%>" rel="Stylesheet" />
    <link type="text/css" href="<%#ResolveUrl("~/resources/scripts/css/start/jquery-ui-1.8.7.custom.css")%>" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" />
    <wo:WorkOrderInputResourceRPCView runat="server" />
    <mmsinc:ScriptInclude runat="server" IncludesPath="~/resources/scripts/" ScriptFileName="jquery.js" />
    <mmsinc:ScriptInclude runat="server" IncludesPath="~/resources/scripts/" ScriptFileName="jquery-ui.min.js" />
    <mmsinc:ScriptInclude runat="server" IncludesPath="~/resources/scripts/" ScriptFileName="utilities.js" />
    </form>
    
    <script type="text/javascript">

        $(".tabsContainer").tabs();
       
    </script>

</body>
</html>
