<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderRestorationProcessingDetailView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.RestorationProcessing.WorkOrderRestorationProcessingDetailView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInitialInputForm" Src="~/Controls/WorkOrders/WorkOrderInputFormView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderRestorationForm" Src="~/Controls/WorkOrders/WorkOrderRestorationForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderDocumentForm" Src="~/Controls/WorkOrders/WorkOrderDocumentsForm.ascx" %>

<%-- TODO:  If this belongs, uncomment it.  Otherwise, remove it.
<asp:Label runat="server" ID="lblCount" />
--%>
<mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID">
    <EditItemTemplate>
        <div id="divMain">
            <div style="font-weight: bold; font-size: larger">
                Order Number: <asp:Label runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' /><br />
                Account Charged: <asp:Label runat="server" ID="lblAccountCharged" Text='<%# Eval("AccountCharged") %>' />
            </div><br />
            <div id="divContent" class="tabsContainer">
                <ul>
                    <li><a href="#initial" class="tab"><span>Initial Information</span></a></li>
                    <li><a href="#restoration" id="restorationTab"><span>Restoration</span></a></li>
                    <li><a href="#document" class="tab"><span>Documents</span></a></li>
                </ul>

                <!-- INITIAL INFORMATION -->
                <div id="initial">
                    <wo:WorkOrderInitialInputForm runat="server" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <!-- RESTORATION -->
                <div id="restoration">
                    <wo:WorkOrderRestorationForm runat="server" ID="wofrRestorations" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                </div>

                <!-- DOCUMENTS -->
                <div id="document">
                    <mmsinc:MvpUpdatePanel runat="server" ID="upDocuments" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <wo:WorkOrderDocumentForm runat="server" ID="woDocumentForm" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" />
                        </ContentTemplate>
                    </mmsinc:MvpUpdatePanel>
                </div>

                <!-- Hidden/DisplayNones -->
                <asp:Label runat="server" ID="lblDateCompleted" Text="Date Completed:" style="display:none;" />
                <asp:TextBox runat="server" ID="txtDateCompleted" Text='<%# Bind("DateCompleted") %>' style="display:none;" autocomplete="off" />
                <atk:CalendarExtender runat="server" ID="dpDateCompleted" TargetControlID="txtDateCompleted" ></atk:CalendarExtender> 
                <asp:CompareValidator runat="server" ID="cvDateCompleted" ErrorMessage="Please enter a validate date"
                    ControlToValidate="txtDateCompleted" Type="Date" Operator="DataTypeCheck" />
                <atk:MaskedEditExtender runat="server" ID="meeDateCompleted" TargetControlID="txtDateCompleted"
                    Mask="99/99/9999" MaskType="Date" />
            </div>
        </div>
    </EditItemTemplate>
</mmsinc:MvpFormView>

<mmsinc:ClientIDRepository runat="server" />

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrder"
    OnInserted="ods_Inserted" OnUpdated="ods_Updated" OnDeleted="ods_Deleted" />
