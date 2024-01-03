<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="Notes.aspx.cs" Inherits="MapCall.Modules.HR.Notes" Title="Notes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Notes   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
Use this page to search all of the notes in the system.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <tr>
                <td style="text-align:right;">
                    Find : 
                </td>
                <td style="text-align:left;">
                    <asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
        </center>
        <br />
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="NoteID" DataSourceID="SqlDataSource1" EmptyDataText="No matching notes were found.">
            <Columns>
                <asp:BoundField DataField="NoteID" HeaderText="NoteID" InsertVisible="False" ReadOnly="True" SortExpression="NoteID" />
                <asp:BoundField DataField="Note" HeaderText="Note" SortExpression="Note" />
                <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy" />
                <asp:BoundField DataField="CreatedAt" HeaderText="Date_Added" SortExpression="CreatedAt" />
                <asp:BoundField DataField="DataLinkID" HeaderText="DataLinkID" SortExpression="DataLinkID" />
                <asp:BoundField DataField="DataTypeID" HeaderText="DataTypeID" SortExpression="DataTypeID" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" SelectCommand="SELECT * FROM [Note]">
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        Deta<asp:DetailsView ID="DetailsView1" runat="server" Height="50px" Width="125px">
        </asp:DetailsView>
        ils
    </asp:Panel>
</asp:Content>
