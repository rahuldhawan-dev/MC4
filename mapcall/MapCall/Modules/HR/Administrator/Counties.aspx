<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="Counties.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.Counties" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHeader">
    Counties
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
            <table style="text-align:left;" border="0">
                <mmsi:DataField runat="server" ID="dfState"
                    HeaderText="State"
                    DataType="DropDownList"
                    DataFieldName="State"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT [Abbreviation] AS [Val], [Abbreviation] AS [Txt] FROM [States]" />
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
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnMap" Visible="true" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount" />
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            DataKeyNames="ID" AllowSorting="true" AllowPaging="true" PageSize="20">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="
            SELECT
                a.Name,
                b.Abbreviation AS State,
                a.SpecialID as CountyID,
                a.CountyID as ID
            FROM
                Counties AS a
            INNER JOIN
                States AS b
            ON
                a.StateID = b.StateID"
        />

    </asp:Panel>
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1"
            AllowNew="false" AllowDelete="false" AllowEdit="false"
            DataElementName="Counties Table"
            DataElementParameterName="CountyID"
            DataElementTableName="Counties"
        />

        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
    </asp:Panel>
</asp:Content>
