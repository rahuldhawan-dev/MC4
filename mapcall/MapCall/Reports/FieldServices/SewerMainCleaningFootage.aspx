<%@ Page Title="Sewer Main Cleaning Footage" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="SewerMainCleaningFootage.aspx.cs" Inherits="MapCall.Reports.FieldServices.SewerMainCleaningFootage" EnableEventValidation="false" %>
<%@ Register Src="~/Controls/ddlMcProdOperatingCenter.ascx" TagName="ddlMcProdOperatingCenter" TagPrefix="mmsi" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Footage of Sewer Main Cleaned
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" CssClass="searchPanel">
        <table>
            <tr>
                <td class="label">OpCode : </td>
                <td class="field">
                    <mmsi:ddlMcProdOperatingCenter runat="server" id="ddlOpCntr" 
                        BaseRole="_Water Non Potable_Sewer" />
                </td>
            </tr>
            <tr>
                <td class="label">Town : </td>
                <td class="field">
                    <asp:DropDownList runat="server" ID="ddlTown" />
                    <atk:CascadingDropDown runat="server" ID="cddTowns" 
                        TargetControlID="ddlTown" ParentControlID="ddlOpCntr$ddlOpCntr" 
                        Category="Town" 
                        EmptyText="None Found" EmptyValue=""
                        PromptText="--Select Here--" PromptValue="" 
                        LoadingText="[Loading Towns...]"
                        ServicePath="~/Modules/Data/DropDowns.asmx" 
                        ServiceMethod="GetTownsByOperatingCenter"
                        SelectedValue='<%# Bind("TownID") %>' 
                    />
                </td>
            </tr>      
            <tr>
                <td class="label">Year : </td>
                <td class="field">
                    <asp:DropDownList runat="server" ID="ddlYear" AppendDataBoundItems="true"
                        DataSourceID="dsYear" DataTextField="year">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsYear"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="Select Distinct Year([Date]) as [year] from SewerMainCleanings order by 1 desc"
                        />
                </td>
            </tr>
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
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="True" 
                        AllowSorting="true" DataSourceID="SqlDataSource1">
                        
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        CancelSelectOnNullParameter="false"
                        SelectCommand="
                            declare @tbl TABLE (OpCntr varchar(20), Town varchar(55), Yr int, Jan int, Feb int, Mar int, Apr int, May int, Jun int, Jul int, Aug int, Sep int, Oct int, Nov int, Dec int)
                            insert into @tbl
                            select 
	                            oc.OperatingCenterCode, 
	                            T.Town as [Town],
	                            Year([Date]) as [Yr],
	                            (Case when (Month([Date]) = 1) then sum(FootageOfMainInspected) else 0 end) as [Jan],
	                            (Case when (Month([Date]) = 2) then sum(FootageOfMainInspected)  else 0 end)  as [Feb], 
	                            (Case when (Month([Date]) = 3) then sum(FootageOfMainInspected)  else 0 end)  as [Mar], 
	                            (Case when (Month([Date]) = 4) then sum(FootageOfMainInspected)  else 0 end)  as [Apr], 
	                            (Case when (Month([Date]) = 5) then sum(FootageOfMainInspected)  else 0 end)  as [May], 
	                            (Case when (Month([Date]) = 6) then sum(FootageOfMainInspected)  else 0 end)  as [Jun], 
	                            (Case when (Month([Date]) = 7) then sum(FootageOfMainInspected)  else 0 end)  as [Jul], 
	                            (Case when (Month([Date]) = 8) then sum(FootageOfMainInspected)  else 0 end)  as [Aug], 
	                            (Case when (Month([Date]) = 9) then sum(FootageOfMainInspected)  else 0 end)  as [Sep], 
	                            (Case when (Month([Date]) = 10) then sum(FootageOfMainInspected)  else 0 end)  as [Oct], 
	                            (Case when (Month([Date]) = 11) then sum(FootageOfMainInspected)  else 0 end)  as [Nov], 
	                            (Case when (Month([Date]) = 12) then sum(FootageOfMainInspected)  else 0 end)  as [Dec]
                            from
	                            SewerMainCleanings smc
                            inner join
	                            OperatingCenters oc
                            on
	                            smc.operatingCenterID = oc.OperatingCenterID
                            inner join
	                            Towns T
                            on
	                            smc.TownID = T.TownID
                            left join
                                SewerMainInspectionTypes smit
                            on
                                smit.Id = smc.InspectionTypeId
	                        where
	                            smc.operatingCenterID = isNull(@OpCntr, smc.operatingCenterID)
	                        and
	                            smc.TownID = isNull(@TownID, smc.TownID)
	                        and
	                            Year([Date]) = isNull(@Year, Year([Date]))
                            -- this column was added for MC-4834, so any cleanings prior to that ticket will have null
                            and
                                (smc.InspectionTypeId IS NULL OR smit.Description = 'MAIN CLEANING PM')
                            group by 
	                            oc.OperatingCenterCode, T.Town, Year([Date]), Month([Date])
                            order by 
	                            1, 2, 3

                            select 
	                            yr as Year, 
	                            OpCntr, 
	                            Town,
	                            sum(jan + feb + mar + apr + may + jun + jul + aug + sep + oct + nov + [dec]) as FootageOfMain,
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
	                            sum([dec]) as [Dec]
                            from 
	                            @tbl 
                            group by 
	                            OpCntr, town, yr
                            order by 
                                yr desc, OpCntr, town	                           
                        ">
                        <SelectParameters>
                            <asp:ControlParameter Name="OpCntr" ControlID="ddlOpCntr" PropertyName="SelectedValue" 
                                ConvertEmptyStringToNull="true" />
                            <asp:ControlParameter Name="TownID" ControlID="ddlTown" PropertyName="SelectedValue"
                                ConvertEmptyStringToNull="true" />
                            <asp:ControlParameter Name="Year" ControlID="ddlYear" PropertyName="SelectedValue"
                                ConvertEmptyStringToNull="true" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
