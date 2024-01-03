<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="SampleResultsComplaints.aspx.cs" Inherits="MapCall.Modules.WaterQuality.SampleResultsComplaints" Title="Water Complaint Sample Results" Theme="bender" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Water Complaint Sample Results
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
<%--            <tr>
                <td style="text-align:right;">
                    OpCode : 
                </td>
                <td style="text-align:left;">
                    <mmsi:ddlOpCode runat="server" ID="ddlOpCntr" />
                </td>
            </tr>--%>
            
            <mmsi:DataField runat="server" ID="dfSampleDate" HeaderText="Sample_Date" DataType="Date" DataFieldName="Sample_Date"  ShowTime="true" />
            <mmsi:DataField runat="server" ID="dfWaterConstituent" 
                HeaderText="WaterConstituent" 
                DataType="DropDownList" 
                DataFieldName="WaterConstituent_ID" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select Id as val, WaterConstituent as txt from WaterConstituents order by 2"
            />
            <mmsi:DataField runat="server" ID="dfSample_Value" HeaderText="Sample_Value" DataType="NumberRange" DataFieldName="Sample_Value" />
            
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
        <asp:Button runat="server" ID="btnMap" Visible="false" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="SampleID"
            AllowSorting="True" AllowPaging="True" PageSize="20"
            AutoGenerateColumns="false"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
                <asp:BoundField DataField="SampleID" HeaderText="SampleID" InsertVisible="False" ReadOnly="True" SortExpression="SampleID" />
                <asp:BoundField DataField="WQ_Complaint_Number" HeaderText="WQ_Complaint_Number" SortExpression="WQ_Complaint_Number" />
                <asp:BoundField DataField="WaterConstituent" HeaderText="WaterConstituent" SortExpression="WaterConstituent" />
                <asp:BoundField DataField="Sample_Date" HtmlEncode="false" DataFormatString="{0:d}" HeaderText="Sample_Date" SortExpression="Sample_Date" />
                <asp:BoundField DataField="Sample_Value" HeaderText="Sample_Value" SortExpression="Sample_Value" />
                <asp:BoundField DataField="UnitOfMeasure" HeaderText="UnitOfMeasure" SortExpression="UnitOfMeasure" />
                <asp:BoundField DataField="Analysis_Performed_By" HeaderText="Analysis_Performed_By" SortExpression="Analysis_Performed_By" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="
            SELECT [SampleID]
                  ,[WQ_Complaint_Number]
                  ,(Select WaterConstituent from WaterConstituents where WaterConstituents.Id = [tblWQSample_Results_Complaints].[WaterConstituent_ID]) as [WaterConstituent]
                  ,[Sample_Date]
                  ,[Sample_Value]
                  ,UnitOfMeasure
                  ,[Analysis_Performed_By]
                  ,[WaterConstituent_ID]
	            FROM [tblWQSample_Results_Complaints]
            ">
        </asp:SqlDataSource>
    </asp:Panel>
        
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" OnItemInserted="DataElement1_ItemInserted" OnPreInit="DataElement1_PreInit"
            DataElementName = "Water Complaint Sample Results"
            DataElementParameterName = "SampleID"
            DataElementTableName = "tblWQSample_Results_Complaints"
        />
        
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="43" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="43" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>


</asp:Content>
