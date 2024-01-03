<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LatLonPickerSample.aspx.cs" Inherits="LINQTo271.Common.LatLonPickerSample" %>
<%@ Register TagPrefix="mmsinc" TagName="LatLonPicker" Src="~/Common/LatLonPicker.ascx" %>
<%@ Register TagPrefix="mmsinc" TagName="ClientIDRepository" Src="~/Common/ClientIDRepository.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>LatLonPicker Sample</title>

    <link type="text/css" rel="stylesheet" href="../Includes/jqModal.css" />
    <script type="text/javascript" src="../Includes/jquery.js"></script>
    <script type="text/javascript" src="../Includes/jqModal.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <mmsinc:ClientIDRepository runat="server" />
    This picker should have a blue icon, because it has coordinates (it also shouldn't work): <br />
    <%-- valve on Brighton Ave, Shark River Hills, Neptune, NJ --%>
    <mmsinc:LatLonPicker runat="server" ID="llpWithCoordinates" AssetTypeID="1" AssetID="53827" /><br />
    This picker should have a red icon, because it has no coordinates: <br />
    <%-- valve on HWY 34, XStreet BECHSTEIN DR, Aberdeen, NJ --%>
    <mmsinc:LatLonPicker runat="server" ID="llpWithoutCoordinates" AssetTypeID="1" AssetID="39359" />
    </form>
</body>
</html>
