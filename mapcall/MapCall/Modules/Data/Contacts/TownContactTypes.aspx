<%@ Page Title="" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="TownContactTypes.aspx.cs" Inherits="MapCall.Modules.Data.Contacts.TownContactTypes" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.DropDowns" TagPrefix="mapcall" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Town Contact Types
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
        DataElementTableName="TownContactTypes"
        DataElementPrimaryFieldName="ContactTypeID" 
        Label="Town Contact Type" DefaultPageMode="Results">
        
        <ResultsGridView>
            <Columns>
                <mapcall:BoundField DataField="Name" />
            </Columns>
        </ResultsGridView>

        <ResultsDataSource SelectCommand="
            select
                c.ContactTypeID,
	            c.Name 
            from [TownContactTypes] town
            inner join [ContactTypes] c on c.ContactTypeID = town.ContactTypeID" />
                                                                                                
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
            DeleteCommand="DELETE FROM [TownContactTypes] WHERE [ContactTypeID] = @ContactTypeID" 
            UpdateCommand="UPDATE [TownContactTypes] Set ContactTypeID = @ContactTypeID WHERE ContactTypeID = @ContactTypeID"
            InsertCommand="INSERT INTO [TownContactTypes] ([ContactTypeID]) VALUES (@ContactTypeID); SELECT @ContactTypeID" 
            SelectCommand="select
                c.ContactTypeID,
	            c.Name 
            from [TownContactTypes] town
            inner join [ContactTypes] c on c.ContactTypeID = town.ContactTypeID
            where (town.[ContactTypeID] = @ContactTypeID)" 
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