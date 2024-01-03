<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="FacilityInspectionReport.aspx.cs" Inherits="MapCall.Modules.HealthAndSafety.FacilityInspectionReport" %>

<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
Facility Inspection Report
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
            <table style="text-align:left;" border="0">
                <mmsi:DataField runat="server" ID="dfDateFIR" DataType="Date" DataFieldName="DateFIR" HeaderText="Date : " />
                <mmsi:DataField runat="server" ID="dfFacility" DataType="DropDownList" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    DataFieldName="FacilityID" HeaderText="Facility : " SelectCommand="SELECT DISTINCT IsNull(FacilityID + ' - ', '') + IsNull(FacilityName, '') AS [Txt], RecordID AS [Val] FROM [tblFacilities];" />
              
                <tr>
                    <td></td>
                    <td>
                        <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                        <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                        <asp:Button runat="server" ID="btnAdd" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <%--<asp:Button runat="server" ID="btnMap" Visible="False" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />--%>
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount" />
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            DataKeyNames="FIRID" AllowSorting="true" AllowPaging="true" PageSize="500">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="SELECT
	                f.[DateFIR],
	                cast(t.[FacilityID] as varchar(255)) + ' - ' + cast(t.[FacilityName] as varchar(255)) AS [Facility],
	                l.[LookupValue] AS [FacilityRating],
	                f.[FIRPerformedBy],
	                l1.[LookupValue] AS [FIRRating],
	                f.[FIRID],
	                f.[Facility] AS [FacilityID]
                FROM
	                [FacilityInspectionReport] AS f
                LEFT JOIN
	                [tblFacilities] AS t
                ON
	                f.[Facility] = t.[RecordId]
                LEFT JOIN
	                [Lookup] AS l
                ON
	                f.[FacilityRating] = l.[LookupID]
                LEFT JOIN
	                [Lookup] AS l1
                ON
	                f.[FIRRating] = l1.[LookupID];" />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName="FacilityInspectionReport"
            DataElementParameterName="FIRID"
            DataElementTableName="FacilityInspectionReport"
            OnPreInit="DataElement1_PreInit"
         />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="84" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="84" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
    </asp:Panel>
</asp:Content>