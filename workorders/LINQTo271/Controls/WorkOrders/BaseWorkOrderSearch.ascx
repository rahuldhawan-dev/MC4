<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BaseWorkOrderSearch.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.BaseWorkOrderSearch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<tr>
    <td colspan="2">
        <mmsinc:MvpLabel runat="server" ID="lblError" ForeColor="Red" />
    </td>
</tr>
<mmsinc:MvpTableRow runat="server" ID="trWorkOrderNumber">
    <asp:TableCell runat="server">Work Order Number:</asp:TableCell>
    <asp:TableCell runat="server">
        <mmsinc:MvpTextBox runat="server" ID="txtWorkOrderNumber" />
<%--        <atk:MaskedEditExtender runat="server" ID="meeWorkOrderNumber" TargetControlID="txtWorkOrderNumber"
            Mask="99999" MaskType="Number" AutoComplete="false" />--%>
    </asp:TableCell>
</mmsinc:MvpTableRow>
<tr>
    <td style="text-align: center" colspan="2">--OR--</td>
</tr>

<mmsinc:MvpTableRow runat="server" ID="trOperatingCenter">
    <asp:TableCell runat="server">Operating Center</asp:TableCell>
    <asp:TableCell runat="server">
        <mmsinc:MvpDropDownList runat="server" ID="ddlOperatingCenter" DataSourceID="odsOperatingCenters"
            DataTextField="FullDescription" DataValueField="OperatingCenterID" AppendDataBoundItems="true"
            OnDataBound="ddlOperatingCenter_DataBound">
            <asp:ListItem Text="--Select Here--" Value="" />
        </mmsinc:MvpDropDownList>
    </asp:TableCell>
</mmsinc:MvpTableRow>

<mmsinc:MvpTableRow runat="server" ID="trTown">
    <asp:TableCell runat="server">Town:</asp:TableCell>
    <asp:TableCell runat="server">
        <mmsinc:MvpDropDownList ID="ddlTown" runat="server" />
        <atk:CascadingDropDown runat="server" ID="cddTowns" TargetControlID="ddlTown" ParentControlID="ddlOperatingCenter"
            Category="Town" EmptyText="None Found" EmptyValue="" PromptText="--Select Here--"
            PromptValue="" ServicePath="~/Views/Towns/TownsServiceView.asmx" ServiceMethod="GetTownsByOperatingCenterID" />
    </asp:TableCell>
</mmsinc:MvpTableRow>
<mmsinc:MvpTableRow runat="server" ID="trTownSection">
    <asp:TableCell runat="server">Town Section:</asp:TableCell>
    <asp:TableCell runat="server">
        <mmsinc:MvpDropDownList ID="ddlTownSection" runat="server" />
        <atk:CascadingDropDown runat="server" ID="cddTownSection" TargetControlID="ddlTownSection"
            ParentControlID="ddlTown" Category="TownSection" EmptyText="None Found" EmptyValue=""
            PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Town Sections...]"
            ServicePath="~/Views/TownSections/TownSectionsServiceView.asmx" ServiceMethod="GetTownSectionsByTownDefined" />
    </asp:TableCell>
</mmsinc:MvpTableRow>
<mmsinc:MvpTableRow runat="server" ID="trStreet">
    <asp:TableCell runat="server">Street:</asp:TableCell>
    <asp:TableCell runat="server">
        <mmsinc:MvpDropDownList runat="server" ID="ddlStreet" />
        <atk:CascadingDropDown runat="server" ID="cddStreet" TargetControlID="ddlStreet"
            ParentControlID="ddlTown" Category="Street" EmptyText="Select A Town" EmptyValue=""
            PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Streets...]"
            ServicePath="~/Views/Streets/StreetsServiceView.asmx" ServiceMethod="GetStreetsByTownDefined" />
    </asp:TableCell>
</mmsinc:MvpTableRow>
<mmsinc:MvpTableRow runat="server" ID="trStreetNumber">
    <asp:TableCell runat="server">Street Number:</asp:TableCell>
    <asp:TableCell runat="server">
        <mmsinc:MvpTextBox runat="server" ID="txtStreetNumber" />
    </asp:TableCell>
</mmsinc:MvpTableRow>
<mmsinc:MvpTableRow runat="server" ID="trApartmentAddtl">
    <asp:TableCell runat="server">Apartment Addtl:</asp:TableCell>
    <asp:TableCell runat="server">
        <mmsinc:MvpTextBox runat="server" ID="txtApartmentAddtl" />
    </asp:TableCell>
</mmsinc:MvpTableRow>
<mmsinc:MvpTableRow runat="server" ID="trNearestCrossStreet">
    <asp:TableCell runat="server">Nearest Cross Street:</asp:TableCell>
    <asp:TableCell runat="server">
        <mmsinc:MvpDropDownList runat="server" ID="ddlNearestCrossStreet" />
        <atk:CascadingDropDown runat="server" ID="cddNearestCrossStreet" TargetControlID="ddlNearestCrossStreet"
            ParentControlID="ddlTown" Category="Street" EmptyText="Select A Town" EmptyValue=""
            PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Streets...]"
            ServicePath="~/Views/Streets/StreetsServiceView.asmx" ServiceMethod="GetStreetsByTownDefined" />
    </asp:TableCell>
</mmsinc:MvpTableRow>
<mmsinc:MvpTableRow runat="server" ID="trAssetType">
    <asp:TableCell runat="server">Asset Type:</asp:TableCell>
    <asp:TableCell runat="server">
        <mmsinc:MvpDropDownList ID="ddlAssetType" runat="server" />
        <atk:CascadingDropDown runat="server" ID="cddAssetType" TargetControlID="ddlAssetType"
            ParentControlID="ddlOperatingCenter" Category="AssetType" EmptyText="Select an OperatingCenter" EmptyValue=""
            PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Asset Types...]"
            ServicePath="~/Views/AssetTypes/AssetTypesServiceView.asmx" ServiceMethod="GetAssetTypesByOperatingCenter"
            />
    </asp:TableCell>
</mmsinc:MvpTableRow>
<mmsinc:MvpTableRow runat="server" ID="trDescriptionOfWork">
    <asp:TableCell runat="server">Description of Work:</asp:TableCell>
    <asp:TableCell runat="server">
        <mmsinc:MvpListBox runat="server" ID="lstDescriptionOfWork" DataSourceID="odsWorkDescriptions"
            DataTextField="Description" SelectionMode="Multiple" DataValueField="WorkDescriptionID"
            Width="350px" />
        <asp:ObjectDataSource runat="server" ID="odsWorkDescriptions" TypeName="WorkOrders.Model.WorkDescriptionRepository"
            SelectMethod="SelectAllSorted" />
    </asp:TableCell>
</mmsinc:MvpTableRow>

<asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
    SelectMethod="SelectUserOperatingCenters" />