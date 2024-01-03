<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="WorkOrderPlanningSearchView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Planning.WorkOrderPlanningSearchView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="mmsinc" TagName="DateRange" Src="~/Common/DateRange.ascx" %>
<%@ Register TagPrefix="wo" TagName="BaseWorkOrderSearch" Src="~/Controls/WorkOrders/BaseWorkOrderSearch.ascx" %>

<center>
    <table>
        <wo:BaseWorkOrderSearch runat="server" ID="baseSearch" />
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
            <td>Date Received:</td>
            <td><mmsinc:DateRange runat="server" ID="drDateReceived" /></td>
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
            <td>Purpose:</td>
            <td class="control">
                <mmsinc:MvpListBox runat="server" ID="lstDrivenBy" DataSourceID="odsWorkOrderPurposes"
                    SelectionMode="Multiple"
                    DataTextField="Description" DataValueField="WorkOrderPurposeID" Width="350px"/>
            </td>
        </tr>
        <mmsinc:MvpPanel runat="server" id="pnlAssignedTo">
        <tr>
            <td>Office Assignment:</td>
            <td class="control">
                <mmsinc:MvpDropDownList runat="server" ID="ddlOfficeAssignment" />
                <atk:CascadingDropDown runat="server" ID="cddOfficeAssignment" TargetControlID="ddlOfficeAssignment"
                    ParentControlID="baseSearch$ddlOperatingCenter" Category="Employee" EmptyText="Select an Employee" EmptyValue=""
                    PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Employees...]"
                    ServicePath="~/Views/Employees/EmployeesServiceView.asmx" 
                    ServiceMethod="GetEmployeesByOperatingCenterID"
                />
            </td>
        </tr>
        </mmsinc:MvpPanel>
        <tr>
            <td>Markout to be Called Today:</td>
            <td><mmsinc:MvpCheckBox runat="server" ID="chkMarkoutToBeCalled" /></td>
        </tr>
        <tr>
            <td>WBS Charged:</td>
            <td><mmsinc:MvpTextBox runat="server" ID="txtWBSCharged"/></td>
        </tr>
        <tr>
            <td>Notes:</td>
            <td><mmsinc:MvpTextBox runat="server" ID="txtNotes"/></td>
        </tr>
        <tr>
            <td colspan="2">
                <center>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                </center>
            </td>
        </tr>
    </table>
</center>

<mmsinc:MvpObjectDataSource runat="server" ID="odsTowns" TypeName="WorkOrders.Model.TownRepository"
    SelectMethod="SelectByOperatingCenterID">
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>
<asp:ObjectDataSource runat="server" ID="odsAssetTypes" TypeName="WorkOrders.Model.AssetTypeRepository"
    SelectMethod="SelectAllAsList" />
<asp:ObjectDataSource runat="server" ID="odsWorkDescriptions" TypeName="WorkOrders.Model.WorkDescriptionRepository"
    SelectMethod="SelectAllSorted" />
<asp:ObjectDataSource runat="server" ID="odsWorkOrderPriorities" TypeName="WorkOrders.Model.WorkOrderPriorityRepository"
    SelectMethod="SelectAllAsList" />
<asp:ObjectDataSource runat="server" ID="odsWorkOrderRequesters" TypeName="WorkOrders.Model.WorkOrderRequesterRepository"
    SelectMethod="SelectAllAsList" />
<asp:ObjectDataSource runat="server" ID="odsMarkoutRequirement" TypeName="WorkOrders.Model.MarkoutRequirementRepository"
    SelectMethod="SelectAllAsList" />
<asp:ObjectDataSource runat="server" ID="odsWorkOrderPurposes" TypeName="WorkOrders.Model.WorkOrderPurposeRepository"
    SelectMethod="SelectAllAsList" />
<asp:ObjectDataSource runat="server" ID="odsOfficeAssignment" TypeName="WorkOrders.Model.EmployeeRepository"
    SelectMethod="GetEmployeesByOperatingCenterID">
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>    
</asp:ObjectDataSource>
<asp:ObjectDataSource runat="server" ID="odsAcoustingMonitoringTypes" TypeName="WorkOrders.Model.AcousticMonitoringTypeRepository"
     SelectMethod="SelectAllAsList"/>


