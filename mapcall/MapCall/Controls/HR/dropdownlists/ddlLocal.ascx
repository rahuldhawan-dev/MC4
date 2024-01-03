<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ddlLocal.ascx.cs" Inherits="MapCall.Controls.HR.dropdownlists.ddlLocal" %>
<asp:DropDownList runat="server" ID="ddl_Local" 
    DataSourceID="ds_Local" 
    AppendDataBoundItems="true"
    DataTextField="Name"
    DataValueField="Id"
    >
    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
</asp:DropDownList>                    

<asp:SqlDataSource runat="server" ID="ds_Local"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    SelectCommand="SELECT * from LocalBargainingUnits order by 1"
    >
</asp:SqlDataSource>