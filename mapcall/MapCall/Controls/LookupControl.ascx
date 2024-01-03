<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LookupControl.ascx.cs" EnableTheming="true" Inherits="MapCall.Controls.LookupControl" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>

<mapcall:DetailsViewDataPageTemplate ID="template" runat="server">
    <ResultsGridView>
        <Columns>
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <%# Eval("Description") %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ResultsGridView>
    
    <DetailsView>
        <Fields>
            <asp:TemplateField InsertVisible="false">
                <ItemTemplate>
                    <%# Eval(TablePrimaryKeyFieldName) %>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <%# Eval(DescriptionFieldName) %>
                </ItemTemplate>
                <EditItemTemplate>
                
                    <asp:TextBox ID="txtDesc" runat="server" 
                        MaxLength="50" 
                        Text='<%# Bind("Description") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
    </DetailsView>
    
</mapcall:DetailsViewDataPageTemplate>
