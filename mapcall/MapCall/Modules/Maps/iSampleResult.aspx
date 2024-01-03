<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/iMap.Master" CodeBehind="iSampleResult.aspx.cs" Inherits="MapCall.Modules.Maps.iSampleResult" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphMain">
    <div style="background-color:White;text-align:left;font-size:smaller;">
        
    <cc1:TabContainer runat="server" ID="tcMain" ActiveTabIndex="0">
        <cc1:TabPanel runat="server" HeaderText="Sample Result" ID="tabSampleResult">
            <ContentTemplate>
                <asp:HyperLink runat="server" id="hlResultDetails" Text="Result Details" Target="_top"/>
                <!-- This should show up -->
                <mmsi:DataElement runat="server" ID="DataElement1" 
                    DataElementName="SampleResult" 
                    ConnectionString="MCProd"
                    DataElementParameterName="Id"
                    DataElementTableName="WaterSamples" 
                    AllowDelete="false" AllowEdit="false" AllowNew="false"
                     />
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
    </div>
</asp:Content>