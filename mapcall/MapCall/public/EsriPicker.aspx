<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EsriPicker.aspx.cs" Inherits="MapCall.public1.EsriPicker" Theme="bender" %>
<%@ Register src="~/Controls/Map/MapMenu.ascx" tagName="MapMenu" tagPrefix="mapcall" %>
<%@ Register assembly="MMSINC.Core.WebForms" tagPrefix="mmsinc" namespace="MMSINC.Controls" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Coordinate Picker</title>
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/arcgis.css")%>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/arcgis-tundra.css")%>" />
    <link type="text/css" href="<%=ResolveClientUrl("~/resources/bender/bender.css")%>" rel="Stylesheet" />
    <style>
        html {
            height: 100%;
        }
        
        body {
            height: 100%;
            overflow: hidden;
            margin: 0;
        }
        
        #map {
            height: 100%;
            width: 100%;
        }
    </style>
</head>
<body>
    <div id="map"></div>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="sm1"></asp:ScriptManager>
        <mapcall:MapMenu runat="server" ID="menu">
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
        </mapcall:MapMenu>

        <div id="Div1" style="position: absolute; top: 6px; left: 70px;z-index: 2;background: white;border: 1px solid black;padding: 4px">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <span style="font-weight: bold;">Address: </span>
                        <asp:TextBox runat="server" ID="txtLocation" Text="" Width="200px" />
                        <input type="button" id="btnGeoCode" onclick="EsriPicker.geocodeLocation(); return false;" value="Find" />
                        <input type="button" id="btnSave" onclick="EsriPicker.saveCoordinates(); return false;" value="Save Coordinates" />
                        <input type="button" id="btnCancel" onclick="EsriPicker.hidePicker(); return false;" value="Cancel" />
                    </td>
                </tr>
            </table>
        </div>

        <%= GetDefaultIconImage() %>

        <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/jquery.js")%>"></script>
        <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/arcgis.js")%>"></script>
        <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/jquery.esri.mappin.js")%>"></script>
        <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/jquery.esri.mappin.defaults.js")%>"></script>
        <script src="../../resources/scripts/jquery-ui.min.js" type="text/javascript"></script>
        <mmsinc:mvphiddenfield id="hidMapId" runat="server" value='<%# MapId %>' />
        <mmsinc:mvphiddenfield id="hidDefaultLatitude" runat="server" value='<%# DefaultLatitude %>' />
        <mmsinc:mvphiddenfield id="hidDefaultLongitude" runat="server" value='<%# DefaultLongitude %>' />
        <script type="text/javascript">
                var Application = Application || {};
                Application.MAP_ID = $('#hidMapId').val();
        </script>
        <script type="text/javascript" src="esri_picker.js"></script>
        <script type="text/javascript">
            jQuery.noConflict();
            jQuery(document).ready(function () {
                EsriPicker.initialize();
            });
        </script>
    </form>
</body>
</html>
