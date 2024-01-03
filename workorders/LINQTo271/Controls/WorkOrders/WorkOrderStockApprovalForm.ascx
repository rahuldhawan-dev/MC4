<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderStockApprovalForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderStockApprovalForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID"
    OnItemUpdating="fvWorkOrder_ItemUpdating">
    <ItemTemplate>
        <table>
            <tbody> 
                <tr>
                    <td>
                        Doc ID:
                    </td>
                    <td>
                        <mmsinc:MvpLabel runat="server" ID="lblDocID" Text='<%# Eval("MaterialsDocID") %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        Work Order Approved By: 
                    </td>
                    <td>
                        <mmsinc:MvpLabel runat="server" ID="lblApprovedBy" Text='<%# Eval("ApprovedBy") %>' />
                    </td>
                </tr>
                <tr>
                    <td>Material Posting Date:</td>
                    <td><mmsinc:MvpLabel runat="server" Id="lblMaterialPostingDate" Text='<%#Eval("MaterialPostingDate") %>'/></td>
                </tr>
                <tr>
                    <td>
                        Stock Approved By: 
                    </td>
                    <td>
                        <mmsinc:MvpLabel runat="server" ID="lblMaterialsApprovedBy" Text='<%# Eval("MaterialsApprovedBy") %>' />
                    </td>
                </tr>
            </tbody>
        </table>  
    </ItemTemplate>
    <EditItemTemplate>
        <table>
            <tbody> 
                <tr>
                    <td>
                        Doc ID:
                    </td>
                    <td>
                        <%-- Sonar doesn't understand the `style` property being set from a ternary operation 
                             so it triggers a false-positive there. --%>
                        <mmsinc:MvpTextBox runat="server" ID="txtDocID" Text='<%# Bind("MaterialsDocID") %>' 
                            ReadOnly='<%#(bool)Eval("IsSapUpdatableWorkOrder") %>'
                            style='<%#!(bool)Eval("IsSapUpdatableWorkOrder") ? "display:;" : "display: none;"%>' />
                        <atk:MaskedEditExtender runat="server" ID="meeDocID" TargetControlID="txtDocID" Mask="999999999999999" />

                        <mmsinc:MvpLabel runat="server" ID="lblDocID" Text='<%# Eval("MaterialsDocID") %>'
                            Visible='<%#(bool)Eval("IsSapUpdatableWorkOrder") %>'/>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        Work Order Approved By: 
                    </td>
                    <td>
                        <mmsinc:MvpLabel runat="server" ID="lblApprovedBy" Text='<%# Eval("ApprovedBy") %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        Stock Approved By: 
                    </td>
                    <td>
                        <mmsinc:MvpLabel runat="server" ID="lblMaterialsApprovedBy" Text='<%# Eval("MaterialsApprovedBy") %>' />
                    </td>
                </tr>
                <tr>
                    <td>Material Posting Date:</td>
                    <td>
                        <mmsinc:MvpTextBox runat="server" Id="txtMaterialPostingDate" Text='<%# Bind("MaterialPostingDate", "{0:d}") %>'
                            Enabled='<%#!((DateTime?)Eval("MaterialsApprovedOn")).HasValue %>' autocomplete="off" />
                        <atk:CalendarExtender runat="server" ID="ceMaterialPostingDate" TargetControlID="txtMaterialPostingDate" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <mmsinc:MvpButton runat="server" ID="btnSave" Text="Approve" OnClick="btnSave_Click" 
                            Enabled='<%#(!((DateTime?)Eval("MaterialsApprovedOn")).HasValue && (Eval("SAPErrorCode") == null || !Eval("SAPErrorCode").ToString().ToUpper().StartsWith("RETRY")))%>'
                            OnClientClick="return WorkOrderStockApprovalForm.validateMaterialsApproval()" />
                    </td>
                </tr>
            </tbody>
        </table>  
    </EditItemTemplate>
</mmsinc:MvpFormView>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrder"
    OnUpdated="ods_Updated" />
