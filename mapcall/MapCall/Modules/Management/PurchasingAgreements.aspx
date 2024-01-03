<%@ Page Title="Purchasing Agreements" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="PurchasingAgreements.aspx.cs" Inherits="MapCall.Modules.Management.PurchasingAgreements" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Purchasing Agreements
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfAgreementStartDate" DataType="Date" DataFieldName="AgreementStartDate" HeaderText="Agreement Start Date:" />
            <mmsi:DataField runat="server" ID="dfAgreementEndDate" DataType="Date" DataFieldName="AgreementEndDate" HeaderText="Agreement End Date:" />
            <mmsi:DataField runat="server" ID="dfContractStatus" DataType="DropDownList" 
                DataFieldName="ContractStatus" HeaderText="Contract Status:" 
                SelectCommand="Select LookupID as val, LookupValue as txt from Lookup Where LookupType='ContractStatus' and tableName='PurchasingAgreements'"    
                ConnectionString="<%$ ConnectionStrings:MCProd %>" />
            <mmsi:DataField runat="server" ID="dfCompany" DataType="DropDownList" 
                DataFieldName="Company" HeaderText="Company:" 
                SelectCommand="Select LookupID as val, LookupValue as txt from Lookup Where LookupType='Company' and tableName='PurchasingAgreements'"    
                ConnectionString="<%$ ConnectionStrings:MCProd %>" />
            <mmsi:DataField runat="server" ID="dfFunctionalArea" DataType="DropDownList" 
                DataFieldName="FunctionalArea" HeaderText="Functional Area:" 
                SelectCommand="Select LookupID as val, LookupValue as txt from Lookup Where LookupType='FunctionalArea' and tableName='PurchasingAgreements'"    
                ConnectionString="<%$ ConnectionStrings:MCProd %>" />
            <mmsi:DataField runat="server" ID="dfCategory" DataType="DropDownList" 
                DataFieldName="Category" HeaderText="Category:" 
                SelectCommand="Select LookupID as val, LookupValue as txt from Lookup Where LookupType='Category' and tableName='PurchasingAgreements'"    
                ConnectionString="<%$ ConnectionStrings:MCProd %>" />
            <mmsi:DataField runat="server" ID="dfSupplierName" DataType="String" DataFieldName="SupplierName" HeaderText="Supplier Name:" />
            <mmsi:DataField runat="server" ID="dfContractOwnerLastName" DataType="String" DataFieldName="ContractOwnerLastName" HeaderText="Contract Owner Last Name:" />                     
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
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="PurchasingAgreementID" AllowSorting="true"
            AutoGenerateColumns="true"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
                SELECT [PurchasingAgreementID]
                    ,l4.lookupvalue as ContractStatusText
                    ,l1.lookupvalue as CompanyText
                    ,l2.lookupvalue as FunctionalAreaText
                    ,l3.lookupvalue as CategoryText
                    ,[SupplierName]
                    ,[AgreementName]
                    ,[AgreementSummary]
                    ,[AmendmentNumber]
                    ,[StrategicSourcingProjectTrackingNumber]
                    ,[AgreementStartDate]
                    ,[AgreementEndDate]
                    ,[EstimatedAnnualSpend]
                    ,[ContractOwnerLastName]
                    ,[NJ1]
                    ,[NJ2]
                    ,[NJ3]
                    ,[NJ4]
                    ,[NJ5]
                    ,[NJ6]
                    ,[NJ7]
                    ,[NJ8]
                    ,[NJ9]
                    ,[EW1]
                    ,[EW2]
                    ,[EW3]
                    ,[EW4]
                    ,[LWC]
                    ,[FolderLocation]
                    ,[ContractStatus]
                    ,[Company]
                    ,[FunctionalArea]
                    ,[Category]
                    
                FROM 
                    [PurchasingAgreements] pa
                left join
                    Lookup l1
                on
                    l1.LookupID = pa.Company
                left join
                    Lookup l2
                on
                    l2.LookupID = pa.FunctionalArea
                left join
                    Lookup l3
                on
                    l3.LookupID = pa.Category
                left join
                    Lookup l4
                on
                    l4.LookupID = pa.ContractStatus
            "
        >
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" 
            OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "Purchasing Agreement"
            ConnectionString="MCProd"
            DataElementParameterName = "PurchasingAgreementID"
            DataElementTableName = "PurchasingAgreements"
            AllowDelete="true"
            OnDataBinding="DataElement1_DataBinding"
        >
        </mmsi:DataElement>
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="103" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="103" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

</asp:Content>
