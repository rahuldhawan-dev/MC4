<%@ Page Title="Projects RP" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ProjectsRP.aspx.cs" Inherits="MapCall.Modules.Engineering.ProjectsRP" EnableEventValidation="false" %>
<%@ Register TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" Src="~/Controls/DetailsViewDataPageTemplate.ascx" %>
<%@ Import Namespace="MapCall.Common.ClassExtensions" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls.DropDowns" Assembly="MapCall" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls" Assembly="MapCall" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls.Data" Assembly="MapCall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagPrefix="mmsi" TagName="OpCntrDataField" %>
<%@ Register TagPrefix="mapcall" Namespace="MMSINC.Controls" Assembly="MMSINC.Core.WebForms" %>
<%@ Register TagPrefix="atk" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.50508.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register TagPrefix="mmsinc" Namespace="MMSINC.Controls" Assembly="MMSINC.Core.WebForms" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    RP-Mains\Valves
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server" 
        DataElementTableName="RPProjects" 
		DataElementPrimaryFieldName="RPProjectID"
		DataTypeId="153" ShowMapButton="True"
        Label="Projects RP">
        
        <SearchBox>
            <Fields>
                <search:TemplatedSearchField FilterMode="Manual">
                    <Template>
                        <mmsi:OpCntrDataField ID="opCntrField" runat="server" 
                            UseTowns="True" 
                            DataFieldName="rp.OperatingCenterID" 
                            TownDataFieldName="rp.TownID" />
                    </Template>
                </search:TemplatedSearchField>
                <search:TextSearchField DataFieldName="WBSNumber"/>
                <search:TextSearchField DataFieldName="ProjectTitle" />
                <search:NumericSearchField DataFieldName="RPProjectID" HeaderText="ProjectID"/>
                <search:TextSearchField DataFieldName="HistoricProjectID" />
                <search:DropDownSearchField
                    HeaderText="Status"
                    DataFieldName="rp.StatusID"
                    TextFieldName="Description"
                    ValueFieldName="RPProjectStatusID"
                    TableName="RPProjectStatuses" SelectMode="Multiple"
                    />
                <search:DropDownSearchField
                    HeaderText="Project Type"
                    DataFieldName="rp.ProjectTypeID"
                    TextFieldName="Description"
                    ValueFieldName="ProjectTypeID"
                    TableName="ProjectTypes" />
                <search:DateTimeSearchField DataFieldName="EstimatedInServiceDate"/>
                <search:DateTimeSearchField DataFieldName="ActualInServiceDate"/>
                <search:DropDownSearchField 
                    HeaderText="Foundational Filing Period"
                    DataFieldName="rp.FoundationalFilingPeriodID"
                    TableName="FoundationalFilingPeriods"
                    TextFieldName="Description"
                    ValueFieldName="FoundationalFilingPeriodID" />
                <search:DropDownSearchField
                    HeaderText="Asset Category"
                    DataFieldName="rp.AssetCategoryId"
                    TextFieldName="Description"
                    ValueFieldName="Id"
                    TableName="AssetCategories" />
                <search:DropDownSearchField
                    HeaderText="Asset Type"
                    DataFieldName="rp.AssetTypeId"
                    TextFieldName="Description"
                    ValueFieldName="AssetTypeId"
                    TableName="AssetTypes" />
            </Fields>
        </SearchBox>
        <ResultsGridView OnRowDataBound="gv_RowDataBound">
            <Columns>
                <mapcall:BoundField DataField="RPProjectID" HeaderText="ProjectID"/>
                <mapcall:BoundField DataField="WBSNumber"/>
                <mapcall:BoundField DataField="HistoricProjectID" />
                <mapcall:BoundField DataField="OperatingCenterCode" HeaderText="Operating Center"/>
                <mapcall:BoundField DataField="Town"/>
                <mapcall:BoundField DataField="ProjectTitle"/>
                <mapcall:BoundField DataField="OriginationYear" />
                <mapcall:BoundField DataField="Latitude" Visible="False"/>
                <mapcall:BoundField DataField="Longitude" Visible="False"/>
                <mapcall:BoundField DataField="Status"/>
                <mapcall:BoundField DataField="ProjectType"/>
                <mapcall:BoundField DataField="AssetCategory"/>
                <mapcall:BoundField DataField="AssetType"/>
            </Columns>
        </ResultsGridView>
        <ResultsDataSource
            SelectCommand="
                SELECT 
                    [RPProjectID], 
                    [ProjectTitle], 
                    rp.[OperatingCenterID], 
                    oc.OperatingCenterCode,
                    rp.[TownID],
                    [District], 
                    [OriginationYear], 
                    [HistoricProjectID], 
                    [NJAWEstimate], 
                    rp.[HighCostFactorID], 
                    rp.[ProjectTypeID], 
                    [ProposedLength], 
                    rp.[ProposedDiameterID], 
                    [ProposedPipeMaterialID], 
                    [Justification],
                    [EstimatedProjectDuration], 
                    [EstimatedInServiceDate],
                    [ActualInServiceDate],
                    cast(year(EstimatedInServiceDate) as varchar(4)) + 'Q' + CAST(datepart(qq, EstimatedInServiceDate) as varchar(2)) as [EstimatedInServicePeriod], 
                    rp.[FoundationalFilingPeriodID],
                    [StatusID],
                    [FinalCriteriaScore], 
                    [FinalRawScore], 
                    C.[CoordinateID],
                    hcf.Description as HighCostFactor,
                    pt.Description as ProjectType,
                    pd.Diameter as PipeDiameter,
                    pm.Description as PipeMaterial,
                    ffp.Description as FoundationalFilingPeriod, 
                    t.Town,
                    C.Latitude, C.Longitude,
                    rp.WbsNumber,
                    Stat.Description as status,
                    ac.Description as AssetCategory,
                    at.Description as AssetType
                FROM 
                    [RPProjects] rp
                LEFT JOIN
	                OperatingCenters oc
                ON
	                oc.OperatingCenterID = rp.OperatingCenterID
                LEFT JOIN
                    Towns t
                ON
                    t.TownID = rp.TownID
                LEFT JOIN
                    HighCostFactors hcf
                ON
                    hcf.HighCostFactorID = rp.HighCostFactorID
                LEFT JOIN
                    ProjectTypes pt
                ON
                    pt.ProjectTypeID = rp.ProjectTypeID
                LEFT JOIN
                    PipeDiameters pd
                ON
                    pd.PipeDiameterID = rp.ProposedDiameterID
                LEFT JOIN
                    PipeMaterials pm
                ON
                    pm.PipeMaterialID = rp.ProposedPipeMaterialID
                LEFT JOIN 
                    FoundationalFilingPeriods ffp
                ON
                    rp.FoundationalFilingPeriodID = ffp.FoundationalFilingPeriodID
                LEFT JOIN 
                    Coordinates C
                ON 
                    C.CoordinateID = rp.CoordinateID
                LEFT JOIN
                    AssetCategories ac
                ON
                    ac.Id = rp.AssetCategoryId
                LEFT JOIN
                    AssetTypes at 
                ON 
                    at.AssetTypeID = rp.AssetTypeID
                LEFT JOIN
                    RPProjectStatuses Stat on Stat.RPProjectStatusID = rp.StatusID"></ResultsDataSource>
        <DetailsView OnDataBound="DetailsView_OnInit">
            <Fields>
                <mapcall:BoundField DataField="RPProjectID" HeaderText="ProjectID" ReadOnly="True" InsertVisible="False" />
                <asp:TemplateField HeaderText="WBS Number">
                    <ItemTemplate><%# Eval("WBSNumber") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txtWBSNumber" Text='<%# Bind("WBSNumber") %>' MaxLength="18" />
                        <asp:CustomValidator runat="server" ID="cvWBSNumber" ClientValidationFunction="CheckActualInServiceDate" ErrorMessage="Required" />
                        <script language="javascript">
                            function CheckActualInServiceDate(sender, args) {
                                args.IsValid = false;
                                if (jQuery('#content_cphMain_cphMain_template_tabView_detailView_txtActualInServiceDate').val() == "") {
                                    args.IsValid = true;
                                }
                                if (jQuery('#content_cphMain_cphMain_template_tabView_detailView_txtActualInServiceDate').val() != "" &&
                                    jQuery('#content_cphMain_cphMain_template_tabView_detailView_txtWBSNumber').val() != "") {
                                    args.IsValid = true;
                                }
                                if (args.IsValid == false) {
                                    jQuery('#content_cphMain_cphMain_template_tabView_detailView_txtWBSNumber').focus();
                                }
                            }
                        </script>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Operating Center">
                    <ItemTemplate>
                        <%# Eval("OperatingCenterCode")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:OperatingCenterDropDownList ID="ddlOps" runat="server" 
                            SelectedValue='<%#Bind("OperatingCenterID") %>' Required="True" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Town">
                    <ItemTemplate><%# Eval("Town") %></ItemTemplate>
                    <EditItemTemplate>
                        <%--This will be a cascading drop-down filtered by opcode --%>
                        <mmsinc:MvpDropDownList runat="server" ID="ddlTown" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlTown" Text="Required" />
                        <atk:CascadingDropDown runat="server" ID="cddTowns" 
                            TargetControlID="ddlTown" ParentControlID="ddlOps" 
                            Category="Town" 
                            EmptyText="None Found" EmptyValue=""
                            PromptText="--Select Here--" PromptValue="" 
                            LoadingText="[Loading Towns...]"
                            ServicePath="~/Modules/Data/DropDowns.asmx" 
                            ServiceMethod="GetTownsByOperatingCenter"
                            SelectedValue='<%# Bind("TownID") %>'                             
                        />
                    </EditItemTemplate>
                </asp:TemplateField>
                <mapcall:BoundField DataField="ProjectTitle" Required="True" MaxLength="255"><ControlStyle Width="450px"/></mapcall:BoundField>
                <mapcall:BoundField DataField="ProjectDescription" Required="True" MaxLength="255"><ControlStyle Width="450px"/></mapcall:BoundField>
                <mapcall:TemplateBoundField HeaderText="Asset Category">
                    <ItemTemplate><%#Eval("AssetCategory") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlAssetCategory" runat="server" Required="True"
                                TableName="AssetCategories"
                                TextFieldName="Description"
                                ValueFieldName="Id" 
                                SelectedValue='<%#Bind("AssetCategoryId") %>' />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField HeaderText="Asset Type">
                    <ItemTemplate><%#Eval("AssetType") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlAssetType" runat="server" Required ="True"
                                TableName="AssetTypes"
                                TextFieldName="Description"
                                ValueFieldName="AssetTypeID"
                                SelectedValue='<%#Bind("AssetTypeID") %>'/>
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>

                <mapcall:BoundField DataField="District" DataType="Int" />
                <mapcall:TemplateBoundField HeaderText="OriginationYear">
                    <ItemTemplate><%#Eval("OriginationYear") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:MvpTextBox runat="server" id="txtOriginationDateEdit" Text='<%#Bind("OriginationYear") %>'/>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <mapcall:MvpTextBox runat="server" id="txtOriginationDate" Text='<%#Bind("OriginationYear")%>'  />
                    </InsertItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataField="HistoricProjectID" MaxLength="14" />
                <mapcall:BoundField DataField="NJAWEstimate" HeaderText="NJAW Estimate (Dollars)" DataType="Int" Required="True" DataFormatString="{0:c0}" ApplyFormatInEditMode="False" />
                <mapcall:TemplateBoundField HeaderText="High Cost Factors">
                    <ItemTemplate>
                        <ul class="list">
                            <asp:Repeater runat="server" DataSourceID="dsHighCostFactors">
                                <ItemTemplate>
                                    <li><%#Eval("Description") %></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <mapcall:McProdDataSource runat="server" ID="dsHighCostFactors"
                            SelectCommand="
                                SELECT
                                    hcf.HighCostFactorID, Description 
                                FROM    
                                    RPProjectsHighCostFactors rphcf
                                JOIN
                                    HighCostFactors hcf on hcf.HighCostFactorID = rphcf.HighCostFactorID
                                WHERE
                                    rphcf.RPProjectID = @RPProjectID">
                            <SelectParameters>
                                <asp:ControlParameter Name="RPProjectID" DbType="Int32"
                                    ControlID="detailView" PropertyName="SelectedValue"/>
                            </SelectParameters>
                        </mapcall:McProdDataSource>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:MultiHighCostFactorsList runat="server" id="mcblHighCostFactors"
                            SelectedSourceFieldName="HighCostFactorID"
                            BaseRole="Field Services_Projects_Read">
                            <SelectedItemsDataSource ID="dsSelectedHighCostFactors" runat="server"
                                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                                SelectCommand="
                                    SELECT
                                        hcf.HighCostFactorID, Description 
                                    FROM    
                                        RPProjectsHighCostFactors rphcf
                                    JOIN
                                        HighCostFactors hcf on hcf.HighCostFactorID = rphcf.HighCostFactorID
                                    WHERE
                                        rphcf.RPProjectID = @RPProjectID">
                                <SelectParameters>
                                    <asp:ControlParameter Name="RPProjectID" DbType="Int32"
                                    ControlID="detailView" PropertyName="SelectedValue"/>
                                </SelectParameters>
                            </SelectedItemsDataSource>
                        </mapcall:MultiHighCostFactorsList>
                        
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField HeaderText="Project Type">
                    <ItemTemplate><%#Eval("ProjectType") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlProjectType" runat="server" Required="True"
                                                        TableName="ProjectTypes"
                                                        TextFieldName="Description"
                                                        ValueFieldName="ProjectTypeID" 
                                                        SelectedValue='<%#Bind("ProjectTypeID") %>' />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                
                <mapcall:BoundField DataField="ProposedLength" HeaderText="Proposed Length (ft)" DataType="Int" Required="True" />
                <mapcall:TemplateBoundField HeaderText="Proposed Diameter">
                    <ItemTemplate><%#Eval("PipeDiameter") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlPipeDiameter" runat="server" Required="True"
                                                        TableName="PipeDiameters"
                                                        TextFieldName="Diameter"
                                                        ValueFieldName="PipeDiameterID" 
                                                        SelectedValue='<%#Bind("ProposedDiameterID") %>' />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField HeaderText="Proposed Pipe Material">
                    <ItemTemplate><%#Eval("PipeMaterial") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlPipeMaterialID" runat="server" Required = "True"
                                                        TableName="PipeMaterials"
                                                        TextFieldName="Description"
                                                        ValueFieldName="PipeMaterialID" 
                                                        SelectedValue='<%#Bind("ProposedPipeMaterialID") %>' />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mmsinc:BoundField TextMode="MultiLine" SqlDataType="Text" DataField="Justification" HeaderText="Justification" Required="true" />
                <mapcall:TemplateBoundField HeaderText="Accelerated Asset Investment Category"
                    HelpText="<a href='DSICReinvestmentCategories.pdf' target='_blank'>Click here</a> for help on this topic.">
                    <ItemTemplate><%#Eval("AcceleratedAssetInvestmentCategory") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlAcceleratedAssetInvestmentCategoryID" runat="server"
                                                        TableName="AssetInvestmentCategories" Required="True"
                                                        TextFieldName="Description"
                                                        ValueFieldName="AssetInvestmentCategoryID" 
                                                        SelectedValue='<%#Bind("AcceleratedAssetInvestmentCategoryID") %>' />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField HeaderText="Secondary Asset Investment Category"
                    HelpText="<a href='DSICReinvestmentCategories.pdf' target='_blank'>Click here</a> for help on this topic.">
                    <ItemTemplate><%#Eval("SecondaryAssetInvestmentCategory") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlSecondaryAssetInvestmentCategoryID" runat="server"
                                                        TableName="AssetInvestmentCategories" Required="False"
                                                        TextFieldName="Description"
                                                        ValueFieldName="AssetInvestmentCategoryID" 
                                                        SelectedValue='<%#Bind("SecondaryAssetInvestmentCategoryID") %>' />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataField="EstimatedProjectDuration" HeaderText="Estimated Project Duration (days)" DataType="Int" />
                <mapcall:BoundField DataField="EstimatedInServiceDate" DataType="Date" />
                <mapcall:BoundField DataField="ActualInServiceDate" DataType="Date" />
                <mapcall:BoundField DataField="EstimatedInServicePeriod" MaxLength="6" ReadOnly="True" InsertVisible="False" />
                
                <mapcall:TemplateBoundField HeaderText="Foundational Filing Period">
                    <ItemTemplate><%#Eval("FoundationalFilingPeriod") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlFoundationalFilingPeriod" runat="server"
                            TableName="FoundationalFilingPeriods"
                            TextFieldName="Description"
                            ValueFieldName="FoundationalFilingPeriodID"
                            SelectedValue='<%#Bind("FoundationalFilingPeriodID") %>'/>
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                
                <mapcall:TemplateBoundField HeaderText="Regulatory Status" InsertVisible="false">
                    <ItemTemplate><%#Eval("RegulatoryStatus") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:MvpHiddenField runat="server" ID="hidRegulatoryStatus" Value='<%#Eval("RPProjectRegulatoryStatusId") %>'/>
                        <mapcall:DataSourceDropDownList ID="ddlRegulatoryStatus" runat="server"
                            TableName="RPProjectRegulatoryStatuses"
                            TextFieldName="Description" ValueFieldName="Id"
                            SelectedValue='<%#Bind("RPProjectRegulatoryStatusId") %>'/>
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>

                <mapcall:TemplateBoundField HeaderText="Status" InsertVisible="False" HelpText="Will always be set to COMPLETE if an Actual In Service Date is entered">
                    <ItemTemplate><%#Eval("Status") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:MvpHiddenField runat="server" ID="hidStatus" Value='<%#Eval("Status") %>' />
                        <mapcall:MvpHiddenField runat="server" ID="hidStatusID" Value='<%#Bind("StatusID") %>' />
                        <br/>
                        <mapcall:MvpDropDownList ID="ddlStatus" runat="server"
                            OnDataBinding="ddlStatus_OnDataBinding">
                            <asp:ListItem Value="" Text="--Select Here--"></asp:ListItem>
                        </mapcall:MvpDropDownList>
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                
                
                <mapcall:BoundField DataField="FinalCriteriaScore" ReadOnly="True" InsertVisible="False" HelpText="Once approved the final criteria score will be calculated and locked." />
                <mapcall:BoundField DataField="FinalRawScore" ReadOnly="True" InsertVisible="False" HelpText="Once approved the final raw score will be calculated and locked." />
                <mmsinc:LatLonPickerField DataField="CoordinateID" Required="True" />
                <mapcall:BoundField DataField="CreatedBy" ReadOnly="True" InsertVisible="False" />
                <mapcall:BoundField DataField="CreatedOn" ReadOnly="True" InsertVisible="False" />
            </Fields>
        </DetailsView>
        <DetailsDataSource
            DeleteCommand="
                -- CASCADES
                DELETE FROM [RPProjectsPipeDataLookupValues] WHERE [RPProjectID] = @RPProjectID; 
                DELETE FROM [RPProjectsHighCostFactors] WHERE [RPProjectID] = @RPProjectID;
                DELETE FROM [RPProjectEndorsements] WHERE [RPProjectID] = @RPProjectID;
                DELETE FROM [RPProjects] WHERE [RPProjectID] = @RPProjectID" 
            InsertCommand="
                -- Don't Allow Approved to be inserted
                Select @StatusID = (select RPProjectStatusID from RPProjectStatuses where Description = 'Proposed')
                INSERT INTO 
                    [RPProjects] ([ProjectTitle], [ProjectDescription], [OperatingCenterID], [District], [OriginationYear], [HistoricProjectID], [NJAWEstimate], [HighCostFactorID], [ProjectTypeID], [ProposedLength], [ProposedDiameterID], [ProposedPipeMaterialID], [Justification], [EstimatedProjectDuration], [EstimatedInServiceDate], [FoundationalFilingPeriodID], [ActualInServiceDate], [CoordinateID], [CreatedBy], [AcceleratedAssetInvestmentCategoryID], [SecondaryAssetInvestmentCategoryID], [StatusID], [TownID], [WBSNumber], [AssetCategoryId], [AssetTypeID]) 
                VALUES 
                    (@ProjectTitle, @ProjectDescription, @OperatingCenterID, @District, @OriginationYear, @HistoricProjectID, @NJAWEstimate, @HighCostFactorID, @ProjectTypeID, @ProposedLength, @ProposedDiameterID, @ProposedPipeMaterialID, @Justification, @EstimatedProjectDuration, @EstimatedInServiceDate, @FoundationalFilingPeriodID, @ActualInServiceDate, @CoordinateID, @CreatedBy, @AcceleratedAssetInvestmentCategoryID, @SecondaryAssetInvestmentCategoryID, @StatusID, @TownID, @WBSNumber, @AssetCategoryId, @AssetTypeID); 
                SELECT 
                    @RPProjectID = (SELECT @@IDENTITY);
                IF (isNull(@CoordinateID,0) != 0)
                    BEGIN
                        UPDATE 
                            Coordinates
                        SET
                            IconID = 
                                CASE WHEN (@StatusID = 2) THEN 30
                                     WHEN (@StatusID = 3) THEN 28
                                     WHEN (@StatusID = 5) THEN 31 
                                     ELSE 29
                                END
                        WHERE 
                            CoordinateID = @CoordinateID
                    END;
                -- Add Default Values
                INSERT INTO [RPProjectsPipeDataLookupValues]
                    SELECT @RPProjectID, PipeDataLookupValueID FROM PipeDataLookupValues where IsEnabled = 1 and IsDefault = 1
                    " 
            SelectCommand="
                SELECT 
                    rp.CreatedBy,
                    rp.CreatedOn,
                    [RPProjectID], 
                    [ProjectTitle],
                    [ProjectDescription], 
                    rp.[OperatingCenterID], 
                    oc.OperatingCenterCode,
                    rp.[TownID], 
                    t.Town,
                    [District], 
                    isNull([OriginationYear], YEAR(rp.CreatedOn)) as OriginationYear, -- default because initially these were not entered
                    [HistoricProjectID], 
                    [NJAWEstimate], 
                    rp.[HighCostFactorID], 
                    rp.[ProjectTypeID], 
                    [ProposedLength], 
                    rp.[ProposedDiameterID], 
                    [ProposedPipeMaterialID], 
                    [Justification],
                    [AcceleratedAssetInvestmentCategoryID],
                    [SecondaryAssetInvestmentCategoryID],
                    [EstimatedProjectDuration], 
                    [EstimatedInServiceDate],
                    [ActualInServiceDate],
                    rp.[FoundationalFilingPeriodID],
                    cast(year(EstimatedInServiceDate) as varchar(4)) + 'Q' + CAST(datepart(qq, EstimatedInServiceDate) as varchar(2)) as [EstimatedInServicePeriod], 
                    rp.[StatusID],
                    [FinalRawScore], 
                    [FinalCriteriaScore],
                    [CoordinateID],
                    hcf.Description as HighCostFactor,
                    pt.Description as ProjectType,
                    pd.Diameter as PipeDiameter,
                    pm.Description as PipeMaterial, 
                    aaic.Description as AcceleratedAssetInvestmentCategory,
                    saic.Description as SecondaryAssetInvestmentCategory,
                    ffp.Description as FoundationalFilingPeriod,
                    rprs.Description as RegulatoryStatus, RPProjectRegulatoryStatusId,
                    s.Description as Status,
                    ac.Description as AssetCategory, AssetCategoryId,
                    at.Description as AssetType, rp.AssetTypeId,
                    rp.WBSNumber,
                    (
                        SELECT COUNT(1) 
                        FROM 
                            [RPProjectsPipeDataLookupValues] ppdlv 
                        JOIN 
                            PipeDataLookupValues pdlv ON pdlv.PipeDataLookupValueID = ppdlv.PipeDataLookupValueID
                        WHERE 
                            ppdlv.RPProjectID = rp.RPProjectID AND pdlv.VariableScore &gt; 0) as NumberOfCriteriaInput
                FROM 
                    [RPProjects] rp
                LEFT JOIN
	                OperatingCenters oc
                ON
	                oc.OperatingCenterID = rp.OperatingCenterID
                LEFT JOIN
                    Towns t
                ON
                    t.TownID = rp.TownID
                LEFT JOIN
                    HighCostFactors hcf
                ON
                    hcf.HighCostFactorID = rp.HighCostFactorID
                LEFT JOIN
                    ProjectTypes pt
                ON
                    pt.ProjectTypeID = rp.ProjectTypeID
                LEFT JOIN
                    PipeDiameters pd
                ON
                    pd.PipeDiameterID = rp.ProposedDiameterID
                LEFT JOIN
                    PipeMaterials pm
                ON
                    pm.PipeMaterialID = rp.ProposedPipeMaterialID
                LEFT JOIN
                    AssetInvestmentCategories aaic
                ON
                    aaic.AssetInvestmentCategoryID = rp.AcceleratedAssetInvestmentCategoryID
                LEFT JOIN
                    AssetInvestmentCategories saic
                ON
                    saic.AssetInvestmentCategoryID = rp.SecondaryAssetInvestmentCategoryID
                LEFT JOIN
                    RPProjectStatuses s
                ON
                    s.RPProjectStatusID = rp.StatusID
                LEFT JOIN 
                    FoundationalFilingPeriods ffp
                ON
                    rp.FoundationalFilingPeriodID = ffp.FoundationalFilingPeriodID
                LEFT JOIN
                    AssetCategories ac
                ON
                    ac.Id = rp.AssetCategoryId
                LEFT JOIN
                    AssetTypes at 
                ON 
                    at.AssetTypeID = rp.AssetTypeID
                LEFT JOIN
                    RPProjectRegulatoryStatuses rprs
                ON
                    rp.RPProjectRegulatoryStatusId = rprs.Id
                WHERE 
                    RPProjectID = @RPProjectID" 
            UpdateCommand="
                -- Score Calculation - Only Calculate if we're setting approved and it was not previously approved.
                DECLARE @ApprovedStatusID int
                select @ApprovedStatusID = (SELECT RPProjectStatusID FROM RPProjectStatuses WHERE Description = 'Approved')
                DECLARE @CurrentlyApproved bit
                SELECT @CurrentlyApproved = (SELECT CASE WHEN ((SELECT [StatusID] FROM RPProjects WHERE [RPProjectID]= @RPProjectID) = @ApprovedStatusID) THEN 1 ELSE 0 END)

                IF (@StatusID = @ApprovedStatusID AND @CurrentlyApproved = 0)
                    BEGIN
                        declare @FinalCriteriaScore decimal(18,2), @FinalRawScore decimal(18,2)
	                    select @FinalCriteriaScore = 
		                    (SELECT	
			                    SUM(Variablescore) / Count(case when pdlv.VariableScore &gt; 0 then 1 else null end) 
		                    FROM 
			                    RPProjects rp 
		                    JOIN
			                    [RPProjectsPipeDataLookupValues] ppdlv on rp.RPProjectID = ppdlv.RPProjectID
		                    JOIN 
			                    PipeDataLookupValues pdlv ON pdlv.PipeDataLookupValueID = ppdlv.PipeDataLookupValueID
		                    WHERE 
			                    ppdlv.RPProjectID = @RPProjectID)
	
	                    select @FinalRawScore = 
		                    (SELECT	
			                    SUM(PriorityWeightedScore) / Count(case when pdlv.VariableScore &gt; 0 then 1 else null end) 
		                    FROM 
			                    RPProjects rp 
		                    JOIN
			                    [RPProjectsPipeDataLookupValues] ppdlv on rp.RPProjectID = ppdlv.RPProjectID
		                    JOIN 
			                    PipeDataLookupValues pdlv ON pdlv.PipeDataLookupValueID = ppdlv.PipeDataLookupValueID
		                    WHERE 
			                    ppdlv.RPProjectID = @RPProjectID)
	
	                    UPDATE
		                    RPProjects
	                    SET
		                    FinalCriteriaScore = @FinalCriteriaScore,
		                    FinalRawScore = @FinalRawScore
	                    FROM 
		                    RPProjects rp 
	                    WHERE 
		                    RPProjectID = @RPProjectID 
                    END

                
                if (@ActualInServiceDate is not null) 
                    select @StatusID = (select RPProjectStatusID from RPProjectStatuses where Description = 'Complete')
                -- Normal Update 
                UPDATE 
                    [RPProjects] 
                SET 
                    [ProjectTitle] = @ProjectTitle, ProjectDescription = @ProjectDescription, [OperatingCenterID] = @OperatingCenterID, 
                    [District] = @District, [OriginationYear] = @OriginationYear, [HistoricProjectID] = @HistoricProjectID, 
                    [NJAWEstimate] = @NJAWEstimate, [HighCostFactorID] = @HighCostFactorID, [ProjectTypeID] = @ProjectTypeID, 
                    [ProposedLength] = @ProposedLength, [ProposedDiameterID] = @ProposedDiameterID, 
                    [ProposedPipeMaterialID] = @ProposedPipeMaterialID, [Justification] = @Justification, 
                    [EstimatedProjectDuration] = @EstimatedProjectDuration, 
                    [EstimatedInServiceDate] = @EstimatedInServiceDate, 
                    [FoundationalFilingPeriodID] = @FoundationalFilingPeriodID,
                    [ActualInServiceDate] = @ActualInServiceDate, 
                    [CoordinateID] = @CoordinateID, [AcceleratedAssetInvestmentCategoryID] = @AcceleratedAssetInvestmentCategoryID,
                    [SecondaryAssetInvestmentCategoryID] = @SecondaryAssetInvestmentCategoryID, [StatusID] = @StatusID,
                    [TownID] = @TownID, [WBSNumber] = @WBSNumber, [RPProjectRegulatoryStatusId] = @RPProjectRegulatoryStatusId,
                    [AssetCategoryId] = @AssetCategoryId, [AssetTypeID] = @AssetTypeID
                WHERE 
                    [RPProjectID] = @RPProjectID;
                IF (isNull(@CoordinateID,0) != 0)
                    BEGIN
                        UPDATE 
                            Coordinates
                        SET
                            IconID = 
                                CASE WHEN (@StatusID = 2) THEN 30
                                     WHEN (@StatusID = 3) THEN 28
                                     WHEN (@StatusID = 5) THEN 31 
                                     ELSE 29
                                END
                        WHERE 
                            CoordinateID = @CoordinateID
                    END    
                ">
            <SelectParameters>
                <asp:Parameter Name="RPProjectID" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="RPProjectID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ProjectTitle" Type="String" />
                <asp:Parameter Name="ProjectDescription" Type="String" />
                <asp:Parameter Name="OperatingCenterID" Type="Int32" />
                <asp:Parameter Name="District" Type="Int32" />
                <asp:Parameter Name="OriginationYear" Type="Int32" DefaultValue="2010" />
                <asp:Parameter Name="HistoricProjectID" Type="String" />
                <asp:Parameter Name="NJAWEstimate" Type="Int32" />
                <asp:Parameter Name="HighCostFactorID" Type="Int32" />
                <asp:Parameter Name="ProjectTypeID" Type="Int32" />
                <asp:Parameter Name="ProposedLength" Type="Int32" />
                <asp:Parameter Name="ProposedDiameterID" Type="Int32" />
                <asp:Parameter Name="ProposedPipeMaterialID" Type="Int32" />
                <asp:Parameter Name="Justification" Type="String" />
                <asp:Parameter Name="EstimatedProjectDuration" Type="Int32" />
                <asp:Parameter Name="EstimatedInServiceDate" Type="DateTime" />
                <asp:Parameter Name="FoundationalFilingPeriodID" Type="Int32" />
                <asp:Parameter Name="ActualInServiceDate" Type="DateTime" />
                <asp:Parameter Name="FinalScore" Type="Decimal" />
                <asp:Parameter Name="CoordinateID" Type="Int32" />
                <asp:Parameter Name="RPProjectID" Type="Int32" Direction="Output" />
                <asp:Parameter Name="CreatedBy" Type="String" />
                <asp:Parameter Name="AcceleratedAssetInvestmentCategoryID" Type="Int32" />
                <asp:Parameter Name="SecondaryAssetInvestmentCategoryID" Type="Int32" />
                <asp:Parameter Name="StatusID" Type="Int32" />
                <asp:Parameter Name="TownID" Type="Int32" />
                <asp:Parameter Name="WBNumber" Type="String" />
                <asp:Parameter Name="AssetCategoryId" Type="Int32" />
                <asp:Parameter Name="AssetTypeID" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="ProjectTitle" Type="String" />
                <asp:Parameter Name="ProjectDescription" Type="String" />
                <asp:Parameter Name="OperatingCenterID" Type="Int32" />
                <asp:Parameter Name="District" Type="Int32" />
                <asp:Parameter Name="OriginationYear" Type="Int32" />
                <asp:Parameter Name="HistoricProjectID" Type="String" />
                <asp:Parameter Name="NJAWEstimate" Type="Int32" />
                <asp:Parameter Name="HighCostFactorID" Type="Int32" />
                <asp:Parameter Name="ProjectTypeID" Type="Int32" />
                <asp:Parameter Name="ProposedLength" Type="Int32" />
                <asp:Parameter Name="ProposedDiameterID" Type="Int32" />
                <asp:Parameter Name="ProposedPipeMaterialID" Type="Int32" />
                <asp:Parameter Name="Justification" Type="String" />
                <asp:Parameter Name="EstimatedProjectDuration" Type="Int32" />
                <asp:Parameter Name="EstimatedInServiceDate" Type="DateTime" />
                <asp:Parameter Name="FoundationalFilingPeriodID" Type="Int32" />
                <asp:Parameter Name="ActualInServiceDate" Type="DateTime" />
                <asp:Parameter Name="FinalScore" Type="Decimal" />
                <asp:Parameter Name="CoordinateID" Type="Int32" />
                <asp:Parameter Name="RPProjectID" Type="Int32" />
                <asp:Parameter Name="AcceleratedAssetInvestmentCategoryID" Type="Int32" />
                <asp:Parameter Name="SecondaryAssetInvestmentCategoryID" Type="Int32" />
                <asp:Parameter Name="StatusID" Type="Int32" />
                <asp:Parameter Name="TownID" Type="Int32" />
                <asp:Parameter Name="WBNumber" Type="String" />
                <asp:Parameter Name="RPProjectRegulatoryStatusId" Type="Int32"  />
                <asp:Parameter Name="AssetCategoryId" Type="Int32" />
                <asp:Parameter Name="AssetTypeID" Type="Int32" />
            </UpdateParameters>
        </DetailsDataSource>
        
        <Tabs>
            <mapcall:Tab ID="altExistingPipes" Label="Pipe Data"
                VisibleDuringInsert="False" VisibleDuringUpdate="False">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <mapcall:MvpGridView runat="server" ID="gvRPProjectsPipeDataLookupValues"
                            DataKeyNames="RPProjectID, RPProjectsPipeDataLookupValueID"
                            ShowFooter="True" AutoGenerateColumns="False"
                            OnRowDataBound="gvRPProjectsPipeDataLookupValues_RowDataBound"
                            OnDataBinding="gvRPProjectsPipeDataLookupValues_DataBinding"
                            OnRowUpdating="gvRPProjectsPipeDataLookupValues_RowUpdating"
                            DataSourceID="dsRPProjectsPipeDataLookupValues">
                            <Columns>
                                <%--Buttons--%>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lbEdit" Text="Edit" CommandName="Edit" 
                                            Visible='<%#Permissions.EditAccess.IsAllowed %>' ValidationGroup="UpdatePipeDataLookupValue" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" ID="lbUpdate" Text="Update" CommandName="Update" ValidationGroup="UpdatePipeDataLookupValue" />
                                        <asp:LinkButton runat="server" ID="lbCancel" Text="Cancel" CausesValidation="False" CommandName="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <%--Type--%>
                                <mapcall:TemplateBoundField HeaderText="Type">
                                    <ItemTemplate><%#Eval("LookupType") %></ItemTemplate>
                                   <EditItemTemplate>
                                        <mapcall:DataSourceDropDownList Enabled="False" ID="ddlEditPipeDataLookupType" runat="server" 
                                            TableName="PipeDataLookupTypes"
                                            TextFieldName="Description" 
                                            ValueFieldName="PipeDataLookupTypeID" 
                                            SelectedValue='<%#Bind("PipeDataLookupTypeID") %>' />
                                    </EditItemTemplate>
                                </mapcall:TemplateBoundField>
                                <%--Value--%>
                                <mapcall:TemplateBoundField HeaderText="Value">
                                    <ItemTemplate><%#Eval("LookupValue") %></ItemTemplate>
                                    <EditItemTemplate>
                                        <mapcall:MvpDropDownList runat="server" ID="ddlEditPipeDataLookupValue" /> 
                                        <atk:CascadingDropDown runat="server" ID="cddlContractor"
                                            TargetControlID="ddlEditPipeDataLookupValue" 
                                            ParentControlID="ddlEditPipeDataLookupType"
                                            Category="PipeDataLookupValue" SelectedValue='<%#Bind("PipeDataLookupValueID") %>'
                                            EmptyText="None Found" EmptyValue=""
                                            PromptText="--Select Here--"
                                            LoadingText="[Loading Lookup Values...]"
                                            ServicePath="~/Modules/Data/DropDowns.asmx"
                                            ServiceMethod="GetPipeDataLookupValueByPipeDataLookupType"/>   
                                        <asp:RequiredFieldValidator runat="server" ValidationGroup="UpdatePipeDataLookupValue"
                                            ControlToValidate="ddlEditPipeDataLookupValue" Text="Required" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblVariableScoreCount"></asp:Label>
                                    </FooterTemplate>
                                </mapcall:TemplateBoundField>
                                <%--Variable Score--%>
                                <mapcall:TemplateBoundField HeaderText="Variable Score">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblVariableScore" Text='<%#Eval("VariableScore") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblVariableScore" />
                                    </FooterTemplate>
                                </mapcall:TemplateBoundField>
                                <%--Priority Weighted Score--%>
                                <mapcall:TemplateBoundField HeaderText="Priority Weighted Score">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblPriorityWeightedScore"
                                            Text='<%#Eval("PriorityWeightedScore") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label runat="server" ID="lblPriorityWeightedScore"/>
                                    </FooterTemplate>
                                </mapcall:TemplateBoundField>
                                <%--Commands--%>
                            </Columns>
                        </mapcall:MvpGridView>
                        <br/><br/>
                    
                        <mapcall:McProdDataSource runat="server" 
                            ID="dsRPProjectsPipeDataLookupValues"
                            SelectCommand="
                                SELECT	
                                    ppdl.*,
                                    pdlt.Description as LookupType,
                                    pdlt.PipeDataLookupTypeID,
                                    pdlv.Description as LookupValue,
                                    pdlv.VariableScore, 
                                    pdlv.PriorityWeightedScore
                                FROM 
	                                [RPProjectsPipeDataLookupValues] ppdl
                                LEFT JOIN
	                                [RPProjects] p on p.RPProjectID = ppdl.RPProjectID
                                LEFT JOIN
	                                PipeDataLookupValues pdlv on pdlv.PipeDataLookupValueID = ppdl.PipeDataLookupValueID
                                LEFT JOIN
	                                PipeDataLookupTypes pdlt on pdlt.PipeDataLookupTypeID = pdlv.PipeDataLookupTypeID
                                WHERE
                                    ppdl.RPProjectID = @RPProjectID"
                            InsertCommand="
                                INSERT INTO [RPProjectsPipeDataLookupValues]
                                    ([RPProjectID], [PipeDataLookupValueID]) 
                                VALUES
                                    (@RPProjectID, @PipeDataLookupValueID)"
                            UpdateCommand="
                                UPDATE 
                                    [RPProjectsPipeDataLookupValues]
                                SET
                                    [PipeDataLookupValueID] = @PipeDataLookupValueID
                                WHERE
                                    [RPProjectsPipeDataLookupValueID] = @RPProjectsPipeDataLookupValueID"
                            DeleteCommand="
                                DELETE FROM 
                                    [RPProjectsPipeDataLookupValues] 
                                WHERE 
                                    [RPProjectsPipeDataLookupValueID] = @RPProjectsPipeDataLookupValueID">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="detailView" PropertyName="SelectedValue" Name="RPProjectID" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:ControlParameter ControlID="detailView" PropertyName="SelectedValue" Name="RPProjectID" Type="Int32" />
                                <asp:Parameter Name="PipeDataLookupValueID" DbType="Int32"/>
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="RPProjectsPipeDataLookupValueID" DbType="Int32"/>
                                <asp:Parameter Name="PipeDataLookupValueID" DbType="Int32"/>
                            </UpdateParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="RPProjectsPipeDataLookupValueID" DbType="Int32"/>
                            </DeleteParameters>
                        </mapcall:McProdDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </mapcall:Tab>
            <mapcall:Tab ID="tabEndorsements" Label="Endorsements"
                VisibleDuringInsert="False" VisibleDuringUpdate="False">
                <asp:UpdatePanel runat="server" ID="pnlEndorsements">
                    <ContentTemplate>
                        <mapcall:MvpGridView runat="server" ID="gvRPProjectEndorsements"
                            DataSourceID="dsRPProjectEndorsements"
                            DataKeyNames="RPProjectEndorsementID"
                            AutoGenerateColumns="False" >
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lbEdit" Text="Edit" CommandName="Edit" Visible='<%#Permissions.EditAccess.IsAllowed %>' ValidationGroup="EndorsementList" />
                                        <asp:LinkButton runat="server" ID="lbDelete" Text="Delete" CommandName="Delete" Visible='<%#Permissions.DeleteAccess.IsAllowed %>' 
                                            ValidationGroup="EndorsementList"
                                            OnClientClick="return confirm('Are you sure you want to delete this record?');"  />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" ID="lbUpdate" Text="Update" CommandName="Update" ValidationGroup="EndorsementList" />
                                        <asp:LinkButton runat="server" ID="lbCancel" Text="Cancel" CausesValidation="False" CommandName="Cancel" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <%--Employee--%>
                                <mapcall:TemplateBoundField HeaderText="Employee">
                                    <ItemTemplate><%#Eval("tblEmployeeID") %></ItemTemplate>
                                </mapcall:TemplateBoundField>
                                <%--Endorsement Status--%>
                                <mapcall:TemplateBoundField HeaderText="Endorsement Status">
                                    <ItemTemplate><%#Eval("EndorsementStatus") %></ItemTemplate>
                                    <EditItemTemplate>
                                        <mapcall:DataSourceDropDownList runat="server" ID="ddlEndorsementStatus"
                                            SelectCommand="SELECT * FROM EndorsementStatuses"
                                            DataTextField="Description"
                                            DataValueField="EndorsementStatusID"
                                            SelectedValue='<%#Bind("EndorsementStatusID") %>'
                                            />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDdlEndorsementStatus"
                                            ValidationGroup="EndorsementStatusEdit"
                                            SetFocusOnError="True" InitialValue="" ErrorMessage="Required" Display="Dynamic"
                                            ControlToValidate="ddlEndorsementStatus" />
                                    </EditItemTemplate>
                                </mapcall:TemplateBoundField>
                                <%--Date--%>
                                <mapcall:BoundField DataType="DateTime" DataField="EndorsementDate" Required="True" />
                                <%--Comment--%>
                                <mapcall:BoundField DataType="Text" DataField="Comment" />
                            </Columns>
                        </mapcall:MvpGridView>
                
                        <asp:Panel runat="server" ID="pnlProjectEndorsements" 
                            Visible='<%#Permissions.CreateAccess.IsAllowed %>' >
                            <h4>Add Endorsement</h4>
                            <mapcall:MvpDetailsView runat="server" ID="dvRPProjectEndorsements"
                                DefaultMode="Insert" 
                                DataSourceID="dsRPProjectEndorsements" CssClass=""
                                OnItemInserting="dvRPProjectEndorsements_Inserting"
                                AutoGenerateRows="False">
                                <Fields>
                                    <mapcall:TemplateBoundField HeaderText="Endorsement Status">
                                        <ItemTemplate><%#Eval("EndorsementStatus") %></ItemTemplate>
                                        <InsertItemTemplate>
                                            <mapcall:DataSourceDropDownList runat="server" ID="ddlEndorsementStatus"
                                                SelectCommand="SELECT * FROM EndorsementStatuses"
                                                DataTextField="Description"
                                                DataValueField="EndorsementStatusID"
                                                SelectedValue='<%#Bind("EndorsementStatusID") %>'
                                                />
                                            <asp:RequiredFieldValidator runat="server" ID="rfvDdlEndorsementStatus"
                                                ValidationGroup="EndorsementInsert"
                                                SetFocusOnError="True" InitialValue="" ErrorMessage="Required" Display="Dynamic"
                                                ControlToValidate="ddlEndorsementStatus" />
                                        </InsertItemTemplate>
                                    </mapcall:TemplateBoundField>
                                    <%--Date--%>
                                    
                                    <%--Comment--%>
                                    <mapcall:BoundField DataType="Text" DataField="Comment" />
                                    <asp:TemplateField>
                                        <InsertItemTemplate>
                                            <asp:LinkButton runat="server" ID="lbInsert" CommandName="Insert" 
                                                Text="Insert" ValidationGroup="EndorsementInsert"/>
                                        </InsertItemTemplate>
                                    </asp:TemplateField>
                                </Fields>
                            </mapcall:MvpDetailsView>
                        </asp:Panel>

                        <mapcall:McProdDataSource runat="server" ID="dsRPProjectEndorsements"
                            SelectCommand="
                                SELECT
                                    pe.*, 
                                    es.Description as EndorsementStatus
                                FROM
                                    RPProjectEndorsements pe
                                JOIN
                                    EndorsementStatuses es on es.EndorsementStatusID = pe.EndorsementStatusID
                                WHERE
                                    RPProjectID = @RPProjectID"
                            InsertCommand="
                                INSERT INTO RPProjectEndorsements(RPProjectID, tblEmployeeID, EndorsementStatusID, EndorsementDate, Comment)
                                    VALUES(@RPProjectID, @tblEmployeeID, @EndorsementStatusID, getDate(), @Comment)"
                            UpdateCommand="
                                UPDATE 
                                    RPProjectEndorsements 
                                SET
                                    EndorsementStatusID = @EndorsementStatusID,
                                    EndorsementDate = @EndorsementDate, 
                                    Comment = @Comment
                                WHERE 
                                    RPProjectEndorsementID = @RPProjectEndorsementID"
                            DeleteCommand="
                                DELETE FROM 
                                    RPProjectEndorsements 
                                WHERE 
                                    RPProjectEndorsementID = @RPProjectEndorsementID">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="detailView" PropertyName="SelectedValue" Name="RPProjectID" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:ControlParameter ControlID="detailView" PropertyName="SelectedValue" Name="RPProjectID" Type="Int32" />
                                <asp:Parameter Name="tblEmployeeID" DbType="String" />
                                <asp:Parameter Name="EndorsementStatusID" DbType="Int32" />
                                
                                <asp:Parameter Name="Comment" DbType="String" />
                            </InsertParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="RPProjectEndorsementID" DbType="Int32"/>
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="RPProjectEndorsementID" DbType="Int32"/>
                                <asp:Parameter Name="EndorsementStatusID" DbType="Int32" />
                                <asp:Parameter Name="EndorsementDate" DbType="DateTime" />
                                <asp:Parameter Name="Comment" DbType="String" />
                            </UpdateParameters>
                        </mapcall:McProdDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </mapcall:Tab>
        </Tabs>

    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>
