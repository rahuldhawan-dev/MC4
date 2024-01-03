<%@ Page Title="Consecutive Estimates" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ConsecutiveEstimates.aspx.cs" Inherits="MapCall.Modules.FieldServices.ConsecutiveEstimates" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Consecutive Estimates
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Use the form to search and manage the Consecutive Estimates.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="DataField2" DataType="NumberRange" DataFieldName="NUMBER_OF_ESTIMATES" HeaderText="# Estimates : " />
            <mmsi:DataField runat="server" ID="DataField4" DataType="Double" DataFieldName="BILL_CLASS" HeaderText="BILL_CLASS : " />
            <mmsi:DataField runat="server" ID="DataField5" DataType="String" DataFieldName="SERVICE_CITY" HeaderText="SERVICE_CITY : " />
            <mmsi:DataField runat="server" ID="DataField1" DataType="Date" DataFieldName="COMPLETION_DATE" HeaderText="COMPLETION_DATE : " />
            <mmsi:DataField runat="server" ID="DataField3" DataType="Double" DataFieldName="District" HeaderText="District : " />
            <mmsi:DataField runat="server" ID="DataField6" DataType="String" DataFieldName="Name" HeaderText="Name : " />
            <mmsi:DataField runat="server" ID="DataField7" DataType="Double" DataFieldName="Route" HeaderText="Route : " />
            <mmsi:DataField runat="server" ID="DataField8" DataType="NumberRange" DataFieldName="METER_SIZE" HeaderText="Meter Size : " />
            <mmsi:DataField runat="server" ID="DataField9" DataType="Double" DataFieldName="SKIP_CODE" HeaderText="Skip Code : " />
            <mmsi:DataField runat="server" ID="DataField10" DataType="Date" DataFieldName="CUR_READ_DATE" HeaderText="Current Read Date : " />
            <mmsi:DataField runat="server" ID="DataField11" DataType="Date" DataFieldName="PREV_READ_DATE" HeaderText="Prev Read Date : " />
            <mmsi:DataField runat="server" ID="DataField12" DataType="Date" DataFieldName="INVESTIGATION_DATE" HeaderText="Investigation Date : " />
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
        <asp:Button runat="server" ID="btnMap" Visible="true" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="ConEstID" AllowSorting="true"
            PageSize="20"
            AllowPaging="true"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select * from tblConsecutive_Estimates">
        </asp:SqlDataSource>
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "ConsecutiveEstimate"
            DataElementParameterName = "ConEstID"
            DataElementTableName = "tblConsecutive_Estimates"
            AllowDelete="true"
            OnPreInit="DataElement1_PreInit"
        />
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="79" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="79" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>
            
</asp:Content>
