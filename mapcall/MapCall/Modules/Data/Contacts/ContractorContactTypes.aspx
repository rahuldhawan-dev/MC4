<%@ Page Title="" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ContractorContactTypes.aspx.cs" Inherits="MapCall.Modules.Data.Contacts.ContractorContactTypes" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Town Contact Types
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
        DataElementTableName="ContactTypesForContractors"
        DataElementPrimaryFieldName="ContactTypeID" 
        Label="Contractor Contact Type" DefaultPageMode="Results">
        
        <ResultsGridView>
            <Columns>
                <mapcall:BoundField DataField="Name" />
            </Columns>
        </ResultsGridView>

        <ResultsDataSource SelectCommand="
            select
                c.ContactTypeID,
	            c.Name 
            from [ContactTypesForContractors] contractor
            inner join [ContactTypes] c on c.ContactTypeID = contractor.ContactTypeID" />
                                                                                                
        <DetailsView>
            <Fields>
                <mapcall:TemplateBoundField HeaderText="Contact Type">
                    <InsertItemTemplate>
                        <mapcall:ContactTypesDropDownList runat="server" ID="ddlContactType"
                            SelectedValue='<%# Bind("ContactTypeID") %>' Required="true"  />
                    </InsertItemTemplate>
                    <ItemTemplate>
                         <%# Eval("Name") %>
                    </ItemTemplate>
                </mapcall:TemplateBoundField>
            </Fields>
        </DetailsView>
        
        <DetailsDataSource 
            DeleteCommand="DELETE FROM [ContactTypesForContractors] WHERE [ContactTypeID] = @ContactTypeID" 
            UpdateCommand="UPDATE [ContactTypesForContractors] SET [ContactTypeID] = @ContactTypeID where [ContactTypeID] = @ContactTypeID"
            InsertCommand="INSERT INTO [ContactTypesForContractors] ([ContactTypeID]) VALUES (@ContactTypeID); SELECT @ContactTypeID" 
            SelectCommand="select
                c.ContactTypeID,
	            c.Name 
            from [ContactTypesForContractors] contractor
            inner join [ContactTypes] c on c.ContactTypeID = contractor.ContactTypeID
            where (contractor.[ContactTypeID] = @ContactTypeID)" 
            >
            <DeleteParameters>
                <asp:Parameter Name="ContactTypeID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ContactTypeID" Type="Int32" Direction="InputOutput" />
            </InsertParameters>
            <SelectParameters>
                <asp:Parameter Name="ContactTypeID" Type="Int32" />
            </SelectParameters>
        </DetailsDataSource>
    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>
