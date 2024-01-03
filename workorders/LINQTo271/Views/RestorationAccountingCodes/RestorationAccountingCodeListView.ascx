<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RestorationAccountingCodeListView.ascx.cs" Inherits="LINQTo271.Views.RestorationAccountingCodes.RestorationAccountingCodeListView" %>

<style>
    .valveInspectionTable select { width: 150px;}
    .valveInspectionTable input[type="text"] { width: 60px;}
</style>
<mmsinc:MvpLabel runat="server" ID="lblError" CssClass="error" EnableViewState="false" />
<mmsinc:MvpGridView runat="server" ID="gvRestorationAccountingCodes" 
    DataSourceID="odsRestorationAccountingCodes" AutoGenerateColumns="False" 
    DataKeyNames="RestorationAccountingCodeID"
    OnSelectedIndexChanged="ListControl_SelectedIndexChanged"
    OnRowDeleting="ListControl_RowDeleting" ShowFooter="True"
    OnRowUpdating="ListControl_RowUpdating" OnRowEditing="ListControl_RowEditing"
    OnRowCancelingEdit="ListControl_RowCancelingEdit"
    AllowSorting="False" CssClass="valveInspectionTable">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <mmsinc:MvpLinkButton runat="server" ID="lbEdit" CausesValidation="false" CommandName="Edit"
                    Text="Edit" />
                <mmsinc:MvpLinkButton runat="server" ID="lbDelete" CausesValidation="false" CommandName="Delete"
                    Text="Delete" OnClientClick="return confirm('Are you sure?');" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton runat="server" ID="lbSave" CausesValidation="true" CommandName="Update" 
                    Text="Update" />
                <asp:LinkButton runat="server" ID="lbCancel" CausesValidation="false" CommandName="Cancel"
                    Text="Cancel" />
            </EditItemTemplate>
            <FooterTemplate>
                <asp:LinkButton runat="server" ID="lbInsert" ValidationGroup="InsertGroup" CausesValidation="true" Text="Insert" OnClick="ListControl_RowInserting" />
                <asp:LinkButton runat="server" ID="lbCancel" ValidationGroup="InsertGroup" CausesValidation="false" CommandName="Cancel"
                    Text="Cancel" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="RestorationAccountingCodeID" HeaderText="Id" ReadOnly="True"/>
        <asp:TemplateField HeaderText="Code">
            <ItemTemplate><%#Eval("Code") %></ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtCode" Text='<%#Bind("Code") %>' MaxLength="8" CssClass="scrollIntoView" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCode" Text="Required" Display="Dynamic" CssClass="error" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtCode" ValidationGroup="InsertGroup" Text='<%#Bind("Code") %>' MaxLength="8" />
                <asp:RequiredFieldValidator runat="server" ValidationGroup="InsertGroup" ControlToValidate="txtCode" Text="Required" Display="Dynamic" CssClass="error" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="SubCode">
            <ItemTemplate><%#Eval("SubCode") %></ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtSubCode" Text='<%#Bind("SubCode") %>' MaxLength="2"/>
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtSubCode" Text='<%#Bind("SubCode") %>' MaxLength="2" ValidationGroup="InsertGroup" />
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
</mmsinc:MvpGridView>

<mmsinc:MvpObjectDataSource runat="server" ID="odsRestorationAccountingCodes" 
    TypeName="WorkOrders.Model.RestorationAccountingCodeRepository"
    SelectMethod="SelectAllAsList" 
    DataObjectTypeName="WorkOrders.Model.RestorationAccountingCode"
    UpdateMethod="Update" DeleteMethod="Delete"/>
    
<mmsinc:ScriptInclude  runat="server" ScriptFileName="RestorationAccountingCodes.js" IncludesPath="~/Views/RestorationAccountingCodes/" />