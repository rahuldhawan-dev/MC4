<%@ Page Title="Sewer Overflow Report" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="SewerOverflowReport.aspx.cs" Inherits="MapCall.Reports.FieldServices.SewerOverflowReport" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Sewer Overflows
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" id="dfIncidentDate" DataType="Date" DataFieldName="IncidentDate" HeaderText="Incident Date:" />
            <mmsi:DataField runat="server" id="DataField1" DataType="DropDownList"
                HeaderText="OpCntr :"
                DataFieldName="OperatingCenterID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct OperatingCenterID as val, OperatingCenterCode as txt from OperatingCenters order by 1 asc"
            />  
            
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
        </center>
    </asp:Panel>

   <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            AutoGenerateColumns="false" Font-Size="Small" >
            <Columns>
                <asp:BoundField DataField="IncidentYear" HeaderText="IncidentYear" />
                <asp:BoundField DataField="IncidentDate" HeaderText="Date" SortExpression="IncidentDate" />
                <asp:BoundField DataField="EnforcingAgencyCaseNumber" HeaderText="Enforcing Agency Case Number" SortExpression="EnforcingAgencyCaseNumber" />
                <asp:BoundField DataField="Town" HeaderText="Town" SortExpression="Town" />
                <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                <asp:BoundField DataField="Amount" HeaderText="AMT/GAL" SortExpression="Amount" />
                <asp:BoundField DataField="BodyOfWater" HeaderText="Body Of Water" SortExpression="BodyOfWater" />
                <asp:BoundField DataField="LocationOfStoppage" HeaderText="Location of Stoppage" SortExpression="LocationOfStoppage" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="System.Data.SqlClient"
            CancelSelectOnNullParameter="false"
            SelectCommand="
SELECT 
    Year(IncidentDate) as IncidentYear,
	IncidentDate as [IncidentDate], 
	EnforcingAgencyCaseNumber,
	(Select Town from Towns where Towns.TownID = SewerOverflows.TownID) as [Town],
	IsNull(StreetNumber,'') + ' ' + isNull((Select FullStName from [Streets] s where s.StreetID = SewerOverflows.StreetID),'') as [Location],
	isNull(GallonsFlowedIntoBodyOfWater,0) as Amount,
	(Select Name from BodiesOfWater where BodiesOfWater.BodyOfWaterID = SewerOverflows.BodyOfWaterID) as [BodyOfWater],
	LocationOfStoppage, 
	OperatingCenterID
FROM 
	SewerOverflows 
WHERE
	EnforcingAgencyCaseNumber is not null
ORDER BY 
    IncidentDate 
            "
        />
    </asp:Panel>
</asp:Content>
