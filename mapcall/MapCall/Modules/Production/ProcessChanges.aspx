<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ProcessChanges.aspx.cs" Inherits="MapCall.Modules.Production.ProcessChanges" Title="Process Changes" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Process Changes
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="DataField1"
                HeaderText="OpCode : "
                DataType="DropDownList"
                DataFieldName="OpCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select OperatingCenterID as Val, OperatingCenterCode as txt from OperatingCenters order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField2"
                HeaderText="Facility : "
                DataType="DropDownList"
                DataFieldName="FacilityID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT RecordID as val, isNull(facilityID,'') + ',' + isNull(facilityName,'') as txt from tblFacilities order by 
                    FacilityId--Left(facilityID, charindex ('-',FacilityID)-1),cast(substring(facilityID,1+ charindex ('-',FacilityID),10) as int)"
            />
            <mmsi:DataField runat="server" ID="DataField3"
                HeaderText="Functional Area : "
                DataType="DropDownList"
                DataFieldName="Functional_Area"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Functional_Area' AND [TableName] = 'tblProcessChanges' order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField4"
                HeaderText="Process Change Category : "
                DataType="DropDownList"
                DataFieldName="Process_Change_Category"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Process_Change_Category' order by 2"
            />
            <mmsi:DataField runat="server" ID="Datafield5" DataType="Date" DataFieldName="Start_Date" HeaderText="Start Date : " />
            <mmsi:DataField runat="server" ID="Datafield6" DataType="Date" DataFieldName="End_Date" HeaderText="End Date : " />
                                    
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
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="Process_Change_ID"
            AllowSorting="true" AllowPaging="true" PageSize="500"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="
                SELECT * from tblProcessChanges
            ">
        </asp:SqlDataSource>
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "Process_Change_Category"
            DataElementParameterName = "Process_Change_ID"
            DataElementTableName = "tblProcessChanges"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="57" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="57" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    
</asp:Content>