<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ddlOpCode.ascx.cs" Inherits="MapCall.Controls.HR.dropdownlists.ddlOpCode" %>
<asp:DropDownList runat="server" ID="ddl_OpCode" 
    DataSourceID="ds_OpCode" 
    AppendDataBoundItems="true"
    DataTextField="txt"
    DataValueField="val"
    >
    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
    <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
    <asp:ListItem Text="AM-SSC" Value="AM-SSC"></asp:ListItem>
</asp:DropDownList>                    
<asp:RequiredFieldValidator runat="server" ID="rfvddlOpCntr" Enabled="false" ControlToValidate="ddl_OpCode" SetFocusOnError="true" InitialValue="" Text="Required"></asp:RequiredFieldValidator>
<asp:SqlDataSource runat="server" ID="ds_OpCode"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    SelectCommand="
        select distinct operatingCenterCode + ' - ' + OperatingCenterName as txt, operatingCenterCode as val from [OperatingCenters] where charindex('IL', operatingcentercode) = 0 order by 1
        "
    >
</asp:SqlDataSource> 