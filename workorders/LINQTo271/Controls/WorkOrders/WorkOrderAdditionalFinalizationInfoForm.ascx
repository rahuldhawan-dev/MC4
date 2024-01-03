<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderAdditionalFinalizationInfoForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderAdditionalFinalizationInfoForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<style>
tr.pitcher-delivery-details, tr.pitcher-delivery-details-other {
    display: none;
}

/* these validation message spans were rendering as invisible, which took up screen space and messed with
 * alignment of things
 */
span.required[style *= 'visibility:hidden'] {
    display: none;
}
</style>

<div>
    <mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID"
        OnDataBound="fvWorkOrder_OnDataBound"
        OnItemUpdating="fvWorkOrder_ItemUpdating" EmptyDataText="No Data">
        <ItemTemplate>
           <table>
            <tr>
                <td>Total Man Hours:</td>
                <td>
                    <mmsinc:MvpLabel ID="MvpLabel1" runat="server" Text='<%# Eval("TotalManHours") %>' />
                </td>
            </tr>
            <tr>
                <td>Total Gallons Lost:</td>
                <td>
                    <mmsinc:MvpLabel ID="MvpLabel2" runat="server" Text='<%# Eval("LostWater") %>' />
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Leakage Chart.pdf" Text="Leakage Chart" Target="_blank" />
                </td>
            </tr>
            <tr>
                <td>Distance From Cross Street (Feet)</td>
                <td>
                    <mmsinc:MvpLabel ID="MvpLabel3" runat="server" Text='<%# Eval("DistanceFromCrossStreet") %>' />
                </td>
            </tr>
            <tr>
                <td>Notes (EST):</td>
                <td>
                    <mmsinc:MvpLabel ID="MvpLabel4" runat="server" Text='<%# (Eval("Notes")!=null) ? ((string)Eval("Notes")).Replace("\n", "<br />") : String.Empty %>' />
                </td>
            </tr>

            <asp:PlaceHolder ID="phUpdatedMobileGIS" runat="server" Visible='<%#((bool)Eval("WorkDescription.IsServiceLineRenewal") || (bool)Eval("WorkDescription.IsServiceLineRetire") || (bool)Eval("WorkDescription.RequiresPitcherFilter")) %>'>
                <tr>
                    <td colspan="2">
                        <asp:PlaceHolder runat="server" Visible='<%#((bool)Eval("WorkDescription.IsServiceLineRenewal") || (bool)Eval("WorkDescription.IsServiceLineRetire")) %>'>
                        <fieldset>
                            <legend>Service Line Info</legend>
                            <table>
                                <asp:PlaceHolder runat="server" Visible='<%#Eval("WorkDescription.IsServiceLineRenewal") %>'>
                                    <tr>
                                        <td>Previous Service Company Material</td>
                                        <td>
                                            <mmsinc:MvpLabel ID="lblPreviousServiceLineMaterial" runat="server" Text='<%#Eval("PreviousServiceLineMaterial") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td>Previous Service Company Size</td>
                                        <td>
                                            <mmsinc:MvpLabel ID="lblPreviousServiceLineSize" runat="server" Text='<%#Eval("PreviousServiceLineSize") %>' /></td>
                                    </tr>
                                </asp:PlaceHolder>
                                <tr>
                                    <td>Service Company Material</td>
                                    <td>
                                        <mmsinc:MvpLabel ID="lblCompanyServiceLineMaterial" runat="server" Text='<%#Eval("CompanyServiceLineMaterial") %>' /></td>
                                </tr>
                                <tr>
                                    <td>Service Company Size</td>
                                    <td>
                                        <mmsinc:MvpLabel ID="lblCompanyServiceLineSize" runat="server" Text='<%#Eval("CompanyServiceLineSize") %>' /></td>
                                </tr>
                                <asp:PlaceHolder runat="server" Visible='<%#Eval("WorkDescription.IsServiceLineRenewal") %>'>
                                    <tr>
                                        <td>Customer Service Line Material</td>
                                        <td>
                                            <mmsinc:MvpLabel ID="lblCustomerServiceLineMaterial" runat="server" Text='<%#Eval("CustomerServiceLineMaterial") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td>Customer Service Line Size</td>
                                        <td>
                                            <mmsinc:MvpLabel ID="lblCustomerServiceLineSize" runat="server" Text='<%#Eval("CustomerServiceLineSize") %>' /></td>
                                    </tr>
                                    <tr>
                                        <td>Door Notice Left</td>
                                        <td>
                                            <mmsinc:MvpLabel ID="lblDoorNoticeLeftDate" runat="server" Text='<%#Eval("DoorNoticeLeftDate","{0:d}") %>' /></td>
                                    </tr>
                                </asp:PlaceHolder>
                            </table>
                        </fieldset>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder
                            runat="server"
                            ID="phComplianceData"
                            Visible='<%# BindItem.WorkDescription.RequiresPitcherFilter %>'>
                            <fieldset>
                                <legend>Compliance Data</legend>
                                <table>
                                    <tr>
                                        <td>Initial Service Line Flush Time (Minutes)</td>
                                        <td>
                                            <mmsinc:MvpLabel
                                                runat="server"
                                                ID="lblInitialServiceLineFlushTime"
                                                Text='<%# BindItem.InitialServiceLineFlushTime %>' />
                                        </td>
                                    </tr>
                                    <asp:PlaceHolder
                                        runat="server"
                                        ID="phInitialFlushTimeEnteredBy"
                                        Visible='<%# Eval("InitialServiceLineFlushTime") != null %>'>
                                        <tr>
                                            <td>Initial Service Line Flush Time Entered By</td>
                                            <td>
                                                <mmsinc:MvpLabel
                                                    runat="server"
                                                    ID="lblInitialServiceLineFlushTimeEnteredBy"
                                                    Text='<%# BindItem.InitialFlushTimeEnteredBy %>'/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Initial Service Line Flush Time Entered At</td>
                                            <td>
                                                <mmsinc:MvpLabel
                                                    runat="server"
                                                    ID="lblInitialServiceLineFlushTimeEnteredAt"
                                                    Text='<%# BindItem.InitialFlushTimeEnteredAt %>'/>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                    <tr>
                                        <td>Pitcher Filter Provided to Customer</td>
                                        <td>
                                            <mmsinc:MvpLabel
                                                runat="server"
                                                ID="lblHasPitcherFilterBeenProvidedToCustomer"
                                                Text='<%# BindItem.HasPitcherFilterBeenProvidedToCustomer %>' />
                                        </td>
                                    </tr>
                                    <asp:PlaceHolder
                                        runat="server"
                                        ID="phPitcherFilterCustomerDeliveryData"
                                        Visible='<%# (bool?)Eval("HasPitcherFilterBeenProvidedToCustomer") ?? false %>'>
                                        <tr>
                                            <td>Date Delivered</td>
                                            <td>
                                                <mmsinc:MvpLabel
                                                    runat="server"
                                                    ID="lblDatePitcherFilterDeliveredToCustomer"
                                                    Text='<%# Eval("DatePitcherFilterDeliveredToCustomer","{0:d}") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>How Delivered?</td>
                                            <td>
                                                <mmsinc:MvpLabel
                                                    runat="server"
                                                    ID="lblPitcherFilterCustomerDeliveryMethod"
                                                    Text='<%# BindItem.PitcherFilterCustomerDeliveryMethod %>' />
                                            </td>
                                        </tr>
                                        <asp:PlaceHolder
                                            runat="server"
                                            ID="phPitcherFilterCustomerDeliveryOtherMethod"
                                            Visible='<%# (Eval("PitcherFilterCustomerDeliveryMethod") ?? "").ToString() == "Other" %>'>
                                            <tr>
                                                <td>Explain Other</td>
                                                <td>
                                                    <mmsinc:MvpLabel
                                                        runat="server"
                                                        ID="lblPitcherFilterCustomerDeliveryOtherMethod"
                                                        Text='<%# BindItem.PitcherFilterCustomerDeliveryOtherMethod %>' />
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                        <tr>
                                            <td>Date Customer Provided AW State Lead Information</td>
                                            <td>
                                                <mmsinc:MvpLabel
                                                    runat="server"
                                                    ID="lblDateCustomerProvidedAWStateLeadInformation"
                                                    Text='<%# Eval("DateCustomerProvidedAWStateLeadInformation","{0:d}") %>' />
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </table>
                            </fieldset>
                        </asp:PlaceHolder>
                    </td>
                </tr>
            </asp:PlaceHolder>
                <tr>
                    <td>Other Links</td>
                    <td>
                        <asp:HyperLink 
                            runat="server" 
                            ID="hlEditGeneral" 
                            Text="General" CssClass="button"
                            NavigateUrl='<%# String.Format("~/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=update&arg={0}", Eval("WorkOrderID")) %>' />
                        <asp:HyperLink 
                            runat="server" 
                            ID="hlFinalization" 
                            Text="Finalization" CssClass="button"
                            NavigateUrl='<%# String.Format("/modules/mvc/FieldOperations/WorkOrderFinalization/Edit/{0}", Eval("WorkOrderID")) %>' />
                        <asp:HyperLink 
                            runat="server" 
                            ID="hlCrewAssignments" 
                            Text="Crew Assignments" CssClass="button"
                            NavigateUrl="/modules/mvc/FieldOperations/CrewAssignment/ShowCalendar" />
                    </td>
                </tr>

           </table>
        </ItemTemplate>
        <EditItemTemplate>
            <table>
                <tr>
                    <td>Total Man Hours:</td>
                    <td>
                        <mmsinc:MvpLabel ID="MvpLabel5" runat="server" Text='<%# (Eval("TotalManHours") != null) ? Eval("TotalManHours") : "n/a" %>' />
                    </td>
                </tr>
                <tr>
                    <td>Final Work Description:</td>
                    <td>
                        <mmsinc:MvpHiddenField runat="server" ID="hidFinalAssetTypeID" Value='<%# Eval("AssetTypeID") %>' />
                        <mmsinc:MvpHiddenField runat="server" ID="hidRevisit" Value='<%# (Eval("OriginalOrderNumber") != null) %>' />
                        <asp:Label runat="server" ID="lblWorkDescription" Text='<%#Eval("WorkDescription") %>'
                            Visible='<%#!(bool)Eval("WorkDescriptionEditable") %>' />
                        <mmsinc:MvpDropDownList runat="server" ID="ddlFinalWorkDescription" CssClass="three-cell"
                            DataSourceID="odsWorkDescriptions" DataTextField="Description" DataValueField="WorkDescriptionID" AppendDataBoundItems="true"
                            SelectedValue='<%# Bind("WorkDescriptionID") %>' onchange="WorkOrderAdditionalFinalizationInfoForm.ddlFinalWorkDescription_Change(this)"
                            OnDataBinding="ddlFinalWorkDescription_OnDataBinding"
                            Visible='<%#(bool)Eval("WorkDescriptionEditable") %>'
                            >
                            <asp:ListItem Value="" Text="--Select Here--"></asp:ListItem>
                        </mmsinc:MvpDropDownList>
                        <mmsinc:MvpObjectDataSource runat="server" ID="odsWorkDescriptions" TypeName="WorkOrders.Model.WorkDescriptionRepository"
                            SelectMethod="SelectByAssetType">
                            <SelectParameters>
                                <asp:ControlParameter Name="assetTypeID" Type="Int32" ControlID="hidFinalAssetTypeID" PropertyName="Value" />
                                <asp:Parameter Name="input" Type="Boolean" DefaultValue="false" />
                                <asp:ControlParameter Name="revisit" Type="Boolean" ControlID="hidRevisit" />
                            </SelectParameters>
                        </mmsinc:MvpObjectDataSource>
                    </td>
                </tr>
                <tr class="trFinalMainBreakInfo" style="display:none">
                    <td>Estimated Customer Impact:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlFinalCustomerImpactRange" DataSourceID="odsCustomerImpactRanges"
                            DataTextField="Description" DataValueField="CustomerImpactRangeID" SelectedValue='<%# Bind("CustomerImpactRangeID") %>'
                            AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </asp:DropDownList>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="WorkOrderAdditionalFinalizationInfoForm.validateCustomerImpactRange"
                            ErrorMessage="Required when work description is for a main break." ValidationGroup="additionalInfoForm" />
                    </td>
                </tr>
                <tr class="trFinalMainBreakInfo" style="display:none">
                    <td>Anticipated Repair Time:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlFinalRepairTimeRange" DataSourceID="odsRepairTimeRanges"
                            DataTextField="Description" DataValueField="RepairTimeRangeID" SelectedValue='<%# Bind("RepairTimeRangeID") %>'
                            AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </asp:DropDownList>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="WorkOrderAdditionalFinalizationInfoForm.validateRepairTimeRange"
                            ErrorMessage="Required when work description is for a main break." ValidationGroup="additionalInfoForm" />
                    </td>
                </tr>
                <tr class="trFinalMainBreakInfo" style="display: none">
                    <td>Alert Issued?</td>
                    <td><asp:DropDownList runat="server" ID="ddlFinalAlertIssued" SelectedValue='<%# Bind("AlertIssued") %>'>
                            <asp:ListItem Text="--Select Here--" Value="" />
                            <asp:ListItem Text="Yes" Value="True" />
                            <asp:ListItem Text="No" Value="False" />
                        </asp:DropDownList></td>
                </tr>
                <tr class="trFinalMainBreakInfo" style="display: none">
                    <td>Significant Traffic Impact?</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlFinalSignificantTrafficImpact" SelectedValue='<%# Bind("SignificantTrafficImpact") %>'>
                            <asp:ListItem Text="--Select Here--" Value="" />
                            <asp:ListItem Text="Yes" Value="True" />
                            <asp:ListItem Text="No" Value="False" />
                        </asp:DropDownList>
                        <asp:CustomValidator ID="CustomValidator4" runat="server" ClientValidationFunction="WorkOrderAdditionalFinalizationInfoForm.validateSignificantTrafficImpact"
                            ErrorMessage="Required when work description is for a main break." ValidationGroup="additionalInfoForm" />
                    </td>
                </tr>
                <tr>
                    <td>Total Gallons Lost:</td>
                    <td>
                        <mmsinc:MvpTextBox runat="server" ID="txtLostWater" Text='<%# Bind("LostWater") %>' />
                        <asp:RangeValidator runat="server" EnableClientScript="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="txtLostWater" Text="Must be a valid number from 0 to 999999999" MaximumValue="999999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" 
                            ClientValidationFunction="WorkOrderAdditionalFinalizationInfoForm.validateLostWater"
                            Display="Dynamic"
                            ErrorMessage="Required when work description is for a main break." ValidationGroup="additionalInfoForm" />
                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="Leakage Chart.pdf" Text="Leakage Chart" Target="_blank" />
                    </td>
                </tr>
                <tr>
                    <td>Distance From Cross Street (Feet)</td>
                    <td>
                        <mmsinc:MvpTextBox runat="server" ID="txtDistanceFromCrossStreet" Text='<%# Bind("DistanceFromCrossStreet") %>' />

                        <asp:RegularExpressionValidator runat="server" ID="revDistanceFromCrossStreet" ControlToValidate="txtDistanceFromCrossStreet"
                            ValidationExpression="\d*\.?\d*" ErrorMessage="Please enter a number" Display="Dynamic" ValidationGroup="additionalInfoForm" />
                        <asp:CustomValidator ID="CustomValidator5" runat="server" ClientValidationFunction="WorkOrderAdditionalFinalizationInfoForm.validateDistanceFromCrossStreet"
                            ErrorMessage="Required when the work order priority is 'Emergency'" ValidationGroup="additionalInfoForm"/>

                        <%-- NOTE: These are "load-bearing" hidden fields.  Do not remove them.  --%>
                        <asp:HiddenField runat="server" ID="hidTownSectionID" Value='<%# Bind("TownSectionID") %>' />
                        <asp:HiddenField runat="server" ID="hidStreetOpeningPermitRequired" Value='<%# Bind("StreetOpeningPermitRequired") %>' />
                        <asp:HiddenField runat="server" ID="hidTrafficControlRequired" Value='<%# Bind("TrafficControlRequired") %>' />
                        <asp:HiddenField runat="server" ID="hidDigitalAsBuiltRequired" Value='<%# Bind("DigitalAsBuiltRequired") %>' />
                        <asp:HiddenField runat="server" ID="hidPriority" Value='<%# Eval("Priority") %>' />
                    </td>
                </tr>
                <tr>
                    <td>Current Notes:</td>
                    <td>
                        <mmsinc:MvpLabel runat="server" ID="lblCurrentNotes" Text='<%# (Eval("Notes")!=null) ? ((string)Eval("Notes")).Replace("\n", "<br />") : String.Empty %>' />
                    </td>
                </tr>
                <tr>
                    <td>Append Notes:</td>
                    <td>
                        <mmsinc:MvpTextBox runat="server" ID="txtAppendNotes" TextMode="MultiLine" style="width:278px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <mmsinc:MvpLinkButton runat="server" ID="lbUpdate" CssClass="button" Text="Update" OnClick="lbUpdate_Click" OnClientClick="return WorkOrderAdditionalFinalizationInfoForm.lbUpdate_Click();"
                            ValidationGroup="additionalInfoForm" />
                    </td>
                </tr>
                
                <asp:PlaceHolder ID="phUpdatedMobileGIS" runat="server" Visible='<%#((bool)Eval("WorkDescription.IsServiceLineRenewal") || (bool)Eval("WorkDescription.IsServiceLineRetire") || (bool)Eval("WorkDescription.RequiresPitcherFilter")) %>'>
                    <tr>
                        <td colspan="2">
                            <asp:PlaceHolder runat="server" Visible='<%#((bool)Eval("WorkDescription.IsServiceLineRenewal") || (bool)Eval("WorkDescription.IsServiceLineRetire")) %>'>
                                <fieldset>
                                <legend>Service Line Info</legend>
                                <table>
                                    <asp:PlaceHolder runat="server" Visible='<%#Eval("WorkDescription.IsServiceLineRenewal") %>'>
                                        <tr>
                                            <td>Previous Service Company Material:</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlPreviousServiceLineMaterial" DataSourceID="odsServiceMaterials"
                                                                  DataTextField="Description" DataValueField="ServiceMaterialID" SelectedValue='<%# Bind("PreviousServiceLineMaterialID") %>'
                                                                  AppendDataBoundItems="true">
                                                    <asp:ListItem Text="--Select Here--" Value="" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Previous Service Company Size:</td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlPreviousServiceLineSize" DataSourceID="odsServiceSizes"
                                                                  DataTextField="ServiceSizeDescription" DataValueField="ServiceSizeID" SelectedValue='<%# Bind("PreviousServiceLineSizeID") %>'
                                                                  AppendDataBoundItems="true">
                                                    <asp:ListItem Text="--Select Here--" Value="" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                    <tr>
                                        <td>Service Company Material:</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlCompanyServiceLineMaterial" DataSourceID="odsServiceMaterials"
                                                              DataTextField="Description" DataValueField="ServiceMaterialID" SelectedValue='<%# Bind("CompanyServiceLineMaterialID") %>'
                                                              AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select Here--" Value="" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Service Company Size:</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlCompanyServiceLineSize" DataSourceID="odsServiceSizes"
                                                              DataTextField="ServiceSizeDescription" DataValueField="ServiceSizeID" SelectedValue='<%# Bind("CompanyServiceLineSizeID") %>'
                                                              AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select Here--" Value="" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <asp:PlaceHolder runat="server" Visible='<%#Eval("WorkDescription.IsServiceLineRenewal") %>'>
                                        <tr>
                                        <td>Customer Service Line Material:</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlCustomerServiceLineMaterial" DataSourceID="odsServiceMaterials"
                                                DataTextField="Description" DataValueField="ServiceMaterialID" SelectedValue='<%# Bind("CustomerServiceLineMaterialID") %>'
                                                AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select Here--" Value="" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Customer Service Line Size:</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlCustomerServiceLineSize" DataSourceID="odsServiceSizes"
                                                DataTextField="ServiceSizeDescription" DataValueField="ServiceSizeID" SelectedValue='<%# Bind("CustomerServiceLineSizeID") %>'
                                                AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select Here--" Value="" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Door Notice Left:</td>
                                        <td>
                                            <mmsinc:MvpTextBox runat="server" ID="txtDoorNoticeLeftDate" Text='<%# Bind("DoorNoticeLeftDate","{0:d}") %>' autocomplete="off" />
                                            <atk:CalendarExtender runat="server" ID="ceDoorNoticeLeftDate" TargetControlID="txtDoorNoticeLeftDate" />
                                            <asp:CompareValidator runat="server" ID="cvDoorNoticeLeftDate" ErrorMessage="Please enter a valid date" ControlToValidate="txtDoorNoticeLeftDate" Type="Date" Operator="DataTypeCheck" />
                                        </td>
                                    </tr>
                                    </asp:PlaceHolder>
                                </table>
                            </fieldset>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder
                                runat="server"
                                Visible='<%# (bool)Eval("WorkDescription.RequiresPitcherFilter") %>'>
                                <fieldset>
                                    <legend>Compliance Data</legend>
                                    <table>
                                        <tr>
                                            <td>
                                                Initial Service Line Flush Time (Minutes); Minimum 30-Minute Flush Required
                                            </td>
                                            <td>
                                                <mmsinc:MvpTextBox
                                                    runat="server"
                                                    onchange="WorkOrderAdditionalFinalizationInfoForm.txtInitialServiceLineFlushTime_Change(this)"
                                                    ID="txtInitialServiceLineFlushTime"
                                                    Text='<%# BindItem.InitialServiceLineFlushTime %>'/>
                                                <asp:CompareValidator
                                                    runat="server"
                                                    ID="cvInitialServiceLineFLushTimeNumeric"
                                                    ErrorMessage="Please enter a valid number"
                                                    ControlToValidate="txtInitialServiceLineFlushTime"
                                                    Type="Integer"
                                                    Operator="DataTypeCheck"/>
                                                <asp:Label runat="server" ID="lblInitialServiceLineFlushTimeBelowMinimum"
                                                    ForeColor="Red" style="display:none">
                                                    Below minimum-reflush
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Pitcher Filter Provided to Customer</td>
                                            <td>
                                                <mmsinc:MvpCheckBox
                                                    runat="server"
                                                    onclick="WorkOrderAdditionalFinalizationInfoForm.chkHasPitcherFilterBeenProvidedToCustomer_Click(this)"
                                                    ID="chkHasPitcherFilterBeenProvidedToCustomer"
                                                    NullablyChecked='<%# BindItem.HasPitcherFilterBeenProvidedToCustomer %>'/>
                                            </td>
                                        </tr>
                                        <tr class="pitcher-delivery-details">
                                            <td>Date Delivered</td>
                                            <td>
                                                <mmsinc:MvpTextBox
                                                    runat="server"
                                                    ID="txtDatePitcherFilterDeliveredToCustomer"
                                                    Text='<%# Bind("DatePitcherFilterDeliveredToCustomer","{0:d}") %>'
                                                    autocomplete="off"/>
                                                <atk:CalendarExtender
                                                    runat="server"
                                                    ID="ceDatePitcherFilterDeliveredToCustomer"
                                                    TargetControlID="txtDatePitcherFilterDeliveredToCustomer"/>
                                                <asp:CompareValidator
                                                    runat="server"
                                                    ID="cvDatePitcherFilterDeliveredToCustomer"
                                                    ErrorMessage="Please enter a valid date"
                                                    ControlToValidate="txtDatePitcherFilterDeliveredToCustomer"
                                                    Type="Date"
                                                    Operator="DataTypeCheck"/>
                                                <asp:RequiredFieldValidator
                                                    runat="server"
                                                    ID="rfvDatePitcherFilterDeliveredToCustomer"
                                                    ValidationGroup="additionalInfoForm"
                                                    Enabled="False"
                                                    ErrorMessage="Required when pitcher has been delivered to customer"
                                                    ControlToValidate="txtDatePitcherFilterDeliveredToCustomer" />
                                            </td>
                                        </tr>
                                        <tr class="pitcher-delivery-details">
                                            <td>How Delivered?</td>
                                            <td>
                                                <mmsinc:MvpDropDownList
                                                    runat="server"
                                                    ID="ddlPitcherFilterCustomerDeliveryMethod"
                                                    DataSourceID="odsPitcherFilterCustomerDeliveryMethods"
                                                    onchange="WorkOrderAdditionalFinalizationInfoForm.ddlPitcherFilterCustomerDeliveryMethod_Change(this)"
                                                    DataTextField="Description"
                                                    DataValueField="Id"
                                                    SelectedValue='<%# BindItem.PitcherFilterCustomerDeliveryMethodId %>'
                                                    AppendDataBoundItems="true">
                                                    <asp:ListItem Text="--Select Here--" Value="" />
                                                </mmsinc:MvpDropDownList>
                                                <asp:CustomValidator
                                                    runat="server"
                                                    ID="cvPitcherFilterCustomerDeliveryMethod"
                                                    ValidationGroup="additionalInfoForm"
                                                    Enabled="False"
                                                    ValidateEmptyText="True"
                                                    ControlToValidate="ddlPitcherFilterCustomerDeliveryMethod"
                                                    ClientValidationFunction="WorkOrderAdditionalFinalizationInfoForm.validatePitcherFilterCustomerDeliveryMethod"
                                                    ErrorMessage="Required when pitcher has been delivered to customer" />
                                            </td>
                                        </tr>
                                        <tr class="pitcher-delivery-details-other">
                                            <td>Explain Other</td>
                                            <td>
                                                <mmsinc:MvpTextBox
                                                    runat="server"
                                                    ID="txtPitcherFilterCustomerDeliveryOtherMethod"
                                                    Text='<%# BindItem.PitcherFilterCustomerDeliveryOtherMethod %>'
                                                    MaxLength="50" />
                                                <asp:RequiredFieldValidator
                                                    runat="server"
                                                    ID="rfvPitcherFilterCustomerDeliveryOtherMethod"
                                                    ValidationGroup="additionalInfoForm"
                                                    Enabled="False"
                                                    ValidateEmptyText="True"
                                                    ControlToValidate="txtPitcherFilterCustomerDeliveryOtherMethod" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Date Customer Provided AW State Lead Information</td>
                                            <td>
                                                <mmsinc:MvpTextBox
                                                    runat="server"
                                                    ID="txtDateCustomerProvidedAWStateLeadInformation"
                                                    Text='<%# Bind("DateCustomerProvidedAWStateLeadInformation","{0:d}") %>'
                                                    autocomplete="off"/>
                                                <atk:CalendarExtender
                                                    runat="server"
                                                    ID="ceDateCustomerProvidedAWStateLeadInformation"
                                                    TargetControlID="txtDateCustomerProvidedAWStateLeadInformation"/>
                                                <asp:CompareValidator
                                                    runat="server"
                                                    ID="cvDateCustomerProvidedAWStateLeadInformation"
                                                    ErrorMessage="Please enter a valid date"
                                                    ControlToValidate="txtDateCustomerProvidedAWStateLeadInformation"
                                                    Type="Date"
                                                    Operator="DataTypeCheck"/>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </asp:PlaceHolder>
                        </td>
                    </tr>
                </asp:PlaceHolder>
                <tr>
                    <td>Other Links</td>
                    <td>
                        <asp:HyperLink 
                            runat="server" 
                            ID="hlEditGeneral" 
                            Text="General" CssClass="button"
                            NavigateUrl='<%# String.Format("~/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=update&arg={0}", Eval("WorkOrderID")) %>' />
                        <asp:HyperLink 
                            runat="server" 
                            ID="hlFinalization" 
                            Text="Finalization" CssClass="button"
                            NavigateUrl='<%# String.Format("/modules/mvc/FieldOperations/WorkOrderFinalization/Edit/{0}", Eval("WorkOrderID")) %>' />
                        <asp:HyperLink 
                            runat="server" 
                            ID="hlCrewAssignments" 
                            Text="Crew Assignments" CssClass="button"
                            NavigateUrl="/modules/mvc/FieldOperations/CrewAssignment/ShowCalendar" />
                    </td>
                </tr>

            </table>
            <asp:ObjectDataSource runat="server" ID="odsServiceMaterials" TypeName="WorkOrders.Model.ServiceMaterialRepository"
                SelectMethod="SelectAllButUnknownSorted" />
            <asp:ObjectDataSource runat="server" ID="odsServiceSizes" TypeName="WorkOrders.Model.ServiceSizeRepository"
                SelectMethod="SelectAllSorted" />

            <asp:ObjectDataSource runat="server" ID="ObjectDataSource2" TypeName="WorkOrders.Model.CustomerImpactRangeRepository"
                SelectMethod="SelectAllSorted" />

            <asp:ObjectDataSource runat="server" ID="odsCustomerImpactRanges" TypeName="WorkOrders.Model.CustomerImpactRangeRepository"
                SelectMethod="SelectAllSorted" />
            <asp:ObjectDataSource runat="server" ID="odsRepairTimeRanges" TypeName="WorkOrders.Model.RepairTimeRangeRepository"
                SelectMethod="SelectAllSorted" />

        <asp:ObjectDataSource
            runat="server"
            ID="odsPitcherFilterCustomerDeliveryMethods"
            TypeName="WorkOrders.Model.PitcherFilterCustomerDeliveryMethodRepository"
            SelectMethod="SelectAllSorted" />
        </EditItemTemplate>
    </mmsinc:MvpFormView>
</div>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrder" OnUpdated="ods_Updated" />
