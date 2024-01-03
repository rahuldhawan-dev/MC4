<%@ Page Title="BPU Report for Services" Theme="bender" Language="C#" MasterPageFile="~/MapCallHIB.Master" AutoEventWireup="true" CodeBehind="BPUServicesReport.aspx.cs" Inherits="MapCall.Reports.FieldServices.BPUServicesReport" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagName="OpCntrDataField" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    BPU Report for Services
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch" CssClass="searchPanel">
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
                            select distinct year(DateInstalled) as txt, year(DateInstalled) as val from tblNJAWService order by 1 desc
                        " />
                
                </td>
            </tr>
            
            <mmsi:OpCntrDataField runat="server" DataFieldName="OpCntr" />
            
            <tr>
                <td class="label"></td>
                <td class="field">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>
        </center>
        <br />
    </asp:Panel>

   <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <table>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
                    <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
                    <asp:Label runat="server" ID="lblInformation"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="false" 
                        Font-Size="Larger" HeaderStyle-HorizontalAlign="Center">
                        <Columns>
                            <mmsinc:BoundField DataField="OperatingCenterCode" HeaderText="OpCntr" />
                            <mmsinc:BoundField DataField="Year" HeaderText="Year" />
                            <mmsinc:BoundField DataField="Installed Size" HeaderText="Installed Size" />
                            <mmsinc:BoundField DataField="Installed Type" HeaderText="Installed Type" />
                            <mmsinc:BoundField DataField="# Installed New(Smart Growth)" HeaderText="# Installed New(Smart Growth)" />
                            <mmsinc:BoundField DataField="# Installed New(Non-Smart Growth)" HeaderText="# Installed New(Non-Smart Growth)" />
                            <mmsinc:BoundField DataField="# Replaced" HeaderText="# Replaced" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        ProviderName="System.Data.SqlClient"
                        CancelSelectOnNullParameter="false"
                        SelectCommand="
                            select Distinct 
	                            s.OpCntr,
                                oc.OperatingCenterCode,
	                            Year(DateInstalled) as [Year], 
	                            (Select Town from Towns where TownID=S.Town) as [Town],
	                            SS.SizeServ as [Installed Size], 
	                            SM.Description as [Installed Type],
	                            (Select count(distinct #serv.recID) from tblNJAWService #serv LEFT JOIN tblNJAWCategoryService cs ON cs.CatOfService = #serv.CatOfService
		                            where #serv.ServMatl = S.ServMatl and #serv.SizeOfService=S.SizeOfService and #serv.Town=S.Town 
		                            and year(#serv.DateInstalled) = @year 
		                            and isNull(cs.CategoryOfServiceGroupID,0) = (SELECT CategoryOfServiceGroupID FROM CategoryOfServiceGroups where Description = 'New')
		                            and #serv.SmartGrowth=1) as [# Installed New(Smart Growth)], 
	                            (Select count(distinct #serv.recID) from tblNJAWService #serv LEFT JOIN tblNJAWCategoryService cs ON cs.CatOfService = #serv.CatOfService
		                            where #serv.ServMatl = S.ServMatl and #serv.SizeOfService=S.SizeOfService and #serv.Town=S.Town 
		                            and year(#serv.DateInstalled) = @year 
		                            and isNull(cs.CategoryOfServiceGroupID,0) = (SELECT CategoryOfServiceGroupID FROM CategoryOfServiceGroups where Description = 'New')
		                            and isNull(#serv.SmartGrowth,0) &lt;&gt; 1
	                            ) as [# Installed New(Non-Smart Growth)], 
	                            (Select count(distinct #serv.recID) from tblNJAWService #serv LEFT JOIN tblNJAWCategoryService cs ON cs.CatOfService = #serv.CatOfService
		                            where #serv.ServMatl = S.ServMatl and #serv.SizeOfService=S.SizeOfService and #serv.Town=S.Town 
		                            and year(#serv.DateInstalled) = @year 
		                            and isNull(cs.CategoryOfServiceGroupID,0) = (SELECT CategoryOfServiceGroupID FROM CategoryOfServiceGroups where Description = 'Renewal')
	                            ) as [# Replaced]
                            from 
	                            tblNJAWService S
                            LEFT JOIN 
                                OperatingCenters oc 
                            ON 
                                oc.OperatingCenterID = S.OpCntr
                            LEFT JOIN 
	                            tblNJAWCategoryService cs 
                            ON 
	                            cs.CatOfService = S.CatOfService
                            LEFT JOIN
                                tblNJAWSizeServ SS
                            ON
                                SS.RecID = S.SizeOfService
                            LEFT JOIN
                                ServiceMaterials SM
                            ON
                                SM.ServiceMaterialID = S.ServMatl
                            where 
	                            year(S.DateInstalled) = @year
                            and
	                            isNull(cs.CategoryOfServiceGroupID,0) in (Select CategoryOfServiceGroupID FROM CategoryOfServiceGroups)
                            order by 
	                            1, 2, 3, 4, 5
                            ">
                            <SelectParameters>
                                <asp:ControlParameter Name="year" ControlID="ddlYear" ConvertEmptyStringToNull="true" />
                            </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
