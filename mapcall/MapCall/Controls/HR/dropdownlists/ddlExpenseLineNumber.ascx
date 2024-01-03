<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ddlExpenseLineNumber.ascx.cs" Inherits="MapCall.Controls.HR.dropdownlists.ddlExpenseLineNumber" %>
<asp:DropDownList runat="server" ID="ddl_ExpenseLineNumber" 
    DataSourceID="ds_ExpenseLineNumber1" 
    AppendDataBoundItems="true"
    DataValueField="expenselinenumber"
    DataTextField="LookupValue1"
    >
    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
</asp:DropDownList>                    

<asp:SqlDataSource runat="server" ID="ds_ExpenseLineNumber1"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
    SelectCommand="
        select distinct expenselinenumber, cast(expenselinenumber as varchar(10))+ ' - ' + expenseline as lookupvalue1, len(expenselinenumber) from tblAccounting_ExpenseLines order by len(expenselinenumber), expenselinenumber
	    "
    >
</asp:SqlDataSource>