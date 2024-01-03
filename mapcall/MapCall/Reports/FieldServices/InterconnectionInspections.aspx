<%@ Page Title="Interconnection Tests" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="InterconnectionInspections.aspx.cs" Inherits="MapCall.Reports.FieldServices.InterconnectionInspections" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagName="OpCntrDataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Interconnection Tests
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">

    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:OpCntrDataField runat="server" DataFieldName="OperatingCenterCode" UseText="true" />
                
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
        </center>
        <br />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            AutoGenerateColumns="true"
            AllowSorting="true"
            >
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="System.Data.SqlClient"
            SelectCommand="
SELECT DISTINCT
	OperatingCenterCode,	
	pws.PWSID,
	Interconnection,
	f.FacilityID,
	FacilityName,
	s.FullStName as StreetName,
	cs.FullStName as NearestCrossStreet,
	t.Town,
	Facility_Contact_Info,
	(Select top 1 InspectionDate from InterconnectionTests it where f.recordID = it.facilityid order by 1 desc) as [InspectionDate],
	(Select top 1 cast(InspectionComments as varchar(2000)) from InterconnectionTests it where f.recordID = it.facilityid order by 1 desc) as [InspectionComments]
FROM
	tblFacilities f
LEFT JOIN
    PublicWaterSupplies pws on pws.Id = f.PublicWaterSupplyId
LEFT JOIN
    OperatingCenters oc on oc.OPeratingCenterID = f.OPeratingCenterID
LEFT JOIN
    Streets s on s.StreetID = f.StreetID
LEFT JOIN
    Streets cs on cs.StreetID = f.CrossStreetID
LEFT JOIN
    Towns t on t.TownID = f.TownID

WHERE
	Interconnection = 1

                ">
        </asp:SqlDataSource>

    </asp:Panel>

</asp:Content>
