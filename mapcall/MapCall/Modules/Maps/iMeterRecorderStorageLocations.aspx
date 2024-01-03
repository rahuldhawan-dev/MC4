<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iMeterRecorderStorageLocations.aspx.cs" Inherits="MapCall.Modules.Maps.iMeterRecorderStorageLocations" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Meter Recorder Storage Locations</title>
    <link rel="Stylesheet" id="lnkLightviewCss" runat="server" enableviewstate="false" />
    <style type="text/css">
        body {font-family:arial;font-size:12px;margin:0px;padding:0px;text-align:left;}
        .leftCol {vertical-align:top;text-align:right;font-weight:bold;border-bottom:1px solid black;width:150px;}
        .rightCol {vertical-align:top;text-align:left;font-weight:normal;border-bottom:1px solid black}
    </style>
</head>
<body style="background-color:White;background-image:none;">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="smMain" />
    <cc1:TabContainer runat="server" ID="tcMain" ActiveTabIndex="0" EnableViewState="false">
        <cc1:TabPanel runat="server" HeaderText="Meter Details" EnableViewState="false" ID="tabMeterDetails">
            <ContentTemplate>
                <!-- This should show up -->
                <mmsi:DataElement runat="server" ID="DataElement1" 
                    DataElementName="MeterRecorderStorageLocation" 
                    ConnectionString="MCprod"
                    DataElementParameterName="MeterRecorderStorageLocationID"
                    DataElementTableName="MeterRecorderStorageLocations" 
                    AllowDelete="false" AllowEdit="false" AllowNew="false"
                     />

            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" HeaderText="Notes">
            <ContentTemplate>
                <mmsi:Notes ID="ntsMain" runat="server" DataTypeID="78" />
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" HeaderText="Documents">
            <ContentTemplate>
                <mmsi:Documents ID="dcsMain" runat="server" DataTypeID="78" />
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
    </form>
</body>
</html>
