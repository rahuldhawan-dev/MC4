<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Documents.ascx.cs" Inherits="MapCall.Controls.Documents.Documents" %>

<div class="noteDocs">
    <div class="header dualCol">
        <div class="left">
            <div>
                Documents
            </div>
        </div>
        <div class="right">
            <div>
                 <asp:Button runat="server" ID="btnDocLinkToggle" Text="Link Document" OnClientClick="document.getElementById('tblDocUnique').style.display = 'none';document.getElementById('tblDocLink').style.display = '';document.getElementById('tblDocUnique').scrollIntoView();return false;" />
                 <asp:Button runat="server" ID="btnDocAddToggle" Text="New Document" OnClientClick="document.getElementById('tblDocUnique').style.display = '';document.getElementById('tblDocLink').style.display = 'none';document.getElementById('tblDocUnique').scrollIntoView();return false;" />
            </div>
        </div>
    </div>
    
    <div class="body">
        <div class="results">
            <asp:GridView 
                OnRowCommand="gvDocuments_RowCommand" 
                OnDataBound="gvDocuments_DataBound" 
                OnRowDataBound="gvDocuments_RowDataBound"
                ID="gvDocuments" runat="server" 
                DataSourceID="dsDocuments" AutoGenerateColumns="False" Width="100%" 
                DataKeyNames="DocumentLinkID,DocumentID" AllowSorting="true"
                >
                <Columns>
                    <asp:BoundField DataField="File_Name" HeaderText="File Name" SortExpression="File_Name" />
                    <asp:BoundField DataField="Document_Type" HeaderText="Document Type" SortExpression="Document_Type" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="btnDocumentCtrlView" runat="server" CausesValidation="False" CommandName="Select"
                                Text="View" CommandArgument='<%# Eval("documentID") %>' />
                            <asp:Button ID="btnDocumentCtrlDelete" runat="server" CausesValidation="False" CommandName="Del"
                                    Text="Delete" CommandArgument='<%# Eval("documentlinkID") %>' OnClientClick="return confirm('are you sure?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="dsDocuments" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                DeleteCommand="Go"
                SelectCommand="
                    select document.documentID, File_Name, Document_Type, DocumentLinkID 
                        from documentlink 
                        left join document on document.documentID = documentLink.documentID
                        left join documentType on documentType.documentTypeID = documentLink.documentTypeID
                        where	documentlink.datatypeID = @DataTypeID and documentlink.DataLinkID = @DataLinkID
                        order by document_type, file_name
                    "
                >
                <SelectParameters>
                    <asp:Parameter Name="DataTypeID" Type="int32" />
                    <asp:Parameter Name="DataLinkID" Type="int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </div>
    
</div>
    
    <%--position:relative required for IE7 or else the div hangs around when you switch tabs.--%>
    <div style="position:relative;font-size:0.9em;border:1px solid #284E98;">
   
        <asp:Label ID="lblDocResults" runat="server" Visible="false" SkinID="lblResults"></asp:Label>
        
        <%--LINK DOCUMENT --%>
        
        <table id="tblDocLink" style="display:none;background-color:#B5C7DE;width:100%;margin-top:1px;font-size:0.9em;border-top:1px solid #284E98;" border=0>
            <tr>
                <td colspan="2">
                    <asp:UpdatePanel runat="server" ID="uPnlDocs" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="pnlLinkDoc" DefaultButton="btnSearchDocument">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        You can link an existing document to this record. Use the quick search to find the document you want to link. 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;width:175px;white-space:nowrap;">Quick Search:</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtDocumentSearch"></asp:TextBox>
                                        <asp:DropDownList runat="Server" ID="ddlAllDocumentTypes"
                                            DataSourceID="DS_AllDocumentTypes"
                                            DataTextField="Document_Type"
                                            DataValueField="DocumentTypeID"
                                            AppendDataBoundItems="true"
                                            Visible="false"
                                        >
                                            <asp:ListItem Text="All Document Types" Value="" />
                                        </asp:DropDownList>
                                        <asp:Button runat="server" ID="btnSearchDocument" Text="List" OnClick="btnSearchDocument_Click" />
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap style="text-align:right;width:175px;white-space:nowrap;">Existing Documents: </td>
                                    <td style="width:100%;">
                                        <asp:DropDownList runat="server" ID="ddlExistingDocuments"
                                            DataSourceID="dsExistingDocuments"
                                            DataTextField="filenm"
                                            DataValueField="documentID"
                                            >                                        
                                        </asp:DropDownList>
                                        <input type="button" id="btnDocInfo" onclick="alert('ok');" value="Info" />
                                        <asp:SqlDataSource ID="dsExistingDocuments" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:MCProd %>"
                                            ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
                                            SelectCommand="Select documentID, [File_Name] as [filenm] from document where documentID = 0"
                                            >
                                        </asp:SqlDataSource> 
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>                
                </td>
            </tr>
            <tr>
                <td nowrap style="text-align:right;width:177px;white-space:nowrap;">Document Type :</td>
                <td style="width:90%;">
                    <asp:DropDownList ID="ddlLinkDocumentType" runat="server"
                        DataSourceID="DS_DocumentType"
                        DataTextField="Document_Type"
                        DataValueField="DocumentTypeID"
                    >
                    </asp:DropDownList>
                    <asp:Button runat="server" ID="btnDocumentLink" Text="Link Document" OnClick="btnDocumentLink_Click" />
                    <input type="Button" id="btnDocumentCancel" value="Cancel" onclick="document.getElementById('tblDocLink').style.display = 'none';" />
                </td>
            </tr>
        </table>
        
        <%-- ATTACH DOCUMENT--%>
        <table id="tblDocUnique" style="display:none;background-color:#B5C7DE;width:100%;margin-top:1px;font-size:0.9em;border-top:1px solid #284E98;" border=1>
            <tr>
                <td nowrap style="text-align:right;width:175px;white-space:nowrap;">Document <span style="font-size:.75em;">(max 50mb)</span>: </td>
                <td style="width:100%;">
                    <asp:FileUpload width="100%" ID="FileDocumentUpload1" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;width:175px;white-space:nowrap;">Document Name :</td>
                <td>
                    <asp:TextBox width="98%" ID="txtDocumentFileName" MaxLength="255" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;width:175px;white-space:nowrap;">Document Type :</td>
                <td>
                    <asp:DropDownList ID="ddlDocumentType" runat="server"
                        DataSourceID="DS_DocumentType"
                        DataTextField="Document_Type"
                        DataValueField="DocumentTypeID"
                    >
                    </asp:DropDownList>
                </td>
            </tr>   
      
            <tr>
                <td style="padding-top:2px;">
                    
                </td>
                <td style="padding-top:2px;">
                    <asp:Button runat="server" ID="btnAddDocument" Text="Attach Document" OnClick="btnAddDocument_Click" />
                    <input type="Button" id="btnCancelDocument" value="Cancel" onclick="document.getElementById('tblDocUnique').style.display = 'none';" />
                </td>
            </tr>
        </table>
        
         <%--DATASOURCES --%>
        <asp:SqlDataSource ID="DS_DocumentType" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
            SelectCommand="SELECT [DocumentTypeID], [Document_Type] FROM [DocumentType] where DataTypeID = @DataTypeID order by Document_Type"
            >
            <SelectParameters>
                <asp:Parameter Name="DataTypeID" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DS_AllDocumentTypes" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
            SelectCommand="
                SELECT [DocumentTypeID], isNull(Data_Type,'') + ' - ' +  isNull([Document_Type],'') as [Document_Type] 
                    FROM [DocumentType]
	                LEFT JOIN DataType on DataType.DataTypeID = [DocumentType].[DataTypeID]
	                ORDER BY Data_Type, Document_Type
	            "
            >
        </asp:SqlDataSource>
        
    </div>