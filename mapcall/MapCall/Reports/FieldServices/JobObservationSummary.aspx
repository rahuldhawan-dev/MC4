<%@ Page Title="Job Observation Summary" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="JobObservationSummary.aspx.cs" Inherits="MapCall.Reports.FieldServices.JobObservationSummary" %>
<%@ Register Src="~/Controls/ChartWithSettings.ascx" TagName="ChartWithSettings" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagName="OpCntrDataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Job Observation Summary
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">

    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <tr>
                <td style="text-align:right">
                    Year :   
                </td>
                <td>
                      <asp:DropDownList runat="server" ID="ddlYear"
                        DataSourceID="dsYears"
                        DataTextField="txt"
                        DataValueField="val"
                        AppendDataBoundItems="true"
                    >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsYears"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="
                            select distinct year(heldon) as txt, year(heldon) as val from tblTailGateTalks where heldon is not null order by year(heldon) desc
                        " />
                
                </td>
            </tr>
            
            <mmsi:OpCntrDataField runat="server" DataFieldName="OpCode" UseText="true" HeaderText="OpCode : " />
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
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="true" Font-Size="Larger" HeaderStyle-HorizontalAlign="Center" />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="System.Data.SqlClient"
            CancelSelectOnNullParameter="false"
            SelectCommand="
            --REALLY? REALLY??
declare @tbl TABLE (OpCode varchar(20), Yr int, Jan int, Feb int, Mar int, Apr int, May int, Jun int, Jul int, Aug int, Sep int, Oct int, Nov int, Dec int)

insert into @tbl
	select 
		o.OperatingCenterCode, 
		Year(t.ObservationDate) as [Yr],
		(Case when (Month(t.ObservationDate) = 1) then count(1) else 0 end) as [Jan],
		(Case when (Month(t.ObservationDate) = 2) then count(1) else 0 end) as [Feb], 
		(Case when (Month(t.ObservationDate) = 3) then count(1) else 0 end) as [Mar], 
		(Case when (Month(t.ObservationDate) = 4) then count(1) else 0 end ) as [Apr], 
		(Case when (Month(t.ObservationDate) = 5) then count(1) else 0 end ) as [May], 
		(Case when (Month(t.ObservationDate) = 6) then count(1) else 0 end ) as [Jun], 
		(Case when (Month(t.ObservationDate) = 7) then count(1) else 0 end ) as [Jul], 
		(Case when (Month(t.ObservationDate) = 8) then count(1) else 0 end ) as [Aug], 
		(Case when (Month(t.ObservationDate) = 9) then count(1) else 0 end ) as [Sep], 
		(Case when (Month(t.ObservationDate) = 10) then count(1) else 0 end ) as [Oct], 
		(Case when (Month(t.ObservationDate) = 11) then count(1) else 0 end ) as [Nov], 
		(Case when (Month(t.ObservationDate) = 12) then count(1) else 0 end ) as [Dec]
	from 
		tblJobObservations as t
	left join
		OperatingCenters o on t.opCode = o.OperatingCenterID
	where 
	    year(t.ObservationDate) = isNull(@year, year(t.ObservationDate))
	group by 
		o.OperatingCenterCode, 
		Year(t.ObservationDate), 
		Month(t.ObservationDate)
	
select 
	OpCode, 
	yr as Year, 
	sum(jan) as Jan, 
	sum(feb) as Feb, 
	sum(mar) as Mar, 
	sum(apr) as Apr, 
	sum(may) as May, 
	sum(jun) as Jun, 
	sum(jul) as Jul, 
	sum(aug) as Aug, 
	sum(sep) as Sep, 
	sum(oct) as Oct, 
	sum(nov) as Nov, 
	sum(dec) as Dec
from 
	@tbl 
group by 
	opcode, yr
order by 
    yr desc, opcode
                ">
                <SelectParameters>
                    <asp:ControlParameter Name="year" ControlID="ddlYear" ConvertEmptyStringToNull="true" />
                </SelectParameters>
        </asp:SqlDataSource>
        <uc1:ChartWithSettings runat="server" id="cws"></uc1:ChartWithSettings>
    </asp:Panel>
</asp:Content>
