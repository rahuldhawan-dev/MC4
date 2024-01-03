<%@ Page Title="Inactive Services" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="InactiveServices.aspx.cs" Inherits="MapCall.Modules.Data.Services.InactiveServices" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Inactive Services
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />

    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfFireService" DataType="BooleanDropDown" DataFieldName="FireService" HeaderText="Fire Service" />
            <mmsi:DataField runat="server" ID="dfDomestic" DataType="BooleanDropDown" DataFieldName="Domestic" HeaderText="Domestic" />
            <mmsi:DataField runat="server" ID="dfIrrigation" DataType="BooleanDropDown" DataFieldName="Irrication" HeaderText="Irrigation" />
            <mmsi:DataField runat="server" ID="dfCommercial" DataType="BooleanDropDown" DataFieldName="Commercial" HeaderText="Commericial" />
            
            <mmsi:DataField runat="server" ID="DataField2" DataType="String" DataFieldName="PremiseNumber" HeaderText="Premise #: " />
            <mmsi:DataField runat="server" id="ddlDistrictCode" DataType="DropDownList"
                HeaderText="District Code :"
                DataFieldName="DistrictCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select Distinct DistrictCode as val, DistrictCode as txt from InactiveServices order by 1"
            />  
            <mmsi:DataField runat="server" id="dfAreaCode" DataType="String" HeaderText="Area Code :" DataFieldName="AreaCode" />  
            <mmsi:DataField runat="server" id="dfAreaCodeDescription" DataType="String" HeaderText="Area Code Description :" DataFieldName="AreaCodeDescription" />  
            
            <mmsi:DataField runat="server" ID="DataField1" DataType="DropDownList" DataFieldName="ServiceCity" HeaderText="City: " 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct ServiceCity as val, ServiceCity as txt from InactiveServices order by ServiceCity asc"
            />            
            <mmsi:DataField runat="server" ID="dfOcc" DataType="BooleanDropDown" DataFieldName="Occupied" HeaderText="Occupied" />
            <mmsi:DataField runat="server" ID="dfInspection" DataType="Date" DataFieldName="InspectionDate" HeaderText="Inspection Date" />
            
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
        <asp:Button runat="server" ID="btnMap" Visible="false" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="InactiveServiceID" AllowSorting="true"
            AutoGenerateColumns="true"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
                Select 
                    *
                from 
                    InactiveServices
            "
        >
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" 
            OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "Inactive Service"
            ConnectionString="MCProd"
            DataElementParameterName = "InactiveServiceID"
            DataElementTableName = "InactiveServices"
            AllowDelete="true"
            OnPreInit="DataElement1_PreInit"
            OnDataBinding="DataElement1_DataBinding"
        >
        </mmsi:DataElement>
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="94" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="94" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>
    
</asp:Content>
