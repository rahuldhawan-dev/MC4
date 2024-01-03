<%@ Page Title="Intiative Steps" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="InitiativeSteps.aspx.cs" Inherits="MapCall.Modules.BusinessPerformance.InitiativeSteps" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Initiative Steps
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />

    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfInitiativeStepID" DataType="Integer" DataFieldName="InitiativeStepID" HeaderText="Initiative Step ID:" />
            <mmsi:DataField runat="server" DataType="DropDownList" 
                DataFieldName="InitiativeID" HeaderText="Initiative"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select Initiative_ID as val, cast(Initiative_ID as varchar) + ', ' + Initiative_Summary as txt from dbo.tblBusinessPerformance_Initiatives order by 1" />
            <mmsi:DataField runat="server" ID="ddlOpCntr" DataType="DropDownList"
                HeaderText="OpCode :"
                DataFieldName="opCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct operatingCenterCode + ' - ' + OperatingCenterName as txt, OperatingCenterID as val from OperatingCenters order by 1"
            />
            <mmsi:DataField runat="server" ID="dfSequence" DataType="NumberRange" DataFieldName="Sequence" HeaderText="Sequence:" />
            <mmsi:DataField runat="server" ID="dfInitiativeStep" DataType="String" DataFieldName="StepAction" HeaderText="Step Action:" />
            <mmsi:DataField runat="server" ID="dfStepDescription" DataType="String" DataFieldName="StepDescription" HeaderText="Step Description:" />
            <tr>
                <td class="label">Goal:</td>
                <td>
                    <asp:ListBox runat="server" ID="lbGoals"
                        Rows="1" SelectionMode="Single"
                        DataSourceID="dsGoals" 
                        DataTextField="Goal"
                        DataValueField="Goal_ID"
                        AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:ListBox>
                    <asp:SqlDataSource runat="server" ID="dsGoals" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
                        SelectCommand="select Goal_ID, Goal from [tblBusinessPerformance_Goals]"></asp:SqlDataSource>
                </td>
            </tr>

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
        <asp:Button runat="server" ID="btnMap" Visible="false" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="InitiativeStepID" AllowSorting="true"
            AutoGenerateColumns="true"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
SELECT 
	bpis.[InitiativeStepID], 
	oc.operatingcentercode as OpCode,
    bpis.[AssignedTo] as [Assigned To],
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
	bpis.[Notes],
	(Select count(1) from tblBusinessPerformanceInitiativesGoals where initiativeID = bpi.initiative_ID and goalID in (@goalIDs)) as goals        
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
ORDER BY 
    InitiativeID, bpis.[Sequence]">
	        <SelectParameters>
	            <asp:Parameter Name="goalIDs" ConvertEmptyStringToNull="false" DefaultValue="" />
	        </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" 
            OnItemInserted="DataElement1_ItemInserted" 
            DataElementName = "Intiative Step"
            ConnectionString="MCProd"
            DataElementParameterName = "InitiativeStepID"
            DataElementTableName = "tblBusinessPerformanceInitiativeSteps"
            AllowDelete="true"
            OnPreInit="DataElement1_PreInit"
            OnDataBinding="DataElement1_DataBinding"
        >
        </mmsi:DataElement>
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="101" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="101" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>
    
</asp:Content>
