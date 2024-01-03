<%@ Page Title="" Language="C#" MasterPageFile="~/MapCall.Master" AutoEventWireup="true" CodeBehind="iMeterRecorderHistory.aspx.cs" Inherits="MapCall.Modules.Maps.iMeterRecorderHistory" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-color:White;text-align:left;font-size:smaller;">
    <cc1:TabContainer runat="server" ID="tcMain" ActiveTabIndex="0">
        <cc1:TabPanel runat="server" HeaderText="Meter Test Details" ID="tabMeterTestDetails">
            <ContentTemplate>
                <!-- This should show up -->
                <mmsi:DataElement runat="server" ID="DataElement1" 
                    DataElementName="MeterRecorderHistory" 
                    ConnectionString="MCprod"
                    DataElementParameterName="MeterRecorderHistoryID"
                    DataElementTableName="MeterRecorderHistory" 
                    AllowDelete="false" AllowEdit="false" AllowNew="false"
                     />

                <mmsi:Notes ID="ntsMain" runat="server" DataTypeID="135" />
                <mmsi:Documents ID="dcsMain" runat="server" DataTypeID="135" />
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" HeaderText="Meter Readings"
            ID="tabMeterReadings">
            <ContentTemplate>
                Coming Soon.
            </ContentTemplate>    
        </cc1:TabPanel>
    </cc1:TabContainer>
    </div>
    
    <link rel="Stylesheet" id="lnkLightviewCss" href='<%=ResolveClientUrl("~/includes/lightview-3.2.7/css/lightview/lightview.css") %>' />
</asp:Content>
