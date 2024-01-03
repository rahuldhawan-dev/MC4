<%@ Page Theme="bender" Title="Project Updates - DV" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ProjectUpdatesDV.aspx.cs" Inherits="MapCall.Modules.Engineering.ProjectUpdatesDV" %>

<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
Project-DV Updates
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
            <table style="text-align:left;" border="0">
                <mmsi:DataField runat="server" ID="dfProject" DataFieldName="ProjectID" DataType="DropDownList"
                    HeaderText="Project : " ConnectionString="<%$ ConnectionStrings:MCProd %>" SelectCommand="SELECT cast(p.[ProjectID] as varchar) + ' - ' + l.[LookupValue] AS [Txt], p.[ProjectID] AS [Val] FROM [ProjectsDV] AS p INNER JOIN [Lookup] AS l ON p.[ProjectCategory] = l.[LookupID];" />
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
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount" />
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            DataKeyNames="ProjectUpdateID" AllowSorting="true" AllowPaging="true" PageSize="500">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" SelectCommand="SELECT
	            cast(p1.[ProjectID] as varchar) + ' - ' + l.[LookupValue] AS [Project],
	            p.[DateOfUpdate],
	            p.[TotalExpense],
	            p.[Update],
	            p.[ProjectUpdateID],
                p.[ProjectID]
            FROM
	            [ProjectUpdatesDV] AS p
            LEFT JOIN
	            [ProjectsDV] AS p1
            ON
	            p.[ProjectID] = p1.[ProjectID]
            LEFT JOIN
                [Lookup] AS l
            ON
                l.LookupID = p1.[ProjectCategory];" />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName="ProjectUpdatesDV"
            DataElementParameterName="ProjectUpdateID"
            DataElementTableName="ProjectUpdatesDV"
            OnPreInit="DataElement1_PreInit"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="77" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="77" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
    </asp:Panel>
</asp:Content>