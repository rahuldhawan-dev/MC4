<%@ Page Title="Management Audits" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ManagementAudits.aspx.cs" Inherits="MapCall.Modules.BPU.ManagementAudits" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Management Audits
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfManagementAuditID" DataType="Integer" DataFieldName="ManagementAuditID" HeaderText="Management Audit ID:" />
            <mmsi:DataField runat="server" ID="dfAuditID" DataType="String" DataFieldName="AuditID" HeaderText="Audit ID:" />
            <mmsi:DataField runat="server" ID="dfAuditArea" DataType="String" DataFieldName="AuditArea" HeaderText="Audit Area:" />
            <mmsi:DataField runat="server" ID="dfSubAuditArea" DataType="String" DataFieldName="SubAuditArea" HeaderText="Sub Audit Area:" />
            <mmsi:DataField runat="server" ID="dfComplete" DataType="BooleanDropDown" DataFieldName="Complete" HeaderText="Complete:" />
            <mmsi:DataField runat="server" ID="dfConfidential" DataType="BooleanDropDown" DataFieldName="Confidential" HeaderText="Confidential:" />
            <mmsi:DataField runat="server" ID="dfPrivileged" DataType="BooleanDropDown" DataFieldName="Privileged" HeaderText="Privileged:" />
            <mmsi:DataField runat="server" ID="dfDateReceived" DataType="Date" DataFieldName="DateReceived" HeaderText="Date Received:" />
            <mmsi:DataField runat="server" ID="dfDateDue" DataType="Date" DataFieldName="DateDue" HeaderText="Date Due:" />
            <mmsi:DataField runat="server" ID="dfDateSubmitted" DataType="Date" DataFieldName="DateSubmitted" HeaderText="Date Submitted:" />
            <mmsi:DataField runat="server" ID="dfDRRating" DataType="DropDownList" 
                DataFieldName="DRRating" HeaderText="DR Rating:" 
                SelectCommand="Select LookupID as val, LookupValue as txt from Lookup Where LookupType='DRRating' and tableName='ManagementAudits'"    
                ConnectionString="<%$ ConnectionStrings:MCProd %>" />
            <mmsi:DataField runat="server" ID="dfAssignedTo" DataType="String" DataFieldName="AssignedTo" HeaderText="Assigned To:" />
            <%--Columns N  through  AH  check boxes--%>
            <mmsi:DataField runat="server" ID="dfParentCompany" DataType="BooleanDropDown" DataFieldName="ParentCompany" HeaderText="Parent Company:" />
            <mmsi:DataField runat="server" ID="dfExecutiveManagement" DataType="BooleanDropDown" DataFieldName="ExecutiveManagement" HeaderText="Executive Management:" />
            <mmsi:DataField runat="server" ID="dfExternalRelations" DataType="BooleanDropDown" DataFieldName="ExternalRelations" HeaderText="External Relations:" />
            <mmsi:DataField runat="server" ID="dfCommunications" DataType="BooleanDropDown" DataFieldName="Communications" HeaderText="Communications:" />
            <mmsi:DataField runat="server" ID="dfAcctAndFinance" DataType="BooleanDropDown" DataFieldName="AcctAndFinance" HeaderText="Acct And Finance:" />
            <mmsi:DataField runat="server" ID="dfOpFops" DataType="BooleanDropDown" DataFieldName="OpFops" HeaderText="Op-Fops:" />
            <mmsi:DataField runat="server" ID="dfOpProd" DataType="BooleanDropDown" DataFieldName="OpProd" HeaderText="Op-Prod:" />
            <mmsi:DataField runat="server" ID="dfOpWQ" DataType="BooleanDropDown" DataFieldName="OpWQ" HeaderText="Op-WQ:" />
            <mmsi:DataField runat="server" ID="dfOpEnviro" DataType="BooleanDropDown" DataFieldName="OpEnviro" HeaderText="Op-Enviro:" />
            <mmsi:DataField runat="server" ID="dfOpRisk" DataType="BooleanDropDown" DataFieldName="OpRisk" HeaderText="Op-Risk:" />
            <mmsi:DataField runat="server" ID="dfOpServices" DataType="BooleanDropDown" DataFieldName="OpServices" HeaderText="Op-Services:" />
            <mmsi:DataField runat="server" ID="dfEngineering" DataType="BooleanDropDown" DataFieldName="Engineering" HeaderText="Engineering:" />
            <mmsi:DataField runat="server" ID="dfHR" DataType="BooleanDropDown" DataFieldName="HR" HeaderText="HR:" />
            <mmsi:DataField runat="server" ID="dfCSC" DataType="BooleanDropDown" DataFieldName="CSC" HeaderText="CSC:" />
            <mmsi:DataField runat="server" ID="dfFRCC" DataType="BooleanDropDown" DataFieldName="FRCC" HeaderText="FRCC:" />
            <mmsi:DataField runat="server" ID="dfSSC" DataType="BooleanDropDown" DataFieldName="SSC" HeaderText="SSC:" />
            <mmsi:DataField runat="server" ID="dfBSC" DataType="BooleanDropDown" DataFieldName="BSC" HeaderText="BSC:" />
            <mmsi:DataField runat="server" ID="dfSecurity" DataType="BooleanDropDown" DataFieldName="Security" HeaderText="Security:" />
            <mmsi:DataField runat="server" ID="dfIT" DataType="BooleanDropDown" DataFieldName="IT" HeaderText="IT:" />
            <mmsi:DataField runat="server" ID="dfBOP" DataType="BooleanDropDown" DataFieldName="BOP" HeaderText="BOP:" />
            <mmsi:DataField runat="server" ID="dfLegal" DataType="BooleanDropDown" DataFieldName="Legal" HeaderText="Legal:" />
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
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="ManagementAuditID" AllowSorting="true"
            AutoGenerateColumns="true" EnableViewState="false"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
                Select 
                    ma.*,
                    l2.lookupvalue as RequestedAction,
                    l1.lookupvalue as DRRating
                from 
                    ManagementAudits ma
                left join
                    Lookup l1
                on
                    l1.LookupID = ma.DRRating
                left join
                    Lookup l2
                on
                    l2.LookupID = ma.RequestedAction
            "
        >
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" 
            OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "Management Audit"
            ConnectionString="MCProd"
            DataElementParameterName = "ManagementAuditID"
            DataElementTableName = "ManagementAudits"
            AllowDelete="true"
            OnPreInit="DataElement1_PreInit"
            OnDataBinding="DataElement1_DataBinding"
        >
        </mmsi:DataElement>
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="100" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="100" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>
    
</asp:Content>

