<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderAccountForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderAccountForm" %>
<%@ Import Namespace="MMSINC.Interface" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register Assembly="LINQTo271" Namespace="LINQTo271.Common" TagPrefix="wo" %>

<mmsinc:ScriptInclude runat="server" ScriptFileName="jqmodal.js" />
<mmsinc:CssInclude runat="server" CssFileName="jqModal.css" />

<mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID"
    OnItemUpdating="fvWorkOrder_ItemUpdating">
    <ItemTemplate>
        <table>
            <tbody> 
                <tr>
                    <td>WBS Charged:</td>
                    <td>
                        <asp:Label runat="server" ID="lblAccountCharged" Text='<%# Eval("AccountCharged") %>' />
                    </td>
                </tr>
                <tr>
                    <td>Accounting Type:</td>
                    <td>
                        <!-- From WorkDescription -->
                        <asp:Label runat="server" ID="lblAccountingType" Text='<%# Eval("WorkDescription.AccountingType") %>' />                                    
                    </td>
                </tr>
                <tr runat="server" id="trBusinessUnit" style="display: none;">
                    <td>
                        Cost Center
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblBUsinessUnit" Text='<%# Eval("BusinessUnit") %>' />
                    </td>
                </tr>
                
                <tr>
                    <td>Previously Approved By:</td>
                    <td>
                        <mmsinc:MvpLabel runat="server" ID="lblApprovedBy" Text='<%# Eval("ApprovedBy") %>' />
                    </td>
                </tr>
                <asp:PlaceHolder runat="server" id="phRequiresInvoice" Visible='<%#Eval("OperatingCenter.HasWorkOrderInvoicing") %>'>
                    <tr>
                        <td>Requires Invoice:</td>
                        <td>
                            <asp:Label runat="server" Id="lblRequiresInvoice" Text='<%#Eval("RequiresInvoice") %>'/>
                        </td>
                    </tr>
                </asp:PlaceHolder>
            </tbody>
        </table>  
    </ItemTemplate>
    <EditItemTemplate>
        <table>
            <tbody>

                <tr>
                    <td><strong>Account Charged:</strong></td>
                    <td>
                        <%-- Sonar doesn't understand the `style` property being set from a ternary operation 
                             so it triggers a false-positive there. --%>
                        <asp:TextBox runat="server" ID="txtAccountCharged" Text='<%# Bind("AccountCharged") %>' MaxLength="30" 
                            ReadOnly='<%#(bool)Eval("IsSapUpdatableWorkOrder") %>' 
                            style='<%#(bool)Eval("IsSapUpdatableWorkOrder") ? "display:none;" : "display:;"%>' />
                        <asp:Label runat="server" ID="lblAccountChargedRequired" Text='Required' CssClass="ErrorMessage" Style="display:none"/>
                        <asp:Label runat="server" ID="lblAccountCharged" Text='<%#Eval("AccountCharged") %>' Visible='<%#(bool)Eval("IsSapUpdatableWorkOrder") %>' />
                    </td>
                </tr>
                <tr>
                    <td>Accounting Type:</td>
                    <td>
                        <!-- From WorkDescription -->
                        <asp:Label runat="server" ID="lblAccountingType" Text='<%# Eval("WorkDescription.AccountingType") %>' />
                    </td>
                </tr>
                <tr runat="server" id="trBusinessUnit" style="display:none;">
                    <td>Business Unit</td>
                    <td>
                        <mmsinc:MvpHiddenField runat="server" ID="hidOperatingCenterID" Value='<%# Eval("OperatingCenterID") %>' />
                        <mmsinc:MvpDropDownList runat="server" ID="ddlBusinessUnit" DataSourceID="odsBusinessUnits"
                            DataTextField="BU" DataValueField="BU" onchange="WorkOrderAccountForm.ddlBusinessUnit_Change()"
                            AppendDataBoundItems="true">
                            <asp:ListItem Text="--Select Here--" Value="" />    
                        </mmsinc:MvpDropDownList>
                        <mmsinc:MvpHiddenField runat="server" ID="hidBusinessUnit" Value='<%# Bind("BusinessUnit") %>' />
                        <mmsinc:MvpObjectDataSource runat="server" ID="odsBusinessUnits"
                            TypeName="WorkOrders.Model.BusinessUnitRepository"
                            SelectMethod="SelectByOperatingCenter" 
                            >
                            <SelectParameters>
                                <asp:ControlParameter Name="OperatingCenterID" ControlID="hidOperatingCenterID" Type="Int16" />
                            </SelectParameters>
                        </mmsinc:MvpObjectDataSource>                    
                    </td>
                </tr>
                
                <tr>
                    <td>Previously Approved By:</td>
                    <td>
                        <mmsinc:MvpLabel runat="server" ID="lblApprovedBy" Text='<%# Eval("ApprovedBy") %>' />
                    </td>
                </tr>
                <asp:PlaceHolder runat="server" id="phRequiresInvoice" Visible='<%#Eval("OperatingCenter.HasWorkOrderInvoicing") %>'>
                    <tr>
                        <td>Requires Invoice:</td>
                        <td>
                            <mmsinc:MvpDropDownList runat="server" Id="ddlRequiresInvoice" SelectedValue='<%#Bind("RequiresInvoice") %>'>
                                <asp:ListItem Value="">-- Select Here --</asp:ListItem>
                                <asp:ListItem Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </mmsinc:MvpDropDownList>
                        </td>
                    </tr>
                </asp:PlaceHolder>

                <tr>
                    <td></td>
                    <td>
                        <asp:PlaceHolder runat="server" id="phApprovalButton" Visible='<%#Eval("CanBeApproved")%>'>
                            <asp:Label runat="server" CssClass="error" Text="Asset and Work Description do not match. Please update the Asset to match the Work Description." Visible='<%#Eval("AssetTypeError") %>'/>
                            <br/>
                            <wo:MyButton runat="server" ID="btnSave" Text="Approve" OnClick="btnSave_Click" 
                                Enabled='<%#!((DateTime?)Eval("ApprovedOn")).HasValue && !(bool)Eval("AssetTypeError") && (bool)Eval("CanBeApproved") %>'
                                ValidationGroup="Account" 
                                OnClientClick="return WorkOrderAccountForm.btnSave_Click();" />
                            <asp:Label runat="server" ID="lblCanBeApprovedError" Visible='<%#!(bool)Eval("CanBeApproved")%>'
                                Text="A Service must be attached to this work order, and that Service must have a valid Installed Date." CssClass="ErrorMessage"
                                />
                        </asp:PlaceHolder>
                    
                        <wo:MyButton runat="server" ID="btnReject" Text="Reject" Enabled='<%#!((DateTime?)Eval("ApprovedOn")).HasValue %>' />
                        <asp:Panel runat="server" ID="modalWindow" CssClass="jqmWindow" style="height:300px;">
                            <div id="jqmTitle" class="container" style="position:relative;">
                                <button class="jqmClose" id="btnClose">Close X</button>
                                <span id="jqmTitleText">Enter the reason for rejection</span>
                             
                            </div>
                            <div class="container" style="text-align: center; padding: 6px;" >
                                <mmsinc:MvpTextBox ID="txtRejectionNotes" runat="server" TextMode="MultiLine" style="width:95%; height:200px;" ValidationGroup="Reject"></mmsinc:MvpTextBox>
                                <br/>
                                <wo:MyButton runat="server" id="btnNotesSubmit" CommandArgument="Reject" CommandName="Update" ValidationGroup="Reject" OnClientClick="return WorkOrderAccountForm.btnNotesSubmit_Click();" Text="Save" />
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                
                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# !bool.Parse(Eval("CanBeApproved").ToString()) %>'>
                <tr>
                    <td>&nbsp;</td>
                    <td style="color: #ffff;">
                        <div style="background: #ff7171; font-weight: bold; float: left; padding: 12px; width: 100%; ">
                        <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# bool.Parse(Eval("HasServiceApprovalIssue").ToString()) %>'>
                            <div>
                            This work order is for a service but either no service asset record is linked to this work order, or the linked service does not have an installed date. 
                            <br/>
                            Please ensure that this work order is linked to a service with a valid installed date.
                            <br/>
                            <a href="../../../../mvc/FieldOperations/Service?PremiseNumber.Value=<%#Eval("PremiseNumber")%>&PremiseNumber.MatchType=1">Services</a>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible='<%# bool.Parse(Eval("HasInvestigativeWorkDescriptionApprovalIssue").ToString()) %>'>
                            <span>Cannot approve orders with Investigative Work Descriptions.</span>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible='<%# bool.Parse(Eval("HasSapNotReleased").ToString()) %>'>
                            <span>This order has not been "Released" properly from SAP. Please correct this error.</span>
                        </asp:PlaceHolder>
                        </div>
                    </td>
                </tr> 
                </asp:PlaceHolder>
            </tbody>
        </table>  
    </EditItemTemplate>
</mmsinc:MvpFormView>
                  
<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" 
    DataObjectTypeName="WorkOrders.Model.WorkOrder" OnUpdated="ods_Updated" />

