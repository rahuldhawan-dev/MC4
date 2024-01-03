<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="PositionHistory.aspx.cs" Inherits="MapCall.Modules.HR.Employee.PositionHistory" Title="Position History" %>

<%@ Register Src="~/Controls/HR/dropdownlists/ddlOpCode.ascx" TagName="ddlOpCode" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlPositions.ascx" TagName="ddlPositions" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlScheduleType.ascx" TagName="ddlScheduleType" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/date.ascx" TagName="date" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/PositionHistory.ascx" TagName="PositionHistory" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Position History
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Use these forms to search, view, and edit the position history.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <tr>
                <td style="text-align:right; ">
                    OpCode:
                </td>
                <td>
                    <mmsi:ddlOpCode runat="server" ID="ddlOpCode" /> 
                </td>
            </tr>

            <tr>
                <td style="text-align:right; ">
                    Local:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLocal" 
                        DataSourceID="dsLocal" 
                        AppendDataBoundItems="true"
                        DataTextField="Name"
                        DataValueField="Id"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsLocal"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT * from LocalBargainingUnits order by 1"
                        >
                    </asp:SqlDataSource>
                </td>
            </tr>
                        
            <tr>
                <td style="text-align:right; ">
                    Status:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlStatus" AppendDataBoundItems="true" Width="310px">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                        <asp:ListItem>Active</asp:ListItem>
                        <asp:ListItem>Inactive</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;" nowrap>
                    Status Change Reason:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlStatus_Change_Reason" 
                        DataSourceID="dsStatus_Change_Reason" 
                        AppendDataBoundItems="true"
                        DataTextField="LookupValue"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsStatus_Change_Reason"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="select LookupValue from Lookup where Lookuptype = 'Status_Change_Reason' order by 1"
                        >
                    </asp:SqlDataSource> 
                </td>
            </tr>
           <tr>
                <td style="text-align:right;">
                    Department (From Position):
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDepartment" AppendDataBoundItems="true"
                        DataSourceID="dsDepartment" 
                        DataTextField="Department">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsDepartment"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT distinct Department from tblPositions_Classifications order by 1"
                        >
                    </asp:SqlDataSource> 
                </td>
            </tr>
           <tr>
                <td style="text-align:right;">
                    Department :
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDepartmentName" AppendDataBoundItems="true"
                        DataSourceID="dsDepartmentName" 
                        DataTextField="LookupValue"
                        DataValueField="LookupID"
                    >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsDepartmentName"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'DepartmentName' and tableName='tblPosition_History' order by 2"
                        >
                    </asp:SqlDataSource> 
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Position Control Number
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPCN"></asp:TextBox> 
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Employee ID:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEmployeeID"></asp:TextBox> 
                </td>
            </tr>            
            <tr>
                <td style="text-align:right;">
                    Employee Last Name:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtLast_Name"></asp:TextBox> 
                </td>
            </tr>            
            <tr>
                <td style="text-align:right;">
                    Position Posting ID:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPosition_Posting_ID"></asp:TextBox> 
                </td>
            </tr>   
           <tr>
                <td style="text-align:right; ">
                    Position : 
                </td>
                <td style="text-align:left;">
                    <mmsi:ddlPositions ID="ddlPositions" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right; ">
                    Position Sub Category:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPosition_Sub_Category"></asp:TextBox> 
                </td>
            </tr>   
            <tr>
                <td style="text-align:right; ">
                    Vacation Grouping:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtVacation_Grouping"></asp:TextBox> 
                </td>
            </tr>
            <tr>
                <td style="text-align:right; ">
                    Position Start Date:
                </td>
                <td>
                    <uc2:date runat="server" ID="txtPosition_Start_Date" SelectedIndex="5" />
                </td>
            </tr>  
            <tr>
                <td style="text-align:right; ">
                    Position End Date:
                </td>
                <td>
                    <uc2:date runat="server" ID="txtPosition_End_Date" SelectedIndex="5" />
                </td>
            </tr>              
            <tr>
                <td style="text-align:right; ">
                    Reporting Facility ID:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlReportingFacilityId" AppendDataBoundItems="true"
                                      DataSourceID="dsReportingFacility" 
                                      DataTextField="FacilityName"
                                      DataValueField="recordId"
                    >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsReportingFacility"
                                       ConnectionString="<%$ ConnectionStrings:MCProd %>"
                                       SelectCommand="select recordId, FacilityName from tblFacilities order by 2"
                    >
                    </asp:SqlDataSource> 
                </td>
            </tr>
            <tr>
                <td style="text-align:right; ">
                    Fully Qualified:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFully_Qualified">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="YES" Value="1"></asp:ListItem>
                        <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Schedule Type: 
                </td>
                <td>
                    <mmsi:ddlScheduleType ID="ddlSchedule" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Labor Category :
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlLaborCategoryTypeID" AppendDataBoundItems="true"
                        DataSourceID="dsLaborCategoryTypeID" 
                        DataTextField="LookupValue"
                        DataValueField="LookupID">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsLaborCategoryTypeID"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'LaborCategory' and tableName='tblPosition_History' order by 2"
                        >
                    </asp:SqlDataSource> 
                </td>
            </tr>    
            <tr>
                <td style="text-align:right;">
                    Employment Agency:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlEmploymentAgencyTypeID" AppendDataBoundItems="true"
                        DataSourceID="dsEmploymentAgencyTypeID" 
                        DataTextField="LookupValue"
                        DataValueField="LookupID">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsEmploymentAgencyTypeID"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'EmploymentAgency' and tableName='tblPosition_History' order by 2"
                        >
                    </asp:SqlDataSource> 
                </td>
            </tr> 
            <tr>
                <td style="text-align:right;">
                    Employment Level:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlEmploymentLevelTypeID" AppendDataBoundItems="true"
                        DataSourceID="dsEmploymentLevelTypeID" 
                        DataTextField="LookupValue"
                        DataValueField="LookupID">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsEmploymentLevelTypeID"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'EmploymentLevel' and tableName='tblPosition_History' order by 2"
                        >
                    </asp:SqlDataSource> 
                </td>
            </tr>        
            <tr>
                <td style="text-align:right; ">
                    Order By: 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlOrderBy">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                        <asp:ListItem Text="OpCode" Value="OpCode"></asp:ListItem>
                        <asp:ListItem Text="Status" Value="Status"></asp:ListItem>
                        <asp:ListItem Text="Department" Value="Department"></asp:ListItem>
                        <asp:ListItem Text="Last Name" Value="Last_Name"></asp:ListItem>
                        <asp:ListItem Text="Position" Value="Position"></asp:ListItem>
                        <asp:ListItem Text="Position Start Date" Value="Position_Start_Date"></asp:ListItem>
                        <asp:ListItem Text="Position End Date" Value="Position_End_Date"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 74px"></td>
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
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="False" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="Position_History_ID" DataSourceID="SqlDataSource1"
            EmptyDataText="There are no data records to display."
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            OnSorting="GridView1_Sorting"
            OnPageIndexChanged="GridView1_PageIndexChanged"
            PageSize="500"
            HeaderStyle-Font-Size="Smaller"
            AlternatingRowStyle-CssClass="HRAlternatingRow"
            
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" ControlStyle-CssClass="RightPadded" />
                <asp:BoundField DataField="Position_History_ID" HeaderText="Position_History_ID" InsertVisible="False" ReadOnly="True" SortExpression="Position_History_ID" />
                <asp:BoundField DataField="OpCode" HeaderText="OpCode" SortExpression="OpCode" />
                <asp:BoundField DataField="Local" HeaderText="Local" SortExpression="Local" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                <asp:BoundField DataField="Status_Change_Reason" HeaderText="Status Change Reason" SortExpression="Status_Change_Reason" />
                <asp:BoundField DataField="Department" HeaderText="Department(from Position)" SortExpression="Department" />
                <asp:BoundField DataField="DepartmentNameText" HeaderText="Department" SortExpression="DepartmentNameText" />
                <asp:BoundField DataField="PositionControlNumber" HeaderText="Position Control Number" SortExpression="PositionControlNumber" />
                <asp:BoundField DataField="EmployeeID" HeaderText="EmployeeID" SortExpression="EmployeeID" />
                <asp:BoundField DataField="Last_Name" HeaderText="Last Name" SortExpression="Last_Name" />
                <asp:BoundField DataField="Position_Posting_ID" HeaderText="Position Posting ID" SortExpression="Position_Posting_ID" />
                <asp:BoundField DataField="PositionText" HeaderText="Position" SortExpression="PositionText" />
                <asp:BoundField DataField="Position_Sub_Category" HeaderText="Position Sub Category" SortExpression="Position_Sub_Category" />
                <asp:BoundField DataField="Vacation_Grouping" HeaderText="Vacation Grouping" SortExpression="Vacation_Grouping" />
                <asp:BoundField DataFormatString="{0:d}" HtmlEncode="False" DataField="Position_Start_Date" HeaderText="Position Start Date" SortExpression="Position_Start_Date" />
                <asp:BoundField DataFormatString="{0:d}" HtmlEncode="False" DataField="Position_End_Date" HeaderText="Postion End Date" SortExpression="Position_End_Date" />
                <asp:BoundField DataField="FacilityName" HeaderText="Reporting FacilityID" SortExpression="FacilityName" />
                <asp:BoundField DataField="License_Requirement_Attainment" HeaderText="License Requirement Attainment" SortExpression="License_Requirement_Attainment" />
                <asp:CheckBoxField DataField="Fully_Qualified" HeaderText="Fully Qualified" SortExpression="Fully_Qualified" />
                <asp:BoundField DataField="ScheduleType" HeaderText="Schedule Type" SortExpression="ScheduleType" />
                <asp:CheckBoxField HeaderText="On Call Requirement" DataField="On_Call_Requirement" SortExpression="On_Call_Requirement" />
                <asp:CheckBoxField HeaderText="Essential Position" DataField="EssentialPosition" SortExpression="EssentialPosition" />
                <asp:BoundField DataField="EmergencyResponsePriority" HeaderText="EmergencyResponsePriority" SortExpression="EmergencyResponsePriority" />
                <asp:BoundField DataField="LaborCategoryTypeDescription" HeaderText="Labor Category" SortExpression="LaborCategoryTypeDescription" />
                <asp:BoundField DataField="EmploymentAgencyTypeDescription" HeaderText="Employment Agency" SortExpression="EmploymentAgencyTypeDescription" />
                <asp:BoundField DataField="EmploymentLevelTypeDescription" HeaderText="Employment Level" SortExpression="EmploymentLevelTypeDescription" />
                <asp:BoundField DataField="NoteCount" HeaderText="Notes" SortExpression="NoteCount" />
                <asp:BoundField DataField="DocumentCount" HeaderText="Documents" SortExpression="DocumentCount" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:PositionHistory runat="server" ID="PositionHistory1" OnDataBinding="PositionHistory1_DataBinding" OnItemInserted="PositionHistory1_ItemInserted" />
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="28" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="28" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        SelectCommand="
            SELECT  Distinct 
                [Position_History_ID], 
				oc.operatingCenterCode as [OpCode], 
                s.Description as Status, 
                [Status_Change_Reason], 
                [Department], 
                [tblEmployee].[EmployeeID], 
                [Last_Name], 
                [Position_Posting_ID], 
                [Position_ID], 
                [tblPositions_classifications].[Position], 
                [Position_Sub_Category], 
                [Vacation_Grouping], 
                [Position_Start_Date], 
                [Position_End_Date], 
                '[' + ocf.OperatingCenterCode + '-' + cast(recordId as varchar) + '] - ' + isNull(facilityName,'') as [FacilityName],
                Lookup.LookupValue as [License_Requirement_Attainment], 
                [Fully_Qualified], 
                LocalBargainingUnits.Id as LocalId, 
                LocalBargainingUnits.Name as Local,
                isNull(oc.operatingCenterCode,'') + '/' + isnull(tblPositions_Classifications.OpCode,'') + '-' + isNull(tblPositions_Classifications.position, '') as [PositionText]
                ,[tblPosition_History].ScheduleTypeID, [ScheduleType].[ScheduleType] , On_Call_Requirement
                , (Select Count(noteID) FROM Note where dataTypeID = 28 and dataLinkID = Position_History_ID) as [NoteCount]
                , (Select Count(documentID) FROM DocumentLink where dataTypeID = 28 and dataLinkID = Position_History_ID) as [DocumentCount]
                , tblEmployee.tblemployeeID, 
                DepartmentName, 
                L2.LookupValue as DepartmentNameText,
				[tblPositions_classifications].EssentialPosition,
				l3.LookupValue as EmergencyResponsePriority,
                [tblPosition_History].EmployeePositionControlID,
                epc.PositionControlNumber as PositionControlNumber,
                LaborCategoryTypeID,
                lookupLaborCategoryTypeID.LookupValue as LaborCategoryTypeDescription,
                EmploymentAgencyTypeID,
                lookupEmploymentAgencyTypeID.LookupValue as EmploymentAgencyTypeDescription,
                EmploymentLevelTypeID,
                lookupEmploymentLevelTypeID.LookupValue as EmploymentLevelTypeDescription

            FROM [tblPosition_History]
            LEFT JOIN [EmployeePositionControls] epc         ON epc.EmployeePositionControlID = [tblPosition_History].EmployeePositionControlID
            LEFT JOIN [tblPositions_classifications]         ON [tblPositions_classifications].positionID = [tblPosition_History].position_id
            LEFT JOIN [LocalBargainingUnits]                 ON [LocalBargainingUnits].Id = tblPositions_Classifications.LocalID
            LEFT JOIN [tblEmployee]                          ON tblEmployee.tblemployeeID = [tblPosition_History].tblemployeeID
            LEFT JOIN [EmployeeStatuses] s                   ON s.Id = [tblEmployee].StatusId
            LEFT JOIN [ScheduleType]                         ON [ScheduleType].ScheduleTypeID = [tblPosition_History].ScheduleTypeID
            LEFT JOIN [Lookup]                               ON [Lookup].LookupID = [tblPositions_classifications].[License_Requirement_Attainment]
            LEFT JOIN [Lookup] L2                            ON L2.LookupID = [tblPosition_History].[DepartmentName]
			LEFT JOIN OperatingCenters oc                    ON oc.OperatingCenterID = LocalBargainingUnits.OperatingCenterId
			LEFT JOIN Lookup l3                              ON L3.lookupID = [tblPositions_classifications].EmergencyResponsePriority
            LEFT JOIN [Lookup] lookupLaborCategoryTypeID     ON lookupLaborCategoryTypeID.LookupID = [tblPosition_History].[LaborCategoryTypeID]
            LEFT JOIN [Lookup] lookupEmploymentAgencyTypeID  ON lookupEmploymentAgencyTypeID.LookupID = [tblPosition_History].[EmploymentAgencyTypeID]
            LEFT JOIN [Lookup] lookupEmploymentLevelTypeID   ON lookupEmploymentLevelTypeID.LookupID = [tblPosition_History].[EmploymentLevelTypeID]
            LEFT JOIN [tblFacilities] F ON F.recordId = tblPosition_History.ReportingFacilityId
            LEFT JOIN OperatingCenters ocf on ocf.OperatingCenterID = F.operatingCenterId
        ">
    </asp:SqlDataSource>
</asp:Content>
