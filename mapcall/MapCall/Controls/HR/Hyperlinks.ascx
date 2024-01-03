<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Hyperlinks.ascx.cs" Inherits="MapCall.Controls.HR.Hyperlinks" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div style="font-size:0.9em;border:1px solid #284E98;">
            
            <table style="background-color:#B5C7DE;width:100%;border-bottom:1px solid #284E98;">
                <tr>
                    <td style="font-weight:bold;">
                        Links
                    </td>
                    <td style="text-align:right;font-size:0.9em;">
                        <asp:Button runat="server" Text="New Link" id="btnAdd"
                        OnClientClick="$('#hyperlinks').toggle();return false;" />
                    </td>
                </tr>
            </table>

            <asp:GridView runat="server" ID="gvHyperlinks" 
                DataSourceID="dsHyperlinks"
                OnRowDataBound="GridView1_RowDataBound"
                DataKeyNames="HyperlinkID"
                AutoGenerateColumns="false"
                Width="100%"
                EnableViewState="false"
            >
                <Columns>
                    <asp:HyperLinkField 
                        DataNavigateUrlFields="LinkURL"
                        DataNavigateUrlFormatString="{0}"
                        DataTextField="LinkText"
                        Target="_blank"
                        HeaderText="Link"
                        />
                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By:" />
                    <asp:BoundField DataField="CreatedOn" HeaderText="Created On:" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDeleteHyperlink" runat="server" CausesValidation="False" CommandName="Delete"
                                Text="Delete" OnClientClick="return confirm('Delete this link permanently?');" ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <table id="hyperlinks" style="display:none;width:100%;">
                <tr>
                    <td >
                        Link Text: (mapcall.amwater.com) <br />
                        <asp:TextBox runat="server" ID="txtLinkText" />
                    </td>
                    
                    <td style="width:100%;">
                        Link URL:  (https://mapcall.amwater.com/)
                        <br />
                        <asp:TextBox runat="server" ID="txtLinkURL" Width="95%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnAddHyperlink" OnClick="btnAddHyperlink_Click" Text="Add" />
                        <asp:RegularExpressionValidator runat="server" ID="revLinkURL" SetFocusOnError="True" 
                            ErrorMessage="Please enter a valid URL - http://www.website.com/" 
                            ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
                            ControlToValidate="txtLinkURL" />
                    </td>
                </tr>
            </table>
            
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:SqlDataSource runat="server" id="dsHyperlinks"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    SelectCommand="Select * from Hyperlinks where DataLinkID=@DataLinkID and DataTypeID = @DataTypeID"
    DeleteCommand="Delete Hyperlinks where HyperlinkID = @HyperlinkID"
    InsertCommand="
        INSERT INTO HyperLinks(DataTypeID, DataLinkID, LinkURL, LinkText, CreatedBy) 
        Values(@DataTypeID, @DataLinkID, @LinkURL, @LinkText, @CreatedBy)
    "
    OnInserted="dsHyperlinks_Inserted"
>
    <SelectParameters>
        <asp:Parameter Name="DataLinkID" Type="Int32" />
        <asp:Parameter Name="DataTypeID" Type="Int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Name="DataTypeID" Type="Int32" />
        <asp:Parameter Name="DataLinkID" Type="Int32" />
        <asp:ControlParameter Name="LinkURL" ControlID="txtLinkURL" Type="String" />
        <asp:ControlParameter Name="LinkText" ControlID="txtLinkText" Type="String" />
        <asp:Parameter Name="CreatedBy" Type="String" />
    </InsertParameters>
    <DeleteParameters>
        <asp:Parameter Name="HyperlinkID" Type="Int32" />
    </DeleteParameters>
</asp:SqlDataSource>
