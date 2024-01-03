<%@ Page Title="Strategic Elements" Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="StrategicElements.aspx.cs" Inherits="MapCall.Modules.BusinessPerformance.StrategicElements" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.HR" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Strategic Elements
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />


<asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <table style="text-align:left;" border="0">
        
            <mmsi:DataField runat="server" ID="dfName"
                HeaderText="Strategic Element Name"
                DataType="String"
                DataFieldName="ElementName"
             />
            <mmsi:DataField runat="server" ID="dfStrategyName"
                HeaderText="Strategy Name:"
                DataType="String"
                DataFieldName="StrategyName"
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
  
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnMap" Visible="False" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="StrategicElementID"
            AllowSorting="true" AllowPaging="true" PageSize="500"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            SelectCommand="SELECT
	                           se.*
	                       FROM
	                           [StrategicElements] se" />
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <div id="divContent">
            <ul class="ui-tabs-nav">
                <li><a href="#se" class="tab"><span>Strategic Elements</span></a></li>
                <li><a href="#notes" class="tab"><span>Notes</span></a></li>
                <li><a href="#documents" class="tab"><span>Documents</span></a></li>
            </ul>
            <div id="se">
                <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
                    DataElementParameterName = "StrategicElementID"
                    DataElementTableName = "StrategicElements"
                    OnDataBinding="DataElement1_DataBinding" ConnectionString="MCProd"
                    AllowDelete="true" ShowKey="true">
                         <mmsi:DataElementField DataFieldName="StrategyName" HeaderName="Strategy Name" runat="server" />
                         <mmsi:DataElementField DataFieldName="ElementName" HeaderName="Element Name" Type="VarChar" Required="true" runat="server" />
                         <mmsi:DataElementField DataFieldName="Description" HeaderName="Description" Required="true" Type="Text" runat="server" />
                    </mmsi:DataElement>
            </div>
            <div id="notes">
                <mmsi:Notes ID="Notes1" runat="server" DataTypeID="138" />
            </div>
            <div id="documents">
                <mmsi:Documents ID="Documents1" runat="server" DataTypeID="138" />
            </div>
        </div>
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search" CausesValidation="false" />
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" CausesValidation="false" />
        </center>
        
    </asp:Panel>
    <script type="text/javascript">
        $(document).ready(function() {
            //tabs
            $('#divContent').tabs();
        });
    </script>
</asp:Content>
