<%@ Page Title="Timeouts" Theme="bender" Language="C#" MasterPageFile="~/MapCallHIB.Master" AutoEventWireup="true" CodeBehind="Timeout.aspx.cs" Inherits="MapCall.Modules.Utility.Timeout" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
    Timeouts
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphInstructions">
    Use this page to setup timeouts for multiple users. Each user that has the timeoutCode set in their 
    Address field will timeout on the date specified in the record.
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Panel ID="pnlSearch" runat="server">
        <center>
            <table style="width:650px;border:1px solid black;">

                <mmsi:DataField runat="server" ID="dsTimeoutcode" DataType="String" HeaderText="Timeout Code: " DataFieldName="TimeoutCode" />
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
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1"
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" 
            DataKeyNames="TimeoutID" AllowPaging="true" AllowSorting="true" PageSize="20">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MCProd %>" SelectCommand="SELECT * FROM Timeouts" />
    </asp:Panel>
    <asp:Panel ID="pnlDetail" runat="server" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" 
            ConnectionString="MCProd"
            DataElementName="Timeouts"
            DataElementParameterName="TimeoutID"
            DataElementTableName="Timeouts" />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
    </asp:Panel>
</asp:Content>



