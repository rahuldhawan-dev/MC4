<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Facility.ascx.cs" Inherits="MapCall.Controls.HR.Facility" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls.DropDowns" Assembly="MapCall" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

&nbsp;
<asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataSourceID="SqlDataSource1"
    Width="100%"
    OnDataBound="DetailsView1_DataBound" 
    HeaderStyle-Width="50%"
    FieldHeaderStyle-Width="50%" DataKeyNames="RecordId"
    >
    <Fields>
        <asp:BoundField DataField="RecordId" HeaderText="RecordId" InsertVisible="False" ReadOnly="True" SortExpression="RecordId" />
        <asp:BoundField DataField="CreatedAt" HeaderText="DateCreated" SortExpression="CreatedAt" />
        <asp:TemplateField HeaderText="Facility_Ownership" SortExpression="Facility_Ownership" >
            <ItemTemplate><asp:Label runat="server" ID="lblFacility_Ownership" Text='<%# Bind("Facility_Ownership_Text") %>' /></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlFacility_Ownership" 
                    DataSourceID="dsFacility_Ownership" 
                    AppendDataBoundItems="true"
                    DataValueField="LookupID"
                    DataTextField="LookupValue"
                    SelectedValue='<%# Bind("Facility_Ownership") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>                    

                <asp:SqlDataSource runat="server" ID="dsFacility_Ownership"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
                    SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'Facility_Ownership' order by 2"
                    >
                </asp:SqlDataSource>
            </EditItemTemplate>
        </asp:TemplateField>       
        
        
        <asp:TemplateField HeaderText="Status" SortExpression="Status">
            <ItemTemplate><asp:Label runat="server" ID="lblStatus" Text='<%# Bind("Status_Text") %>' /></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlStatus" 
                    DataSourceID="dsStatus" 
                    AppendDataBoundItems="true"
                    DataValueField="LookupID"
                    DataTextField="LookupValue"
                    SelectedValue='<%# Bind("Status") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>                    

                <asp:SqlDataSource runat="server" ID="dsStatus"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
                    SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'Facility_Status' order by 2"
                    >
                </asp:SqlDataSource>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Corporation" HeaderText="Corporation" SortExpression="Corporation" />
        <asp:BoundField DataField="Region" HeaderText="Region" SortExpression="Region" />
        <asp:BoundField DataField="CompanySubsidiary" HeaderText="CompanySubsidiary" SortExpression="CompanySubsidiary" />
        <asp:TemplateField HeaderText="Department">
            <ItemTemplate><%#Eval("Department") %></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlDepartment"
                    DataSourceID="dsDepartment"
                    AppendDataBoundItems="true"
                    DataValueField="DepartmentID"
                    DataTextField="Description"
                    SelectedValue='<%#Bind("DepartmentID") %>'>
                    <asp:ListItem Value="" Text="--Select Here--" />
                </asp:DropDownList>
                <asp:SqlDataSource runat="server" ID="dsDepartment"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
                    SelectCommand="Select distinct Description, DepartmentID from Departments order by 1" />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Operations" HeaderText="Operations" SortExpression="Operations" />
        <asp:BoundField DataField="District" HeaderText="District" SortExpression="District" />
        <asp:BoundField DataField="System" HeaderText="System" SortExpression="System" />
        <asp:BoundField DataField="PWSID" HeaderText="PWSID" SortExpression="PWSID" />
        <asp:TemplateField HeaderText="Operating Center">
            <ItemTemplate><%#Eval("OperatingCenterCode") %></ItemTemplate>
            <EditItemTemplate>
                <mapcall:DataSourceDropDownList ID="ddlOperatingCenters" runat="server" 
                    SelectedValue='<%#Bind("OperatingCenterID") %>'
                    TableName="OperatingCenters"
                    TextFieldName="OperatingCenterCode"
                    ValueFieldName="OperatingCenterID"
                />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Business Unit">
            <ItemTemplate><%#Eval("BU") %></ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" SortExpression="FacilityID" />
        <asp:BoundField DataField="FacilityName" HeaderText="FacilityName" SortExpression="FacilityName" />
        <asp:BoundField DataField="StreetNumber" HeaderText="StreetNumber" SortExpression="StreetNumber" />
        <asp:BoundField DataField="StreetPrefix" HeaderText="StreetPrefix" SortExpression="StreetPrefix" />
        <asp:BoundField DataField="StreetName" HeaderText="StreetName" SortExpression="StreetName" />
        <asp:BoundField DataField="StreetSuffix" HeaderText="StreetSuffix" SortExpression="StreetSuffix" />
        <asp:BoundField DataField="NearestCrossStreet" HeaderText="NearestCrossStreet" SortExpression="NearestCrossStreet" />
        
        <asp:TemplateField HeaderText="Town">
            <ItemTemplate><asp:Label runat="server" ID="lblTown" Text='<%# Bind("Town") %>' /></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlTowns" 
                    DataSourceID="dsTowns" 
                    AppendDataBoundItems="true"
                    DataValueField="TownID"
                    DataTextField="Town"
                    SelectedValue='<%# Bind("TownID") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>                    

                <asp:SqlDataSource runat="server" ID="dsTowns"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
                    SelectCommand="Select TownID, Town from Towns order by 2"
                    >
                </asp:SqlDataSource>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="TownSection" HeaderText="TownSection" SortExpression="TownSection" />
        <asp:BoundField DataField="County" HeaderText="County" SortExpression="County" />
        <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
        <asp:BoundField DataField="ZipCode" HeaderText="ZipCode" SortExpression="ZipCode" />
        <asp:BoundField DataField="Production_Capacity_Maximum_MGD" HeaderText="Production_Capacity_Maximum_MGD" SortExpression="Production_Capacity_Maximum_MGD" />
        <asp:BoundField DataField="GradientZoneServed" HeaderText="GradientZoneServed" SortExpression="GradientZoneServed" />
        <asp:BoundField DataField="CriticalRating" HeaderText="CriticalRating" SortExpression="CriticalRating" />
        <asp:BoundField DataField="JDEBULocationNumber" HeaderText="JDEBULocationNumber" SortExpression="JDEBULocationNumber" />
        <asp:BoundField DataField="JDEParentNumber" HeaderText="JDEParentNumber" SortExpression="JDEParentNumber" />
        <asp:BoundField DataField="JDEItemNumber" HeaderText="JDEItemNumber" SortExpression="JDEItemNumber" />
        <asp:BoundField DataField="YearInService" HeaderText="YearInService" SortExpression="YearInService" />
        <mmsinc:LatLonPickerField DataField="CoordinateID" />
        <asp:BoundField DataField="Elevation" HeaderText="Elevation" SortExpression="Elevation" />
        <asp:BoundField DataField="TaxDistrict" HeaderText="TaxDistrict" SortExpression="TaxDistrict" />
        <asp:BoundField DataField="Lot" HeaderText="Lot" SortExpression="Lot" />
        <asp:BoundField DataField="Block" HeaderText="Block" SortExpression="Block" />
        <asp:BoundField DataField="Qualifier" HeaderText="Qualifier" SortExpression="Qualifier" />
        <asp:BoundField DataField="SICNumber" HeaderText="SICNumber" SortExpression="SICNumber" />
        <asp:BoundField DataField="DEPIDNumber" HeaderText="DEPIDNumber" SortExpression="DEPIDNumber" />
        <asp:BoundField DataField="FederalTaxID" HeaderText="FederalTaxID" SortExpression="FederalTaxID" />
        <asp:BoundField DataField="WaterShed" HeaderText="WaterShed" SortExpression="WaterShed" />
        <asp:BoundField DataField="RegionalPlanningArea" HeaderText="RegionalPlanningArea" SortExpression="RegionalPlanningArea" />
        <asp:CheckBoxField DataField="PropertyOnly" HeaderText="PropertyOnly" SortExpression="PropertyOnly" />
        <asp:CheckBoxField DataField="Administration" HeaderText="Administration" SortExpression="Administration" />
        <asp:CheckBoxField DataField="EmergencyPower" HeaderText="EmergencyPower" SortExpression="EmergencyPower" />
        <asp:CheckBoxField DataField="Ground_Water_Supply" HeaderText="Ground_Water_Supply" SortExpression="Ground_Water_Supply" />
        <asp:CheckBoxField DataField="Surface_Water_Supply" HeaderText="Surface_Water_Supply" SortExpression="Surface_Water_Supply" />
        <asp:CheckBoxField DataField="Reservoir" HeaderText="Reservoir" SortExpression="Reservoir" />
        <asp:CheckBoxField DataField="Dam" HeaderText="Dam" SortExpression="Dam" />
        <asp:CheckBoxField DataField="Interconnection" HeaderText="Interconnection" SortExpression="Interconnection" />
        <asp:CheckBoxField DataField="Point_Of_Entry" HeaderText="Point_Of_Entry" SortExpression="Point_Of_Entry" />
        <asp:CheckBoxField DataField="WaterTreatmentFacility" HeaderText="WaterTreatmentFacility" SortExpression="WaterTreatmentFacility" />
        <asp:CheckBoxField DataField="ChemicalFeed" HeaderText="ChemicalFeed" SortExpression="ChemicalFeed" />
        <asp:CheckBoxField DataField="DPCC" HeaderText="DPCC" SortExpression="DPCC" />
        <asp:CheckBoxField DataField="PSMTCPA" HeaderText="PSMTCPA" SortExpression="PSMTCPA" />
        <asp:CheckBoxField DataField="Filtration" HeaderText="Filtration" SortExpression="Filtration" />
        <asp:CheckBoxField DataField="Residuals_Generation" HeaderText="Residuals_Generation" SortExpression="Residuals_Generation" />
        <asp:CheckBoxField DataField="T_Report" HeaderText="T_Report" SortExpression="T_Report" />
        <asp:CheckBoxField DataField="Distributive_Pumping" HeaderText="Distributive_Pumping" SortExpression="Distributive_Pumping" />
        <asp:CheckBoxField DataField="Booster_Pumping" HeaderText="Booster_Pumping" SortExpression="Booster_Pumping" />
        <asp:CheckBoxField DataField="PressureReducing" HeaderText="PressureReducing" SortExpression="PressureReducing" />
        <asp:CheckBoxField DataField="Ground_Storage" HeaderText="Ground_Storage" SortExpression="Ground_Storage" />
        <asp:CheckBoxField DataField="Elevated_Storage" HeaderText="Elevated_Storage" SortExpression="Elevated_Storage" />
        <asp:CheckBoxField DataField="On_Site_Analytical_Instruments" HeaderText="On_Site_Analytical_Instruments" SortExpression="On_Site_Analytical_Instruments" />
        <asp:CheckBoxField DataField="System_Delivery_Facility" HeaderText="System_Delivery_Facility" SortExpression="System_Delivery_Facility" />
        <asp:CheckBoxField DataField="SewerLift" HeaderText="SewerLift" SortExpression="SewerLift" />
        <asp:CheckBoxField DataField="SewerTreatment" HeaderText="SewerTreatment" SortExpression="SewerTreatment" />
        <asp:CheckBoxField DataField="FieldOperations" HeaderText="FieldOperations" SortExpression="FieldOperations" />
        <asp:CheckBoxField DataField="Spoils_Staging" HeaderText="Spoils_Staging" SortExpression="Spoils_Staging" />
        
        <asp:BoundField DataField="Confined_Space_Requirement" HeaderText="Confined_Space_Requirement" SortExpression="Confined_Space_Requirement" />
        <asp:TemplateField HeaderText="FEMA_Flood_Rating" SortExpression="FEMA_Flood_Rating">
            <ItemTemplate><asp:Label runat="server" ID="lblFEMA_Flood_Rating" Text='<%# Bind("FEMA_Flood_Rating_Text") %>'></asp:Label></ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList runat="server" ID="ddlFEMA_Flood_Rating" 
                    DataSourceID="dsFEMA_Flood_Rating" 
                    AppendDataBoundItems="true"
                    DataValueField="LookupID"
                    DataTextField="LookupValue"
                    SelectedValue='<%# Bind("FEMA_Flood_Rating") %>'
                    >
                    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                </asp:DropDownList>                    

                <asp:SqlDataSource runat="server" ID="dsFEMA_Flood_Rating"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
                    SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'FEMA_Flood_Rating' order by 2"
                    >
                </asp:SqlDataSource>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:CheckBoxField DataField="CellularAntenna" HeaderText="CellularAntenna" SortExpression="CellularAntenna" />
        <asp:BoundField DataField="NJDEP_Designation_TreatmentPlant" HeaderText="NJDEP_Designation_TreatmentPlant" SortExpression="NJDEP_Designation_TreatmentPlant" />
        <asp:BoundField DataField="NJDEP_Designation_PumpStation" HeaderText="NJDEP_Designation_PumpStation" SortExpression="NJDEP_Designation_PumpStation" />
        <asp:BoundField DataField="DEP_Firm_Capacity_Facility_MGD" HeaderText="DEP_Firm_Capacity_Facility_MGD" SortExpression="DEP_Firm_Capacity_Facility_MGD" />
        <asp:BoundField DataField="DEP_Total_Effective_Capacity_Facility_MGD" HeaderText="DEP_Total_Effective_Capacity_Facility_MGD" SortExpression="DEP_Total_Effective_Capacity_Facility_MGD" />
        <asp:BoundField DataField="DEP_Production_Capacity_Facility_MGD" HeaderText="DEP_Production_Capacity_Facility_MGD" SortExpression="DEP_Production_Capacity_Facility_MGD" />
        <asp:BoundField DataField="DEP_Production_Capacity_Under_Aux_Power_Facility_MGD" HeaderText="DEP_Production_Capacity_Under_Aux_Power_Facility_MGD"
            SortExpression="DEP_Production_Capacity_Under_Aux_Power_Facility_MGD" />
        <asp:BoundField DataField="Facility_Inspection_Frequency" HeaderText="Facility_Inspection_Frequency" SortExpression="Facility_Inspection_Frequency" />
        <asp:BoundField DataField="Facility_Loop_Grouping" HeaderText="Facility_Loop_Grouping" SortExpression="Facility_Loop_Grouping" />
        <asp:BoundField DataField="Facility_Loop_Grouping_Sub" HeaderText="Facility_Loop_Grouping_Sub" SortExpression="Facility_Loop_Grouping_Sub" />
        <asp:BoundField DataField="Facility_Loop_Sequence" HeaderText="Facility_Loop_Sequence" SortExpression="Facility_Loop_Sequence" />
        <asp:BoundField DataField="Security_Category" HeaderText="Security_Category" SortExpression="Security_Category" />
        <asp:BoundField DataField="Security_Grouping" HeaderText="Security_Grouping" SortExpression="Security_Grouping" />
        <asp:BoundField DataField="Security_Inspection_Frequency" HeaderText="Security_Inspection_Frequency" SortExpression="Security_Inspection_Frequency" />
        <asp:BoundField DataField="Security_Loop_Sequence" HeaderText="Security_Loop_Sequence" SortExpression="Security_Loop_Sequence" />
        <asp:CheckBoxField DataField="SCADA_Intrusion_Alarm" HeaderText="SCADA_Intrusion_Alarm" SortExpression="SCADA_Intrusion_Alarm" />
        <asp:CheckBoxField DataField="PSM_TCPA" HeaderText="PSM_TCPA" SortExpression="PSM_TCPA" />
        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
        <asp:BoundField DataField="Facility_Contact_Info" HeaderText="Facility_Contact_Info" SortExpression="Facility_Contact_Info" />
        <asp:BoundField DataField="ObjectID" HeaderText="ObjectID" SortExpression="ObjectID" />
        
        <asp:TemplateField ShowHeader="False">
            <InsertItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
            </InsertItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure?');"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Fields>
    <FieldHeaderStyle Width="50%" />
    <HeaderStyle Width="50%" />
</asp:DetailsView>

<asp:Label runat="server" ID="lblResults"></asp:Label>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
    DeleteCommand="DELETE FROM [tblFacilities] WHERE [RecordId] = @RecordId"
    InsertCommand="
        INSERT INTO [tblFacilities] (
            [Corporation], [Region], [CompanySubsidiary], [Operations], [District], [System], [PWSID],
            [OperatingCenterID], [FacilityID], [FacilityName], [StreetNumber], [StreetPrefix], [StreetName],
            [StreetSuffix], [NearestCrossStreet], [Town], [TownSection], [TownID], [County], [CountyID],
            [State], [StateID], [ZipCode], [Production_Capacity_Maximum_MGD],
            [GradientZoneServed], [CriticalRating], [SICNumber], [JDEBULocationNumber], [JDEParentNumber],
            [JDEItemNumber], [YearInService], [CoordinateID], [Elevation], [TaxDistrict], [Lot],
            [Block], [Qualifier], [DEPIDNumber], [FederalTaxID], [WaterShed], [RegionalPlanningArea],
            [Administration], [WaterTreatmentFacility], [ChemicalFeed], [EmergencyPower], [Interconnection],
            [PressureReducing], [PropertyOnly], [Distributive_Pumping], [Surface_Water_Supply], [Reservoir],
            [Dam], [SewerLift], [SewerTreatment], [Ground_Water_Supply], [PSMTCPA], [CellularAntenna],
            [Notes], [Facility_Contact_Info], [Facility_Ownership], [Status], [Point_Of_Entry], [Elevated_Storage],
            [Ground_Storage], [T_Report], [Booster_Pumping], [System_Delivery_Facility], [Filtration],
            [Residuals_Generation], [NJDEP_Designation_TreatmentPlant], [NJDEP_Designation_PumpStation],
            [DEP_Firm_Capacity_Facility_MGD], [DEP_Total_Effective_Capacity_Facility_MGD],
            [DEP_Production_Capacity_Facility_MGD], [DEP_Production_Capacity_Under_Aux_Power_Facility_MGD],
            [Facility_Inspection_Frequency], [Facility_Loop_Grouping], [Facility_Loop_Grouping_Sub],
            [Facility_Loop_Sequence], [On_Site_Analytical_Instruments], [Security_Category], [Security_Grouping],
            [Security_Inspection_Frequency], [Security_Loop_Sequence], [SCADA_Intrusion_Alarm], [DPCC],
            [PSM_TCPA], [Confined_Space_Requirement], FEMA_Flood_Rating, FieldOperations, Spoils_Staging, 
            ObjectID, tblTownsID, DepartmentID
        ) VALUES (
            @Corporation, @Region, @CompanySubsidiary, @Operations, @District, @System, @PWSID, @OperatingCenterID,
            @FacilityID, @FacilityName, @StreetNumber, @StreetPrefix, @StreetName, @StreetSuffix,
            @NearestCrossStreet, @Town, @TownSection, @TownID, @County, @CountyID, @State, @StateID, @ZipCode,
            @Production_Capacity_Maximum_MGD, @GradientZoneServed, @CriticalRating, @SICNumber,
            @JDEBULocationNumber, @JDEParentNumber, @JDEItemNumber, @YearInService, @CoordinateID,
            @Elevation, @TaxDistrict, @Lot, @Block, @Qualifier, @DEPIDNumber, @FederalTaxID, @WaterShed,
            @RegionalPlanningArea, @Administration, @WaterTreatmentFacility, @ChemicalFeed, @EmergencyPower,
            @Interconnection, @PressureReducing, @PropertyOnly, @Distributive_Pumping, @Surface_Water_Supply,
            @Reservoir, @Dam, @SewerLift, @SewerTreatment, @Ground_Water_Supply, @PSMTCPA, @CellularAntenna,
            @Notes, @Facility_Contact_Info, @Facility_Ownership, @Status, @Point_Of_Entry, @Elevated_Storage, @Ground_Storage,
            @T_Report, @Booster_Pumping, @System_Delivery_Facility, @Filtration, @Residuals_Generation,
            @NJDEP_Designation_TreatmentPlant, @NJDEP_Designation_PumpStation, @DEP_Firm_Capacity_Facility_MGD,
            @DEP_Total_Effective_Capacity_Facility_MGD, @DEP_Production_Capacity_Facility_MGD,
            @DEP_Production_Capacity_Under_Aux_Power_Facility_MGD, @Facility_Inspection_Frequency,
            @Facility_Loop_Grouping, @Facility_Loop_Grouping_Sub, @Facility_Loop_Sequence,
            @On_Site_Analytical_Instruments, @Security_Category, @Security_Grouping,
            @Security_Inspection_Frequency, @Security_Loop_Sequence, @SCADA_Intrusion_Alarm, @DPCC,
            @PSM_TCPA, @Confined_Space_Requirement, @FEMA_Flood_Rating, @FieldOperations, @Spoils_Staging,
            @ObjectID, @tblTownsID, @DepartmentID
        );
        Select @RecordId = @@IDENTITY;"
    SelectCommand="
        SELECT tblFacilities.[RecordId], [CreatedAt], [Corporation], [Region], [CompanySubsidiary] ,[Operations],
            tblFacilities.[District], [System], tblFacilities.[PWSID], tblFacilities.[OperatingCenterID], [FacilityID], [FacilityName],
            [StreetNumber], [StreetPrefix], [StreetName], [StreetSuffix], [NearestCrossStreet], tblFacilities.[ZipCode],
            [Production_Capacity_Maximum_MGD], [GradientZoneServed], [CriticalRating], [SICNumber],
            [JDEBULocationNumber], [JDEParentNumber], [JDEItemNumber], [YearInService], [tblFacilities].[CoordinateID], [Elevation],
            [TaxDistrict], [Lot], [Block], [Qualifier], [DEPIDNumber], [FederalTaxID], [WaterShed], [RegionalPlanningArea],
            [Administration], [WaterTreatmentFacility], [ChemicalFeed], [EmergencyPower], [Interconnection],
            [PressureReducing], [PropertyOnly], [Distributive_Pumping], [Surface_Water_Supply], [Reservoir], [Dam],
            [SewerLift], [SewerTreatment], [Ground_Water_Supply], [PSMTCPA], [CellularAntenna], [Notes], [Facility_Contact_Info],
            [Facility_Ownership], [Status], [Point_Of_Entry], [Elevated_Storage], [Ground_Storage], [T_Report],
            [Booster_Pumping], [System_Delivery_Facility], [Filtration], [Residuals_Generation], [NJDEP_Designation_TreatmentPlant],
            [NJDEP_Designation_PumpStation], [DEP_Firm_Capacity_Facility_MGD], [DEP_Total_Effective_Capacity_Facility_MGD],
            [DEP_Production_Capacity_Facility_MGD], [DEP_Production_Capacity_Under_Aux_Power_Facility_MGD],
            [Facility_Inspection_Frequency], [Facility_Loop_Grouping], [Facility_Loop_Grouping_Sub], [Facility_Loop_Sequence],
            [On_Site_Analytical_Instruments], [Security_Category], [Security_Grouping], [Security_Inspection_Frequency],
            [Security_Loop_Sequence], [SCADA_Intrusion_Alarm], [DPCC], [PSM_TCPA], [Confined_Space_Requirement], tblFacilities.tblTownsID, 
            Towns.Town, TownSections.Name as townSection, tblFacilities.TownID,
            (select Name from Counties where Counties.SpecialID = Towns.CountyID and Counties.StateID = Towns.StateID) as County,
            (select Abbreviation from States where States.StateID = Towns.StateID) as State,
            #1.LookupValue as Facility_Ownership_Text, #2.LookupValue as Status_Text,
            #3.LookUpValue as FEMA_Flood_Rating_Text, FEMA_Flood_Rating, FieldOperations, Spoils_Staging, ObjectID, tblFacilities.DepartmentID, departments.Description as Department,
            bu.Bu,
            oc.OperatingCenterCode
        FROM [tblFacilities] 
        LEFT JOIN [Lookup] #1 on [tblFacilities].Facility_Ownership = #1.LookupID
        LEFT JOIN [Lookup] #2 on [tblFacilities].Status = #2.LookupID
        LEFT JOIN [Lookup] #3 on [tblFacilities].FEMA_Flood_Rating = #3.LookupID
        LEFT JOIN Towns on Towns.TownID = tblFacilities.TownID
        LEFT JOIN TownSections on TownSections.TownSectionID = tblFacilities.tblTownSectionID
        LEFT JOIN OperatingCenters oc on oc.OperatingCenterID = tblFacilities.OperatingCenterID
        LEFT JOIN Departments On Departments.DepartmentID = tblFacilities.DepartmentID
        LEFT JOIN BusinessUnits bu on bu.OPeratingCenterID = oc.OperatingCenterID and bu.DepartmentID = Departments.DepartmentID
        WHERE tblFacilities.RecordId = @RecordId"
    UpdateCommand="
        UPDATE [tblFacilities] SET
            [CreatedAt] = @DateCreated, [Corporation] = @Corporation, [Region] = @Region, [CompanySubsidiary] = @CompanySubsidiary,
            [Operations] = @Operations, [District] = @District, [System] = @System, [PWSID] = @PWSID, [OperatingCenterID] = @OperatingCenterID,
            [FacilityID] = @FacilityID, [FacilityName] = @FacilityName, [StreetNumber] = @StreetNumber, [StreetPrefix] = @StreetPrefix,
            [StreetName] = @StreetName, [StreetSuffix] = @StreetSuffix, [NearestCrossStreet] = @NearestCrossStreet,
            [Town] = @Town, [TownSection] = @TownSection, [TownID] = @TownID, [County] = @County, [CountyID] = @CountyID,
            [State] = @State, [StateID] = @StateID, [ZipCode] = @ZipCode,
            [Production_Capacity_Maximum_MGD] = @Production_Capacity_Maximum_MGD, [GradientZoneServed] = @GradientZoneServed,
            [CriticalRating] = @CriticalRating, [SICNumber] = @SICNumber, [JDEBULocationNumber] = @JDEBULocationNumber,
            [JDEParentNumber] = @JDEParentNumber, [JDEItemNumber] = @JDEItemNumber, [YearInService] = @YearInService,
            [CoordinateID] = @CoordinateID, [Elevation] = @Elevation, [TaxDistrict] = @TaxDistrict, [Lot] = @Lot,
            [Block] = @Block, [Qualifier] = @Qualifier, [DEPIDNumber] = @DEPIDNumber, [FederalTaxID] = @FederalTaxID,
            [WaterShed] = @WaterShed, [RegionalPlanningArea] = @RegionalPlanningArea, [Administration] = @Administration,
            [WaterTreatmentFacility] = @WaterTreatmentFacility, [ChemicalFeed] = @ChemicalFeed, [EmergencyPower] = @EmergencyPower,
            [Interconnection] = @Interconnection, [PressureReducing] = @PressureReducing, [PropertyOnly] = @PropertyOnly,
            [Distributive_Pumping] = @Distributive_Pumping, [Surface_Water_Supply] = @Surface_Water_Supply, [Reservoir] = @Reservoir,
            [Dam] = @Dam, [SewerLift] = @SewerLift, [SewerTreatment] = @SewerTreatment, [Ground_Water_Supply] = @Ground_Water_Supply,
            [PSMTCPA] = @PSMTCPA, [CellularAntenna] = @CellularAntenna, [Notes] = @Notes, [Facility_Contact_Info] = @Facility_Contact_Info,
            [Facility_Ownership] = @Facility_Ownership, [Status] = @Status, [Point_Of_Entry] = @Point_Of_Entry,
            [Elevated_Storage] = @Elevated_Storage, [Ground_Storage] = @Ground_Storage, [T_Report] = @T_Report,
            [Booster_Pumping] = @Booster_Pumping, [System_Delivery_Facility] = @System_Delivery_Facility,
            [Filtration] = @Filtration, [Residuals_Generation] = @Residuals_Generation,
            [NJDEP_Designation_TreatmentPlant] = @NJDEP_Designation_TreatmentPlant,
            [NJDEP_Designation_PumpStation] = @NJDEP_Designation_PumpStation,
            [DEP_Firm_Capacity_Facility_MGD] = @DEP_Firm_Capacity_Facility_MGD,
            [DEP_Total_Effective_Capacity_Facility_MGD] = @DEP_Total_Effective_Capacity_Facility_MGD,
            [DEP_Production_Capacity_Facility_MGD] = @DEP_Production_Capacity_Facility_MGD,
            [DEP_Production_Capacity_Under_Aux_Power_Facility_MGD] = @DEP_Production_Capacity_Under_Aux_Power_Facility_MGD,
            [Facility_Inspection_Frequency] = @Facility_Inspection_Frequency, [Facility_Loop_Grouping] = @Facility_Loop_Grouping,
            [Facility_Loop_Grouping_Sub] = @Facility_Loop_Grouping_Sub, [Facility_Loop_Sequence] = @Facility_Loop_Sequence,
            [On_Site_Analytical_Instruments] = @On_Site_Analytical_Instruments, [Security_Category] = @Security_Category,
            [Security_Grouping] = @Security_Grouping, [Security_Inspection_Frequency] = @Security_Inspection_Frequency,
            [Security_Loop_Sequence] = @Security_Loop_Sequence, [SCADA_Intrusion_Alarm] = @SCADA_Intrusion_Alarm, [DPCC] = @DPCC,
            [PSM_TCPA] = @PSM_TCPA, [Confined_Space_Requirement] = @Confined_Space_Requirement, FEMA_Flood_Rating = @FEMA_Flood_Rating, 
            FieldOperations = @FieldOperations, [Spoils_Staging] = @Spoils_Staging, [ObjectID] = @ObjectID, tblTownsID = @tblTownsID,
            DepartmentID = @DepartmentID
        WHERE [RecordId] = @RecordId"
    OnInserted="SqlDataSource1_Inserted"
    OnDeleted="SqlDataSource1_Deleted"
    OnUpdated="SqlDataSource1_Updated">
    <InsertParameters>
        <asp:Parameter Name="Corporation" Type="String" />
        <asp:Parameter Name="Region" Type="String" />
        <asp:Parameter Name="CompanySubsidiary" Type="String" />
        <asp:Parameter Name="Operations" Type="String" />
        <asp:Parameter Name="District" Type="String" />
        <asp:Parameter Name="System" Type="String" />
        <asp:Parameter Name="PWSID" Type="String" />
        <asp:Parameter Name="OperatingCenterID" Type="Int32" />
        <asp:Parameter Name="FacilityID" Type="String" />
        <asp:Parameter Name="FacilityName" Type="String" />
        <asp:Parameter Name="StreetNumber" Type="String" />
        <asp:Parameter Name="StreetPrefix" Type="String" />
        <asp:Parameter Name="StreetName" Type="String" />
        <asp:Parameter Name="StreetSuffix" Type="String" />
        <asp:Parameter Name="NearestCrossStreet" Type="String" />
        <asp:Parameter Name="Town" Type="String" />
        <asp:Parameter Name="TownSection" Type="String" />
        <asp:Parameter Name="TownID" Type="Double" />
        <asp:Parameter Name="County" Type="String" />
        <asp:Parameter Name="CountyID" Type="String" />
        <asp:Parameter Name="State" Type="String" />
        <asp:Parameter Name="StateID" Type="String" />
        <asp:Parameter Name="ZipCode" Type="String" />
        <asp:Parameter Name="Production_Capacity_Maximum_MGD" Type="Double" />
        <asp:Parameter Name="GradientZoneServed" Type="String" />
        <asp:Parameter Name="CriticalRating" Type="Double" />
        <asp:Parameter Name="SICNumber" Type="Double" />
        <asp:Parameter Name="JDEBULocationNumber" Type="String" />
        <asp:Parameter Name="JDEParentNumber" Type="String" />
        <asp:Parameter Name="JDEItemNumber" Type="String" />
        <asp:Parameter Name="YearInService" Type="String" />
        <asp:Parameter Name="CoordinateID" Type="Int32" />
        <asp:Parameter Name="Elevation" Type="Double" />
        <asp:Parameter Name="TaxDistrict" Type="String" />
        <asp:Parameter Name="Lot" Type="String" />
        <asp:Parameter Name="Block" Type="String" />
        <asp:Parameter Name="Qualifier" Type="String" />
        <asp:Parameter Name="DEPIDNumber" Type="String" />
        <asp:Parameter Name="FederalTaxID" Type="String" />
        <asp:Parameter Name="WaterShed" Type="String" />
        <asp:Parameter Name="RegionalPlanningArea" Type="String" />
        <asp:Parameter Name="Administration" Type="Boolean" />
        <asp:Parameter Name="WaterTreatmentFacility" Type="Boolean" />
        <asp:Parameter Name="ChemicalFeed" Type="Boolean" />
        <asp:Parameter Name="EmergencyPower" Type="Boolean" />
        <asp:Parameter Name="Interconnection" Type="Boolean" />
        <asp:Parameter Name="PressureReducing" Type="Boolean" />
        <asp:Parameter Name="PropertyOnly" Type="Boolean" />
        <asp:Parameter Name="Distributive_Pumping" Type="Boolean" />
        <asp:Parameter Name="Surface_Water_Supply" Type="Boolean" />
        <asp:Parameter Name="Reservoir" Type="Boolean" />
        <asp:Parameter Name="Dam" Type="Boolean" />
        <asp:Parameter Name="SewerLift" Type="Boolean" />
        <asp:Parameter Name="SewerTreatment" Type="Boolean" />
        <asp:Parameter Name="Ground_Water_Supply" Type="Boolean" />
        <asp:Parameter Name="PSMTCPA" Type="Boolean" />
        <asp:Parameter Name="CellularAntenna" Type="Boolean" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="Facility_Contact_Info" Type="String" />
        <asp:Parameter Name="Facility_Ownership" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Point_Of_Entry" Type="Boolean" />
        <asp:Parameter Name="Elevated_Storage" Type="Boolean" />
        <asp:Parameter Name="Ground_Storage" Type="Boolean" />
        <asp:Parameter Name="T_Report" Type="Boolean" />
        <asp:Parameter Name="Booster_Pumping" Type="Boolean" />
        <asp:Parameter Name="System_Delivery_Facility" Type="Boolean" />
        <asp:Parameter Name="Filtration" Type="Boolean" />
        <asp:Parameter Name="Residuals_Generation" Type="Boolean" />
        <asp:Parameter Name="FieldOperations" Type="Boolean" />        
        <asp:Parameter Name="NJDEP_Designation_TreatmentPlant" Type="String" />
        <asp:Parameter Name="NJDEP_Designation_PumpStation" Type="String" />
        <asp:Parameter Name="DEP_Firm_Capacity_Facility_MGD" Type="Double" />
        <asp:Parameter Name="DEP_Total_Effective_Capacity_Facility_MGD" Type="Double" />
        <asp:Parameter Name="DEP_Production_Capacity_Facility_MGD" Type="Double" />
        <asp:Parameter Name="DEP_Production_Capacity_Under_Aux_Power_Facility_MGD" Type="Double" />
        <asp:Parameter Name="Facility_Inspection_Frequency" Type="String" />
        <asp:Parameter Name="Facility_Loop_Grouping" Type="String" />
        <asp:Parameter Name="Facility_Loop_Grouping_Sub" Type="String" />
        <asp:Parameter Name="Facility_Loop_Sequence" Type="Int32" />
        <asp:Parameter Name="On_Site_Analytical_Instruments" Type="Boolean" />
        <asp:Parameter Name="Security_Category" Type="String" />
        <asp:Parameter Name="Security_Grouping" Type="String" />
        <asp:Parameter Name="Security_Inspection_Frequency" Type="String" />
        <asp:Parameter Name="Security_Loop_Sequence" Type="String" />
        <asp:Parameter Name="SCADA_Intrusion_Alarm" Type="Boolean" />
        <asp:Parameter Name="DPCC" Type="Boolean" />
        <asp:Parameter Name="PSM_TCPA" Type="Boolean" />
        <asp:Parameter Name="Confined_Space_Requirement" Type="String" />
        <asp:Parameter Name="FEMA_Flood_Rating" Type="Int32" />
        <asp:Parameter Name="ObjectID" Type="Int32" />
        <asp:Parameter Name="tblTownsID" Type="Int32" />
        <asp:Parameter Name="departmentID" Type="Int32" />
        <asp:Parameter Name="RecordId" Type="Int32" Direction="Output" />
        
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="DateCreated" Type="DateTime" />
        <asp:Parameter Name="Corporation" Type="String" />
        <asp:Parameter Name="Region" Type="String" />
        <asp:Parameter Name="CompanySubsidiary" Type="String" />
        <asp:Parameter Name="Operations" Type="String" />
        <asp:Parameter Name="District" Type="String" />
        <asp:Parameter Name="System" Type="String" />
        <asp:Parameter Name="PWSID" Type="String" />
        <asp:Parameter Name="OperatingCenterID" Type="Int32" />
        <asp:Parameter Name="FacilityID" Type="String" />
        <asp:Parameter Name="FacilityName" Type="String" />
        <asp:Parameter Name="StreetNumber" Type="String" />
        <asp:Parameter Name="StreetPrefix" Type="String" />
        <asp:Parameter Name="StreetName" Type="String" />
        <asp:Parameter Name="StreetSuffix" Type="String" />
        <asp:Parameter Name="NearestCrossStreet" Type="String" />
        <asp:Parameter Name="Town" Type="String" />
        <asp:Parameter Name="TownSection" Type="String" />
        <asp:Parameter Name="TownID" Type="Double" />
        <asp:Parameter Name="County" Type="String" />
        <asp:Parameter Name="CountyID" Type="String" />
        <asp:Parameter Name="State" Type="String" />
        <asp:Parameter Name="StateID" Type="String" />
        <asp:Parameter Name="ZipCode" Type="String" />
        <asp:Parameter Name="Production_Capacity_Maximum_MGD" Type="Double" />
        <asp:Parameter Name="GradientZoneServed" Type="String" />
        <asp:Parameter Name="CriticalRating" Type="Double" />
        <asp:Parameter Name="SICNumber" Type="Double" />
        <asp:Parameter Name="JDEBULocationNumber" Type="String" />
        <asp:Parameter Name="JDEParentNumber" Type="String" />
        <asp:Parameter Name="JDEItemNumber" Type="String" />
        <asp:Parameter Name="YearInService" Type="String" />
        <asp:Parameter Name="CoordinateID" Type="Int32" />
        <asp:Parameter Name="Elevation" Type="Double" />
        <asp:Parameter Name="TaxDistrict" Type="String" />
        <asp:Parameter Name="Lot" Type="String" />
        <asp:Parameter Name="Block" Type="String" />
        <asp:Parameter Name="Qualifier" Type="String" />
        <asp:Parameter Name="DEPIDNumber" Type="String" />
        <asp:Parameter Name="FederalTaxID" Type="String" />
        <asp:Parameter Name="WaterShed" Type="String" />
        <asp:Parameter Name="RegionalPlanningArea" Type="String" />
        <asp:Parameter Name="Administration" Type="Boolean" />
        <asp:Parameter Name="WaterTreatmentFacility" Type="Boolean" />
        <asp:Parameter Name="ChemicalFeed" Type="Boolean" />
        <asp:Parameter Name="EmergencyPower" Type="Boolean" />
        <asp:Parameter Name="Interconnection" Type="Boolean" />
        <asp:Parameter Name="PressureReducing" Type="Boolean" />
        <asp:Parameter Name="PropertyOnly" Type="Boolean" />
        <asp:Parameter Name="Distributive_Pumping" Type="Boolean" />
        <asp:Parameter Name="Surface_Water_Supply" Type="Boolean" />
        <asp:Parameter Name="Reservoir" Type="Boolean" />
        <asp:Parameter Name="Dam" Type="Boolean" />
        <asp:Parameter Name="SewerLift" Type="Boolean" />
        <asp:Parameter Name="SewerTreatment" Type="Boolean" />
        <asp:Parameter Name="Ground_Water_Supply" Type="Boolean" />
        <asp:Parameter Name="PSMTCPA" Type="Boolean" />
        <asp:Parameter Name="CellularAntenna" Type="Boolean" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="Facility_Contact_Info" Type="String" />
        <asp:Parameter Name="Facility_Ownership" Type="Int32" />
        <asp:Parameter Name="Status" Type="Int32" />
        <asp:Parameter Name="Point_Of_Entry" Type="Boolean" />
        <asp:Parameter Name="Elevated_Storage" Type="Boolean" />
        <asp:Parameter Name="Ground_Storage" Type="Boolean" />
        <asp:Parameter Name="T_Report" Type="Boolean" />
        <asp:Parameter Name="Booster_Pumping" Type="Boolean" />
        <asp:Parameter Name="System_Delivery_Facility" Type="Boolean" />
        <asp:Parameter Name="Filtration" Type="Boolean" />
        <asp:Parameter Name="Residuals_Generation" Type="Boolean" />
        <asp:Parameter Name="FieldOperations" Type="Boolean" />
        <asp:Parameter Name="NJDEP_Designation_TreatmentPlant" Type="String" />
        <asp:Parameter Name="NJDEP_Designation_PumpStation" Type="String" />
        <asp:Parameter Name="DEP_Firm_Capacity_Facility_MGD" Type="Double" />
        <asp:Parameter Name="DEP_Total_Effective_Capacity_Facility_MGD" Type="Double" />
        <asp:Parameter Name="DEP_Production_Capacity_Facility_MGD" Type="Double" />
        <asp:Parameter Name="DEP_Production_Capacity_Under_Aux_Power_Facility_MGD" Type="Double" />
        <asp:Parameter Name="Facility_Inspection_Frequency" Type="String" />
        <asp:Parameter Name="Facility_Loop_Grouping" Type="String" />
        <asp:Parameter Name="Facility_Loop_Grouping_Sub" Type="String" />
        <asp:Parameter Name="Facility_Loop_Sequence" Type="Int32" />
        <asp:Parameter Name="On_Site_Analytical_Instruments" Type="Boolean" />
        <asp:Parameter Name="Security_Category" Type="String" />
        <asp:Parameter Name="Security_Grouping" Type="String" />
        <asp:Parameter Name="Security_Inspection_Frequency" Type="String" />
        <asp:Parameter Name="Security_Loop_Sequence" Type="String" />
        <asp:Parameter Name="SCADA_Intrusion_Alarm" Type="Boolean" />
        <asp:Parameter Name="DPCC" Type="Boolean" />
        <asp:Parameter Name="PSM_TCPA" Type="Boolean" />
        <asp:Parameter Name="Confined_Space_Requirement" Type="String" />
        <asp:Parameter Name="FEMA_Flood_Rating" Type="Int32" />
        <asp:Parameter Name="ObjectID" Type="Int32" />
        <asp:Parameter Name="tblTownsID" Type="Int32" />
        <asp:Parameter Name="departmentID" Type="Int32" />
        <asp:Parameter Name="RecordId" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:Parameter Name="RecordId" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:Parameter Name="RecordId" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>
&nbsp;<br />
<br />
&nbsp;
