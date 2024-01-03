<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapMenu.ascx.cs" Inherits="LINQTo271.Common.MapMenu" %>

<div id="map_menu_wrapper">
    <div id="map_menu">
        <asp:PlaceHolder runat="server" ID="layersOuterPlaceHolder">
        <div id="mapLayersHeader" class="map_menu_header">Layers</div>
        <div id="mapLayers" class="map_menu_section">
            <asp:PlaceHolder runat="server" ID="layersInnerPlaceHolder" />
        </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="legendOuterPlaceHolder">
        <div id="mapLegendHeader" class="map_menu_header">Legend</div>
        <div id="mapLegend" class="map_menu_section">
            <asp:PlaceHolder runat="server" ID="legendInnerPlaceHolder" />
        </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="gisLayersOuterPlaceHolder">
            <div id="mapGISLayersHeader" class="map_menu_header">GIS Layers</div>
            <div id="mapGISLayers" class="map_menu_section"></div>
        </asp:PlaceHolder>
        <div id="mapGISLegendHeader" class="map_menu_header">GIS Legend</div>
        <div id="mapGISLegend" class="map_menu_section">
            <div id="gisLegend"></div>
        </div>
        <div id="basemapGalleryHeader" class="map_menu_header">Basemap</div>
        <div id="basemapGallery" class="map_menu_section">
            <div id="basemapSelector"></div>
        </div>
        <asp:PlaceHolder runat="server" ID="optionsOuterPlaceHolder">
            <div id="mapOptionsHeader" class="map_menu_header">Options</div>
            <div id="mapOptions" class="map_menu_section">
                <asp:PlaceHolder runat="server" ID="optionsInnerPlaceHolder" />
            </div>
        </asp:PlaceHolder>
        <div id="mapNotesHeader" class="map_menu_header">Note</div>
        <div id="mapNotes" class="map_menu_section">
            <asp:PlaceHolder runat="server" ID="notesPlaceHolder"></asp:PlaceHolder>
        </div>
        <div id="LocateButton"></div>
        <asp:PlaceHolder runat="server" ID="bottomPlaceHolder" />
        <script type="text/javascript" src="<%= ResolveClientUrl("~/resources/scripts/MapMenu.js") %>"></script>
        <link rel="stylesheet" href="<%= ResolveClientUrl("~/resources/scripts/css/MapMenu.css") %>"/>
    </div>
</div>
