<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderScheduleOfValuesForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderScheduleOfValuesForm" ValidateRequestMode="Disabled"%>
<%@ Import Namespace="MMSINC.Utilities" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<mmsinc:MvpUpdatePanel runat="server" id="upScheduleOfValues" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <mmsinc:MvpGridView runat="server" id="gvScheduleOfValues"
            AutoGenerateColumns="false" 
            AutoGenerateSelectButton="false"
            AutoGenerateEditButton="false" 
            DataKeyNames="Id" 
            DataSourceID="odsWorkOrdersScheduleOfValues"
            ShowFooter="true" EmptyDataText="Please enter schedule of values using the fields below.">                                                       
            <Columns>
                <asp:TemplateField HeaderText="Schedule of Value Category">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblScheduleOfValueCategory" Text='<%#Eval("ScheduleOfValue.ScheduleOfValueCategory") %>'/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList runat="server" ID="ddlScheduleOfValueCategoryEdit"
                            DataSourceID="odsScheduleOfValueCategories"
                            DataTextField="Description" DataValueField="Id" Width="350px"
                            SelectedValue='<%#Eval("ScheduleOfValue.ScheduleOfValueCategory.Id") %>'
                            AppendDataBoundItems="True" ConvertEmptyStringToNull="True">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList runat="server" ID="ddlScheduleOfValueCategory"
                            DataSourceID="odsScheduleOfValueCategories"
                            DataTextField="Description" DataValueField="Id" Width="350px"
                            AppendDataBoundItems="True">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Schedule of Value">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblScheduleOfValue" Text='<%#Eval("ScheduleOfValue") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList ID="ddlScheduleOfValueEdit" runat="server" ValidateRequestMode="Disabled"/>
                        
                        <atk:CascadingDropDown runat="server" ID="cddScheduleOfValueEdit" 
                            TargetControlID="ddlScheduleOfValueEdit"
                            ParentControlID="ddlScheduleOfValueCategoryEdit" 
                            Category="ScheduleOfValue" EmptyText="None Found" EmptyValue="null"
                            PromptText="--Select Here--" PromptValue="Please select a category" 
                            LoadingText="[Loading Labor Items...]"
                            ServicePath="~/Views/ScheduleOfValues/ScheduleOfValuesServiceView.asmx" 
                            ServiceMethod="GetScheduleOfValuesByCategoryID"
                            SelectedValue='<%# Bind("ScheduleOfValueID") %>' 
                            ValidateRequestMode="Disabled"
                            BehaviorID="cddScheduleOfValueEdit" />

                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList ID="ddlScheduleOfValue" runat="server" ValidateRequestMode="Disabled"/>
                        
                        <atk:CascadingDropDown runat="server" ID="cddScheduleOfValue" 
                            TargetControlID="ddlScheduleOfValue"
                            ParentControlID="ddlScheduleOfValueCategory" 
                            Category="ScheduleOfValue" EmptyText="None Found" EmptyValue="null"
                            PromptText="--Select Here--" PromptValue="Please select a category" 
                            LoadingText="[Loading Schedule of Values...]"
                            ServicePath="~/Views/ScheduleOfValues/ScheduleOfValuesServiceView.asmx" 
                            ServiceMethod="GetScheduleOfValuesByCategoryID"
                            ValidateRequestMode="Disabled"
                            BehaviorID="cddScheduleOfValue" />
                        
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Other Description">
                    <ItemTemplate><asp:Label runat="server" ID="lblOtherDescription" Text='<%#Eval("OtherDescription") %>'/></ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtOtherDescriptionEdit" Text='<%#Bind("OtherDescription") %>'/>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtOtherDescription" Text='<%#Bind("OtherDescription") %>'/>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Hours / Total">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTotal" Text='<%#string.Format(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, Eval("Total")) %>' />    
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtTotalEdit" Text='<%#Bind("Total") %>'/>
                        <asp:RegularExpressionValidator runat="server" ID="revTotal" ControlToValidate="txtTotalEdit" 
                            ValidationGroup="ScheduleOfValueEdit" ErrorMessage="Must be a valid decimal" CssClass="error" ValidationExpression="\d+(\.\d{1,2})?" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvTotal" ControlToValidate="txtTotalEdit" 
                            ValidationGroup="ScheduleOfValueEdit" ErrorMessage="Required" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtTotal" />
                        <asp:RegularExpressionValidator runat="server" ID="revTotal" ControlToValidate="txtTotal" 
                            ValidationGroup="ScheduleOfValueFooter" ErrorMessage="Must be a valid decimal" CssClass="error" ValidationExpression="\d+(\.\d{1,2})?" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvTotal" ControlToValidate="txtTotal" 
                            ValidationGroup="ScheduleOfValueFooter" ErrorMessage="Required" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Unit of Measure">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblUnitOfMeasure" Text='<%#Eval("ScheduleOfValue.UnitOfMeasure") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Is Overtime?">
                    <ItemTemplate>
                        <mmsinc:MvpCheckBox runat="server" ID="chkIsOvertimeShow" Checked='<%#Eval("IsOvertime") %>'/>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpCheckBox runat="server" ID="chkIsOvertimeEdit" Checked='<%# Bind("IsOvertime") %>' />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpCheckBox runat="server" ID="chkIsOvertime" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>
                        <mmsinc:MvpLinkButton runat="server" ID="lbEdit" CausesValidation="false" CommandName="Edit" ClientIDMode="AutoID"
                            Text="Edit" />
                        <mmsinc:MvpLinkButton runat="server" ID="lbDelete" CausesValidation="false" CommandName="Delete"
                            Text="Delete" OnClientClick="return confirm('Are you sure?');" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpLinkButton runat="server" ID="lbSave" CausesValidation="true" CommandName="Update" ClientIDMode="AutoID"
                            Text="Update" ValidationGroup="ScheduleOfValueEdit" OnClientClick="WorkOrderScheduleOfValuesForm.validateScheduleOfValueEdit()" />
                        <mmsinc:MvpLinkButton runat="server" ID="lbCancelEdit" CausesValidation="false" CommandName="Cancel"
                            Text="Cancel" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpLinkButton runat="server" ID="lbInsert" CausesValidation="true" ClientIDMode="AutoID"
                            Text="Insert" ValidationGroup="ScheduleOfValueFooter" OnClientClick="WorkOrderScheduleOfValuesForm.validateScheduleOfValue()" 
                            OnClick="lbScheduleOfValueInsert_Click" />
                        <mmsinc:MvpLinkButton runat="server" ID="lbCancel" CausesValidation="false" 
                            CommandName="Cancel" Text="Cancel" OnClick="lbCancel_Click" />
                    </FooterTemplate>
                </asp:TemplateField>    
            </Columns>
        </mmsinc:MvpGridView>

        <mmsinc:MvpObjectDataSource runat="server" ID="odsWorkOrdersScheduleOfValues"
            SelectMethod="GetScheduleOfValuesByWorkOrder"
            DeleteMethod="DeleteWorkOrderScheduleOfValue"
            InsertMethod="InsertWorkOrderScheduleOfValue"
            UpdateMethod="UpdateWorkOrderScheduleOfValue"
            TypeName="WorkOrders.Model.WorkOrderScheduleOfValueRepository"
            OnInserting="odsWorkOrdersScheduleOfValues_Inserting"
            OnUpdating="odsWorkOrdersScheduleOfValues_Updating">
            <SelectParameters>
                <asp:Parameter Name="WorkOrderID" Type="Int32" />
            </SelectParameters>

            <InsertParameters>
                <asp:Parameter Name="WorkOrderID" Type="Int32" />
                <asp:Parameter Name="ScheduleOfValueID" Type="Int32" />
                <asp:Parameter Name="Total" Type="Decimal" />
                <asp:Parameter Name="IsOvertime" Type="Boolean" />
                <asp:Parameter Name="OtherDescription" Type="String"/>
            </InsertParameters>
        </mmsinc:MvpObjectDataSource>
        
        <mmsinc:MvpObjectDataSource runat="server" ID="odsScheduleOfValues" 
            SelectMethod="GetScheduleOfValues"
            TypeName="WorkOrders.Model.ScheduleOfValueRepository" />
        <mmsinc:MvpObjectDataSource runat="server" ID="odsScheduleOfValueCategories"
            TypeName="WorkOrders.Model.ScheduleOfValueCategoryRepository"
            SelectMethod="GetScheduleOfValueLaborCategories"/>
    </ContentTemplate>
</mmsinc:MvpUpdatePanel>