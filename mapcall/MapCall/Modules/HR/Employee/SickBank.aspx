    <%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="SickBank.aspx.cs" Inherits="MapCall.Controls.HR.SickBank" Title="Sick Bank" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Sick Bank
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfOpCode"
                HeaderText="OpCode : "
                DataType="DropDownList"
                DataFieldName="OperatingCenterId"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select OperatingCenterId as val,operatingCenterCode as txt from OperatingCenters order by 1" />
            <mmsi:DataField runat="server" ID="DataField1" 
                HeaderText="Calendar Year : " 
                DataType="DropDownList" 
                DataFieldName="Calendar_Year" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select distinct [LookupID] AS [Val], [LookupValue] AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Calendar_Year' order by 2" />
            <mmsi:DataField runat="server" ID="dfEmployee"
                DataType="DropDownList" DataFieldName="EmployeeID"
                HeaderText="Employee : " 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT
	                    [tblEmployeeID] AS [val],
	                    [Last_Name] + isNull(', ' + [First_Name] + isNull(' ' + [Middle_Name], ''), '') as [txt]
                    FROM
	                    tblEmployee
                    WHERE
	                    [Last_Name] IS NOT NULL
                    ORDER BY
	                    [Last_Name], isNull([First_Name], '')"/>
            <mmsi:DataField runat="server" ID="DataField3" DataType="Date" DataFieldName="Date_Updated" HeaderText="Date Updated : " />
            <mmsi:DataField runat="server" ID="DataField4" 
                HeaderText="Entry Type : " 
                DataType="DropDownList" 
                DataFieldName="Entry_Type" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="SELECT LookupID AS [Val], LookupValue AS [Txt] FROM [Lookup] WHERE [LookupType] = 'Entry_Type'"
            />
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                    <asp:Button runat="server" ID="btnAdd" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
        </center>
        <br />
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnMap" Visible="False" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="Sick_Bank_ID"
            AllowSorting="true" AllowPaging="true" PageSize="500">
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="
            SELECT
                e.[tblEmployeeID] as [EmployeeID],
                --(Select opCode from [tblPositions_Classifications] where PositionID = (select top 1 Position_ID from [tblPosition_History] where tblEmployeeID = e.[tblEmployeeID] order by Position_History_ID Desc)) as OpCode,                
				--operatingcenterCode as OpCode,
				oc.OperatingCenterCode as OpCode,
                e.OperatingCenterId,
                e.Last_Name,
                e.First_Name,
                l2.LookupValue AS [Type],
                e.EmployeeID as [AW_Employee_ID],
                l1.LookupValue AS [Year],
                cast(esb.Hours as decimal(20,2)) as Hours,
                esb.Notes,
                esb.Date AS [Date_Updated],
                esb.Sick_Bank_Id,
                esb.Calendar_Year,
                esb.Entry_Type
            FROM
                tblEmployee_Sick_Bank AS esb
            INNER JOIN
                tblEmployee AS e
            ON
                esb.EmployeeID = e.tblEmployeeID
            INNER JOIN
                Lookup AS l1
            ON
                l1.LookupID = esb.Calendar_Year
            INNER JOIN
                Lookup AS l2
            ON
                l2.LookupID = esb.Entry_Type
            LEFT JOIN
                OperatingCenters as oc on oc.OperatingCenterId = e.OperatingCenterId
        " />
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" 
            OnDataBinding="DataElement1_DataBinding"
            DataElementName = "SickBank"
            DataElementParameterName = "Sick_Bank_ID"
            DataElementTableName = "tblEmployee_Sick_Bank"
            AllowDelete="true"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="55" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="55" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    
</asp:Content>