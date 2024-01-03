<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaterialListView.ascx.cs" Inherits="LINQTo271.Views.Materials.MaterialListView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="mmsinc" Namespace="MMSINC.Controls" Assembly="MMSINC.Core.WebForms" %>

<mmsinc:MvpGridView runat="server" ID="gvMaterials" DataSourceID="odsMaterials"
    AutoGenerateEditButton="true" AutoGenerateColumns="false" DataKeyNames="MaterialID"
    OnSelectedIndexChanged="ListControl_SelectedIndexChanged" 
    OnRowUpdating="ListControl_RowUpdating"
    OnRowEditing="ListControl_RowEditing"
    OnRowCancelingEdit="ListControl_RowCancelingEdit"
    AllowSorting="false">
    <Columns>
        <asp:CheckBoxField DataField="IsActive" HeaderText="Active" />
        <asp:BoundField DataField="PartNumber" HeaderText="Part #" />
        <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True" />
        <asp:CheckBoxField DataField="DoNotOrder" HeaderText="Do Not Order" />
    </Columns>
</mmsinc:MvpGridView>
    
<asp:ObjectDataSource runat="server" ID="odsMaterials" TypeName="WorkOrders.Model.MaterialRepository" 
    SelectMethod="SelectAllAsList" DataObjectTypeName="WorkOrders.Model.Material" UpdateMethod="Update"/>
