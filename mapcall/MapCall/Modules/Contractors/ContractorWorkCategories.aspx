<%@ Page Title="ContractorWo" Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="ContractorWorkCategories.aspx.cs" Inherits="MapCall.Modules.Contractors.ContractorWorkCategories" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.Data" TagPrefix="cc2" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Contractor Work Categories 
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">

<mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
    DataElementTableName="ContractorWorkCategoryTypes" 
    DataElementPrimaryFieldName="ContractorWorkCategoryTypeID"
    Label="Contractor Work Category">
    
    <ResultsGridView>
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
        </Columns>
    </ResultsGridView>
    
    <ResultsDataSource SelectCommand="SELECT * FROM [ContractorWorkCategoryTypes]" />
    
    <DetailsView>
        <Fields>
            <asp:TemplateField HeaderText="Category Name">
                <ItemTemplate><%# Eval("Name") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtName" Text='<%# Bind("Name") %>' />
                    <cc2:UniqueFieldValidator ID="ufvName" 
                        ControlToValidate="txtName"
                        ErrorMessage="A contractor work category already exists with this name.." 
                        PrimaryKeyName="ContractorWorkCategoryTypeID" 
                        PrimaryKeyValue='<%#Bind("ContractorWorkCategoryTypeID") %>'
                        UniqueFieldName="Name" 
                        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                        SelectCommand="SELECT * FROM ContractorWorkCategoryTypes
                                           WHERE ContractorWorkCategoryTypeID &lt;&gt; @ContractorWorkCategoryTypeID
                                           AND Name = @Name"
                        runat="server" />
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
    </DetailsView>
    
    <DetailsDataSource CancelSelectOnNullParameter="true" 
                DeleteCommand="DELETE FROM [ContractorWorkCategoryTypes] WHERE [ContractorWorkCategoryTypeID] = @ContractorWorkCategoryTypeID" 
                InsertCommand="INSERT INTO [ContractorWorkCategoryTypes] ([Name], [CreatedBy]) VALUES (@Name, @CreatedBy); SELECT @ContractorWorkCategoryTypeID = (SELECT @@IDENTITY)" 
                SelectCommand="SELECT * FROM [ContractorWorkCategoryTypes] WHERE ContractorWorkCategoryTypeID = @ContractorWorkCategoryTypeID" 
                UpdateCommand="UPDATE [ContractorWorkCategoryTypes] SET [Name] = @Name WHERE [ContractorWorkCategoryTypeID] = @ContractorWorkCategoryTypeID">
        <SelectParameters>
            <asp:Parameter Name="ContractorWorkCategoryTypeID" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="ContractorWorkCategoryTypeID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="ContractorWorkCategoryTypeID" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ContractorWorkCategoryTypeID" Type="Int32" Direction="Output" />
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="CreatedBy" Type="String" />
        </InsertParameters>
    </DetailsDataSource>
    
</mapcall:DetailsViewDataPageTemplate>

</asp:Content>
