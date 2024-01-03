<%@ Page Title="" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="FoundationalFilingPeriods.aspx.cs" Inherits="MapCall.Modules.Engineering.FoundationalFilingPeriods" %>
<%@ Register TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" Src="~/Controls/DetailsViewDataPageTemplate.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="MapCall.Controls.Data" Assembly="MapCall" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Foundational Filing Periods
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    

<mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
    DataElementTableName="FoundationalFilingPeriods" 
    DataElementPrimaryFieldName="FoundationalFilingPeriodID"
    Label="Foundational Filing Period">
    
    <ResultsGridView>
        <Columns>
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
        </Columns>
    </ResultsGridView>
    
    <ResultsDataSource SelectCommand="SELECT * FROM [FoundationalFilingPeriods]" />
    
    <DetailsView>
        <Fields>
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate><%# Eval("Description") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtDescription" Text='<%# Bind("Description") %>' />
                    <cc2:UniqueFieldValidator ID="ufvDescription" 
                        ControlToValidate="txtDescription"
                        ErrorMessage="A foundational filing periods already exists with this description.." 
                        PrimaryKeyName="FoundationalFilingPeriodID" 
                        PrimaryKeyValue='<%#Bind("FoundationalFilingPeriodID") %>'
                        UniqueFieldName="Description" 
                        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                        SelectCommand="SELECT * FROM FoundationalFilingPeriods
                                           WHERE FoundationalFilingPeriodID &lt;&gt; @FoundationalFilingPeriodID
                                           AND Description = @Description"
                        runat="server" />
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
    </DetailsView>
    
    <DetailsDataSource CancelSelectOnNullParameter="true" 
        DeleteCommand="DELETE FROM [FoundationalFilingPeriods] WHERE [FoundationalFilingPeriodID] = @FoundationalFilingPeriodID" 
        InsertCommand="INSERT INTO [FoundationalFilingPeriods] ([Description]) VALUES (@Description); SELECT @FoundationalFilingPeriodID = (SELECT @@IDENTITY)" 
        SelectCommand="SELECT * FROM [FoundationalFilingPeriods] WHERE FoundationalFilingPeriodID = @FoundationalFilingPeriodID" 
        UpdateCommand="UPDATE [FoundationalFilingPeriods] SET [Description] = @Description WHERE [FoundationalFilingPeriodID] = @FoundationalFilingPeriodID">
        <SelectParameters>
            <asp:Parameter Name="FoundationalFilingPeriodID" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="FoundationalFilingPeriodID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="FoundationalFilingPeriodID" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="FoundationalFilingPeriodID" Type="Int32" Direction="Output" />
            <asp:Parameter Name="Description" Type="String" />
        </InsertParameters>
    </DetailsDataSource>
    
</mapcall:DetailsViewDataPageTemplate>

</asp:Content>
