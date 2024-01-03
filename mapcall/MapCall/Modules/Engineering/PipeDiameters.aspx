<%@ Page Title="Pipe Diameters" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="PipeDiameters.aspx.cs" Inherits="MapCall.Modules.Engineering.PipeDiameters" %>
<%@ Register TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" Src="~/Controls/DetailsViewDataPageTemplate.ascx" %>
<%@ Register TagPrefix="cc2" Namespace="MapCall.Controls.Data" Assembly="MapCall" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Pipe Diameters
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">

<mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
    DataElementTableName="PipeDiameters" 
    DataElementPrimaryFieldName="PipeDiameterID"
    Label="Pipe Diameter">
    
    <ResultsGridView>
        <Columns>
            <asp:BoundField DataField="Diameter" HeaderText="Diameter" SortExpression="Diameter" />
        </Columns>
    </ResultsGridView>
    
    <ResultsDataSource SelectCommand="SELECT * FROM [PipeDiameters]" />
    
    <DetailsView>
        <Fields>
            <asp:TemplateField HeaderText="Diameter">
                <ItemTemplate><%# Eval("Diameter") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" ID="txtDiameter" Text='<%# Bind("Diameter") %>' />
                    <cc2:UniqueFieldValidator ID="ufvDiameter" 
                        ControlToValidate="txtDiameter"
                        ErrorMessage="A pipe diameter already exists with this diameter.." 
                        PrimaryKeyName="PipeDiameterID" 
                        PrimaryKeyValue='<%#Bind("PipeDiameterID") %>'
                        UniqueFieldName="Diameter" 
                        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                        SelectCommand="SELECT * FROM PipeDiameters
                                           WHERE PipeDiameterID &lt;&gt; @PipeDiameterID
                                           AND Diameter = @Diameter"
                        runat="server" />
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
    </DetailsView>
    
    <DetailsDataSource CancelSelectOnNullParameter="true" 
                DeleteCommand="DELETE FROM [PipeDiameters] WHERE [PipeDiameterID] = @PipeDiameterID" 
                InsertCommand="INSERT INTO [PipeDiameters] ([Diameter], [CreatedBy]) VALUES (@Diameter, @CreatedBy); SELECT @PipeDiameterID = (SELECT @@IDENTITY)" 
                SelectCommand="SELECT * FROM [PipeDiameters] WHERE PipeDiameterID = @PipeDiameterID" 
                UpdateCommand="UPDATE [PipeDiameters] SET [Diameter] = @Diameter WHERE [PipeDiameterID] = @PipeDiameterID">
        <SelectParameters>
            <asp:Parameter Name="PipeDiameterID" Type="Int32" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="PipeDiameterID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Diameter" Type="String" />
            <asp:Parameter Name="PipeDiameterID" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="PipeDiameterID" Type="Int32" Direction="Output" />
            <asp:Parameter Name="Diameter" Type="String" />
            <asp:Parameter Name="CreatedBy" Type="String" />
        </InsertParameters>
    </DetailsDataSource>
    
</mapcall:DetailsViewDataPageTemplate>

</asp:Content>

