<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RestorationProductCodeListView.ascx.cs" Inherits="LINQTo271.Views.RestorationProductCodes.RestorationProductCodeListView" %>

<style>
    .valveInspectionTable select { width: 150px;}
    .valveInspectionTable input[type="text"] { width: 60px;}
</style>
<mmsinc:MvpLabel runat="server" ID="lblError" CssClass="error" EnableViewState="false" />
<mmsinc:MvpGridView runat="server" ID="gvRestorationProductCodes" 
    DataSourceID="odsRestorationProductCodes" AutoGenerateColumns="False" 
    DataKeyNames="RestorationProductCodeID"
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
        <asp:BoundField DataField="RestorationProductCodeID" HeaderText="Id" ReadOnly="True"/>
        <asp:TemplateField HeaderText="Code">
            <ItemTemplate><%#Eval("Code") %></ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtCode" Text='<%#Bind("Code") %>' MaxLength="4"/>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCode" Text="Required" Display="Dynamic" CssClass="error" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpTextBox runat="server" ValidationGroup="InsertGroup" ID="txtCode" Text='<%#Bind("Code") %>' MaxLength="4"/>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="InsertGroup" ControlToValidate="txtCode" Text="Required" Display="Dynamic" CssClass="error" />
            </FooterTemplate>
        </asp:TemplateField>

    </Columns>
</mmsinc:MvpGridView>

<asp:ObjectDataSource runat="server" ID="odsRestorationProductCodes" 
    TypeName="WorkOrders.Model.RestorationProductCodeRepository"
    SelectMethod="SelectAllAsList" 
    DataObjectTypeName="WorkOrders.Model.RestorationProductCode"
    UpdateMethod="Update" DeleteMethod="Delete" />