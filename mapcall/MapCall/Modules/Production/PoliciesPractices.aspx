<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="PoliciesPractices.aspx.cs" Inherits="MapCall.Modules.Production.PoliciesPractices" Title="Untitled Page" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Policies-Practices
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
                HeaderText="PP Status : "
                DataType="DropDownList"
                DataFieldName="PP_Status"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'PP_Status' order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField3"
                HeaderText="PP Category : "
                DataType="DropDownList"
                DataFieldName="PP_Category"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'PP_Category' order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField4"
                HeaderText="PP Sub Category : "
                DataType="DropDownList"
                DataFieldName="PP_Sub_Category"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'PP_Sub_Category' order by 2"
            />
            <mmsi:DataField runat="server" ID="DataField5"
                HeaderText="Functional Area : "
                DataType="DropDownList"
                DataFieldName="Functional_Area"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Functional_Area' AND [TableName] = 'tblPoliciesPractices' order by 2"
            />

            <mmsi:DataField runat="server" ID="DataField6" DataType="Date" DataFieldName="Date_Approved" HeaderText="Date Approved : " />
            <mmsi:DataField runat="server" ID="DataField7" DataType="Date" DataFieldName="Date_Issued" HeaderText="Date Issued : " />
                                    
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
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="PP_ID"
            AllowSorting="true" AllowPaging="true" PageSize="500"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="
SELECT 
	[PP_ID]
	,(Select LookupValue from Lookup where LookupID = PP_Status) as [Status]
	,(Select LookupValue from Lookup where LookupID = PP_Category) as [Category]
	,(Select LookupValue from Lookup where LookupID = PP_Sub_Category) as [SubCategory]
	,[PP_Description],[PP_Summary]
	,(Select LookupValue from Lookup where LookupID = Functional_Area) as [FunctionArea]
	,[Regulation_ID]
	,(select OperatingCenterCode from OperatingCenters where OperatingCenterID = opCode) as OpCode
	,[Facility_ID]
	,[Equipment_ID]
	,[Date_Approved]
	,[Date_Issued]
	,[Revision]
	,[Review_Frequency_Days]
	,[PSM_TCPA]
	,[DPCC]
	,[OSHA]
	,[Company]
	,[SOX]
	,[PP_Status]
	,[PP_Category]
	,[PP_Sub_Category]
	,[Functional_Area]
	,[OpCode]
from 
	tblPoliciesPractices PP


            ">
        </asp:SqlDataSource>
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "PP_Status"
            DataElementParameterName = "PP_ID"
            DataElementTableName = "tblPoliciesPractices"
            AllowDelete="true"
            OnDataBinding="DataElement1_DataBinding"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="68" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="68" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    
</asp:Content>