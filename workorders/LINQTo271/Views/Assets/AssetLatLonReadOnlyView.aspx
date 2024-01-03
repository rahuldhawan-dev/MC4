<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetLatLonReadOnlyView.aspx.cs" Inherits="LINQTo271.Views.Assets.AssetLatLonReadOnlyView" %>
<%@ Register TagPrefix="wo" TagName="MapMenu" Src="~/Common/MapMenu.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title></title>
    
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/arcgis.css")%>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/arcgis-tundra.css")%>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/currentLocationStyle.css")%>" />
    <link rel="stylesheet" href="<%=ResolveUrl("https://js.arcgis.com/3.33/esri/css/esri.css")%>" />
    <link href="<%=ResolveClientUrl("~/resources/bender/bender.css")%>" rel="Stylesheet" type="text/css" />

    <style type="text/css">
        body, html {
            margin: 0px;
            padding: 0px;
            width: 100%;
            height: 100%;
        }
        
        #map {
            width: 100%;
            height: 100%;
            position: absolute;
            left: 0px;
            top: 0px;
        }

        .bold {
            font-weight: bold;
        }

        #txtLocation {
            width: 200px;
        }
    </style>
</head>
<body onload="EsriPicker.loadMap()">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />
        <div id="map"></div>

        <wo:MapMenu runat="server">
            <NotesTemplate>
                <p>
                    Not all GIS layers are active at every zoom level. The GIS Legend displays the layers which are visible at the current scale from among those selected.
                </p>
                <p>
                    GIS Layer data is current as of 24 hours prior to the current date/time. Raw data layers (layers whose names begin with 'Raw') represent new coordinate points in the system pending verification and inclusion into the official GIS map.
                </p>
                <p style="display: none">
                    Base map last updated <asp:Label runat="server" Text='<%# Container.GISDataDate %>'></asp:Label>.
                </p>
            </NotesTemplate>
        </wo:MapMenu>

        <mmsinc:MvpHiddenField runat="server" ID="hidMapId" Value='<%# MapId %>' />
        <mmsinc:MvpHiddenField runat="server" ID="hidLongitude" Value='<%# Longitude %>' EnableViewState="true" />
        <mmsinc:MvpHiddenField runat="server" ID="hidLatitude" Value='<%# Latitude %>' EnableViewState="true" />

        <mmsinc:ClientIDRepository runat="server" />
        <mmsinc:ScriptInclude runat="server" ScriptFileName="jquery.js" IncludesPath="~/resources/scripts/" />
        <mmsinc:ScriptInclude runat="server" IncludesPath="~/resources/scripts/" ScriptFileName="jquery-ui.min.js" />
        <mmsinc:ScriptInclude runat="server" ScriptFileName="arcgis.js" IncludesPath="~/resources/scripts/" />
        <mmsinc:ScriptInclude runat="server" ScriptFileName="jquery.esri.mappin.js" IncludesPath="~/resources/scripts/" />
        <mmsinc:ScriptInclude runat="server" ScriptFileName="jquery.esri.mappin.defaults.js" IncludesPath="~/resources/scripts/" />
        <script type="text/javascript">
            var Application = Application || {};
            Application.MAP_ID = $('#hidMapId').val();
        </script>
        <script type="text/javascript" src="EsriPicker.js"></script>
    </form>
</body>
</html>
