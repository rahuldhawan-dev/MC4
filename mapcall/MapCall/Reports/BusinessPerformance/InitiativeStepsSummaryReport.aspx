<%@ Page Title="Initiative Steps Summary" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="InitiativeStepsSummaryReport.aspx.cs" Inherits="MapCall.Modules.BusinessPerformance.InitiativeStepsSummaryReport" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Initiative Steps Summary
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch" CssClass="searchPanel">
        <table>
            <mmsi:DataField runat="server" ID="dfInitiativeFocus"
                HeaderText="Initiative Focus : "
                DataType="DropDownList"
                DataFieldName="InitiativeFocusID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Initiative_Focus' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfInitiativeGrouping"
                HeaderText="Initiative Grouping : "
                DataType="DropDownList"
                DataFieldName="InitiativeGroupingID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Initiative_Grouping' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfImplementationHorizon"
                HeaderText="Implementation Horizon : "
                DataType="DropDownList"
                DataFieldName="ImplementationHorizonID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Implementation_Horizon' order by 2"
            />
            <mmsi:DataField runat="server" ID="ddlOpCntr" DataType="DropDownList"
                HeaderText="OpCode :"
                DataFieldName="opCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct operatingCenterCode + ' - ' + OperatingCenterName as txt, OperatingCenterID as val from OperatingCenters order by 1"
            />
            <mmsi:DataField runat="server" ID="ddlAssignedTo" DataType="DropDownList" HeaderText="Assigned To :"
                DataFieldName="AssignedTo" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct Last_Name + ', ' + First_Name + IsNull(' ' + Middle_Name, '') as txt, Last_Name + ', ' + First_Name + IsNull(' ' + Middle_Name, '') as val from tblEmployee order by 1" />

            <tr>
                <td class="label"></td>
                <td class="field">
                    <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="View Report" />
                </td>
            </tr>
        </table>
    </asp:Panel>
   
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <table>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export to Excel" />
                    <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search" />
                    <asp:Label runat="server" ID="lblInformation" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                        AllowSorting="false" OnDataBinding="GridView1_DataBinding" OnSorting="GridView1_Sorting">
                        <Columns>
                            <mmsinc:BoundField DataField="InitiativeStepID" HeaderText="InitiativeStepID" SortExpression="InitiativeStepID" />
                            <mmsinc:BoundField DataField="OpCode" HeaderText="OpCode" SortExpression="OpCode" />
                            <mmsinc:BoundField DataField="AssignedTo" HeaderText="AssignedTo" SortExpression="AssignedTo" />
                            <mmsinc:BoundField DataField="Sequence" HeaderText="Sequence" SortExpression="Sequence" />
                            <mmsinc:BoundField DataField="StepAction" HeaderText="StepAction" SortExpression="StepAction" />
                            <mmsinc:BoundField DataField="StepDescription" HeaderText="StepDescription" SortExpression="StepDescription" />
                            <mmsinc:BoundField DataField="EstimatedStartDate" HeaderText="EstimatedStartDate" SortExpression="EstimatedStartDate" />
                            <mmsinc:BoundField DataField="EstimatedCompletionDate" HeaderText="EstimatedCompletionDate" SortExpression="EstimatedCompletionDate" />
                            <mmsinc:BoundField DataField="Milestones" HeaderText="Milestones" SortExpression="Milestones" />
                            <mmsinc:BoundField DataField="PercentComplete" HeaderText="PercentComplete" SortExpression="PercentComplete" />
                            <mmsinc:BoundField DataField="DatePercentCompleteUpdated" HeaderText="DatePercentCompleteUpdated" SortExpression="DatePercentCompleteUpdated" />
                            <mmsinc:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="
SELECT 
    t.[Initiative_Grouping] AS [InitiativeGroupingID],
    t.[Initiative_ID],
    l2.[LookupValue] AS [Initiative_Focus],
    l.[LookupValue] AS [Initiative_Grouping],
    l1.[LookupValue] AS [Implementation_Horizon],
    t.[Initiative_Summary],
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
	bpis.[InitiativeStepID], 
	oc.operatingcentercode as OpCode,
    e.[Last_Name] + ', ' + e.[First_Name] + IsNull(' ' + e.[Middle_Name], '') as [AssignedTo],
	bpis.[InitiativeID],
	bpi.Initiative_Summary,
	bpis.[Sequence],
	bpis.[StepAction],
	bpis.[StepDescription],
	bpis.[EstimatedStartDate],
	bpis.[EstimatedCompletionDate],
	bpis.[Milestones],
	bpis.[StartDate],
	bpis.[PercentComplete],
	bpis.[DatePercentCompleteUpdated],
	bpis.[DateCompleted],
	bpis.[Notes]     
FROM 
	tblBusinessPerformanceInitiativeSteps bpis
LEFT JOIN
    OperatingCenters oc
ON
    oc.OperatingCenterID = bpis.opCode
LEFT JOIN
	tblBusinessPerformance_Initiatives bpi
ON
	bpi.Initiative_ID = bpis.InitiativeID
INNER JOIN
	tblbusinessperformance_initiatives t on bpis.[InitiativeID] = t.[Initiative_ID]
LEFT JOIN
    [Lookup] AS l
ON
    t.[Initiative_Grouping] = l.[LookupID]
LEFT JOIN
    [Lookup] AS l1
ON
    t.[Implementation_Horizon] = l1.[LookupID]
LEFT JOIN
    [Lookup] AS l2
ON
    t.[Initiative_Focus] = l2.[LookupID]
LEFT JOIN
	tblEmployee as e
ON
	bpis.[AssignedTo] = e.tblEmployeeID
ORDER BY
	l.[LookupValue], l2.[LookupValue], l1.[LookupValue], t.[Initiative_Summary]

                        ">
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
</asp:Content>
