<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderInputFormView.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderInputFormView" %>
<%@ Import Namespace="MMSINC.Interface" %>
<%@ Import Namespace="MMSINC.Utilities" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register Assembly="LINQTo271" Namespace="LINQTo271.Common" TagPrefix="wo" %>
<%@ Register TagPrefix="mmsinc" TagName="LatLonPicker" Src="~/Common/LatLonPicker.ascx" %>
<%@ Register TagPrefix="wo" TagName="RequesterIDsScript" Src="~/Views/WorkOrderRequesters/WorkOrderRequestersJSView.ascx" %>
<%@ Register TagPrefix="wo" TagName="AssetTypeIDsScript" Src="~/Views/AssetTypes/AssetTypesJSView.ascx" %>
<%@ Register tagPrefix="wo" tagName="WorkDescriptionsScript" Src="~/Views/WorkDescriptions/WorkDescriptionsJSView.ascx" %>
<%@ Register TagPrefix="wo" TagName="AssetLink" Src="~/Controls/AssetLink.ascx" %>
<%@ Register TagPrefix="mmsinc" Namespace="MapCall.Common.Controls" Assembly="MapCall.Common" %>

<asp:PlaceHolder runat="server" ID="phDeprecatedMessage" Visible='<%# CurrentMvpMode == DetailViewMode.Insert %>'>
<div style="padding: 10px; font-size: larger; font-weight: bold; background-color: lightcoral; color: darkred; border: 1px solid darkred; margin-bottom: 6px;" class="warn">
    This page has been deprecated. If you are a visiting here from a bookmark, please remove the bookmark and then visit 
    <asp:Hyperlink runat="server" id="hlInputLInk" Text="this link" NavigateUrl="/Modules/mvc/FieldOperations/WorkOrder/New"/>
    for the new page and create a new bookmark.
    <br/>
</div>
</asp:PlaceHolder>

<wo:WorkOrdersFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID"
    OnItemInserting="fvWorkOrder_ItemInserting" GridLines="None" OnItemUpdating="fvWorkOrder_ItemUpdating">
    <ItemTemplate>
        <mmsinc:MvpPlaceHolder runat="server" id="pnlSAPErrorCode" Visible='<%#Eval("HasRealSapError") %>'>
            <div style="font-weight: bold; font-size: larger;background-color: red; padding: 3px; color: white;">
                <asp:Label runat="server" ID="lblSAPErrorCodeNotification" Text='<%#"SAP Error: " + Eval("SAPErrorCode") %>'/>
            </div>
        </mmsinc:MvpPlaceHolder>
        <table class="grid WorkOrderDisplay">
            <tr>
                <td>Order Number:</td>
                <td colspan='<%# (Eval("OriginalOrderNumber") == null) ? 3 : 1 %>'>
                    <asp:Label runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' />
                </td>
                <td style="display: <%# (Eval("OriginalOrderNumber") == null) ? "none" : "run-in" %>">
                    Original Order Number:
                </td>
                <td style="display: <%# (Eval("OriginalOrderNumber") == null) ? "none" : "run-in" %>">
                    <asp:HyperLink runat="server" ID="hlOriginalOrder" Text='<%# Eval("OriginalOrderNumber") %>'
                        NavigateUrl=
                        '<%# string.Format("~/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg={0}", Eval("OriginalOrderNumber")) %>' />
                </td>
            </tr>
            <tr>
                <td>Town:</td>
                <td><asp:Label runat="server" Text='<%# Eval("Town") %>' /></td>
                <td>Town Section:</td>
                <td><asp:Label runat="server" Text='<%# Eval("TownSection") %>' /></td>
            </tr>
            <tr class="trMainBreakTownInfo" style="display: none;">
                <td>Main Break Note:</td>
                <td colspan="3" style="color:red;">
                    <%# Eval("Town.CriticalMainBreakNotes") %>
                </td>
            </tr>
            <tr>
                <td>Street Number:</td>
                <td><asp:Label runat="server" ID="lblStreetNumber" Text='<%# Eval("StreetNumber") %>' /></td>
                <td>Street:</td>
                <td><asp:Label runat="server" Text='<%# Eval("Street") %>' /></td>
            </tr>
            <tr>
                <td>Apartment Addtl:</td>
                <td><asp:Label runat="server" ID="lblApartmentAddtl" Text ='<%# Eval("ApartmentAddtl") %>' /></td>
            </tr>
            <tr>
                <td>Nearest Cross Street:</td>
                <td><asp:Label ID="lblNearestCrossStreet" runat="server" Text='<%# Eval("NearestCrossStreet") %>' /></td>
                <td>Zip Code:</td>
                <td><asp:Label runat="server" ID="lblZipCode" Text='<%# Eval("ZipCode") %>' /></td>
            </tr>
            <tr>
                <td>Asset Type:</td>
                <td><asp:Label runat="server" ID="lblAssetType" Text='<%# Eval("AssetType") %>' /></td>
                <td>
                    <div id="divAssetLabel"></div>...
                    <div>
                        Data Collection: 
                        <asp:HyperLink runat="server" ID="hlCollector"
                           NavigateUrl='<%#Eval("ArcCollectorLink")%>'
                           ImageUrl="~/Includes/collector.png"
                           Target="_blank"
                           ImageHeight="20" ImageWidth="20"/>
                    </div>
                </td>
                <td>
                    <div style='color: red;'>
                        <asp:Label runat="server" Text='<%#Eval("AssetCriticalNotes") %>'></asp:Label>
                    </div>
                    <wo:AssetLink runat="server" AssetType='<%# Eval("AssetType") %>' AssetIdentifier='<%# Eval("AssetId") %>' AssetId='<%# Convert.ToInt32(Eval("Asset.InnerAsset.AssetKey")) %>' />
                            <br/>

                            Device Location: <asp:Label runat="server" ID="lblDeviceLocation" Text='<%#Eval("DeviceLocation") %>'/> <br/>
                            Equipment #: <asp:Label runat="server" ID="lblSAPEquipmentNumber" Text='<%#Eval("SAPEquipmentNumber") %>'/> <br/>
                            Installation: <asp:Label runat="server" ID="lblInstallation" Text='<%#Eval("Installation") %>'/> 

                        <div style='display: <%# (Eval("AssetTypeID").ToString() == "4" || Eval("AssetTypeID").ToString() == "6" ) ? "" : "none" %>;'>
                            <asp:HyperLink runat="server" ID="HyperLink2" Text="View Service" 
                                           style='<%# (Eval("ServiceID") != null) ? "display:;" : "display:none;"%>'
                                           NavigateUrl='<%# string.Format("../../../../modules/mvc/fieldoperations/service/show/{0}", Eval("ServiceID")) %>' />
                            <br/>
                            <div style='color: red; <%# ((bool)Eval("IsPremiseLinkedToSampleSite")) ? "display:;" : "display:none;"%>' id="premiseWarningDiv" >
                                <asp:Label runat="server" Text='This Premise is Linked to a Sample Site. Contact WQ before making any changes.'></asp:Label>
                            </div>
                            <asp:Hyperlink runat="server" id="hlPremiseInfo" 
                                Text="Premise Details" Target="_blank"
                                NavigateUrl='<%# string.Format("../../../../Modules/mvc/Customer/Premise/Index?PremiseNumber.Value={0}&PremiseNumber.MatchType=0&", Eval("PremiseNumber")) %>'
                            />
                            <br/>
                            <asp:HyperLink runat="server" id="hlTMDInfo"
                                Text="SAP Technical Master Data" Target="_blank"
                                NavigateUrl='<%# string.Format("../../../../Modules/mvc/Customer/SAPTechnicalMasterAccount?Equipment={1}&InstallationType=&PremiseNumber={0}", 
                                    Eval("PremiseNumber"), Eval("Equipment")) %>'
                            />
                        </div>

                        <mmsinc:MvpPlaceHolder runat="server" id="phEchoshoreLeakAlert" Visible='<%#Eval("EchoshoreLeakAlertId") != null %>'>
                            <div>Echoshore Leak Alert:
                                <mmsinc:MvpHyperLink runat="server" ID="hlEchoshoreLeakAlert"
                                   Text='<%#Eval("EchoshoreLeakAlertId") %>'
                                   NavigateUrl='<%# string.Format("../../../../Modules/mvc/FieldOperations/EchoshoreLeakAlert/Show/{0}", Eval("EchoshoreLeakAlertId"))%>'></mmsinc:MvpHyperLink>
                            </div>
                        </mmsinc:MvpPlaceHolder>
                </td>
            </tr>
            <tr>
                <td>Requested By:</td>
                <td><asp:Label runat="server" Text='<%# Eval("RequestedBy") %>' ID="lblRequestedBy" /></td>
                <td><div id="divRequesterLabel"></div></td>
                <td>
                    <asp:Label runat="server" Text='<%# Eval("RequestingEmployee.FullName")%>' ID="lblRequestingEmployeeID" style="display: none;" />
                    <asp:Label runat="server" Text='<%# Eval("CustomerName") %>' ID="lblCustomerName" style="display: none;" />
                    <asp:Label runat="server" Text='<%# Eval("AcousticMonitoringType") %>' ID="lblAcousticMonitoringType" style="display: none;" />
                </td>
            </tr>
            <tr class="trCustomerInfo" style="display: none;">
                <td>Phone Number:</td>
                <td><asp:Label runat="server" Text='<%# Eval("PhoneNumber") %>' /></td>
                <td>Secondary Number:</td>
                <td><asp:Label runat="server" Text='<%# Eval("SecondaryPhoneNumber") %>' /></td>
            </tr>
            <tr>
                <td>Purpose:</td>
                <td><asp:Label runat="server" Text='<%# Eval("DrivenBy") %>' /></td>
                <td>Job Priority:</td>
                <td><asp:Label runat="server" Text='<%# Eval("Priority") %>' /></td>
            </tr>
            <tr>
                <td>Description of Work:</td>
                <td colspan="3"><asp:Label runat="server" ID="lblDescriptionOfWork" Text='<%# Eval("WorkDescription") %>' /></td>
            </tr>
            <tr class="trMainBreakInfo" style="display: none">
                <td>Estimated Customer Impact</td>
                <td><asp:Label runat="server" Text='<%# Eval("CustomerImpactRange") %>' ID="lblCustomerImpactRange" /></td>
                <td>Anticipated Repair Time</td>
                <td><asp:Label runat="server" Text='<%# Eval("RepairTimeRange") %>' ID="lblRepairTimeRange" /></td>
            </tr>
            <tr class="trMainBreakInfo" style="display: none">
                <td>Alert Issued?</td>
                <td>
                    <asp:Label runat="server" Text='<%# Eval("AlertIssued") == null ? "" : ((bool)Eval("AlertIssued")) ? "Yes" : "No" %>'
                        ID="lblAlertIssued" />
                    <asp:HiddenField runat="server" ID="hidAlertStarted" Value='<%# Eval("AlertStarted") %>'/>
                </td>
                <td>Significant Traffic Impact</td>
                <td><asp:Label runat="server" Text='<%# Eval("SignificantTrafficImpact") == null ? "" : ((bool)Eval("SignificantTrafficImpact")) ? "Yes" : "No" %>'
                        ID="lblSignificantTrafficImpact" /></td>
            </tr>
            <tr>
                <td>PMAT Override</td>
                <td><asp:Label runat="server" Text='<%#Eval("PlantMaintenanceActivityTypeOverride") %>'/></td>
                <td>Markout Requirement:</td>
                <td><mmsinc:MvpLabel runat="server" id="lbMarkoutRequirement" Text='<%# Eval("MarkoutRequirement") %>' /></td>
            </tr>
            <tr>
                <td>WBS Charged:</td>
                <td><asp:Label runat="server" Text='<%# Eval("AccountCharged") %>'/></td>
            </tr>
            <tr id="trSafetyRequirements">
                <td>Traffic Control Required?</td>
                <td><asp:CheckBox runat="server" Checked='<%# Eval("TrafficControlRequired") %>' Enabled="false" /></td>
                <td>Street Opening Permit Required?</td>
                <td><asp:CheckBox runat="server" Checked='<%# Eval("StreetOpeningPermitRequired") %>' Enabled="false" /></td>
            </tr>

        <mmsinc:MvpPlaceHolder runat="server" Visible='<%# !((bool?)Eval("DigitalAsBuiltCompleted")).HasValue %>'>
            <tr>
                <td>Digital As-Built Required?</td>
                <td colspan="3">
                    <asp:CheckBox runat="server" Checked='<%# Eval("DigitalAsBuiltRequired") %>' Enabled="false" />
                </td>
            </tr>
        </mmsinc:MvpPlaceHolder>
        <mmsinc:MvpPlaceHolder runat="server" Visible='<%# ((bool?)Eval("DigitalAsBuiltCompleted")).HasValue %>'>
            <tr>
                <td>Digital As-Built Required?</td>
                <td>
                    <asp:CheckBox runat="server" Checked='<%# Eval("DigitalAsBuiltRequired") %>' Enabled="false" />
                </td>
                <td>Digital As-Built Completed?</td>
                <td>
                    <asp:CheckBox runat="server" Checked='<%# ((bool?)Eval("DigitalAsBuiltCompleted")).HasValue ? ((bool?)Eval("DigitalAsBuiltCompleted")) : false %>' Enabled="false" />
                </td>
            </tr>
        </mmsinc:MvpPlaceHolder>

            <tr>
                <td>Notes:</td>
                <td colspan="3"><asp:Label runat="server" ID="lblNotes" Text='<%# Eval("Notes") %>' /></td>
            </tr>
            <tr>
                <td>Created By:</td>
                <td><asp:Label runat="server" Text='<%# Eval("CreatedBy") %>' /></td>
                <td>Date Received:</td>
                <td><asp:Label runat="server" ID="lblDateReceived" Text='<%# Eval("DateReceived", "{0:M/d/yyyy}") %>' /></td>
            </tr>
            <tr>
                <td>Completed By:</td>
                <td><asp:Label runat="server" ID="lblCompletedBy" Text='<%# Eval("ActuallyCompletedBy") %>' /></td>
                <td>Date Completed:</td>
                <td><asp:Label runat="server" Text='<%# Eval("DateCompleted", "{0:M/d/yyyy}") %>' ID="lblDateCompleted" /></td>
            </tr>
            <tr>
                <td>Flushing Notice Type:</td>
                <td><asp:Label runat="server" ID="lblFlushingNoticeType" Text='<%# Eval("FlushingNoticeType")%>' /></td>
                <td class="label">Created On:</td>
                <td><asp:Label runat="server" id="Label3" Text='<%#Eval("CreatedOn",CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE) %>'></asp:Label></td>
            </tr>
            <tr>
                <td>SAP Notification #:</td>
                <td><asp:Label runat="server" ID="lblSAPNotificationNumber" Text='<%# Eval("SAPNotificationNumber")%>' /></td>
                <td class="label">Supervisor Approved On:</td>
                <td><asp:Label runat="server" id="lblApprovedOn" Text='<%#Eval("ApprovedOn",CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE) %>'></asp:Label></td>
            </tr>
            <tr>
                <td>SAP Work Order #:</td>
                <td><asp:Label runat="server" ID="lblSAPWorkOrderNumber" Text='<%# Eval("SAPWorkOrderNumber")%>' /></td>
                <td class="label">Materials Approved On:</td>
                <td><asp:Label runat="server" id="Label2" Text='<%#Eval("MaterialsApprovedOn",CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE) %>'></asp:Label></td>

            </tr>
            <tr>
                <td>SAP Status:</td>
                <td><asp:Label runat="server" ID="lblSAPErrorCode" Text='<%# Eval("SAPErrorCode")%>' /></td>
                <td>Material Planning Completed On:</td>
                <td><asp:Label runat="server" ID="lblMaterialPlanningCompletedOn" Text='<%#Eval("MaterialPlanningCompletedOn") %>'></asp:Label></td>
            </tr>
        </table>
        
        <mmsinc:MvpPlaceHolder runat="server" ID="phContractorAssigned" 
            Visible='<%#Eval("AssignedContractor") != null %>'>
            
            Contractor Assigned to: <asp:Label runat="server" Text='<%# string.Concat(Eval("AssignedContractor"), " On ", Eval("AssignedToContractorOn")) %>'/>
        </mmsinc:MvpPlaceHolder>

        <script type="text/javascript">
            $(document).ready(function() { WorkOrderInputFormView.initializeReadOnly(); });
        </script>
    </ItemTemplate>
    <EditItemTemplate>
        <mmsinc:MvpPlaceHolder runat="server" id="pnlSAPErrorCode" Visible='<%#CurrentMvpMode != DetailViewMode.Insert && (bool)Eval("HasRealSapError") %>'>
            <div style="font-weight: bold; font-size: larger;background-color: red; padding: 3px; color: white;">
                <asp:Label runat="server" ID="lblSAPErrorCodeNotification" Text='<%#"SAP Error: " + Eval("SAPErrorCode") %>'/>
            </div>
        </mmsinc:MvpPlaceHolder>

        <table class="grid">
            <%-- THIS STAYS UP TOP --%>
            <tr id="trNotificationArea" style="display: none;">
                <td id="tdNotificationArea" colspan="4"></td>
            </tr>
            <%-- /THIS STAYS UP TOP --%>
            <tr id="trOrderNumbers" style='<%# (CurrentMvpMode != DetailViewMode.Insert && (bool)Eval("IsRevisit")) ? "display:;" : "display:none;"%>'>
                <td></td>
                <td></td>
                <td class="label required">Original Order Number:</td>
                <td class="control">
                    <mmsinc:MvpTextBox runat="server" ID="txtOriginalOrderNumber" onchange="WorkOrderInputFormView.txtOriginalOrderNumber_Change(this)"
                        Text='<%# Bind("OriginalOrderNumber") %>' />
                </td>
            </tr>
            <tr>
                <td class="label required">Operating Center:</td>
                <td class="control">
                    <mmsinc:MvpDropDownList ID="ddlOperatingCenter" runat="server" DataSourceID="odsOperatingCenters"
                        DataTextField="FullDescription" OnDataBound="ddlOperatingCenter_DataBound" DataValueField="OperatingCenterID"
                        onchange="WorkOrderInputFormView.ddlOperatingCenter_Change()"
                        SelectedValue='<%# Bind("OperatingCenterID") %>' AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                    </mmsinc:MvpDropDownList>
                    
                </td>
                <td class="required">
                    <asp:RadioButton runat="server" ID="rdoRevisitInitial" GroupName="Revisit" Checked='<%#CurrentMvpMode != DetailViewMode.Insert && !(bool)Eval("IsRevisit") %>'
                        onclick="WorkOrderInputFormView.rdoRevisit_Click('initial')" Enabled='<%# Eval("WorkOrderID") == null %>' />
                    Initial
                </td>
                <td class="required">
                    <asp:RadioButton runat="server" ID="rdoRevisitRevisit" GroupName="Revisit" Checked='<%#CurrentMvpMode != DetailViewMode.Insert && (bool)Eval("IsRevisit") %>'
                        onclick="WorkOrderInputFormView.rdoRevisit_Click('revisit')" Enabled='<%# Eval("WorkOrderID") == null %>' />
                    Revisit
                </td>
            </tr>
            <tr>
                <td class="label required">Town: </td>
                <td class="control">
                    <asp:DropDownList ID="ddlTown" runat="server" onchange="WorkOrderInputFormView.ddlTown_Change()" />
                    <atk:CascadingDropDown runat="server" ID="cddTowns" TargetControlID="ddlTown"
                        ParentControlID="ddlOperatingCenter" Category="Town" EmptyText="None Found" EmptyValue=""
                        PromptText="--Select Here--" PromptValue=""
                        ServicePath="~/Views/Towns/TownsServiceView.asmx" ServiceMethod="GetTownsByOperatingCenterID"
                        SelectedValue='<%# Bind("TownID") %>' BehaviorID="cddTowns" />
                </td>
                <td class="label">Town Section: </td>
                <td class="control">
                    <asp:DropDownList ID="ddlTownSection" runat="server" onchange="WorkOrderInputFormView.ddlTownSection_Change()" />
                    <atk:CascadingDropDown runat="server" ID="cddTownSection" TargetControlID="ddlTownSection"
                        ParentControlID="ddlTown" Category="TownSection" EmptyText="None Found" EmptyValue="null"
                        PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Town Sections...]"
                        ServicePath="~/Views/TownSections/TownSectionsServiceView.asmx" ServiceMethod="GetTownSectionsByTownDefined"
                        SelectedValue='<%# Bind("TownSectionID") %>' BehaviorID="cddTownSection" />
                </td>
            </tr>
            <tr>
                <td class="label required">Street Number: </td>
                <td class="control">
                    <asp:TextBox runat="server" ID="txtStreetNumber" Text='<%# Bind("StreetNumber") %>'
                        onchange="WorkOrderInputFormView.txtStreetNumber_Change()" />
                </td>
                <td class="label required">Street: </td>
                <td class="control">
                    <asp:DropDownList runat="server" ID="ddlStreet" onchange="WorkOrderInputFormView.ddlStreet_Change()" />
                    <atk:CascadingDropDown runat="server" ID="cddStreet" TargetControlID="ddlStreet"
                        ParentControlID="ddlTown" Category="Street" EmptyText="Select A Town" EmptyValue=""
                        PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Streets...]"
                        ServicePath="~/Views/Streets/StreetsServiceView.asmx" ServiceMethod="GetStreetsByTownDefined"
                        SelectedValue='<%# Bind("StreetID") %>' BehaviorID="cddStreet" />
                </td>
            </tr>
            <tr>
                <td class="label">Apartment Addtl: </td>
                <td class="control">
                    <asp:TextBox runat="server" ID="txtApartmentAddtl" Text='<%# Bind("ApartmentAddtl") %>'/>
                </td>
            </tr>
            <tr>
                <td class="label required">Nearest Cross Street: </td>
                <td class="control">
                    <asp:DropDownList runat="server" ID="ddlNearestCrossStreet" onchange="WorkOrderInputFormView.ddlNearestCrossStreet_Change()" />
                    <atk:CascadingDropDown runat="server" ID="cddNearestCrossStreet" TargetControlID="ddlNearestCrossStreet"
                        ParentControlID="ddlTown" Category="Street" EmptyText="Select A Town" EmptyValue=""
                        PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Streets...]"
                        ServicePath="~/Views/Streets/StreetsServiceView.asmx" ServiceMethod="GetStreetsByTownDefined"
                        SelectedValue='<%# Bind("NearestCrossStreetID") %>' BehaviorID="cddNearestCrossStreet" />
                </td>
                <td class="label">Zip Code: </td>
                <td class="control">
                    <asp:TextBox runat="server" ID="txtZipCode" Text='<%# Bind("ZipCode") %>' MaxLength="10" />
                </td>
            </tr>
            <tr>
                <td class="label required">Asset Type: </td>
                <td class="control">
                    <asp:Label runat="server" ID="Label1" Text='<%#Eval("AssetType") %>' Visible='<%#CurrentMvpMode != DetailViewMode.Insert && !(bool)Eval("AssetTypeEditable") %>' />

                    <mmsinc:MvpDropDownList ID="ddlAssetType" runat="server" onchange="WorkOrderInputFormView.ddlAssetType_Change(this)"
                        style='<%#CurrentMvpMode == DetailViewMode.Insert || (bool)Eval("AssetTypeEditable") ? "display:;" : "display:none;"%>' />
                    <atk:CascadingDropDown runat="server" ID="cddAssetType" TargetControlID="ddlAssetType"
                        ParentControlID="ddlOperatingCenter" Category="AssetType" EmptyText="Select an OperatingCenter"
                        EmptyValue="" PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Asset Types...]"
                        ServicePath="~/Views/AssetTypes/AssetTypesServiceView.asmx" ServiceMethod="GetAssetTypesByOperatingCenter"
                        SelectedValue='<%# Bind("AssetTypeID") %>' BehaviorID="cddAssetType" />
                        <div style='<%#CurrentMvpMode == DetailViewMode.Insert || (bool)Eval("AssetTypeEditable") ? "display:;" : "display:none;"%>'>
                            <asp:HyperLink runat="server" ID="hlEditMvc" Text="Change/Update Service Asset" 
                            NavigateUrl='<%# string.Format("../../../../modules/mvc/fieldoperations/workorder/edit/{0}", Eval("WorkOrderID")) %>' />
                        </div>
                </td>
                <td class="label required" id="lblAssetID">
                    Asset ID:
                    <br/>Data Collection: 
                    <asp:HyperLink runat="server" ID="HyperLink1"
                                   NavigateUrl='<%#string.Format("https://collector.arcgis.app?itemID={0}", Eval("OperatingCenter.MapId")) %>'
                                   ImageUrl="~/Includes/collector.png"
                                   Target="_blank"
                                   Visible='<%#CurrentMvpMode != DetailViewMode.Insert%>'
                                   ImageHeight="20" ImageWidth="20" />
                </td>
                <td class="control">
                    <div style='color: red;'>
                        <asp:Label runat="server" Text='<%#Eval("AssetCriticalNotes") %>'></asp:Label>
                    </div>

                    <asp:Label runat="server" ID="lblAssetID" Text='<%#Eval("AssetID") %>' Visible='<%#CurrentMvpMode != DetailViewMode.Insert && !(bool)Eval("AssetEditable") %>' />
                    <div style='<%#CurrentMvpMode == DetailViewMode.Insert || (bool)Eval("AssetEditable") ? "display:;" : "display:none;"%>'>
                    <asp:DropDownList ID="ddlDummyAssetID" runat="server" CssClass="asset-select" disabled="true">
                        <asp:ListItem>Select Asset Type</asp:ListItem>
                    </asp:DropDownList>

                    <asp:DropDownList runat="server" ID="ddlValve" CssClass="asset-select" Style="display: none;"
                        onchange="WorkOrderInputFormView.ddlAssetID_Change(this)" />
                    <atk:CascadingDropDown runat="server" ID="cddValve" TargetControlID="ddlValve" ParentControlID="ddlStreet"
                        Category="Valve" EmptyText="None Found" EmptyValue="" PromptText="--Select Here--"
                        PromptValue="" LoadingText="[Loading Valves]" ServicePath="~/Views/Valves/ValvesServiceView.asmx"
                        ServiceMethod="GetValvesByStreet" SelectedValue='<%# Bind("ValveID") %>' BehaviorID="cddValve" />
                    
                    <asp:DropDownList runat="server" ID="ddlHydrant" CssClass="asset-select" Style="display: none;"
                        onchange="WorkOrderInputFormView.ddlAssetID_Change(this)" />
                    <atk:CascadingDropDown runat="server" ID="cddHydrant" TargetControlID="ddlHydrant"
                        ParentControlID="ddlStreet" Category="Hydrant" EmptyText="None Found" EmptyValue=""
                        PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Hydrants]"
                        ServicePath="~/Views/Hydrants/HydrantsServiceView.asmx" ServiceMethod="GetHydrantsByStreet"
                        SelectedValue='<%# Bind("HydrantID") %>' BehaviorID="cddHydrant" />
                    
                    <asp:DropDownList runat="server" ID="ddlMainCrossing" CssClass="asset-select" Style="display: none;"
                        onchange="WorkOrderInputFormView.ddlAssetID_Change(this)" />
                    <atk:CascadingDropDown runat="server" ID="cddMainCrossing" TargetControlID="ddlMainCrossing"
                        ParentControlID="ddlTown" Category="MainCrossing" EmptyText="No Crossing exist within the town." EmptyValue=""
                        PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Main Crossings]"
                        ServicePath="~/Views/MainCrossings/MainCrossingsServiceView.asmx" ServiceMethod="GetMainCrossingsByTown"
                        SelectedValue='<%#Bind("MainCrossingID") %>' BehaviorID="cddMainCrossing"/>

                    <asp:DropDownList runat="server" ID="ddlSewerOpening" CssClass="asset-select" Style="display: none;"
                        onchange="WorkOrderInputFormView.ddlAssetID_Change(this)" />                        
                    <atk:CascadingDropDown runat="server" ID="cddSewerOpening" TargetControlID="ddlSewerOpening"
                        ParentControlID="ddlStreet" Category="SewerOpening" EmptyText="None Found" EmptyValue=""
                        PromptText="--Select Here--" PromptValue="" LoadingText="[Loading SewerOpenings]"
                        ServicePath="~/Views/SewerOpenings/SewerOpeningsServiceView.asmx" ServiceMethod="GetSewerOpeningsByStreet"
                        SelectedValue='<%# Bind("SewerOpeningID") %>' BehaviorID="cddSewerOpening" />

                        
                    <asp:DropDownList runat="server" ID="ddlStormCatch" CssClass="asset-select" Style="display:none;"
                        onchange="WorkOrderInputFormView.ddlAssetID_Change(this)" />
                    <atk:CascadingDropDown runat="server" ID="cddStormCatch" TargetControlID="ddlStormCatch"
                        ParentControlID="ddlStreet" Category="StormCatch" EmptyText="None Found" EmptyValue=""
                        PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Storm Catches]"
                        ServicePath="~/Views/StormCatches/StormCatchesServiceView.asmx" 
                        ServiceMethod="GetStormCatchesByStreet"
                        SelectedValue='<%# Bind("StormCatchID") %>' BehaviorID="cddStormCatch" />                    
                            
                    <asp:DropDownList runat="server" ID="ddlEquipment" CssClass="asset-select" style="display:none;" 
                        onchange="WorkOrderInputFormView.ddlAssetID_Change(this)" />
                    <atk:CascadingDropDown runat="server" ID="cddEquipment" TargetControlID="ddlEquipment"
                        ParentControlID="ddlTown" Category="Equipment" EmptyText="None Found" EmptyValue="" PromptText="--Select Here--"
                        PromptValue="" LoadingText="[Loading Equipment]" ServicePath="~/Views/Equipments/EquipmentServiceView.asmx"
                        ServiceMethod="GetEquipmentByTown" SelectedValue='<%# Bind("EquipmentID") %>'/>
                                        
                    <asp:DropDownList runat="server" ID="ddlMain" CssClass="asset-select" Style="display: none;"
                        onchange="WorkOrderInputFormView.ddlAssetID_Change(this)" />
                    <span runat="server" id="pnlService" style="display:none;">
                        <asp:TextBox runat="server" ID="txtPremiseNumber" CssClass="asset-select" MaxLength="10"
                            style="width:131px;" Text='<%# Bind("PremiseNumber") %>' onchange="WorkOrderInputFormView.txtPremiseNumber_Change(this)" />
                        <br />
                        <asp:TextBox runat="server" ID="txtServiceNumber" CssClass="asset-select" MaxLength="12" 
                            style="width:131px;" Text='<%# Bind("ServiceNumber") %>' /> 
                        
                        <div >
                            Device Location: <asp:Label runat="server" ID="lblDeviceLocation" Text='<%#Eval("DeviceLocation") %>'/> <br/>
                            Equipment #: <asp:Label runat="server" ID="lblSAPEquipmentNumber" Text='<%#Eval("SAPEquipmentNumber") %>'/> <br/>
                            Installation: <asp:Label runat="server" ID="lblInstallation" Text='<%#Eval("Installation") %>'/> 
                            <asp:Hyperlink runat="server" id="hlPremiseInfo" 
                                Text="Premise Details" Target="_blank"
                                NavigateUrl='<%# string.Format("../../../../Modules/mvc/Customer/Premise/Index?PremiseNumber.Value={0}&PremiseNumber.MatchType=0&", Eval("PremiseNumber")) %>'
                            />
                            <br/>
                            <asp:HyperLink runat="server" id="hlTMDInfo"
                                Text="SAP Technical Master Data" Target="_blank"
                                NavigateUrl='<%# string.Format("../../../../Modules/mvc/Customer/SAPTechnicalMasterAccount?Equipment={1}&InstallationType=&PremiseNumber={0}", 
                                    Eval("PremiseNumber"), Eval("Equipment")) %>'
                            />
                            <br/>
                        </div>
                        
                        <div style='display:<%#Eval("HasCriticalNotes")%>'>
                            <asp:Label runat="server" Text='<%#Eval("AssetCriticalNotes") %>'></asp:Label>
                        </div>
                    </span>
                    </div>
                    
                    <mmsinc:LatLonPicker runat="server" ID="llpAsset" Latitude='<%# DataBinder.Eval(Container.DataItem, "Latitude") %>'
                        Longitude='<%# DataBinder.Eval(Container.DataItem, "Longitude") %>' AssetTypeID='<%# DataBinder.Eval(Container.DataItem, "AssetTypeID") %>'
                        AssetID='<%# DataBinder.Eval(Container.DataItem, "AssetKey") %>' ClientClickHandler="WorkOrderInputFormView.llpAsset_Click" />

                </td>
            </tr>
            <tr>
                <td class="label required">Requested By: </td>
                <td class="control">
                    <asp:DropDownList runat="server" ID="ddlRequestedBy" DataSourceID="odsWorkOrderRequesters"
                        DataTextField="Description" DataValueField="WorkOrderRequesterID" AppendDataBoundItems="true"
                        onchange="WorkOrderInputFormView.ddlRequestedBy_Change(this)" SelectedValue='<%# Bind("RequesterID") %>'>
                        <asp:ListItem>--Select Here--</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="label required" id="lblRequester"></td>
                <td class="control">
                    <asp:TextBox runat="server" ID="txtCustomerName" style="display: none;" Text='<%# Bind("CustomerName") %>' MaxLength="30"/>
                    <asp:DropDownList runat="server" ID="ddlRequestingEmployee" Style="display: none;" />
                    <atk:CascadingDropDown runat="server" ID="cddRequestingEmployee" TargetControlID="ddlRequestingEmployee"
                        ParentControlID="ddlOperatingCenter" Category="Employee" EmptyText="Select an Employee"
                        EmptyValue="" PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Employees...]"
                        ServicePath="~/Views/Employees/EmployeesServiceView.asmx" ServiceMethod="GetEmployeesByOperatingCenterID"
                        SelectedValue='<%# Bind("RequestingEmployeeID") %>' BehaviorID="cddRequestingEmployee" />
                    <asp:DropDownList runat="server" ID="ddlAcousticMonitoringType" DataSourceID="odsAcoustingMonitoringTypes"
                        DataTextField="Description" DataValueField="Id"
                        AppendDataBoundItems="True" SelectedValue='<%#Bind("AcousticMonitoringTypeId") %>' >
                        <asp:ListItem Value="">--Select Here--</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="trCustomerInfo" style="display: none;">
                <td class="label required">Phone Number: </td>
                <td class="control">
                    <asp:TextBox runat="server" ID="txtPhoneNumber" Text='<%# Bind("PhoneNumber") %>' />
                    <atk:MaskedEditExtender runat="server" ID="meePhoneNumber" TargetControlID="txtPhoneNumber"
                        Mask="(999)999-9999" ClearMaskOnLostFocus="true" />
                </td>
                <td class="label required">2nd Phone Number: </td>
                <td class="control">
                    <asp:TextBox runat="server" ID="txtSecondaryPhoneNumber" Text='<%# Bind("SecondaryPhoneNumber") %>' />
                    <atk:MaskedEditExtender runat="server" ID="meeSecondaryPhoneNumber" TargetControlID="txtSecondaryPhoneNumber"
                        Mask="(999)999-9999" ClearMaskOnLostFocus="true" />
                </td>
            </tr>
            <tr>
                <td class="label required">Purpose: </td>
                <td class="control">
                    <asp:DropDownList ID="ddlDrivenBy" runat="server" DataSourceID="odsWorkOrderPurposes"
                        DataTextField="Description" DataValueField="WorkOrderPurposeID" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("PurposeID") %>'>
                        <asp:ListItem>--Select Here--</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="label required">Priority: </td>
                <td class="control">
                    <asp:DropDownList ID="ddlPriority" runat="server" DataSourceID="odsWorkOrderPriorities"
                        DataTextField="Description" DataValueField="WorkOrderPriorityID" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("PriorityID") %>'>
                        <asp:ListItem>--Select Here--</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="label required">Description of Work: </td>
                <td colspan="3">
                    <asp:Panel runat="server" ID="pnlWorkDescriptionEdit" Visible='<%#CurrentMvpMode != DetailViewMode.Insert && !(bool)Eval("WorkDescriptionEditable") %>'>
                        <asp:Label runat="server" ID="lblDescriptionOfWork" Text='<%# Eval("WorkDescription") %>' />
                        <div style='<%#CurrentMvpMode == DetailViewMode.Insert || (bool)Eval("WorkDescriptionEditable") ? "display:;" : "display:none;"%>'>
                            <asp:HyperLink runat="server" ID="hlEditWorkDescription" Text="Edit Work Description" 
                            NavigateUrl='<%# string.Format("../../../../modules/mvc/fieldoperations/workorder/edit/{0}", Eval("WorkOrderID")) %>' />
                        </div>
                        
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlWorkDescriptionInsert">
                        <asp:DropDownList ID="ddlDescriptionOfWork" runat="server" CssClass="three-cell" onchange="WorkOrderInputFormView.ddlDescriptionOfWork_Change(this);" 
                            style='<%#CurrentMvpMode == DetailViewMode.Insert || (bool)Eval("WorkDescriptionEditable") ? "display:;" : "display:none;"%>'/>
                        <atk:CascadingDropDown runat="server" ID="cddDescriptionOfWork" TargetControlID="ddlDescriptionOfWork"
                            ParentControlID="ddlAssetType" Category="AssetType" EmptyText="No Work Descriptions found."
                            EmptyValue="" PromptText="--Select Here--" PromptValue="" LoadingText="[Loading Descriptions...]"
                            ServicePath="~/Views/WorkDescriptions/WorkDescriptionsServiceView.asmx" ServiceMethod="GetWorkDescriptionsByAssetType"
                            SelectedValue='<%# Bind("WorkDescriptionID") %>' 
                            ContextKey='<%# string.Format("{0}{1}", (Eval("OriginalOrderNumber") == null) ? "initial" : "revisit", (Eval("WorkOrderID") == null) ? "input" : "editonly") %>'
                            BehaviorID="cddDescriptionOfWork" />
                    </asp:Panel>
                </td>
            </tr>
            <tr class="trMainBreakInfo" style="display: none">
                <td class="label required">Estimated Customer Impact:</td>
                <td class="control">
                    <asp:DropDownList runat="server" ID="ddlCustomerImpactRange" DataSourceID="odsCustomerImpactRanges"
                        DataTextField="Description" DataValueField="CustomerImpactRangeID" SelectedValue='<%# Bind("CustomerImpactRangeID") %>'
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </asp:DropDownList>
                </td>
                <td class="label required">Anticipated Repair Time:</td>
                <td class="control">
                    <asp:DropDownList runat="server" ID="ddlRepairTimeRange" DataSourceID="odsRepairTimeRanges"
                        DataTextField="Description" DataValueField="RepairTimeRangeID" SelectedValue='<%# Bind("RepairTimeRangeID") %>'
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="trMainBreakInfo" style="display: none">
                <td>Alert Issued?</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlAlertIssued" SelectedValue='<%# Bind("AlertIssued") %>'>
                        <asp:ListItem Text="--Select Here--" Value="" />
                        <asp:ListItem Text="Yes" Value="True" />
                        <asp:ListItem Text="No" Value="False" />
                    </asp:DropDownList>
                    <asp:HiddenField runat="server" ID="hidAlertStarted" Value='<%# Bind("AlertStarted") %>'/>
                </td>
                <td class="label required">Significant Traffic Impact?</td>
                <td class="control">
                    <asp:DropDownList runat="server" ID="ddlSignificantTrafficImpact" SelectedValue='<%# Bind("SignificantTrafficImpact") %>'>
                        <asp:ListItem Text="--Select Here--" Value="" />
                        <asp:ListItem Text="Yes" Value="True" />
                        <asp:ListItem Text="No" Value="False" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="label ">PMAT Override</td>
                <td class="control">
                    <asp:DropDownList runat="server" ID="ddlPlantMaintenanceActivityTypeOverride" DataSourceID="odsPlantMaintenanceActivityTypes"
                        onchange="WorkOrderInputFormView.ddlPlantMaintenanceActivityTypeOverride_Change()"
                        DataTextField="Display" DataValueField="Id" AppendDataBoundItems="True" 
                        Enabled='<%#CurrentMvpMode == DetailViewMode.Insert || (bool)Eval("PlantMaintenanceActivityTypeOverrideEditable") %>'
                        Visible='<%#CurrentMvpMode == DetailViewMode.Insert || (bool)Eval("PlantMaintenanceActivityTypeOverrideEditable") %>'
                        SelectedValue='<%#Bind("PlantMaintenanceActivityTypeOverrideID") %>'>
                        <asp:ListItem Text="--Select Here--" Value=""/>
                    </asp:DropDownList>
                    

                    <asp:Label runat="server" ID="lblPlantMaintenanceActivityTypeOverride"
                        Visible='<%#CurrentMvpMode != DetailViewMode.Insert && !(bool)Eval("PlantMaintenanceActivityTypeOverrideEditable") %>'
                        Text='<%#Eval("PlantMaintenanceActivityTypeOverride") %>'/>
                </td>
                <td class="label required">Markout Requirement: </td>
                <td class="control">
                    <asp:DropDownList ID="ddlMarkoutRequirement" runat="server" DataSourceID="odsMarkoutRequirements"
                        DataTextField="Description" DataValueField="MarkoutRequirementID" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("MarkoutRequirementID") %>'>
                        <asp:ListItem>--Select Here--</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="label required">WBS Number:</td>
                <td class="control">
                <asp:TextBox runat="server" ID ="txtAccountCharged" Text='<%#Bind("AccountCharged") %>' 
                             style='<%#CurrentMvpMode == DetailViewMode.Insert || (bool)Eval("AccountNumberEditable") ? "display:;" : "display:none;"%>' />
                <asp:Label runat="server" ID ="lblAccountCharged" Text='<%#Eval("AccountCharged") %>' 
                           Visible='<%#CurrentMvpMode != DetailViewMode.Insert && !(bool)Eval("AccountNumberEditable") %>'/>
                </td>
            </tr>
            <tr id="trSafetyRequirements">
                <td class="label required">Traffic Control Required? </td>
                <td class="control">
                    <asp:CheckBox runat="server" ID="chkTrafficControlRequired" Checked='<%# Bind("TrafficControlRequired") %>' />
                </td>
                <td class="label required">Street Opening Permit Required? </td>
                <td class="control">
                    <asp:CheckBox runat="server" ID="chkStreetOpeningPermitRequired" Checked='<%# Bind("StreetOpeningPermitRequired") %>' />
                </td>
            </tr>
            <tr>
                <td class="label required">Digital As-Built Required?</td>
                <td class="control">
                    <asp:CheckBox runat="server" ID="chkDigitalAsBuiltRequired" Checked='<%# Bind("DigitalAsBuiltRequired") %>' />
                </td>
                <td class="label required">Digital As-Built Completed?</td>
                <td class="control">
                    <mmsinc:MvpDropDownList runat="server" id="ddlDigitalAsBuiltCompleted" SelectedValue='<%# Bind("DigitalAsBuiltCompleted") %>'>
                        <asp:ListItem Text="--Select Here--" Value="" />
                        <asp:ListItem Text="Yes" Value="True" />
                        <asp:ListItem Text="No" Value="False" />
                    </mmsinc:MvpDropDownList>
                </td>
            </tr>
            <tr>
                <td class="label">Notes: </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtNotes" TextMode="MultiLine" CssClass="three-cell" Text='<%# Bind("Notes") %>' Visible='<%#AllowNotesEdit %>' />
                    <asp:Label runat="server" ID="lblNotes" Text='<%# Eval("Notes") %>' Visible='<%# !this.AllowNotesEdit %>'></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Date Completed:</td>
                <td><asp:Label runat="server" Text='<%# Eval("DateCompleted", "{0:M/d/yyyy}") %>' ID="lblDateCompleted" /></td>
                <td class="label required">Date Received:</td>
                <td class="control">
                    <asp:TextBox runat="server" ID="ccDateReceived" Text='<%# Bind("DateReceived") %>' Visible='<%# AllowNotesEdit %>' autocomplete="off" />
                    <asp:Label runat="server" ID="lblDateReceived" Text='<%#Eval("DateReceived",CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE) %>' Visible='<%# !AllowNotesEdit %>' />
                    <atk:CalendarExtender runat="server" ID="ceDateReceived" TargetControlID="ccDateReceived" Enabled='<%#AllowNotesEdit %>' Format=""/>
                </td>
            </tr>
            <tr>
                <td>SAP Notification #:</td>
                <td>
                    <asp:TextBox runat="server" ID="txtSAPNotificationNumber" Text='<%# Bind("SAPNotificationNumber")%>' ReadOnly='<%#CurrentMvpMode != DetailViewMode.Insert && (bool)Eval("IsSapUpdatableWorkOrder") %>' />
                </td>
                <td class="label">Supervisor Approved On:</td>
                <td><asp:Label runat="server" id="lblApprovedOn" Text='<%#Eval("ApprovedOn",CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE) %>'></asp:Label></td>
            </tr>
            <tr>
                <td>SAP Work Order #:</td>
                <td>
                    <asp:TextBox runat="server" ID="txtSAPWorkOrderNumber" Text='<%# Bind("SAPWorkOrderNumber")%>' ReadOnly='<%#CurrentMvpMode != DetailViewMode.Insert && (bool)Eval("IsSapUpdatableWorkOrder") %>' />
                </td>
                <td class="label">Materials Approved On:</td>
                <td><asp:Label runat="server" id="Label2" Text='<%#Eval("MaterialsApprovedOn",CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE) %>'></asp:Label></td>
            </tr>
            
            <tr>
                <td>SAP Status:</td>
                <%--This is read only--%>
                <td><asp:Label runat="server" ID="lblSAPErrorCode" Text='<%# Eval("SAPErrorCode")%>' /></td>
                <td>Material Planning Completed On:</td>
                <td><asp:Label runat="server" ID="lblMaterialPlanningCompletedOn" Text='<%#Eval("MaterialPlanningCompletedOn") %>'></asp:Label></td>

            </tr>
        </table>

        <script type="text/javascript">
            $(document).ready(function() { WorkOrderInputFormView.initializeInputEdit() });
        </script>
        
        <mmsinc:MvpPlaceHolder runat="server" ID="phContractorAssigned" 
            Visible='<%#Eval("AssignedContractor") != null && CurrentMvpMode == DetailViewMode.Edit %>'>
            Contractor Assigned to: <asp:Label runat="server" Text='<%# string.Concat(Eval("AssignedContractor"), " On ", Eval("AssignedToContractorOn")) %>'/>
            <asp:LinkButton runat="server" ID="btnRemoveContractorAssignment"
                 OnClick="btnRemoveContractorAssignment_Click" 
                 Text="(click to remove assigned contractor)"
                 OnClientClick="return confirm('Are you sure you want to remove this contractor assignment?');"/>
                 <br/>
        </mmsinc:MvpPlaceHolder>

    </EditItemTemplate>
</wo:WorkOrdersFormView>


<mmsinc:MvpButton runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" OnClientClick="return WorkOrderInputFormView.btnSave_Click();" Visible="false" />
<input type="button" id="btnDummySave" value="Saving..." style="display:none;" disabled="disabled" />

<wo:RequesterIDsScript runat="server" />
<wo:AssetTypeIDsScript runat="server" />
<wo:WorkDescriptionsScript runat="server" />

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrder"
    OnInserted="ods_Inserted" OnUpdated="ods_Updated" OnDeleted="ods_Deleted" />

<mmsinc:MvpObjectDataSource runat="server" ID="odsTowns" TypeName="WorkOrders.Model.TownRepository"
    SelectMethod="SelectByOperatingCenterID">
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" DbType="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>

<asp:ObjectDataSource runat="server" ID="odsAssetTypes" TypeName="WorkOrders.Model.AssetTypeRepository"
    SelectMethod="SelectAllAsList" />

<asp:ObjectDataSource runat="server" ID="odsWorkOrderRequesters" TypeName="WorkOrders.Model.WorkOrderRequesterRepository"
    SelectMethod="SelectAllAsList" />

<asp:ObjectDataSource runat="server" ID="odsAcoustingMonitoringTypes" TypeName="WorkOrders.Model.AcousticMonitoringTypeRepository"
    SelectMethod="SelectAllAsList"/>

<mmsinc:MvpObjectDataSource runat="server" ID="odsEmployees" TypeName="WorkOrders.Model.EmployeeRepository"
    SelectMethod="GetEmployeesByOperatingCenterID">
    <SelectParameters>
        <asp:Parameter Name="OperatingCenterID" Type="Int32" />
    </SelectParameters>
</mmsinc:MvpObjectDataSource>

<asp:ObjectDataSource runat="server" ID="odsWorkOrderPurposes" TypeName="WorkOrders.Model.WorkOrderPurposeRepository"
    SelectMethod="SelectAllAsList" />

<asp:ObjectDataSource runat="server" ID="odsWorkOrderPriorities" TypeName="WorkOrders.Model.WorkOrderPriorityRepository"
    SelectMethod="SelectAllAsList" />

<asp:ObjectDataSource runat="server" ID="odsMarkoutRequirements" TypeName="WorkOrders.Model.MarkoutRequirementRepository"
    SelectMethod="SelectAllAsList" />
<asp:ObjectDataSource runat="server" ID="odsPlantMaintenanceActivityTypes" TypeName="WorkOrders.Model.PlantMaintenanceActivityTypeRepository"
    SelectMethod="SelectForWorkOrderInput" />
<asp:ObjectDataSource runat="server" ID="odsOperatingCenters" 
    TypeName="WorkOrders.Library.Permissions.SecurityService"
    SelectMethod="SelectUserOperatingCenters" />

<asp:ObjectDataSource runat="server" ID="odsCustomerImpactRanges" TypeName="WorkOrders.Model.CustomerImpactRangeRepository"
    SelectMethod="SelectAllSorted" />

<asp:ObjectDataSource runat="server" ID="odsRepairTimeRanges" TypeName="WorkOrders.Model.RepairTimeRangeRepository"
    SelectMethod="SelectAllSorted" />
