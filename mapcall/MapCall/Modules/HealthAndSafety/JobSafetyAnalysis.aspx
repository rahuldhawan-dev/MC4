<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="JobSafetyAnalysis.aspx.cs" Inherits="MapCall.Modules.HealthAndSafety.JobSafetyAnalysis" %>

<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
Job Safety Analysis
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
            <table style="text-align:left;" border="0">
                <mmsi:DataField runat="server" ID="dfDateJSA" DataType="Date" DataFieldName="DateJSA" HeaderText="Date : " />
                <mmsi:DataField runat="server" ID="dfOpCode" DataType="DropDownList" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    DataFieldName="OpCodeID" HeaderText="OpCode : " SelectCommand="SELECT DISTINCT [OperatingCenterCode] AS [Txt], [OperatingCenterID] AS [Val] FROM [OperatingCenters];" />
                <mmsi:DataField runat="server" ID="dfFunctionalArea" DataType="DropDownList" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    DataFieldName="FunctionalAreaID" HeaderText="Functional Area : " SelectCommand="SELECT DISTINCT [LookupValue] AS [Txt], [LookupID] AS [Val] FROM [Lookup] WHERE [LookupType] = 'FunctionalArea' AND [TableName] = 'JobSafetyAnalysis' ORDER BY 1" />
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
            DataKeyNames="JSAID" AllowSorting="true" AllowPaging="true" PageSize="500">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="SELECT
	                j.[DateJSA],
	                l.[LookupValue] AS [FunctionalArea],
	                j.[JSADescriptionOfWork],
	                t.[OperatingCenterCode] AS [OpCode],
	                l1.[LookupValue] AS [SafetyRating],
	                t1.[PP_ID] AS [SOP],
	                j.[JSAID],
	                j.[FunctionalArea] AS [FunctionalAreaID],
	                j.[OpCode] AS [OpCodeID]
                FROM
	                [JobSafetyAnalysis] AS j
                LEFT JOIN
	                [Lookup] AS l
                ON
	                j.[FunctionalArea] = l.[LookupID]
                LEFT JOIN
	                [OperatingCenters] AS t
                ON
	                j.[OpCode] = t.[OperatingCenterID]
                LEFT JOIN
	                [Lookup] AS l1
                ON
	                j.[SafetyRating] = l1.[LookupID]
                LEFT JOIN
	                [tblSOP] AS t1
                ON
	                j.[SOP] = t1.[SOP_ID];" />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName="JobSafetyAnalysis"
            DataElementParameterName="JSAID"
            DataElementTableName="JobSafetyAnalysis"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="155" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="155" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
    </asp:Panel>
</asp:Content>