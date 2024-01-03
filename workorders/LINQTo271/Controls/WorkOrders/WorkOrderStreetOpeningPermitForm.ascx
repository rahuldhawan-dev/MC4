<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="WorkOrderStreetOpeningPermitForm.ascx.cs"
    Inherits="LINQTo271.Controls.WorkOrders.WorkOrderStreetOpeningPermitForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<!-- Street Opening Permits -->
<mmsinc:MvpUpdatePanel runat="server" UpdateMode="Conditional"
    ChildrenAsTriggers="true">
    <ContentTemplate>
        <mmsinc:MvpGridView runat="server" ID="gvStreetOpeningPermits" AutoGenerateColumns="false"
            AutoGenerateSelectButton="false" DataKeyNames="StreetOpeningPermitID" DataSourceID="odsStreetOpeningPermits"
            ShowFooter="true">
            <Columns>
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>                            
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbSOPEdit" CausesValidation="false" CommandName="Edit"
                            Text="Edit" />
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbSOPDelete" CausesValidation="false" CommandName="Delete"
                            Text="Delete" OnClientClick="return confirm('Are you sure?');" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbSOPSave" CausesValidation="true" CommandName="Update"
                            Text="Update" ValidationGroup="StreetOpeningPermitsUpdate" />
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbSOPCancel" CausesValidation="false" CommandName="Cancel"
                            Text="Cancel" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbSOPInsert" CausesValidation="true"
                            Text="Insert" OnClick="lbSOPInsert_Click" ValidationGroup="StreetOpeningPermits" />
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbSOPCancel" CausesValidation="false" CommandName="Cancel"
                            Text="Cancel" OnClick="lbSOPCancel_Click" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Permit #">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStreetOpeningPermitNumber" Text='<%# Eval("StreetOpeningPermitNumber") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txtStreetOpeningPermitNumberEdit" Text='<%# Bind("StreetOpeningPermitNumber") %>'
                            Width="70px" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvStreetOpeningPermitNumberEdit"
                            ControlToValidate="txtStreetOpeningPermitNumberEdit" Display="Dynamic" ErrorMessage="Required"
                            ValidationGroup="StreetOpeningPermitsUpdate"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtStreetOpeningPermitNumber" Text='<%# Bind("StreetOpeningPermitNumber") %>'
                            Width="70px" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvStreetOpeningPermitNumber" ControlToValidate="txtStreetOpeningPermitNumber"
                            Display="Dynamic" ErrorMessage="Required" ValidationGroup="StreetOpeningPermits"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date Requested">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDateRequested" Text='<%# Eval("DateRequested", "{0:d}") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="ccDateRequestedEdit" Text='<%# Bind("DateRequested", "{0:d}") %>'
                            Width="70px" autocomplete="off" />
                        <atk:CalendarExtender runat="server" ID="ceDateRequestedEdit" TargetControlID="ccDateRequestedEdit" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvDateRequestedEdit" Display="Dynamic"
                            ErrorMessage="Required" ControlToValidate="ccDateRequestedEdit" ValidationGroup="StreetOpeningPermits"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="ccDateRequested" Width="70px" autocomplete="off" />
                        <atk:CalendarExtender runat="server" ID="ceDateRequested" TargetControlID="ccDateRequested" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvDateRequestedEdit" Display="Dynamic"
                            ErrorMessage="Required" ControlToValidate="ccDateRequested" ValidationGroup="StreetOpeningPermits"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="PermitID" InsertVisible="False">
                    <ItemTemplate>
                        <mmsinc:MvpLabel runat="server" ID="lblPermitID" Text='<%#Eval("PermitId")%>' />
                        <mmsinc:MvpLinkButton runat="server" ID="lbPermit" Text="Update" Visible="False"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Has Met Drawing Requirements" InsertVisible="False">
                    <ItemTemplate>
                        <mmsinc:MvpCheckBox runat="server" ID="chkDrawing" Checked='<%#Eval("HasMetDrawingRequirement") ?? false%>' Enabled="False"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Paid" InsertVisible="False">
                    <ItemTemplate>
                        <mmsinc:MvpCheckBox runat="server" ID="chkIsPaidFor" Checked='<%#Eval("IsPaidFor") ?? false %>' Enabled="False"/>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Date Issued">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDateIssued" Text='<%# Eval("DateIssued", "{0:d}") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="ccDateIssuedEdit" Text='<%# Bind("DateIssued", "{0:d}") %>'
                            Width="70px" autocomplete="off" />
                        <atk:CalendarExtender runat="server" ID="ceDateIssuedEdit" TargetControlID="ccDateIssuedEdit" />
                        <asp:CompareValidator runat="server" ID="cvDateIssuedEdit" ControlToCompare="ccDateRequestedEdit"
                            ControlToValidate="ccDateIssuedEdit" Type="Date" Operator="GreaterThanEqual"
                            Display="Dynamic" ErrorMessage="Must be after Date Requested" ValidationGroup="StreetOpeningPermits"></asp:CompareValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="ccDateIssued" Width="70px" autocomplete="off" />
                        <atk:CalendarExtender runat="server" ID="ceDateIssued" TargetControlID="ccDateIssued" />
                        <asp:CompareValidator runat="server" ID="cvDateIssued" ControlToCompare="ccDateRequested"
                            ControlToValidate="ccDateIssued" Type="Date" Operator="GreaterThanEqual" Display="Dynamic"
                            ErrorMessage="Must be after Date Requested" ValidationGroup="StreetOpeningPermits"></asp:CompareValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Expiration Date">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblExpirationDate" Text='<%# Eval("ExpirationDate", "{0:d}")%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="ccExpirationDateEdit" Text='<%# Bind("ExpirationDate", "{0:d}") %>'
                            Width="70px" autocomplete="off" />
                        <atk:CalendarExtender runat="server" ID="ceExpirationDateEdit" TargetControlID="ccExpirationDateEdit" />
                        <asp:CompareValidator runat="server" ID="cvExpirationDateEdit" ControlToCompare="ccDateIssuedEdit"
                            ControlToValidate="ccExpirationDateEdit" Type="Date" Operator="GreaterThanEqual"
                            Display="Dynamic" ErrorMessage="Must be after Date Issued" ValidationGroup="StreetOpeningPermits"></asp:CompareValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="ccExpirationDate" Text='<%# Bind("ExpirationDate", "{0:d}") %>'
                            Width="70px" autocomplete="off" />
                        <atk:CalendarExtender runat="server" ID="ceExpirationDate" TargetControlID="ccExpirationDate" />
                        <asp:CompareValidator runat="server" ID="cvExpirationDate" ControlToCompare="ccDateIssued"
                            ControlToValidate="ccExpirationDate" Type="Date" Operator="GreaterThanEqual"
                            Display="Dynamic" ErrorMessage="Must be after Date Issued" ValidationGroup="StreetOpeningPermits"></asp:CompareValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Notes">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblNotes" Text='<%# Eval("Notes") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txtNotesEdit" Text='<%# Bind("Notes") %>' Width="120px"
                            TextMode="MultiLine" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtNotes" TextMode="MultiLine" Width="120px" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="StreetOpeningPermitID" ReadOnly="true" Visible="false" />
            </Columns>
        </mmsinc:MvpGridView>
        <mmsinc:MvpButton runat="server" ID="btnCreatePermit" Text="Submit New Permit" OnClick="btnCreatePermit_Click" CausesValidation="False" />
        <mmsinc:MvpObjectDataSource runat="server" ID="odsStreetOpeningPermits" SelectMethod="GetStreetOpeningPermitsByWorkOrder"
            DeleteMethod="DeleteStreetOpeningPermit" InsertMethod="InsertStreetOpeningPermit"
            UpdateMethod="UpdateStreetOpeningPermit" TypeName="WorkOrders.Model.StreetOpeningPermitRepository"
            OnInserting="odsStreetOpeningPermits_Inserting">
            <SelectParameters>
                    <asp:Parameter Name="WorkOrderID" DbType="Int32" />
            </SelectParameters>
        </mmsinc:MvpObjectDataSource>
        
    </ContentTemplate>
</mmsinc:MvpUpdatePanel>
