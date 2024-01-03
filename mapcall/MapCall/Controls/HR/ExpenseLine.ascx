<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpenseLine.ascx.cs" Inherits="MapCall.Controls.HR.ExpenseLine" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlExpenseLine.ascx" TagName="ddlExpenseLine" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlBudgetCategory.ascx" TagName="ddlBudgetCategory" TagPrefix="mmsi" %>
<%@ Register assembly="MMSINC" namespace="MMSINC" tagPrefix="mmsinc" %>

<asp:DetailsView runat="server" id="DetailsView1" 
    AutoGenerateRows="False" 
    DataKeyNames="BudgetExpenseLineID" 
    DataSourceID="SqlDataSource1"
    OnDataBound="DetailsView1_DataBound"
    OnPreRender="DetailsView1_PreRender"
    Width="100%" FieldHeaderStyle-Width="100px"
>
    <Fields>
        <asp:BoundField DataField="BudgetExpenseLineID" HeaderText="BudgetExpenseLineID" InsertVisible="False" ReadOnly="True" SortExpression="BudgetExpenseLineID" />
        <asp:CheckBoxField DataField="Include" HeaderText="Include" SortExpression="Include" />
        <asp:CheckBoxField DataField="Flag" HeaderText="Flag" SortExpression="Flag" />
        <asp:BoundField ReadOnly="true" InsertVisible="false" DataField="DTM_Entered" HeaderText="DTM_Entered" SortExpression="DTM_Entered" />
        <asp:TemplateField HeaderText="OpCode" SortExpression="OpCode" InsertVisible="false">
            <ItemTemplate><asp:Label runat="server" ID="lblOpCode" Text='<%# Bind("OpCode") %>'></asp:Label></ItemTemplate>
        </asp:TemplateField>
        <mmsinc:BoundField DataField="BudgetYear" HeaderText="BudgetYear" SortExpression="BudgetYear" />
        <asp:TemplateField HeaderText="Budget_Category" SortExpression="Budget_Category">
            <ItemTemplate><asp:Label runat="server" ID="lblBudget_Category" Text='<%# Bind("Budget_Category_Text") %>'></asp:Label></ItemTemplate>
            <InsertItemTemplate><mmsi:ddlBudgetCategory runat="server" ID="ddlBudget_Category" SelectedValue='<%# Bind("Budget_Category") %>' /></InsertItemTemplate>
            <EditItemTemplate><mmsi:ddlBudgetCategory runat="server" ID="ddlBudget_Category" SelectedValue='<%# Bind("Budget_Category") %>' /></EditItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Expense_Line" SortExpression="Expense_Line">
            <ItemTemplate><asp:Label runat="server" ID="lblExpenseLine" Text='<%# Bind("ExpenseLine_Text") %>'></asp:Label></ItemTemplate>
            <InsertItemTemplate>
                <mmsi:ddlExpenseLine runat="server" ID="ddlExpenseLine" SelectedValue='<%# Bind("expenselinenumber_ID") %>' />
                <asp:Button runat="server" id="btnLookup" Text="Create Reforecast Record" OnClick="btnLookup_Click" />
                <asp:Label runat="server" ID="lblExpenseLine" ForeColor="Red"></asp:Label>
            </InsertItemTemplate>
            <EditItemTemplate>
                <mmsi:ddlExpenseLine runat="server" ID="ddlExpenseLine" SelectedValue='<%# Bind("expenselinenumber_ID") %>' />
            </EditItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="ExpenseLineNumber" InsertVisible="false">
            <ItemTemplate><asp:Label runat="server" ID="lblExpenseLineNumber" Text='<%# Bind("ExpenseLineNumber") %>' /></ItemTemplate>
        </asp:TemplateField>
        
        <asp:BoundField DataField="BU" HeaderText="BU" SortExpression="BU" InsertVisible="false" ReadOnly="true" />
        <asp:BoundField DataField="Object" HeaderText="Object" SortExpression="Object" InsertVisible="false" ReadOnly="true" />
        <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity" InsertVisible="false" ReadOnly="true" />
        <asp:BoundField DataField="Task" HeaderText="Task" SortExpression="Task" />
        
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Jan" HeaderText="Jan" SortExpression="Jan" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Feb" HeaderText="Feb" SortExpression="Feb" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Mar" HeaderText="Mar" SortExpression="Mar" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Apr" HeaderText="Apr" SortExpression="Apr" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="May" HeaderText="May" SortExpression="May" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Jun" HeaderText="Jun" SortExpression="Jun" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Jul" HeaderText="Jul" SortExpression="Jul" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Aug" HeaderText="Aug" SortExpression="Aug" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Sep" HeaderText="Sep" SortExpression="Sep" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Oct" HeaderText="Oct" SortExpression="Oct" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Nov" HeaderText="Nov" SortExpression="Nov" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Dec" HeaderText="Dec" SortExpression="Dec" />
        <mmsinc:BoundField SetTableCellId="true" HtmlEncode="false" DataFormatString="{0:c}" DataField="Total" HeaderText="Total" SortExpression="Total" ReadOnly="true" InsertVisible="false" />
        <mmsinc:BoundField TextMode="MultiLine" Rows="3" ControlStyle-Width="98%" DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
        <asp:TemplateField ShowHeader="False">
            <EditItemTemplate>
                <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
            </EditItemTemplate>
            <InsertItemTemplate>
                <asp:LinkButton ID="btnInsert" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert"></asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" Visible="false"></asp:LinkButton>
            </InsertItemTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"></asp:LinkButton>
                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure?');"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>

    </Fields>

</asp:DetailsView>
<br />
<div id="divCharts" runat="server">
    <b>Charts</b>
    <asp:LinkButton runat="server" ID="lbShowCharts" OnClick="lbShowCharts_Click" Text="show"></asp:LinkButton>
    <br />
    <asp:Image Visible="false" runat="server" ID="imgChart" BorderStyle="solid" BorderWidth="1" ImageUrl="~/images/.gif" />
    <asp:Image Visible="false" runat="server" ID="imgPieChart" BorderStyle="solid" BorderWidth="1" ImageUrl="~/images/.gif"  />
</div>

<asp:Label runat="server" ID="lblResults" ForeColor="Green"></asp:Label>

<asp:SqlDataSource runat="server" ID="dsExpenseLine" 
    ConnectionString="<%$ ConnectionStrings:MCProd %>" 
    SelectCommand="
        SELECT 
	        Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec 
        FROM 
	        tblBudget_ExpenseLines_and_Ref 
        WHERE
	        BudgetYear = @BudgetYear
        AND
	        Budget_Category = @BudgetCategory
        AND
	        ExpenseLineNumber_ID = @ExpenseLineNumber
    "
>
    <SelectParameters>
        <asp:Parameter Name="BudgetYear" Type="Int32" />
        <asp:Parameter Name="BudgetCategory" Type="Int32" />
        <asp:Parameter Name="ExpenseLineNumber" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>

    

<asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
    DeleteCommand="DELETE FROM [tblBudget_ExpenseLines_and_Ref] WHERE [BudgetExpenseLineID] = @BudgetExpenseLineID" 
    InsertCommand="
        INSERT INTO [tblBudget_ExpenseLines_and_Ref] 
            ([Include], [Flag], [BudgetYear], [Budget_Category], [ExpenseLineNumber_ID], [Task], [Notes], [Jan], [Feb], [Mar], [Apr], [May], [Jun], [Jul], [Aug], [Sep], [Oct], [Nov], [Dec]) 
        VALUES 
            (@Include, @Flag, @BudgetYear, @Budget_Category, @ExpenseLineNumber_ID, @Task, @Notes, @Jan, @Feb, @Mar, @Apr, @May, @Jun, @Jul, @Aug, @Sep, @Oct, @Nov, @Dec);
        SELECT @BudgetExpenseLineID = @@IDENTITY;" 
    SelectCommand="
        SELECT 
            #1.LookupValue as Budget_Category_Text, #2.ExpenseLine as ExpenseLine_Text, #2.expenselinenumber, 
            #2.OpCode, #2.Bu, #2.Object, #2.Activity, #2.expenselinenumber_ID, 
            [tblBudget_ExpenseLines_and_Ref].*,
            jan+feb+mar+apr+may+jun+jul+aug+sep+oct+nov+dec as Total 
            FROM [tblBudget_ExpenseLines_and_Ref]
            LEFT JOIN Lookup #1 on #1.LookupID = Budget_Category
			LEFT JOIN tblAccounting_ExpenseLines #2 on #2.expenselinenumber_ID = [tblBudget_ExpenseLines_and_Ref].expenselinenumber_ID
            Where BudgetExpenseLineID = @BudgetExpenseLineID" 
    UpdateCommand="
        UPDATE [tblBudget_ExpenseLines_and_Ref] 
            SET [Include] = @Include, [Flag] = @Flag,
                [BudgetYear] = @BudgetYear, [Budget_Category] = @Budget_Category, 
                [ExpenseLineNumber_ID] = @ExpenseLineNumber_ID, 
                [Task] = @Task, 
                [Notes] = @Notes, [Jan] = @Jan, [Feb] = @Feb, [Mar] = @Mar, [Apr] = @Apr, [May] = @May, [Jun] = @Jun, [Jul] = @Jul, [Aug] = @Aug, [Sep] = @Sep, [Oct] = @Oct, [Nov] = @Nov, [Dec] = @Dec WHERE [BudgetExpenseLineID] = @BudgetExpenseLineID"
    OnInserted="SqlDataSource1_Inserted"
    OnDeleted="SqlDataSource1_Deleted"
    OnUpdated="SqlDataSource1_Updated"
    >
    <SelectParameters>
        <asp:Parameter Name="BudgetExpenseLineID" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="BudgetExpenseLineID" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Include" Type="Boolean" />
        <asp:Parameter Name="Flag" Type="Boolean" />
        <asp:Parameter Name="BudgetYear" Type="String" />
        <asp:Parameter Name="Budget_Category" Type="String" />
        <asp:Parameter Name="ExpenseLineNumber_ID" Type="String" />
        <asp:Parameter Name="Task" Type="String" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="Jan" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Feb" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Mar" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Apr" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="May" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Jun" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Jul" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Aug" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Sep" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Oct" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Nov" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Dec" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="BudgetExpenseLineID" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Include" Type="Boolean" />
        <asp:Parameter Name="Flag" Type="Boolean" />
        <asp:Parameter Name="BudgetYear" Type="String" />
        <asp:Parameter Name="Budget_Category" Type="String" />
        <asp:Parameter Name="ExpenseLineNumber_ID" Type="Int32" />
        <asp:Parameter Name="Task" Type="String" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="Jan" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Feb" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Mar" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Apr" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="May" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Jun" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Jul" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Aug" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Sep" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Oct" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Nov" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="Dec" Type="Decimal" DefaultValue="0.0" />
        <asp:Parameter Name="BudgetExpenseLineID" Type="Int32" Direction="Output" />
    </InsertParameters>
</asp:SqlDataSource>
