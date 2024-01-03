<%@ Page Language="C#" AutoEventWireup="true" Theme="bender" CodeBehind="Maps.aspx.cs" Inherits="MapCall.Modules.Maps.Maps" %>
<%@ Register Assembly="MapCall.Common" Namespace="MapCall.Common.Controls" TagPrefix="mapcall" %>
<%@ Register src="~/Controls/Map/MapMenu.ascx" tagName="MapMenu" tagPrefix="mapcall" %>
<%@ Register assembly="MMSINC.Core.WebForms" tagPrefix="mmsinc" namespace="MMSINC.Controls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>MapCall</title>

    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/arcgis.css")%>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/arcgis-tundra.css")%>" />

    <link type="text/css" href="<%=ResolveClientUrl("~/resources/bender/bender.css")%>" rel="Stylesheet" />
    <link type="text/css" href="<%=ResolveClientUrl("~/resources/scripts/css/start/jquery-ui-1.8.7.custom.css")%>" rel="Stylesheet" />
    <link href="../../includes/mapOverlay.css" rel="stylesheet" type="text/css" />

    <script src="<%=ResolveClientUrl("~/resources/scripts/jquery.js")%>" type="text/javascript"></script>

    <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/arcgis.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/jquery.esri.mappin.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/jquery.esri.mappin.defaults.js")%>"></script>
</head>
<body style="text-align: left; direction: ltr;">
    <form id="form1" runat="server">
        <cc1:ToolkitScriptManager runat="server" CombineScripts="true">
        </cc1:ToolkitScriptManager>
        <script src="../../resources/scripts/jquery-ui.min.js" type="text/javascript"></script>
        <mmsinc:mvphiddenfield id="hidMapId" runat="server" value='<%# MapId %>' />
        <script type="text/javascript">
                var Application = Application || {};
                Application.MAP_ID = $('#hidMapId').val();
        </script>
        <script src="../../includes/mapOverlay.js" type="text/javascript"></script>
        <script type="text/javascript">
            

            // Done this way to make VS stop yelling about the semicolon being invalid.
            <%= "Maps._icons = " + GetMapIcons() + ";" %>

            <%= RenderMarkerScript() %>

            $(document).ready(function() {
                Maps.init();
            });
        </script>

        <asp:Panel runat="server" ID="pnlMap" Width="100%" Height="100%" BackColor="White">

            <div id="map" class="divMap"></div>
            <mapcall:MapMenu runat="server" ID="menu">
                <LegendTemplate>
                    <asp:Panel runat="server" ID="pnlWaterQuality">
                        <div style="color: black;">Black - Aesthetics</div>
                        <div style="color: Green;">Green - Medical</div>
                        <div style="color: Purple;">Purple - Information</div>
                        <div style="color: Red;">Red - N/A</div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlProjects" Visible="False">
                        <div style="color: #CCCC02;">Yellow - Submitted</div>
                        <div style="color: green;">Green - Approved</div>
                        <div style="color: blue;">Blue - Complete</div>
                        <div style="color: Red;">Red - Canceled</div>
                    </asp:Panel>
                </LegendTemplate>
                <LayersTemplate>
                    <input id="chkData" type="checkbox" name="chkData" checked="checked" onclick="Maps.toggleLayer('chkData', '', 'Sample_Site');" />
                    <label for="chkData">Data</label>
                    <br />
                    <input id="chkSampleSites" type="checkbox" name="chkSampleSites" onclick="Maps.toggleLayer('chkSampleSites', 'btnLoadSampleSites', 'Sample_Site');"/>
                    <label for="chkSampleSites">Sample Sites</label>
                    <br />
                    <input id="chkFlushingSchedules" type="checkbox" name="chkFlushingSchedules" onclick="Maps.toggleLayer('chkFlushingSchedules', 'btnLoadFlushingSchedules', 'FlushingSchedule');"/>
                    <label for="chkFlushingSchedules">Flushing Schedules</label>
                    <br />
                    <input id="chkMeterRoutes" type="checkbox" name="chkMeterRoutes" onclick="Maps.toggleLayer('chkMeterRoutes', 'btnLoadMeterRoutes', 'MeterRoute');"/>
                    <label for="chkMeterRoutes">Meter Routes</label>
                </LayersTemplate>
                <NotesTemplate>
                    <p>
                        Not all GIS layers are active at every zoom level. The GIS Legend displays the layers which are visible at the current scale from among those selected.
                    </p>
                    <p>
                        All MapCall map data is current as of <asp:Label runat="server" runat="server" Text='<%# Container.MapDataDate %>'></asp:Label>.
                    </p>
                    <p>
                        GIS Layer data is current as of 24 hours prior to the current date/time. Raw data layers (layers whose names begin with 'Raw') represent new coordinate points in the system pending verification and inclusion into the official GIS map.
                    </p>
                    <p style="display: none">
                        Base map last updated <asp:Label runat="server" Text='<%# Container.GISDataDate %>'></asp:Label>.
                    </p>
                </NotesTemplate>
            </mapcall:MapMenu>

            <div id="toolbar" class="divToolbar" style="height: 26px;">
                <img alt="Refresh" src="../../images/view-refresh.png" width="22px;" onclick="location.reload(true);" />
            </div>

            <div id="loading" class="divLoading">
                &nbsp;<asp:Image runat="server" ID="img1" ImageUrl="~/images/loading_animation.gif" Style="margin-top: 3px;" />
                <br />
                loading
            </div>

            <div id="addressSearch">
                <input type="text" id="txtLocation" name="txtLocation" value="" style="width: 200px;" />
                <asp:Button runat="server" ID="btnGeoCode"
                                    OnClientClick="Maps.geocodeLocation(); return false;"
                                    Text="Find" UseSubmitBehavior="false"
                                    CausesValidation="false" />
            </div>

            <div style="display: none; position: absolute; top: 8px; left: 100px; height: 30px; width: 200px; border: 1px solid black; background-color: white;">
                <asp:UpdatePanel runat="server" ID="pnlUpdate">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnLoadSampleSites" OnClick="btnLoadSampleSites_Click" Text="add sample sites" CssClass="DisplayNone" CausesValidation="false" />
                        <asp:Button runat="server" ID="btnLoadFlushingSchedules" OnClick="btnLoadFlushingSChedules_Click" Text="add flushing schedules" CssClass="displayNone" CausesValidation="false" />
                        <asp:Button runat="server" ID="btnLoadMeterRoutes" OnClick="btnLoadMeterRoutes_Click" Text="add flushing schedules" CssClass="displayNone" CausesValidation="false" />
                        <asp:TextBox runat="server" ID="txtRecentActivityComplaintID" CssClass="DisplayNone" />
                        <asp:TextBox runat="server" ID="txtNorthEastLat" CssClass="DisplayNone" />
                        <asp:TextBox runat="server" ID="txtNorthEastLng" CssClass="DisplayNone" />
                        <asp:TextBox runat="server" ID="txtSouthWestLat" CssClass="DisplayNone" />
                        <asp:TextBox runat="server" ID="txtSouthWestLng" CssClass="DisplayNone" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </asp:Panel>
    </form>
</body>
</html>
