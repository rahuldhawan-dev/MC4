<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="Complaints.aspx.cs" Inherits="MapCall.Modules.WaterQuality.Complaints" Title="Water Quality Complaints" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register Assembly="MapCall" Namespace="MapCall.Controls.HR" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Water Quality Complaints
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>&nbsp;
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfOpCode" HeaderText="OpCode : "
                DataType="DropDownList"
                DataFieldName="OpCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT DISTINCT [OperatingCenterCode] AS [Val], [OperatingCenterCode] AS [Txt] FROM [OperatingCenters] ORDER BY [OperatingCenterCode]"
                />
            <mmsi:DataField runat="server" ID="dfStreetName" HeaderText="Street Name : " DataType="String" DataFieldName="Street_Name" />
            <mmsi:DataField runat="server" ID="dfTown" 
                HeaderText="Town : " 
                DataType="DropDownList" 
                DataFieldName="Town" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select TownID as Val, Town as Txt from Towns order by 2"
            />
            <mmsi:DataField runat="server" ID="dfTownName" HeaderText="Town : " DataType="String" DataFieldName="Town_Name" />
            <mmsi:DataField runat="server" ID="dfStateDataField1" DataType="String" DataFieldName="State" HeaderText="State : " />
            <mmsi:DataField runat="server" ID="dfZip" DataType="String" DataFieldName="Zip_Code" HeaderText="Zip : " />
            <mmsi:DataField runat="server" ID="dfDateComplaintReceived" DataType="Date" DataFieldName="Date_Complaint_Received" HeaderText="Date_Complaint_Received :" ShowTime="true" />
            <mmsi:DataField runat="server" ID="dfComplaintCloseDate" DataType="Date" DataFieldName="Complaint_Close_Date" HeaderText="Complaint_Close_Date" ShowTime="true" />

            <mmsi:DataField runat="server" ID="dfSampleSiteID" 
                HeaderText="Customer Satisfaction : " 
                DataType="DropDownList" 
                DataFieldName="CustomerSatisfactionID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select lookupId as val, lookupvalue as txt from lookup where lookuptype='Customer_Satisfaction'"
            />
            <tr>
                <td style="text-align:right;">
                    Complaint Category: 
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlComplaintCategory">
                        <asp:ListItem Text="--Select Here--" />
                        <asp:ListItem Text="Aesthetics" Value="215,216,217,218,219,220,221,222,223,224,225,226,227" />
                        <asp:ListItem Text="Information Inquiry" Value="228,229,230,231" />
                        <asp:ListItem Text="Medical" Value="232,233" />
                    </asp:DropDownList>
                </td>
            </tr>
            <mmsi:DataField runat="server" ID="dfWQComplaintType" 
                HeaderText="Complaint Type: " 
                DataType="ListBox" 
                DataFieldName="WQ_Complaint_Type" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select lookupId as val, lookupvalue as txt from lookup where lookuptype='WQ_Complaint_Type'"
            />            
            <tr>
                <td style="text-align:right;">
                    Complaint Status : 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlComplaintStatus">
                        <asp:ListItem Value="" Text="--Select Here--" />
                        <asp:ListItem Text="Open" Value="0" />
                        <asp:ListItem Text="Closed" Value="1" />
                    </asp:DropDownList>
                </td>
            </tr> 
            <mmsi:DataField runat="server" ID="dfSiteVisitRequired" 
                HeaderText="Site_Visit_Required" 
                DataType="Boolean" 
                DataFieldName="Site_Visit_Required" 
            />
            <mmsi:DataField runat="server" ID="dfRootCauseIdentified" 
                HeaderText="Root_Cause_Identified" 
                DataType="Boolean" 
                DataFieldName="Root_Cause_Identified" 
            />
            <mmsi:DataField runat="server" ID="dfEnteredBy" 
                HeaderText="Entered_By" 
                DataType="DropDownList" 
                DataFieldName="Entered_By" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select Distinct Entered_By as Val, Entered_By as txt from tblWQ_Complaints where isNull(Entered_By,'') <> '' order by 1"
            />
            <mmsi:DataField runat="server" ID="dfFollowUpPostCardSent"
                HeaderText="Follow Up Postcard Sent : "
                DataType="BooleanDropDown" DataFieldName="FollowUpPostCardSent" />
                        
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
        <asp:Button runat="server" ID="btnMap" Visible="true" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="Complaint_Number"
            AllowSorting="true" AllowPaging="true" PageSize="20"
            AlternatingRowStyle-CssClass="HRAlternatingRow"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="SELECT
	            t.[Complaint_Number],
	            t.[ORCOM_OrderNumber],
	            l3.[LookupValue] AS [ORCOM_OrderType],
	            t.[Date_Complaint_Received],
	            t.[InitialLocalResponseDate],
	            l2.[LookupValue] AS [InitialLocalResponseType],
	            t.[Entered_By],
	            t.[Complaint_Close_Date],
	            t.[Closed_By],
	            t.[Customer_Name],
	            t.[Home_Phone_Number],
	            t.[Work_Phone_Number],
	            t.[Ext],
	            t.[Street_Number],
	            t.[Street_Name],
	            t.[Apartment_Number],
	            t2.[Town] AS [Town_Name],
	            t3.[Name] AS [Town_Section],
	            t.[State],
	            t.[Zip_Code],
	            t.[Premise_Number],
	            t.[Service_Number],
	            t.[Account_Number],
	            l10.[LookupValue] AS [ComplaintType],
	            t.[Complaint_Description],
	            t.[Complaint_Start_Date],
	            l8.[LookupValue] AS [WQ_Complaint_Problem_Area],
	            l9.[LookupValue] AS [WQ_Complaint_Source],
	            t.[Site_Visit_Required],
	            t.[Site_Visit_By],
	            t.[Site_Comments],
	            t.[Water_Filter_On_Complaint_Source],
	            t.[Cross_Connection_Detected],
	            t.[Nearest_Hydrant],
	            t.[Material_Of_Main],
	            l5.[LookupValue] AS [Size_Of_Main],
	            t.[Service_Year_Installed],
	            t.[Material_Of_Service],
	            l7.[LookupValue] AS [WQ_Complaint_Probable_Cause],
	            l6.[LookupValue] AS [WQ_Complaint_Action_Taken],
	            t.[Customer_Anticipated_Followup_Date],
	            t.[Actual_Customer_Followup_Date],
	            l.[LookupValue] AS [Customer_Expectation],
	            l1.[LookupValue] AS [Customer_Satisfaction],
	            t.[Customer_Satisfaction_Followup_Letter],
	            t.[Customer_Satisfaction_Followup_Call],
	            t.[Customer_Satisfaction_Followup_Comments],
	            t.[Root_Cause_Identified],
	            l4.[LookupValue] AS [Root_Cause],
	            cast(t1.[PWSID] as varchar(255)) AS [PWSID],
                oc.OperatingCenterCode as [OpCode],
                t.[Town],
                t.[WQ_Complaint_Type],
                t.[Customer_Satisfaction] AS [CustomerSatisfactionID],
                t.[FollowUpPostCardSent], 
                c.Latitude, c.Longitude
            FROM [tblWQ_Complaints] AS t
            LEFT JOIN
	            [Coordinates] AS c
            ON
	            t.[CoordinateID] = c.[CoordinateID]
            LEFT JOIN
	            [Lookup] AS l
            ON
	            t.[Customer_Expectation] = l.[LookupID]
            LEFT JOIN
	            [Lookup] AS l1
            ON
	            t.[Customer_Satisfaction] = l1.[LookupID]
            LEFT JOIN
	            [Lookup] AS l2
            ON
	            t.[InitialLocalResponseType] = l2.[LookupID]
            LEFT JOIN
	            [Lookup] AS l3
            ON
	            t.[ORCOM_OrderType] = l3.[LookupID]
            LEFT JOIN
	            [PublicWaterSupplies] AS t1
            ON
	            t.[PWSID] = t1.[Id]
            LEFT JOIN
                OperatingCenters as OC
            on
                t1.OperatingCenterID = oc.OperatingCenterId
            LEFT JOIN
	            [Lookup] AS l4
            ON
	            t.[Root_Cause] = l4.[LookupID]
            LEFT JOIN
	            [Lookup] AS l5
            ON
	            t.[Size_Of_Main] = l5.[LookupID]
            LEFT JOIN
	            [Towns] AS t2
            ON
	            t.[Town] = t2.[TownID]
            LEFT JOIN
	            [TownSections] AS t3
            ON
	            t.[Town_Section] = t3.[TownSectionID]
            LEFT JOIN
	            [Lookup] AS l6
            ON
	            t.[WQ_Complaint_Action_Taken] = l6.[LookupID]
            LEFT JOIN
	            [Lookup] AS l7
            ON
	            t.[WQ_Complaint_Probable_Cause] = l7.[LookupID]
            LEFT JOIN
	            [Lookup] AS l8
            ON
	            t.[WQ_Complaint_Problem_Area] = l8.[LookupID]
            LEFT JOIN
	            [Lookup] AS l9
            ON
	            t.[WQ_Complaint_Source] = l9.[LookupID]
            LEFT JOIN
	            [Lookup] AS l10
            ON
	            t.[WQ_Complaint_Type] = l10.[LookupID];" />
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" OnPreInit="DataElement1_PreInit"
            DataElementName = "Water Quality Complaints"
            DataElementParameterName = "Complaint_Number"
            DataElementTableName = "tblWQ_Complaints"
        >
            <mmsi:DataElementField runat="server" DataFieldName="CoordinateID" HeaderName="CoordinateID" Type="int" ID="defCoordinateID" />
            <mmsi:DataElementField runat="server" DataFieldName="ORCOM_OrderNumber" HeaderName="ORCOM_OrderNumber" Type="VarChar" ID="defORCOM_OrderNumber" />
            <mmsi:DataElementField runat="server" DataFieldName="ORCOM_OrderType" HeaderName="ORCOM_OrderType" Type="int" ID="defORCOM_OrderType" />
            <mmsi:DataElementField runat="server" DataFieldName="PWSID" HeaderName="PWSID" Type="Int" ID="defPWSID" />
            <mmsi:DataElementField runat="server" DataFieldName="WQ_Complaint_Type" HeaderName="WQ_Complaint_Type" Type="int" ID="defWQ_Complaint_Type" />
            <mmsi:DataElementField runat="server" DataFieldName="Date_Complaint_Received" HeaderName="Date_Complaint_Received" Type="DateTime2" ID="defDate_Complaint_Received" />
            <mmsi:DataElementField runat="server" DataFieldName="InitialLocalResponseDate" HeaderName="Initial Local Response Date" Type="DateTime2" ID="defInitialLocalResponseDate" />
            <mmsi:DataElementField runat="server" DataFieldName="InitialLocalResponseType" HeaderName="Initial Local Response Type" Type="int" ID="defInitialLocalResponseType" />
            <mmsi:DataElementField runat="server" DataFieldName="Entered_By" HeaderName="Entered_By" Type="nvarchar" ID="defEntered_By" />
            <mmsi:DataElementField runat="server" DataFieldName="Complaint_Close_Date" HeaderName="Complaint_Close_Date" Type="DateTime2" ID="defComplaint_Close_Date" />
            <mmsi:DataElementField runat="server" DataFieldName="Closed_By" HeaderName="Closed_By" Type="nvarchar" ID="defClosed_By" />
            <mmsi:DataElementField runat="server" DataFieldName="Customer_Name" HeaderName="Customer_Name" Type="nvarchar" ID="defCustomer_Name" />
            <mmsi:DataElementField runat="server" DataFieldName="Home_Phone_Number" HeaderName="Home_Phone_Number" Type="nvarchar" ID="defHome_Phone_Number" />
            <mmsi:DataElementField runat="server" DataFieldName="Ext" HeaderName="Ext" Type="nvarchar" ID="defExt" />
            <mmsi:DataElementField runat="server" DataFieldName="Street_Number" HeaderName="Street_Number" Type="nvarchar" ID="defStreet_Number" />
            <mmsi:DataElementField runat="server" DataFieldName="Street_Name" HeaderName="Street_Name" Type="nvarchar" ID="defStreet_Name" />
            <mmsi:DataElementField runat="server" DataFieldName="Apartment_Number" HeaderName="Apartment_Number" Type="nvarchar" ID="defApartment_Number" />
            <mmsi:DataElementField runat="server" DataFieldName="Town" HeaderName="Town" Type="int" ID="defTown" />
            <mmsi:DataElementField runat="server" DataFieldName="Town_Section" HeaderName="Town_Section" Type="int" ID="defTown_Section" />
            <mmsi:DataElementField runat="server" DataFieldName="State" HeaderName="State" Type="nvarchar" ID="defState" />
            <mmsi:DataElementField runat="server" DataFieldName="Zip_Code" HeaderName="Zip_Code" Type="nvarchar" ID="defZip_Code" />
            <mmsi:DataElementField runat="server" DataFieldName="Premise_Number" HeaderName="Premise_Number" Type="nvarchar" ID="defPremise_Number" />
            <mmsi:DataElementField runat="server" DataFieldName="Service_Number" HeaderName="Service_Number" Type="nvarchar" ID="defService_Number" />
            <mmsi:DataElementField runat="server" DataFieldName="Account_Number" HeaderName="Account_Number" Type="nvarchar" ID="defAccount_Number" />
            <mmsi:DataElementField runat="server" DataFieldName="Complaint_Description" HeaderName="Complaint_Description" Type="text" ID="defComplaint_Description" />
            <mmsi:DataElementField runat="server" DataFieldName="Complaint_Start_Date" HeaderName="Complaint_Start_Date" Type="datetime" ID="defComplaint_Start_Date" />
            <mmsi:DataElementField runat="server" DataFieldName="WQ_Complaint_Problem_Area" HeaderName="WQ_Complaint_Problem_Area" Type="int" ID="defWQ_Complaint_Problem_Area" ControlCssClass="NormalWidthSelect" />
            <mmsi:DataElementField runat="server" DataFieldName="WQ_Complaint_Source" HeaderName="WQ_Complaint_Source" Type="int" ID="defWQ_Complaint_Source" ControlCssClass="NormalWidthSelect" />
            <mmsi:DataElementField runat="server" DataFieldName="Site_Visit_Required" HeaderName="Site_Visit_Required" Type="bit" ID="defSite_Visit_Required" />
            <mmsi:DataElementField runat="server" DataFieldName="Site_Visit_By" HeaderName="Site_Visit_By" Type="nvarchar" ID="defSite_Visit_By" />
            <mmsi:DataElementField runat="server" DataFieldName="Site_Comments" HeaderName="Site_Comments" Type="ntext" ID="defSite_Comments" />
            <mmsi:DataElementField runat="server" DataFieldName="Water_Filter_On_Complaint_Source" HeaderName="Water_Filter_On_Complaint_Source" Type="bit" ID="defWater_Filter_On_Complaint_Source" />
            <mmsi:DataElementField runat="server" DataFieldName="Cross_Connection_Detected" HeaderName="Cross_Connection_Detected" Type="bit" ID="defCross_Connection_Detected" />
            <mmsi:DataElementField runat="server" DataFieldName="WQ_Complaint_Probable_Cause" HeaderName="WQ_Complaint_Probable_Cause" Type="int" ID="defWQ_Complaint_Probable_Cause" />
            <mmsi:DataElementField runat="server" DataFieldName="WQ_Complaint_Action_Taken" HeaderName="WQ_Complaint_Action_Taken" Type="int" ID="defWQ_Complaint_Action_Taken" />
            <mmsi:DataElementField runat="server" DataFieldName="Customer_Anticipated_Followup_Date" HeaderName="Customer_Anticipated_Followup_Date" Type="datetime" ID="defCustomer_Anticipated_Followup_Date" />
            <mmsi:DataElementField runat="server" DataFieldName="Actual_Customer_Followup_Date" HeaderName="Actual_Customer_Followup_Date" Type="datetime" ID="defActual_Customer_Followup_Date" />
            <mmsi:DataElementField runat="server" DataFieldName="Customer_Expectation" HeaderName="Customer_Expectation" Type="int" ID="defCustomer_Expectation" ControlCssClass="NormalWidthSelect" />
            <mmsi:DataElementField runat="server" DataFieldName="Customer_Satisfaction" HeaderName="Customer_Satisfaction" Type="int" ID="defCustomer_Satisfaction" />
            <mmsi:DataElementField runat="server" DataFieldName="Customer_Satisfaction_Followup_Letter" HeaderName="Customer_Satisfaction_Followup_Letter" Type="bit" ID="defCustomer_Satisfaction_Followup_Letter" />
            <mmsi:DataElementField runat="server" DataFieldName="Customer_Satisfaction_Followup_Call" HeaderName="Customer_Satisfaction_Followup_Call" Type="bit" ID="defCustomer_Satisfaction_Followup_Call" />
            <mmsi:DataElementField runat="server" DataFieldName="Customer_Satisfaction_Followup_Comments" HeaderName="Customer_Satisfaction_Followup_Comments" Type="ntext" ID="defCustomer_Satisfaction_Followup_Comments" />
            <mmsi:DataElementField runat="server" DataFieldName="FollowUpPostCardSent" HeaderName="Follow Up Postcard Sent?" Type="bit" ID="defFollowUpPostCardSent" />
            <mmsi:DataElementField runat="server" DataFieldName="Root_Cause_Identified" HeaderName="Root_Cause_Identified" Type="bit" ID="defRoot_Cause_Identified" />
            <mmsi:DataElementField runat="server" DataFieldName="Root_Cause" HeaderName="Root_Cause" Type="int" ID="defRoot_Cause" ControlCssClass="NormalWidthSelect" />
        </mmsi:DataElement>
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="44" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="44" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    
</asp:Content>
