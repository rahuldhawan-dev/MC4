<%@ Control Language="C#" EnableTheming="true" AutoEventWireup="true" CodeBehind="Notes.ascx.cs"  Inherits="MapCall.Controls.Data.Notes" %>

<%--Note that styling needs to be tested on regular pages and iMap pages.--%>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="noteDocs">
        
        <div class="header dualCol">
            <div class="left">
                <div>
                    Notes
                </div>
            </div>
            <div class="right">
                <div>
                    <asp:Button runat="server" ID="btnAddToggle" Text="New Note" EnableViewState="false" 
                    OnClientClick="
                        document.getElementById('tblNoteUnique').style.display = 'block';
                        document.getElementById('tblNoteUnique').children[0].children[0].children[0].children[0].innerHTML='';
                        document.getElementById('tblNoteUnique').children[0].children[0].children[0].children[0].focus();
                        return false;
                        " />
                </div>
            </div>
        </div>
        
        <div class="body">
            <div class="results">
                <%-- This GridView needs to have ViewState enabled or else it doesn't function properly. --%>
                <asp:GridView Width="100%" ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="NoteID"
                DataSourceID="SqlDataSource1" OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated"
                EmptyDataText="There are no notes for this record."  
                AllowPaging="True" AllowSorting="True" 
                EnableViewState="true">
                <Columns>
                    <asp:TemplateField HeaderText="Note" ItemStyle-Width="65%">
                        <ItemStyle CssClass="NoteCell" />
                        <ItemTemplate>
                            <%#Eval("Note") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" EnableViewState="false" ID="txtNote" Rows="10" TextMode="MultiLine" Text='<%# Bind("Note") %>' Width="98%"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ReadOnly="true" DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" ControlStyle-Width="20%" />
                    <asp:BoundField ReadOnly="true" DataField="CreatedAt" HeaderText="Added" SortExpression="CreatedAt" ControlStyle-Width="20%" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" EnableViewState="false" />
                            <asp:LinkButton ID="btnDeleteNote1" runat="server" CausesValidation="False" CommandName="Delete" EnableViewState="false"
                                Text="Delete" OnClientClick="return confirm('Delete this note permanently?');" ></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" />
                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
            
            <table id="tblNoteUnique" class="addNote">
                <tr>
                    <td style="width:85%;padding-top:2px;">
                        <asp:TextBox runat="server" ID="txtNote" Width="100%" Rows="10" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td style="padding-top:2px;vertical-align:bottom;text-align:right;">
                        <asp:Button runat="server" ID="btnAddNote" Text="Add Note" OnClick="btnAddNote_Click" />
                        <br />
                        <asp:Button runat="server" id="btnCancelNote" Text="Cancel" OnClientClick="document.getElementById('tblNoteUnique').style.display = 'none';return false;" />
                    </td>
                </tr>
            </table>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" EnableViewState="false" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                DeleteCommand="DELETE FROM [Note] WHERE [NoteID] = @NoteID" 
                InsertCommand="INSERT INTO [Note] ([Note], [DataLinkID], [DataTypeID], [CreatedBy]) VALUES (@Note, @DataLinkID, @DataTypeID, @CreatedBy)"
                ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
                SelectCommand="SELECT [NoteID], [Note], [CreatedBy], [CreatedAt], [DataLinkID], [DataTypeID] FROM [Note] where [DataLinkID] = @DataLinkID and [DataTypeID] = @DataTypeID"
                UpdateCommand="UPDATE [Note] SET [Note] = @Note WHERE [NoteID] = @NoteID">
                <InsertParameters>
                    <asp:Parameter Name="Note" Type="String" />
                    <asp:Parameter Name="DataLinkID" Type="Int32" />
                    <asp:Parameter Name="DataTypeID" Type="Int32" />
                    <asp:Parameter Name="CreatedBy" Type="String" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Note" Type="String" />
                    <asp:Parameter Name="NoteID" Type="Int32" />
                </UpdateParameters>
                <DeleteParameters>
                    <asp:Parameter Name="NoteID" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="DataTypeID" Type="Int32" />
                    <asp:Parameter Name="DataLinkID" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
    
        </div>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>


    



