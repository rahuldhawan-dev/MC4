<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="BusPerformanceGoals.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.BusPerformanceGoals" Title="Business Performance Goals" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Business Performance Goals
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
Business Performance Goals
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfGoalStatus"
                HeaderText="Goal Status : "
                DataType="DropDownList"
                DataFieldName="Goal_Status"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] as Val, [LookupValue] as txt from [Lookup] WHERE [LookupType] = 'Goal_Status' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfGoalLevel"
                HeaderText="Goal Level : "
                DataType="DropDownList"
                DataFieldName="Goal_Level"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] as Val, [LookupValue] as txt from [Lookup] WHERE [LookupType] = 'Goal_Level' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfBSCArea"
                HeaderText="BSC Area : "
                DataType="DropDownList"
                DataFieldName="BSCArea"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] as Val, [LookupValue] as txt from [Lookup] WHERE [LookupType] = 'BSCArea' and TableName = 'tblBusinessPerformance_Goals' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfObjective"
                HeaderText="Objective : "
                DataType="DropDownList"
                DataFieldName="ObjectiveID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] as Val, [LookupValue] as txt from [Lookup] WHERE [LookupType] = 'Objective' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfStrategicElement"
                HeaderText="Strategic Element : "
                DataType="DropDownList"
                DataFieldName="StrategicElementID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select strategicElementID as val,isNull(StrategyName  + ', ','') + isNull(Elementname,'') as txt from StrategicElements order by StrategyName,ElementName"
            />
            <mmsi:DataField runat="server" ID="dfStartDate" DataType="Date" DataFieldName="Start_Date" HeaderText="Start Date : " />
            <mmsi:DataField runat="server" ID="dfEndDate" DataType="Date" DataFieldName="End_Date" HeaderText="End Date : " />

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
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="Goal_ID"
            AllowSorting="true" AllowPaging="true" PageSize="500"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="SELECT
	        l2.[LookupValue] AS [Status],
	        l1.[LookupValue] AS [Level],
	        l.[LookupValue] AS [BSCArea],
	        l3.[LookupValue] AS [Objective],
	        se.[ElementName] AS [Strategic_Element],
	        t.[Goal],
	        t.[Opportunity],
	        t.[Risk],
	        t.[Start_Date],
	        t.[End_Date],
	        t.[Polygon],
	        t.[Owners],
	        c.[Latitude] AS [CoordinateID],
	        t.[Goal_Description],
	        t.[Goal_ID],
            t.[Goal_Status],
            t.[Goal_Level],
            t.[BSCArea],
            t.[Objective] AS [ObjectiveID],
            t.[Strategic_Element] AS [StrategicElementID]
        FROM
	        [tblbusinessperformance_goals] AS t
        LEFT JOIN
	        [Lookup] AS l
        ON
	        t.[BSCArea] = l.[LookupID]
        LEFT JOIN
	        [Coordinates] AS c
        ON
	        t.[CoordinateID] = c.[CoordinateID]
        LEFT JOIN
	        [Lookup] AS l1
        ON
	        t.[Goal_Level] = l1.[LookupID]
        LEFT JOIN
	        [Lookup] AS l2
        ON
	        t.[Goal_Status] = l2.[LookupID]
        LEFT JOIN
	        [Lookup] AS l3
        ON
	        t.[Objective] = l3.[LookupID]
        LEFT JOIN
	        StrategicElements se 
	    ON
	        t.Strategic_Element = se.StrategicElementID" />
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <div class="container tabsContainer">
            <ul class="ui-tabs-nav">
                <li><a href="#goal" class="tab"><span>Goal</span></a></li>
                <li><a href="#strategicelements" class="tab"><span>Strategic Elements</span></a></li>
                <li><a href="#notes" class="tab"><span>Notes</span></a></li>
                <li><a href="#documents" class="tab"><span>Documents</span></a></li>
            </ul>
        
            <div id="goal" class="ui-tabs-panel">            
                <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "Goal_Status"
            DataElementParameterName = "Goal_ID"
            DataElementTableName = "tblBusinessPerformance_Goals"
            OnDataBinding="DataElement1_DataBinding"
            AllowDelete="true" ShowKey="true"
        />
            </div>
            
            <div id="strategicelements" class="ui-tabs-panel">
                <asp:UpdatePanel runat="server" ID="pnlStrategicElements" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvGoalsStrategicElements" runat="server" DataSourceID="dsGoalsStrategicElements"  
                            DataKeyNames="StrategicElementID,GoalID"
                            EmptyDataText="No Strategic Elements have been linked to this Goal."
                            AutoGenerateColumns="false" >
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" Text="View"
                                            NavigateUrl='<%# ResolveUrl("~/Modules/BusinessPerformance/StrategicElements.aspx?view=" + Eval("StrategicElementID")) %>' />
                                        <asp:LinkButton runat="server" CommandName="Delete" CommandArgument="StrategicElementID" Text="Delete" OnClientClick="return confirm('Are you sure you want to remove the strategic element from this Goal?');" />
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <mmsinc:BoundField DataField="Strategic Element" HeaderText="Strategic Element" SortExpression="Strategic Element" />                                
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="dsGoalsStrategicElements"
                            ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                            SelectCommand="
                                SELECT
	                                StrategyName + ' - ' + ElementName as [Strategic Element], gse.*
                                FROM
	                                GoalsStrategicElements gse
                                INNER JOIN
	                                StrategicElements se
                                ON
	                                gse.StrategicElementID = se.StrategicElementID
                                WHERE
	                                gse.GoalID = @GoalID
                            "
                            InsertCommand="INSERT INTO GoalsStrategicElements(StrategicElementID,GoalID) Values(@StrategicElementID, @GoalID)"
                            DeleteCommand="DELETE GoalsStrategicElements WHERE StrategicElementID = @StrategicElementID AND GoalID = @GoalID">
                            <SelectParameters>
                                <asp:ControlParameter Name="GoalID" PropertyName="SelectedValue" ControlID="DataElement1$DetailsView1" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:ControlParameter Name="GoalID" PropertyName="SelectedValue" ControlID="DataElement1$DetailsView1" Type="Int32" />
                                <asp:ControlParameter Name="StrategicElementID" PropertyName="SelectedValue" ControlID="ddlStrategicElements" Type="Int32" />
                            </InsertParameters>
                            <DeleteParameters>
                                <asp:ControlParameter Name="GoalID" PropertyName="SelectedValue" ControlID="DataElement1$DetailsView1" Type="Int32" />
                                <asp:ControlParameter Name="StrategicElementID" PropertyName="SelectedValue" ControlID="GridView1" Type="Int32" />
                            </DeleteParameters>
                        </asp:SqlDataSource>   
                        <br />
                        <asp:DropDownList runat="server" ID="ddlStrategicElements" DataSourceID="dsStrategicElements"
                            DataTextField="Strategic Element" DataValueField="StrategicElementID" />
                        <asp:Button runat="server" ID="btnAddStrategicElement" OnClick="btnAddStrategicElement_Click" Text="Add" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:SqlDataSource runat="server" ID="dsStrategicElements" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                    SelectCommand="select StrategicElementID, StrategyName + ' - ' + ElementName as [Strategic Element] from StrategicElements" />

            </div>
            
            <div id="notes" class="ui-tabs-panel">
                <mmsi:Notes ID="Notes1" runat="server" DataTypeID="58" />
            </div>
        
            <div id="documents" class="ui-tabs-panel">
                <mmsi:Documents ID="Documents1" runat="server" DataTypeID="58" />
            </div>

            <div class="buttonContainer">
                <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
                <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
            </div>
        </div>
    </asp:Panel>

</asp:Content>