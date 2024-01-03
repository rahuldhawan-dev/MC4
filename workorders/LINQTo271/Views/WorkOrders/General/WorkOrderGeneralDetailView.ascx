<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderGeneralDetailView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.General.WorkOrderGeneralDetailView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInputFormView" Src="~/Controls/WorkOrders/WorkOrderInputFormView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMaterialsUsedForm" Src="~/Controls/WorkOrders/WorkOrderMaterialsUsedForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderSpoilsForm" Src="~/Controls/WorkOrders/WorkOrderSpoilsForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderRestorationForm" Src="~/Controls/WorkOrders/WorkOrderRestorationForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMainBreakForm" Src="~/Controls/WorkOrders/WorkOrderMainBreakForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderStreetOpeningPermitForm" Src="~/Controls/WorkOrders/WorkOrderStreetOpeningPermitForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMapForm" Src="~/Controls/WorkOrders/WorkOrderMapForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderAccountForm" Src="~/Controls/WorkOrders/WorkOrderAccountForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderTrafficControlForm" Src="~/Controls/WorkOrders/WorkOrderTrafficControlForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderAdditionalFinalizationInfoForm" Src="~/Controls/WorkOrders/WorkOrderAdditionalFinalizationInfoForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderStockApprovalForm" Src="~/Controls/WorkOrders/WorkOrderStockApprovalForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutForm" Src="~/Controls/WorkOrders/WorkOrderMarkoutForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutViolationForm" Src="~/Controls/WorkOrders/WorkOrderMarkoutViolationForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutDamageForm" Src="~/Controls/WorkOrders/WorkOrderMarkoutDamageForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderSewerOverflowForm" Src="~/Controls/WorkOrders/WorkOrderSewerOverflowForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderDocumentForm" Src="~/Controls/WorkOrders/WorkOrderDocumentsForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderCrewAssignmentForm" Src="~/Controls/WorkOrders/WorkOrderCrewAssignmentForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderRequisitionForm" Src="~/Controls/WorkOrders/WorkOrderRequisitionForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="JobSiteCheckListForm" Src="~/Controls/WorkOrders/JobSiteCheckListForm.ascx" %>
<%@ Register tagPrefix="wo" tagName="JobObservationForm" src="~/Controls/WorkOrders/JobObservationForm.ascx" %>
<%@ Register tagPrefix="wo" tagName="WorkOrderScheduleOfValuesForm" src="~/Controls/WorkOrders/WorkOrderScheduleOfValuesForm.ascx" %>
<%@ Register tagPrefix="wo" tagName="WorkOrderInvoiceForm" src="~/Controls/WorkOrders/WorkOrderInvoiceForm.ascx" %>
<%@ Register tagPrefix="wo" tagName="WorkOrderNewServiceInstallation" src="~/Controls/WorkOrders/NewServiceInstallationForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="TrafficControlForm" Src="~/Controls/WorkOrders/TrafficControlForm.ascx" %>
<%@ Register TagPrefix="wo" Namespace="LINQTo271.Controls.WorkOrders" Assembly="LINQTo271" %>

<mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID" 
    EmptyDataText="Unable to locate the Work Order. Either the Work Order does not exist or you do not have the required access rights.">
    <ItemTemplate>
        <div id="divMain">
            <div style="font-weight: bold; font-size: larger; <%# Eval("CancelledAt") != null ? "background-color: orange;" : "" %>">
                Order Number: <asp:Label runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' CssClass="WorkOrderIDLabel" />
                <mmsinc:MvpPanel runat="server" id="pnlCancellationDetails" Visible='<%# Eval("CancelledAt") != null %>'>
                    Cancelled On:
                    <asp:Label runat="server" id="lblCancelledAt" Text='<%# Eval("CancelledAt", "{0:M/d/yyyy}") %>'/>
                    By:
                    <asp:Label runat="server" ID="lblCancelledBy" Text='<%# Eval("CancelledBy") %>'/>
                    <br/>
                    Reason: 
                    <asp:Label runat="server" ID="Label1" Text='<%# Eval("WorkOrderCancellationReason") %>'/>
                </mmsinc:MvpPanel>
            </div><br /><br/>
            <div id="divContent" class="tabsContainer">
                <ul>
                    <li><a href="#initial" class="tab"><span>Initial Information</span></a></li>
                    <li><a href="#map" id="mapTab"><span>Map</span></a></li>
                    <li><a href="#materialsUsed" id="materialsUsedTab"><span>Materials</span></a></li>
                    <li><a href="#spoils" id="spoilsTab"><span>Spoils</span></a></li>
                    <li><a href="#markouts" class="tab"><span>Markouts</span></a></li>
                    <asp:PlaceHolder runat="server" ID="PlaceHolder1">
                        <li><a href="#markoutViolations" class="tab"><span>Markout Violations</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#markoutDamage" class="tab" id="markoutDamagesTab"><span>Markout Damages</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phSewerOverflows" Visible='<%# Eval("SewerOverflowsVisible") %>'>
                        <li><a href="#sewerOverflows" class="tab"><span>Sewer Overflows</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#traffic" class="tab"><span>Traffic Control</span></a></li>
                    <li><a href="#restoration" id="restorationTab"><span>Restoration</span></a></li>
                    <li><a href="#crewAssignments" id="crewAssignmentsTab"><span>Crew Assignments</span></a></li>
                    <li><a href="#additional" id="additionalTab"><span>Additional</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phAccountTab" Visible='<%# Eval("WorkCompleted") %>'>
                        <li><a href="#account" class="tab"><span>Account</span></a></li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phSOPTab" Visible='<%# Eval("StreetOpeningPermitRequired") %>'>
                        <li><a href="#street-opening-permit"><span>Street Opening Permit</span></a></li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phWorkOrderMainBreakForm">
                        <li id='mainBreakLi' style='display: <%# (Eval("WorkDescription.IsMainReplaceOrRepair").ToString().ToLower() == "true") ? "" : "none"  %>'>
                            <a href="#mainbreak" id="mainBreakTab"><span>Main Break</span></a>
                        </li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phScheduleOfValues" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>'>
                        <li><a href="#scheduleOfValues" id="scheduleOfValuesTab"><span>Sched of Values</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#requisitions" id="requisitionsTab" class="tab"><span>Purchase Orders (PO)</span></a></li>
                    <li><a href="#jobsitechecklist" id="jobsitechecklistTab" class="tab"><span>Job Site Check Lists</span></a></li>
                    <li><a href="#jobobservation" id="jobobservationTab" class="tab"><span>Job Observations</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phInvoicesTab" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>'>
                        <li><a href="#invoices" id="invoicesTab" class="tab"><span>Invoices</span></a></li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" id="phNewServiceInstallation" Visible='<%# Eval("IsNewServiceInstallation") %>'>
                        <li><a href="#newServiceInstallations" id="newServiceInstallationsTab" class="tab"><span>Set Meter</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#document" class="tab"><span>Documents</span></a></li>
                </ul>

                <!-- INITIAL INFORMATION -->
                <div id="initial">
                    <wo:WorkOrderInputFormView runat="server" ID="wofvInitialInformation" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- MAP -->
                <div id="map">
                    <wo:WorkOrderMapForm runat="server" InitialMode="Edit" WorkOrderID='<%# Eval("WorkOrderID") %>' />
                </div>
                
                <!-- MATERIALS USED -->
                <div id="materialsUsed">
                    <wo:WorkOrderMaterialsUsedForm runat="server" ID="womuMaterialsUsed" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                    <wo:WorkOrderStockApprovalForm runat="server" ID="woStockApproval" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
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
                <div id="markoutDamage">
                    <wo:WorkOrderMarkoutDamageForm runat="server" InitialMode="ReadOnly" WorkOrderID='<%# Eval("WorkOrderID") %>' Visible="True" />
                </div>

                <!-- SEWER OVERFLOWS -->
                <div id="sewerOverflows">
                    <wo:WorkOrderSewerOverflowForm runat="server" InitialMode="ReadOnly" WorkOrderID='<%# Eval("WorkOrderID") %>' Visible='<%# Eval("SewerOverflowsVisible") %>' />
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

                <!-- ADDITIONAL INFORMATION -->
                <div id="additional">
                    <wo:WorkOrderAdditionalFinalizationInfoForm runat="server" ID="woafiAdditionalInfo" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- ACCOUNT INFORMATION -->
                <asp:PlaceHolder runat="server" ID="pnlAccountForm" Visible='<%# Eval("WorkCompleted") %>'>
                    <div id="account">
                        <wo:WorkOrderAccountForm runat="server" id="woAccountForm" InitialMode="ReadOnly" WorkOrderID='<%# Eval("WorkOrderID") %>' />
                    </div>
                </asp:PlaceHolder>

                <!-- STREET OPENING PERMITS -->
                <asp:PlaceHolder runat="server" ID="pnlStreetOpeningPermit" Visible='<%# Eval("StreetOpeningPermitRequired") %>'>
                    <div id="street-opening-permit">
                        <wo:WorkOrderStreetOpeningPermitForm runat="server" id="woStreetOpeningPermitForm" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                    </div>
                </asp:PlaceHolder>
                
                <!-- MAIN BREAKS -->
                <asp:PlaceHolder runat="server" ID="pnlMainBreak" Visible='<%# Eval("WorkDescription.IsMainReplaceOrRepair") %>'>
                    <div id="mainbreak">
                        <wo:WorkOrderMainBreakForm runat="server" id="woMainBreakForm" WorkOrderID='<%# Eval("WorkOrderID") %>' WorkDescriptionID='<%#Eval("WorkDescriptionID")%>' InitialMode="ReadOnly" />
                    </div>
                </asp:PlaceHolder>
                
                <!-- SCHEDULE OF VALUES -->
                <asp:PlaceHolder runat="server" ID="pnlScheduleOfValues" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing")%>' >
                    <div id="scheduleOfValues">
                        <wo:WorkOrderScheduleOfValuesForm runat="server" ID="woScheduleOfValues" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly"/>
                    </div>
                </asp:PlaceHolder>
                                
                <!-- REQUISITIONS -->
                <div id="requisitions">
                    <wo:WorkOrderRequisitionForm runat="server" ID="worRequisitions" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>
                
                <!-- JOB SITE CHECK LISTS -->
                <div id="jobsitechecklist">
                    <wo:JobSiteCheckListForm runat="server" ID="worJobSiteCheckListForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="true" />
                </div>
                
                <!-- JOB OBSERVATIONS -->
                <div id="jobobservation">
                    <wo:JobObservationForm runat="server" ID="worJobObservationForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="true"/>
                </div>
                
                <!-- INVOICES -->
                <asp:PlaceHolder runat="server" ID="phInvoices" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>'>
                <div id="invoices">
                    <wo:WorkOrderInvoiceForm runat="server" ID="woWorkOrderInvoiceForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="true"/>
                </div>
                </asp:PlaceHolder>

                <!-- NEW SERVICE INSTALLATION -->
                <asp:PlaceHolder runat="server" ID="pnlNewServiceInstallations" Visible='<%# Eval("IsNewServiceInstallation") %>'>
                    <div id="newServiceInstallations">
                        <wo:WorkOrderNewServiceInstallation runat="server" ID="woWorkOrderNewServiceInstallationForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="True"/>
                    </div>
                </asp:PlaceHolder>

                </asp:PlaceHolder>
                <!-- DOCUMENTS -->
                <div id="document">
                    <wo:WorkOrderDocumentForm runat="server" ID="woDocumentForm" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>
            </div>
        </div>
    </ItemTemplate>
    <EditItemTemplate>
        <div id="divMain">
            <div style="font-weight: bold; font-size: larger; <%# Eval("CancelledAt") != null ? "background-color: orange;" : "" %>">
                Order Number: <asp:Label runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' CssClass="WorkOrderIDLabel" /><br />
                <mmsinc:MvpPanel runat="server" id="pnlCancellationDetails" Visible='<%# Eval("CancelledAt") != null %>'>
                    Cancelled On:
                    <asp:Label runat="server" id="lblCancelledAt" Text='<%# Eval("CancelledAt", "{0:M/d/yyyy}") %>'/>
                    By:
                    <asp:Label runat="server" ID="lblCancelledBy" Text='<%# Eval("CancelledBy") %>'/>
                    <br/>
                    Reason: 
                    <asp:Label runat="server" ID="Label1" Text='<%# Eval("WorkOrderCancellationReason") %>'/>
                </mmsinc:MvpPanel>
            </div><br />
            <div id="divContent" class="tabsContainer">
                <ul>
                    <li><a href="#initial" class="tab"><span>Initial Information</span></a></li>
                    <li><a href="#map" id="mapTab"><span>Map</span></a></li>
                    <li><a href="#materialsUsed" id="materialsUsedTab"><span>Materials</span></a></li>
                    <li><a href="#spoils" id="spoilsTab"><span>Spoils</span></a></li>
                    <li><a href="#markouts" class="tab"><span>Markouts</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phMarkoutViolations">
                        <li><a href="#markoutViolations" class="tab"><span>Markout Violations</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#markoutDamage" class="tab"><span>Markout Damage</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phSewerOverflows" Visible='<%# Eval("SewerOverflowsVisible") %>'>
                        <li><a href="#sewerOverflows" class="tab"><span>Sewer Overflows</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#traffic" class="tab"><span>Traffic Control</span></a></li>
                    <li><a href="#restoration" id="restorationTab"><span>Restoration</span></a></li>
                    <li><a href="#crewAssignments" id="crewAssignmentsTab"><span>Crew Assignments</span></a></li>
                    <li><a href="#additional" id="additionalTab"><span>Additional</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phAccountTab" Visible='<%# Eval("WorkCompleted") %>'>
                        <li><a href="#account" class="tab"><span>Account</span></a></li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phSOPTab" Visible='<%# Eval("StreetOpeningPermitRequired") %>'>
                        <li><a href="#street-opening-permit"><span>Street Opening Permit</span></a></li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phWorkOrderMainBreakForm">
                        <li id='mainBreakLi' style='display: <%# (Eval("WorkDescription.IsMainReplaceOrRepair").ToString().ToLower() == "true") ? "" : "none"  %>'>
                            <a href="#mainbreak" id="mainBreakTab"><span>Main Break</span></a>
                        </li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phScheduleOfValues" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>'>
                        <li><a href="#scheduleOfValues" id="scheduleOfValuesTab"><span>Sched of Values</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#requisitions" id="requisitionsTab" class="tab"><span>Purchase Orders (PO)</span></a></li>
                    <li><a href="#jobsitechecklist" id="jobsitechecklistTab" class="tab"><span>Job Site Check Lists</span></a></li>
                    <li><a href="#invoices" id="invoicesTab" class="tab" Visible="False"><span>Invoices</span></a></li>
                    <asp:PlaceHolder runat="server" id="phNewServiceInstallation" Visible='<%# Eval("IsNewServiceInstallation") %>'>
                        <li><a href="#newServiceInstallations" id="newServiceInstallationsTab" class="tab"><span>Set Meter</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#document" class="tab"><span>Documents</span></a></li>
                </ul>

                <!-- INITIAL INFORMATION -->
                <div id="initial">
                    <wo:WorkOrderInputFormView runat="server" ID="wofvInitialInformation" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" OnUpdating="ods_Updated" />
                </div>

                <!-- MAP -->
                <div id="map">
                    <wo:WorkOrderMapForm runat="server" InitialMode="Edit" WorkOrderID='<%# Eval("WorkOrderID") %>' />
                </div>
                
                <!-- MATERIALS USED -->
                <div id="materialsUsed">
                    <asp:HiddenField runat="server" ID="hidOperatingCenterIDForMaterialLookup" Value='<%# Eval("OperatingCenterID") %>' />
                    <table>
                        <tbody>
                            <tr>
                                <td>Search:</td>
                                <td>
                                    <input type="text" id="txtPartSearch" onkeyup="WorkOrderGeneralDetailView.txtPartSearch_Keyup(this)" />
                                    <input type="button" value="Search" onclick="WorkOrderGeneralDetailView.txtPartSearch_Keyup(txtPartSearch)"/>
                                </td>
                                <td>Results:</td>
                                <td>
                                    <select size="5" id="lbPartSearchResults" onchange="WorkOrderGeneralDetailView.lbPartSearchResults_Change(this)" style="height: 100px; width: 325px;">
                                        <option disabled="disabled">No results found.</option>
                                    </select>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <mmsinc:MvpUpdatePanel runat="server" ID="upMaterialsUsed" UpdateMode="Always" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <wo:WorkOrderMaterialsUsedForm runat="server" ID="womuMaterialsUsed" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                        </ContentTemplate>
                    </mmsinc:MvpUpdatePanel>
                    <wo:WorkOrderStockApprovalForm runat="server" ID="woStockApproval" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- SPOILS -->
                <div id="spoils">
                    <wo:WorkOrderSpoilsForm runat="server" ID="wospSpoils" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                </div>

                <!-- MARKOUTS -->
                <div id="markouts">
                    <wo:WorkOrderMarkoutForm runat="server" InitialMode="Edit" WorkOrderID='<%# Eval("WorkOrderID") %>' MarkoutRequirementID='<%# Eval("MarkoutRequirementID") %>'></wo:WorkOrderMarkoutForm>
                </div>
                
                <!-- MARKOUT VIOLATIONS -->
                <div id="markoutViolations">
                    <wo:WorkOrderMarkoutViolationForm runat="server" InitialMode="ReadOnly" WorkOrderID='<%#Eval("WorkOrderID") %>'/>
                </div>
                
                <!-- MARKOUT DAMAGES -->
                <div id="markoutDamage">
                    <wo:WorkOrderMarkoutDamageForm runat="server" InitialMode="Edit" WorkOrderID='<%# Eval("WorkOrderID") %>' Visible="True" />
                </div>

                <!-- SEWER OVERFLOWS -->
                <div id="sewerOverflows">
                    <wo:WorkOrderSewerOverflowForm runat="server" InitialMode="ReadOnly"  WorkOrderID='<%#Eval("WorkOrderID") %>' Visible='<%#Eval("SewerOverflowsVisible") %>' />
                </div>

                <!-- TRAFFIC CONTROL -->
                <div id="traffic">
                    <wo:WorkOrderTrafficControlForm runat="server" id="woTrafficControl" InitialMode="Edit" 
                        WorkOrderID='<%# Eval("WorkOrderID") %>'  OnUpdating="ods_Updated"  />
                    <wo:TrafficControlForm runat="server" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="true" />
                </div>

                <!-- RESTORATION -->
                <div id="restoration">
                    <wo:WorkOrderRestorationForm runat="server" ID="worRestoration" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                </div>

                <!-- ADDITIONAL INFORMATION -->
                <div id="additional">
                    <wo:WorkOrderAdditionalFinalizationInfoForm runat="server" ID="woafiAdditionalInfo" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" OnUpdating="ods_Updated" />
                </div>

                <!-- CREW ASSIGNMENTS -->
                <div id="crewAssignments">
                    <mmsinc:MvpUpdatePanel runat="server" ID="upCrewAssignments" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <wo:WorkOrderCrewAssignmentForm runat="server" ID="woCrewAssignments" WorkOrderID='<%# Eval("WorkOrderID") %>'
                                InitialMode="Edit" />
                        </ContentTemplate>
                    </mmsinc:MvpUpdatePanel>
                </div>

                <!-- ACCOUNT INFORMATION -->
                <asp:PlaceHolder runat="server" ID="pnlAccountForm" Visible='<%# Eval("WorkCompleted") %>'>
                    <div id="account">
                        <wo:WorkOrderAccountForm runat="server" id="woAccountForm" InitialMode="ReadOnly" WorkOrderID='<%# Eval("WorkOrderID") %>'  />
                    </div>
                </asp:PlaceHolder>

                <!-- STREET OPENING PERMITS -->
                <asp:PlaceHolder runat="server" ID="pnlStreetOpeningPermit" Visible='<%# Eval("StreetOpeningPermitRequired") %>'>
                    <div id="street-opening-permit">
                        <wo:WorkOrderStreetOpeningPermitForm runat="server" id="woStreetOpeningPermitForm" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                    </div>
                </asp:PlaceHolder>
                
                <!-- MAIN BREAKS -->
                <asp:PlaceHolder runat="server" ID="pnlMainBreak" >
                    <div id="mainbreak">
                        <wo:WorkOrderMainBreakForm runat="server" id="woMainBreakForm" WorkOrderID='<%# Eval("WorkOrderID") %>' WorkDescriptionID='<%#Eval("WorkDescriptionID")%>' InitialMode="Edit" />
                    </div>
                </asp:PlaceHolder>
                                
                <!-- SCHEDULE OF VALUES -->
                <asp:PlaceHolder runat="server" ID="pnlScheduleOfValues" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing")%>' >
                    <div id="scheduleOfValues">
                        <asp:HiddenField runat="server" ID="hidOperatingCenterHasWorkOrderInvoicing" Value='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>' />
                        <wo:WorkOrderScheduleOfValuesForm runat="server" ID="woScheduleOfValues" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="Edit" />
                    </div>
                </asp:PlaceHolder>

                <!-- REQUISITIONS -->
                <div id="requisitions">
                    <wo:WorkOrderRequisitionForm runat="server" ID="worRequisitions" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="Edit" />
                </div>
                
                <!-- JOB SITE CHECK LISTS -->
                <div id="jobsitechecklist">
                    <wo:JobSiteCheckListForm runat="server" ID="worJobSiteCheckListForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="Edit" Visible="true" />
                </div>
                
                <!-- INVOICES -->
                <asp:PlaceHolder runat="server" ID="phInvoices" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>'>
                <div id="invoices">
                    <wo:WorkOrderInvoiceForm runat="server" ID="woWorkOrderInvoiceForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="Edit" />
                </div>
                </asp:PlaceHolder>
                
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
    </EditItemTemplate>
</mmsinc:MvpFormView>

<div class="container">
    <asp:Button runat="server" ID="btnRefresh" Text="Refresh" OnClick="btnEdit_Click" Visible="false" />
    <asp:Button runat="server" ID="btnEdit" Text="Edit" OnClick="btnEdit_Click" />
    <mmsinc:MvpButton runat="server" ID="btnCancelOrder" Text="Cancel Order" OnClientClick="$('#cancelDiv').toggle();return false;getServerElementById('ddlWorkOrderCancellationReasons').focus();"  />
    <mmsinc:MvpButton runat="server" ID="btnMaterialPlanningComplete" onClick="btnPlanningComplete_Click" 
        Text="Complete Material Planning" OnClientClick="return confirm('Are you sure you want to complete material planning?');" />
    <asp:HyperLink runat="server" Text="SAP Notifications" CssClass="linkButton" style="padding:6px 12px 0px 12px; min-height: 18px; top: .25px;" 
        NavigateUrl="../../../../mvc/FieldOperations/SapNotification/Search?invokeSearch=1" />
    
    <asp:HyperLink ID="lnkCreateService" runat="server" Text="Create Service" Visible="false" CssClass="linkButton" style="padding:6px 12px 0px 12px; min-height: 18px; top: .25px;" />
    <div id="cancelDiv" style="display: none; padding: 6px;">
    <fieldset>
        <legend>Cancel</legend>
        Reason for Cancellation: 
        <mmsinc:MvpDropDownList runat="server" ID="ddlWorkOrderCancellationReasons" 
            DataSourceID="odsWorkOrderCancellationReasons" DataTextField="Description" DataValueField="Id"
            AppendDataBoundItems="True" 
            ValidationGroup="cancelOrder">
            <asp:ListItem Text="--Select Here--" Value="" />
        </mmsinc:MvpDropDownList>
        <asp:Button runat="server" ID="btnDelete" Text="Cancel Order" OnClick="btnDelete_Click" 
            ValidationGroup="cancelOrder"/>
    </fieldset>
    </div>
</div>

<mmsinc:ClientIDRepository runat="server" />

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrder"
    OnUpdated="ods_Updated" />
<asp:ObjectDataSource runat="server" ID="odsWorkOrderCancellationReasons" TypeName="WorkOrders.Model.WorkOrderCancellationReasonRepository"
    SelectMethod="SelectAllAsList" />