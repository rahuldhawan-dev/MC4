<%@ Page Title="Initiatives Summary Report" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="InitiativeSummaryReport.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.InitiativeSummaryReport" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Initiatives Summary Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" CssClass="searchPanel">
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
            <mmsi:DataField runat="server" ID="dfPriorityRanking"
                HeaderText="Priority Ranking : "
                DataType="DropDownList"
                DataFieldName="Priority_Ranking"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct Priority_Ranking as Val, Priority_Ranking as txt from tblBusinessPerformance_Initiatives order by 2"
            />
            
            <mmsi:DataField runat="server" ID="dfStartDate" DataType="Date" DataFieldName="Start_Date" HeaderText="Start Date : " />
            <mmsi:DataField runat="server" ID="dfDateComplete" DataType="Date" DataFieldName="Date_Complete" HeaderText="Date Complete : " />
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
                        AllowSorting="false" OnDataBinding="GridView1_DataBinding" OnSorting="GridView1_Sorting" >
                        <Columns>
                            <mmsinc:BoundField DataField="Initiative_Grouping" HeaderText="Initiative_Grouping" SortExpression="Initiative_Grouping" />
                            <mmsinc:BoundField DataField="Initiative_Focus" HeaderText="Initiative Focus" SortExpression="Initiative_Focus" />
                            <mmsinc:BoundField DataField="Implementation_Horizon" HeaderText="Implementation Horizon" SortExpression="Implementation_Horizon" />  
                            <mmsinc:BoundField DataField="Initiative_Summary" HeaderText="Initiative_Summary" SortExpression="Initiative_Summary" />
                            <mmsinc:BoundField DataField="Priority_Ranking" HeaderText="Priority_Ranking" SortExpression="Priority_Ranking" />
                            <mmsinc:BoundField DataField="Obsticles" HeaderText="Obsticles" SortExpression="Obsticles" />
                            <mmsinc:BoundField DataField="Approach" HeaderText="Approach" SortExpression="Approach" />
                            <mmsinc:BoundField DataField="Resource_Requirements" HeaderText="Resource_Requirements" SortExpression="Resource_Requirements" />
                            <mmsinc:BoundField DataField="Change_Requirements" HeaderText="Change_Requirements" SortExpression="Change_Requirements" />
                            <mmsinc:BoundField DataField="Estimated_Cost" HeaderText="Estimated_Cost" SortExpression="Estimated_Cost" />
                            <mmsinc:BoundField DataField="Estimated_Annual_Savings" HeaderText="Estimated_Annual_Savings" SortExpression="Estimated_Annual_Savings" />
                            <mmsinc:BoundField DataField="ROI_Years" HeaderText="ROI_Years" SortExpression="ROI_Years" />
                            <mmsinc:BoundField DataField="Estimated_Start_Date" HeaderText="Estimated_Start_Date" SortExpression="Estimated_Start_Date" />
                            <mmsinc:BoundField DataField="Estimated_Completion_Date" HeaderText="Estimated_Completion_Date" SortExpression="Estimated_Completion_Date" />
                            <mmsinc:BoundField DataField="Start_Date" HeaderText="Start_Date" SortExpression="Start_Date" />
                            <mmsinc:BoundField DataField="Date_Completed" HeaderText="Date_Completed" SortExpression="Date_Completed" />
                            <mmsinc:BoundField DataField="Percent_Complete" HeaderText="Percent_Complete" SortExpression="Percent_Complete" />
                            <mmsinc:BoundField DataField="Date_Percent_Complete_Updated" HeaderText="Date_Percent_Complete_Updated" SortExpression="Date_Percent_Complete_Updated" />
                            <mmsinc:BoundField DataField="Administration" HeaderText="Administration" SortExpression="Administration" />
                            <mmsinc:BoundField DataField="Human_Resources" HeaderText="Human_Resources" SortExpression="Human_Resources" />
                            <mmsinc:BoundField DataField="Engineering" HeaderText="Engineering" SortExpression="Engineering" />
                            <mmsinc:BoundField DataField="Environmental" HeaderText="Environmental" SortExpression="Environmental" />
                            <mmsinc:BoundField DataField="Finance" HeaderText="Finance" SortExpression="Finance" />
                            <mmsinc:BoundField DataField="Production" HeaderText="Production" SortExpression="Production" />
                            <mmsinc:BoundField DataField="Water_Quality" HeaderText="Water_Quality" SortExpression="Water_Quality" />
                            <mmsinc:BoundField DataField="Field_Operations" HeaderText="Field_Operations" SortExpression="Field_Operations" />
                            <mmsinc:BoundField DataField="Call_Center" HeaderText="Call_Center" SortExpression="Call_Center" />
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
    t.[Implementation_Horizon] AS [ImplementationHorizonID]
FROM
    [tblbusinessperformance_initiatives] AS t
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
ORDER BY 
    Initiative_Grouping, Initiative_Focus, Implementation_Horizon
                        ">
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>
    

</asp:Content>
