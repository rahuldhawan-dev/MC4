<%@ Page Title="Tailgate Talks Training Hours" theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="TailgateTalksTrainingHours.aspx.cs" Inherits="MapCall.Reports.FieldServices.TailgateTalksTrainingHours" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Tailgate Talks Training Hours
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">

    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" id="DataField2" DataType="DropDownList"
                HeaderText="OpCode :"
                DataFieldName="OperatingCenterCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct OperatingCenterCode as txt, OperatingCenterCode as val from OperatingCenters where charindex('IL', OperatingCenterCode) = 0 order by 1"
            />
            <mmsi:DataField runat="server" id="DataField1" DataType="DropDownList"
                HeaderText="Employee :"
                DataFieldName="tblEmployeeID"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT distinct tblEmployeeID as val, FullName as [txt], Last_Name FROM Employees ORDER by Last_Name"
            />
            <mmsi:DataField runat="server" ID="DataField3" DataType="NumberRange" DataFieldName="Total Traing Hours" HeaderText="Training Time Hours : " />
            <mmsi:DataField runat="server" id="DataField4" DataType="DropDownList"
                HeaderText="Year :"
                DataFieldName="Year"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT distinct Year(HeldOn) as [val], Year(HeldOn) as [txt] FROM tblTailGateTalks order by 1 desc"
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
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="System.Data.SqlClient"
            SelectCommand="
create TABLE #tmp(OperatingCenterCode varchar(16), tblEmployeeID int, FullName varchar(255), Last_Name varchar(55), month int, year int, [hours] float)
insert into #tmp 
SELECT 
	e.OpCode, e.tblEmployeeID, e.FullName, e.Last_Name, 
	cast(month(tt.HeldOn) as varchar(2)) as [Month], 
	cast(year(tt.HeldOn) as varchar(4)) as [Year], 
	isNull(sum(TrainingTimeHours) , 0) as [Total Traing Hours]
FROM 
	employeeLink el
LEFT JOIN 
	tblTailGateTalks tt on el.datalinkID = tt.tailgatetalkID
LEFT JOIN
	Employees e on e.tblEmployeeID = el.tblEmployeeID
WHERE
	el.DataTypeID = 81
GROUP BY 
	e.OpCode, e.tblEmployeeID, e.FullName, e.Last_Name, 
	cast(month(tt.HeldOn) as varchar(2)),
	cast(year(tt.HeldOn) as varchar(4))

--select * from #tmp 

select distinct
	OperatingCenterCode, tblEmployeeID, FullName, Last_Name, 
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 1 and X.[Year] = T.[Year]) as [Jan],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 2 and X.[Year] = T.[Year]) as [Feb],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 3 and X.[Year] = T.[Year]) as [Mar],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 4 and X.[Year] = T.[Year]) as [Apr],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 5 and X.[Year] = T.[Year]) as [May],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 6 and X.[Year] = T.[Year]) as [Jun],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 7 and X.[Year] = T.[Year]) as [Jul],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 8 and X.[Year] = T.[Year]) as [Aug],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 9 and X.[Year] = T.[Year]) as [Sep],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 10 and X.[Year] = T.[Year]) as [Oct],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 11 and X.[Year] = T.[Year]) as [Nov],
	(select hours from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Month] = 12 and X.[Year] = T.[Year]) as [Dec],
	(select sum(hours) from #tmp X where T.tblEmployeeID = x.tblEmployeeID and X.[Year] = T.[Year]) as [Total],
	[year]
from 
	#tmp T
ORDER BY [Year], OperatingCenterCode, Last_Name

drop table #tmp
                ">
        </asp:SqlDataSource>

    </asp:Panel>

</asp:Content>
