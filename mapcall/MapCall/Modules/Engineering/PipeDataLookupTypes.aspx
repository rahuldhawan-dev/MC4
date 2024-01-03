<%@ Page Title="Pipe Data Lookup Types" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="PipeDataLookupTypes.aspx.cs" Inherits="MapCall.Modules.Engineering.PipeDataLookupTypes" %>
<%@ Register TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" Src="~/Controls/DetailsViewDataPageTemplate.ascx" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls" Assembly="MapCall" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Pipe Data Lookup Types
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
<mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
    DataElementTableName="PipeDataLookupTypes" 
    DataElementPrimaryFieldName="PipeDataLookupTypeID"
    Label="Pipe Data Lookup Type">
    <ResultsGridView>
        <Columns>
            <mapcall:BoundField DataField="Description"/>
            <mapcall:BoundField DataField="TotalValues"/>
        </Columns>
    </ResultsGridView>
    
    <ResultsDataSource SelectCommand="
        SELECT 
            t.*, 
            (Select Count(1) from PipeDataLookupValues v where v.PipeDataLookupTypeID = t.PipeDataLookupTypeID) as TotalValues
        FROM 
            [PipeDataLookupTypes] t" />
    
    <DetailsView>
        <Fields>
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate><%# Eval("Description") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtDescription" Text='<%# Bind("Description") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
    </DetailsView>
    
    <DetailsDataSource CancelSelectOnNullParameter="true" 
                DeleteCommand="DELETE FROM [PipeDataLookupTypes] WHERE [PipeDataLookupTypeID] = @PipeDataLookupTypeID" 
                InsertCommand="INSERT INTO [PipeDataLookupTypes] ([Description]) VALUES (@Description); SELECT @PipeDataLookupTypeID = (SELECT @@IDENTITY)" 
                SelectCommand="SELECT * FROM [PipeDataLookupTypes] WHERE PipeDataLookupTypeID = @PipeDataLookupTypeID" 
                UpdateCommand="UPDATE [PipeDataLookupTypes] SET [Description] = @Description WHERE [PipeDataLookupTypeID] = @PipeDataLookupTypeID">
        <SelectParameters>
            <asp:Parameter Name="PipeDataLookupTypeID" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="PipeDataLookupTypeID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="PipeDataLookupTypeID" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="PipeDataLookupTypeID" Type="Int32" Direction="Output" />
            <asp:Parameter Name="Description" Type="String" />
        </InsertParameters>
    </DetailsDataSource>
    
</mapcall:DetailsViewDataPageTemplate>

</asp:Content>
