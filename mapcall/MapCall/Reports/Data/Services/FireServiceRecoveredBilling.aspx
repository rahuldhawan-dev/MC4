﻿<%@ Page Title="Fire Services - Recovered Billing" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="FireServiceRecoveredBilling.aspx.cs" Inherits="MapCall.Reports.Data.Services.FireServiceRecoveredBilling" %>
<%@ Register Src="~/Controls/ChartWithSettings.ascx" TagName="ChartWithSettings" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Recovered Billing
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" id="ddlDistrictCode" DataType="DropDownList"
                HeaderText="District Code :"
                DataFieldName="DistrictCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select Distinct DistrictCode as val, DistrictCode as txt from InactiveServices order by 1"
            />  
            <mmsi:DataField runat="server" id="ddlYear" DataType="DropDownList"
                HeaderText="Year :"
                DataFieldName="Year"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select Distinct Year(BillingRestartDate) as val, Year(BillingRestartDate) as txt from InactiveServices where isNull(Year(BillingRestartDate), 0) <> 0 order by 1"
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
            AutoGenerateColumns="False" Font-Size="Small" >
            <Columns>
                <asp:BoundField DataField="Year" HeaderText="Year" 
                    SortExpression="Year" />
                <asp:BoundField DataField="DistrictCode" HeaderText="District Code" 
                    SortExpression="District Code" />
                <asp:BoundField DataField="AreaCodeDescription" HeaderText="Area Code Description" 
                    SortExpression="AreaCodeDescription" />
                <asp:BoundField DataField="Jan" DataFormatString="{0:c0}" HeaderText="Jan" 
                    ReadOnly="True" SortExpression="Jan" />
                <asp:BoundField DataField="Feb" DataFormatString="{0:c0}" HeaderText="Feb" 
                    ReadOnly="True" SortExpression="Feb" />
                <asp:BoundField DataField="Mar" DataFormatString="{0:c0}" HeaderText="Mar" 
                    ReadOnly="True" SortExpression="Mar" />
                <asp:BoundField DataField="Apr" DataFormatString="{0:c0}" HeaderText="Apr" 
                    ReadOnly="True" SortExpression="Apr" />
                <asp:BoundField DataField="May" DataFormatString="{0:c0}" HeaderText="May" 
                    ReadOnly="True" SortExpression="May" />
                <asp:BoundField DataField="Jun" DataFormatString="{0:c0}" HeaderText="Jun" 
                    ReadOnly="True" SortExpression="Jun" />
                <asp:BoundField DataField="Jul" DataFormatString="{0:c0}" HeaderText="Jul" 
                    ReadOnly="True" SortExpression="Jul" />
                <asp:BoundField DataField="Aug" DataFormatString="{0:c0}" HeaderText="Aug" 
                    ReadOnly="True" SortExpression="Aug" />
                <asp:BoundField DataField="Sep" DataFormatString="{0:c0}" HeaderText="Sep" 
                    ReadOnly="True" SortExpression="Sep" />
                <asp:BoundField DataField="Oct" DataFormatString="{0:c0}" HeaderText="Oct" 
                    ReadOnly="True" SortExpression="Oct" />
                <asp:BoundField DataField="Nov" DataFormatString="{0:c0}" HeaderText="Nov" 
                    ReadOnly="True" SortExpression="Nov" />
                <asp:BoundField DataField="Dec" DataFormatString="{0:c0}" HeaderText="Dec" 
                    ReadOnly="True" SortExpression="Dec" />
                <asp:BoundField DataField="Total" DataFormatString="{0:c0}" HeaderText="Total" 
                    ReadOnly="True" SortExpression="Total" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="System.Data.SqlClient"
            CancelSelectOnNullParameter="false"
            SelectCommand="
Select 
	Year(RecoveredBillingDate) as [Year],
	DistrictCode,
	AreaCodeDescription,
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 1 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Jan],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 2 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Feb],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 3 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Mar],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 4 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Apr],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 5 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [May],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 6 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Jun],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 7 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Jul],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 8 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Aug],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 9 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Sep],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 10 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Oct],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 11 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Nov],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode and Month(fs.RecoveredBillingDate) = 12 group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as [Dec],
	(Select Sum(RecoveredBillingValue) from InactiveServices as fs where fs.AreaCodeDescription = #F.AreaCodeDescription and fs.DistrictCode = #F.DistrictCode group by year(fs.RecoveredBillingDate) having year(fs.RecoveredBillingDate) = year(#F.RecoveredBillingDate)) as Total
from 
    InactiveServices #F
Where
    #F.FireService = 1
AND
	RecoveredBillingDate is not null 
GROUP BY 
	Year(RecoveredBillingDate), DistrictCode, AreaCodeDescription
ORDER BY 1,2
            ">
        </asp:SqlDataSource>
        
        <uc1:ChartWithSettings runat="server" id="cws"></uc1:ChartWithSettings>

    </asp:Panel>

</asp:Content>
