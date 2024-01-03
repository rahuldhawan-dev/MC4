<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpoilRemovalListView.ascx.cs" Inherits="LINQTo271.Views.SpoilRemovals.SpoilRemovalListView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<mmsinc:MvpGridView runat="server" ID="gvSpoilRemovals" DataSourceID="odsSpoilRemovals"
    AutoGenerateColumns="false" DataKeyNames="SpoilRemovalID" OnSelectedIndexChanged="ListControl_SelectedIndexChanged"
    ShowFooter="true" OnDataBinding="ListControl_DataBinding" AllowSorting="true">
    <Columns>
        <%--RemovedFrom--%>
        <asp:TemplateField HeaderText="Removed From" SortExpression="RemovedFrom.Name">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblRemovedFrom" Text='<%# Eval("RemovedFrom.Name") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlSpoilStorageLocation" SelectedValue='<%# Bind("RemovedFromID") %>'
                    DataSourceID="odsSpoilStorageLocation" DataTextField="Name" DataValueField="SpoilStorageLocationID"
                    AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
                <asp:RequiredFieldValidator runat="server" ID="rfvRemovedFrom" 
                    ControlToValidate="ddlSpoilStorageLocation" Display="dynamic" InitialValue="" 
                    ErrorMessage="Please select the removed from location." ValidationGroup="SpoilRemovalEdit" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlSpoilStorageLocation" DataSourceID="odsSpoilStorageLocation"
                    DataTextField="Name" DataValueField="SpoilStorageLocationID" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
                <asp:RequiredFieldValidator runat="server" ID="rfvRemovedFrom" 
                    ControlToValidate="ddlSpoilStorageLocation" Display="dynamic" InitialValue="" 
                    ErrorMessage="Please select the removed from location." ValidationGroup="SpoilRemovalInsert" />
            </FooterTemplate>
        </asp:TemplateField>
        
        <%--FinalDestination--%>
        <asp:TemplateField HeaderText="Final Destination" SortExpression="FinalDestination.Name">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblFinalDestination" Text='<%# Eval("FinalDestination.Name") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlSpoilFinalProcessingLocation" SelectedValue='<%# Bind("FinalDestinationID") %>'
                    DataSourceID="odsSpoilFinalProcessingLocation" DataTextField="Name" DataValueField="SpoilFinalProcessingLocationID"
                    AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
                <asp:RequiredFieldValidator runat="server" ID="rfvSpoilFinalProcessingLocation" 
                    ControlToValidate="ddlSpoilFinalProcessingLocation" Display="dynamic" InitialValue="" 
                    ErrorMessage="Please select the final destination." ValidationGroup="SpoilRemovalEdit" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlSpoilFinalProcessingLocation" DataSourceID="odsSpoilFinalProcessingLocation"
                    DataTextField="Name" DataValueField="SpoilFinalProcessingLocationID" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
                <asp:RequiredFieldValidator runat="server" ID="rfvSpoilFinalProcessingLocation" 
                    ControlToValidate="ddlSpoilFinalProcessingLocation" Display="dynamic" InitialValue="" 
                    ErrorMessage="Please select the final destination." ValidationGroup="SpoilRemovalInsert" />
            </FooterTemplate>
        </asp:TemplateField>        

        <%--DateRemoved--%>
        <asp:TemplateField HeaderText="Date Removed" SortExpression="DateRemoved">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblDateRemoved" Text='<%# Eval("DateRemoved", "{0:d}") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtDateRemoved" Text='<%# Bind("DateRemoved", "{0:d}") %>' autocomplete="off" />
                <atk:CalendarExtender runat="server" ID="ceDateRemoved" TargetControlID="txtDateRemoved" />
                <asp:CompareValidator runat="server" ID="cvDateRemoved" ControlToValidate="txtDateRemoved" 
                    Operator="DataTypeCheck" Type="Date" ErrorMessage="Please enter a date."
                    Display="dynamic" ValidationGroup="SpoilRemovalEdit" />
                <asp:RequiredFieldValidator runat="server" ID="rfvDateRemoved" ControlToValidate="txtDateRemoved"
                    Display="dynamic" ErrorMessage="Please enter a date." ValidationGroup="SpoilRemovalEdit" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtDateRemoved" autocomplete="off" />
                <atk:CalendarExtender runat="server" ID="ceDateRemoved" TargetControlID="txtDateRemoved" />
                <asp:CompareValidator runat="server" ID="cvDateRemoved" ControlToValidate="txtDateRemoved" 
                    Operator="DataTypeCheck" Type="Date" ErrorMessage="Please enter a date."
                    Display="dynamic" ValidationGroup="SpoilRemovalInsert" />
                <asp:RequiredFieldValidator runat="server" ID="rfvDateRemoved" ControlToValidate="txtDateRemoved"
                    Display="dynamic" ErrorMessage="Please enter a date." ValidationGroup="SpoilRemovalInsert" />
            </FooterTemplate>
        </asp:TemplateField>
        
        <%--Quantity--%>
        <asp:TemplateField HeaderText="Quantity (cubic yards)" SortExpression="Quantity">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Quantity") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtQuantity" Text='<%# Bind("Quantity") %>' />
                <asp:CompareValidator runat="server" ID="cvQuantity" ControlToValidate="txtQuantity" 
                    Operator="DataTypeCheck" Type="Double" ErrorMessage="Please enter a numeric quantity"
                    Display="dynamic" ValidationGroup="SpoilRemovalEdit"  />
                <asp:RequiredFieldValidator runat="server" ID="rfvQuantity" ControlToValidate="txtQuantity"
                    Display="dynamic" ErrorMessage="Please enter a quantity." ValidationGroup="SpoilRemovalEdit" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtQuantity" />
                <asp:CompareValidator runat="server" ID="cvQuantity" ControlToValidate="txtQuantity" 
                    Operator="DataTypeCheck" Type="Double" ErrorMessage="Please enter a numeric quantity"
                    Display="dynamic" ValidationGroup="SpoilRemovalInsert" />
                <asp:RequiredFieldValidator runat="server" ID="rfvQuantity" ControlToValidate="txtQuantity"
                    Display="dynamic" ErrorMessage="Please enter a quantity." ValidationGroup="SpoilRemovalInsert" />
            </FooterTemplate>
        </asp:TemplateField>
    
        <%--Buttons--%>
        <asp:TemplateField ShowHeader="false">
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbEdit" CausesValidation="false" CommandName="Edit"
                    Text="Edit" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton runat="server" ID="lbSave" CausesValidation="false" CommandName="Update"
                    Text="Update" ValidationGroup="SpoilRemovalEdit" />
                <asp:LinkButton runat="server" ID="lbCancel" CausesValidation="false" CommandName="Cancel"
                    Text="Cancel" />
            </EditItemTemplate>
            <FooterTemplate>
                <asp:LinkButton runat="server" ID="lbInsert" CausesValidation="true"
                    Text="Insert" OnClick="lbInsert_Click" ValidationGroup="SpoilRemovalInsert" />
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
</mmsinc:MvpGridView>

<mmsinc:MvpObjectDataSource runat="server" ID="odsSpoilRemovals" TypeName="WorkOrders.Model.SpoilRemovalRepository"
    SelectMethod="SelectLastTenByOperatingCenter" UpdateMethod="Update" InsertMethod="InsertSpoilRemoval"
    OnInserting="odsSpoilRemovals_Inserting" SortParameterName="sortExpression">
    <InsertParameters>
        <asp:Parameter Name="removedFrom" DbType="Int32" />
        <asp:Parameter Name="finalDestination" DbType="Int32" />
        <asp:Parameter Name="dateRemoved" DbType="datetime" />
        <asp:Parameter Name="quantity" DbType="Decimal" />
    </InsertParameters>
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>

<mmsinc:MvpObjectDataSource runat="server" ID="odsSpoilStorageLocation" TypeName="WorkOrders.Model.SpoilStorageLocationRepository"
    SelectMethod="SelectByOperatingCenterID">
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>

<mmsinc:MvpObjectDataSource runat="server" ID="odsSpoilFinalProcessingLocation" TypeName="WorkOrders.Model.SpoilFinalProcessingLocationRepository"
    SelectMethod="SelectByOperatingCenterID">
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>
