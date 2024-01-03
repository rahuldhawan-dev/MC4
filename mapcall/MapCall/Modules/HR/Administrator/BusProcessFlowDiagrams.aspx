<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="BusProcessFlowDiagrams.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.BusProcessFlowDiagrams" Title="Business Process Flow Diagrams" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Business Process Flow Diagrams
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
Business Process Flow Diagrams
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfOpCode"
                HeaderText="OpCode : "
                DataType="DropDownList"
                DataFieldName="OpCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select OperatingCenterCode as Val, OperatingCenterCode as txt from OperatingCenters order by 2"
            />
            <mmsi:DataField runat="server" ID="dfFunctionalArea"
                HeaderText="Functional Area : "
                DataType="DropDownList"
                DataFieldName="FunctionalAreaID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Functional_Area' AND [TableName] = 'tblBusinessProcess_FlowDiagrams' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfProcessFlowStatus"
                HeaderText="Process Flow Status : "
                DataType="DropDownList"
                DataFieldName="ProcessFlowStatusID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Process_Flow_Status' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfProcessFlowGrouping"
                HeaderText="Process Flow Grouping : "
                DataType="DropDownList"
                DataFieldName="Process_Flow_Grouping"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct Process_Flow_Grouping as Val, Process_Flow_Grouping as txt from tblBusinessProcess_FlowDiagrams order by 2"
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
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="Process_Flow_ID"
            AllowSorting="true" AllowPaging="true" PageSize="500"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="SELECT
	                t1.[OperatingCenterCode] AS [OpCode],
	                l.[LookupValue] AS [Functional_Area],
	                l1.[LookupValue] AS [Process_Flow_Status],
	                t.[Process_Flow_Grouping],
	                t.[Process_Flow_Description],
	                t.[Sequence],
	                t.[Risks],
	                t.[Opportunities],
	                t.[Process_Flow_ID],
                    t.[Functional_Area] AS [FunctionalAreaID],
                    t.[Process_Flow_Status] AS [ProcessFlowStatusID]
                FROM
	                [tblBusinessProcess_FlowDiagrams] AS t
                LEFT JOIN
	                [Lookup] AS l
                ON
	                t.[Functional_Area] = l.[LookupID]
                LEFT JOIN
	                [OperatingCenters] AS t1
                ON
	                t.[OpCode] = t1.[OperatingCenterID]
                LEFT JOIN
	                [Lookup] AS l1
                ON
	                t.[Process_Flow_Status] = l1.[LookupID];">
        </asp:SqlDataSource>
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "Process_Flow_Status"
            DataElementParameterName = "Process_Flow_ID"
            DataElementTableName = "tblBusinessProcess_FlowDiagrams"
            AllowDelete="true"
            OnDataBinding="DataElement1_DataBinding"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="64" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="64" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    
</asp:Content>