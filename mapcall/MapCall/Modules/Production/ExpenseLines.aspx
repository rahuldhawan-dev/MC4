<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="ExpenseLines.aspx.cs" Inherits="MapCall.Modules.HR.Accounting.ExpenseLines" Title="Budget Expense Lines and Reforecasts by Year" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/number.ascx" TagName="Number" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlOpCode.ascx" TagName="ddlOpCode" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlExpenseLine.ascx" TagName="ddlExpenseLine" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlExpenseLineNumber.ascx" TagName="ddlExpenseLineNumber" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/ExpenseLine.ascx" TagName="ExpenseLine" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Budget Expense Lines and Reforecasts by Year
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
Manage the Expense Lines
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />

    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <tr>
                <td style="text-align:right;">
                    OpCode : 
                </td>
                <td style="text-align:left;">
                    <mmsi:ddlOpCode runat="server" ID="ddlOpCntr" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Expense Line : 
                </td>
                <td style="text-align:left;">
                    <mmsi:ddlExpenseLine runat="server" id="ddlExpenseLine" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Expense Line Number: 
                </td>
                <td style="text-align:left;">
                    <mmsi:ddlExpenseLineNumber runat="server" id="ddlExpenseLineNumber" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    BU : 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlBusinessUnit"
                        DataSourceID="dsBusinessUnit" 
                        AppendDataBoundItems="true"
                        DataTextField="BU"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsBusinessUnit"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT distinct BU from tblAccounting_ExpenseLines order by 1"
                        >
                    </asp:SqlDataSource> 
                </td>
            </tr>  
            <tr>
                <td style="text-align:right;">
                    Object : 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlObject"
                        DataSourceID="dsObject" 
                        AppendDataBoundItems="true"
                        DataTextField="Object"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsObject"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT distinct Object from tblAccounting_ExpenseLines order by 1"
                        >
                    </asp:SqlDataSource> 
                </td>
            </tr>  
            <tr>
                <td style="text-align:right;">
                    Activity : 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlActivity" 
                        DataSourceID="dsActivity" 
                        AppendDataBoundItems="true"
                        DataTextField="Activity"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsActivity"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="SELECT distinct Activity from tblAccounting_ExpenseLines order by 1"
                        >
                    </asp:SqlDataSource> 
                </td>
            </tr>    
            <tr>
                <td style="text-align:right;">
                    Budget Year : 
                </td>
                <td style="text-align:left;">
                    <mmsi:Number ID="txtBudgetYear" runat="server" SelectedIndex="5" />
                </td>
            </tr>            
            <tr>
                <td style="text-align:right;">
                    Budget Category : 
                </td>
                <td style="text-align:left;">
                    <asp:ListBox Rows="8" SelectionMode="multiple" runat="server" ID="lbBudgetCategory" 
                        DataSourceID="dsBudgetCategory" 
                        AppendDataBoundItems="true"
                        DataValueField="LookupID"
                        DataTextField="LookupValue"
                        > 
                    </asp:ListBox>                    

                    <asp:SqlDataSource runat="server" ID="dsBudgetCategory"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
                        SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'Budget_Category' order by 2"
                        >
                    </asp:SqlDataSource>
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
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:GridView runat="server" ID="GridView1" AutoGenerateColumns="False" DataKeyNames="BudgetExpenseLineID" 
            DataSourceID="SqlDataSource1"
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            OnSorting="GridView1_Sorting"
            OnPageIndexChanged="GridView1_PageIndexChanged"
            AlternatingRowStyle-CssClass="HRAlternatingRow"
            AllowSorting="true"
        >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
                <asp:BoundField DataField="OpCode" HeaderText="OpCode" SortExpression="OpCode" />
                <asp:CheckBoxField DataField="Include" HeaderText="Include" SortExpression="Include" />
                <asp:CheckBoxField DataField="Flag" HeaderText="Flag" SortExpression="Flag" />
                <asp:BoundField DataField="ExpenseLine_text" HeaderText="Expense_Line" SortExpression="ExpenseLine_text" />
                <asp:BoundField DataField="ExpenseLineNumber" HeaderText="ExpenseLineNumber" SortExpression="eln" />
                <asp:BoundField DataField="BU" HeaderText="BU" SortExpression="BU" />
                <asp:BoundField DataField="Object" HeaderText="Object" SortExpression="Object" />
                <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity" />
                <asp:BoundField DataField="Task" HeaderText="Task" SortExpression="Task" />
                <asp:BoundField DataField="BudgetYear" HeaderText="BudgetYear" SortExpression="BudgetYear" />
                <asp:BoundField DataField="Budget_Category_Text" HeaderText="Budget_Category" SortExpression="Budget_Category_Text" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Jan" HeaderText="Jan" SortExpression="Jan" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Feb" HeaderText="Feb" SortExpression="Feb" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Mar" HeaderText="Mar" SortExpression="Mar" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Apr" HeaderText="Apr" SortExpression="Apr" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="May" HeaderText="May" SortExpression="May" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Jun" HeaderText="Jun" SortExpression="Jun" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Jul" HeaderText="Jul" SortExpression="Jul" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Aug" HeaderText="Aug" SortExpression="Aug" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Sep" HeaderText="Sep" SortExpression="Sep" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Oct" HeaderText="Oct" SortExpression="Oct" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Nov" HeaderText="Nov" SortExpression="Nov" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Dec" HeaderText="Dec" SortExpression="Dec" />
                <asp:BoundField HtmlEncode="false" DataFormatString="{0:c}" DataField="Total" HeaderText="Total" SortExpression="Total" />
                <asp:BoundField HtmlEncode="False" DataFormatString="{0:d}" DataField="DTM_Entered" HeaderText="DTM_Entered" SortExpression="DTM_Entered" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:BoundField DataField="NoteCount" HeaderText="Notes" SortExpression="NoteCount" />
                <asp:BoundField DataField="DocumentCount" HeaderText="Documents" SortExpression="DocumentCount" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            SelectCommand="

                SELECT [BudgetExpenseLineID],[Include],[Flag]
                    ,#1.LookupValue as Budget_Category_Text, #2.ExpenseLine as ExpenseLine_Text, #2.expenselinenumber
                    ,#2.OpCode, #2.Bu, #2.Object, #2.Activity, #2.expenselinenumber_ID
                    ,[Task],[BudgetYear], #1.LookupValue as Budget_Category_Text, Budget_Category
                    ,[Jan],[Feb],[Mar],[Apr],[May],[Jun],[Jul],[Aug],[Sep],[Oct],[Nov],[Dec]
                    ,jan+feb+mar+apr+may+jun+jul+aug+sep+oct+nov+dec as Total 
                    ,[Notes]
                    ,[DTM_Entered]
                    , (Select Count(noteID) FROM Note where dataTypeID = 31 and dataLinkID = BudgetExpenseLineID) as [NoteCount]
                    , (Select Count(documentID) FROM DocumentLink where dataTypeID = 31 and dataLinkID = BudgetExpenseLineID) as [DocumentCount]
                    , cast(#2.expenselinenumber as int) as [eln]
                    FROM [tblBudget_ExpenseLines_and_Ref]
										LEFT JOIN Lookup #1 on #1.LookupID = Budget_Category
										LEFT JOIN tblAccounting_ExpenseLines #2 on #2.expenselinenumber_ID = [tblBudget_ExpenseLines_and_Ref].expenselinenumber_ID
            "
            >
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:ExpenseLine runat="server" ID="ExpenseLine1" OnItemInserted="ExpenseLine1_ItemInserted" OnDataBinding="ExpenseLine1_DataBinding" />
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="31" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="31" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

</asp:Content>
