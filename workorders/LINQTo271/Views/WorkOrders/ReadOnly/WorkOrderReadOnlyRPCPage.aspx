<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderReadOnlyRPCPage.aspx.cs" Inherits="LINQTo271.Views.WorkOrders.ReadOnly.WorkOrderReadOnlyRPCPage" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderReadOnlyResourceRPCView" Src="~/Views/WorkOrders/ReadOnly/WorkOrderReadOnlyResourceRPCView.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Work Order Print Form</title>
    <link rel="Stylesheet" href="../../../Includes/reset.css" />
    <link rel="Stylesheet" href="../../../Includes/960.css" />
    <link rel="Stylesheet" href="WorkOrderReadOnlyDetailView.css" />
</head>
<body>
    <form id="form1" runat="server">
    <wo:WorkOrderReadOnlyResourceRPCView runat="server" />
    </form>
</body>
</html>
