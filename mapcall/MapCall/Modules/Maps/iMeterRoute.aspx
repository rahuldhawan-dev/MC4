<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/iMap.Master" CodeBehind="iMeterRoute.aspx.cs" Inherits="MapCall.Modules.Maps.iMeterRoute" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphMain">
    <div style="background-color:White;text-align:left;font-size:smaller;">
    <cc1:TabContainer runat="server" ID="tcMain" ActiveTabIndex="0">
        <cc1:TabPanel runat="server" HeaderText="Meter Route" ID="tabMeterRoute">
            <ContentTemplate>
                <a href='<%= ResolveClientUrl("~/Modules/Customer/MeterRoutes.aspx?view=" + Request.QueryString["recordID"]) %>' target="_top">View</a>
                    
                <!-- This should show up -->
                <mmsi:DataElement runat="server" ID="DataElement1" 
                    DataElementName="MeterRoute" 
                    ConnectionString="MCProd"
                    DataElementParameterName="MeterRouteID"
                    DataElementTableName="MeterRoutes" 
                    AllowDelete="false" AllowEdit="false" AllowNew="false"
                     />

                <mmsi:Notes ID="ntsMain" runat="server" DataTypeID="142" />
                <mmsi:Documents ID="dcsMain" runat="server" DataTypeID="142" />
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
    </div>
</asp:Content>