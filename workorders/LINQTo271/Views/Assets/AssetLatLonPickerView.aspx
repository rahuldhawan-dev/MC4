<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssetLatLonPickerView.aspx.cs" Inherits="LINQTo271.Views.Assets.AssetLatLonPickerView" %>
<%@ Register TagPrefix="wo" TagName="AssetTypeIDsScript" Src="~/Views/AssetTypes/AssetTypesJSView.ascx" %>
<%@ Register TagPrefix="wo" TagName="MapMenu" Src="~/Common/MapMenu.ascx" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/arcgis.css")%>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/arcgis-tundra.css")%>" />
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
        }

        .bold {
            font-weight: bold;
        }

        #txtLocation {
            width: 200px;
        }

        .control-wrapper {
            position: absolute;
            border: 1px solid black;
            background: white;
            left: 70px;
            top: 6px;
            padding: 4px;
        }
    </style>
</head>
<body onload="EsriPicker.loadMap()">
    <div id="map"></div>
    <form runat="server" defaultbutton="btnGeoCode">
        <asp:ScriptManager runat="server" />
        <div class="control-wrapper">
            <span class="bold">Address :</span>
            <input type="text" id="txtLocation" value="<%# Location %>" />
            <asp:Button runat="server" ID="btnGeoCode" Text="Find" OnClientClick="EsriPicker.geocodeLocation(); return false;" />
            <asp:Button runat="server" ID="btnSave" Text="Save Coordinates" OnClick="btnFormSave_Click" OnClientClick="return EsriPicker.btnSave_Click();" />
        </div>

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

        <wo:AssetTypeIDsScript runat="server" />
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
