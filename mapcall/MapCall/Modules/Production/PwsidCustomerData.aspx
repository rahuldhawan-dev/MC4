<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="PwsidCustomerData.aspx.cs" Inherits="MapCall.Modules.Production.PwsidCustomerData" Title="PWSID Customer Data" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.HR" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
PWSID Customer Data
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="ddlPWSID" 
                HeaderText="PWSID ID : " 
                DataType="DropDownList" 
                DataFieldName="PWSID_ID" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select Id as Val, PWSID as Txt from PublicWaterSupplies order by 2"
            />
            <mmsi:DataField runat="server" ID="ddlOpCode" 
                HeaderText="OpCode : " 
                DataType="DropDownList" 
                DataFieldName="OpCode" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select OperatingCenterCode as Val, OperatingCenterCode as txt from OperatingCenters order by 2"
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
        <asp:Label runat="server" ID="lblRecordCount" />
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="CustomerDataID"
            AllowSorting="true" AllowPaging="true" PageSize="500"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="SELECT c.[CustomerDataID], c.[PWSID] AS [PWSID_ID], oc.OperatingCenterCode as [OpCode], p.[PWSID] AS [PWSID], p.[System], pwss.Description as [Status], c.[Number_Customers],
            c.[Population_Served], c.[Notes] 
            FROM    
                tblPWSID_Customer_Data AS c 
            LEFT JOIN 
                PublicWaterSupplies AS p ON c.PWSID = p.Id 
            LEFT JOIN 
	            OperatingCentersPublicWaterSupplies ocpws on ocpws.PublicWaterSupplyId = p.Id
            LEFT JOIN 
	            OperatingCenters oc on oc.OperatingCenterID = ocpws.OperatingCenterID 
            LEFT JOIN 
                PublicWaterSupplyStatuses pwss on pwss.Id = p.StatusId" />
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "CustomerDataID"
            DataElementParameterName = "CustomerDataID"
            DataElementTableName = "tblPWSID_Customer_Data"
        >
            <mmsi:DataElementField DataFieldName="PWSID" HeaderName="PWSID" Type="Int" runat="server" />
            <mmsi:DataElementField DataFieldName="Number_Customers" HeaderName="Number of Customers" Type="Int" runat="server" />
            <mmsi:DataElementField DataFieldName="Population_Served" HeaderName="Population Served" Type="Int" runat="server" />
            <mmsi:DataElementField DataFieldName="Notes" HeaderName="Notes" Type="Text" runat="server" />
            <mmsi:DataElementField DataFieldName="UpdatedAt" HeaderName="Date Updated" Type="DateTime" runat="server" />
        </mmsi:DataElement>
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="54" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="54" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    
</asp:Content>