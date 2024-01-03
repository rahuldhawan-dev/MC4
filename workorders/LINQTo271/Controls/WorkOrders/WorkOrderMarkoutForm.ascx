<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderMarkoutForm.ascx.cs"
    Inherits="LINQTo271.Controls.WorkOrders.WorkOrderMarkoutForm" %>
<%@ Import Namespace="MMSINC.Utilities" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%--
Normal editing under this tab is for routine markouts only.  Only the markout number and
date of request will need to be entered here; date ready and expiration date get run though
the workday engine.
--%>
<mmsinc:MvpUpdatePanel runat="server" ID="upMarkouts" UpdateMode="Conditional" ChildrenAsTriggers="true" 
        OnInit="upMarkouts_OnInit" OnDataBinding="upMarkouts_OnDataBinding" 
        OnPreRender="upMarkouts_OnPreRender" OnLoad="upMarkouts_OnLoad" 
    >
    <ContentTemplate>
        <mmsinc:MvpLabel runat="server" ID="lblMarkoutError" CssClass="error"></mmsinc:MvpLabel>
        <mmsinc:MvpLabel runat="server" ID="lblTypeNeeded"></mmsinc:MvpLabel>
        <mmsinc:MvpGridView runat="server" ID="gvMarkouts" AutoGenerateColumns="false" AutoGenerateSelectButton="false" OnRowCreated="gvMarkouts_OnRowCreated"
            DataKeyNames="MarkoutID" DataSourceID="odsMarkouts" ShowFooter="true" OnPreRender="gvMarkouts_OnPreRender" OnDataBound="gvMarkouts_OnDataBound"
            OnRowDeleting="gvMarkouts_OnRowDeleting" OnRowDeleted="gvMarkouts_OnRowDeleted"
            RowStyle-VerticalAlign="Top" FooterStyle-VerticalAlign="Top">
            <Columns>
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>
                        <mmsinc:MvpLinkButton runat="server" ID="lbEdit" CssClass="button" CausesValidation="false" CommandName="Edit"
                            Text="Edit"/>
                        <mmsinc:MvpLinkButton runat="server" ID="lbDelete" CssClass="button" CausesValidation="false" CommandName="Delete"
                            Text="Delete" OnClientClick="return confirm('Are you sure?');" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton runat="server" ID="lbSave" CssClass="button" CausesValidation="true" CommandName="Update" 
                            Text="Update" ValidationGroup="EditMarkoutForm" 
                            OnClientClick="return WorkOrderMarkoutForm.lbUpdate_Click();" />
                        <asp:LinkButton runat="server" ID="lbCancel" CssClass="button" CausesValidation="false" CommandName="Cancel"
                            Text="Cancel" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton runat="server" ID="lbInsert" CssClass="button" CausesValidation="true" Text="Insert" 
                            OnClick="lbInsert_Click" ValidationGroup="MarkoutForm"
                            OnClientClick="return WorkOrderMarkoutForm.lbInsert_Click();" />
                        <asp:LinkButton runat="server" ID="lbCancel" CssClass="button" CausesValidation="false" CommandName="Cancel"
                            Text="Cancel" OnClick="lbCancel_Click" />
                        <asp:ValidationSummary runat="server" ID="MarkoutFormSummary" ValidationGroup="MarkoutForm" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Markout #">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="lnkMarkoutSearch" Text='<%# Eval("MarkoutNumber") %>'
                            NavigateUrl='<%# String.Format("/modules/mvc/FieldOperations/OneCallMarkoutTicket/Index?RequestNumber={0}", Eval("MarkoutNumber")) %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txtMarkoutNumberEdit" Text='<%# Bind("MarkoutNumber") %>' MaxLength="20" Width="100px" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtMarkoutNumber" Text='<%# Bind("MarkoutNumber") %>' MaxLength="20" Width="100px" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Markout Type">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblMarkoutType" Text='<%# Eval("MarkoutType") %>' />
                        <br />
                        <asp:Label runat="server" ID="lblNote" Text='<%# Eval("Note") %>' style="width:425px" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList runat="server" ID="ddlMarkoutTypeEdit" DataSourceID="odsMarkoutTypes"
                            SelectedValue='<%# Bind("MarkoutTypeID") %>' Style="font-size:smaller;width:425px;"
                            DataTextField="Description" DataValueField="MarkoutTypeID"
                            AppendDataBoundItems="true"
                            onChange="WorkOrderMarkoutForm.ddlMarkoutType_Changed(this);">
                            <asp:ListItem Text="--Select Here--" Value="" />    
                        </mmsinc:MvpDropDownList>
                        <br />
                        <mmsinc:MvpTextBox runat="server" ID="txtNoteEdit" Text='<%# Bind("Note") %>' 
                            style="width:425px;display:none;" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList runat="server" ID="ddlMarkoutType" DataSourceID="odsMarkoutTypes"
                            DataTextField="Description" DataValueField="MarkoutTypeID" 
                            Style="font-size:smaller;width:425px;" AppendDataBoundItems="true"
                            onChange="WorkOrderMarkoutForm.ddlMarkoutType_Changed(this);">
                            <asp:ListItem Text="--Select Here--" Value="" />    
                        </mmsinc:MvpDropDownList>
                        <br />
                        <mmsinc:MvpTextBox runat="server" ID="txtNote" Text='<%# Bind("Note") %>' 
                            style="width:425px;display:none;" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date of Request">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDateOfRequest" Text='<%# Eval("DateOfRequest") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="ccDateOfRequestEdit" Text='<%# Bind("DateOfRequest") %>'
                            Width="70px" autocomplete="off" />
                        <atk:CalendarExtender runat="server" ID="ceDateOfRequestEdit" TargetControlID="ccDateOfRequestEdit" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvDateOfRequestEdit" ControlToValidate="ccDateOfRequestEdit" ValidationGroup="EditMarkoutForm" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="ccDateOfRequest" Text='<%# Bind("DateOfRequest") %>'
                            Width="70px" autocomplete="off" />
                        <atk:CalendarExtender runat="server" ID="ceDateOfRequest" TargetControlID="ccDateOfRequest" />
                        <asp:RequiredFieldValidator runat="server" ID="rfvDateOfRequest" ControlToValidate="ccDateOfRequest" ValidationGroup="MarkoutForm" />
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Ready Date" FooterText="--">
                    <ItemTemplate>  
                        <asp:Label runat="server" ID="lblReadyDate" Text='<%# Eval("ReadyDate") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label runat="server" ID="lblReadyDate" Text='<%# Eval("ReadyDate") %>' Visible='<%#!(bool)Eval("WorkOrder.OperatingCenter.MarkoutsEditable") %>'/>
                        <mmsinc:MvpTextBox runat="server" ID="ccMarkoutReadyDateEdit" Text='<%# Bind("ReadyDate") %>'
                            Width="150px"  Visible="<%#MarkoutsEditable%>" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="ccMarkoutReadyDate" Text='<%# Bind("ReadyDate") %>' Width="150px" OnPreRender="ccMarkoutReadyDate_OnPreRender" />
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Expiration Date" FooterText="--">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblExpirationDate" Text='<%# Eval("ExpirationDate") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label runat="server" ID="lblExpirationDate" Text='<%# Eval("ExpirationDate") %>' Visible='<%#!(bool)Eval("WorkOrder.OperatingCenter.MarkoutsEditable") %>' />
                        <mmsinc:MvpTextBox runat="server" ID="ccMarkoutExpirationDateEdit" Text='<%# Bind("ExpirationDate") %>'
                            Width="150px"  Visible='<%#Eval("WorkOrder.OperatingCenter.MarkoutsEditable") %>' />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="ccMarkoutExpirationDate" Text='<%# Bind("ExpirationDate") %>' Width="150px" OnPreRender="ccMarkoutExpirationDate_OnPreRender" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MarkoutID" ReadOnly="true" Visible="false" />
            </Columns>
        </mmsinc:MvpGridView>
        <mmsinc:MvpObjectDataSource runat="server" ID="odsMarkouts" SelectMethod="GetMarkoutsByWorkOrder"
            DeleteMethod="DeleteMarkout" InsertMethod="InsertMarkout" UpdateMethod="UpdateMarkout"
            TypeName="WorkOrders.Model.MarkoutRepository" 
            OnUpdating="odsMarkouts_OnUpdating"
            OnDeleting="odsMarkouts_OnDeleting"
            OnInserting="odsMarkouts_Inserting" OnInserted="odsMarkouts_Inserted">
            <SelectParameters>
                <asp:Parameter Name="WorkOrderID" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="workOrderID" Type="Int32" />
                <asp:Parameter Name="markoutTypeID" Type="Int16" />
                <asp:Parameter Name="markoutNumber" Type="String" />
                <asp:Parameter Name="dateOfRequest" Type="DateTime" />
                <asp:Parameter Name="readyDate" Type="DateTime" />
                <asp:Parameter Name="expirationDate" Type="DateTime" />
                <asp:Parameter Name="creatorID" Type="Int32" />
                <asp:Parameter Name="note" Type="String" />
            </InsertParameters>
        </mmsinc:MvpObjectDataSource>
        
        <asp:ObjectDataSource runat="server" ID="odsMarkoutTypes" SelectMethod="SelectAllSorted"
            TypeName="WorkOrders.Model.MarkoutTypeRepository" />

    </ContentTemplate>
</mmsinc:MvpUpdatePanel>
