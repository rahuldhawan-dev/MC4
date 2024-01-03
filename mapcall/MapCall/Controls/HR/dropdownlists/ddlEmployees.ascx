<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ddlEmployees.ascx.cs" Inherits="MapCall.Controls.HR.dropdownlists.ddlEmployees" %>
<asp:DropDownList runat="server" ID="ddl_Employees" 
    DataSourceID="ds_Employees" 
    AppendDataBoundItems="true"
    DataValueField="tblEmployeeID"
    DataTextField="fullname"
    >
    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
</asp:DropDownList>                    

<asp:SqlDataSource runat="server" ID="ds_Employees"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
    SelectCommand="
        --SELECT distinct tblEmployeeID, replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') as [FullName], Last_Name from tblEmployee order by Last_Name
        SELECT 
            distinct tblEmployeeID, FullName, Last_Name
        FROM 
            Employees
        ORDER by Last_Name
    "
    >
</asp:SqlDataSource>

<asp:SqlDataSource runat="server" ID="dsActiveEmployees"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
    SelectCommand="
        --SELECT distinct tblEmployeeID, replace(isNull(Last_Name,'') + ', ' + isNull(First_name, '') + ' ' + isNull(Middle_Name,'') + ' - '  + isNull(employeeID,''),'  ', ' ') as [FullName], Last_Name from tblEmployee where Status = 'Active' order by Last_Name
        SELECT 
            distinct tblEmployeeID, FullName, Last_Name
        FROM 
            ActiveEmployees
        ORDER by Last_Name
    "
    >
</asp:SqlDataSource>
