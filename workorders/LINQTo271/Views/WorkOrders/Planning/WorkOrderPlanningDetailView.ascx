<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderPlanningDetailView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Planning.WorkOrderPlanningDetailView" %>
<%@ Register Assembly="LINQTo271" Namespace="LINQTo271.Common" TagPrefix="wo" %>
<%@ Register TagPrefix="mmsinc" TagName="ClientIDRepository" Src="~/Common/ClientIDRepository.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInputFormView" Src="~/Controls/WorkOrders/WorkOrderInputFormView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMapForm" Src="~/Controls/WorkOrders/WorkOrderMapForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMarkoutForm" Src="~/Controls/WorkOrders/WorkOrderMarkoutForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderTrafficControlForm" Src="~/Controls/WorkOrders/WorkOrderTrafficControlForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMainBreakForm" Src="~/Controls/WorkOrders/WorkOrderMainBreakForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderStreetOpeningPermitForm" Src="~/Controls/WorkOrders/WorkOrderStreetOpeningPermitForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderDocumentForm" Src="~/Controls/WorkOrders/WorkOrderDocumentsForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderCrewAssignmentForm" Src="~/Controls/WorkOrders/WorkOrderCrewAssignmentForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="TrafficControlForm" Src="~/Controls/WorkOrders/TrafficControlForm.ascx" %>

<wo:WorkOrdersFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID">
<EditItemTemplate>
    <div id="divMain">
        <div style="font-weight: bold; font-size: larger">
            Order Number: <mmsinc:MvpLabel runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' /><br />
        </div><br />
        <div id="divContent" class="tabsContainer">
            <ul>
                <li><a href="#initial" class="tab"><span>Initial Information</span></a></li>
                <li><a href="#map" id="mapTab"><span>Map</span></a></li>
                <li><a href="#markouts" class="tab" id="markoutsTab"><span>Markouts</span></a></li>
                <li><a href="#traffic-control" class="tab"><span>Traffic Control/Notes</span></a></li>
                <asp:PlaceHolder runat="server" ID="phWorkOrderMainBreakForm" Visible='<%# Eval("WorkDescription.IsMainReplaceOrRepair") %>'>
                    <li><a href="#mainbreak"><span>Main Break</span></a></li>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phSOPTab" Visible='<%# Eval("StreetOpeningPermitRequired") %>'>
                    <li><a href="#street-opening-permit"><span>Street Opening Permit</span></a></li>
                </asp:PlaceHolder>
                <li><a href="#crewAssignments" id="crewAssignmentsTab"><span>Crew Assignments</span></a></li>
                <li><a href="#document" class="tab"><span>Documents</span></a></li>
            </ul>

            <!-- INITIAL INFORMATION -->
            <div id="initial">
                <wo:WorkOrderInputFormView runat="server" ID="wofvInitialInformation" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" OnUpdating="ods_Updated" />

                <mmsinc:MvpButton runat="server" ID="btnEditInitialInfo" Text="Edit" OnClick="btnEditInitialInfo_Click" />
                <mmsinc:MvpButton runat="server" ID="btnCancelInitialInfo" Text="Cancel" OnClick="btnCancelInitialInfo_Click" Visible="false" />
            </div>

            <!-- MAP -->
            <div id="map">
                <wo:WorkOrderMapForm runat="server" InitialMode="Edit" WorkOrderID='<%# Eval("WorkOrderID") %>' />
            </div>
            
            <!-- MARKOUTS -->
            <div id="markouts">
                <wo:WorkOrderMarkoutForm runat="server" InitialMode="Edit" WorkOrderID='<%# Eval("WorkOrderID") %>' MarkoutRequirementID='<%# Eval("MarkoutRequirementID") %>' />
            </div>

            <!-- TRAFFIC CONTROL -->
            <div id="traffic-control">
                <wo:WorkOrderTrafficControlForm runat="server" ID="woTrafficControl" InitialMode="Edit"
                    WorkOrderID='<%# Eval("WorkOrderID") %>' OnUpdating="ods_Updated" />
                <wo:TrafficControlForm runat="server" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" Visible="true" />
            </div>
            
            <!-- MAIN BREAK -->
            <asp:PlaceHolder runat="server" ID="pnlMainBreak" Visible='<%# Eval("WorkDescription.IsMainReplaceOrRepair") %>'>
                <div id="mainbreak">
                    <wo:WorkOrderMainBreakForm runat="server" id="WorkOrderMainBreakForm" WorkOrderID='<%# Eval("WorkOrderID") %>' WorkDescriptionID='<%#Eval("WorkDescriptionID")%>' InitialMode="Edit" />
                </div>
            </asp:PlaceHolder>
                
            <!-- STREET OPENING PERMITS -->
            <asp:PlaceHolder runat="server" ID="pnlStreetOpeningPermit" Visible='<%# Eval("StreetOpeningPermitRequired") %>'>
                <div id="street-opening-permit">
                    <wo:WorkOrderStreetOpeningPermitForm runat="server" id="StreetOpeningPermitTab" InitialMode="Edit" WorkOrderID='<%# Eval("WorkOrderID") %>' />
                </div>
            </asp:PlaceHolder>

            <!-- CREW ASSIGNMENTS -->
            <div id="crewAssignments">
                <wo:WorkOrderCrewAssignmentForm runat="server" ID="woCrewAssignments" WorkOrderID='<%# Eval("WorkOrderID") %>'
                    InitialMode="ReadOnly" />
            </div>

            <!-- DOCUMENTS -->
            <div id="document">
                <mmsinc:MvpUpdatePanel runat="server" ID="upDocuments" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <wo:WorkOrderDocumentForm runat="server" ID="woDocumentForm" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                    </ContentTemplate>
                </mmsinc:MvpUpdatePanel>
            </div>
            
        </div>

</EditItemTemplate>
</wo:WorkOrdersFormView>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrder"
    OnUpdated="ods_Updated" OnDeleted="ods_Deleted"  />
<%-- IGNORE ANY ERROR MESSAGES ABOUT THE PREVIOUS LINE --%>

<mmsinc:ClientIDRepository runat="server" />
<mmsinc:CssInclude runat="server" CssFileName="jqModal.css" />
