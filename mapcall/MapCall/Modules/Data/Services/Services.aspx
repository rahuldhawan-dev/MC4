<%@ Page Theme="bender" Title="" Language="C#" MasterPageFile="~/MapCallFS.Master" AutoEventWireup="true" CodeBehind="Services.aspx.cs" Inherits="MapCall.Modules.Data.Services.Services" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    IL Services
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Use this page to search and view service images. To bring back more results use less information. E.g. Street Name = 'MAIN' will return both Main Street and Main St.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Panel ID="pnlSearch" runat="server">
        <center>
            <table style="width:650px;border:1px solid black;">
                <mmsi:DataField runat="server" ID="DataField2" DataType="String" HeaderText="Service # : " DataFieldName="Service_Num" />
                <mmsi:DataField runat="server" ID="DataField3" DataType="String" HeaderText="Premise # : " DataFieldName="Premise_Num" />
                <mmsi:DataField runat="server" ID="DataField4" DataType="String" HeaderText="Service House# : " DataFieldName="ServiceHouse_Num" />
                <mmsi:DataField runat="server" ID="DataField5" DataType="String" HeaderText="Service Prefix : " DataFieldName="Service_Prefix" />
                <mmsi:DataField runat="server" ID="DataField6" DataType="String" HeaderText="Service Street : " DataFieldName="Service_Street" />
                <mmsi:DataField runat="server" ID="DataField1" DataType="DropDownList"
                    HeaderText="Service Street : "
                    DataFieldName="Service_Street"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT
                            Distinct Service_Street AS [Txt],
                            Service_Street AS [Val]
                        FROM
                            ILServices
                        ORDER BY 1" 
                />
                <mmsi:DataField runat="server" ID="DataField7" DataType="String" HeaderText="Service Suffix : " DataFieldName="Service_Suffix" />
                <mmsi:DataField runat="server" ID="DataField8" DataType="String" HeaderText="Service Apt# : " DataFieldName="ServiceApt_Num" />
                <mmsi:DataField runat="server" ID="dfTown" DataType="DropDownList"
                    HeaderText="Service City : "
                    DataFieldName="Service_City"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT
                            Distinct Service_City AS [Txt],
                            Service_City AS [Val]
                        FROM
                            ILServices
                        ORDER BY 1" 
                />
                <mmsi:DataField runat="server" ID="DataField10" DataType="String" HeaderText="Service Zip : " DataFieldName="Service_Zip" />
                <mmsi:DataField runat="server" ID="DataField11" DataType="String" HeaderText="Path : " DataFieldName="Path" />
                <mmsi:DataField runat="server" ID="DataField12" DataType="String" HeaderText="Service Type : " DataFieldName="ServiceType" />
                <mmsi:DataField runat="server" ID="DataField13" DataType="String" HeaderText="Full Street Name : " DataFieldName="FullStreetName" />
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
    </asp:Panel>
    <asp:Panel ID="pnlResults" runat="server" Visible="false">
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1"
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="ServiceID" AllowPaging="true" AllowSorting="true" PageSize="20">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
                <asp:HyperLinkField DataTextField="Path" DataNavigateUrlFields="Path" DataNavigateUrlFormatString="~/NJAWC_TAP/images/IL/{0}" Target="_blank" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MCProd %>" SelectCommand="
                SELECT [ServiceID]
                  ,[Service_Num]
                  ,[Premise_Num]
                  ,[ServiceHouse_Num]
                  ,[Service_Prefix]
                  ,[Service_Street]
                  ,[Service_Suffix]
                  ,[ServiceApt_Num]
                  ,[Service_City]
                  ,[Service_Zip]
                  ,[Path]
                  ,[ServiceType]
                  ,[FullStreetName]
              FROM [ILServices]
        " />
    </asp:Panel>
    <asp:Panel ID="pnlDetail" runat="server" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" DataElementName="ILServices"
            DataElementParameterName="ServiceID"
            DataElementTableName="ILServices"
            ConnectionString="MCProd" />
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="83" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="83" />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
    </asp:Panel>
</asp:Content>
