<%@ Page Title="Meter Profiles" Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="MeterProfiles.aspx.cs" Inherits="MapCall.Modules.FieldServices.MeterProfiles" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
    Meter Profiles
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
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
        <asp:HiddenField runat="server" ID="HiddenField1" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnMap" Visible="false" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="MeterProfileID" AllowSorting="true"
            AutoGenerateColumns="true"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
           SelectCommand="
SELECT 
	[MeterProfileID],
	[ProfileName],
	mt.Description as MeterType,
	ms.Size as MeterSize,
	mm.Description as MeterManufacturer,
	[EstimatedMaximumFlow],
	uom.Description as [UnitOfMeasurement],
	[AWWALowerLimitPercentage],
	[AWWAUpperLimitPercentage],
	[MeterTestComments]
FROM
	[MeterProfiles] mp
LEFT JOIN
	MeterTypes mt
ON
	mt.MeterTypeID = mp.MeterTypeID
LEFT JOIN
	MeterSizes ms
ON
	ms.MeterSizeID = mp.MeterSizeID
LEFT JOIN
	MeterManufacturers mm
ON
	mm.MeterManufacturerID = mp.MeterManufacturerID
LEFT JOIN
	UnitsOfMeasure uom
ON
	uom.UnitOfMeasureID = mp.UnitOfMeasureID
           "
        >
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" 
            OnItemInserted="DataElement1_ItemInserted" 
            DataElementName="MeterProfiles"
            DataElementParameterName="MeterProfileID"
            DataElementTableName="MeterProfiles"
            ConnectionString="MCProd"
            AllowDelete="true"
            OnPreInit="DataElement1_PreInit"
            OnDataBinding="DataElement1_DataBinding"
        >
        </mmsi:DataElement>
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="129" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="129" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button4" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="Button5" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>
   
</asp:Content>