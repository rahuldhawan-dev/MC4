<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="BusPerformanceReportingRequirements.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.BusPerformanceReportingRequirements" Title="Business Performance Reporting Requirements" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Business Performance Reporting Requirements
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
Business Performance Reporting Requirements
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfInitiativeID"
                HeaderText="Initiative ID : "
                DataType="DropDownList"
                DataFieldName="InitiativeID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct Initiative_ID as Val, Initiative_ID as txt from tblBusinessPerformance_ReportingRequirements order by 2"
            />
            <mmsi:DataField runat="server" ID="dfKPIID"
                HeaderText="KPI ID : "
                DataType="DropDownList"
                DataFieldName="KPIID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct KPI_ID as Val, KPI_ID as txt from tblBusinessPerformance_ReportingRequirements order by 2"
            />
            <mmsi:DataField runat="server" ID="dfReportCategory"
                HeaderText="Report Category : "
                DataType="DropDownList"
                DataFieldName="ReportCategoryID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Report_Category' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfReportOwner"
                HeaderText="Report Owner : "
                DataType="DropDownList"
                DataFieldName="Report_Owner"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct Report_Owner as Val, Report_Owner as txt from tblBusinessPerformance_ReportingRequirements order by 2"
            />
           
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
        <br />
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnMap" Visible="False" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="Reporting_ID"
            AllowSorting="true" AllowPaging="true" PageSize="500"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="SELECT
                t1.[Initiative_Summary] AS [Initiative_ID],
	            t2.[KPI_Measurement] AS [KPI_ID],
	            l.[LookupValue] AS [Report_Category],
	            t.[Report_Summary],
	            t.[Report_Information_Source],
	            t.[Report_Owner],
	            l1.[LookupValue] AS [Report_Grouping],
	            t.[Report_Sequence],
	            t.[Prepared_By],
	            t.[Sent_To],
	            l2.[LookupValue] AS [Reporting_Frequency],
	            t.[Due_By],
	            t.[Reporting_ID],
                t.[Initiative_ID] AS [InitiativeID],
                t.[KPI_ID] AS [KPIID],
                t.[Report_Category] AS [ReportCategoryID]
            FROM
	            [tblBusinessPerformance_ReportingRequirements] AS t
            LEFT JOIN
	            [tblBusinessPerformance_Initiatives] AS t1
            ON
	            t.[Initiative_ID] = t1.[Initiative_ID]
            LEFT JOIN
	            [tblBusinessPerformance_KPI] AS t2
            ON
	            t.[KPI_ID] = t2.[KPI_ID]
            LEFT JOIN
	            [Lookup] AS l
            ON
	            t.[Report_Category] = l.[LookupID]
            LEFT JOIN
	            [Lookup] AS l1
            ON
	            t.[Report_Grouping] = l1.[LookupID]
            LEFT JOIN
	            [Lookup] AS l2
            ON
	            t.[Reporting_Frequency] = l2.[LookupID];">
        </asp:SqlDataSource>
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "Report_Category"
            DataElementParameterName = "Reporting_ID"
            DataElementTableName = "tblBusinessPerformance_ReportingRequirements"
            AllowDelete="true"
            OnDataBinding="DataElement1_DataBinding"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="62" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="62" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    
</asp:Content>