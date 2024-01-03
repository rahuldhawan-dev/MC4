<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="Positions.aspx.cs" Inherits="MapCall.Modules.HR.Employee.Positions" Title="Positions" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/Position.ascx" TagName="Position" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlOpCode.ascx" TagName="ddlOpCode" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlLocal.ascx" TagName="ddlLocal" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Positions
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
Use these forms to search, view, and edit the positions.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <tr>
                <td style="text-align:right;">
                    OpCode:
                </td>
                <td>
                    <mmsi:ddlOpCode runat="server" ID="ddlOpCode" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Position : 
                </td>
                <td style="text-align:left;">
                    <asp:TextBox runat="server" ID="txtPosition"></asp:TextBox>                   
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Position Category:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCategory" 
                        DataSourceID="dsCategory" 
                        AppendDataBoundItems="true"
                        DataTextField="Category"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsCategory"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT distinct Category from tblPositions_Classifications order by 1"
                        >
                    </asp:SqlDataSource>  
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Local:
                </td>
                <td>
                    <mmsi:ddlLocal runat="server" ID="ddlLocal" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    EEO Job Code:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlEEO_Job_Code" 
                        DataSourceID="dsEEO_Job_Code" 
                        AppendDataBoundItems="true"
                        DataTextField="EEO_Job_Code"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsEEO_Job_Code"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT distinct EEO_Job_Code from tblPositions_Classifications order by 1"
                        >
                    </asp:SqlDataSource>  
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    EEO Job Description:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlEEO_Job_Description" 
                        DataSourceID="dsEEO_Job_Description" 
                        AppendDataBoundItems="true"
                        DataTextField="EEO_Job_Description"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsEEO_Job_Description"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT distinct EEO_Job_Description from tblPositions_Classifications order by 1"
                        >
                    </asp:SqlDataSource>  
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    FLSA Status:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFLSAStatus"
                        DataSourceID="dsFLSAStatus"
                        DataTextField="LookupValue"
                        DataValueField="LookupID"
                        AppendDataBoundItems="true"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="dsFLSAStatus"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT LookupId, LookupValue from Lookup where LookupType='FLSAstatus' order by 2"
                    />                
                </td>
            </tr>

            <tr>
                <td style="text-align:right;">
                    Department:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDepartment" 
                        DataSourceID="dsDepartment" 
                        AppendDataBoundItems="true"
                        DataTextField="Department"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsDepartment"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT distinct Department from tblPositions_Classifications order by 1"
                        >
                    </asp:SqlDataSource>  
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Common Name:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCommon_Name" 
                        DataSourceID="dsCommon_Name" 
                        AppendDataBoundItems="true"
                        DataValueField="JobTitleCommonNameID"
                        DataTextField="Description"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsCommon_Name"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT JobTitleCommonNameID, Description from JobTitleCommonNames order by 2"
                        >
                    </asp:SqlDataSource>  
                </td>
            </tr>
            <mmsi:DataField runat="server" ID="dfEssentialFunction" HeaderText="Essential Position : " 
                DataType="BooleanDropDown" DataFieldName="EssentialPosition" />
                
            <mmsi:DataField runat="server" ID="dfKPIStatus"
                HeaderText="Emergency Response Priority : "
                DataType="DropDownList"
                DataFieldName="EmergencyResponsePriority"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select lookupID as val, lookupValue as txt from Lookup where lookuptype = 'EmergencyResponsePriority'"
            />
            <tr>
                <td style="text-align:right;">
                    Order By: 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlOrderBy">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                        <asp:ListItem Text="OpCode" Value="OpCode"></asp:ListItem>
                        <asp:ListItem Text="Position" Value="Position"></asp:ListItem>
                        <asp:ListItem Text="Category" Value="Category"></asp:ListItem>
                        <asp:ListItem Text="EEO Job Code" Value="EEO_Job_Code"></asp:ListItem>
                        <asp:ListItem Text="EEO Job Description" Value="EEO_Job_Description"></asp:ListItem>
                        <asp:ListItem Text="Department" Value="Department"></asp:ListItem>
                        <asp:ListItem Text="Common Name" Value="Common_Name"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
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
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="False" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="PositionID" DataSourceID="SqlDataSource1"
            EmptyDataText="There are no data records to display."
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            OnSorting="GridView1_Sorting"
            OnPageIndexChanged="GridView1_PageIndexChanged"
            AlternatingRowStyle-CssClass="HRAlternatingRow"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" ControlStyle-CssClass="RightPadded" />
                <asp:BoundField DataField="PositionID" HeaderText="ID" ReadOnly="True" SortExpression="PositionID" />
                <asp:BoundField DataField="Position" HeaderText="Position" SortExpression="Position" />
                <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                <asp:BoundField DataField="Local" HeaderText="Local" SortExpression="Local" />
                <asp:BoundField DataField="OpCode" HeaderText="OpCode" SortExpression="OpCode" />
                <asp:BoundField DataField="EEO_Job_Code" HeaderText="EEO_Job_Code" SortExpression="EEO_Job_Code" />
                <asp:BoundField DataField="EEO_Job_Description" HeaderText="EEO_Job_Description" SortExpression="EEO_Job_Description" />
                <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                <asp:BoundField DataField="JobTitleCommonName" HeaderText="Common Name" SortExpression="JobTitleCommonName" />
                <asp:CheckBoxField HeaderText="Essential Position" DataField="EssentialPosition" SortExpression="EssentialPosition" />
                <asp:BoundField HeaderText="Emergency Response Priority" DataField="EmergencyResponsePriorityText" SortExpression="EmergencyResponsePriorityText" />                
                <asp:BoundField DataField="NoteCount" HeaderText="Notes" SortExpression="NoteCount" />
                <asp:BoundField DataField="DocumentCount" HeaderText="Documents" SortExpression="DocumentCount" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:Position runat="server" ID="Position1" OnPreRender="Position1_DataBinding"
            OnItemInserted="Position1_ItemInserted" />
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="27" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="27" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>" 
        SelectCommand="
                select 
                    OperatingCenters.OperatingCenterCode as [OpCode], --Ambiguous putting this first makes this one show, not the one in the main table.
                    tblPositions_Classifications.*,
                    LocalBargainingUnits.Name as Local,
                    LocalBargainingUnits.*,
                    (Select Count(noteID) FROM Note where dataTypeID = 27 and dataLinkID = PositionID) as [NoteCount],
                    (Select Count(documentID) FROM DocumentLink where dataTypeID = 27 and dataLinkID = PositionID) as [DocumentCount],
                    l.lookupvalue as FLSAStatusText,
                    (Select LookupValue from Lookup where LookupID = EmergencyResponsePriority) as EmergencyResponsePriorityText,
                    JT.Description as JobTitleCommonName
                  from tblPositions_Classifications
            	  left join LocalBargainingUnits on LocalBargainingUnits.Id = tblPositions_Classifications.LocalID
            	  LEFT JOIN OperatingCenters on LocalBargainingUnits.OperatingCenterId = OperatingCenters.OperatingCenterID
            	  left join lookup l on l.lookupid = tblPositions_Classifications.FLSAstatus
                  LEFT JOIN JobTitleCommonNames JT ON JT.JobTitleCommonNameID = Common_Name" >
    </asp:SqlDataSource>

</asp:Content>
