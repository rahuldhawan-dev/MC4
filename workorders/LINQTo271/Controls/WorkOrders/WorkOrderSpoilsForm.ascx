<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderSpoilsForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderSpoilsForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<mmsinc:MvpGridView runat="server" ID="gvSpoils" AutoGenerateColumns="false" ShowFooter="true" DataSourceID="odsSpoils" DataKeyNames="SpoilID">
    <Columns>
        <asp:TemplateField HeaderText="Quantity (CY)">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Quantity") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtQuantityEdit" Text='<%# Bind("Quantity") %>' />
                <asp:RequiredFieldValidator runat="server" ID="rfvQuantity" ControlToValidate="txtQuantityEdit"
                    ErrorMessage="You must enter the quantity." Display="dynamic" ValidationGroup="SpoilsUpdate" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtQuantity" />
                <asp:RequiredFieldValidator runat="server" ID="rfvQuantity" ControlToValidate="txtQuantity"
                    ErrorMessage="You must enter the quantity." Display="dynamic" ValidationGroup="SpoilsInsert" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Storage Location">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("SpoilStorageLocation") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlSpoilStorageLocationEdit" AppendDataBoundItems="true"
                    DataSourceID="odsOperatingCenterSpoilStorageLocations" DataTextField="Name" DataValueField="SpoilStorageLocationID"
                    SelectedValue='<%# Bind("SpoilStorageLocationID") %>'>
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
                <asp:RequiredFieldValidator runat="server" ID="rfvSpoilStorageLocation" ControlToValidate="ddlSpoilStorageLocationEdit"
                    ErrorMessage="You must choose a Storage Location." Display="dynamic" ValidationGroup="SpoilsUpdate" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlSpoilStorageLocation" AppendDataBoundItems="true"
                    DataSourceID="odsOperatingCenterSpoilStorageLocations" DataTextField="Name" DataValueField="SpoilStorageLocationID">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
                <asp:RequiredFieldValidator runat="server" ID="rfvSpoilStorageLocation" ControlToValidate="ddlSpoilStorageLocation"
                    ErrorMessage="You must choose a Storage Location." Display="dynamic" ValidationGroup="SpoilsInsert" />
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="false">
            <ItemTemplate>
                <mmsinc:MvpLinkButton runat="server" ID="lbEdit" CssClass="button" CausesValidation="false" CommandName="Edit"
                    Text="Edit" />
                <mmsinc:MvpLinkButton runat="server" ID="lbDelete" CssClass="button" CausesValidation="false" CommandName="Delete"
                    Text="Delete" OnClientClick="return confirm('Are you sure?');" />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpLinkButton runat="server" ID="lbSave" CssClass="button" CausesValidation="true" CommandName="Update"
                    Text="Update" ValidationGroup="SpoilsUpdate" />
                <mmsinc:MvpLinkButton runat="server" ID="lbCancel" CssClass="button" CausesValidation="false" CommandName="Cancel"
                    Text="Cancel" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpLinkButton runat="server" ID="lbInsert" CssClass="button" CausesValidation="true"
                    Text="Insert" OnClick="lbInsert_Click" ValidationGroup="SpoilsInsert" />
                <mmsinc:MvpLinkButton runat="server" ID="lbCancel" CssClass="button" CausesValidation="false" CommandName="Cancel"
                    Text="Cancel" OnClick="lbCancel_Click" />
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
</mmsinc:MvpGridView>

<mmsinc:MvpObjectDataSource runat="server" ID="odsOperatingCenterSpoilStorageLocations"
    TypeName="WorkOrders.Model.SpoilStorageLocationRepository" SelectMethod="SelectByOperatingCenterID">
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>

<mmsinc:MvpObjectDataSource runat="server" ID="odsSpoils" TypeName="WorkOrders.Model.SpoilRepository"
    SelectMethod="GetSpoilsByWorkOrder" InsertMethod="InsertSpoil" UpdateMethod="UpdateSpoil"
    DeleteMethod="DeleteSpoil" OnInserting="odsSpoils_Inserting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" Name="WorkOrderID" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter DbType="Int32" Name="WorkOrderID" />
        <asp:Parameter DbType="Decimal" Name="Quantity" />
        <asp:Parameter DbType="Int32" Name="SpoilStorageLocationID" />
    </InsertParameters>
</mmsinc:MvpObjectDataSource>