<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderGeneralSearchView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.General.WorkOrderGeneralSearchView" %>
<%@ Register TagPrefix="wo" TagName="BaseWorkOrderSearch" Src="~/Controls/WorkOrders/BaseWorkOrderSearch.ascx" %>
<%@ Register TagPrefix="wo" TagName="AssetTypeIDsScript" Src="~/Views/AssetTypes/AssetTypesJSView.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="mmsinc" TagName="DateRange" Src="~/Common/DateRange.ascx" %>

    <table class="centered">
        <wo:BaseWorkOrderSearch runat="server" ID="baseSearch" ShowAssetType="false" ShowDescriptionOfWork="false" />
        <tr>
            <td>Asset Type:</td>
            <td>
                <mmsinc:MvpDropDownList ID="ddlAssetType" runat="server" onchange="WorkOrderGeneralSearchView.ddlAssetType_Change(this)"/>
                <atk:CascadingDropDown runat="server" ID="cddAssetType" TargetControlID="ddlAssetType"
                    ParentControlID="baseSearch$ddlOperatingCenter" Category="AssetType" EmptyText="Select an OperatingCenter" EmptyValue=""
                    PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Asset Types...]"
                    ServicePath="~/Views/AssetTypes/AssetTypesServiceView.asmx" ServiceMethod="GetAssetTypesByOperatingCenter"
                    SelectedValue='<%# Bind("AssetTypeID") %>'
                    />
            </td>
        </tr>
        <tr>
            <td>Last Crew Assigned:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlLastCrewAssigned" />
                <atk:CascadingDropDown runat="server" ID="cddLastCrewAssigned" TargetControlID="ddlLastCrewAssigned"
                                       ParentControlID="baseSearch$ddlOperatingCenter" Category="Crew" EmptyText="Select an OperatingCenter"
                                       EmptyValue="" PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Crews...]"
                                       ServicePath="~/Views/Crews/CrewsServiceView.asmx" ServiceMethod="GetCrewsByOperatingCenterID"
                                       SelectedValue='<%# Bind("LastCrewID") %>' />
            </td>
        </tr>
        <tr id="trAssetID" style="display:none">
            <td id="tdAssetIDLabel">AssetID:</td>
            <td><mmsinc:MvpTextBox runat="server" ID="txtAssetID" /></td>
        </tr>
        <tr>
            <td>Description of Work:</td>
            <td>
                <mmsinc:MvpListBox runat="server" ID="lstDescriptionOfWork" DataSourceID="odsWorkDescriptions"
                    DataTextField="Description" SelectionMode="Multiple" DataValueField="WorkDescriptionID"
                    Width="350px" />
                <asp:ObjectDataSource runat="server" ID="odsWorkDescriptions" TypeName="WorkOrders.Model.WorkDescriptionRepository"
                    SelectMethod="SelectAllSorted" />
            </td>
        </tr>
        <tr id="trSAPNotificationNumber" >
            <td>SAP Notification #:</td>
            <td><mmsinc:MvpTextBox runat="server" ID="txtSAPNotificationNumber" /></td>
        </tr>
        <tr id="trSAPWorkOrderNumber" >
            <td>SAP Work Order #:</td>
            <td><mmsinc:MvpTextBox runat="server" ID="txtSAPWorkOrderNumber" /></td>
        </tr>
        <tr>
            <td>Document Types:</td>
            <td>
                <mmsinc:MvpListBox runat="server" ID="lstDocumentType" DataSourceID="odsDocumentTypes"
                    DataTextField="DocumentTypeName" SelectionMode="Multiple" DataValueField="DocumentTypeID"
                    Width="350px" />
                <asp:ObjectDataSource runat="server" ID="odsDocumentTypes" TypeName="WorkOrders.Model.DocumentTypeRepository"
                    SelectMethod="SelectAllWorkOrderDocumentTypes" />
            </td>
        </tr>
        <tr>
            <td>
                <mmsinc:MvpDropDownList ID="ddlDateType" runat="server">
                    <asp:ListItem Value="DateReceived"></asp:ListItem>
                    <asp:ListItem Value="DateCompleted"></asp:ListItem>
                    <asp:ListItem Value="DateDocumentAttached"></asp:ListItem>
                    <asp:ListItem Value="CancelledAt"></asp:ListItem>
                </mmsinc:MvpDropDownList>
            </td>
            <td>
                <mmsinc:DateRange runat="server" ID="drDateToSearch" />
            </td>
        </tr>
        <tr>
            <td>Completed?</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlCompleted">
                    <asp:ListItem Text="" Value="" />
                    <asp:ListItem Text="Yes" Value="true" />
                    <asp:ListItem Text="No" Value="false" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td>Cancelled?</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlCancelled">
                    <asp:ListItem Text="" Value="" />
                    <asp:ListItem Text="Yes" Value="true" />
                    <asp:ListItem Text="No" Value="false" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td>Purpose:</td>
            <td class="control">
                <mmsinc:MvpListBox runat="server" ID="lstDrivenBy" DataSourceID="odsWorkOrderPurposes"
                    SelectionMode="Multiple"
                    DataTextField="Description" DataValueField="WorkOrderPurposeID" Width="350px"/>
            </td>
        </tr>
        <tr>
            <td>Priority:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlPriority" DataSourceID="odsWorkOrderPriorities"
                    DataTextField="Description" DataValueField="WorkOrderPriorityID" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>

        <tr>
            <td>Created By:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlCreatedBy" />
                <atk:CascadingDropDown runat="server" ID="cddCreatedBy" TargetControlID="ddlCreatedBy"
                    ParentControlID="baseSearch$ddlOperatingCenter" Category="Employee" EmptyText="Select an Employee"
                    EmptyValue="" PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Employees...]"
                    ServicePath="~/Views/Employees/EmployeesServiceView.asmx" ServiceMethod="GetEmployeesByOperatingCenterID" />
            </td>
        </tr>

        <tr>
            <td>Requested By:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlRequestedBy" DataSourceID="odsWorkOrderRequesters"
                    DataTextField="Description" DataValueField="WorkOrderRequesterID" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>        
        <tr>
            <td>Acoustic Monitoring Type:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlAcousticMonitoringType" DataSourceID="odsAcoustingMonitoringTypes"
                                        DataTextField="Description" DataValueField="Id" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td>Markout Requirement:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlMarkoutRequirement" DataSourceID="odsMarkoutRequirement"
                    DataTextField="Description" DataValueField="MarkoutRequirementID" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td>SOP Requirement:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlSOPRequirement">
                    <asp:ListItem Text="--Select Here--" Value="" />
                    <asp:ListItem Text="Required" Value="true" />
                    <asp:ListItem Text="Not Required" Value="false" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td>SOP Requested:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlStreetOpeningPermitRequested">
                    <asp:ListItem Text="--Select Here--" Value="" />
                    <asp:ListItem Text="Yes" Value="true" />
                    <asp:ListItem Text="No" Value="false" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td>SOP Issued:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlStreetOpeningPermitIssued">
                    <asp:ListItem Text="--Select Here--" Value="" />
                    <asp:ListItem Text="Yes" Value="true" />
                    <asp:ListItem Text="No" Value="false" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td>Requires Invoice:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlRequiresInvoice">
                    <asp:ListItem Text="--Select Here--" Value="" />
                    <asp:ListItem Text="Required" Value="true" />
                    <asp:ListItem Text="Not Required" Value="false" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td>Has Invoice:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlHasInvoice">
                    <asp:ListItem Text="--Select Here--" Value="" />
                    <asp:ListItem Text="Yes" Value="true" />
                    <asp:ListItem Text="No" Value="false" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td>Is Assigned to a Contractor:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlIsAssignedToContractor">
                    <asp:ListItem Text="--Select Here--" Value="" />
                    <asp:ListItem Text="Yes" Value="true" />
                    <asp:ListItem Text="No" Value="false" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <td>Assigned Contractor:</td>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlContractor" Enabled="False" />
                <atk:CascadingDropDown runat="server" ID="cddlContractor" TargetControlID="ddlContractor"
                    BehaviorID="cddContractor"
                    ParentControlID="baseSearch$ddlOperatingCenter" Category="Contractor" 
                    EmptyText="Select a Contractor" EmptyValue="" 
                    PromptText="--Select Here--" PromptValue="" 
                    LoadingText="[Loading Contractors...]"
                    ServicePath="~/Views/Contractors/ContractorsServiceView.asmx" 
                    ServiceMethod="GetContractorsByOperatingCenterID"/>
            </td>
        </tr>
        <tr>
            <td>WBS Charged:</td>
            <td><mmsinc:MvpTextBox runat="server" ID="txtWBSCharged"/></td>
        </tr>
        <tr>
            <td colspan="2">
                <center>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" />
                </center>
            </td>
        </tr>
    </table>

<wo:AssetTypeIDsScript ID="AssetTypeIDsScript1" runat="server" />

<asp:ObjectDataSource runat="server" ID="odsWorkOrderPurposes" TypeName="WorkOrders.Model.WorkOrderPurposeRepository"
    SelectMethod="SelectAllAsList" />
<asp:ObjectDataSource runat="server" ID="odsWorkOrderPriorities" TypeName="WorkOrders.Model.WorkOrderPriorityRepository"
    SelectMethod="SelectAllAsList" />
<asp:ObjectDataSource runat="server" ID="odsWorkOrderRequesters" TypeName="WorkOrders.Model.WorkOrderRequesterRepository"
    SelectMethod="SelectAllAsList" />
<asp:ObjectDataSource runat="server" ID="odsMarkoutRequirement" TypeName="WorkOrders.Model.MarkoutRequirementRepository"
    SelectMethod="SelectAllAsList" />
<asp:ObjectDataSource runat="server" ID="odsAcoustingMonitoringTypes" TypeName="WorkOrders.Model.AcousticMonitoringTypeRepository"
                      SelectMethod="SelectAllAsList"/>
