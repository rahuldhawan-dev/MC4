<%@ Page Title="Departments" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="Departments.aspx.cs" Inherits="MapCall.Modules.Admin.Departments" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Departments
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
        DataElementTableName="Departments"
        DataElementPrimaryFieldName="DepartmentID"
        Label="Department">
        <SearchBox runat="server">
            <Fields>
                <search:TextSearchField DataFieldName="Code" Label="Code"/>
                <search:TextSearchField DataFieldName="Description" Label="Description"/> 
            </Fields>
        </SearchBox>
        
        <ResultsGridView>
            <Columns>
                <mapcall:BoundField DataField="Code" />
                <mapcall:BoundField DataField="Description" />
            </Columns>
        </ResultsGridView>
        
        <ResultsDataSource SelectCommand="SELECT * FROM [Departments]"/>
        
        <DetailsView>
            <Fields>
                <mapcall:BoundField DataField="Code" MaxLength="2" Required />
                <mapcall:BoundField DataField="Description" MaxLength="50" Required />
            </Fields>
        </DetailsView>
        
        <DetailsDataSource
            DeleteCommand = "DELETE FROM [Departments] where [DepartmentID] = @DepartmentID"
            InsertCommand = "INSERT INTO [Departments](Code, Description) Values(@Code, @Description);SELECT @DepartmentID = (SELECT @@IDENTITY)"
            UpdateCommand = "UPDATE [Departments] SET Code = @Code, Description = @Description WHERE DepartmentID = @DepartmentID"
            SelectCommand = "SELECT * FROM [Departments] WHERE DepartmentID = @DepartmentID"
        >
            <DeleteParameters>
                <asp:Parameter Name="DepartmentID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="DepartmentID" Type="Int32" Direction="Output"/>
                <asp:Parameter Name="Code" Type="String"/>
                <asp:Parameter Name="Description" Type="String"/>
            </InsertParameters>
            <SelectParameters>
                <asp:Parameter Name="DepartmentID" Type="Int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="DepartmentID" Type="Int32" />
                <asp:Parameter Name="Code" Type="String"/>
                <asp:Parameter Name="Description" Type="String"/>
            </UpdateParameters>
        </DetailsDataSource>
    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>

