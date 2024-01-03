<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Employee.ascx.cs" Inherits="MapCall.Controls.HR.Employee" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlFacility.ascx" TagName="ddlFacility" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlEmployees.ascx" TagName="ddlEmployees" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlOpCode.ascx" TagName="ddlOpCode" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

            <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="tblEmployeeID" DataSourceID="SqlDataSource1"
                Width="100%" 
                OnDataBound="DetailsView1_DataBound" 
                FieldHeaderStyle-Width="100px"
                >
                <Fields>
                    <asp:BoundField DataField="tblEmployeeID" HeaderText="tblEmployeeID" InsertVisible="False" ReadOnly="True" SortExpression="tblEmployeeID" />
                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                        <ItemTemplate><asp:Label runat="server" ID="lblStatus" Text='<%# Bind("Status") %>'></asp:Label></ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList runat="server" ID="ddlStatus" AppendDataBoundItems="true" SelectedValue='<%# Bind("Status") %>'>
                                <asp:ListItem Text="--Select Here--" Value="" />
                                <asp:ListItem>Active</asp:ListItem>
                                <asp:ListItem>Inactive</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DropDownList runat="server" ID="ddlStatus" AppendDataBoundItems="true" SelectedValue='<%# Bind("Status") %>'>
                                <asp:ListItem Text="--Select Here--" Value="" />
                                <asp:ListItem>Active</asp:ListItem>
                                <asp:ListItem>Inactive</asp:ListItem>
                            </asp:DropDownList>            
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="OpCode" SortExpression="OpCode">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblOpCode" Text='<%# Bind("OpCode") %>'></asp:Label>
                        </ItemTemplate>
                        <InsertItemTemplate><mmsi:ddlOpCode runat="server" ID="ddlOpCode" SelectedValue='<%# Bind("OpCode") %>' /></InsertItemTemplate>
                        <EditItemTemplate><mmsi:ddlOpCode runat="server" ID="ddlOpCode" SelectedValue='<%# Bind("OpCode") %>' /></EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Category" HeaderText="Position Category" SortExpression="Category" ReadOnly="true" InsertVisible="false" />
                    <asp:BoundField DataField="Department_Name" HeaderText="Department_Name" SortExpression="Department_Name" InsertVisible="false" />
                    <asp:TemplateField HeaderText="Reporting_FacilityID" SortExpression="Reporting_FacilityID">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblReporting_FacilityID" Text='<%# Bind("Reporting_FacilityID") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <mmsi:ddlFacility runat="server" ID="ddlFacility" SelectedValue='<%# Bind("Reporting_FacilityID") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <mmsinc:BoundField Required="True" DataField="EmployeeID" HeaderText="EmployeeID" SortExpression="EmployeeID" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="SS_Last_4_Places" HeaderText="SS_Last_4_Places" SortExpression="SS_Last_4_Places" />
                    <mmsinc:BoundField Required="True" DataField="Last_Name" HeaderText="Last_Name" SortExpression="Last_Name" HeaderStyle-Font-Bold="true" />
                    <mmsinc:BoundField Required="True" DataField="First_Name" HeaderText="First_Name" SortExpression="First_Name" HeaderStyle-Font-Bold="true" />
                    <asp:BoundField DataField="Middle_Name" HeaderText="Middle_Name" SortExpression="Middle_Name" />
                    <asp:TemplateField HeaderText="Gender" SortExpression="Gender" HeaderStyle-Font-Bold="true">
                        <ItemTemplate><asp:Label runat="server" ID="lblGender" Text='<%# Bind("Gender") %>'></asp:Label></ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList runat="server" ID="ddlGender" AppendDataBoundItems="true" SelectedValue='<%# Bind("Gender") %>'>
                                <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                                <asp:ListItem>Male</asp:ListItem>
                                <asp:ListItem>Female</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvGender" ControlToValidate="ddlGender" InitialValue="" Text="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <mmsinc:BoundField HeaderStyle-Font-Bold="true" Required="True" SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Date_Hired" HeaderText="Date_Hired" SortExpression="Date_Hired" />
                    <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Seniority_Date" HeaderText="Seniority_Date" SortExpression="Seniority_Date" />
                    <asp:BoundField DataField="Seniority_Ranking" HeaderText="Seniority_Ranking" SortExpression="Seniority_Ranking" />
                    <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Inactive_Date" HeaderText="Inactive_Date" SortExpression="Inactive_Date" />
                    <asp:TemplateField HeaderText="Reason_for_Departure" SortExpression="Reason_for_Departure">
                        <ItemTemplate><asp:Label runat="server" ID="lblReason_For_Departure" Text='<%# Bind("Reason_For_Departure_Text") %>' /></ItemTemplate>
                        <EditItemTemplate>
		                    <asp:DropDownList runat="server" ID="ddlReason_For_Departure"
		                    DataSourceID="dsReason_For_Departure"
		                    AppendDataBoundItems="true"
		                    DataValueField="LookupID"
		                    DataTextField="LookupValue"
		                    SelectedValue='<%# Bind("Reason_For_DepartureID") %>'
		                    >
		                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
		                    </asp:DropDownList>

		                    <asp:SqlDataSource runat="server" ID="dsReason_For_Departure"
		                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
		                    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
		                    SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'Reason_For_Departure' order by 2"
		                    >
		                    </asp:SqlDataSource>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reports_To" SortExpression="Reports_To_Text">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblReports_To" Text='<%# Bind("Reports_To_Text") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <mmsi:ddlEmployees runat="server" ID="ddlReports_To" SelectedValue='<%# Bind("Reports_To") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Position_Start_Date" HeaderText="Position_Start_Date" SortExpression="Position_Start_Date" ReadOnly="true"  InsertVisible="false"/>
                    <asp:BoundField DataField="Position" HeaderText="Position" SortExpression="Position" ReadOnly="true" InsertVisible="false" />
                    <asp:BoundField DataField="Position_Sub_Category" HeaderText="Position_Sub_Category" SortExpression="Position_Sub_Category" ReadOnly="true" InsertVisible="false" />
                    <asp:BoundField DataField="Vacation_Grouping" HeaderText="Vacation_Grouping" SortExpression="Vacation_Grouping" ReadOnly="true"  InsertVisible="false"/>
                    <asp:BoundField DataField="License_Requirement_Attainment" HeaderText="License_Requirement_Attainment" SortExpression="License_Requirement_Attainment" ReadOnly="true"  InsertVisible="false"/>

                    <asp:BoundField DataField="License_Water_Treatment" HeaderText="License_Water_Treatment" SortExpression="License_Water_Treatment" />
                    <asp:BoundField DataField="T_License" HeaderText="T_License" SortExpression="T_License" />
                    <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Date_of_T_License" HeaderText="Date_of_T_License" SortExpression="Date_of_T_License" />
                    <asp:BoundField DataField="License_Water_Distribution" HeaderText="License_Water_Distribution" SortExpression="License_Water_Distribution" />
                    <asp:BoundField DataField="W_License" HeaderText="W_License" SortExpression="W_License" />
                    <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Date_of_W_License" HeaderText="Date_of_W_License" SortExpression="Date_of_W_License" />
                    <asp:BoundField DataField="License_Sewer_Collection" HeaderText="License_Sewer_Collection" SortExpression="License_Sewer_Collection" />
                    <asp:BoundField DataField="C_License" HeaderText="C_License" SortExpression="C_License" />
                    <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Date_of_C_License" HeaderText="Date_of_C_License" SortExpression="Date_of_C_License" />
                    <asp:BoundField DataField="License_Sewer_Treatment" HeaderText="License_Sewer_Treatment" SortExpression="License_Sewer_Treatment" />
                    <asp:BoundField DataField="S_License" HeaderText="S_License" SortExpression="S_License" />
                    <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Date_of_S_License" HeaderText="Date_of_S_License" SortExpression="Date_of_S_License" />
                    <asp:BoundField DataField="License_Industrial_Discharge" HeaderText="License_Industrial_Discharge" SortExpression="License_Industrial_Discharge" />
                    <asp:BoundField DataField="N_License" HeaderText="N_License" SortExpression="N_License" />
                    <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Date_of_N_License" HeaderText="Date_of_N_License" SortExpression="Date_of_N_License" />
                    <asp:BoundField DataField="CDL" HeaderText="CDL" SortExpression="CDL" />
                    <asp:BoundField DataField="Drivers_License" HeaderText="Drivers_License" SortExpression="Drivers_License" />
                    <asp:BoundField DataField="EmergencyContactName" HeaderText="EmergencyContactName" SortExpression="EmergencyContactName" />
                    <asp:BoundField DataField="EmergencyContactPhone" HeaderText="EmergencyContactPhone" SortExpression="EmergencyContactPhone" />
                    <mmsinc:LatLonPickerField DataField="CoordinateID" />
                    <asp:TemplateField HeaderText="Email Address">
                        <ItemTemplate><%#Eval("EmailAddress") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txtEmailAddress" Text='<%#Bind("EmailAddress") %>' />
                            <asp:RegularExpressionValidator ID="valEmailAddress"
                                ControlToValidate="txtEmailAddress"	ValidationExpression=".*@.*\..*" 
                                ErrorMessage="Email address is invalid." 
                                Display="Dynamic" EnableClientScript="true" 
                                Runat="server" SetFocusOnError="true"
                                />

                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                    <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
                    <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
                    <asp:BoundField DataField="Zip_Code" HeaderText="Zip_Code" SortExpression="Zip_Code" />
                    <asp:BoundField DataField="Phone_Pager" HeaderText="Phone_Pager" SortExpression="Phone_Pager" />
                    <asp:BoundField DataField="Phone_Cellular" HeaderText="Phone_Cellular" SortExpression="Phone_Cellular" />
                    <asp:BoundField DataField="Phone_Home" HeaderText="Phone_Home" SortExpression="Phone_Home" />
                    <asp:BoundField DataField="Phone_Work" HeaderText="Phone_Work" SortExpression="Phone_Work" />
                    <asp:BoundField DataField="Phone_Personal_Cellular" HeaderText="Phone_Personal_Cellular" SortExpression="Phone_Personal_Cellular" />
                    <asp:TemplateField HeaderText="Union_Affiliation" SortExpression="Union_Affiliation">
                        <ItemTemplate><asp:Label runat="server" ID="lblUnion_Affiliation" Text='<%# Bind("Union_Affiliation_text") %>' /></ItemTemplate>
                        <EditItemTemplate>
		                    <asp:DropDownList runat="server" ID="ddlUnion_Affiliation"
		                        DataSourceID="dsUnion_Affiliation"
		                        AppendDataBoundItems="true"
		                        DataValueField="LookupID"
		                        DataTextField="LookupValue"
		                        SelectedValue='<%# Bind("Union_AffiliationID") %>'
		                    >
		                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
		                    </asp:DropDownList>

		                    <asp:SqlDataSource runat="server" ID="dsUnion_Affiliation"
		                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
		                        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
		                        SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'Union_Affiliation' order by 2"
		                    >
		                    </asp:SqlDataSource>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:BoundField DataField="Purchase_Card_Number" HeaderText="Purchase_Card_Number" SortExpression="Purchase_Card_Number" />
                    <asp:BoundField DataField="MonthlyDollarLimit" HeaderText="MonthlyDollarLimit" SortExpression="MonthlyDollarLimit" />
                    <asp:BoundField DataField="SingleDollarLimit" HeaderText="SingleDollarLimit" SortExpression="SingleDollarLimit" />
                    
                    <asp:TemplateField HeaderText="TCPA_Status" SortExpression="TCPA_Status_Text" >
                        <ItemTemplate><asp:Label runat="server" ID="lblTCPA_Status" Text='<%# Bind("TCPA_Status_text") %>' /></ItemTemplate>
                        <EditItemTemplate>
		                    <asp:DropDownList runat="server" ID="ddlTCPA_Status"
		                    DataSourceID="dsTCPA_Status"
		                    AppendDataBoundItems="true"
		                    DataValueField="LookupID"
		                    DataTextField="LookupValue"
		                    SelectedValue='<%# Bind("TCPA_StatusID") %>'
		                    >
		                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
		                    </asp:DropDownList>

		                    <asp:SqlDataSource runat="server" ID="dsTCPA_Status"
		                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
		                    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
		                    SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'TCPA_Status' order by 2"
		                    >
		                    </asp:SqlDataSource>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="DPCC_Status" SortExpression="DPCC_Status_Text" >
                        <ItemTemplate><asp:Label runat="server" ID="lblDPCC_Status" Text='<%# Bind("DPCC_Status_text") %>' /></ItemTemplate>
                        <EditItemTemplate>
		                    <asp:DropDownList runat="server" ID="ddlDPCC_Status"
		                    DataSourceID="dsDPCC_Status"
		                    AppendDataBoundItems="true"
		                    DataValueField="LookupID"
		                    DataTextField="LookupValue"
		                    SelectedValue='<%# Bind("DPCC_StatusID") %>'
		                    >
		                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
		                    </asp:DropDownList>

		                    <asp:SqlDataSource runat="server" ID="dsDPCC_Status"
		                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
		                    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
		                    SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'DPCC_Status' order by 2"
		                    >
		                    </asp:SqlDataSource>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Purchase_Card_Reviewer" SortExpression="Purchase_Card_Reviewer_Text">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblPurchase_Card_Reviewer" Text='<%# Bind("Purchase_Card_Reviewer_Text") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <mmsi:ddlEmployees runat="server" ID="ddlPurchase_Card_Reviewer" SelectedValue='<%# Bind("Purchase_Card_Reviewer") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Purchase_Card_Approver" SortExpression="Purchase_Card_Approver_Text">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblPurchase_Card_Approver" Text='<%# Bind("Purchase_Card_Approver_Text") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <mmsi:ddlEmployees runat="server" ID="ddlPurchase_Card_Approver" SelectedValue='<%# Bind("Purchase_Card_Approver") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="InstitutionalKnowledge">
                        <ItemTemplate><asp:Label runat="server" ID="lblInstitutionalKnowledge" Text='<%# Bind("InstitutionalKnowledge_text") %>' /></ItemTemplate>
                        <EditItemTemplate>
		                    <asp:DropDownList runat="server" ID="ddlInstitutionalKnowledge"
		                    DataSourceID="dsInstitutionalKnowledge"
		                    AppendDataBoundItems="true"
		                    DataValueField="LookupID"
		                    DataTextField="LookupValue"
		                    SelectedValue='<%# Bind("InstitutionalKnowledge") %>'
		                    >
		                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
		                    </asp:DropDownList>

		                    <asp:SqlDataSource runat="server" ID="dsInstitutionalKnowledge"
		                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
		                    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
		                    SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'InstitutionalKnowledge' order by 2"
		                    >
		                    </asp:SqlDataSource>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <mmsinc:BoundField DataField="InstitutionalKnowledgeDescription" HeaderText="InstitutionalKnowledgeDescription" SqlDataType="Text" />
                    
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert"></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="New" Visible="false" Text="New"></asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure?');"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Fields>
            </asp:DetailsView>
            <asp:Label runat="server" ID="lblResults"></asp:Label>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" DeleteCommand="DELETE FROM [tblEmployee] WHERE [tblEmployeeID] = @tblEmployeeID"
                InsertCommand="INSERT INTO [tblEmployee] ([Status], [Department_Name], [Reporting_FacilityID], [EmployeeID], [SS_Last_4_Places],
                    [Last_Name], [First_Name], [Middle_Name], [Gender], [Date_Hired], [Seniority_Date], [Seniority_Ranking],
                    [Inactive_Date], [Reason_For_DepartureID], [Reports_To], [License_Water_Treatment], [T_License], [Date_of_T_License],
                    [License_Water_Distribution], [W_License], [Date_of_W_License], [License_Sewer_Collection], [C_License], [Date_of_C_License],
                    [License_Sewer_Treatment], [S_License], [Date_of_S_License], [License_Industrial_Discharge], [N_License],
                    [Date_of_N_License], [CDL], [Drivers_License], [EmergencyContactName], [EmergencyContactPhone], [CoordinateID], [Address],
                    [City], [State], [Zip_Code], [Phone_Pager], [Phone_Cellular], [Phone_Home], [Phone_Work], [Phone_Personal_Cellular],
                    [Union_AffiliationID], [Purchase_Card_Number], [MonthlyDollarLimit], [SingleDollarLimit], [TCPA_StatusID],
                    [DPCC_StatusID], Purchase_Card_Reviewer, Purchase_Card_Approver, OpCode, InstitutionalKnowledge, InstitutionalKnowledgeDescription, EmailAddress) 
                    
                    VALUES (@Status, @Department_Name, @Reporting_FacilityID,
                    @EmployeeID, @SS_Last_4_Places, @Last_Name, @First_Name, @Middle_Name, @Gender, @Date_Hired, @Seniority_Date,
                    @Seniority_Ranking, @Inactive_Date, @Reason_For_DepartureID, @Reports_To, @License_Water_Treatment, @T_License,
                    @Date_of_T_License, @License_Water_Distribution, @W_License, @Date_of_W_License, @License_Sewer_Collection, @C_License,
                    @Date_of_C_License, @License_Sewer_Treatment, @S_License, @Date_of_S_License, @License_Industrial_Discharge, @N_License,
                    @Date_of_N_License, @CDL, @Drivers_License, @EmergencyContactName, @EmergencyContactPhone, @CoordinateID, @Address,
                    @City, @State, @Zip_Code, @Phone_Pager, @Phone_Cellular, @Phone_Home, @Phone_Work, @Phone_Personal_Cellular,
                    @Union_AffiliationID, @Purchase_Card_Number, @MonthlyDollarLimit, @SingleDollarLimit, @TCPA_StatusID,
                    @DPCC_StatusID, @Purchase_Card_Reviewer, @Purchase_Card_Approver, @OpCode, @InstitutionalKnowledge, @InstitutionalKnowledgeDescription, @EmailAddress);SELECT @tblEmployeeID = @@IDENTITY;"
                SelectCommand="
                    SELECT 
                        tblEmployee.[tblEmployeeID], [tblEmployee].[Status], [tblEmployee].[OpCode], 
                        [Category], tblPositions_Classifications.[Department] as [Department_Name], [tblEmployee].[Reporting_FacilityID], 
                        [tblEmployee].[EmployeeID], tblEmployee.[SS_Last_4_Places], [tblEmployee].[Last_Name], 
                        tblEmployee.[First_Name], tblEmployee.[Middle_Name], tblEmployee.[Gender], 
                        tblEmployee.[Date_Hired], tblEmployee.[Seniority_Date], 
                        tblEmployee.[Seniority_Ranking], tblEmployee.[Inactive_Date], tblEmployee.[Reports_To], 
                        [Position_Start_Date], [tblPositions_Classifications].[Position], 
                        [Position_Sub_Category], [Vacation_Grouping], tblEmployee.Union_AffiliationID,
                        Lookup.LookupValue as [License_Requirement_Attainment], 
                        tblEmployee.[License_Water_Treatment], tblEmployee.[T_License], tblEmployee.[Date_of_T_License], 
                        tblEmployee.[License_Water_Distribution], tblEmployee.[W_License], tblEmployee.[Date_of_W_License], 
                        tblEmployee.[License_Sewer_Collection], tblEmployee.[C_License], tblEmployee.[Date_of_C_License], 
                        tblEmployee.[License_Sewer_Treatment], tblEmployee.[S_License], tblEmployee.[Date_of_S_License], 
                        tblEmployee.[License_Industrial_Discharge], tblEmployee.[N_License], tblEmployee.[Date_of_N_License], 
                        tblEmployee.[CDL], tblEmployee.[Drivers_License], tblEmployee.[EmergencyContactName], 
                        tblEmployee.[EmergencyContactPhone], tblEmployee.[CoordinateID], tblEmployee.EmailAddress,
                        tblEmployee.[Address], tblEmployee.[City], tblEmployee.[State], tblEmployee.[Zip_Code], 
                        tblEmployee.[Phone_Pager], tblEmployee.[Phone_Cellular], tblEmployee.[Phone_Home], tblEmployee.[Phone_Work], 
                        tblEmployee.[Phone_Personal_Cellular], tblEmployee.[Purchase_Card_Number], 
                        tblEmployee.[MonthlyDollarLimit], tblEmployee.[SingleDollarLimit], tblEmployee.[TCPA_StatusID], 
                        tblEmployee.[DPCC_StatusID], tblEmployee.Reason_For_DepartureID, 
                        #rfd.LookupValue as Reason_For_Departure_Text, 
                        #Union_Affiliation.LookupValue as Union_Affiliation_text,
                        #TCPA_Status.LookupValue as TCPA_Status_text, 
                        #DPCC_Status.LookupValue as DPCC_Status_text,
                        isNull(#tblEmp.Last_Name,'') + ', ' + isNull(#tblEmp.First_Name,'') + ' ' + isNull(#tblEmp.Middle_Name,'') as [Reports_To_Text],
                        isNull(#tblEmp1.Last_Name,'') + ', ' + isNull(#tblEmp1.First_Name,'') + ' ' + isNull(#tblEmp1.Middle_Name,'') as [Purchase_Card_Reviewer_Text],tblEmployee.Purchase_Card_Reviewer,
                        isNull(#tblEmp2.Last_Name,'') + ', ' + isNull(#tblEmp2.First_Name,'') + ' ' + isNull(#tblEmp2.Middle_Name,'') as [Purchase_Card_Approver_Text],tblEmployee.Purchase_Card_Approver,
                        #IK.LookupValue as InstitutionalKnowledge_text, tblEmployee.InstitutionalKnowledge, tblEmployee.InstitutionalKnowledgeDescription
                    FROM [tblEmployee] 
                    LEFT JOIN [tblPosition_History] on [tblPosition_History].tblEmployeeID = tblEmployee.tblEmployeeID and tblPosition_History.Position_History_ID = (SELECT max(Position_History_ID) FROM tblPosition_History WHERE [Position_End_Date] IS NULL AND tblEmployeeID = tblEmployee.tblEmployeeID)
                    LEFT JOIN [tblPositions_Classifications] on [tblPositions_Classifications].PositionID = [tblPosition_History].Position_ID
                    LEFT JOIN [Lookup] on [Lookup].LookupID = [tblPositions_classifications].[License_Requirement_Attainment]
                    LEFT JOIN [Lookup] #rfd on #rfd.LookupID = Reason_for_departureID
                    LEFT JOIN [Lookup] #Union_Affiliation on #Union_Affiliation.LookupID = Union_AffiliationID
                    LEFT JOIN Lookup #TCPA_Status on #TCPA_Status.LookupID = TCPA_StatusID
                    LEFT JOIN Lookup #DPCC_Status on #DPCC_Status.LookupID = DPCC_StatusID
                    LEFT JOIN Lookup #IK on #IK.LookupID = InstitutionalKnowledge
                    LEFT JOIN tblEmployee #tblEmp on #tblEmp.tblEmployeeID = tblEmployee.Reports_To
                    LEFT JOIN tblEmployee #tblEmp1 on #tblEmp1.tblEmployeeID = tblEmployee.Purchase_Card_Reviewer
                    LEFT JOIN tblEmployee #tblEmp2 on #tblEmp2.tblEmployeeID = tblEmployee.Purchase_Card_Approver
                    WHERE tblEmployee.[tblEmployeeID] = @tblEmployeeID" 
                    
                    UpdateCommand="UPDATE [tblEmployee] SET [Status] = @Status, [Department_Name] = @Department_Name,
                    [Reporting_FacilityID] = @Reporting_FacilityID, [EmployeeID] = @EmployeeID, [SS_Last_4_Places] = @SS_Last_4_Places,
                    [Last_Name] = @Last_Name, [First_Name] = @First_Name, [Middle_Name] = @Middle_Name, [Gender] = @Gender,
                    [Date_Hired] = @Date_Hired, [Seniority_Date] = @Seniority_Date,
                    [Seniority_Ranking] = @Seniority_Ranking, [Inactive_Date] = @Inactive_Date,
                    [Reason_For_DepartureID] = @Reason_For_DepartureID, [Reports_To] = @Reports_To,
                    [License_Water_Treatment] = @License_Water_Treatment, [T_License] = @T_License,
                    [Date_of_T_License] = @Date_of_T_License, [License_Water_Distribution] = @License_Water_Distribution,
                    [W_License] = @W_License, [Date_of_W_License] = @Date_of_W_License,
                    [License_Sewer_Collection] = @License_Sewer_Collection, [C_License] = @C_License,
                    [Date_of_C_License] = @Date_of_C_License, [License_Sewer_Treatment] = @License_Sewer_Treatment,
                    [S_License] = @S_License, [Date_of_S_License] = @Date_of_S_License,
                    [License_Industrial_Discharge] = @License_Industrial_Discharge, [N_License] = @N_License,
                    [Date_of_N_License] = @Date_of_N_License, [CDL] = @CDL, [Drivers_License] = @Drivers_License,
                    [EmergencyContactName] = @EmergencyContactName, [EmergencyContactPhone] = @EmergencyContactPhone,
                    [CoordinateID] = @CoordinateID, [Address] = @Address, [City] = @City, [State] = @State, [Zip_Code] = @Zip_Code,
                    [Phone_Pager] = @Phone_Pager, [Phone_Cellular] = @Phone_Cellular, [Phone_Home] = @Phone_Home,
                    [Phone_Work] = @Phone_Work, [Phone_Personal_Cellular] = @Phone_Personal_Cellular,
                    [Union_AffiliationID] = @Union_AffiliationID, 
                    [Purchase_Card_Number] = @Purchase_Card_Number, [MonthlyDollarLimit] = @MonthlyDollarLimit,
                    [SingleDollarLimit] = @SingleDollarLimit, [TCPA_StatusID] = @TCPA_StatusID, [DPCC_StatusID] = @DPCC_StatusID,
                    Purchase_Card_Reviewer = @Purchase_Card_Reviewer, Purchase_Card_Approver = @Purchase_Card_Approver, 
                    OpCode = @OpCode, InstitutionalKnowledge=@InstitutionalKnowledge, InstitutionalKnowledgeDescription = @InstitutionalKnowledgeDescription,
                    EmailAddress = @EmailAddress
                    
                    WHERE [tblEmployeeID] = @tblEmployeeID" OnInserted="SqlDataSource1_Inserted"
                OnDeleted="SqlDataSource1_Deleted" OnUpdated="SqlDataSource1_Updated">
                <InsertParameters>
                    <asp:Parameter Name="Status" Type="String" />
                    <asp:Parameter Name="Department_Name" Type="String" />
                    <asp:Parameter Name="Reporting_FacilityID" Type="String" />
                    <asp:Parameter Name="EmployeeID" Type="String" />
                    <asp:Parameter Name="SS_Last_4_Places" Type="String" />
                    <asp:Parameter Name="Last_Name" Type="String" />
                    <asp:Parameter Name="First_Name" Type="String" />
                    <asp:Parameter Name="Middle_Name" Type="String" />
                    <asp:Parameter Name="Gender" Type="String" />
                    <asp:Parameter Name="Date_Hired" Type="DateTime" />
                    <asp:Parameter Name="Seniority_Date" Type="DateTime" />
                    <asp:Parameter Name="Seniority_Ranking" Type="Double" />
                    <asp:Parameter Name="Inactive_Date" Type="DateTime" />
                    <asp:Parameter Name="Reason_For_DepartureID" Type="Int32" />
                    <asp:Parameter Name="Reports_To" Type="String" />
                    <asp:Parameter Name="License_Water_Treatment" Type="String" />
                    <asp:Parameter Name="T_License" Type="String" />
                    <asp:Parameter Name="Date_of_T_License" Type="DateTime" />
                    <asp:Parameter Name="License_Water_Distribution" Type="String" />
                    <asp:Parameter Name="W_License" Type="String" />
                    <asp:Parameter Name="Date_of_W_License" Type="DateTime" />
                    <asp:Parameter Name="License_Sewer_Collection" Type="String" />
                    <asp:Parameter Name="C_License" Type="String" />
                    <asp:Parameter Name="Date_of_C_License" Type="DateTime" />
                    <asp:Parameter Name="License_Sewer_Treatment" Type="String" />
                    <asp:Parameter Name="S_License" Type="String" />
                    <asp:Parameter Name="Date_of_S_License" Type="DateTime" />
                    <asp:Parameter Name="License_Industrial_Discharge" Type="String" />
                    <asp:Parameter Name="N_License" Type="String" />
                    <asp:Parameter Name="Date_of_N_License" Type="DateTime" />
                    <asp:Parameter Name="CDL" Type="String" />
                    <asp:Parameter Name="Drivers_License" Type="String" />
                    <asp:Parameter Name="EmergencyContactName" Type="String" />
                    <asp:Parameter Name="EmergencyContactPhone" Type="String" />
                    <asp:Parameter Name="CoordinateID" Type="Int32" />
                    <asp:Parameter Name="Address" Type="String" />
                    <asp:Parameter Name="City" Type="String" />
                    <asp:Parameter Name="State" Type="String" />
                    <asp:Parameter Name="Zip_Code" Type="String" />
                    <asp:Parameter Name="Phone_Pager" Type="String" />
                    <asp:Parameter Name="Phone_Cellular" Type="String" />
                    <asp:Parameter Name="Phone_Home" Type="String" />
                    <asp:Parameter Name="Phone_Work" Type="String" />
                    <asp:Parameter Name="Phone_Personal_Cellular" Type="String" />
                    <asp:Parameter Name="Union_AffiliationID" Type="Int32" />
                    <asp:Parameter Name="Purchase_Card_Number" Type="String" />
                    <asp:Parameter Name="MonthlyDollarLimit" Type="Decimal" />
                    <asp:Parameter Name="SingleDollarLimit" Type="Decimal" />
                    <asp:Parameter Name="TCPA_StatusID" Type="Int32" />
                    <asp:Parameter Name="DPCC_StatusID" Type="Int32" />
                    <asp:Parameter Name="Purchase_Card_Reviewer" Type="Int32" />
                    <asp:Parameter Name="Purchase_Card_Approver" Type="Int32" />
                    <asp:Parameter Name="OpCode" Type="String" />
                    <asp:Parameter Name="tblEmployeeID" Type="Int32" Direction="Output" />
                    <asp:Parameter Name="InstitutionalKnowledge" Type="Int32" />
                    <asp:Parameter Name="InstitutionalKnowledgeDescription" Type="String" />
                    <asp:Parameter Name="EmailAddress" Type="String" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Status" Type="String" />
                    <asp:Parameter Name="Department_Name" Type="String" />
                    <asp:Parameter Name="Reporting_FacilityID" Type="String" />
                    <asp:Parameter Name="EmployeeID" Type="String" />
                    <asp:Parameter Name="SS_Last_4_Places" Type="String" />
                    <asp:Parameter Name="Last_Name" Type="String" />
                    <asp:Parameter Name="First_Name" Type="String" />
                    <asp:Parameter Name="Middle_Name" Type="String" />
                    <asp:Parameter Name="Gender" Type="String" />
                    <asp:Parameter Name="Date_Hired" Type="DateTime" />
                    <asp:Parameter Name="Seniority_Date" Type="DateTime" />
                    <asp:Parameter Name="Seniority_Ranking" Type="Double" />
                    <asp:Parameter Name="Inactive_Date" Type="DateTime" />
                    <asp:Parameter Name="Reason_For_DepartureID" Type="Int32" />
                    <asp:Parameter Name="Reports_To" Type="String" />
                    <asp:Parameter Name="License_Water_Treatment" Type="String" />
                    <asp:Parameter Name="T_License" Type="String" />
                    <asp:Parameter Name="Date_of_T_License" Type="DateTime" />
                    <asp:Parameter Name="License_Water_Distribution" Type="String" />
                    <asp:Parameter Name="W_License" Type="String" />
                    <asp:Parameter Name="Date_of_W_License" Type="DateTime" />
                    <asp:Parameter Name="License_Sewer_Collection" Type="String" />
                    <asp:Parameter Name="C_License" Type="String" />
                    <asp:Parameter Name="Date_of_C_License" Type="DateTime" />
                    <asp:Parameter Name="License_Sewer_Treatment" Type="String" />
                    <asp:Parameter Name="S_License" Type="String" />
                    <asp:Parameter Name="Date_of_S_License" Type="DateTime" />
                    <asp:Parameter Name="License_Industrial_Discharge" Type="String" />
                    <asp:Parameter Name="N_License" Type="String" />
                    <asp:Parameter Name="Date_of_N_License" Type="DateTime" />
                    <asp:Parameter Name="CDL" Type="String" />
                    <asp:Parameter Name="Drivers_License" Type="String" />
                    <asp:Parameter Name="EmergencyContactName" Type="String" />
                    <asp:Parameter Name="EmergencyContactPhone" Type="String" />
                    <asp:Parameter Name="CoordinateID" Type="Int32" />
                    <asp:Parameter Name="Address" Type="String" />
                    <asp:Parameter Name="City" Type="String" />
                    <asp:Parameter Name="State" Type="String" />
                    <asp:Parameter Name="Zip_Code" Type="String" />
                    <asp:Parameter Name="Phone_Pager" Type="String" />
                    <asp:Parameter Name="Phone_Cellular" Type="String" />
                    <asp:Parameter Name="Phone_Home" Type="String" />
                    <asp:Parameter Name="Phone_Work" Type="String" />
                    <asp:Parameter Name="Phone_Personal_Cellular" Type="String" />
                    <asp:Parameter Name="Union_AffiliationID" Type="Int32" />
                    <asp:Parameter Name="Purchase_Card_Number" Type="String" />
                    <asp:Parameter Name="MonthlyDollarLimit" Type="Decimal" />
                    <asp:Parameter Name="SingleDollarLimit" Type="Decimal" />
                    <asp:Parameter Name="TCPA_StatusID" Type="Int32" />
                    <asp:Parameter Name="DPCC_StatusID" Type="Int32" />
                    <asp:Parameter Name="Purchase_Card_Reviewer" Type="Int32" />
                    <asp:Parameter Name="Purchase_Card_Approver" Type="Int32" />
                    <asp:Parameter Name="OpCode" Type="String" />
                    <asp:Parameter Name="tblEmployeeID" Type="Int32" />
                    <asp:Parameter Name="InstitutionalKnowledge" Type="Int32" />
                    <asp:Parameter Name="InstitutionalKnowledgeDescription" Type="String" />
                    <asp:Parameter Name="EmailAddress" Type="String" />
                </UpdateParameters>
                <DeleteParameters>
                    <asp:Parameter Name="tblEmployeeID" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="tblEmployeeID" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
