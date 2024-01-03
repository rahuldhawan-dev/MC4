<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PositionHistory.ascx.cs" Inherits="MapCall.Controls.HR.PositionHistory" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlPositions.ascx" TagName="ddlPositions" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlFacility.ascx" TagName="ddlFacility" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlScheduleType.ascx" TagName="ddlScheduleType" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="Position_History_ID" DataSourceID="SqlDataSource1"
    Width="100%"
    OnDataBound="DetailsView1_DataBound" 
    FieldHeaderStyle-Width="100px"

>
    <Fields>
        <asp:BoundField DataField="Position_History_ID" HeaderText="Position_History_ID" InsertVisible="False" ReadOnly="True" SortExpression="Position_History_ID" />
        <asp:TemplateField HeaderText="OpCode" SortExpression="OpCode">
            <ItemTemplate>
                <asp:GridView runat="server" ID="gvOpCode" Visible="false"
                    DataSourceID="dsOpCodeSelected" AutoGenerateColumns="true" ShowHeader="false" GridLines="None" RowStyle-HorizontalAlign="Left" ForeColor="Black"
                />
                <%# Eval("OpCode") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:ListBox runat="server" ID="lbOpCode"
                    DataSourceID="dsOpCode"
                    DataValueField="OpCode"
                    AppendDataBoundItems="true"
                    Rows="11"
                    Visible="false" SelectionMode="multiple" 
                />
                <asp:Label runat="server" ID="lblOpCode" Text='<%# Bind("OpCode") %>'></asp:Label>
            </EditItemTemplate>
            <InsertItemTemplate>Set this after you create the position history record.</InsertItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Local" SortExpression="Local" DataField="Local" ReadOnly="true" HeaderStyle-Font-Italic="true" InsertVisible="false" />
        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ReadOnly="true" HeaderStyle-Font-Italic="true" InsertVisible="false" />
        <asp:TemplateField HeaderText="Position_Category" SortExpression="Position_Category" HeaderStyle-Font-Italic="true" InsertVisible="false" >
            <ItemTemplate><%# Eval("Category") %></ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status_Change_Reason" SortExpression="Status_Change_Reason" HeaderStyle-Font-Bold="true">
            <ItemTemplate>
                <%# Eval("Status_Change_Reason") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlStatus_Change_Reason" 
                    DataSourceID="dsStatus_Change_Reason" 
                    AppendDataBoundItems="true"
                    DataTextField="LookupValue"
                    SelectedValue='<%# Bind("Status_Change_Reason") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>                    
                <asp:SqlDataSource runat="server" ID="dsStatus_Change_Reason"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="select LookupValue from Lookup where Lookuptype = 'Status_Change_Reason' order by 1"
                    >
                </asp:SqlDataSource> 
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlStatus_Change_Reason" InitialValue="" Text="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </EditItemTemplate>
        </asp:TemplateField>        
        <asp:BoundField DataField="Department" HeaderText="Department (from Position)" SortExpression="Department" ReadOnly="true" HeaderStyle-Font-Italic="true" InsertVisible="false" />
        
        <asp:TemplateField HeaderText="Department" SortExpression="Department" >
            <ItemTemplate>
                <%# Eval("DepartmentNameText") %>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlDepartmentName" 
                    DataSourceID="dsDepartmentName" 
                    AppendDataBoundItems="true"
                    DataTextField="LookupValue"
                    DataValueField="LookupID"
                    SelectedValue='<%# Bind("DepartmentName") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="dsDepartmentName"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'DepartmentName' and tableName='tblPosition_History' order by 2"
                    >
                </asp:SqlDataSource> 
                <asp:RequiredFieldValidator runat="server" ID="rfvDepartmentName" ControlToValidate="ddlDepartmentName" InitialValue="" Text="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </EditItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="EmployeeID" SortExpression="EmployeeID" HeaderStyle-Font-Bold="true">
            <ItemTemplate>
                <%# Eval("EmployeeID") %>
            </ItemTemplate>
            <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="ddlEmployees" 
                        DataSourceID="dsEmployees" 
                        AppendDataBoundItems="true"
                        DataValueField="tblEmployeeID"
                        DataTextField="fullname"
                        SelectedValue='<%# Bind("tblEmployeeID") %>'
                        Width="50%"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    <asp:SqlDataSource runat="server" ID="dsEmployees"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT distinct tblEmployeeID, replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') as [FullName], Last_Name from tblEmployee order by Last_Name"
                        >
                    </asp:SqlDataSource>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlEmployees" InitialValue="" Text="Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </EditItemTemplate>
        </asp:TemplateField>        
        <asp:BoundField DataField="Last_Name" HeaderText="Last_Name" SortExpression="Last_Name" ReadOnly="true" HeaderStyle-Font-Italic="true" InsertVisible="false" />
        <asp:TemplateField HeaderText="Position_Posting_ID" SortExpression="Position_Posting_ID">
            <ItemTemplate>
                <%# Eval("Position_Posting_ID") %>
            </ItemTemplate>
            <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="ddlPosition_Posting_ID" 
                        DataSourceID="dsPosition_Posting_ID" 
                        AppendDataBoundItems="true"
                        DataValueField="Position_Posting_ID"
                        DataTextField="PositionText"
                        SelectedValue='<%# Bind("Position_Posting_ID") %>'
                        Width="50%"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsPosition_Posting_ID"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="
                              SELECT 
	                            Position_Posting_ID, 
	                            cast(Position_Posting_ID as varchar(15)) + ' - ' + isNull(convert(varchar(12), Date_of_Posting, 101),'') + ' - ' +  isNull(opCode, '')+ ' - ' + isNull(tblPositions_Classifications.Position, '') as [PositionText]
	                            from tblPosition_Postings
	                            left join tblPositions_Classifications on tblPositions_Classifications.PositionID = Position_ID order by Date_of_Posting desc
	                        "
                        >
                    </asp:SqlDataSource>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Position" SortExpression="Position" HeaderStyle-Font-Bold="true">
            <EditItemTemplate>
                <mmsi:ddlPositions ID="ddlPositions" runat="server" SelectedValue='<%# Bind("Position_ID") %>' Required="true" />
            </EditItemTemplate>
            <ItemTemplate>
                <%# Eval("Position") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Position_Sub_Category" HeaderText="Position_Sub_Category" SortExpression="Position_Sub_Category" />
        <asp:BoundField DataField="Vacation_Grouping" HeaderText="Vacation_Grouping" SortExpression="Vacation_Grouping" />
        <mmsinc:BoundField HeaderStyle-Font-Bold="true" SqlDataType="DateTime" DataFormatString="{0:d}" Required="True" HtmlEncode="False" DataField="Position_Start_Date" HeaderText="Position_Start_Date" SortExpression="Position_Start_Date" />
        <mmsinc:BoundField SqlDataType="DateTime" DataFormatString="{0:d}" HtmlEncode="False" DataField="Position_End_Date" HeaderText="Position_End_Date" SortExpression="Position_End_Date" />
        <asp:TemplateField HeaderText="ReportingFacilityID" SortExpression="ReportingFacilityID">
            <ItemTemplate><%# Eval("FacilityName") %></ItemTemplate>
            <EditItemTemplate><mmsi:ddlFacility ID="ddlFacility" runat="server" SelectedValue='<%# Bind("ReportingFacilityID") %>' /></EditItemTemplate>
            <InsertItemTemplate><mmsi:ddlFacility ID="ddlFacility" runat="server" SelectedValue='<%# Bind("ReportingFacilityID") %>' /></InsertItemTemplate>
        </asp:TemplateField>
 
        <asp:BoundField HeaderText="License_Requirement_Attainment" SortExpression="License_Requirement_Attainment" DataField="License_Requirement_Attainment" ReadOnly="true" HeaderStyle-Font-Italic="true" InsertVisible="false" />
        <asp:CheckBoxField DataField="Fully_Qualified" HeaderText="Fully_Qualified" SortExpression="Fully_Qualified" />
        <asp:TemplateField HeaderText="Schedule" >
            <ItemTemplate>
                <asp:Label runat="server" ID="hlSchedule" Text='<%# Bind("ScheduleType") %>' style="cursor:pointer;text-decoration:underline;color:Navy;"></asp:Label>
                <cc1:PopupControlExtender ID="PopupControlExtender1" runat="server"
                    TargetControlID="hlSchedule"
                    PopupControlID="panelschedule"
                    Position="Top"
                >
                </cc1:PopupControlExtender>
                <asp:Panel id="panelschedule" runat="server" style="display:none;">
                    <iframe src='<%# ScheduleURL(Eval("ScheduleTypeID").ToString()) %>' style="border:0px;width:225px;"></iframe>
                </asp:Panel>
                <asp:Label Visible="false" ID="lblSchedule" runat="server" Text='<%# Bind("ScheduleType") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate><mmsi:ddlScheduleType ID="ddlSchedule" runat="server" SelectedValue='<%# Bind("ScheduleTypeID") %>' /></EditItemTemplate>
            <InsertItemTemplate><mmsi:ddlScheduleType ID="ddlSchedule" runat="server" SelectedValue='<%# Bind("ScheduleTypeID") %>' /></InsertItemTemplate>
        </asp:TemplateField>
        <asp:CheckBoxField HeaderText="On Call Requirement" DataField="On_Call_Requirement" />

        <asp:TemplateField HeaderText="Fixed_Shift_Status" SortExpression="Fixed_Shift_Status" >
            <ItemTemplate><%# Eval("Fixed_Shift_Status_text") %></ItemTemplate>
            <EditItemTemplate>
				<asp:DropDownList runat="server" ID="ddlFixed_Shift_Status"
				DataSourceID="dsFixed_Shift_Status"
				AppendDataBoundItems="true"
				DataValueField="LookupID"
				DataTextField="LookupValue"
				SelectedValue='<%# Bind("Fixed_Shift_StatusID") %>'
				>
				<asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
				</asp:DropDownList>

				<asp:SqlDataSource runat="server" ID="dsFixed_Shift_Status"
				ConnectionString="<%$ ConnectionStrings:MCProd %>"
				SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'Fixed_Shift_Status' order by 2"
				>
				</asp:SqlDataSource>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Fixed_Shift_Status_Sequence" HeaderText="Fixed_Shift_Status_Sequence" />
        <asp:BoundField DataField="Rotating_Shift_Grouping" HeaderText="Rotating_Shift_Grouping" />
        <asp:TemplateField HeaderText="Labor Category">
            <ItemTemplate>
                <%# Eval("LaborCategoryTypeDescription")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlLaborCategoryTypeID" 
                    DataSourceID="dsLaborCategoryTypeID" 
                    AppendDataBoundItems="true"
                    DataTextField="LookupValue"
                    DataValueField="LookupID"
                    SelectedValue='<%# Bind("LaborCategoryTypeID") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="dsLaborCategoryTypeID"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'LaborCategory' and tableName='tblPosition_History' order by 2"
                    >
                </asp:SqlDataSource> 
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Employment Agency">
            <ItemTemplate>
                <%# Eval("EmploymentAgencyTypeDescription")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlEmploymentAgencyTypeID" 
                    DataSourceID="dsEmploymentAgencyTypeID" 
                    AppendDataBoundItems="true"
                    DataTextField="LookupValue"
                    DataValueField="LookupID"
                    SelectedValue='<%# Bind("EmploymentAgencyTypeID") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="dsEmploymentAgencyTypeID"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'EmploymentAgency' and tableName='tblPosition_History' order by 2"
                    >
                </asp:SqlDataSource> 
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Employment Level">
            <ItemTemplate>
                <%# Eval("EmploymentLevelTypeDescription")%>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlEmploymentLevelTypeID" 
                    DataSourceID="dsEmploymentLevelTypeID" 
                    AppendDataBoundItems="true"
                    DataTextField="LookupValue"
                    DataValueField="LookupID"
                    SelectedValue='<%# Bind("EmploymentLevelTypeID") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="dsEmploymentLevelTypeID"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'EmploymentLevel' and tableName='tblPosition_History' order by 2"
                    >
                </asp:SqlDataSource> 
            </EditItemTemplate>
        </asp:TemplateField>
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
                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure?');"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Fields>
</asp:DetailsView>

<asp:Label runat="server" ID="lblResults"></asp:Label>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
    DeleteCommand="DELETE FROM [tblPosition_History] WHERE [Position_History_ID] = @Position_History_ID"
    InsertCommand="INSERT INTO [tblPosition_History] ([Status_Change_Reason], [tblEmployeeID], [Position_Posting_ID], [Position_ID], [Position_Sub_Category], [Vacation_Grouping], [Position_Start_Date], [Position_End_Date], [ReportingFacilityID], [Fully_Qualified], [ScheduleTypeID], [On_Call_Requirement], [Fixed_Shift_StatusID], [Fixed_Shift_Status_Sequence], [Rotating_Shift_Grouping], [DepartmentName], [LaborCategoryTypeID], [EmploymentAgencyTypeID], [EmploymentLevelTypeID]) VALUES (@Status_Change_Reason, @tblEmployeeID, @Position_Posting_ID, @Position_ID, @Position_Sub_Category, @Vacation_Grouping, @Position_Start_Date, @Position_End_Date, @ReportingFacilityID, @Fully_Qualified, @ScheduleTypeID, @On_Call_Requirement, @Fixed_Shift_StatusID, @Fixed_Shift_Status_Sequence, @Rotating_Shift_Grouping, @DepartmentName, @LaborCategoryTypeID, @EmploymentAgencyTypeID, @EmploymentLevelTypeID);Select @Position_History_ID=@@IDENTITY"
    SelectCommand="
        SELECT 
            tblPosition_History.tblEmployeeID, [Position_History_ID], [oc].operatingCenterCode as [OpCode],
            s.[Description] as Status, 
            [tblPositions_classifications].[Category], [Status_Change_Reason], Department,
            tblEmployee.[EmployeeID], [Last_Name], [Position_Posting_ID], [Position_ID], 
            [tblPositions_classifications].[Position], [Position_Sub_Category], [Vacation_Grouping], 
            [Position_Start_Date], [Position_End_Date], 
            '[' + ocf.OperatingCenterCode + '-' + cast(recordId as varchar) + '] - ' + isNull(facilityName,'') as [FacilityName], 
            [tblPosition_History].ReportingFacilityId,
            Lookup.LookupValue as [License_Requirement_Attainment],  
            [Fully_Qualified], LocalBargainingUnits.Name as Local, [tblPosition_History].ScheduleTypeID, [ScheduleType].ScheduleType, 
            On_Call_Requirement, tblPosition_History.Fixed_Shift_StatusID, tblPosition_History.Fixed_Shift_Status_Sequence, 
            tblPosition_History.Rotating_Shift_Grouping, #Fixed_Shift_Status.LookupValue as Fixed_Shift_Status_text,
            DepartmentName,L2.LookupValue as DepartmentNameText,
            LaborCategoryTypeID,
            lookupLaborCategoryTypeID.LookupValue as LaborCategoryTypeDescription,
            EmploymentAgencyTypeID,
            lookupEmploymentAgencyTypeID.LookupValue as EmploymentAgencyTypeDescription,
            EmploymentLevelTypeID,
            lookupEmploymentLevelTypeID.LookupValue as EmploymentLevelTypeDescription
            
           FROM [tblPosition_History] 
            LEFT JOIN [tblPositions_classifications]         ON [tblPositions_classifications].positionID = [tblPosition_History].position_id
            LEFT JOIN [LocalBargainingUnits]                 ON [LocalBargainingUnits].Id = tblPositions_Classifications.LocalID
            LEFT JOIN [tblEmployee]                          ON tblEmployee.tblemployeeID = [tblPosition_History].tblemployeeID
            LEFT JOIN [EmployeeStatuses] s                   ON s.Id = [tblEmployee].StatusId
            LEFT JOIN [ScheduleType]                         ON [ScheduleType].ScheduleTypeID = [tblPosition_History].ScheduleTypeID
            LEFT JOIN [Lookup]                               ON [Lookup].LookupID = [tblPositions_classifications].[License_Requirement_Attainment]
            LEFT JOIN [Lookup] #Fixed_Shift_Status           ON #Fixed_Shift_Status.LookupID = tblPosition_History.Fixed_Shift_StatusID
            LEFT JOIN [Lookup] L2                            ON L2.LookupID = DepartmentName
            LEFT JOIN [OperatingCenters] oc                  ON oc.OperatingCenterID = LocalBargainingUnits.OperatingCenterId
            LEFT JOIN [Lookup] lookupLaborCategoryTypeID     ON lookupLaborCategoryTypeID.LookupID = [tblPosition_History].[LaborCategoryTypeID]
            LEFT JOIN [Lookup] lookupEmploymentAgencyTypeID  ON lookupEmploymentAgencyTypeID.LookupID = [tblPosition_History].[EmploymentAgencyTypeID]
            LEFT JOIN [Lookup] lookupEmploymentLevelTypeID   ON lookupEmploymentLevelTypeID.LookupID = [tblPosition_History].[EmploymentLevelTypeID]
            LEFT JOIN [tblFacilities] F ON F.recordId = tblPosition_History.ReportingFacilityId
            LEFT JOIN OperatingCenters ocf on ocf.OperatingCenterID = F.operatingCenterId
            WHERE [Position_History_ID] = @Position_History_ID
        "
    UpdateCommand="UPDATE [tblPosition_History] SET [Status_Change_Reason] = @Status_Change_Reason, [tblEmployeeID] = @tblEmployeeID, [Position_Posting_ID] = @Position_Posting_ID, [Position_ID] = @Position_ID, [Position_Sub_Category] = @Position_Sub_Category, [Vacation_Grouping] = @Vacation_Grouping, [Position_Start_Date] = @Position_Start_Date, [Position_End_Date] = @Position_End_Date, [ReportingFacilityID] = @ReportingFacilityID, [Fully_Qualified] = @Fully_Qualified, ScheduleTypeID=@ScheduleTypeID, [On_Call_Requirement] = @On_Call_Requirement, [Fixed_Shift_StatusID] = @Fixed_Shift_StatusID, [Fixed_Shift_Status_Sequence] = @Fixed_Shift_Status_Sequence, [Rotating_Shift_Grouping] = @Rotating_Shift_Grouping, [DepartmentName] = @DepartmentName, [LaborCategoryTypeID] = @LaborCategoryTypeID, [EmploymentAgencyTypeID] = @EmploymentAgencyTypeID, [EmploymentLevelTypeID] = @EmploymentLevelTypeID WHERE [Position_History_ID] = @Position_History_ID"
    OnInserted="SqlDataSource1_Inserted"
    OnDeleted="SqlDataSource1_Deleted"
    OnUpdated="SqlDataSource1_Updated" 
    >    
    <InsertParameters>
        <asp:Parameter Name="Status_Change_Reason" Type="String" />
        <asp:Parameter Name="tblEmployeeID" Type="Int32" />
        <asp:Parameter Name="Position_Posting_ID" Type="Double" />
        <asp:Parameter Name="Position_ID" Type="String" />
        <asp:Parameter Name="Position_Sub_Category" Type="String" />
        <asp:Parameter Name="Vacation_Grouping" Type="String" />
        <asp:Parameter Name="Position_Start_Date" Type="DateTime" />
        <asp:Parameter Name="Position_End_Date" Type="DateTime" />
        <asp:Parameter Name="ReportingFacilityID" Type="String" />
        <asp:Parameter Name="Fully_Qualified" Type="Boolean" />
        <asp:Parameter Name="Position_History_ID" Type="Int32" Direction="output" />
        <asp:Parameter Name="ScheduleTypeID" Type="Int32" />
        <asp:Parameter Name="On_Call_Requirement" Type="Boolean" />
        <asp:Parameter Name="Fixed_Shift_StatusID" Type="Int32" />
        <asp:Parameter Name="Fixed_Shift_Status_Sequence" Type="String" />
        <asp:Parameter Name="Rotating_Shift_Grouping" Type="String" />
        <asp:Parameter Name="DepartmentName" Type="Int32" />
        <asp:Parameter Name="LaborCategoryTypeID" Type="Int32" />
        <asp:Parameter Name="EmploymentAgencyTypeID" Type="Int32" />
        <asp:Parameter Name="EmploymentLevelTypeID" Type="Int32" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Status_Change_Reason" Type="String" />
        <asp:Parameter Name="tblEmployeeID" Type="Int32" />
        <asp:Parameter Name="Position_Posting_ID" Type="Double" />
        <asp:Parameter Name="Position_ID" Type="String" />
        <asp:Parameter Name="Position_Sub_Category" Type="String" />
        <asp:Parameter Name="Vacation_Grouping" Type="String" />
        <asp:Parameter Name="Position_Start_Date" Type="DateTime" />
        <asp:Parameter Name="Position_End_Date" Type="DateTime" />
        <asp:Parameter Name="ReportingFacilityID" Type="String" />
        <asp:Parameter Name="Fully_Qualified" Type="Boolean" />
        <asp:Parameter Name="Position_History_ID" Type="Int32" />
        <asp:Parameter Name="ScheduleTypeID" Type="Int32" />
        <asp:Parameter Name="On_Call_Requirement" Type="Boolean" />
        <asp:Parameter Name="Fixed_Shift_StatusID" Type="Int32" />
        <asp:Parameter Name="Fixed_Shift_Status_Sequence" Type="String" />
        <asp:Parameter Name="Rotating_Shift_Grouping" Type="String" />
        <asp:Parameter Name="DepartmentName" Type="Int32" />
        <asp:Parameter Name="LaborCategoryTypeID" Type="Int32" />
        <asp:Parameter Name="EmploymentAgencyTypeID" Type="Int32" />
        <asp:Parameter Name="EmploymentLevelTypeID" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="Position_History_ID" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter Name="Position_History_ID" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource runat="server" ID="dsOpCode"
    ConnectionString="<%$ ConnectionStrings:MCProd%>"
    SelectCommand="
        Select operatingCenterCode as OpCode from OperatingCenters where operatingCenterCode not in ('IL1','IL2')
    "
    />
<asp:SqlDataSource runat="server" ID="dsOpCodeSelected"
    ConnectionString="<%$ ConnectionStrings:MCProd%>"
    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
    SelectCommand="select opCode from tblPosition_History_Opcode where Position_History_ID = @Position_History_ID"
>
    <SelectParameters>
        <asp:ControlParameter ControlID="DetailsView1" Name="Position_History_ID" />
    </SelectParameters>
</asp:SqlDataSource>