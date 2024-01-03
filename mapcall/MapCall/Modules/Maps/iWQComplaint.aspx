<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iWQComplaint.aspx.cs" Inherits="MapCall.Modules.Maps.iWQComplaint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server" id="head">
    <title>Untitled Page</title>
    <style type="text/css">
        body {font-family:arial;font-size:12px;margin:0px;padding:0px;text-align:left;}
        .leftCol {vertical-align:top;text-align:right;font-weight:bold;border-bottom:1px solid black;width:150px;}
        .rightCol {vertical-align:top;text-align:left;font-weight:normal;border-bottom:1px solid black}
        .pnlImageLink {position:absolute;top:0px;right:0px;cursor:pointer;color:navy;text-decoration:underline;padding-right:9px;}	
    </style>
    
    <script src="<%#ResolveClientUrl("~/resources/scripts/jquery.js")%>" type="text/javascript"></script>
</head>

<body style="background-color:White;background-image:none;">
    <form id="form1" runat="server">
        <asp:HyperLink runat="server" ID="hl" Text="View" Target="_top"/>
    <div style="background-color:White;text-align:left;font-size:smaller;">
        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" >
            <cc1:TabPanel runat="server" HeaderText="Water Quality Complaint" ID="tabSampleSite">
                <ContentTemplate>
                    <asp:ScriptManager runat="server" ID="sm1"></asp:ScriptManager>
                    <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
                        DataElementName = "Water Quality Complaints"
                        DataElementParameterName = "Id"
                        DataElementTableName = "WaterQualityComplaints"
                        AllowEdit="False"
                        
                    />
                </ContentTemplate>
            </cc1:TabPanel>
        </cc1:TabContainer>
    </div>
    </form>
</body>
</html>
