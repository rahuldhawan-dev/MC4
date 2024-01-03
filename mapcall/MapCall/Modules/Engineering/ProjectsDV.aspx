<%@ Page Theme="bender" Title="Projects-DV" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ProjectsDV.aspx.cs" Inherits="MapCall.Modules.Engineering.ProjectsDV" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
Projects-DV
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
            <table style="text-align:left;" border="0">
                <mmsi:DataField runat="server" ID="dfOpCode" DataType="DropDownList"
                    HeaderText="OpCode : "
                    DataFieldName="OpCode"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                    SelectCommand="SELECT DISTINCT [OperatingCenterCode] AS [Txt], [OperatingCenterCode] AS [Val] FROM [OperatingCenters] ORDER BY 1" />
                <mmsi:DataField runat="server" ID="dfPWSID" HeaderText="PWSID: "
                    DataType="DropDownList"
                    DataFieldName="RecordID"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT DISTINCT [PWSID] AS [txt], [RecordID] AS val FROM [PublicWaterSupplies]"
                />
                <mmsi:DataField runat="server" ID="dfTown" DataType="DropDownList"
                    HeaderText="Town : "
                    DataFieldName="Town"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT
                            [Town] AS [Txt],
                            [Town] AS [Val]
                        FROM
                            [Towns]" />
                <mmsi:DataField runat="server" ID="dfProjectNumber" DataType="String" DataFieldName="ProjectNumber" HeaderText="Project # :" />
                <mmsi:DataField runat="server" ID="dfPPWorkorder" DataType="String" DataFieldName="PPWorkorder" HeaderText="PPWorkorder :" />
                <mmsi:DataField runat="server" ID="DataField4" HeaderText="Category: "
                    DataType="DropDownList"
                    DataFieldName="Category"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT DISTINCT LookupValue AS [txt], LookupValue AS val FROM Lookup where LookupType = 'Category'"
                />
                <mmsi:DataField runat="server" ID="DataField7" HeaderText="Asset Category: "
                    DataType="DropDownList"
                    DataFieldName="AssetCategory"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT DISTINCT LookupValue AS [txt], LookupValue AS val FROM Lookup where LookupType = 'AssetCategory'"
                />
                <mmsi:DataField runat="server" ID="DataField1" HeaderText="Phase: "
                    DataType="DropDownList"
                    DataFieldName="Phase"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT DISTINCT LookupValue AS [txt], LookupValue AS val FROM Lookup where LookupType = 'Phase'"
                />
                <mmsi:DataField runat="server" ID="DataField2" DataType="NumberRange" DataFieldName="EstProjectCost" HeaderText="Estimated Project Cost :" />
                <mmsi:DataField runat="server" ID="DataField3" DataType="Boolean" DataFieldName="ProjectFlagged" HeaderText="Project Flagged :" />
                <mmsi:DataField runat="server" ID="DataField5" DataType="Date" DataFieldName="ForecastedInServiceDate" HeaderText="Forecasted In Service Date :" />
                <mmsi:DataField runat="server" ID="DataField6" DataType="Date" DataFieldName="InServiceDate" HeaderText="In Service Date :" />                
                 
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
        <asp:Button runat="server" ID="btnMap" Visible="true" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount" />
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            DataKeyNames="ProjectID" AllowSorting="true" AllowPaging="true" PageSize="500">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="SELECT
                b.[OperatingCenterCode] AS [OpCode],
	            l2.[LookupValue] AS [ProjectCategory],
	            p.[ProjectNumber],
	            l3.[LookupValue] AS [ProjectStatus],
	            t1.[BU] AS [BU],
	            p.[ProjectDescription],
	            p.[ProjectObstacles],
	            p.[ProjectRisks],
	            p.[ProjectApproach],
	            p.[ProjectDurationMonths],
	            p.[EstProjectCost],
	            cast(t7.[Last_Name] as varchar(255)) + cast(t7.[First_Name] as varchar(255)) + cast(t7.[EmployeeID] as varchar(255))AS [ProjectManager],
	            cast(t8.[PWSID] as varchar(255)) + cast(t8.[System] as varchar(255))AS [PWSID],
	            p.[StreetName],
	            t9.[Town] AS [Town],
	            p.[CreatedBy],
	            l1.[LookupValue] AS [Phase],
	            p.[MISDates],
	            p.[COE],
	            p.[ForecastedInServiceDate],
	            p.[InServiceDate],
	            p.[PPWorkOrder],
	            l5.LookupValue as [Category],
	            l4.LookupValue as [AssetCategory],
	            p.[ProjectID]
            FROM
	            [projectsDV] AS p
	        LEFT JOIN 
	            [OperatingCenters] as b 
	        ON 
	            b.[OperatingCenterID] = p.[OpCode]	        
            LEFT JOIN
	            [BusinessUnits] AS t1
            ON
	            p.[BU] = t1.[BusinessUnitID]
            LEFT JOIN
	            [Coordinates] AS c
            ON
	            p.[CoordinateID] = c.[CoordinateID]
            LEFT JOIN
	            [Lookup] AS l1
            ON
	            p.[Phase] = l1.[LookupID]
            LEFT JOIN
	            [Lookup] AS l2
            ON
	            p.[ProjectCategory] = l2.[LookupID]
            LEFT JOIN
	            [Lookup] AS l5
            ON
	            p.[category] = l5.[LookupID]
            LEFT JOIN
	            [Lookup] AS l4
            ON
	            p.[AssetCategory] = l4.[LookupID]
            LEFT JOIN
	            [tblEmployee] AS t7
            ON
	            p.[ProjectManager] = t7.[tblEmployeeID]
            LEFT JOIN
	            [Lookup] AS l3
            ON
	            p.[ProjectStatus] = l3.[LookupID]
            LEFT JOIN
	            [PublicWaterSupplies] AS t8
            ON
	            p.[PWSID] = t8.[RecordId]
            LEFT JOIN
	            [Towns] AS t9
            ON
	            p.[Town] = t9.[TownID];" />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName="ProjectsDV"
            DataElementParameterName="ProjectID"
            DataElementTableName="ProjectsDV"
            AllowDelete="true"
            OnPreInit="DataElement1_PreInit"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="164" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="164" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
    </asp:Panel>
</asp:Content>
