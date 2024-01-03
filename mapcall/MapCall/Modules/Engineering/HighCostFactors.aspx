<%@ Page Title="High Cost Factors" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="HighCostFactors.aspx.cs" Inherits="MapCall.Modules.Engineering.HighCostFactors" %>
<%@ Register TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" Src="~/Controls/DetailsViewDataPageTemplate.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="MapCall.Controls.Data" Assembly="MapCall" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    High Cost Factors
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">

<mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
    DataElementTableName="HighCostFactors" 
    DataElementPrimaryFieldName="HighCostFactorID"
    Label="High Cost Factor">
    
    <ResultsGridView>
        <Columns>
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
        </Columns>
    </ResultsGridView>
    
    <ResultsDataSource SelectCommand="SELECT * FROM [HighCostFactors]" />
    
    <DetailsView>
        <Fields>
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate><%# Eval("Description") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtDescription" Text='<%# Bind("Description") %>' />
                    <cc2:UniqueFieldValidator ID="ufvDescription" 
                        ControlToValidate="txtDescription"
                        ErrorMessage="A high cost factor already exists with this description.." 
                        PrimaryKeyName="HighCostFactorID" 
                        PrimaryKeyValue='<%#Bind("HighCostFactorID") %>'
                        UniqueFieldName="Description" 
                        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                        SelectCommand="SELECT * FROM HighCostFactors
                                           WHERE HighCostFactorID &lt;&gt; @HighCostFactorID
                                           AND Description = @Description"
                        runat="server" />
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
    </DetailsView>
    
    <DetailsDataSource CancelSelectOnNullParameter="true" 
                DeleteCommand="DELETE FROM [HighCostFactors] WHERE [HighCostFactorID] = @HighCostFactorID" 
                InsertCommand="INSERT INTO [HighCostFactors] ([Description], [CreatedBy]) VALUES (@Description, @CreatedBy); SELECT @HighCostFactorID = (SELECT @@IDENTITY)" 
                SelectCommand="SELECT * FROM [HighCostFactors] WHERE HighCostFactorID = @HighCostFactorID" 
                UpdateCommand="UPDATE [HighCostFactors] SET [Description] = @Description WHERE [HighCostFactorID] = @HighCostFactorID">
        <SelectParameters>
            <asp:Parameter Name="HighCostFactorID" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="HighCostFactorID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="HighCostFactorID" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="HighCostFactorID" Type="Int32" Direction="Output" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="CreatedBy" Type="String" />
        </InsertParameters>
    </DetailsDataSource>
    
</mapcall:DetailsViewDataPageTemplate>

</asp:Content>
