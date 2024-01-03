<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="PositionPostings.aspx.cs" Inherits="MapCall.Modules.HR.Employee.PositionPostings" Title="Position Postings" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlPositions.ascx" TagName="ddlPositions" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlEmployees.ascx" TagName="ddlEmployees" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlOpCode.ascx" TagName="ddlOpCode" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlLocal.ascx" TagName="ddlLocal" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/employees.ascx" TagName="Employees" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/date.ascx" TagName="date" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/PositionPosting.ascx" TagName="PositionPosting" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Position Postings 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Use these forms to search, view, and edit the position postings.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <tr>
                <td style="text-align:right;">
                    Position : 
                </td>
                <td style="text-align:left;">
                    <mmsi:ddlPositions ID="ddlPositions" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Position Posting ID : 
                </td>
                <td style="text-align:left;">
                    <asp:TextBox runat="server" ID="txtPosition_Posting_ID"></asp:TextBox>                   
                    <asp:CompareValidator runat="server" ID="cvPostingID" 
                        ControlToValidate="txtPosition_Posting_ID"  
                        Type="Integer" Display="Dynamic"
                        ErrorMessage="Please enter an integer."
                        Operator="DataTypeCheck"
                        />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Position Control Number : 
                </td>
                <td style="text-align:left;">
                    <asp:TextBox runat="server" ID="Position_Control_Number"></asp:TextBox>                   
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Date of Posting:
                </td>
                <td>
                    <uc2:date runat="server" ID="txtDate_of_Posting" SelectedIndex="5" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    OpCode : 
                </td>
                <td style="text-align:left;">
                    <mmsi:ddlOpCode runat="server" id="ddlOpCode"></mmsi:ddlOpCode>
                </td>                
            </tr>
            <tr>
                <td style="text-align:right;">
                    Local Bargaining Unit : 
                </td>
                <td style="text-align:left;">
                    <mmsi:ddlLocal runat="server" id="ddlLocal" />
                </td>                
            </tr>
            <tr>
                <td style="text-align:right;">
                    Internal Posting:
                </td>
                <td>
                    <asp:CheckBox ID="chkInternal_Posting" runat="server" Checked='<%# Bind("Internal_Posting") %>' />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    External Recruitment:
                </td>
                <td>
                    <asp:CheckBox ID="chkExternal_Recruitment" runat="server" Checked='<%# Bind("External_Recruitment") %>' />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Interviewing:
                </td>
                <td>
                    <asp:CheckBox ID="chkInterviewing" runat="server" Checked='<%# Bind("Interviewing") %>' />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Candidate_Selected
                </td>
                <td>
                    <asp:CheckBox ID="chkCandidate_Selected" runat="server" Checked='<%# Bind("Candidate_Selected") %>' />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Effective Date:
                </td>
                <td>
                    <uc2:date runat="server" ID="txtEffective_Date" SelectedIndex="5" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Position Awarded To : 
                </td>
                <td style="text-align:left;">
                    <mmsi:ddlEmployees runat="server" id="ddlEmployees" />
                </td>
            </tr>
<%--            <tr>
                <td style="text-align:right;">
                    No Qualified Bidders : 
                </td>
                <td style="text-align:left;">
                    <asp:TextBox runat="server" ID="txtNo_Qualified_Bidders"></asp:TextBox>                   
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    No Bidders : 
                </td>
                <td style="text-align:left;">
                    <asp:TextBox runat="server" ID="txtNo_Bidders"></asp:TextBox>                   
                </td>
            </tr>--%>
            <tr>
                <td style="text-align:right;">
                    Order By: 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlOrderBy">
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Position" Value="Position"></asp:ListItem>
                        <asp:ListItem Text="Date Of Posting" Value="Date_Of_Posting"></asp:ListItem>
                        <asp:ListItem Text="Position Awarded To" Value="empName"></asp:ListItem>
                        <asp:ListItem Text="Effective Date" Value="Effective_Date"></asp:ListItem>
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
            AutoGenerateColumns="False" DataKeyNames="Position_Posting_ID" DataSourceID="SqlDataSource1"
            EmptyDataText="There are no data records to display."
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            OnSorting="GridView1_Sorting"
            OnPageIndexChanged="GridView1_PageIndexChanged"
            AlternatingRowStyle-CssClass="HRAlternatingRow"
            Width="100%"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" ControlStyle-CssClass="RightPadded" />
                <asp:BoundField DataField="Position_Posting_ID" HeaderText="ID" ReadOnly="True" SortExpression="Position_Posting_ID" />
                <asp:BoundField DataField="opCode" HeaderText="OpCode" SortExpression="opCode" />
                <asp:BoundField DataField="Local" HeaderText="Local" SortExpression="Local" />
                <asp:BoundField DataField="PositionText" HeaderText="Position" SortExpression="PositionText" />
                <asp:BoundField DataFormatString="{0:d}" HtmlEncode="False" DataField="Date_of_Posting" HeaderText="Date of Posting" SortExpression="Date_of_Posting" />
                <asp:CheckBoxField DataField="Internal_Posting" HeaderText="Internal Posting" SortExpression="Internal_Posting" />
                <asp:CheckBoxField DataField="External_Recruitment" HeaderText="External Recruitment" SortExpression="External_Recruitment" />
                <asp:CheckBoxField DataField="Interviewing" HeaderText="Interviewing" SortExpression="Interviewing" />
                <asp:CheckBoxField DataField="Candidate_Selected" HeaderText="Candidate Selected" SortExpression="Candidate_Selected" />
                <asp:BoundField DataFormatString="{0:d}" HtmlEncode="False" DataField="Effective_Date" HeaderText="Effective Date" SortExpression="Effective_Date" />
                <asp:BoundField DataField="empname" HeaderText="Position Awarded To" SortExpression="empname" />
                <asp:CheckBoxField DataField="No_Qualified_Bidders" HeaderText="No Qualified Bidders" SortExpression="No_Qualified_Bidders" />
                <asp:CheckBoxField DataField="No_Bidders" HeaderText="No Bidders" SortExpression="No_Bidders" />
                <asp:BoundField DataField="NoteCount" HeaderText="Notes" SortExpression="NoteCount" />
                <asp:BoundField DataField="DocumentCount" HeaderText="Documents" SortExpression="DocumentCount" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:PositionPosting runat="server" ID="PositionPosting1" OnDataBinding="PositionPostings1_DataBinding" OnItemInserted="PositionPostings1_ItemInserted" />
        <mmsi:Employees runat="server" ID="Employee1" DataTypeID="29"  />
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="29" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="29" />
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
            SELECT 
					replace(isNull(First_Name, '') + ' ' +  isNull(Middle_Name,'') + ' ' + isNull(Last_Name,''), '  ', ' ') as [fullname],
					replace(isNull(Last_Name, '') + ', ' +  isNull(First_Name,'') + ' ' + isNull(Middle_Name,''), '  ', ' ') as [empname],
					tblPosition_Postings.tblEmployeeID, Position_Control_Number, 
					[Position_Posting_ID], [Position_ID], [tblPositions_classifications].[Position], [Date_of_Posting], 
					[Effective_Date], [Notes], [No_Qualified_Bidders], [No_Bidders], isNull(OperatingCenters.operatingCenterCode,'') + '-' + isNull(tblPositions_Classifications.position, '') as [PositionText]
					,LocalBargainingUnits.Name as Local, LocalBargainingUnits.Id as LocalId, operatingCenterCode as opcode
					,Internal_Posting, External_Recruitment, Interviewing, Candidate_Selected
                    , (Select Count(noteID) FROM Note where dataTypeID = 29 and dataLinkID = Position_Posting_ID) as [NoteCount]
                    , (Select Count(documentID) FROM DocumentLink where dataTypeID = 29 and dataLinkID = Position_Posting_ID) as [DocumentCount]
                FROM [tblPosition_Postings] 
                LEFT JOIN [tblPositions_classifications] on [tblPositions_classifications].positionID = [tblPosition_Postings].position_id
                LEFT JOIN [LocalBargainingUnits] on tblPositions_classifications.LocalId = [LocalBargainingUnits].Id
				LEFT JOIN [OperatingCenters] on [LocalBargainingUnits].OperatingCenterId = OperatingCenters.OperatingCenterID
				LEFT JOIN tblEmployee on tblEmployee.tblEmployeeID = [tblPosition_Postings].tblEmployeeID">
    </asp:SqlDataSource>

</asp:Content>
