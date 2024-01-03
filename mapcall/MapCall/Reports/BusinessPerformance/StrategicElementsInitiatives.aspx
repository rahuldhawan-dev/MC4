<%@ Page Title="Strategic Elements - Business Performance Initiatives" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="StrategicElementsInitiatives.aspx.cs" Inherits="MapCall.Reports.BusinessPerformance.StrategicElementsInitiatives" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Strategic Elements - Business Performance Initiatives
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" CssClass="searchPanel">
        <table>
            <mmsi:DataField runat="server" ID="dfStrategicElement"
                HeaderText="Strategic Element : "
                DataType="DropDownList"
                DataFieldName="Strategic_Element"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select strategicElementID as val,isNull(StrategyName  + ', ','') + isNull(Elementname,'') as txt from StrategicElements order by StrategyName,ElementName"
            />
            <mmsi:DataField runat="server" ID="dfInitiatives"
                HeaderText="Initiatives : "
                DataType="ListBox"
                DataFieldName="Initiative_ID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select Initiative_ID as val, Initiative_Summary as txt from tblBusinessPerformance_Initiatives order by 2"
            />
            <mmsi:DataField runat="server" ID="dfInitiativeFocus"
                HeaderText="Initiative Focus : "
                DataType="DropDownList"
                DataFieldName="Initiative_Focus"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Initiative_Focus' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfImplementationHorizon"
                HeaderText="Implementation Horizon : "
                DataType="DropDownList"
                DataFieldName="Implementation_Horizon"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Implementation_Horizon' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfGoals"
                HeaderText="Goals : "
                DataType="ListBox"
                DataFieldName="Goal_ID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select Goal_ID as val, Goal as txt from [tblBusinessPerformance_Goals] order by 2 "
            />
            <mmsi:DataField runat="server" ID="dfPriorityRanking"
                HeaderText="Priority Ranking : "
                DataType="DropDownList"
                DataFieldName="Priority_Ranking"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct Priority_Ranking as Val, Priority_Ranking as txt from tblBusinessPerformance_Initiatives order by 2"
            />            

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
                           <mmsinc:BoundField DataField="StrategyName" HeaderText="Strategy" />
                           <mmsinc:BoundField DataField="ElementName" HeaderText="Strategic Element" />
                           <mmsinc:BoundField DataField="Initiative_Summary" HeaderText="Initiative Summary" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="
select 
	se.StrategyName, 
	se.ElementName, 
	I.initiative_Summary,
	Strategic_Element, Initiative_ID, Initiative_Focus, Implementation_Horizon, Priority_Ranking, Goal_ID
from
	StrategicElements se
left join
	GoalsStrategicElements gse
on
	se.StrategicElementID = gse.StrategicElementID
inner join
	tblBusinessPerformance_Goals G
on
	g.Goal_ID = gse.GoalID
left join
	tblBusinessPerformanceInitiativesGoals bpiG
on
	bpiG.GoalID = g.Goal_ID
left join
	[tblbusinessperformance_initiatives] I
on
	i.Initiative_ID = bpiG.initiativeID

                        ">
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
