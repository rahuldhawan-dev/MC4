<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="UtilityAccounts.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.UtilityAccounts" Title="Untitled Page" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Utility Accounts
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="DataField9" 
                HeaderText="Utility Rate : " 
                DataType="DropDownList" 
                DataFieldName="UtilRateID" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select distinct UtilRateID as Val, isNull(TypeofUtility,'') + ',' + isNull(UtilitySupplier,'') + ',' + isNull(UtilityRateStructure,'') as txt from tblUtilityRates order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField7" DataType="Date" DataFieldName="DateCreated" HeaderText="Date Created : " />
            
            <mmsi:DataField runat="server" ID="DataField1" 
                HeaderText="OpCode : " 
                DataType="DropDownList" 
                DataFieldName="OpCode" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select OperatingCenterCode as Val, OperatingCenterCode as txt from OperatingCenters order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField2"
                HeaderText="Facility ID : "
                DataType="DropDownList"
                DataFieldName="FacilityID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT RecordID as val, isNull(facilityID,'') + ',' + isNull(facilityName,'') as txt from tblFacilities order by FacilityId--Left(facilityID, charindex ('-',FacilityID)-1),cast(substring(facilityID,1+ charindex ('-',FacilityID),10) as int)"
            />
            <mmsi:DataField runat="server" ID="DataField3"
                HeaderText="Facility Name : "
                DataType="DropDownList"
                DataFieldName="FacilityName"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct FacilityName as Val, FacilityName as txt from tblUtilityAccounts order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField4"
                HeaderText="Facility Location : "
                DataType="DropDownList"
                DataFieldName="FacilityLocation"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct FacilityLocation as Val, FacilityLocation as txt from tblUtilityAccounts order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField5"
                HeaderText="Utility Account Number : "
                DataType="DropDownList"
                DataFieldName="UtilityAcctNumber"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct UtilityAcctNumber as Val, UtilityAcctNumber as txt from tblUtilityAccounts order by 2"
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
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="RecordID"
            AllowSorting="true" AllowPaging="true" PageSize="500"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="SELECT * from tblUtilityAccounts" />
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "FacilityName"
            DataElementParameterName = "RecordID"
            DataElementTableName = "tblUtilityAccounts"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="66" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="66" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    
</asp:Content>
