<%@ Page Title="Union Affliation" theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="UnionAffiliation.aspx.cs" Inherits="MapCall.Reports.HR.UnionAffiliation" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Union Affiliation
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
SELECT 
		LocalBargainingUnits.Name as Local,
		Last_Name, 
		oc.OperatingCenterCode as OpCode
	FROM tblEmployee
	LEFT JOIN tblPosition_History on tblPosition_History.tblemployeeID = tblEmployee.tblemployeeID
	LEFT JOIN scheduletype ON scheduletype.scheduletypeID = tblPosition_History.scheduletypeID
	LEFT JOIN tblPositions_Classifications on tblPositions_Classifications.PositionID = tblPosition_History.Position_ID
	LEFT JOIN LocalBargainingUnits on LocalBargainingUnits.Id = tblPositions_Classifications.LocalID
	LEFT JOIN OperatingCenters OC on oc.OperatingCenterID = tblEmployee.OperatingCenterId
	where tblPosition_History.Position_End_Date is null 
	Order by Seniority_Ranking

                ">
        </asp:SqlDataSource>

    </asp:Panel>

</asp:Content>
