<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/iMap.Master" CodeBehind="iFlushingSchedule.aspx.cs" Inherits="MapCall.Modules.Maps.iFlushingSchedule" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <div style="background-color:White;text-align:left;font-size:smaller;">
    <cc1:TabContainer runat="server" ID="tcMain" ActiveTabIndex="0">
        <cc1:TabPanel runat="server" HeaderText="Flushing Schedule" ID="tabFlushingSchedule">
            <ContentTemplate>
                <a href='<%= ResolveClientUrl("~/Modules/FieldServices/FlushingSchedules.aspx?view=" + Request.QueryString["ID"]) %>' target="_top">View</a>
                <!-- This should show up -->
                <mmsi:DataElement runat="server" ID="DataElement1" 
                    DataElementName="FlushingSchedule" 
                    ConnectionString="MCProd"
                    DataElementParameterName="FlushingScheduleID"
                    DataElementTableName="FlushingSchedules" 
                    AllowDelete="false" AllowEdit="false" AllowNew="false"
                     />

                <mmsi:Notes ID="ntsMain" runat="server" DataTypeID="128" />
                <mmsi:Documents ID="dcsMain" runat="server" DataTypeID="128" />
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
    </div>
</asp:Content>