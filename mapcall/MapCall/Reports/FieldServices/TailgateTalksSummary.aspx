<%@ Page theme="bender" Title="Tailgate Talks Summary" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="TailgateTalksSummary.aspx.cs" Inherits="MapCall.Reports.FieldServices.TailgateTalksSummary" %>
<%@ Register Src="~/Controls/ChartWithSettings.ascx" TagName="ChartWithSettings" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagName="OpCntrDataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Tailgate Talks Summary
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">

    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <tr>
                <td class="leftcol">
                    Presented By :
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlPresentedBy"
                        DataSourceID="dsPresentedBy"
                        DataTextField="txt"
                        DataValueField="val"
                        AppendDataBoundItems="true"
                    >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsPresentedBy"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT distinct tblEmployeeID as val, replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') as [txt], Last_Name from tblEmployee order by Last_Name"
                    />
                </td>
            </tr>
            
            <tr>
                <td>
                    Tailgate Topic Category :
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlTailgateCategory"
                        DataSourceID="dsTailgateCategory"
                        DataTextField="txt"
                        DataValueField="val"
                        AppendDataBoundItems="true"
                    >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsTailgateCategory"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="
                            SELECT
	                            LookupValue AS [Txt],
	                            LookupID AS [Val]
                            FROM
	                            [Lookup]
                            where
	                            LookupType='TailgateCategory'
                            order by LookupValue
                        " />
                </td>
            </tr>
            
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
                            select distinct year(heldon) as txt, year(heldon) as val from tblTailGateTalks
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
		oc.OperatingCenterCode, 
		Year(t.HeldOn) as [Yr],
		(Case when (Month(t.HeldOn) = 1) then count(1) else 0 end) as [Jan],
		(Case when (Month(t.HeldOn) = 2) then count(1) else 0 end) as [Feb], 
		(Case when (Month(t.HeldOn) = 3) then count(1) else 0 end) as [Mar], 
		(Case when (Month(t.HeldOn) = 4) then count(1) else 0 end ) as [Apr], 
		(Case when (Month(t.HeldOn) = 5) then count(1) else 0 end ) as [May], 
		(Case when (Month(t.HeldOn) = 6) then count(1) else 0 end ) as [Jun], 
		(Case when (Month(t.HeldOn) = 7) then count(1) else 0 end ) as [Jul], 
		(Case when (Month(t.HeldOn) = 8) then count(1) else 0 end ) as [Aug], 
		(Case when (Month(t.HeldOn) = 9) then count(1) else 0 end ) as [Sep], 
		(Case when (Month(t.HeldOn) = 10) then count(1) else 0 end ) as [Oct], 
		(Case when (Month(t.HeldOn) = 11) then count(1) else 0 end ) as [Nov], 
		(Case when (Month(t.HeldOn) = 12) then count(1) else 0 end ) as [Dec]
	from 
		tblTailGateTalks as t
	left join 
		tblEmployee as e on e.tblEmployeeID = t.presentedby
	left join 
		tblTailgateTopics tt on t.tailgatetopicid = tt.tailgatetopicid
    left join
        OperatingCenters oc on oc.OperatingCenterId = e.OperatingCenterId
	where
		presentedby = isnull(@presentedby, presentedby)
	and
		isNull(tt.tailgatecategory, '') = isNull(@tailgatecategory, isNull(tailgatecategory,''))
	and
	    year(t.HeldOn) = isNull(@year, year(t.HeldOn))
	group by 
		oc.OperatingCenterCode, Year(t.HeldOn), Month(t.heldOn)
	
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
order by yr desc, opcode
                ">
                <SelectParameters>
                    <asp:ControlParameter Name="year" ControlID="ddlYear" ConvertEmptyStringToNull="true" />
                    <asp:ControlParameter Name="presentedby" ControlID="ddlPresentedBy" ConvertEmptyStringToNull="true" />
                    <asp:ControlParameter Name="tailgatecategory" ControlID="ddlTailgateCategory" ConvertEmptyStringToNull="true" />
                </SelectParameters>
        </asp:SqlDataSource>
        <uc1:ChartWithSettings runat="server" id="cws"></uc1:ChartWithSettings>
    </asp:Panel>
 
</asp:Content>
