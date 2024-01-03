<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="UtilityRates.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.UtilityRates" Title="Untitled Page" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Utility Rates
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="DataField1" 
                HeaderText="Type of Utility : " 
                DataType="DropDownList" 
                DataFieldName="TypeOfUtility" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select Distinct TypeOfUtility as Val, TypeOfUtility as txt from tblUtilityRates order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField2"
                HeaderText="Utility Supplier : "
                DataType="DropDownList"
                DataFieldName="UtilitySupplier"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select Distinct UtilitySupplier as Val, UtilitySupplier as txt from tblUtilityRates order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField3" 
                HeaderText="Utility Rate Structure : " 
                DataType="DropDownList" 
                DataFieldName="UtilityRateStructure" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select Distinct UtilityRateStructure as Val, UtilityRateStructure as txt from tblUtilityRates order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField4" DataType="Date" DataFieldName="Start_Date" HeaderText="Start Date : " />
            <mmsi:DataField runat="server" ID="DataField5" DataType="Date" DataFieldName="End_Date" HeaderText="End Date : " />
                                    
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
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="UtilRateID"
            AllowSorting="true" AllowPaging="true" PageSize="500"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="SELECT * from tblUtilityRates" />
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "TypeOfUtility"
            DataElementParameterName = "UtilRateID"
            DataElementTableName = "tblUtilityRates"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="67" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="67" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    
</asp:Content>
