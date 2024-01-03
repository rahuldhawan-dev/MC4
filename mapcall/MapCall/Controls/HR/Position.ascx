<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Position.ascx.cs" Inherits="MapCall.Controls.HR.Position" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlLicenseType.ascx" TagName="ddlLicenseType" TagPrefix="mmsi" %>
<asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="PositionID" DataSourceID="SqlDataSource1"
    Width="100%" CssClass="prettyTable"
    OnDataBound="DetailsView1_DataBound" 
    EnableViewState="true"
>
    <Fields>
        <asp:BoundField DataField="PositionID" HeaderText="PositionID" InsertVisible="False" ReadOnly="True" SortExpression="PositionID" />
        <asp:BoundField DataField="OpCode" HeaderText="OpCode (from local)" SortExpression="OpCode" InsertVisible="false" ReadOnly="true" />
        <asp:BoundField DataField="Position" HeaderText="Position" SortExpression="Position" />
        <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
        <asp:TemplateField HeaderText="Local" SortExpression="Local">
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlLocal" 
                    DataSourceID="dsLocal" 
                    AppendDataBoundItems="true"
                    DataTextField="Name"
                    DataValueField="Id"
                    SelectedValue='<%# Bind("LocalID") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value="0"></asp:ListItem>
                </asp:DropDownList>                    
                
                <asp:SqlDataSource runat="server" ID="dsLocal"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT Id, Name from LocalBargainingUnits order by 1"
                    >
                </asp:SqlDataSource> 
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:DropDownList runat="server" ID="ddlLocal" 
                    DataSourceID="dsLocal" 
                    AppendDataBoundItems="true"
                    DataTextField="Name"
                    DataValueField="Id"
                    SelectedValue='<%# Bind("LocalID") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>                    
                
                <asp:SqlDataSource runat="server" ID="dsLocal"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT * from LocalBargainingUnits order by 1"
                    >
                </asp:SqlDataSource> 
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Local") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="EEO_Job_Code" HeaderText="EEO_Job_Code" SortExpression="EEO_Job_Code" />
        <asp:BoundField DataField="EEO_Job_Description" HeaderText="EEO_Job_Description" SortExpression="EEO_Job_Description" />
        <asp:TemplateField HeaderText="FLSA_Status">
            <ItemTemplate><%# Eval("FLSA_Status")%></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlFLSAStatus"
                    DataSourceID="dsFLSAStatus"
                    DataTextField="LookupValue"
                    DataValueField="LookupID"
                    SelectedValue='<%# Bind("FLSAStatus")%>'
                    AppendDataBoundItems="true"
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="dsFLSAStatus"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT LookupId, LookupValue from Lookup where LookupType='FLSAstatus' order by 2"
                />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
        <asp:TemplateField HeaderText="Common Name">
            <ItemTemplate><%#Eval("JobTitleCommonName") %></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlCommon_Name" 
                    DataSourceID="dsCommon_Name" 
                    AppendDataBoundItems="true"
                    DataValueField="JobTitleCommonNameID"
                    DataTextField="Description"
                    SelectedValue='<%#Bind("Common_Name") %>'>
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>                    
                    
                <asp:SqlDataSource runat="server" ID="dsCommon_Name"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT JobTitleCommonNameID, Description from JobTitleCommonNames order by 2"
                    >
                </asp:SqlDataSource>  
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="License_Requirement_Attainment" SortExpression="License_Requirement_Attainment">
            <ItemTemplate><asp:Label runat="server" ID="lblLicenseRequirementAttainment" Text='<%# Bind("License_Requirement_Attainment_Text") %>'></asp:Label></ItemTemplate>
            <InsertItemTemplate><mmsi:ddlLicenseType runat="server" id="ddlLicenseType" SelectedValue='<%# Bind("License_Requirement_Attainment") %>'></mmsi:ddlLicenseType></InsertItemTemplate>
            <EditItemTemplate><mmsi:ddlLicenseType runat="server" id="ddlLicenseType" SelectedValue='<%# Bind("License_Requirement_Attainment") %>'></mmsi:ddlLicenseType></EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="License_Requirement_Entrance" SortExpression="License_Requirement_Entrance">
            <ItemTemplate><asp:Label runat="server" ID="lblLicenseRequirementEntrance" Text='<%# Bind("License_Requirement_Entrance_Text") %>'></asp:Label></ItemTemplate>
            <InsertItemTemplate><mmsi:ddlLicenseType runat="server" id="ddlLicenseTypeA" SelectedValue='<%# Bind("License_Requirement_Entrance") %>'></mmsi:ddlLicenseType></InsertItemTemplate>
            <EditItemTemplate><mmsi:ddlLicenseType runat="server" id="ddlLicenseTypeA" SelectedValue='<%# Bind("License_Requirement_Entrance") %>'></mmsi:ddlLicenseType></EditItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Estimated_Training_Qualification_Days" HeaderText="Estimated_Training_Qualification_Days" />
        <asp:BoundField DataField="Authorized_Staffing_Level" HeaderText="Authorized_Staffing_Level" />
        <asp:TemplateField HeaderText="Business Unit">
            <ItemTemplate><%#Eval("BU") %></ItemTemplate> 
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlBusinessUnit"
                    DataSourceID="dsBusinessUnit"
                    DataTextField="BU"
                    DataValueField="BusinessUnitID"
                    SelectedValue='<%# Bind("BusinessUnitID")%>'
                    AppendDataBoundItems="true"
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="dsBusinessUnit"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT BusinessUnitID, BU from BusinessUnits order by BU"
                />
            </EditItemTemplate>       
        </asp:TemplateField>
        <asp:CheckBoxField DataField="EssentialPosition" HeaderText="Essential Position" />
        <asp:TemplateField HeaderText="Emergency Response Priority">
            <ItemTemplate><%# Eval("EmergencyResponsePriorityText")%></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlEmergencyResponsePriority"
                    DataSourceID="dsEmergencyResponsePriority"
                    DataTextField="LookupValue"
                    DataValueField="LookupID"
                    SelectedValue='<%# Bind("EmergencyResponsePriority")%>'
                    AppendDataBoundItems="true"
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="dsEmergencyResponsePriority"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="SELECT LookupId, LookupValue from Lookup where LookupType='EmergencyResponsePriority' order by 2"
                />
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
    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
    DeleteCommand="DELETE FROM [tblPositions_Classifications] WHERE [PositionID] = @PositionID"
    InsertCommand="INSERT INTO [tblPositions_Classifications] ([Local_ID], FLSAStatus, [LocalID], [Category], [OpCenter], [Position], [EEO_Job_Code], [EEO_Job_Description], [Department], [Common_Name], [License_Requirement_Attainment], [License_Requirement_Entrance], Estimated_Training_Qualification_Days, Authorized_Staffing_Level, BusinessUnitID, EssentialPosition, EmergencyResponsePriority) VALUES (@Local_ID, @FLSAStatus, @LocalID, @Category, @OpCenter, @Position, @EEO_Job_Code, @EEO_Job_Description, @Department, @Common_Name, @License_Requirement_Attainment, @License_Requirement_Entrance, @Estimated_Training_Qualification_Days, @Authorized_Staffing_Level, @BusinessUnitID, @EssentialPosition, @EmergencyResponsePriority);Select @PositionID=@@IDENTITY"
    SelectCommand="
        SELECT [PositionID], OperatingCenters.OperatingCenterCode as [OpCode], [Local_ID], isNull([tblPositions_Classifications].[LocalID], 0) as LocalID, LocalBargainingUnits.Name as [Local], [Category], [OpCenter], [Position], [EEO_Job_Code], [EEO_Job_Description], tblPositions_Classifications.[Department], [Common_Name], 
        #1.lookupvalue as [License_Requirement_Attainment_Text], #2.lookupvalue as [License_Requirement_Entrance_Text], 
        #1.lookupid as [License_Requirement_Attainment], #2.lookupid as [License_Requirement_Entrance], 
        #3.lookupid as FLSAStatus, #3.lookupvalue as FLSA_Status, Estimated_Training_Qualification_Days, Authorized_Staffing_Level, 
        #4.BU as BU, #4.BusinessUnitID, #5.LookupValue as EmergencyResponsePriorityText, EmergencyResponsePriority, EssentialPosition, JT.Description as JobTitleCommonName
            FROM [tblPositions_Classifications] 
            LEFT JOIN LocalBargainingUnits ON LocalBargainingUnits.Id = tblPositions_Classifications.LocalID 
            LEFT JOIN OperatingCenters on LocalBargainingUnits.OperatingCenterId = OperatingCenters.OperatingCenterID
            LEFT JOIN Lookup #1 on #1.LookupID = License_Requirement_Attainment
            LEFT JOIN Lookup #2 on #2.LookupID = License_Requirement_Entrance
            LEFT JOIN Lookup #3 on #3.LookupID = FLSAStatus
            LEFT JOIN BusinessUnits #4 on #4.BusinessUnitID = tblPositions_Classifications.BusinessUnitID
            LEFT JOIN Lookup #5 on #5.LookupID = EmergencyResponsePriority
            LEFT JOIN JobTitleCommonNames JT ON JT.JobTitleCommonNameID = Common_Name
            WHERE [PositionID] = @PositionID
    "
    UpdateCommand="UPDATE [tblPositions_Classifications] SET [Local_ID] = @Local_ID, [FLSAStatus]=@FLSAstatus, [LocalID] = @LocalID, [Category] = @Category, [OpCenter] = @OpCenter, [Position] = @Position, [EEO_Job_Code] = @EEO_Job_Code, [EEO_Job_Description] = @EEO_Job_Description, [Department] = @Department, [Common_Name] = @Common_Name, License_Requirement_Attainment = @License_Requirement_Attainment, License_Requirement_Entrance = @License_Requirement_Entrance, Estimated_Training_Qualification_Days = @Estimated_Training_Qualification_Days, Authorized_Staffing_Level = @Authorized_Staffing_Level, [BusinessUnitID] = @BusinessUnitID, [EssentialPosition] = @EssentialPosition, [EmergencyResponsePriority] = @EmergencyResponsePriority WHERE [PositionID] = @PositionID"
    OnInserted="SqlDataSource1_Inserted"
    OnDeleted="SqlDataSource1_Deleted"
    OnUpdated="SqlDataSource1_Updated"
    >
    <InsertParameters>
        <%--<asp:Parameter Name="OpCode" Type="String" />--%>
        <asp:Parameter Name="Local_ID" Type="String" />
        <asp:Parameter Name="LocalID" Type="Int32" />
        <asp:Parameter Name="FLSAStatus" Type="Int32" />
        <asp:Parameter Name="Category" Type="String" />
        <asp:Parameter Name="OpCenter" Type="String" />
        <asp:Parameter Name="Position" Type="String" />
        <asp:Parameter Name="EEO_Job_Code" Type="String" />
        <asp:Parameter Name="EEO_Job_Description" Type="String" />
        <asp:Parameter Name="Department" Type="String" />
        <asp:Parameter Name="Common_Name" Type="Int32" />
        <asp:Parameter Name="PositionID" Type="Int32" Direction="Output" />
        <asp:Parameter Name="License_Requirement_Attainment" Type="Int32" />
        <asp:Parameter Name="License_Requirement_Entrance" Type="Int32" />        
        <asp:Parameter Name="Estimated_Training_Qualification_Days" Type="Double" />
        <asp:Parameter Name="Authorized_Staffing_Level" Type="Double" />
        <asp:Parameter Name="BusinessUnitID" Type="Int32" />
        <asp:Parameter Name="EssentialPosition" Type="Boolean" />
        <asp:Parameter Name="EmergencyResponsePriority" Type="Int32" />        
    </InsertParameters>
    <UpdateParameters>
        <%--<asp:Parameter Name="OpCode" Type="String" />--%>
        <asp:Parameter Name="Local_ID" Type="String" />
        <asp:Parameter Name="FLSAStatus" Type="Int32" />
        <asp:Parameter Name="LocalID" Type="Int32" />
        <asp:Parameter Name="Category" Type="String" />
        <asp:Parameter Name="OpCenter" Type="String" />
        <asp:Parameter Name="Position" Type="String" />
        <asp:Parameter Name="EEO_Job_Code" Type="String" />
        <asp:Parameter Name="EEO_Job_Description" Type="String" />
        <asp:Parameter Name="Department" Type="String" />
        <asp:Parameter Name="Common_Name" Type="Int32" />
        <asp:Parameter Name="PositionID" Type="Int32" />
        <asp:Parameter Name="License_Requirement_Attainment" Type="Int32" />
        <asp:Parameter Name="License_Requirement_Entrance" Type="Int32" />
        <asp:Parameter Name="Estimated_Training_Qualification_Days" Type="Double" />
        <asp:Parameter Name="Authorized_Staffing_Level" Type="Double" />
        <asp:Parameter Name="BusinessUnitID" Type="Int32" />        
        <asp:Parameter Name="EssentialPosition" Type="Boolean" />
        <asp:Parameter Name="EmergencyResponsePriority" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="PositionID" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter Name="PositionID" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
