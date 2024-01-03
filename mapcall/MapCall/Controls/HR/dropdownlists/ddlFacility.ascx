<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ddlFacility.ascx.cs" Inherits="MapCall.Controls.HR.dropdownlists.ddlFacility" %>

<asp:DropDownList runat="server" ID="ddl_Facility"
    DataTextField="facilityName"
    DataValueField="recordId"
    DataSourceID="ds_Facilities"
    AppendDataBoundItems="true"
>                              
    <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>  
</asp:DropDownList>
<asp:SqlDataSource runat="server" ID="ds_Facilities"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    SelectCommand="SELECT recordId, '[' + oc.OperatingCenterCode + '-' + cast(recordId as varchar) + '] - ' + isNull(facilityName,'') as facilityName from tblFacilities F left join OperatingCenters oc on oc.OperatingCenterID = F.operatingCenterId order by FacilityName"
>
</asp:SqlDataSource>