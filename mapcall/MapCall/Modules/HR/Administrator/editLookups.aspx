<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="editLookups.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.editLookups" Title="Lookups" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Lookups
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
Use this page to edit the Lookup Values contained within drop-down menus.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin-bottom:3px">
        LookupType: <asp:TextBox runat="server" ID="txtAddLookupType" />
        LookupValue: <asp:TextBox runat="server" ID="txtAddLookupValue" />
        TableName: <asp:DropDownList ID="ddlAddTableName" runat="server" DataSourceID="dsTableNames"
                        DataTextField="TableDesc" DataValueField="TableName" 
                        AppendDataBoundItems="True">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </asp:DropDownList>
        <asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btnAdd_Click" />
    </div>
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="LookupID" DataSourceID="SqlDataSource1"
        EmptyDataText="There are no data records to display."
        OnRowDataBound="GridView1_OnRowDataBound"
        >
        <Columns>
            <asp:BoundField DataField="LookupID" HeaderText="LookupID" ReadOnly="True" SortExpression="LookupID" />
            <asp:BoundField DataField="LookupType" HeaderText="LookupType" SortExpression="LookupType" />
            <asp:BoundField DataField="LookupValue" HeaderText="LookupValue" SortExpression="LookupValue" />
            <asp:TemplateField SortExpression="TableName" ShowHeader="true">
                <HeaderTemplate>
                    <asp:LinkButton runat="server" Text="TableName" CommandName="Sort" CommandArgument="TableName" />
                </HeaderTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlTableName" runat="server" DataSourceID="dsTableNames"
                        DataTextField="TableDesc" DataValueField="TableName" SelectedValue='<%# Bind("TableName") %>'
                        AppendDataBoundItems="True">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </asp:DropDownList>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblTableName" runat="server" Text='<%# Bind("TableName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" Enabled="false"
                        OnClientClick="return confirm('Disabled until the databases can be merged.');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        DeleteCommand="DELETE FROM [Lookup] WHERE [LookupID] = @LookupID"
        InsertCommand="INSERT INTO [Lookup] ([LookupType], [LookupValue], [TableName]) VALUES (@LookupType, @LookupValue, @TableName)" 
        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
        SelectCommand="SELECT [LookupID], [LookupType], [LookupValue], IsNull([TableName], '') AS [TableName] FROM [Lookup] order by [TableName], [LookupType]"
        UpdateCommand="UPDATE [Lookup] SET [LookupType] = @LookupType, [LookupValue] = @LookupValue, [TableName] = @TableName WHERE [LookupID] = @LookupID">
        <InsertParameters>
            <asp:ControlParameter Name="LookupType" Type="String" ControlID="txtAddLookupType" />
            <asp:ControlParameter Name="LookupValue" Type="String" ControlID="txtAddLookupValue" />
            <asp:ControlParameter Name="TableName" Type="String" ControlID="ddlAddTableName" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="LookupType" Type="String" />
            <asp:Parameter Name="LookupValue" Type="String" />
            <asp:Parameter Name="LookupID" Type="Int32" />
            <asp:Parameter Name="TableName" Type="String" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="LookupID" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsTableNames" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
        SelectCommand="
            SELECT 
                rtrim(ltrim([Name])) as [TableName], rtrim(ltrim([Name])) + ' (mapcall)' AS [TableDesc] FROM [sysobjects] WHERE [xtype] = 'u' And left([Name],1) &lt;&gt; '_' 
            ORDER BY 1 ASC" />
</asp:Content>
