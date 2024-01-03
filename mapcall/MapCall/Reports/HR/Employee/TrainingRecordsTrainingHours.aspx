<%@ Page Title="Training Records Training Hours" theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="TrainingRecordsTrainingHours.aspx.cs" Inherits="MapCall.Reports.HR.Employee.TrainingRecordsTrainingHours" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Training Records Training Hours
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" id="DataField2" DataType="DropDownList"
                HeaderText="OpCode :"
                DataFieldName="OpCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct OperatingCenterCode as txt, OperatingCenterCode as val from OperatingCenters where charindex('IL', OperatingCenterCode) = 0 order by 1"
            />
            <mmsi:DataField runat="server" id="DataField1" DataType="DropDownList"
                HeaderText="Employee :"
                DataFieldName="tblEmployeeID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT distinct tblEmployeeID as val, FullName as [txt], Last_Name FROM Employees ORDER by Last_Name"
            />
            <mmsi:DataField runat="server" ID="DataField3" DataType="NumberRange" DataFieldName="Total Training Hours" HeaderText="Training Time Hours : " />
            <mmsi:DataField runat="server" id="DataField4" DataType="DropDownList"
                HeaderText="Year :"
                DataFieldName="Year"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT distinct Year(HeldOn) as [val], Year(HeldOn) as [txt] FROM tblTrainingRecords"
            />
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
        </center>
        <br />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            AutoGenerateColumns="true"
            AllowSorting="true"
            >
        </asp:GridView>
        <!-- NOTE: WHEN YOU MOVE THIS TO MVC, APPLY THE SEARCH PARAMETERS TO THE FIRST WHERE CLAUSE AFTER WITH CTE AS... -->
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="System.Data.SqlClient"
            SelectCommand="
WITH CTE AS (SELECT 
	e.OpCode, e.tblEmployeeID, e.FullName, e.Last_Name, 
	month(tt.HeldOn) as [Month], 
	year(tt.HeldOn) as [Year],
	tm.TotalHours
FROM 
	employeeLink el
LEFT JOIN 
	tblTrainingRecords tt on el.datalinkID = tt.TrainingRecordID
LEFT JOIN
	tblTrainingModules tm on tt.TrainingModuleID = tm.TrainingModuleID
LEFT JOIN
	Employees e on e.tblEmployeeID = el.tblEmployeeID
WHERE
	el.DataTypeID = 88)

select 
	OpCode,
	tblEmployeeID,
	FullName,
	Last_Name,
	[1] as Jan,
	[2] as Feb,
	[3] as Mar,
	[4] as Apr,
	[5] as May,
	[6] as Jun,
	[7] as Jul,
	[8] as Aug,
	[9] as Sep,
	[10] as Oct,
	[11] as Nov,
	[12] as Dec,
	(select sum(TotalHours) from CTE where tblEmployeeID = t.tblEmployeeID and Year = t.Year) as [Total],
	[Year]
from
(
select * from CTE
PIVOT
(
	sum(TotalHours) FOR Month IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
) as pvtMonth
) t
                ">
        </asp:SqlDataSource>

    </asp:Panel>

</asp:Content>
