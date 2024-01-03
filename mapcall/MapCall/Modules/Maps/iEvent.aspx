<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iEvent.aspx.cs" Inherits="MapCall.Modules.Maps.iEvent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Events</title>
    <link rel="Stylesheet" id="lnkLightviewCss" runat="server" />
    <style type="text/css">
        body {font-family:arial;font-size:12px;margin:0px;padding:0px;text-align:left;}
        .leftCol {vertical-align:top;text-align:right;font-weight:bold;border-bottom:1px solid black;width:150px;}
        .rightCol {vertical-align:top;text-align:left;font-weight:normal;border-bottom:1px solid black}
    </style>
</head>
<body style="background-color:White;background-image:none;">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="smMain" />
    <asp:HyperLink runat="server" ID="hl" Text="View" Target="_top"/>
    <div style="background-color:White;text-align:left;font-size:smaller;">
    <cc1:TabContainer runat="server" ID="tcMain" ActiveTabIndex="0">
        <cc1:TabPanel runat="server" HeaderText="Event Details" ID="tabEventDetails">
            <ContentTemplate>
                <!-- This should show up -->
                <mmsi:DataElement runat="server" ID="DataElement1" DataElementName="Events" DataElementParameterName="Id"
                    DataElementTableName="Events" AllowDelete="false" AllowEdit="false" AllowNew="false" />

                <mmsi:Notes ID="ntsMain" runat="server" DataTypeID="72" AllowDelete="False" AllowEdit="False" AllowAdd="False" />
                <mmsi:Documents ID="dcsMain" runat="server" DataTypeID="72" AllowDelete="False" AllowEdit="False" AllowAdd="False" />
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
    </div>
    </form>
</body>
</html>
