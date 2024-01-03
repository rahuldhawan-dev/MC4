<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="CriticalCustomers.aspx.cs" Inherits="MapCall.Modules.Customer.CriticalCustomers" %>

<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
Critical Customers
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Label runat="server" ID="lblPermissionErrors" />

    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
            <table style="text-align:left;" border="0">
                <mmsi:DataField runat="server" ID="dfCustomerCategory" DataType="DropDownList" DataFieldName="CustomerCategory"
                    HeaderText="Customer Category : " ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT LookupValue as [Txt], LookupID as [Val] FROM [Lookup] WHERE LookupType = 'CustomerCategory' AND TableName = 'CriticalCustomers'" />
                <mmsi:DataField runat="server" ID="dfTown" DataType="DropDownList" HeaderText="Town : "
                    DataFieldName="Town" ConnectionString="<%$ ConnectionStrings:MCProd %>" SelectCommand="SELECT [Town] AS [Txt], [Town] AS [Val] FROM [Towns]" />
                <mmsi:DataField runat="server" ID="dfEstDailyVolumeGallons" DataType="NumberRange" DataFieldName="EstDailyVolumeGallons" HeaderText="Est. Daily Volume (Gallons) : " />
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
            DataKeyNames="CriticalCustomerID" AllowSorting="true" AllowPaging="true" PageSize="500">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="SELECT
	                l.[LookupValue] AS [CustomerCategoryDescription],
	                c.[PremiseNumber],
	                c.[ServiceNumber],
	                c.[StreetNumber],
	                c.[StreetName],
	                t.[Town] AS [Town],
	                c.[ContactName],
	                c.[ContactPhoneNumber],
	                c.[ContactCellNumber],
	                c.[EstDailyVolumeGallons],
	                c.[EmergencyInstructions],
	                c.[CriticalCustomerID],
                    c.[CustomerCategory] AS [CustomerCategory],
                    c.[Town] AS [TownID]
                FROM
	                [CriticalCustomers] AS c
                LEFT JOIN
	                [Lookup] AS l
                ON
	                c.[CustomerCategory] = l.[LookupID]
                LEFT JOIN
	                [Towns] AS t
                ON
	                c.[Town] = t.[TownID];" />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName="CriticalCustomers"
            DataElementParameterName="CriticalCustomerID"
            DataElementTableName="CriticalCustomers"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="75" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="75" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
    </asp:Panel>
</asp:Content>
