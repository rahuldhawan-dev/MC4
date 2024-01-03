<%@ Page Title="" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="UploadCount.aspx.cs" Inherits="MapCall.Modules.FieldServices.UploadCount" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Image Upload Counts
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" id="DataField1" DataType="DropDownList"
                HeaderText="OpCntr :"
                DataFieldName="OperatingCenterCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct OperatingCenterCode as val, OperatingCenterCode as txt from OperatingCenters order by OperatingCenterCode asc"
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
            AutoGenerateColumns="true" Font-Size="Small" >
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="System.Data.SqlClient"
            CancelSelectOnNullParameter="false"
            SelectCommand="
Declare @ImageCounts TABLE (OperatingCenterCode varchar(5), [Year] int, [Month] int, [Type] varchar(10), [Total] int)

-- NJ TAP
Insert Into @ImageCounts
	select distinct 
		oc.OperatingCenterCode,
		year(CreatedAt),
		month(CreatedAt), 'Tap',
		count(1)
	from 
		TapImages 
    inner join OperatingCenters oc on oc.OperatingCenterId = TapImages.OperatingCenterId
	where
		CreatedAt is not null
	group by
		OperatingCenterCode, year(CreatedAt), month(CreatedAt)
	order by 
		1, 2
        
-- VALVES - [CreatedAt] select * from NYValve
-- NJValve, NYValve
Insert Into @ImageCounts
	select distinct 
		oc.OperatingCenterCode,
		year(CreatedAt),
		month(CreatedAt), 'Valve',
		count(1)
	from 
		ValveImages 
    inner join OperatingCenters oc on oc.OperatingCenterId = ValveImages.OperatingCenterId
	where
		CreatedAt is not null
	group by
		oc.OperatingCenterCode, year(CreatedAt), month(CreatedAt)
	order by 
		1, 2


-- AS-BUILTS - [CreatedAt] - select * from NJAsBuilts
-- NJAsBuilts
Insert Into @ImageCounts
	select distinct 
		operatingCenterCode,
		year(CreatedAt),
		month(CreatedAt), 'AsBuilt',
		count(1)
	from 
		AsBuiltImages abi 
	left join
		operatingCenters oc on oc.OperatingCenterID = abi.OperatingCenterID
	where
		abi.operatingCenterId is not null
	and
		CreatedAt is not null
	group by
		operatingCenterCode, year(CreatedAt), month(CreatedAt)
	order by 
		1, 2

-- SELECT RESULTS
select 
	ic.[Year], 
	ic.[Month], 
	Datename(mm,cast(ic.[Month] as varchar(2)) + '/01/1900') as [Mon], 
	ic.OperatingCenterCode,
	[Type], 
	ic.[Total]
from
	@ImageCounts ic
where
	ic.[month] is not null
order by 
	[Year], 
	ic.[Month], 
	ic.[OperatingCenterCode],
	[Type]

            ">
        </asp:SqlDataSource>
        
    </asp:Panel>

</asp:Content>
