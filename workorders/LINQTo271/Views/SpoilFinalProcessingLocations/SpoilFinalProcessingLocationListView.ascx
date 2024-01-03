<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpoilFinalProcessingLocationListView.ascx.cs" Inherits="LINQTo271.Views.SpoilFinalProcessingLocations.SpoilFinalProcessingLocationListView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<mmsinc:MvpGridView runat="server" ID="gvSpoilFinalProcessingLocations" DataSourceID="odsSpoilFinalProcessingLocations"
    AutoGenerateColumns="false" DataKeyNames="SpoilFinalProcessingLocationID" OnSelectedIndexChanged="ListControl_SelectedIndexChanged"
    ShowEmptyTable="true" ShowFooter="true" OnDataBinding="ListControl_DataBinding" AllowSorting="true">
    <Columns>
        <asp:TemplateField HeaderText="Name" SortExpression="Name">
            <ItemTemplate>
                <asp:label runat="server" ID="lblName" Text='<%# Eval("Name") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtNameEdit" Text='<%# Bind("Name") %>' />
                <asp:RequiredFieldValidator runat="server" ID="rfvNameEdit" ControlToValidate="txtNameEdit"
                    Display="dynamic" ErrorMessage="Please enter a name for this location." ValidationGroup="LocationsUpdate" />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpTextBox runat="server" ID="txtName" Text='<%# Bind("Name") %>' />
                <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="txtName"
                    Display="dynamic" ErrorMessage="Please enter a name for this location." ValidationGroup="LocationsInsert" />
            </FooterTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Town" SortExpression="Town.Name">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblTown" Text='<%# Eval("Town") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlTownEdit" SelectedValue='<%# Bind("TownID") %>'
                    AppendDataBoundItems="true" DataSourceID="odsTowns" DataTextField="Name" DataValueField="TownID">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlTown" AppendDataBoundItems="true" DataSourceID="odsTowns"
                    DataTextField="Name" DataValueField="TownID">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
            </FooterTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Street" SortExpression="Street.FullStName">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblStreet" Text='<%# Eval("Street") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlStreetEdit" />
                <atk:cascadingdropdown runat="server" id="cddStreetEdit" targetcontrolid="ddlStreetEdit"
                    parentcontrolid="ddlTownEdit" category="Street" emptytext="Select A Town" emptyvalue=""
                    prompttext="--Select Here--" promptvalue="" loadingtext="[Loading Streets...]"
                    servicepath="~/Views/Streets/StreetsServiceView.asmx" servicemethod="GetStreetsByTown"
                    selectedvalue='<%# Bind("StreetID") %>' />
            </EditItemTemplate>
            <FooterTemplate>
                <mmsinc:MvpDropDownList runat="server" ID="ddlStreet" />
                <atk:cascadingdropdown runat="server" id="cddStreet" targetcontrolid="ddlStreet"
                    parentcontrolid="ddlTown" category="Street" emptytext="Select A Town" emptyvalue=""
                    prompttext="--Select Here--" promptvalue="" loadingtext="[Loading Streets...]"
                    servicepath="~/Views/Streets/StreetsServiceView.asmx" servicemethod="GetStreetsByTown" />
            </FooterTemplate>
        </asp:TemplateField>

        <asp:TemplateField ShowHeader="false">
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbEdit" CausesValidation="false" CommandName="Edit"
                    Text="Edit" />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton runat="server" ID="lbSave" CausesValidation="true" CommandName="Update"
                    Text="Update" ValidationGroup="LocationsUpdate" />
                <asp:LinkButton runat="server" ID="lbCancel" CausesValidation="false" CommandName="Cancel"
                    Text="Cancel" />
            </EditItemTemplate>
            <FooterTemplate>
                <asp:LinkButton runat="server" ID="lbInsert" CausesValidation="true"
                    Text="Insert" OnClick="lbInsert_Click" ValidationGroup="LocationsInsert" />
            </FooterTemplate>
        </asp:TemplateField>
    </Columns>
</mmsinc:MvpGridView>

<mmsinc:MvpObjectDataSource runat="server" ID="odsSpoilFinalProcessingLocations" TypeName="WorkOrders.Model.SpoilFinalProcessingLocationRepository"
    SelectMethod="SelectByOperatingCenterID" UpdateMethod="Update" InsertMethod="InsertSpoilFinalProcessingLocation"
    OnInserting="odsSpoilFinalProcessingLocations_Inserting" OnUpdating="odsSpoilFinalProcessingLocations_Updating"
    SortParameterName="sortExpression">
    <InsertParameters>
        <asp:Parameter name="OperatingCenterID" DbType="Int32" />
        <asp:Parameter name="Name" DbType="String" />
        <asp:Parameter name="TownID" DbType="Int32" />
        <asp:Parameter name="StreetID" DbType="Int32" />
    </InsertParameters>
    <SelectParameters>
        <asp:Parameter name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>

<mmsinc:MvpObjectDataSource runat="server" ID="odsTowns" TypeName="WorkOrders.Model.TownRepository"
    SelectMethod="SelectByOperatingCenterID">
    <SelectParameters>
        <asp:Parameter name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>
