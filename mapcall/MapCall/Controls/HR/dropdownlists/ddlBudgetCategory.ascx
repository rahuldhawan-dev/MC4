<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ddlBudgetCategory.ascx.cs" Inherits="MapCall.Controls.HR.dropdownlists.ddlBudgetCategory" %>
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
    SelectCommand="Select LookupID, LookupValue from Lookup where LookupType = 'Budget_Category' order by 2"
    >
</asp:SqlDataSource>
