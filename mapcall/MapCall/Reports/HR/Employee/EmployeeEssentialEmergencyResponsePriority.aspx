<%@ Page Title="Employee Essential Emergency Response Priority" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="EmployeeEssentialEmergencyResponsePriority.aspx.cs" Inherits="MapCall.Reports.HR.Employee.EmployeeEssentialEmergencyResponsePriority" EnableEventValidation="false" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadTagScripts" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeadTag" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Employee Essential Emergency Response Priority
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
   <asp:Label runat="server" ID="lblPermissionErrors" />
   <asp:Panel runat="server" ID="pnlSearch" CssClass="searchPanel">
        <table>
            <mmsi:DataField runat="server" DataType="DropDownList"
                HeaderText="OpCode :"
                DataFieldName="opCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct operatingCenterCode + ' - ' + OperatingCenterName as txt, operatingCenterCode as val from OperatingCenters order by 1"
            />
            <mmsi:DataField runat="server" DataType="DropDownList"
                HeaderText="Status :"
                DataFieldName="StatusId"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct Description as txt, Id as val from EmployeeStatuses order by 1"
            />
            <mmsi:DataField runat="server" DataType="DropDownList"
                HeaderText="Department :"
                DataFieldName="departmentName"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct lookupvalue as txt, lookupID as val from Lookup 
                    where lookuptype = 'departmentName' and tablename = 'tblPosition_History' order by 1"
            />
            <!-- Position Category -->
            <mmsi:DataField runat="server" DataType="DropDownList"  
                DataFieldName="Category" HeaderText="Category : "
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct category as val, category as txt from tblpositions_classifications order by 1" />
            <!-- Position -->
            <mmsi:DataField runat="server" DataType="DropDownList"
                DataFieldName="Position_ID" HeaderText="Current Position" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="
                    SELECT distinct 
	                    positionID as val,
	                    isNull(coalesce(operatingcentercode, opcode),'') + '-' + isNull(position, '') + ' [' + cast(positionID as varchar(15)) + ']' as txt
                    from 
	                    tblPositions_Classifications 
                    left join 
	                    LocalBargainingUnits
                    on
	                    LocalBargainingUnits.Id = tblPositions_Classifications.localiD
                    left join 
	                    OperatingCenters
                    on
	                    OperatingCenters.OperatingCenterID = LocalBargainingUnits.OperatingCenterId
                    order by 
	                    2" />
            <!-- Reports To -->
            <mmsi:DataField runat="server" DataType="String" HeaderText="Reports To : <span class='help'>(last name only)</span>" DataFieldName="Reports To"  />
            <!-- Essential Position -->
            <mmsi:DataField runat="server" DataType="BooleanDropDown" HeaderText="Essential Position" DataFieldName="EssentialPosition" />
            <!-- Emergency Response Priority -->
            <mmsi:DataField runat="server" DataType="DropDownList" HeaderText="Emergency Response Priority" 
                DataFieldName="EmergencyResponsePriority" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select LookupValue as txt, LookupID as val from Lookup where LookupType = 'EmergencyResponsePriority' order by 1"
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
                        AllowSorting="true" DataSourceID="SqlDataSource1" >
                        <Columns>
                            <mmsinc:BoundField DataField="OpCode" HeaderText="OpCode" SortExpression="OpCode" />
                            <mmsinc:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                            <mmsinc:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                            <mmsinc:BoundField DataField="Reporting Location" HeaderText="Reporting Location" SortExpression="Reporting Location" />
                            <mmsinc:BoundField DataField="Position" HeaderText="Position" SortExpression="Position" />
                            <mmsinc:BoundField DataField="Position Category" HeaderText="Position Category" SortExpression="Position Category" />
                            <mmsinc:BoundField DataField="Reports To" HeaderText="Reports To" SortExpression="Reports To" />
                            <asp:CheckBoxField DataField="EssentialPosition" HeaderText="Essential Position" SortExpression="EssentialPosition" />
                            <mmsinc:BoundField DataField="EmergencyResponsePriorityText" HeaderText="Emergency Response Priority" SortExpression="EmergencyResponsePriorityText" />
                            <mmsinc:BoundField DataField="Last_Name" HeaderText="Last Name" SortExpression="Last_Name" />
                            <mmsinc:BoundField DataField="First_Name" HeaderText="First Name" SortExpression="First_Name" />
                            <mmsinc:BoundField DataField="Phone_Home" HeaderText="Phone Home" SortExpression="Phone_Home" />
                            <mmsinc:BoundField DataField="Phone_Cellular" HeaderText="Phone Cellular" SortExpression="Phone_Cellular" />
                            <mmsinc:BoundField DataField="Phone_Personal_Cellular" HeaderText="Phone Personal Cellular" SortExpression="Phone_Personal_Cellular" />
                            <mmsinc:BoundField DataField="Phone_Work" HeaderText="Phone Work" SortExpression="Phone_Work" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="
                            Select 
                                oc.OperatingCenterCode as [OpCode], 
                                s.[Description] as Status,
                                s.Id as StatusId,
                                D.LookupValue as [Department],
                                f.FacilityId as [Reporting Location],
					            pc.Position,
                                pc.category as [Position Category],
                                RT.Last_Name as [Reports To], 
					            pc.EssentialPosition,
					            (Select LookupValue from Lookup where LookupID = pc.EmergencyResponsePriority) as EmergencyResponsePriorityText,
					            E.Last_name, 
					            E.First_Name,
					            E.Phone_Home,
					            E.Phone_Cellular,
					            E.Phone_Personal_Cellular,
					            E.Phone_Work,
					            pc.EmergencyResponsePriority,
					            ph.departmentName,
					            ph.Position_ID
                            FROM 
                                [tblEmployee] E
            	            LEFT JOIN 
            	                [tblPosition_History] ph 
            	            ON 
            	                ph.tblEmployeeID = E.tblEmployeeID and ph.Position_History_ID = (Select top 1 #3.Position_History_ID from [tblPosition_History] #3 where #3.tblEmployeeID = E.tblEmployeeID order by Position_Start_Date desc)
            	            LEFT JOIN 
            	                [tblPositions_Classifications] pc 
            	            ON 
            	                pc.PositionID = ph.Position_ID
                            LEFT JOIN 
                                Lookup D on D.LookupID = ph.departmentName
                            LEFT JOIN 
                                tblEmployee RT on RT.tblEmployeeID = E.Reports_To
                            LEFT JOIN 
                                EmployeeStatuses s on s.Id = E.StatusId
                            LEFT JOIN
                                OperatingCenters oc on oc.OperatingCenterId = e.OperatingCenterId
                            LEFT JOIN
                                tblFacilities f on f.RecordId = e.ReportingFacilityId">
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
