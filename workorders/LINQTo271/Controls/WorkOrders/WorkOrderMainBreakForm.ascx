<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderMainBreakForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderMainBreakForm" %>

<mmsinc:MvpUpdatePanel runat="server" ID="upMainBreak" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <mmsinc:MvpLabel runat="server" ID="lblWorkDescriptionID" style="display:none;" />
        <mmsinc:MvpGridView runat="server" ID="gvMainBreak" AutoGenerateColumns="false" 
            AutoGenerateSelectButton="false"
            AutoGenerateEditButton="false" DataKeyNames="MainBreakID" DataSourceID="odsMainBreak"
            OnRowCreated="gvMainBreak_OnRowCreated"
            ShowFooter="true">                                                       
            <Columns>
                
                <asp:TemplateField HeaderText="Size">
                    <ItemTemplate>
                        <asp:Label ID="lbServiceSize" runat="server" Text='<%# Eval("ServiceSize.ServiceSizeDescription") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList ID="ddlServiceSize" runat="server" DataSourceID="odsServiceSize" DataTextField="ServiceSizeDescription"
                            DataValueField="ServiceSizeID" AppendDataBoundItems="true"
                            ConvertEmptyStringToNull="true" SelectedValue='<%# Bind("ServiceSizeID") %>'>
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>                        
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList ID="ddlServiceSize" runat="server" DataSourceID="odsServiceSize" DataTextField="ServiceSizeDescription"
                            DataValueField="ServiceSizeID" AppendDataBoundItems="true"
                            ConvertEmptyStringToNull="true">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvServiceSize" ControlToValidate="ddlServiceSize"
                            ValidationGroup="MainBreakFooter" ErrorMessage="Required" CssClass="error" Display="Dynamic" />
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Existing Material">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblMaterial" Text='<%# Eval("MainBreakMaterial.Description") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList ID="ddlMaterial" runat="server" DataSourceID="odsMainBreakMaterial" DataTextField="Description"
                            DataValueField="MainBreakMaterialID" SelectedValue='<%# Bind("MainBreakMaterialID") %>' ></mmsinc:MvpDropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList ID="ddlMaterial" runat="server" DataSourceID="odsMainBreakMaterial" DataTextField="Description"
                            DataValueField="MainBreakMaterialID" AppendDataBoundItems="true"
                            ConvertEmptyStringToNull="true">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvMaterial" ControlToValidate="ddlMaterial"
                            ValidationGroup="MainBreakFooter" ErrorMessage="Required" CssClass="error" Display="Dynamic" />
                    </FooterTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Footage Replaced">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblFootageReplaced" Text='<%#Eval("FootageReplaced") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtFootageReplaced" Text='<%#Bind("FootageReplaced") %>' Enabled="False" />
                        <asp:RegularExpressionValidator runat="server" ID="revFootageReplaced" ControlToValidate="txtFootageReplaced" ValidationGroup="MainBreakEdit" ErrorMessage="Please enter a whole number" CssClass="error" ValidationExpression="\d+" Display="Dynamic"/>
                        <asp:RequiredFieldValidator runat="server"     ID="rfvFootageReplaced" ControlToValidate="txtFootageReplaced" ValidationGroup="MainBreakEdit" ErrorMessage="Required" CssClass="error" Display="Dynamic" Enabled="False" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtFootageReplaced" Text='<%#Bind("FootageReplaced") %>' Enabled="False" />
                        <asp:RegularExpressionValidator runat="server" ID="revFootageReplaced" ValidationGroup="MainBreakFooter" ControlToValidate="txtFootageReplaced" ErrorMessage="Please enter a whole number" CssClass="error" ValidationExpression="\d+" Display="Dynamic"/>
                        <asp:RequiredFieldValidator runat="server"     ID="rfvFootageReplaced" ValidationGroup="MainBreakFooter" ControlToValidate="txtFootageReplaced" ErrorMessage="Required" CssClass="error" Display="Dynamic" Enabled="False" />
                    </FooterTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Replaced With">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblReplacedWith" Text='<%#Eval("ReplacedWith") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList runat="server" ID="ddlReplacedWith" DataSourceID="odsMainBreakMaterial" DataTextField="Description"
                            DataValueField="MainBreakMaterialID" SelectedValue='<%#Bind("ReplacedWithId") %>' AppendDataBoundItems="true"
                            ConvertEmptyStringToNull="True" Enabled="False">
                            <asp:ListItem Text="--Select Here--" Value=""/>
                        </mmsinc:MvpDropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvReplacedWithId" ValidationGroup="MainBreakEdit" 
                            ControlToValidate="ddlReplacedWith" CssClass="error" Display="Dynamic" Enabled="False"
                            ErrorMessage="Please select the material used for replacement."/>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList runat="server" ID="ddlReplacedWith" DataSourceID="odsMainBreakMaterial" DataTextField="Description"
                            DataValueField="MainBreakMaterialID" AppendDataBoundItems="True"
                            ConvertEmptyStringToNull="True" Enabled="False">
                            <asp:ListItem Text="--Select Here--" Value=""/>
                        </mmsinc:MvpDropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvReplacedWithId" ValidationGroup="MainBreakFooter" 
                            ControlToValidate="ddlReplacedWith" CssClass="error" Display="Dynamic" Enabled="False"
                            ErrorMessage="Please select the material used for replacement."/>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Main Condition">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCondition" Text='<%# Eval("MainCondition.Description") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList ID="ddlCondition" runat="server" DataSourceID="odsMainCondition" DataTextField="Description"
                            DataValueField="MainConditionID" SelectedValue='<%# Bind("MainConditionID") %>'></mmsinc:MvpDropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList ID="ddlCondition" runat="server" DataSourceID="odsMainCondition" DataTextField="Description"
                            DataValueField="MainConditionID" AppendDataBoundItems="true"
                            ConvertEmptyStringToNull="true">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvCondition" ControlToValidate="ddlCondition"
                            ValidationGroup="MainBreakFooter" ErrorMessage="Required" CssClass="error" Display="Dynamic" />
                    </FooterTemplate>                                    
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Failure Type">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblFailureType" Text='<%# Eval("MainFailureType.Description") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList ID="ddlFailureType" runat="server" DataSourceID="odsMainFailureType" DataTextField="Description"
                            DataValueField="MainFailureTypeID" SelectedValue='<%# Bind("MainFailureTypeID") %>'></mmsinc:MvpDropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList ID="ddlFailureType" runat="server" DataSourceID="odsMainFailureType" DataTextField="Description"
                            DataValueField="MainFailureTypeID" AppendDataBoundItems="true"
                            ConvertEmptyStringToNull="true">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvFailureType" ControlToValidate="ddlFailureType"
                            ValidationGroup="MainBreakFooter" ErrorMessage="Required" CssClass="error" Display="Dynamic" />
                    </FooterTemplate>                                    
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Depth (in.)">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDepth" Text='<%# Eval("Depth") %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtDepth" Text='<%# Bind("Depth") %>' />
                        <asp:RegularExpressionValidator runat="server" ID="revDepth" ControlToValidate="txtDepth" ErrorMessage="Must be a valid decimal" CssClass="error" ValidationExpression="\d+(\.\d{1,2})?" Display="Dynamic" ValidationGroup="MainBreakEdit"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvDepth" ControlToValidate="txtDepth" ErrorMessage="Required" CssClass="error" ValidationGroup="MainBreakEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtDepth" Text='<%# Bind("Depth") %>' />
                        <asp:RegularExpressionValidator runat="server" ID="revDepth" ValidationGroup="MainBreakFooter" ControlToValidate="txtDepth" ErrorMessage="Must be a valid decimal" CssClass="error" ValidationExpression="\d+(\.\d{1,2})?" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvDepth" ValidationGroup="MainBreakFooter" ControlToValidate="txtDepth" ErrorMessage="Required" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>                                        
                    </FooterTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Soil Condition">
                    <ItemTemplate>
                       <asp:Label ID="lbSoilCondition" runat="server" Text='<%# Eval("MainBreakSoilCondition.Description") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList ID="ddlSoilCondition" runat="server" DataSourceID="odsMainBreakSoilCondition" DataTextField="Description"
                            DataValueField="MainBreakSoilConditionID" SelectedValue='<%# Bind("MainBreakSoilConditionID") %>'></mmsinc:MvpDropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList ID="ddlSoilCondition" runat="server" DataSourceID="odsMainBreakSoilCondition" DataTextField="Description"
                            DataValueField="MainBreakSoilConditionID" AppendDataBoundItems="true"
                            ConvertEmptyStringToNull="true">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvSoilCondition" ControlToValidate="ddlSoilCondition"
                            ValidationGroup="MainBreakFooter" ErrorMessage="Required" CssClass="error" Display="Dynamic" />
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Customers Affected">
                    <ItemTemplate>
                        <asp:Label ID="lbCustomersAffected" runat="server" Text='<%# Eval("CustomersAffected") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpTextBox ID="txtCustomersAffected" runat="server" Text='<%# Bind("CustomersAffected") %>'></mmsinc:MvpTextBox>
                        <asp:RegularExpressionValidator runat="server" ID="revCustomersAffected" ControlToValidate="txtCustomersAffected" ValidationGroup="MainBreakEdit" ErrorMessage="Must be a valid integer" CssClass="error" ValidationExpression="\d+" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvCustomersAffected" ControlToValidate="txtCustomersAffected" ValidationGroup="MainBreakEdit" ErrorMessage="Required" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox ID="txtCustomersAffected" runat="server"></mmsinc:MvpTextBox>
                        <asp:RegularExpressionValidator runat="server" ID="revCustomersAffected" ValidationGroup="MainBreakFooter" ControlToValidate="txtCustomersAffected" ErrorMessage="Must be a valid integer" CssClass="error" ValidationExpression="\d+" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvCustomersAffected" ValidationGroup="MainBreakFooter" ControlToValidate="txtCustomersAffected" ErrorMessage="Required" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>                                

                <asp:TemplateField HeaderText="Shut Down Time (Hrs)">
                    <ItemTemplate>
                        <asp:Label ID="lbShutdownTime" runat="server" Text='<%# Eval("ShutdownTime") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpTextBox ID="txtShutdownTime" runat="server" Text='<%# Bind("ShutdownTime") %>'></mmsinc:MvpTextBox>
                        <asp:RegularExpressionValidator runat="server" ID="revShutdownTime" ControlToValidate="txtShutdownTime" ValidationGroup="MainBreakEdit" ErrorMessage="Must be a valid decimal" CssClass="error" ValidationExpression="\d+(\.\d{1,2})?" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvShutdownTime" ControlToValidate="txtShutdownTime" ValidationGroup="MainBreakEdit" ErrorMessage="Required" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox ID="txtShutdownTime" runat="server"></mmsinc:MvpTextBox>
                        <asp:RegularExpressionValidator runat="server" ID="revShutdownTime" ValidationGroup="MainBreakFooter" ControlToValidate="txtShutdownTime" ErrorMessage="Must be a valid decimal" CssClass="error" ValidationExpression="\d+(\.\d{1,2})?" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvShutdownTime" ValidationGroup="MainBreakFooter" ControlToValidate="txtShutdownTime" ErrorMessage="Required" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>                                        
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Disinfection Method">
                    <ItemTemplate>
                        <asp:Label ID="lbDisinfectionMethod" runat="server" Text='<%# Eval("MainBreakDisinfectionMethod.Description") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList ID="ddlDisinfectionMethod" runat="server" DataSourceID="odsMainBreakDisinfectionMethod"
                            DataTextField="Description" DataValueField="MainBreakDisinfectionMethodID" SelectedValue='<%# Bind("MainBreakDisinfectionMethodID") %>' />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList ID="ddlDisinfectionMethod" runat="server" DataSourceID="odsMainBreakDisinfectionMethod"
                            DataTextField="Description" DataValueField="MainBreakDisinfectionMethodID" AppendDataBoundItems="true"
                            ConvertEmptyStringToNull="true">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvDisinfectionMethod" ControlToValidate="ddlDisinfectionMethod"
                            ValidationGroup="MainBreakFooter" ErrorMessage="Required" CssClass="error" Display="Dynamic" />
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Flush Method">
                    <ItemTemplate>
                        <asp:Label ID="lbFlushMethod" runat="server" Text='<%# Eval("MainBreakFlushMethod.Description") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpDropDownList ID="ddlFlushMethod" runat="server" DataSourceID="odsMainBreakFlushMethod" DataTextField="Description"
                            DataValueField="MainBreakFlushMethodID" SelectedValue='<%# Bind("MainBreakFlushMethodID") %>'></mmsinc:MvpDropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpDropDownList ID="ddlFlushMethod" runat="server" DataSourceID="odsMainBreakFlushMethod" DataTextField="Description"
                            DataValueField="MainBreakFlushMethodID" AppendDataBoundItems="true"
                            ConvertEmptyStringToNull="true">
                            <asp:ListItem Text="--Select Here--" Value="" />
                        </mmsinc:MvpDropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvFlushMethod" ControlToValidate="ddlFlushMethod"
                            ValidationGroup="MainBreakFooter" ErrorMessage="Required" CssClass="error" Display="Dynamic" />
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Chlorine Residual">
                    <ItemTemplate>
                        <asp:Label ID="lbChlorineResidual" runat="server" Text='<%# Eval("ChlorineResidual") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpTextBox ID="txtChlorineResidual" runat="server" Text='<%# Bind("ChlorineResidual") %>'></mmsinc:MvpTextBox>
                        <asp:CompareValidator runat="server" ID="cvChlorineResidualLessThanTwo" ValidationGroup="MainBreakEdit" ControlToValidate="txtChlorineResidual" Operator="LessThanEqual" ValueToCompare="4.0" ErrorMessage="Must be less than 4" Display="Dynamic"></asp:CompareValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvChlorineResidualShutdownTime" ValidationGroup="MainBreakEdit" ControlToValidate="txtChlorineResidual" ErrorMessage="Required" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ID="revChlorineResidual" ValidationGroup="MainBreakEdit" ControlToValidate="txtChlorineResidual" ErrorMessage="Must be in steps of .1" CssClass="error" ValidationExpression="(^[0-4]$)|(^[0-4]?\.[0-9]$)" Display="Dynamic"></asp:RegularExpressionValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpTextBox ID="txtChlorineResidual" runat="server" ></mmsinc:MvpTextBox>
                        <asp:CompareValidator runat="server" ID="cvChlorineResidualLessThanTwo" ValidationGroup="MainBreakFooter" ControlToValidate="txtChlorineResidual" Operator="LessThanEqual" ValueToCompare="4.0" ErrorMessage="Must be less than 4" Display="Dynamic"></asp:CompareValidator>
                        <asp:RequiredFieldValidator runat="server" ID="rfvChlorineResidualShutdownTime" ValidationGroup="MainBreakFooter" ControlToValidate="txtChlorineResidual" ErrorMessage="Required" CssClass="error" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" ID="revChlorineResidual" ValidationGroup="MainBreakFooter" ControlToValidate="txtChlorineResidual" ErrorMessage="Must be in steps of .1" CssClass="error" ValidationExpression="(^[0-4]$)|(^[0-4]?\.[0-9]$)" Display="Dynamic"></asp:RegularExpressionValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Boil Alert Issued">
                    <ItemTemplate>
                        <mmsinc:MvpCheckBox ID="cbBoilAlertIssued" runat="server" Checked='<%# Eval("BoilAlertIssued") %>' Enabled="false" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpCheckBox ID="cbBoilAlertIssued" runat="server" Checked='<%# Bind("BoilAlertIssued") %>' Enabled="true" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpCheckBox ID="cbBoilAlertIssued" runat="server" Enabled="true" />
                    </FooterTemplate>
                </asp:TemplateField>
               
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbEdit" CausesValidation="false" CommandName="Edit"
                            Text="Edit" />
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbDelete" CausesValidation="false" CommandName="Delete"
                            Text="Delete" OnClientClick="return confirm('Are you sure?');" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbSave" CausesValidation="true" CommandName="Update"
                            Text="Update" ValidationGroup="MainBreakEdit"/>
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbCancel" CausesValidation="false" CommandName="Cancel"
                            Text="Cancel" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbInsert" CausesValidation="true"
                            Text="Insert" ValidationGroup="MainBreakFooter" OnClick="lbMainBreakInsert_Click"
                            OnClientClick="return WorkOrderMainBreakForm.lbMainBreakInsert_Click(event, this)" />
                        <mmsinc:MvpLinkButton runat="server" CssClass="button" ID="lbCancel" CausesValidation="false" CommandName="Cancel"
                            Text="Cancel" OnClick="lbCancel_Click" />
                    </FooterTemplate>
                </asp:TemplateField>                                    
            </Columns>
        </mmsinc:MvpGridView>
        
        <mmsinc:MvpObjectDataSource runat="server" ID="odsMainBreak" SelectMethod="GetMainBreaksByWorkOrder"
            DeleteMethod="DeleteMainBreak" InsertMethod="InsertMainBreak" UpdateMethod="UpdateMainBreak"
            TypeName="WorkOrders.Model.MainBreakRepository" OnInserting="odsMainBreak_Inserting">
            <SelectParameters>
                <asp:Parameter Name="WorkOrderID" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="WorkOrderID" Type="Int32" />
                <asp:Parameter Name="MainBreakMaterialID" Type="Int32" />
                <asp:Parameter Name="MainConditionID" Type="Int32" />
                <asp:Parameter Name="MainFailureTypeID" Type="Int32" />
                <asp:Parameter Name="Depth" Type="Decimal" />
                <asp:Parameter Name="MainBreakSoilConditionID" Type="Int32" />
                <asp:Parameter Name="CustomersAffected" Type="Int32" />
                <asp:Parameter Name="ShutdownTime" Type="decimal" />
                <asp:Parameter Name="MainBreakDisinfectionMethodID" Type="Int32" />                                
                <asp:Parameter Name="MainBreakFlushMethodID" Type="Int32" />     
                <asp:Parameter Name="ChlorineResidual" Type="decimal" ConvertEmptyStringToNull="true"/>
                <asp:Parameter Name="BoilAlertIssued" Type="Boolean" />
                <asp:Parameter Name="ServiceSizeID" Type="Int32" />
                <asp:Parameter Name="FootageReplaced" Type="Int32" ConvertEmptyStringToNull="True"/>
                <asp:Parameter Name="ReplacedWithId" Type="Int32" ConvertEmptyStringToNull="True"/>
            </InsertParameters>                            
        </mmsinc:MvpObjectDataSource>        
                       
        <mmsinc:MvpObjectDataSource runat="server" ID="odsMainBreakMaterial" SelectMethod="GetMainBreakMaterials"
            TypeName="WorkOrders.Model.MainBreakMaterialRepository" />
        <mmsinc:MvpObjectDataSource runat="server" ID="odsMainCondition" SelectMethod="GetMainConditions"
            TypeName="WorkOrders.Model.MainConditionRepository" />                   
        <mmsinc:MvpObjectDataSource runat="server" ID="odsMainFailureType" SelectMethod="GetMainFailureTypes"
            TypeName="WorkOrders.Model.MainFailureTypeRepository" />
        <mmsinc:MvpObjectDataSource runat="server" ID="odsMainBreakSoilCondition" SelectMethod="GetMainBreakSoilConditions"
            TypeName="WorkOrders.Model.MainBreakSoilConditionRepository" />
        <mmsinc:MvpObjectDataSource runat="server" ID="odsMainBreakDisinfectionMethod" SelectMethod="GetMainBreakDisinfectionMethods"
            TypeName="WorkOrders.Model.MainBreakDisinfectionMethodRepository" />
        <mmsinc:MvpObjectDataSource runat="server" ID="odsMainBreakFlushMethod" SelectMethod="GetMainBreakFlushMethods"
            TypeName="WorkOrders.Model.MainBreakFlushMethodRepository" />                              
        <mmsinc:MvpObjectDataSource runat="server" ID="odsServiceSize" SelectMethod="GetMainBreakSizes"
            TypeName="WorkOrders.Model.ServiceSizeRepository" />                              
        
    </ContentTemplate>
    
</mmsinc:MvpUpdatePanel>
