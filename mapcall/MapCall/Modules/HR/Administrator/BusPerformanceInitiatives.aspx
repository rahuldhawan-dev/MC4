<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="BusPerformanceInitiatives.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.BusPerformanceInitiatives" Title="Performance Initiatives" Theme="bender" %>
<%@ Register Src="~/Controls/HR/Hyperlinks.ascx" TagName="Hyperlinks" TagPrefix="mmsi" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls" TagPrefix="mapcall" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.SearchFields" TagPrefix="search" %>
<%@ Register Src="~/Controls/DetailsViewDataPageTemplate.ascx" TagPrefix="mapcall" TagName="DetailsViewDataPageTemplate" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls.DropDowns" Assembly="MapCall" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Performance Initiatives
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
       
    <mapcall:DetailsViewDataPageTemplate ID="template" runat="server"
        DataElementTableName="tblBusinessPerformance_Initiatives"
        DataElementPrimaryFieldName="Initiative_ID"
        Label="Initiative"
        DataTypeId="59">
        <SearchBox runat="server">
            <Fields>
                <search:TextSearchField DataFieldName="Initiative_Summary" />
                <search:DropDownSearchField 
                    Label="Initiative Focus"
                    DataFieldName="t.Initiative_Focus"
                    TextFieldName="Txt"
                    ValueFieldName="Val"
                    SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Initiative_Focus' order by 2"
                    />
                <search:DropDownSearchField 
                    Label="Initiative Grouping"
                    DataFieldName="t.Initiative_Grouping"
                    TextFieldName="Txt"
                    ValueFieldName="Val"
                    SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Initiative_Grouping' order by 2"
                    />
                <search:DropDownSearchField 
                    Label="Implementation Horizon"
                    DataFieldName="t.Implementation_Horizon"
                    TextFieldName="Txt"
                    ValueFieldName="Val"
                    SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Implementation_Horizon' order by 2"
                    />
                <search:DropDownSearchField 
                    Label="Priority Ranking"
                    DataFieldName="t.Priority_Ranking"
                    TextFieldName="Txt"
                    ValueFieldName="Val"
                    SelectCommand="select distinct Priority_Ranking as Val, Priority_Ranking as txt from tblBusinessPerformance_Initiatives order by 2"
                    />
                <search:DropDownSearchField 
                    Label="Goal"
                    DataFieldName="g.goalId"
                    TableName="tblBusinessPerformance_Goals"
                    TextFieldName="Goal"
                    ValueFieldName="Goal_ID"
                    />
                <search:DateTimeSearchField DataFieldName="Start_Date" />
                <search:DateTimeSearchField DataFieldName="Date_Completed" />
            </Fields>
        </SearchBox>

        <ResultsGridView>
            <Columns>
                <mapcall:BoundField DataField="Initiative_ID" />
                <mapcall:BoundField DataField="Initiative_Focus" />
                <mapcall:BoundField DataField="Initiative_Grouping" />
                <mapcall:BoundField DataField="Implementation_Horizon" />
                <mapcall:BoundField DataField="Initiative_Summary" />
                <mapcall:BoundField DataField="CriticalSuccessFactors" />
                <mapcall:BoundField DataField="Priority_Ranking" />
                <mapcall:BoundField DataField="Obsticles" />
                <mapcall:BoundField DataField="Approach" />
                <mapcall:BoundField DataField="Resource_Requirements" />
                <mapcall:BoundField DataField="Change_Requirements" />
                <mapcall:BoundField DataField="Estimated_Cost" />
                <mapcall:BoundField DataField="Estimated_Annual_Savings" />
                <mapcall:BoundField DataField="ROI_Years" />
                <mapcall:BoundField DataField="Estimated_Start_Date" />
                <mapcall:BoundField DataField="Estimated_Completion_Date" />
                <mapcall:BoundField DataField="Start_Date" />
                <mapcall:BoundField DataField="Date_Completed" />
                <mapcall:BoundField DataField="Percent_Complete" />
                <mapcall:BoundField DataField="Date_Percent_Complete_Updated" />
                <mapcall:BoundField DataField="Administration" />
                <mapcall:BoundField DataField="Human_Resources" />
                <mapcall:BoundField DataField="Engineering" />
                <mapcall:BoundField DataField="Environmental" />
                <mapcall:BoundField DataField="Finance" />
                <mapcall:BoundField DataField="Production" />
                <mapcall:BoundField DataField="Water_Quality" />
                <mapcall:BoundField DataField="Field_Operations" />
                <mapcall:BoundField DataField="Call_Center" />
                <mapcall:BoundField DataField="InitiativeFocusID"/>
                <mapcall:BoundField DataField="ImplementationHorizonID" />
                <mapcall:BoundField DataField="InitiativeGroupingID" />
            </Columns>
        </ResultsGridView>

        <ResultsDataSource SelectCommand="
            SELECT
                t.[Initiative_ID],
                t.[Initiative_Summary],
                t.[CriticalSuccessFactors],
                t.[Priority_Ranking],
                t.[Obsticles],
                t.[Approach],
                t.[Resource_Requirements],
                t.[Change_Requirements],
                t.[Estimated_Cost],
                t.[Estimated_Annual_Savings],
                t.[ROI_Years],
                t.[Estimated_Start_Date],
                t.[Estimated_Completion_Date],
                t.[Start_Date],
                t.[Date_Completed],
                t.[Percent_Complete],
                t.[Date_Percent_Complete_Updated],
                t.[Administration],
                t.[Human_Resources],
                t.[Engineering],
                t.[Environmental],
                t.[Finance],
                t.[Production],
                t.[Water_Quality],
                t.[Field_Operations],
                t.[Call_Center],
                t.[Initiative_Focus] AS [InitiativeFocusID],
                t.[Implementation_Horizon] AS [ImplementationHorizonID],
                t.[Initiative_Grouping] AS [InitiativeGroupingID],
                l.[LookupValue] AS [Initiative_Grouping],
                l1.[LookupValue] AS [Implementation_Horizon],
                l2.[LookupValue] AS [Initiative_Focus]
            FROM
                [tblbusinessperformance_initiatives] AS t
            LEFT JOIN                [Lookup] AS l            ON                t.[Initiative_Grouping] = l.[LookupID]   
            LEFT JOIN                [Lookup] AS l1            ON                t.[Implementation_Horizon] = l1.[LookupID]
            LEFT JOIN                [Lookup] AS l2            ON                t.[Initiative_Focus] = l2.[LookupID]
            LEFT JOIN [tblBusinessPerformanceInitiativesGoals] g  ON g.initiativeID = t.Initiative_ID">
        </ResultsDataSource>

        <DetailsView>
            <Fields>
                <mapcall:BoundField DataField="Initiative_ID" ReadOnly="true" InsertVisible="false" />
                <mapcall:TemplateBoundField HeaderText="Initiative Focus">
                    <ItemTemplate><%# Eval("Initiative_Focus_text") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlFocus" runat="server" 
                            TextFieldName="Txt"
                            ValueFieldName="Val"
                            SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Initiative_Focus' order by 2"
                            SelectedValue='<%#Bind("Initiative_Focus") %>'
                        />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField HeaderText="Initiative Grouping">
                    <ItemTemplate><%# Eval("Initiative_Grouping_text") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlGrouping" runat="server" 
                            TextFieldName="Txt"
                            ValueFieldName="Val"
                            SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Initiative_Grouping' order by 2"
                            SelectedValue='<%#Bind("Initiative_Grouping") %>'
                        />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField HeaderText="Sequence">
                    <ItemTemplate><%# Eval("Sequence_text") %></ItemTemplate>      
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlSequence" runat="server" 
                            TextFieldName="Txt"
                            ValueFieldName="Val"
                            SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Sequence' order by 2"
                            SelectedValue='<%#Bind("Sequence") %>'
                        />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:TemplateBoundField HeaderText="Implementation Horizon">
                    <ItemTemplate><%# Eval("Implementation_Horizon_text") %></ItemTemplate>   
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlImplementationHorizon" runat="server" 
                            TextFieldName="Txt"
                            ValueFieldName="Val"
                            SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Implementation_Horizon' order by 2"
                            SelectedValue='<%#Bind("Implementation_Horizon") %>'
                        />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataField="Initiative_Summary" MaxLength="255">
                    <TextBoundFieldOptions RequiresMultiLineTextBox="true" />
                </mapcall:BoundField>
                <mapcall:BoundField DataField="CriticalSuccessFactors" MaxLength="255">
                    <TextBoundFieldOptions RequiresMultiLineTextBox="true" />
                </mapcall:BoundField>
                <mapcall:TemplateBoundField HeaderText="BSC Area">
                    <ItemTemplate><%# Eval("BSCArea_text") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlBSCArea" runat="server" 
                            TextFieldName="Txt"
                            ValueFieldName="Val"
                            SelectCommand="select [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'BSCArea' And [TableName] = 'tblBusinessPerformance_Initiatives' order by 2"
                            SelectedValue='<%#Bind("BSCArea") %>'
                        />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>
                <mapcall:BoundField DataField="Priority_Ranking" DataType="Float" />

                <mapcall:TemplateBoundField HeaderText="Impact">
                    <ItemTemplate><%# Eval("Impact_text") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlImpact" runat="server" 
                            TextFieldName="Txt"
                            ValueFieldName="Val"
                            SelectCommand="select [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Impact' And [TableName] = 'tblBusinessPerformance_Initiatives' order by 2"
                            SelectedValue='<%#Bind("Impact") %>'
                        />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>                
                
                <mapcall:TemplateBoundField HeaderText="Effort">
                    <ItemTemplate><%# Eval("Effort_text") %></ItemTemplate>
                    <EditItemTemplate>
                        <mapcall:DataSourceDropDownList ID="ddlEffort" runat="server" 
                            TextFieldName="Txt"
                            ValueFieldName="Val"
                            SelectCommand="select [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Effort' And [TableName] = 'tblBusinessPerformance_Initiatives' order by 2"
                            SelectedValue='<%#Bind("Effort") %>'
                        />
                    </EditItemTemplate>
                </mapcall:TemplateBoundField>                
                
                <mapcall:BoundField DataField="EIResult" DataType="Int" />

                <mapcall:BoundField DataField="Administration" DataType="Bit" />
                <mapcall:BoundField DataField="Human_Resources" DataType="Bit" />
                <mapcall:BoundField DataField="Engineering" DataType="Bit" />
                <mapcall:BoundField DataField="Environmental" DataType="Bit" />
                <mapcall:BoundField DataField="Finance" DataType="Bit" />
                <mapcall:BoundField DataField="Production" DataType="Bit" />
                <mapcall:BoundField DataField="Water_Quality" DataType="Bit" />
                <mapcall:BoundField DataField="Field_Operations" DataType="Bit" />
                <mapcall:BoundField DataField="Call_Center" DataType="Bit" />
                <mapcall:BoundField DataField="Obsticles" MaxLength="255">
                    <TextBoundFieldOptions RequiresMultiLineTextBox="true" />
                </mapcall:BoundField>
                <mapcall:BoundField DataField="Approach" MaxLength="255">
                    <TextBoundFieldOptions RequiresMultiLineTextBox="true" />
                </mapcall:BoundField>
                <mapcall:BoundField DataField="Resource_Requirements" MaxLength="255">
                    <TextBoundFieldOptions RequiresMultiLineTextBox="true" />
                </mapcall:BoundField>
                <mapcall:BoundField DataField="Change_Requirements" MaxLength="255">
                    <TextBoundFieldOptions RequiresMultiLineTextBox="true" />
                </mapcall:BoundField>
                <mapcall:BoundField DataField="Estimated_Cost" MaxLength="255" />
                <mapcall:BoundField DataField="Estimated_Annual_Savings" MaxLength="255" />
                <mapcall:BoundField DataField="ROI_Years" MaxLength="255" />
                <mapcall:BoundField DataField="Estimated_Start_Date" MaxLength="255" />
                <mapcall:BoundField DataField="Estimated_Completion_Date" MaxLength="255" />
                <mapcall:BoundField DataField="Start_Date" DataType="DateTime" />
                <mapcall:BoundField DataField="Date_Completed" DataType="DateTime" />
                <mapcall:BoundField DataField="Percent_Complete" MaxLength="255" />
                <mapcall:BoundField DataField="Date_Percent_Complete_Updated" MaxLength="255" />
            </Fields>
        </DetailsView>

        <DetailsDataSource 
            DeleteCommand="DELETE FROM [tblBusinessPerformance_Initiatives] WHERE [Initiative_ID] = @Initiative_ID" 
            InsertCommand="INSERT INTO [tblBusinessPerformance_Initiatives] ([Initiative_Focus], [Initiative_Grouping], [Sequence], [Implementation_Horizon], [Initiative_Summary], [CriticalSuccessFactors], [BSCArea], [Priority_Ranking], [Administration], [Human_Resources], [Engineering], [Environmental], [Finance], [Production], [Water_Quality], [Field_Operations], [Call_Center], [Obsticles], [Approach], [Resource_Requirements], [Change_Requirements], [Estimated_Cost], [Estimated_Annual_Savings], [ROI_Years], [Estimated_Start_Date], [Estimated_Completion_Date], [Start_Date], [Date_Completed], [Percent_Complete], [Date_Percent_Complete_Updated], [Impact], [Effort], [EIResult]) VALUES (@Initiative_Focus, @Initiative_Grouping, @Sequence, @Implementation_Horizon, @Initiative_Summary, @CriticalSuccessFactors, @BSCArea, @Priority_Ranking, @Administration, @Human_Resources, @Engineering, @Environmental, @Finance, @Production, @Water_Quality, @Field_Operations, @Call_Center, @Obsticles, @Approach, @Resource_Requirements, @Change_Requirements, @Estimated_Cost, @Estimated_Annual_Savings, @ROI_Years, @Estimated_Start_Date, @Estimated_Completion_Date, @Start_Date, @Date_Completed, @Percent_Complete, @Date_Percent_Complete_Updated, @Impact, @Effort, @EIResult); SELECT @Initiative_ID = (SELECT @@IDENTITY)" 
            UpdateCommand="UPDATE [tblBusinessPerformance_Initiatives] SET [Initiative_Focus] = @Initiative_Focus, [Initiative_Grouping] = @Initiative_Grouping, [Sequence] = @Sequence, [Implementation_Horizon] = @Implementation_Horizon, [Initiative_Summary] = @Initiative_Summary, [CriticalSuccessFactors] = @CriticalSuccessFactors, [BSCArea] = @BSCArea, [Priority_Ranking] = @Priority_Ranking, [Administration] = @Administration, [Human_Resources] = @Human_Resources, [Engineering] = @Engineering, [Environmental] = @Environmental, [Finance] = @Finance, [Production] = @Production, [Water_Quality] = @Water_Quality, [Field_Operations] = @Field_Operations, [Call_Center] = @Call_Center, [Obsticles] = @Obsticles, [Approach] = @Approach, [Resource_Requirements] = @Resource_Requirements, [Change_Requirements] = @Change_Requirements, [Estimated_Cost] = @Estimated_Cost, [Estimated_Annual_Savings] = @Estimated_Annual_Savings, [ROI_Years] = @ROI_Years, [Estimated_Start_Date] = @Estimated_Start_Date, [Estimated_Completion_Date] = @Estimated_Completion_Date, [Start_Date] = @Start_Date, [Date_Completed] = @Date_Completed, [Percent_Complete] = @Percent_Complete, [Date_Percent_Complete_Updated] = @Date_Percent_Complete_Updated, [Impact] = @Impact, [Effort] = @Effort, [EIResult] = @EIResult WHERE [Initiative_ID] = @Initiative_ID"
            SelectCommand="SELECT 
                               [tblBusinessPerformance_Initiatives].*,
                               #lookup1.[LookupValue] AS [Initiative_Grouping_text],
                               #lookup2.[LookupValue] AS [Implementation_Horizon_text],
                               #lookup3.[LookupValue] AS [Initiative_Focus_text],
                               #lookup4.[LookupValue] AS [Sequence_text],
                               #lookup5.[LookupValue] AS [BSCArea_text],
                               #lookup6.[LookupValue] AS [Impact_text],
                               #lookup7.[LookupValue] AS [Effort_text]
                          FROM [tblBusinessPerformance_Initiatives]
                          LEFT JOIN [Lookup] AS #lookup1 ON #lookup1.[LookupID] = [tblBusinessPerformance_Initiatives].[Initiative_Grouping]
                          LEFT JOIN [Lookup] AS #lookup2 ON #lookup2.[LookupID] = [tblBusinessPerformance_Initiatives].[Implementation_Horizon]
                          LEFT JOIN [Lookup] AS #lookup3 ON #lookup3.[LookupID] = [tblBusinessPerformance_Initiatives].[Initiative_Focus]
                          LEFT JOIN [Lookup] AS #lookup4 ON #lookup4.[LookupID] = [tblBusinessPerformance_Initiatives].[Sequence]
                          LEFT JOIN [Lookup] AS #lookup5 ON #lookup5.[LookupID] = [tblBusinessPerformance_Initiatives].[BSCArea]
                          LEFT JOIN [Lookup] AS #lookup6 ON #lookup6.[LookupID] = [tblBusinessPerformance_Initiatives].[Impact]
                          LEFT JOIN [Lookup] AS #lookup7 ON #lookup7.[LookupID] = [tblBusinessPerformance_Initiatives].[Effort]
                          WHERE
                              [tblBusinessPerformance_Initiatives].[Initiative_ID] = @Initiative_ID">
            <DeleteParameters>
                <asp:Parameter Name="Initiative_ID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="Initiative_ID" Type="Int32" Direction="Output" />
                <asp:Parameter Name="Initiative_Focus" Type="Int32" />
                <asp:Parameter Name="Initiative_Grouping" Type="Int32" />
                <asp:Parameter Name="Sequence" Type="Int32" />
                <asp:Parameter Name="Implementation_Horizon" Type="Int32" />
                <asp:Parameter Name="Initiative_Summary" Type="String" />
                <asp:Parameter Name="CriticalSuccessFactors" Type="String" />
                <asp:Parameter Name="BSCArea" Type="Int32" />
                <asp:Parameter Name="Priority_Ranking" Type="Double" />
                <asp:Parameter Name="Administration" Type="Boolean" />
                <asp:Parameter Name="Human_Resources" Type="Boolean" />
                <asp:Parameter Name="Engineering" Type="Boolean" />
                <asp:Parameter Name="Environmental" Type="Boolean" />
                <asp:Parameter Name="Finance" Type="Boolean" />
                <asp:Parameter Name="Production" Type="Boolean" />
                <asp:Parameter Name="Water_Quality" Type="Boolean" />
                <asp:Parameter Name="Field_Operations" Type="Boolean" />
                <asp:Parameter Name="Call_Center" Type="Boolean" />
                <asp:Parameter Name="Obsticles" Type="String" />
                <asp:Parameter Name="Approach" Type="String" />
                <asp:Parameter Name="Resource_Requirements" Type="String" />
                <asp:Parameter Name="Change_Requirements" Type="String" />
                <asp:Parameter Name="Estimated_Cost" Type="String" />
                <asp:Parameter Name="Estimated_Annual_Savings" Type="String" />
                <asp:Parameter Name="ROI_Years" Type="String" />
                <asp:Parameter Name="Estimated_Start_Date" Type="String" />
                <asp:Parameter Name="Estimated_Completion_Date" Type="String" />
                <asp:Parameter Name="Start_Date" Type="DateTime" />
                <asp:Parameter Name="Date_Completed" Type="DateTime" />
                <asp:Parameter Name="Percent_Complete" Type="String" />
                <asp:Parameter Name="Date_Percent_Complete_Updated" Type="String" />
                <asp:Parameter Name="Impact" Type="Int32" />
                <asp:Parameter Name="Effort" Type="Int32" />
                <asp:Parameter Name="EIResult" Type="Int32" />
            </InsertParameters>
            <SelectParameters>
                <asp:Parameter Name="Initiative_ID" Type="Int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="Initiative_Focus" Type="Int32" />
                <asp:Parameter Name="Initiative_Grouping" Type="Int32" />
                <asp:Parameter Name="Sequence" Type="Int32" />
                <asp:Parameter Name="Implementation_Horizon" Type="Int32" />
                <asp:Parameter Name="Initiative_Summary" Type="String" />
                <asp:Parameter Name="CriticalSuccessFactors" Type="String" />
                <asp:Parameter Name="BSCArea" Type="Int32" />
                <asp:Parameter Name="Priority_Ranking" Type="Double" />
                <asp:Parameter Name="Administration" Type="Boolean" />
                <asp:Parameter Name="Human_Resources" Type="Boolean" />
                <asp:Parameter Name="Engineering" Type="Boolean" />
                <asp:Parameter Name="Environmental" Type="Boolean" />
                <asp:Parameter Name="Finance" Type="Boolean" />
                <asp:Parameter Name="Production" Type="Boolean" />
                <asp:Parameter Name="Water_Quality" Type="Boolean" />
                <asp:Parameter Name="Field_Operations" Type="Boolean" />
                <asp:Parameter Name="Call_Center" Type="Boolean" />
                <asp:Parameter Name="Obsticles" Type="String" />
                <asp:Parameter Name="Approach" Type="String" />
                <asp:Parameter Name="Resource_Requirements" Type="String" />
                <asp:Parameter Name="Change_Requirements" Type="String" />
                <asp:Parameter Name="Estimated_Cost" Type="String" />
                <asp:Parameter Name="Estimated_Annual_Savings" Type="String" />
                <asp:Parameter Name="ROI_Years" Type="String" />
                <asp:Parameter Name="Estimated_Start_Date" Type="String" />
                <asp:Parameter Name="Estimated_Completion_Date" Type="String" />
                <asp:Parameter Name="Start_Date" Type="DateTime" />
                <asp:Parameter Name="Date_Completed" Type="DateTime" />
                <asp:Parameter Name="Percent_Complete" Type="String" />
                <asp:Parameter Name="Date_Percent_Complete_Updated" Type="String" />
                <asp:Parameter Name="Impact" Type="Int32" />
                <asp:Parameter Name="Effort" Type="Int32" />
                <asp:Parameter Name="EIResult" Type="Int32" />
                <asp:Parameter Name="Initiative_ID" Type="Int32" />
            </UpdateParameters>
        </DetailsDataSource>

        <Tabs>
            <mapcall:Tab Label="Initiative Steps" runat="server" EnableViewState="false">
                <asp:GridView runat="server" id="gvIntiativeSteps" DataSourceID="dsInitiativeSteps"
                    EmptyDataText="No steps have been defined."
                    AutoGenerateColumns="false">
                    <Columns>
                        <mapcall:TemplateBoundField HeaderText="InitiativeStepID">
                            <ItemTemplate>
                                <a href="<%# ResolveUrl("~/modules/businessperformance/initiativeSteps.aspx?view=") + Eval("InitiativeStepID")  %>"><%#Eval("InitiativeStepID") %></a>
                            </ItemTemplate>
                        </mapcall:TemplateBoundField>
                        <mapcall:BoundField DataField="Sequence" />
                        <mapcall:BoundField DataField="StepAction" />
                        <mapcall:BoundField DataField="StepDescription" />
                    </Columns>
                </asp:GridView>
                
                <asp:SqlDataSource ID="dsInitiativeSteps" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                    SelectCommand="
                        Select 
                            InitiativeStepID,
                            Sequence, 
                            StepAction, 
                            StepDescription 
                        from 
                            tblBusinessPerformanceInitiativeSteps
                        where
                            initiativeID=@Initiative_ID">           
                    <SelectParameters>
                        <asp:ControlParameter Name="Initiative_ID" PropertyName="SelectedValue" ControlID="detailView" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </mapcall:Tab>
            <mapcall:Tab Label="Initiative Goals" runat="server">
                <asp:UpdatePanel runat="server" ID="pnlInitiativeGoals" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvInitiativeGoals" runat="server" DataSourceID="dsInitiativeGoals"  
                            DataKeyNames="InitiativeID,GoalID"
                            EmptyDataText="No goals have been linked to this initiative."
                            AutoGenerateColumns="false" >
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" Text="View"
                                            NavigateUrl='<%# ResolveUrl("~/Modules/HR/Administrator/BusPerformanceGoals.aspx?view=" + Eval("GoalID")) %>' />
                                        <asp:LinkButton runat="server" CommandName="Delete" CommandArgument="GoalID" Text="Delete" OnClientClick="return confirm('Are you sure you want to remove this goal from this Initiative?');" />
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <mmsinc:BoundField DataField="Goal" HeaderText="Goal" SortExpression="Goal" />                                
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="dsInitiativeGoals" runat="server"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                            SelectCommand="
                                Select 
	                                g.Goal, ig.goalID, ig.InitiativeID
                                from 
	                                tblBusinessPerformanceInitiativesGoals ig
                                inner join
	                                [tblBusinessPerformance_Goals] g
                                on
	                                g.goal_ID = ig.goalID
                                where 
	                                InitiativeID = @initiativeID
                            "
                            InsertCommand="INSERT INTO tblBusinessPerformanceInitiativesGoals(InitiativeID,GoalID) Values(@InitiativeID, @GoalID)"
                            DeleteCommand="DELETE tblBusinessPerformanceInitiativesGoals WHERE InitiativeID = @InitiativeID AND GoalID = @GoalID">
                            <SelectParameters>
                                <asp:ControlParameter Name="InitiativeID" PropertyName="SelectedValue" ControlID="detailView" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:ControlParameter Name="InitiativeID" PropertyName="SelectedValue" ControlID="detailView" Type="Int32" />
                                <asp:ControlParameter Name="GoalID" PropertyName="SelectedValue" ControlID="ddlGoals" Type="Int32" />
                            </InsertParameters>
                            <DeleteParameters>
                                <asp:ControlParameter Name="InitiativeID" PropertyName="SelectedValue" ControlID="detailView" Type="Int32" />
                                <asp:ControlParameter Name="GoalID" PropertyName="SelectedValue" ControlID="gvInitiativeGoals" Type="Int32" />
                            </DeleteParameters>
                        </asp:SqlDataSource>    
                         <br />                               
                        <asp:DropDownList runat="server" ID="ddlGoals" DataSourceID="dsGoals" DataTextField="Goal" DataValueField="Goal_ID"></asp:DropDownList>
                        
                         <asp:SqlDataSource runat="server" ID="dsGoals" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                              SelectCommand="select Goal_ID, Goal from [tblBusinessPerformance_Goals]"></asp:SqlDataSource>
                        <asp:Button runat="server" ID="btnAddGoal" Text="Add Goal" OnClick="btnAddGoal_Click" />        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </mapcall:Tab>
            <mapcall:Tab Label="Links" runat="server">
                <mmsi:Hyperlinks ID="Hyperlinks1" runat="server" />
            </mapcall:Tab>
        </Tabs>

    </mapcall:DetailsViewDataPageTemplate>
</asp:Content>