<%@ Page Title="Web Links" Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="WebLinks.aspx.cs" Inherits="MapCall.Modules.Management.WebLinks" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Web Links
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">

<mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
    DataElementTableName="WebLinks" 
    DataElementPrimaryFieldName="WebLinkID"
    Label="Web Link">
    <SearchBox> 
        <Fields>
            <search:TextSearchField DataFieldName="Title" />
            <search:TextSearchField DataFieldName="Url" />
        </Fields>
    </SearchBox>
    
    <ResultsGridView>
        <Columns>
            <asp:TemplateField HeaderText="Category" SortExpression="CategoryDescription">
               <ItemTemplate>
                    <%# Eval("CategoryDescription") %>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Link" SortExpression="Title">
                <ItemTemplate>
                    <a href="<%# FixUrl(Eval("Url").ToString()) %>" target="_blank"><%# HttpUtility.HtmlEncode(Eval("Title").ToString()) %></a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </ResultsGridView>
    
    <ResultsDataSource SelectCommand="SELECT 
	                                        link.*,
	                                        cats.Description as CategoryDescription
                                        FROM
	                                        [WebLinks] link
                                        INNER JOIN
	                                        [WebLinkCategories] cats
                                        ON
	                                        cats.WebLinkCategoryID = link.WebLinkCategoryID" />
    
    <DetailsView>
        <Fields>
            <asp:TemplateField HeaderText="Category">
                <ItemTemplate>
                    <%# Eval("CategoryDescription") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlCategory" runat="server"
                        AppendDataBoundItems="true"
                        DataSourceID="dsCategory" 
                        DataTextField="Description" 
                        DataValueField="WebLinkCategoryID"
                        SelectedValue='<%# Bind("WebLinkCategoryID") %>'>
                        <asp:ListItem Value="">-- Select Category --</asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsCategory" runat="server" ConnectionString='<%$ ConnectionStrings:MCProd %>'
                        SelectCommand="SELECT * FROM [WebLinkCategories]" />
                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server" 
                        ControlToValidate="ddlCategory" 
                        InitialValue="" 
                        ErrorMessage="Required" />
                </EditItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Title">
                <ItemTemplate>
                    <%# Eval("Title") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbTitle" runat="server" MaxLength="100" Text='<%# Bind("Title") %>' />
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" 
                        ControlToValidate="tbTitle" 
                        InitialValue=""
                        ErrorMessage="Required" />
                </EditItemTemplate>
            </asp:TemplateField>
            

            <asp:TemplateField HeaderText="Url">
                <ItemTemplate>
                   <a href="<%# FixUrl(Eval("Url").ToString()) %>" target="_blank"><%# Eval("Url")%></a>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbUrl" runat="server" MaxLength="200" Text='<%# Bind("Url") %>' />
                    <asp:RequiredFieldValidator ID="rfvUrl" runat="server" 
                        ControlToValidate="tbUrl" 
                        InitialValue=""
                        ErrorMessage="Required" />
                    <asp:CustomValidator ID="rfvUrlValidate" runat="server" 
                        ControlToValidate="tbUrl" ErrorMessage="Required" OnServerValidate="rfvUrlOnServerValidate" />
                </EditItemTemplate>
            </asp:TemplateField>                    
        </Fields>
    </DetailsView>
    
    <DetailsDataSource DeleteCommand="DELETE FROM [WebLinks] WHERE [WebLinkID] = @WebLinkID" 
                InsertCommand="INSERT INTO [WebLinks] ([WebLinkCategoryID], [Url], [Title], [CreatedBy]) VALUES (@WebLinkCategoryID, @Url, @Title, @CreatedBy); SELECT @WebLinkID = (SELECT @@IDENTITY)" 
                UpdateCommand="UPDATE [WebLinks] SET [WebLinkCategoryID] = @WebLinkCategoryID, [Url] = @Url, [Title] = @Title WHERE [WebLinkID] = @WebLinkID"
                SelectCommand="SELECT 
                                    link.*,
                                    cats.Description as CategoryDescription
                                FROM
                                    [WebLinks] link
                                INNER JOIN
                                    [WebLinkCategories] cats
                                ON
                                    cats.WebLinkCategoryID = link.WebLinkCategoryID
                                WHERE
                                    link.WebLinkID = @WebLinkID" >
                <SelectParameters>
                    <asp:Parameter Name="WebLinkID" Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="WebLinkID" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="WebLinkCategoryID" Type="Int32" />
                    <asp:Parameter Name="Url" Type="String" />
                    <asp:Parameter Name="Title" Type="String" />
                    <asp:Parameter Name="WebLinkID" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="WebLinkID" Type="Int32" Direction="Output" />
                    <asp:Parameter Name="WebLinkCategoryID" Type="Int32" />
                    <asp:Parameter Name="Url" Type="String" />
                    <asp:Parameter Name="Title" Type="String" />
                    <asp:Parameter Name="CreatedBy" Type="String" />
                </InsertParameters>
     </DetailsDataSource>
    
</mapcall:DetailsViewDataPageTemplate>
    
</asp:Content>
