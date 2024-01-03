<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RestorationDetailView.ascx.cs" Inherits="LINQTo271.Views.Restorations.RestorationDetailView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register Assembly="LINQTo271" Namespace="LINQTo271.Common" TagPrefix="wo" %>


<wo:WorkOrdersFormView runat="server" ID="fvRestoration" DataSourceID="odsRestoration"
    DataKeyNames="RestorationID" OnItemInserting="fvRestoration_ItemInserting" OnItemUpdating="fvRestoration_ItemUpdating">
    <ItemTemplate>
        <table>
            <tr>
                <th>Work Order Number:</th>
                <td colspan="3"><mmsinc:MvpLabel runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' /></td>
            </tr> 
            <tr>
                <th>Type of Restoration:</th>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblRestorationType" Text='<%# Eval("RestorationType") %>' Style="width: 100%" CssClass='lblRestorationType' />
                </td>
            </tr>
            <tr id="trPavingSquareFootage">
                <th colspan="2">Estimated Restoration Footage:</th>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblPavingSquareFootage" Text='<%# Eval("PavingSquareFootage") %>' />
                </td>
            </tr>
            <tr id="trLinearFtOfCurb" style="display: none;">
                <th colspan="2">Estimated Restoration Footage:</th>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblLinearFeetOfCurb" Text='<%# Eval("LinearFeetOfCurb") %>' />
                </td>
            </tr>
            <tr>
                <th>8" Stab Base By Company Forces:</th>
                <td>
                    <asp:CheckBox runat="server" ID="chkEightInchStabilizeBaseByCompanyForces" Enabled="false"
                        Checked='<%# Eval("EightInchStabilizeBaseByCompanyForces") %>' />
                </td>
                <th>Saw Cut By Company Forces:</th>
                <td>
                    <asp:CheckBox runat="server" ID="chkSawCutByCompanyForces" Enabled="false" Checked='<%# Eval("SawCutByCompanyForces") %>' />
                </td>
            </tr>
            <tr>
                <th>Total Accrued Cost</th>
                <td>
                    <asp:Label runat="server" ID="lblTotalAccruedCost" Text='<%#DataBinder.Eval(Container.DataItem, "TotalAccruedCost", "{0:c}") %>' />
                </td>
            </tr>
            <tr>
                <th>Restoration Notes:</th>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblRestorationNotes" Text='<%# Eval("RestorationNotes") %>'
                        Style="width: 100%;white-space: nowrap" />
                </td>
            </tr>
            
            <tr>
                <th colspan="4" style="font-size: larger;text-align: center">--Base/Temporary Restoration--</th>
            </tr>
            <tr>
                <th>Restoration Method:</th>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblPartialRestorationMethod" Style="width: 100%" Text='<%# Eval("PartialRestorationMethod") %>' />
                </td>
            </tr>
            <tr>
                <th>Restoration Invoice Number:</th>
                <td>
                    <asp:Label runat="server" ID="lblPartialRestorationInvoiceNumber" Text='<%# Eval("PartialRestorationInvoiceNumber") %>' />
                </td>
                <th>Restoration Completed (S.F./L.F):</th>
                <td>
                    <asp:Label runat="server" ID="lblPartialPavingSquareFootage" Text='<%# Eval("PartialPavingSquareFootage") %>' />
                </td>
            </tr>
            <tr>
                <th>Restoration Date:</th>
                <td>
                    <asp:Label runat="server" ID="lblPartialRestorationDate" Text='<%# Eval("PartialRestorationDate") %>' />
                </td>
                <th>Paving Break Out 8" S.F.:</th>
                <td>
                    <asp:Label runat="server" ID="lblPartialPavingBreakOutEightInches" Text='<%# Eval("PartialPavingBreakOutEightInches") %>' />
                </td>
            </tr>
            <tr>
                <th>Restoration By:</th>
                <td>
                    <asp:Label runat="server" ID="txtPartialRestorationCompletedBy" Text='<%# Eval("PartialRestorationCompletedBy") %>' />
                </td>
                <th>Paving Break Out 10" S.F.:</th>
                <td>
                    <asp:Label runat="server" ID="lblPartialPavingBreakOutTenInches" Text='<%# Eval("PartialPavingBreakOutTenInches") %>' />
                </td>
            </tr>
            <tr>
                <th>Traffic Control Cost:</th>
                <td>
                    <asp:Label runat="server" ID="lblTrafficControlCostPartialRestoration" Text='<%# Eval("TrafficControlCostPartialRestoration") %>' />
                </td>
                <th>Saw Cutting L.F.:</th>
                <td>
                    <asp:Label runat="server" ID="lblPartialSawCutting" Text='<%# Eval("PartialSawCutting") %>' />
                </td>
            </tr>
            <tr>
                <th>Invoiced Amount:</th>
                <td>
                    <asp:Label runat="server" ID="lblTotalInitialActualCost" Text='<%#DataBinder.Eval(Container.DataItem, "TotalInitialActualCost", "{0:c}") %>' />                    
                </td>
                <th></th>
                <td></td>
            </tr>
            <tr>
                <th colspan="4" style="font-size: larger;text-align: center">--Final Restoration--</th>
            </tr>
            <tr>
                <th>Restoration Method:</th>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblFinalRestorationMethod" style="width: 100%" Text='<%# Eval("FinalRestorationMethod") %>' />
                </td>
            </tr>
            <tr>
                <th>Restoration Invoice Number:</th>
                <td>
                    <asp:Label runat="server" ID="lblFinalRestorationInvoiceNumber" Text='<%# Eval("FinalRestorationInvoiceNumber") %>' />
                </td>
                <th>Restoration Completed (S.F./L.F):</th>
                <td>
                    <asp:Label runat="server" ID="lblFinalPavingSquareFootage" Text='<%# Eval("FinalPavingSquareFootage") %>' />
                </td>
            </tr>
            <tr>
                <th>Restoration Date:</th>
                <td>
                    <asp:Label runat="server" ID="lblFinalRestorationDate" Text='<%# Eval("FinalRestorationDate") %>' />
                </td>
                <th>
                    Saw Cutting L.F.:
                </th>
                <td>
                    <asp:Label runat="server" ID="lblFinalSawCutting" Text='<%# Eval("FinalSawCutting") %>' />
                </td>
            </tr>
            <tr>
                <th>Restoration By:</th>
                <td>
                    <asp:Label runat="server" ID="lblFinalRestorationCompletedBy" Text='<%# Eval("FinalRestorationCompletedBy") %>' />
                </td>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <th>Traffic Control Cost:</th>
                <td>
                    <asp:Label runat="server" ID="lblTrafficControlCostFinalRestoration" Text='<%# Eval("TrafficControlCostFinalRestoration") %>' />
                </td>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <th>
                    Final Restoration Actual Cost:
                </th>
                <td>
                    <asp:Label runat="server" ID="lblFinalRestorationActualCost" Text='<%#DataBinder.Eval(Container.DataItem, "FinalRestorationActualCost", "{0:c}") %>' />
                </td>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr>
                <th>Date Approved:</th>
                <td>
                    <asp:Label runat="server" ID="lblDateApproved" Text='<%# Eval("DateApproved") %>' />
                </td>
                <th>Date Rejected:</th>
                <td>
                    <asp:Label runat="server" ID="lblDateRejected" Text='<%# Eval("DateRejected") %>' />
                </td>
            </tr>
        </table>
    </ItemTemplate>
    <EditItemTemplate>
        <table>
            <tr>
                <th>Work Order Number:</th>
                <td colspan="3"><mmsinc:MvpLabel runat="server" ID="lblWorkOrderID" Text='<%# Eval("WorkOrderID") %>' /></td>
            </tr>
            <tr>
                <th>Type of Restoration:</th>
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddlRestorationType" DataSourceID="odsRestorationType"
                        DataTextField="Description" DataValueField="RestorationTypeID" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("RestorationTypeID") %>' Style="width: 100%" onchange="RestorationDetailView.ddlRestorationType_Change(this)" CssClass="ddlRestorationType">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trPavingSquareFootage">
                <th colspan="2">Estimated Restoration Footage:</th>
                <td colspan="2">
                    <mmsinc:MvpTextBox runat="server" ID="txtPavingSquareFootage" Text='<%# Bind("PavingSquareFootage") %>' />
                </td>
            </tr>
            <tr id="trLinearFtOfCurb" style="display: none;">
                <th colspan="2">Estimated Restoration Footage:</th>
                <td colspan="2">
                    <mmsinc:MvpTextBox runat="server" ID="txtLinearFeetOfCurb" Text='<%# Bind("LinearFeetOfCurb") %>' />
                </td>
            </tr>
            <tr>
                <th>8" Stab Base By Company Forces:</th>
                <td>
                    <mmsinc:MvpCheckBox runat="server" ID="chkEightInchStabilizeBaseByCompanyForces" Checked='<%# Bind("EightInchStabilizeBaseByCompanyForces") %>' />
                </td>
                <th>Saw Cut By Company Forces:</th>
                <td>
                    <asp:CheckBox runat="server" ID="chkSawCutByCompanyForces" Checked='<%# Bind("SawCutByCompanyForces") %>' />
                </td>
            </tr>
            <tr>
                <th>Total Accrued Cost</th>
                <td>
                    <asp:Label runat="server" ID="lblTotalAccruedCost" Text='<%#DataBinder.Eval(Container.DataItem, "TotalAccruedCost", "{0:c}") %>' />
                </td>
            </tr>
            <tr>
                <th>Restoration Notes:</th>
                <td colspan="3">
                    <mmsinc:MvpTextBox runat="server" ID="txtRestorationNotes" TextMode="MultiLine" Text='<%# Bind("RestorationNotes") %>'
                        Style="width: 100%" />
                </td>
            </tr>
            <tr>
                <th colspan="4" style="font-size: larger;text-align: center">--Base/Temporary Restoration--</th>
            </tr>
            <tr>
                <th>Restoration Method:</th>
                <td colspan="3">
                    <mmsinc:MvpDropDownList runat="server" ID="ddlPartialRestorationMethod" style="width: 100%" ConvertEmptyStringToNull="" />
                    <atk:CascadingDropDown runat="server" ID="cddPartialRestorationMethod" TargetControlID="ddlPartialRestorationMethod"
                        ParentControlID="ddlRestorationType" Category="PartialRestorationMethod" PromptText="--Select Here--"
                        PromptValue="" LoadingText="[Loading Restoration Methods...]" ServicePath="~/Views/RestorationMethods/RestorationMethodsServiceView.asmx"
                        ServiceMethod="GetInitialRestorationMethodsByRestorationType" SelectedValue='<%# Bind("PartialRestorationMethodID") %>' />
                    <asp:CustomValidator ID="cvTotalInitialActualCost" runat="server" OnServerValidate="CheckInitialRestorationDateFields"
                        CssClass="required"
                        ErrorMessage="Initial Restoration Invoice Number is required." Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <th>Restoration Invoice Number:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtPartialRestorationInvoiceNumber" Text='<%# Bind("PartialRestorationInvoiceNumber") %>' />
                </td>
                <th>Restoration Completed (S.F./L.F.):</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtPartialPavingSquareFootage" Text='<%# Bind("PartialPavingSquareFootage") %>' />
                    <atk:maskededitextender runat="server" targetcontrolid="txtPartialPavingSquareFootage"
                        id="meePartialPavingSquareFootage" mask="9999" clearmaskonlostfocus="true" />
                </td>
            </tr>
            <tr>
                <th>Restoration Date:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="ccPartialRestorationDate" Text='<%# Bind("PartialRestorationDate") %>' autocomplete="off" />
                    <atk:CalendarExtender runat="server" ID="cePartialRestorationDate" TargetControlID="ccPartialRestorationDate" />
                </td>
                <th>Paving Break Out 8" S.F.:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtPartialPavingBreakOutEightInches" Text='<%# Bind("PartialPavingBreakOutEightInches") %>' />
                    <atk:maskededitextender runat="server" targetcontrolid="txtPartialPavingBreakOutEightInches"
                        id="meePartialPavingBreakOutEightInches" mask="9999" clearmaskonlostfocus="true" />
                </td>
            </tr>
            <tr>
                <th>Restoration By:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtPartialRestorationCompletedBy" Text='<%# Bind("PartialRestorationCompletedBy") %>' />
                </td>
                <th>Paving Break Out 10" S.F.:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtPartialPavingBreakOutTenInches" Text='<%# Bind("PartialPavingBreakOutTenInches") %>' />
                    <atk:maskededitextender runat="server" targetcontrolid="txtPartialPavingBreakOutTenInches"
                        id="meePartialPavingBreakOutTenInches" mask="9999" clearmaskonlostfocus="true" />
                </td>
            </tr>
            <tr>
                <th>Traffic Control Cost:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtTrafficControlCostPartialRestoration" Text='<%# Bind("TrafficControlCostPartialRestoration") %>' />
                    <atk:maskededitextender runat="server" targetcontrolid="txtTrafficControlCostPartialRestoration"
                        id="meeTrafficControlCostPartialRestoration" mask="9999" clearmaskonlostfocus="true" />
                </td>
                <th>Saw Cutting L.F.:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtPartialSawCutting" Text='<%# Bind("PartialSawCutting") %>' />
                    <atk:maskededitextender runat="server" targetcontrolid="txtPartialSawCutting" id="meetxtPartialSawCutting"
                        mask="9999" clearmaskonlostfocus="true" />
                </td>
            </tr>
            <tr>
                <th>Invoiced Amount:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtTotalInitalActualCost" Text='<%#Bind("TotalInitialActualCost","{0:#######0.00}") %>' />
                    <atk:maskededitextender runat="server" targetcontrolid="txtTotalInitalActualCost" id="meeTotalInitialActualCost"
                        mask="9999999.99" MaskType="Number" AutoComplete="false" DisplayMoney="Left"  clearmaskonlostfocus="true" Enabled="true"/>
                </td>
            </tr>
            <tr>
                <th colspan="4" style="font-size: larger;text-align: center">--Final Restoration--</th>
            </tr>
            <tr>
                <th>Restoration Method:</th>
                <td colspan="3">
                    <mmsinc:MvpDropDownList runat="server" ID="ddlFinalRestorationMethod" style="width: 100%" />
                    <atk:CascadingDropDown runat="server" ID="cddFinalRestorationMethod" TargetControlID="ddlFinalRestorationMethod"
                        ParentControlID="ddlRestorationType" Category="FinalRestorationMethod" PromptText="--Select Here--"
                        PromptValue="" LoadingText="[Loading Restoration Methods...]" ServicePath="~/Views/RestorationMethods/RestorationMethodsServiceView.asmx"
                        ServiceMethod="GetFinalRestorationMethodsByRestorationType" SelectedValue='<%# Bind("FinalRestorationMethodID") %>' />
                </td>
            </tr>
            <tr>
                <th>Restoration Invoice Number:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtFinalRestorationInvoiceNumber" Text='<%# Bind("FinalRestorationInvoiceNumber") %>' />
                </td>
                <th>Restoration Completed (S.F./L.F.):</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtFinalPavingSquareFootage" Text='<%# Bind("FinalPavingSquareFootage") %>' />
                    <atk:maskededitextender runat="server" targetcontrolid="txtFinalPavingSquareFootage"
                        id="meeFinalPavingSquareFootage" mask="9999" clearmaskonlostfocus="true" />
                </td>
            </tr>
            <tr>
                <th>Restoration Date:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="ccFinalRestorationDate" Text='<%# Bind("FinalRestorationDate") %>' autocomplete="off" />
                    <atk:CalendarExtender runat="server" ID="ceFinalRestorationDate" TargetControlID="ccFinalRestorationDate" />
                    <asp:CustomValidator ID="cvFinalRestorationDate" runat="server" OnServerValidate="CheckFinalRestorationDateFields" 
                        CssClass="required"
                        ErrorMessage="Final Restoration Actual Cost, Final Restoration Invoice Number and Final Restoration By are required." 
                        Display="Dynamic"></asp:CustomValidator>
                </td>
                <th>Saw Cutting L.F.:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtFinalSawCutting" Text='<%# Bind("FinalSawCutting") %>' />
                    <atk:MaskedEditExtender runat="server" TargetControlID="txtFinalSawCutting" ID="meeFinalSawCutting"
                        Mask="9999" ClearMaskOnLostFocus="true" />
                </td>
            </tr>
            <tr>
                <th>Restoration By:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtFinalRestorationCompletedBy" Text='<%# Bind("FinalRestorationCompletedBy") %>' />
                </td>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <th>Traffic Control Cost:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtTrafficControlCostFinalRestoration" Text='<%# Bind("TrafficControlCostFinalRestoration") %>' />
                    <atk:maskededitextender runat="server" targetcontrolid="txtTrafficControlCostFinalRestoration"
                        id="meeTrafficControlCostFinalRestoration" mask="9999" clearmaskonlostfocus="true" />
                </td>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <th>Final Restoration Actual Cost:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="txtFinalRestorationActualCost" Text='<%#Bind("FinalRestorationActualCost","{0:#######0.00}") %>' />
                    <atk:MaskedEditExtender runat="server" TargetControlID="txtFinalRestorationActualCost"
                        ID="meeFinalRestorationActualCost" Mask="9999999.99" MaskType="Number" AutoComplete="false"
                        DisplayMoney="Left" ClearMaskOnLostFocus="true" Enabled="true" />
                </td>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr>
                <th>Date Approved:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="ccDateApproved" Text='<%# Bind("DateApproved") %>' autocomplete="off" />
                    <atk:CalendarExtender runat="server" ID="ceDateApproved" TargetControlID="ccDateApproved" />
                </td>
                <th>Date Rejected:</th>
                <td>
                    <mmsinc:MvpTextBox runat="server" ID="ccDateRejected" Text='<%# Bind("DateRejected") %>' autocomplete="off" />
                    <atk:CalendarExtender runat="server" ID="ceDateRejected" TargetControlID="ccDateRejected" />
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</wo:WorkOrdersFormView>

<asp:Button runat="server" ID="btnEdit" Text="Edit" OnClick="btnEdit_Click" />
<asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" OnClientClick="return RestorationDetailView.btnSave_Click()" />
<asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click" />
<input type="button" id="btnDone" value="Done" onclick="RestorationDetailView.btnDone_Click()" />

<mmsinc:ClientIDRepository runat="server" />

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsRestoration" DataObjectTypeName="WorkOrders.Model.Restoration"
    OnInserted="ods_Inserted" OnUpdated="ods_Updated" OnDeleted="ods_Deleted" />

<asp:ObjectDataSource runat="server" ID="odsRestorationType" SelectMethod="SelectAllAsList"
    TypeName="WorkOrders.Model.RestorationTypeRepository" />
