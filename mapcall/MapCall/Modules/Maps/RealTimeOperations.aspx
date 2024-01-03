<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RealTimeOperations.aspx.cs" Inherits="MapCall.Modules.Maps.RealTimeOperations" %>
<%@ Register src="~/Controls/Map/MapMenu.ascx" tagName="MapMenu" tagPrefix="mapcall" %>
<%@ Register assembly="MMSINC.Core.WebForms" tagPrefix="mmsinc" namespace="MMSINC.Controls" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>American Water - Realtime Operations - powered by MapCall™</title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8"/>
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/arcgis.css")%>" />
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/resources/bender/arcgis-tundra.css")%>" />
    <link type="text/css" href="<%=ResolveClientUrl("~/resources/bender/bender.css")%>" rel="Stylesheet" />
    <link type="text/css" href="<%=ResolveClientUrl("~/resources/bender/currentLocationStyle.css") %>" rel="Stylesheet"/>
    <link type="text/css" href="<%=ResolveClientUrl("~/resources/scripts/css/start/jquery-ui-1.8.7.custom.css")%>" rel="Stylesheet" />
    <link type="text/css" href="<%=ResolveClientUrl("https://js.arcgis.com/3.33/esri/css/esri.css") %>" rel="Stylesheet"/>
    <link type="text/css" href="realtimeoperations.css" rel="stylesheet" />
    <link type="text/css" href="../../scripts/css/smoothness/jquery-ui-1.8rc3.custom.css" rel="stylesheet" />
</head>
<body>
    <div id="map_canvas"></div>
    <mapcall:MapMenu runat="server">
        <LayersTemplate>
            <input type="checkbox" id="chkHydrants" name="chkHydrants"
                   onclick="RealTimeOperations.ajax.toggleData('hydrants', this.checked);" />
            <label for="chkHydrants">Hydrants</label>
            <br />
            <input type="checkbox" id="chkValves" name="chkValves"
                   onclick="RealTimeOperations.ajax.toggleData('valves', this.checked);" />
            <label for="chkValves">Valves</label>
            <br />
            <input type="checkbox" id="chkWorkOrders" name="chkWorkOrders"
                   onclick="RealTimeOperations.ajax.toggleData('workOrders', this.checked);" />
            <label for="chkWorkOrders">Work Orders</label>
            <br />
            <input type="checkbox" id="chkFRCCWorkOrders" name="chkFRCCWorkOrders"
                   onclick="RealTimeOperations.ajax.toggleData('frccWorkOrders', this.checked);" />
            <label for="chkFRCCWorkOrders">FRCC Work Orders</label>
            <br />
            <input type="checkbox" id="chkOneCallTickets" name="chkOneCallTickets"
                   onclick="RealTimeOperations.ajax.toggleData('onecalltickets', this.checked);" />
            <label for="chkOneCallTickets">811 Tickets</label>
            <br />
            <input type="checkbox" id="chkLeaks" name="chkLeaks"
                   onclick="RealTimeOperations.ajax.toggleData('leaks', this.checked);" />
            <label for="chkLeaks">Leaks</label>
            <br />
            <input type="checkbox" id="chkFlushingSchedules" name="chkFlushingSchedules"
                   onclick="RealTimeOperations.ajax.toggleData('flushingSchedules', this.checked);" />
            <label for="chkFlushingSchedules">Flushing Sched.</label>
            <br />
            <input type="checkbox" id="chkMainBreaks" name="chkMainBreaks"
                   onclick="RealTimeOperations.ajax.toggleData('mainBreaks', this.checked);" />
            <label for="chkMainBreaks">Main Breaks</label>
            <br />
            <input type="checkbox" id="chkComplaints" name="chkComplaints"
                   onclick="RealTimeOperations.ajax.toggleData('complaints', this.checked);" />
            <label for="chkComplaints">WQ Complaints</label>
            <br />
            <input type="checkbox" id="chkBacti" name="chkBacti"
                   onclick="RealTimeOperations.ajax.toggleData('bactis', this.checked);" />
            <label for="chkBacti">Bacti Samples</label>
            <br />
            <input type="checkbox" id="chkLeadCopper" name="chkLeadCopper"
                   onclick="RealTimeOperations.ajax.toggleData('leadCoppers', this.checked);" />
            <label for="chkLeadCopper">Lead/Copper Samples</label>
            <br />
            <input type="checkbox" id="chkEvents" name="chkEvents"
                   onclick="RealTimeOperations.ajax.toggleData('events', this.checked);" />
            <label for="chkEvents">Events</label>
            <br />
            <input type="checkbox" id="chkOverflows" name="chkOverflows"
                   onclick="RealTimeOperations.ajax.toggleData('overflows', this.checked);" />
            <label for="chkOverflows">Sewer Overflows</label>
            <br />
        </LayersTemplate>
        <LegendTemplate>
            <%-- needs to be here so the legend will load --%>
        </LegendTemplate>
        <OptionsTemplate>
            <input type="text" id="txtRange" value="24" style="width:28px;"/> range 
                <select id="ddlRange" onchange="RealTimeOperations.UI.toggleDateRange();">
                    <option>h</option>
                    <option>d</option>
                </select>
            <br />
            <input type="text" id="txtInterval" value="5" style="width:28px;" onchange="RealTimeOperations.updateQueueInterval();" /> refresh(m) <br /> 
            <input type="button" value="update" onclick="RealTimeOperations.UI.updateData();" />
        </OptionsTemplate>
        <NotesTemplate>
            <p>
                Not all GIS layers are active at every zoom level. The GIS Legend displays the layers which are visible at the current scale from among those selected.
            </p>
            <p>
                GIS Layer data is current as of 24 hours prior to the current date/time. Raw data layers (layers whose names begin with 'Raw') represent new coordinate points in the system pending verification and inclusion into the official GIS map.
            </p>
            <p>
                All MapCall RTO data is served live as of the time displayed below.
            </p>
            <p style="display: none">
                Base map last updated <asp:Label runat="server" Text='<%# Container.GISDataDate %>'></asp:Label>.
            </p>
        </NotesTemplate>
        <BottomTemplate>
            <div id="LocateButton"></div>
            <div id="lblUpdated"></div>
        </BottomTemplate>
    </mapcall:MapMenu>
    <form runat="server">
        <mmsinc:MvpHiddenField id="hidMapId" runat="server" value='<%# MapId %>' />
    </form>
    <div id="map_search">
        <input type="text" id="txtSearch" />
        <input type="button" value="Search" onclick="RealTimeOperations.geocodeLocation()" style="font-size:10px" />
    </div>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/jquery.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/jquery-ui.min.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/arcgis.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/jquery.esri.mappin.js")%>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/resources/scripts/jquery.esri.mappin.defaults.js")%>"></script>
    <script type="text/javascript">
            var Application = Application || {};
            Application.MAP_ID = $('#hidMapId').val();
    </script>
    <script type="text/javascript" src="realtimeoperations.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            RealTimeOperations.initialize();
        });
    </script>
</body>
</html>
