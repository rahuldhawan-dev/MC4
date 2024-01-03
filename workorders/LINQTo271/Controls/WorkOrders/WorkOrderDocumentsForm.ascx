<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderDocumentsForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderDocumentsForm" %>
<%@ Import Namespace="MMSINC.Common" %>
<%@ Import Namespace="MMSINC.Interface" %>
<%@ Import Namespace="WorkOrders.Model" %>

<mmsinc:MvpLabel runat="server" ID="lblMessage" />
    <div class="container">
        <asp:HyperLink runat="server" Text="Add Document" CssClass="button"
                       Visible='<%# CurrentMvpMode == DetailViewMode.Edit %>'
                       NavigateUrl='<%# String.Format("../../../../Modules/mvc/FieldOperations/GeneralWorkOrder/Show/{0}#DocumentsTab", Eval("WorkOrderId")) %>' />
    </div>
    <div class="container">
        <mmsinc:MvpGridView runat="server" ID="gvDocuments" DataKeyNames="DocumentID"
        DataSourceID="odsDocuments" AutoGenerateColumns="false" OnRowCreated="gvDocuments_OnRowCreated"
        EmptyDataText="There are no documents attached to this work order.">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text="Edit" CssClass="button"
                                   Visible="<%# (CurrentMvpMode != DetailViewMode.ReadOnly && SecurityService.IsAdmin) %>"
                                   NavigateUrl='<%# String.Format("../../../../Modules/mvc/FieldOperations/GeneralWorkOrder/Show/{0}#DocumentsTab", Eval("WorkOrderId")) %>' />
                    <mmsinc:MvpLinkButton runat="server" ID="lbView" Text="View" CausesValidation="false"
                        docid='<%# Eval("DocumentID") %>' OnClientClick="return WorkOrderDocumentsForm.lbView_click(this);" />
                </ItemTemplate>
                <EditItemTemplate>
                    <mmsinc:MvpLinkButton runat="server" id="btnSave" Text="Update"
                        CommandName="Update" CommandArgument='<%# Eval("DocumentID") %>'
                        CausesValidation="true" ValidationGroup="vgGvDocument" />
                    <mmsinc:MvpLinkButton runat="server" id="btnCancel" Text="Cancel" 
                        CommandName="Cancel" CommandArgument='<%# Eval("DocumentID") %>'
                        CausesValidation="False" />
                </EditItemTemplate>
            </asp:templatefield>
            <asp:TemplateField HeaderText="File Name">
                <ItemTemplate><%# Eval("FileName") %></ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox runat="server" id="txtEditFileName" Text='<%# Bind("FileName") %>' />
                    <asp:RequiredFieldValidator runat="server" id="rfvEditFileName" 
                        Text="Required" ControlToValidate="txtEditFileName"
                        ValidationGroup="vgGvDocument" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Document Type">
                <ItemTemplate><%#Eval("DocumentType") %></ItemTemplate>
                <EditItemTemplate>
                    <mmsinc:MvpDropDownList runat="server" id="ddlDocumentType"
                        DataSourceID="dsDocumentTypes"
                        DataTextField="DocumentTypeName"
                        DataValueField="DocumentTypeID"
                        SelectedValue='<%#Bind("DocumentTypeID") %>'
                    />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Created On (EST)">
                <ItemTemplate><%#DataBinder.Eval(Container.DataItem, "CreatedOn", "{0:d}") %></ItemTemplate>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="Created By">
                <ItemTemplate><%#Eval("EmployeeCreatedBy") ?? Eval("CreatedBy") %></ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </mmsinc:MvpGridView>
    </div>
    <div class="container">
<%--        <asp:Hyperlink runat="server" 
            Visible='<%# HttpApplicationBase.IsProduction && Eval("AssetTypeID").ToString() == AssetTypeRepository.Indices.VALVE.ToString() %>' 
            Text="View Linked Images" 
            NavigateUrl='<%# "~/../../modules/data/valves/valvelink.aspx?recID=" + Eval("ValveID") %>'
            Target="_new" />
        <asp:Hyperlink runat="server" 
            Visible='<%# HttpApplicationBase.IsProduction && Eval("AssetTypeID").ToString() == AssetTypeRepository.Indices.SERVICE.ToString() %>'
            Text="View Linked Images" 
            NavigateUrl='<%# "~/../../modules/data/services/servicelink.aspx?premise=" + Eval("PremiseNumber") + "&service=" + Eval("ServiceNumber") %>'
            Target="_new" />--%>
    </div>
<style>.header { font-weight: bold; text-align: right; }</style>
<mmsinc:MvpPanel id="pnlDetailsView" runat="server" style="display: none;">
<mmsinc:MvpDetailsView runat="server" id="dvDocument"
    DataKeyNames="DocumentID" DataSourceID="odsDocument" DefaultMode="Insert"     
    AutoGenerateRows="false"
    EmptyDataText="No Record has been selected" 
    Width="100%">
    <Fields>
        <%-- NOTE: The file upload script that comes with AjaxToolkit does not work in Firefox 22. The stupid
                   fix is to give the file upload input an actual width. See this bug: http://ajaxcontroltoolkit.codeplex.com/workitem/27429 --%>
        <asp:TemplateField HeaderText="File" HeaderStyle-CssClass="header">
            <ItemTemplate>
                <div style="display: none;">
                    <mmsinc:MvpAsyncFileUpload runat="server" ID="afuDocument"                   
                        UploadingBackColor="Yellow"
                        OnUploadedComplete="afuDocument_UploadedComplete"
                        OnClientUploadStarted="WorkOrderDocumentsForm.fileUploadStarted"
                        OnClientUploadComplete="WorkOrderDocumentsForm.fileUploadComplete"
                        ClientIDMode="AutoID" CssClass="asyncFileUpload"  />
                </div>
            </ItemTemplate>
            <InsertItemTemplate>
                <mmsinc:MvpAsyncFileUpload runat="server" ID="afuDocument"                     
                    UploadingBackColor="Yellow"
                    OnUploadedComplete="afuDocument_UploadedComplete"                    
                    OnClientUploadStarted="WorkOrderDocumentsForm.fileUploadStarted"
                    OnClientUploadComplete="WorkOrderDocumentsForm.fileUploadComplete"
                    ClientIDMode="AutoID" CssClass="asyncFileUpload" />
            </InsertItemTemplate>
            <EditItemTemplate>
                <div style="display: none;">
                    <mmsinc:MvpAsyncFileUpload runat="server" ID="afuDocument"                     
                        UploadingBackColor="Yellow"
                        OnUploadedComplete="afuDocument_UploadedComplete"
                        OnClientUploadComplete="WorkOrderDocumentsForm.fileUploadComplete"
                        OnClientUploadStarted="WorkOrderDocumentsForm.fileUploadStarted"
                        ClientIDMode="AutoID" CssClass="asyncFileUpload" />
                </div>
            </EditItemTemplate>            
        </asp:TemplateField>
        <asp:TemplateField HeaderText="File Name" HeaderStyle-CssClass="header">
            <ItemTemplate></ItemTemplate>
            <InsertItemTemplate>
                <mmsinc:MvpTextBox runat="server" id="txtFileName"
                    Text='<%# Bind("FileName") %>'/>
                <asp:RequiredFieldValidator runat="server" id="rfvFileName"
                    ControlToValidate="txtFileName" Text="Required"
                    ValidationGroup="vgDvDocument" />            
            </InsertItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Document Type" HeaderStyle-CssClass="header">
            <ItemTemplate></ItemTemplate>
            <InsertItemTemplate>
               <mmsinc:MvpDropDownList runat="server" id="ddlDocumentType"
                    DataSourceID="dsDocumentTypes"
                    DataTextField="DocumentTypeName"
                    DataValueField="DocumentTypeID"
                    SelectedValue='<%# Bind("DocumentTypeID") %>'
                    AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>   
                <asp:RequiredFieldValidator runat="server" id="rfvDocumentType"
                    ControlToValidate="ddlDocumentType" Text="Required"
                    ValidationGroup="vgDvDocument" />             
            </InsertItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <InsertItemTemplate>
                <mmsinc:MvpButton runat="server" id="btnInsert" CommandName="Insert" Text="Insert"
                    disabled="disabled" CausesValidation="true" ValidationGroup="vgDvDocument"                     
                />
            </InsertItemTemplate>
        </asp:TemplateField>
    </Fields> 
</mmsinc:MvpDetailsView>
</mmsinc:MvpPanel>


<mmsinc:MvpObjectDataSource runat="server" ID="dsDocumentTypes"
    TypeName="WorkOrders.Model.DocumentTypeRepository"
    SelectMethod="SelectAllWorkOrderDocumentTypes"/>
    
<mmsinc:MvpObjectDataSource runat="server" id="odsDocuments"
    TypeName="WorkOrders.Model.DocumentRepository"
    SelectMethod="GetDocumentsForWorkOrder"
    InsertMethod="InsertDocument"
    UpdateMethod="UpdateDocument"
    OnUpdating="odsDocuments_Updating"
>
    <SelectParameters>
        <asp:Parameter Name="WorkOrderID" DbType="Int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Name="FileName" DbType="String" />
        <asp:Parameter Name="DocumentTypeID" DbType="Int32" />
        <asp:Parameter Name="CreatedByID" DbType="Int32" />
        <asp:Parameter Name="FileSize" DbType="Int32" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="DocumentID" DbType="Int32" />
        <asp:Parameter Name="FileName" DbType="String" />
        <asp:Parameter Name="DocumentTypeID" DbType="Int32" />
        <asp:Parameter Name="ModifiedOn" DbType="DateTime" />
        <asp:Parameter Name="ModifiedByID" DbType="Int32" />
    </UpdateParameters>

</mmsinc:MvpObjectDataSource>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsDocument" DataObjectTypeName="WorkOrders.Model.Document"
    OnInserted="ods_Inserted" />
