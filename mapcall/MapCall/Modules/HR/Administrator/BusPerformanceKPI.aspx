<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="BusPerformanceKPI.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.BusPerformanceKPI" Title="Business Performance KPI" Theme="bender"%>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Business Performance KPI
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
Business Performance KPI
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfKPIStatus"
                HeaderText="KPI Status : "
                DataType="DropDownList"
                DataFieldName="KPIStatusID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select lookupID as val, lookupValue as txt from Lookup where lookuptype = 'KPI_Status'"
            />            
            <tr>
                <td>Initiatives:</td>
                <td>
                    <asp:ListBox runat="server" ID="lbInitiatives" DataSourceID="dsInitiatives"
                        SelectionMode="Multiple"
                        DataTextField="Initiative_Summary" DataValueField="Initiative_ID" />
                </td>
            </tr>
            <tr>
                <td>Goals:</td>
                <td>
                    <asp:ListBox runat="server" ID="lbGoals" DataSourceID="dsGoals" 
                        SelectionMode="Multiple"
                        DataTextField="Goal" DataValueField="Goal_ID" />
                </td>
            </tr>
            <mmsi:DataField runat="server" ID="dfKPILevel"
                HeaderText="KPI Level : "
                DataType="DropDownList"
                DataFieldName="KPI_Level"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupValue] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'KPI_Level' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfKPIMeasurement"
                HeaderText="KPI Measurement : "
                DataType="DropDownList"
                DataFieldName="KPI_Measurement"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct KPI_Measurement as Val, KPI_Measurement as txt from tblBusinessPerformance_KPI order by 2"
            />
            
            <mmsi:DataField runat="server" ID="dfGrouping" HeaderText="Grouping :" DataFieldName="Grouping" DataType="Double" />
            
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                    <asp:Button runat="server" ID="btnAdd" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
        </center>
        <br />
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnMap" Visible="False" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="KPI_ID"
            AllowSorting="true" AllowPaging="true" PageSize="500"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" >
        </asp:SqlDataSource>
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        
        <div class="container tabsContainer">
            <ul class="ui-tabs-nav">
                <li><a href="#kpi" class="tab"><span>KPI</span></a></li>
                <li><a href="#measurements" class="tab"><span>KPI Measurements</span></a></li>
                <li><a href="#initiatives" class="tab"><span>Initiatives</span></a></li>
                <li><a href="#goals" class="tab"><span>Initiative Goals</span></a></li>
                <li><a href="#notes" class="tab"><span>Notes</span></a></li>
                <li><a href="#documents" class="tab"><span>Documents</span></a></li>
            </ul>
            
            <div id="kpi" class="ui-tabs-panel">
                <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
                    DataElementName = "KPI_Status"
                    DataElementParameterName = "KPI_ID"
                    DataElementTableName = "tblBusinessPerformance_KPI"
                    AllowDelete="true"
                    OnDataBinding="DataElement1_DataBinding"
                    ShowKey="true"
                />
            </div>
            
            <div id="measurements" class="ui-tabs-panel">
                <asp:UpdatePanel runat="server" ID="pnlMeasurements"><ContentTemplate>
                <asp:GridView runat="server" ID="gvKPIMeasurements" DataSourceID="dsKPIMeasurements"
                    EmptyDataText="No measurement records are linked to this KPI" 
	                DataKeyNames="KPI_Measurement_ID" AllowSorting="true">
	                <Columns>
	                    <asp:HyperLinkField HeaderText="ID" SortExpression="KPI_Measurement_ID"
	                        DataNavigateUrlFormatString="~/modules/hr/administrator/BusPerformanceKPIMeasurement.aspx?view={0}" 
	                        DataNavigateUrlFields="KPI_Measurement_ID" DataTextField="KPI_Measurement_ID" />
	                </Columns>
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="dsKPIMeasurements"
                    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                    SelectCommand="
SELECT
	oc.OperatingCenterCode as OpCode,
	tbu.BU,
	t.KPIYear,
	(Select LookupValue from Lookup where LookupID = t.KPIMeasurementCategory) as [KPI Measurement Category], 
    t.Jan, t.Feb, t.Mar, t.Apr,
    t.May, t.Jun, t.Jul, t.Aug, 
    t.Sep, t.Oct, t.Nov, t.Dec, 
    t.Total, 
    replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') as [Employee Responsible],
    KPI_Measurement_ID
FROM
    [tblbusinessperformance_kpi_measurement] AS t
LEFT JOIN
    [tblBusinessPerformance_KPI] AS t1
ON
    t.[KPI_ID] = t1.[KPI_ID]
LEFT JOIN 
    [OperatingCenters] as oc
ON
    oc.OperatingCenterID = t.OpCode 
LEFT JOIN
    [tblEmployee] te
ON
    te.tblEmployeeID = t.employeeResponsible
LEFT JOIN   
    BusinessUnits tbu
ON
    tbu.BusinessUnitID = t.BU
WHERE
    t.[KPI_ID] = @KPIID
                    "
                >
                    <SelectParameters>
                        <asp:ControlParameter Name="KPIID" PropertyName="SelectedValue" ControlID="DataElement1$DetailsView1" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </ContentTemplate></asp:UpdatePanel>
            </div>
            
            <div id="initiatives" class="ui-tabs-panel">
                <asp:UpdatePanel runat="server" ID="pnlIntiatives" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvKPIInitiatives" runat="server" DataSourceID="dsKPIInitiatives"  
                            DataKeyNames="KPIID,InitiativeID"
                            EmptyDataText="No Initiatives have been linked to this KPI."
                            AutoGenerateColumns="false" >
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" Text="View"
                                            NavigateUrl='<%# ResolveUrl("~/Modules/HR/Administrator/BusPerformanceInitiatives.aspx?view=" + Eval("InitiativeID")) %>' />
                                        <asp:LinkButton runat="server" CommandName="Delete" CommandArgument="InitiativeID" Text="Delete" OnClientClick="return confirm('Are you sure you want to remove the initiative from this KPI?');" />
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <mmsinc:BoundField DataField="Initiative Summary" HeaderText="Initiative Summary" SortExpression="Initiative Summary" />                                
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="dsKPIInitiatives"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                            SelectCommand="
                                Select 
	                                i.Initiative_Summary as [Initiative Summary], ik.InitiativeID, ik.KPIID
                                from 
	                                InitiativesKPIs ik
                                inner join
	                                [tblBusinessPerformance_Initiatives] i
                                on
	                                i.initiative_ID = ik.initiativeID
                                where 
	                                KPIID = @KPIID
                            "
                            InsertCommand="INSERT INTO InitiativesKPIs(KPIID,InitiativeID) Values(@KPIID, @InitiativeID)"
                            DeleteCommand="DELETE InitiativesKPIs WHERE KPIID = @KPIID AND InitiativeID = @InitiativeID">
                            <SelectParameters>
                                <asp:ControlParameter Name="KPIID" PropertyName="SelectedValue" ControlID="DataElement1$DetailsView1" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:ControlParameter Name="KPIID" PropertyName="SelectedValue" ControlID="DataElement1$DetailsView1" Type="Int32" />
                                <asp:ControlParameter Name="InitiativeID" PropertyName="SelectedValue" ControlID="ddlInitiatives" Type="Int32" />
                            </InsertParameters>
                            <DeleteParameters>
                                <asp:ControlParameter Name="KPIID" PropertyName="SelectedValue" ControlID="DataElement1$DetailsView1" Type="Int32" />
                                <asp:ControlParameter Name="InitiativeID" PropertyName="SelectedValue" ControlID="GridView1" Type="Int32" />
                            </DeleteParameters>
                        </asp:SqlDataSource>   
                        <br />
                        <asp:DropDownList runat="server" ID="ddlInitiatives" DataSourceID="dsInitiatives"
                            DataTextField="Initiative_Summary" DataValueField="Initiative_ID" />
                        <asp:Button runat="server" ID="btnAddInitiative" OnClick="btnAddInitiative_Click" Text="Add" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
            <div id="goals" class="ui-tabs-panel">
                <asp:UpdatePanel runat="server" ID="pnlGoals" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvKPIGoals" runat="server" DataSourceID="dsKPIGoals"  
                            DataKeyNames="KPIID,GoalID"
                            EmptyDataText="No goals have been linked to this KPI."
                            AutoGenerateColumns="false" >
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" Text="View"
                                            NavigateUrl='<%# ResolveUrl("~/Modules/HR/Administrator/BusPerformanceGoals.aspx?view=" + Eval("GoalID")) %>' />
                                        <asp:LinkButton runat="server" CommandName="Delete" CommandArgument="GoalID" Text="Delete" OnClientClick="return confirm('Are you sure you want to remove the goal from this KPI?');" />
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <mmsinc:BoundField DataField="Goal Description" HeaderText="Goal Description" SortExpression="Goal Description" />                                
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="dsKPIGoals" runat="server"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                            SelectCommand="
        Select 
	        g.Goal_Description as [Goal Description], gk.goalID, gk.KPIID
        from 
	        GoalsKPIs gk
        inner join
	        [tblBusinessPerformance_Goals] g
        on
	        g.goal_ID = gk.goalID
        where 
	        KPIID = @KPIID
                            "
                            InsertCommand="INSERT INTO GoalsKPIs(KPIID,GoalID) Values(@KPIID, @GoalID)"
                            DeleteCommand="DELETE GoalsKPIs WHERE KPIID = @KPIID AND GoalID = @GoalID">
                            <SelectParameters>
                                <asp:ControlParameter Name="KPIID" PropertyName="SelectedValue" ControlID="DataElement1$DetailsView1" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:ControlParameter Name="KPIID" PropertyName="SelectedValue" ControlID="DataElement1$DetailsView1" Type="Int32" />
                                <asp:ControlParameter Name="GoalID" PropertyName="SelectedValue" ControlID="ddlGoals" Type="Int32" />
                            </InsertParameters>
                            <DeleteParameters>
                                <asp:ControlParameter Name="KPIID" PropertyName="SelectedValue" ControlID="DataElement1$DetailsView1" Type="Int32" />
                                <asp:ControlParameter Name="GoalID" PropertyName="SelectedValue" ControlID="GridView1" Type="Int32" />
                            </DeleteParameters>
                        </asp:SqlDataSource>    
                         <br />                               
                        <asp:DropDownList runat="server" ID="ddlGoals" DataSourceID="dsGoals" DataTextField="Goal" DataValueField="Goal_ID"></asp:DropDownList>
                        <asp:Button runat="server" ID="btnAddGoal" Text="Add Goal" OnClick="btnAddGoal_Click" />        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
            <div id="notes" class="ui-tabs-panel">
                <mmsi:Notes ID="Notes1" runat="server" DataTypeID="60" />
            </div>
        
            <div id="documents" class="ui-tabs-panel">
                <mmsi:Documents ID="Documents1" runat="server" DataTypeID="60" />
            </div>

            <div class="buttonContainer">
                <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
                <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
            </div>
        </div>
    </asp:Panel>

    <asp:SqlDataSource runat="server" ID="dsInitiatives" ConnectionString="<%$ ConnectionStrings:MCProd %>"
        SelectCommand="select Initiative_ID, Initiative_Summary from tblBusinessPerformance_Initiatives order by 2" />
    <asp:SqlDataSource runat="server" ID="dsGoals" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="select Goal_ID, Goal from [tblBusinessPerformance_Goals] order by 2"></asp:SqlDataSource>

</asp:Content>