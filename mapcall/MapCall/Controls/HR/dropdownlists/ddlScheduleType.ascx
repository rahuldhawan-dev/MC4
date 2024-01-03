<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ddlScheduleType.ascx.cs" Inherits="MapCall.Controls.HR.dropdownlists.ddlScheduleType" %>
<asp:DropDownList runat="server" ID="ddl_ScheduleType" 
    DataSourceID="ds_ScheduleType" 
    AppendDataBoundItems="true"
    DataValueField="ScheduleTypeID"
    DataTextField="ScheduleType"
    >
    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
</asp:DropDownList>                    

<asp:SqlDataSource runat="server" ID="ds_ScheduleType"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
    SelectCommand="
          SELECT ScheduleTypeID, ScheduleType from ScheduleType            
        "
    >
</asp:SqlDataSource>