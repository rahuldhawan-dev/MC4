<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderFinalizationDetailView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Finalization.WorkOrderFinalizationDetailView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register Assembly="LINQTo271" Namespace="LINQTo271.Common" TagPrefix="wo" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInitialInputForm" Src="~/Controls/WorkOrders/WorkOrderInputFormView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMainBreakForm" Src="~/Controls/WorkOrders/WorkOrderMainBreakForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMaterialsUsedForm" Src="~/Controls/WorkOrders/WorkOrderMaterialsUsedForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderSpoilsForm" Src="~/Controls/WorkOrders/WorkOrderSpoilsForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderRestorationForm" Src="~/Controls/WorkOrders/WorkOrderRestorationForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderAdditionalFinalizationInfoForm" Src="~/Controls/WorkOrders/WorkOrderAdditionalFinalizationInfoForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderDocumentForm" Src="~/Controls/WorkOrders/WorkOrderDocumentsForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderCrewAssignmentForm" Src="~/Controls/WorkOrders/WorkOrderCrewAssignmentForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutForm" Src="~/Controls/WorkOrders/WorkOrderMarkoutForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutDamageForm" Src="~/Controls/WorkOrders/WorkOrderMarkoutDamageForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutViolationForm" Src="~/Controls/WorkOrders/WorkOrderMarkoutViolationForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderSewerOverflowForm" Src="~/Controls/WorkOrders/WorkOrderSewerOverflowForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="JobSiteCheckListForm" Src="~/Controls/WorkOrders/JobSiteCheckListForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="TrafficControlForm" Src="~/Controls/WorkOrders/TrafficControlForm.ascx" %>
<%@ Register tagPrefix="wo" tagName="WorkOrderScheduleOfValuesForm" src="~/Controls/WorkOrders/WorkOrderScheduleOfValuesForm.ascx" %>
<%@ Register tagPrefix="wo" tagName="WorkOrderNewServiceInstallation" src="~/Controls/WorkOrders/NewServiceInstallationForm.ascx" %>

<mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID" OnItemUpdating="fvWorkOrder_ItemUpdating"> 
    <EditItemTemplate>
        <div id="divMain">
            <div style="font-weight: bold; font-size: larger">
                Order Number: <asp:Label runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' /><br />
            </div><br />
            <div class="tabsContainer">
                <ul>
                    <li><a href="#initial" class="tab"><span>Initial Information</span></a></li>
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
                    <li><a href="#restoration" id="restorationTab"><span>Restoration</span></a></li>
                    <li><a href="#crewAssignments" id="crewAssignmentsTab"><span>Crew Assignments</span></a></li>
                    <li><a href="#traffic" class="tab"><span>Traffic Control</span></a></li>
                    <li><a href="#additional" id="additionalTab"><span>Additional</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phWorkOrderMainBreakForm">
                        <li id='mainBreakLi' style='display: <%# (Eval("WorkDescription.IsMainReplaceOrRepair").ToString() == "true") ? "" : "none"  %>'>
                            <a href="#mainbreak" id="mainBreakTab"><span>Main Break</span></a>
                        </li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phScheduleOfValues" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>'>
                        <li><a href="#scheduleOfValues" id="scheduleOfValuesTab"><span>Sched of Values</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#jobsitechecklist" id="jobsitechecklistTab" class="tab"><span>Job Site Check Lists</span></a></li>
                    <li><a href="#document" class="tab"><span>Documents</span></a></li>
                     <asp:PlaceHolder runat="server" id="phNewServiceInstallation" Visible='<%# Eval("IsNewServiceInstallation") %>'>
                        <li><a href="#newServiceInstallations" id="newServiceInstallationsTab" class="tab"><span>Set Meter</span></a></li>
                    </asp:PlaceHolder>
                </ul>

                <!-- INITIAL INFORMATION -->
                <div id="initial">
                    <wo:WorkOrderInitialInputForm runat="server" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- MATERIALS USED -->
                <div id="materialsUsed">
                    <asp:HiddenField runat="server" ID="hidOperatingCenterIDForMaterialLookup" Value='<%# Eval("OperatingCenterID") %>' />
                    <mmsinc:MvpUpdatePanel runat="server" ID="upMaterialsUsed" UpdateMode="Always" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Search:</td>
                                        <td>
                                            <input type="text" id="txtPartSearch" onkeyup="WorkOrderFinalizationDetailView.txtPartSearch_Keyup(this)" />
                                            <input type="button" value="Search" onclick="WorkOrderFinalizationDetailView.txtPartSearch_Keyup(txtPartSearch)"/>
                                        </td>
                                        <td>Results:</td>
                                        <td>
                                            <select size="5" id="lbPartSearchResults" onchange="WorkOrderFinalizationDetailView.lbPartSearchResults_Change(this)" style="height: 100px; width: 325px;">
                                                <option disabled="disabled">No results found.</option>
                                            </select>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <wo:WorkOrderMaterialsUsedForm runat="server" TableCssClass="grid" ID="womufMaterialsUsed" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                        </ContentTemplate>
                    </mmsinc:MvpUpdatePanel>
                </div>

                <!-- SPOILS -->
                <div id="spoils">
                    <mmsinc:MvpUpdatePanel runat="server" ID="upSpoils">
                        <ContentTemplate>
                            <wo:WorkOrderSpoilsForm runat="server" ID="wospSpoils" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                        </ContentTemplate>
                    </mmsinc:MvpUpdatePanel>
                </div>

                <!-- MARKOUTS -->
                <div id="markouts">
                    <wo:WorkOrderMarkoutForm runat="server" InitialMode="Edit" WorkOrderID='<%# Eval("WorkOrderID") %>' MarkoutRequirementID='<%# Eval("MarkoutRequirementID") %>' />
                </div>
                
                <!-- MARKOUT VIOLATIONS -->
                <div id="markoutViolations">
                    <wo:WorkOrderMarkoutViolationForm runat="server" InitialMode="Edit" WorkOrderID='<%#Eval("WorkOrderID") %>'/>
                </div>
                
                <!-- MARKOUT DAMAGES -->
                <div id="markoutDamages">
                    <wo:WorkOrderMarkoutDamageForm runat="server" InitialMode="ReadOnly" WorkOrderID='<%# Eval("WorkOrderID") %>' Visible="True" />
                </div>

                <!-- SEWER OVERFLOWS -->
                <div id="sewerOverflows">
                    <wo:WorkOrderSewerOverflowForm runat="server" InitialMode="ReadOnly"  WorkOrderID='<%#Eval("WorkOrderID") %>' Visible='<%#Eval("SewerOverflowsVisible") %>' />
                </div>

                <!-- RESTORATION -->
                <div id="restoration">
                    <wo:WorkOrderRestorationForm runat="server" ID="wofrRestorations" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                </div>

                <!-- CREW ASSIGNMENTS -->
                <div id="crewAssignments">
                    <mmsinc:MvpUpdatePanel runat="server" ID="upCrewAssignments">
                        <ContentTemplate>
                            <wo:WorkOrderCrewAssignmentForm runat="server" id="woCrewAssignments" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                        </ContentTemplate>
                    </mmsinc:MvpUpdatePanel>
                </div>

                <!-- TRAFFIC CONTROL -->
                <div id="traffic">
                    <wo:TrafficControlForm runat="server" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="true" />
                </div>

                <!-- ADDITIONAL -->
                <div id="additional">
                    <wo:WorkOrderAdditionalFinalizationInfoForm runat="server" ID="woafiAdditionalInfo" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" OnUpdating="ods_Updated" />
                </div>
                
                <!-- MAIN BREAK -->
                <asp:PlaceHolder runat="server" ID="pnlMainBreak">
                    <div id="mainbreak" style='display: <%# (Eval("WorkDescription.IsMainReplaceOrRepair").ToString() == "true") ? "" : "none"  %>'>
                        <wo:WorkOrderMainBreakForm runat="server" id="WorkOrderMainBreakForm" WorkOrderID='<%# Eval("WorkOrderID") %>' WorkDescriptionID='<%#Eval("WorkDescriptionID")%>' InitialMode="Edit" />
                    </div>
                </asp:PlaceHolder>
                
                <!-- SCHEDULE OF VALUE LABOR ITEMS -->
                <asp:PlaceHolder runat="server" ID="pnlScheduleOfValues" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing")%>' >
                    <div id="scheduleOfValues">
                        <asp:HiddenField runat="server" ID="hidOperatingCenterHasWorkOrderInvoicing" Value='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>' />
                        <wo:WorkOrderScheduleOfValuesForm runat="server" ID="woScheduleOfValues" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="Edit"/>
                    </div>
                </asp:PlaceHolder>

                <!-- JOB SITE CHECK LISTS -->
                <div id="jobsitechecklist">
                    <wo:JobSiteCheckListForm runat="server" ID="worJobSiteCheckListForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="true" />
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

        Date Completed:
        <asp:TextBox runat="server" ID="txtDateCompleted" Text='<%# Bind("DateCompleted") %>' autocomplete="off" />
        <atk:CalendarExtender runat="server" ID="dpDateCompleted" TargetControlID="txtDateCompleted" PopupPosition="TopLeft" /> 
        <asp:CompareValidator runat="server" ID="cvDateCompleted" ErrorMessage="Please enter a validate date"
            ControlToValidate="txtDateCompleted" Type="Date" Operator="DataTypeCheck" />
        <atk:MaskedEditExtender runat="server" ID="meeDateCompleted" TargetControlID="txtDateCompleted"
            Mask="99/99/9999" MaskType="Date" /><br />

        Flushing Notice Type:
        <mmsinc:MvpDropDownList runat="server" id="ddlFlushingNoticeType" DataSourceId="odsFlushingNoticeTypes" DataTextField="Description" DataValueField="Id" SelectedValue='<%# Bind("FlushingNoticeTypeId") %>' AppendDataBoundItems="true">
            <asp:ListItem Text="--Select Here--" Value="" />
        </mmsinc:MvpDropDownList>
        <asp:ObjectDataSource runat="server" ID="odsFlushingNoticeTypes" TypeName="WorkOrders.Model.FlushingNoticeTypeRepository"
            SelectMethod="SelectAllSorted" /><br />

        Digital As-Built Completed:
        <mmsinc:MvpDropDownList runat="server" id="ddlDigitalAsBuiltCompleted" SelectedValue='<%# Bind("DigitalAsBuiltCompleted") %>'>
            <asp:ListItem Text="--Select Here--" Value="" />
            <asp:ListItem Text="Yes" Value="True" />
            <asp:ListItem Text="No" Value="False" />
        </mmsinc:MvpDropDownList>
        
        <%-- THESE NEED TO BE HERE SO THAT THESE VALUES WILL PERSIST --%>
        <mmsinc:MvpHiddenField runat="server" ID="hidTrafficControlRequired" Value='<%# Bind("TrafficControlRequired") %>' />
        <mmsinc:MvpHiddenField runat="server" ID="hidStreetOpeningPermitRequired" Value='<%# Bind("StreetOpeningPermitRequired") %>' />
        <mmsinc:MvpHiddenField runat="server" ID="hidDigitalAsBuiltRequired" Value='<%# Bind("DigitalAsBuiltRequired") %>' />
    </EditItemTemplate>
    <ItemTemplate>
        <div id="divMain">
            <div style="font-weight: bold; font-size: larger">
                Order Number: <asp:Label runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' /><br />
            </div><br />
            <div id="divContent" class="tabsContainer">
                <ul>
                    <li><a href="#initial" class="tab"><span>Initial Information</span></a></li>
                    <li><a href="#materialsUsed" id="materialsUsedTab"><span>Materials</span></a></li>
                    <li><a href="#restoration" id="restorationTab"><span>Restoration</span></a></li>
                    <li><a href="#additional" id="additionalTab"><span>Additional</span></a></li>
                    <asp:PlaceHolder runat="server" ID="phSewerOverflows" Visible='<%# Eval("SewerOverflowsVisible") %>'>
                        <li><a href="#sewerOverflows" class="tab"><span>Sewer Overflows</span></a></li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phWorkOrderMainBreakForm" Visible='<%# Eval("WorkDescription.IsMainReplaceOrRepair") %>'>
                        <li><a href="#mainbreak" id="a1"><span>Main Break</span></a></li>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phScheduleOfValues" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing") %>'>
                        <li><a href="#scheduleOfValues" id="scheduleOfValuesTab"><span>Sched of Values</span></a></li>
                    </asp:PlaceHolder>
                    <li><a href="#document" class="tab"><span>Documents</span></a></li>
                     <asp:PlaceHolder runat="server" id="phNewServiceInstallation" Visible='<%# Eval("IsNewServiceInstallation") %>'>
                        <li><a href="#newServiceInstallations" id="newServiceInstallationsTab" class="tab"><span>Set Meter</span></a></li>
                    </asp:PlaceHolder>
                </ul>

                <!-- INITIAL INFORMATION -->
                <div id="initial">
                    <wo:WorkOrderInitialInputForm runat="server" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- MATERIALS USED -->
                <div id="materialsUsed">
                    <wo:WorkOrderMaterialsUsedForm runat="server" ID="womufMaterialsUsed" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- RESTORATION -->
                <div id="restoration">
                    <wo:WorkOrderRestorationForm runat="server" ID="wofrRestorations" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- ADDITIONAL -->
                <div id="additional">
                    <wo:WorkOrderAdditionalFinalizationInfoForm runat="server" ID="woafiAdditionalInfo" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- SEWER OVERFLOWS -->
                <div id="sewerOverflows">
                    <wo:WorkOrderSewerOverflowForm runat="server" InitialMode="ReadOnly" WorkOrderID='<%# Eval("WorkOrderID") %>' Visible='<%# Eval("SewerOverflowsVisible") %>' />
                </div>
                
                <!-- MAIN BREAK -->
                <asp:PlaceHolder runat="server" ID="pnlMainBreak" Visible='<%# Eval("WorkDescription.IsMainReplaceOrRepair") %>'>
                    <div id="mainbreak">
                        <wo:WorkOrderMainBreakForm runat="server" id="WorkOrderMainBreakForm" WorkOrderID='<%# Eval("WorkOrderID") %>' WorkDescriptionID='<%#Eval("WorkDescriptionID")%>' InitialMode="ReadOnly" />
                    </div>
                </asp:PlaceHolder>
                
                <!-- SCHEDULE OF VALUE LABOR ITEMS -->
                <asp:PlaceHolder runat="server" ID="pnlScheduleOfValues" Visible='<%# Eval("OperatingCenter.HasWorkOrderInvoicing")%>' >
                    <div id="scheduleOfValues">
                        <wo:WorkOrderScheduleOfValuesForm runat="server" ID="woScheduleOfValues" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly"/>
                    </div>
                </asp:PlaceHolder>
                
                <!-- NEW SERVICE INSTALLATION -->
                <asp:PlaceHolder runat="server" ID="pnlNewServiceInstallations" Visible='<%# Eval("IsNewServiceInstallation") %>'>
                    <div id="newServiceInstallations">
                        <wo:WorkOrderNewServiceInstallation runat="server" ID="woWorkOrderNewServiceInstallationForm" WorkOrderID='<%#Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="True"/>
                    </div>
                </asp:PlaceHolder>

                <!-- DOCUMENT --> 
                <div id="document">
                    <wo:WorkOrderDocumentForm runat="server" ID="woDocumentForm" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>
            </div>
        </div>
    </ItemTemplate>
</mmsinc:MvpFormView>

<mmsinc:ClientIDRepository runat="server" />

<asp:Label ID="lblMainBreakError" runat="server" CssClass="error"></asp:Label>

<div class="container">
    <mmsinc:MvpLinkButton runat="server" id="lnkEdit" Text="Edit" CssClass="link-button"/>
    <wo:MyButton runat="server" ID="btnEdit" Text="Edit" OnClick="btnEdit_Click" CssClass="DisplayNone" Visible="False" />
    <wo:MyButton runat="server" ID="btnRefresh" Text="Refresh" OnClick="btnEdit_Click"  CssClass="DisplayNone" />
    <wo:MyButton runat="server" ID="btnSave" Text="Finalize" OnClick="btnSave_Click" OnClientClick="return WorkOrderFinalizationDetailView.btnSave_Click();" />
    <!--Add link service button here, should appear in WorkOrders-->
    <mmsinc:MvpHyperLink ID="lnkCreateService" runat="server" Text="Create Service" Visible="false" CssClass="linkButton" style="padding:6px 12px 0px 12px; min-height: 18px; top: .25px;" />
    <input type="button" id="btnDummySaveFinalization" value="Saving..." style="display:none;" disabled="disabled" />
    <wo:MyButton runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" CssClass="DisplayNone"/>
    <br/><div style="margin-top: 6px;">Back to <asp:HyperLink runat="server" Text="Crew Assignments" NavigateUrl="/modules/mvc/FieldOperations/CrewAssignment/ShowCalendar" /></div> 
</div>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrder"
    OnInserted="ods_Inserted" OnUpdated="ods_Updated" OnDeleted="ods_Deleted" />

<mmsinc:CssInclude runat="server" CssFileName="jquery.dialog.css" />
<mmsinc:ScriptInclude runat="server" ScriptFileName="jquery-idleTimeout.js" />