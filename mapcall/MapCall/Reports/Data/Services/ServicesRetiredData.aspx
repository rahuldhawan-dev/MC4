<%@ Page Title="ServicesRetiredData" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ServicesRetiredData.aspx.cs" Inherits="MapCall.Reports.Data.Services.ServicesRetiredData" EnableEventValidation="false" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/OpCntrDataField.ascx" TagName="OpCntrDataField" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadTagScripts" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeadTag" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Services Retired Data
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
   <asp:Panel runat="server" ID="pnlSearch" CssClass="searchPanel">
        <table>
            
            <mmsi:OpCntrDataField runat="server"
                DataFieldName="OpCntrID"
                TownDataFieldName="TownID"
                UseTowns="true" />
                
            <mmsi:DataField runat="server"
                HeaderText="Year Retired : " DataType="DropDownList"
                DataFieldName="Year"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct Year(RetireDate) as txt, Year(RetireDate) as val from tblNJAWService order by 1 desc" />
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
                        AllowSorting="true" DataSourceID="SqlDataSource1">
                        <Columns>
                           <mmsinc:BoundField DataField="OpCntr" HeaderText="OpCntr" />
                           <mmsinc:BoundField DataField="Town" HeaderText="Town" />
                           <mmsinc:BoundField DataField="Year" HeaderText="Year" />
                           <mmsinc:BoundField DataField="CatOfService" HeaderText="CatOfService" />
                           <mmsinc:BoundField DataField="PrevServiceMatl" HeaderText="PrevServiceMatl" />
                           <mmsinc:BoundField DataField="PrevServiceSize" HeaderText="PrevServiceSize" />
                           <mmsinc:BoundField DataField="Total" HeaderText="Total" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="
                            SELECT
	                            OC.OperatingCenterCode as OpCntr,
	                            OC.OperatingCenterID as OpCntrID, 
	                            t.Town, t.TownID as TownID, 
	                            Year(RetireDate) as Year,
	                            SC.Description as CatOfService,
	                            SM.Description as PrevServiceMatl,
	                            SS.SizeServ as PrevServiceSize,
	                            Count(1)as Total
                            FROM 
	                            tblNJAWService S
                            LEFT JOIN Towns T ON T.TownID = S.Town 
                            LEFT JOIN OperatingCenters OC ON OC.OperatingCenterID = S.OpCntr
                            LEFT JOIN ServiceCategories SC ON SC.ServiceCategoryID = S.CatofService
                            LEFT JOIN ServiceMaterials SM ON SM.ServiceMaterialID = S.PrevServiceMatl
                            LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = S.PrevServiceSize                            
                            GROUP BY
	                            OC.OperatingCenterCode, 
	                            OC.OperatingCenterID,
	                            T.Town, t.TownID,
	                            Year(RetireDate),
	                            SC.Description,
	                            SM.Description,
	                            SS.SizeServ
                            ORDER BY 
	                            OC.OperatingCenterCode,
	                            T.Town,
	                            Year(RetireDate),
	                            SC.Description,
	                            SM.Description,
	                            SS.SizeServ
                        ">
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>


