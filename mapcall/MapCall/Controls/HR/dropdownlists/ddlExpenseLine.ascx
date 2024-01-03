<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ddlExpenseLine.ascx.cs" Inherits="MapCall.Controls.HR.dropdownlists.ddlExpenseLine" %>
<asp:DropDownList runat="server" ID="ddl_LookupType" 
    DataSourceID="ds_LookupType" 
    AppendDataBoundItems="true"
    DataValueField="LookupID"
    DataTextField="LookupValue"
    >
    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
</asp:DropDownList>                    

<asp:SqlDataSource runat="server" ID="ds_LookupType"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
    SelectCommand="
        select ExpenseLineNumber_ID as LookupId, isNull(cast(ExpenseLineNumber as varchar(10)), '') + ' - ' + isNull(ExpenseLine,'') + ' - ' + isNull(OpCode, '') + ' - ' + isNull(BU, '') + ' - ' + isNull([Object],'') + ' - ' + isNull(Activity,'') as lookupvalue
            from tblAccounting_ExpenseLines order by expenselinenumber, opcode"
    >
</asp:SqlDataSource>