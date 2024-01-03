<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderSupervisorApprovalDetailView.ascx.cs"
    Inherits="LINQTo271.Views.WorkOrders.SupervisorApproval.WorkOrderSupervisorApprovalDetailView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInitialInputForm" Src="~/Controls/WorkOrders/WorkOrderInputFormView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMaterialsUsedForm" Src="~/Controls/WorkOrders/WorkOrderMaterialsUsedForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderRestorationForm" Src="~/Controls/WorkOrders/WorkOrderRestorationForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderRequisitionForm" Src="~/Controls/WorkOrders/WorkOrderRequisitionForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderAdditionalFinalizationInfoForm" Src="~/Controls/WorkOrders/WorkOrderAdditionalFinalizationInfoForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderSpoilsForm" Src="~/Controls/WorkOrders/WorkOrderSpoilsForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMainBreakForm" Src="~/Controls/WorkOrders/WorkOrderMainBreakForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderAccountForm" Src="~/Controls/WorkOrders/WorkOrderAccountForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderDocumentForm" Src="~/Controls/WorkOrders/WorkOrderDocumentsForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderCrewAssignmentForm" Src="~/Controls/WorkOrders/WorkOrderCrewAssignmentForm.ascx" %>
<%@ Register tagPrefix="wo" tagName="WorkOrderScheduleOfValuesForm" src="~/Controls/WorkOrders/WorkOrderScheduleOfValuesForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderSewerOverflowForm" Src="~/Controls/WorkOrders/WorkOrderSewerOverflowForm.ascx" %>
<%@ Register tagPrefix="wo" tagName="WorkOrderNewServiceInstallation" src="~/Controls/WorkOrders/NewServiceInstallationForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutForm" Src="~/Controls/WorkOrders/WorkOrderMarkoutForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutViolationForm" Src="~/Controls/WorkOrders/WorkOrderMarkoutViolationForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutDamageForm" Src="~/Controls/WorkOrders/WorkOrderMarkoutDamageForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderTrafficControlForm" Src="~/Controls/WorkOrders/WorkOrderTrafficControlForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="TrafficControlForm" Src="~/Controls/WorkOrders/TrafficControlForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="JobSiteCheckListForm" Src="~/Controls/WorkOrders/JobSiteCheckListForm.ascx" %>
<%@ Register tagPrefix="wo" tagName="JobObservationForm" src="~/Controls/WorkOrders/JobObservationForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMapForm" Src="~/Controls/WorkOrders/WorkOrderMapForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderStreetOpeningPermitForm" Src="~/Controls/WorkOrders/WorkOrderStreetOpeningPermitForm.ascx" %>

<mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID">
    <EditItemTemplate>
        <div id="divMain">
            <div style="font-weight: bold; font-size: larger">
                Order Number:
                <asp:Label runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' /><br />
            </div>
            <br />
            <div id="divContent" class="tabsContainer">
                <ul>
                    <li><a href="#initial" class="tab"><span>Initial Information</span></a></li>
<%-- NOT NEEDED JUST YET:
                <li><a href="#safetyMarkers" id="safetyMarkersTab"><span>Safety Markers</span></a></li>
--%>
                    <li><a href="#map" id="mapTab"><span>Map</span></a></li>
                    <li><a href="#materialsUsed" id="materialsUsedTab"><span>Materials</span></a></li>
                    <li><a href="#spoils" id="spoilsTab"><span>Spoils</span></a></li>
                    <li><a href="#markouts" class="tab"><span>Markouts</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phMarkoutViolations">
                        <li><a href="#markoutViolations" class="tab"><span>Markout Violations</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#markoutDamages" class="tab" id="markoutDamagesTab"><span>Markout Damages</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phSewerOverflows" Visible='<%# Eval("SewerOverflowsVisible") %>'>
                        <li><a href="#sewerOverflows" class="tab"><span>Sewer Overflows</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#traffic" class="tab"><span>Traffic Control</span></a></li>
                    <li><a href="#restoration" id="restorationTab"><span>Restoration</span></a></li>
                    <li><a href="#crewAssignments" id="crewAssignmentsTab"><span>Crew Assignments</span></a></li>
                    <li><a href="#additional" id="additionalTab"><span>Additional</span></a></li>
                    <li><a href="#account" id="accountTab"><span>Account</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phSOPTab" Visible='<%# Eval("StreetOpeningPermitRequired") %>'>
                        <li><a href="#street-opening-permit"><span>Street Opening Permit</span></a></li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phWorkOrderMainBreakForm" Visible='<%# Eval("WorkDescription.IsMainReplaceOrRepair") %>'>
                        <li><a href="#mainbreak" id="a1"><span>Main Break</span></a></li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phScheduleOfValues" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>'>
                        <li><a href="#scheduleOfValues" id="scheduleOfValuesTab"><span>Sched of Values</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#requisitions" id="requisitionsTab"><span>Purchase Orders (PO)</span></a></li>
                    <li><a href="#jobsitechecklist" id="jobsitechecklistTab" class="tab"><span>Job Site Check Lists</span></a></li>
                    <li><a href="#jobobservation" id="jobobservationTab" class="tab"><span>Job Observations</span></a></li>
                    <asp:PlaceHolder runat="server" id="phNewServiceInstallation" Visible='<%# Eval("IsNewServiceInstallation") %>'>
                        <li><a href="#newServiceInstallations" id="newServiceInstallationsTab" class="tab"><span>Set Meter</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#document" class="tab"><span>Documents</span></a></li>
                </ul>

                <!-- INITIAL INFORMATION -->
                <div id="initial">
                    <wo:WorkOrderInitialInputForm runat="server" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>
                
                <!-- MAP -->
                <div id="map">
                    <wo:WorkOrderMapForm runat="server" InitialMode="Edit" WorkOrderID='<%# Eval("WorkOrderID") %>' />
                </div>

                <!-- MATERIALS USED -->
                <div id="materialsUsed">
                    <wo:WorkOrderMaterialsUsedForm runat="server" ID="womuMaterialsUsed" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- SPOILS -->
                <div id="spoils">
                    <wo:WorkOrderSpoilsForm runat="server" ID="wospSpoils" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>
                
                <!-- MARKOUTS -->
                <div id="markouts">
                    <wo:WorkOrderMarkoutForm runat="server" InitialMode="ReadOnly" WorkOrderID='<%# Eval("WorkOrderID") %>' MarkoutRequirementID='<%# Eval("MarkoutRequirementID") %>' />
                </div>
                
                <!-- MARKOUT VIOLATIONS -->
                <div id="markoutViolations">
                    <wo:WorkOrderMarkoutViolationForm runat="server" InitialMode="ReadOnly" WorkOrderID='<%#Eval("WorkOrderID") %>'/>
                </div>
                
                <!-- MARKOUT DAMAGES -->
                <div id="markoutDamages">
                    <wo:WorkOrderMarkoutDamageForm runat="server" InitialMode="ReadOnly" WorkOrderID='<%# Eval("WorkOrderID") %>' Visible="True" />
                </div>

                <!-- SEWER OVERFLOWS -->
                <div id="sewerOverflows">
                    <wo:WorkOrderSewerOverflowForm runat="server" InitialMode="ReadOnly"  WorkOrderID='<%#Eval("WorkOrderID") %>' Visible='<%#Eval("SewerOverflowsVisible") %>' />
                </div>

                <!-- TRAFFIC CONTROL -->
                <div id="traffic">
                    <wo:WorkOrderTrafficControlForm runat="server" id="woTrafficControl" InitialMode="ReadOnly" WorkOrderID='<%# Eval("WorkOrderID") %>' />
                    <wo:TrafficControlForm runat="server" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="true" />
                </div>
                
                <!-- RESTORATION -->
                <div id="restoration">
                    <wo:WorkOrderRestorationForm runat="server" ID="worRestoration" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- CREW ASSIGNMENTS -->
                <div id="crewAssignments">
                    <wo:WorkOrderCrewAssignmentForm runat="server" ID="woCrewAssignments" WorkOrderID='<%# Eval("WorkOrderID") %>'
                        InitialMode="ReadOnly" />
                </div>
                
                <!-- ADDITIONAL -->
                <div id="additional">
                    <wo:WorkOrderAdditionalFinalizationInfoForm runat="server" ID="woafiAdditionalInfo" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" OnUpdating="ods_Updated" />
                </div>

                <!-- ACCOUNT -->
                <div id="account"> 
                    <wo:WorkOrderAccountForm runat="server" id="woAccountForm" InitialMode="Edit" WorkOrderID='<%# Eval("WorkOrderID") %>' OnUpdating="ods_Updated" />
                </div>
               
                <!-- STREET OPENING PERMITS -->
                <asp:PlaceHolder runat="server" ID="pnlStreetOpeningPermit" Visible='<%# Eval("StreetOpeningPermitRequired") %>'>
                    <div id="street-opening-permit">
                        <wo:WorkOrderStreetOpeningPermitForm runat="server" id="woStreetOpeningPermitForm" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                    </div>
                </asp:PlaceHolder>

                <!-- MAIN BREAK -->
                <asp:PlaceHolder runat="server" ID="pnlMainBreak" Visible='<%# Eval("WorkDescription.IsMainReplaceOrRepair") %>'>
                    <div id="mainbreak">
                        <wo:WorkOrderMainBreakForm runat="server" id="WorkOrderMainBreakForm" WorkOrderID='<%# Eval("WorkOrderID") %>' WorkDescriptionID='<%#Eval("WorkDescriptionID")%>' />
                    </div>
                </asp:PlaceHolder>
                                
                <!-- SCHEDULE OF VALUE LABOR ITEMS -->
                <asp:PlaceHolder runat="server" ID="pnlScheduleOfValues" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing")%>' >
                    <div id="scheduleOfValues">
                        <asp:HiddenField runat="server" ID="hidOperatingCenterHasWorkOrderInvoicing" Value='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>' />
                        <wo:WorkOrderScheduleOfValuesForm runat="server" ID="woScheduleOfValues" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="Edit"/>
                    </div>
                </asp:PlaceHolder>

                <!-- REQUISITIONS -->
                <div id="requisitions">
                    <wo:WorkOrderRequisitionForm runat="server" ID="worRequisitions" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="Edit" />
                </div>

                <!-- JOB SITE CHECK LISTS -->
                <div id="jobsitechecklist">
                    <wo:JobSiteCheckListForm runat="server" ID="worJobSiteCheckListForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="true" />
                </div>
                
                <!-- JOB OBSERVATIONS -->
                <div id="jobobservation">
                    <wo:JobObservationForm runat="server" ID="worJobObservationForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="true"/>
                </div>

                <!-- NEW SERVICE INSTALLATION -->
                <asp:PlaceHolder runat="server" ID="pnlNewServiceInstallations" Visible='<%# Eval("IsNewServiceInstallation") %>'>
                    <div id="newServiceInstallations">
                        <wo:WorkOrderNewServiceInstallation runat="server" ID="woWorkOrderNewServiceInstallationForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="True"/>
                    </div>
                </asp:PlaceHolder>

                <!-- DOCUMENTS -->
                <div id="document">
                    <mmsinc:MvpUpdatePanel runat="server" ID="upDocuments" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <wo:WorkOrderDocumentForm runat="server" ID="woDocumentForm" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                        </ContentTemplate>
                    </mmsinc:MvpUpdatePanel>
                </div>
            </div>
        </div>
        <!-- Hidden/DisplayNones -->
        <%-- THESE NEED TO BE HERE SO THAT THESE VALUES WILL PERSIST --%>
        <mmsinc:MvpHiddenField runat="server" ID="hidTrafficControlRequired" Value='<%# Bind("TrafficControlRequired") %>' />
        <mmsinc:MvpHiddenField runat="server" ID="hidStreetOpeningPermitRequired" Value='<%# Bind("StreetOpeningPermitRequired") %>' />
        <mmsinc:MvpHiddenField runat="server" ID="hidDigitalAsBuiltRequired" Value='<%# Bind("DigitalAsBuiltRequired") %>' />
        <mmsinc:MvpHiddenField runat="server" ID="hidHydrantID" Value='<%#Eval("HydrantID") %>'/>
        <mmsinc:MvpHiddenField runat="server" ID="hidValveID" Value='<%#Eval("ValveID") %>'/>
        <mmsinc:MvpHiddenField runat="server" ID="hidPremiseNumber" Value='<%#Eval("PremiseNumber") %>'/>
        <mmsinc:MvpHiddenField runat="server" ID="hidAssetTypeID" Value='<%#Eval("AssetTypeID") %>'/>
    </EditItemTemplate>
</mmsinc:MvpFormView>

<mmsinc:ClientIDRepository runat="server" />

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrder"
    OnInserted="ods_Inserted" OnUpdated="ods_Updated" OnDeleted="ods_Deleted" />
