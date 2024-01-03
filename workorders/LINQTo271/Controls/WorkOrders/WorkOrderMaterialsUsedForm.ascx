<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderMaterialsUsedForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderMaterialsUsedForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<mmsinc:MvpGridView runat="server" ID="gvMaterialsUsed" DataSourceID="odsMaterialsUsed"
    AutoGenerateColumns="false" DataKeyNames="MaterialsUsedID" ShowFooter="true">
    <Columns>
        <asp:TemplateField HeaderText="Part Number">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblPartNumber" Text='<%# Eval("PartNumber") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlPartNumberEdit" AppendDataBoundItems="true"
                    DataSourceID="odsOperatingCenterStockedMaterials" DataTextField="PartNumber"
                    DataValueField="MaterialID" onchange="WorkOrderMaterialsUsedForm.ddlPartNumber_Change(this)" SelectedValue='<%# Bind("MaterialID") %>'>
                    <asp:ListItem Text="n/a" Value="" />
                </mmsinc:MvpDropDownList>
                <asp:CustomValidator ID="cvPartNumberEdit" runat="server" ControlToValidate="ddlPartNumberEdit"
                    OnServerValidate="ddlPartNumberEdit_Validate" ErrorMessage="You must choose a part number AND stock location"
                    Display="dynamic" ValidationGroup="MaterialsUpdate">
                </asp:CustomValidator>
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlPartNumber" AppendDataBoundItems="true" DataSourceID="odsOperatingCenterStockedMaterials"
                    DataTextField="PartNumber" DataValueField="MaterialID" onchange="WorkOrderMaterialsUsedForm.ddlPartNumber_Change(this)">
                    <asp:ListItem Text="n/a" Value="" />
                </mmsinc:MvpDropDownList>
                <asp:CustomValidator ID="cvPartNumber" runat="server" ControlToValidate="ddlPartNumber"
                    OnServerValidate="ddlPartNumber_Validate" ErrorMessage="You must choose a part number AND stock location"
                    Display="dynamic" ValidationGroup="MaterialsInsert">
                </asp:CustomValidator>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Stock Location">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblStockLocation" Text='<%# Eval("StockLocation") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlStockLocationEdit" AppendDataBoundItems="true"
                    DataSourceID="odsAllStockLocations" DataTextField="Description" DataValueField="StockLocationID"
                    SelectedValue='<%# Bind("StockLocationID") %>'>
                    <asp:ListItem Text="n/a" Value="" />
                </mmsinc:MvpDropDownList>
                <asp:CustomValidator ID="cvStockLocationEdit" runat="server" ControlToValidate="ddlStockLocationEdit"
                    OnServerValidate="ddlPartNumberEdit_Validate" ErrorMessage="You must choose a part number AND stock location"
                    Display="dynamic" ValidationGroup="MaterialsUpdate">
                </asp:CustomValidator>
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlStockLocation" AppendDataBoundItems="true"
                    DataSourceID="odsActiveStockLocations" DataTextField="Description" DataValueField="StockLocationID">
                    <asp:ListItem Text="n/a" Value="" />
                </mmsinc:MvpDropDownList>
                <asp:CustomValidator ID="cvStockLocation" runat="server" ControlToValidate="ddlStockLocation"
                    OnServerValidate="ddlPartNumber_Validate" ErrorMessage="You must choose a part number AND stock location"
                    Display="dynamic" ValidationGroup="MaterialsInsert">
                </asp:CustomValidator>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="SAP Stock Location">
            <ItemTemplate><%#Eval("StockLocation.SAPStockLocation") %></ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Description">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblDescription" Text='<%# Eval("Description") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtNonStockDescriptionEdit" Text='<%# Bind("Description") %>' />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtNonStockDescription" />
                <span id="lblDescription"></span>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Quantity">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Quantity") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtQuantityEdit" Text='<%# Bind("Quantity") %>' Width="70px" />
                <asp:RequiredFieldValidator runat="server" ID="rfvLbUpdateQuantity" ControlToValidate="txtQuantityEdit"
                    Display="Dynamic" ErrorMessage="Required" ValidationGroup="MaterialsUpdate" />
                <asp:RegularExpressionValidator runat="server" ID="revQuantity" ControlToValidate="txtQuantityEdit"
                                                ValidationExpression="^[0-9]+$" ErrorMessage="Must be a whole number"
                                                Display="Dynamic" ValidationGroup="MaterialsInsert"></asp:RegularExpressionValidator>
                </FooterTemplate>
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtQuantity" Width="70px" />
                <asp:RequiredFieldValidator runat="server" ID="rfvLbInsert" ControlToValidate="txtQuantity"
                    Display="Dynamic" ErrorMessage="Required" ValidationGroup="MaterialsInsert" />
                <asp:RegularExpressionValidator runat="server" ID="revQuantity" ControlToValidate="txtQuantity"
                                     ValidationExpression="^[0-9]+$" ErrorMessage="Must be a whole number"
                                     Display="Dynamic" ValidationGroup="MaterialsInsert"></asp:RegularExpressionValidator>
            </FooterTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="false">
            <ItemTemplate>
                <mmsinc:MvpLinkButton runat="server" ID="lbEdit" CausesValidation="false" CommandName="Edit"
                    Text="Edit" CssClass="button" />
                <mmsinc:MvpLinkButton runat="server" ID="lbDelete" CausesValidation="false" CommandName="Delete"
                    Text="Delete" OnClientClick="return confirm('Are you sure?');" />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpLinkButton runat="server" ID="lbSave" CausesValidation="true" CommandName="Update"
                    Text="Update" ValidationGroup="MaterialsUpdate" CssClass="button" />
                <mmsinc:MvpLinkButton runat="server" ID="lbCancel" CausesValidation="false" CommandName="Cancel"
                    Text="Cancel" CssClass="button" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpLinkButton runat="server" ID="lbInsert" CausesValidation="true"
                    Text="Insert" OnClick="lbInsert_Click" ValidationGroup="MaterialsInsert" CssClass="button" />
                <mmsinc:MvpLinkButton runat="server" ID="lbCancel" CausesValidation="false" CommandName="Cancel"
                    Text="Cancel" OnClick="lbCancel_Click" CssClass="button" />
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
</mmsinc:MvpGridView>

<mmsinc:MvpObjectDataSource runat="server" ID="odsOperatingCenterStockedMaterials" SelectMethod="GetStockedMaterialsByOperatingCenter"
    TypeName="WorkOrders.Model.MaterialRepository">
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" DbType="Int32" />
        <asp:Parameter Name="activeMaterialsOnly" DbType="Boolean" DefaultValue="true" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>

<mmsinc:MvpObjectDataSource runat="server" ID="odsActiveStockLocations" SelectMethod="SelectActiveByOperatingCenter"
    TypeName="WorkOrders.Model.StockLocationRepository">
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>
<mmsinc:MvpObjectDataSource runat="server" ID="odsAllStockLocations" SelectMethod="SelectByOperatingCenter"
    TypeName="WorkOrders.Model.StockLocationRepository">
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>

<mmsinc:MvpObjectDataSource runat="server" ID="odsMaterialsUsed" TypeName="WorkOrders.Model.MaterialsUsedRepository"
    SelectMethod="GetMaterialsUsedByWorkOrder" InsertMethod="InsertMaterialUsed" 
    UpdateMethod="UpdateMaterialUsed" DeleteMethod="DeleteMaterialUsed"
    OnInserting="odsMaterialsUsed_Inserting">
    <SelectParameters>
        <asp:Parameter DbType="Int32" Name="WorkOrderID" />
    </SelectParameters>
    <InsertParameters>
        <asp:Parameter Name="workOrderID" Type="Int32" />
        <asp:Parameter Name="materialID" Type="Int32" ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="description" Type="String" />
        <asp:Parameter Name="quantity" Type="Int32" />
        <asp:Parameter Name="stockLocationID" Type="Int32" ConvertEmptyStringToNull="true" />
    </InsertParameters>
</mmsinc:MvpObjectDataSource>
