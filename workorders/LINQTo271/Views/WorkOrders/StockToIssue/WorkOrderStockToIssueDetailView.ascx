<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderStockToIssueDetailView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.StockToIssue.WorkOrderStockToIssueDetailView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInputFormView" Src="~/Controls/WorkOrders/WorkOrderInputFormView.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderMaterialsUsedForm" Src="~/Controls/WorkOrders/WorkOrderMaterialsUsedForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderStockApprovalForm" Src="~/Controls/WorkOrders/WorkOrderStockApprovalForm.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderAdditionalFinalizationInfoForm" Src="~/Controls/WorkOrders/WorkOrderAdditionalFinalizationInfoForm.ascx" %>



<mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID">
    <EditItemTemplate>
        <div id="divMain">
            <div style="font-weight: bold; font-size: larger">
                Order Number: <asp:Label runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' /><br />
                Account Number: <asp:Label runat="server" ID="lblAccountCharged" Text='<%# Eval("AccountCharged") %>' /><br />
                Accounting Type: <asp:Label runat="server" ID="lblAccountingType" Text='<%# Eval("WorkDescription.AccountingType") %>' />
            </div><br />
            <div id="divContent" class="tabsContainer">
                <ul>
                    <li><a href="#initial" class="tab"><span>Initial Information</span></a></li>
                    <li><a href="#materialsUsed" id="materialsUsedTab"><span>Materials</span></a></li>
                    <li><a href="#additional" id="additionalTab"><span>Additional</span></a></li>
                </ul>

                <!-- INITIAL INFORMATION -->
                <div id="initial">
                    <wo:WorkOrderInputFormView runat="server" ID="woifvInitial" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>

                <div id="materialsUsed">
                    <wo:WorkOrderMaterialsUsedForm runat="server" ID="womufMaterialsUsed" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                    <br />
                    <wo:WorkOrderStockApprovalForm runat="server" ID="woStockApproval" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="Edit" OnUpdating="ods_Updated" />
                </div>
                <div id="additional">
                    <wo:WorkOrderAdditionalFinalizationInfoForm runat="server" ID="woafiAdditionalInfo" WorkOrderID='<%# Eval("WorkOrderID") %>' InitialMode="ReadOnly" />
                </div>
            </div>
        </div>
    </EditItemTemplate>
</mmsinc:MvpFormView>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrder"
    OnInserted="ods_Inserted" OnUpdated="ods_Updated" OnDeleted="ods_Deleted" />

