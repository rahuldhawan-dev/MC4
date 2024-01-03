<%@ Page Language="C#" MasterPageFile="~/MapCall.Master" Theme="bender"AutoEventWireup="true" CodeBehind="RptServicesRenewedByYear.aspx.cs" Inherits="MapCall.Reports.Data.RptServicesRenewedByYear" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="RptTable" border="0" cellpadding="3" cellspacing="0">
        <tr>
            <td colspan="4" class="RptHeader">
                Services Renewals
            </td>
        </tr>
        <tr>
            <td nowrap style="vertical-align:bottom;">
                Start Year
                <asp:RangeValidator runat="server" ID="rvStart" ControlToValidate="txtstartYear" Type="Integer" MinimumValue="1901" MaximumValue="10000"  ErrorMessage="Must be greater than 1900"></asp:RangeValidator>
                <asp:RequiredFieldValidator runat="server" ID="rfvStart" ControlToValidate="txtstartYear" ErrorMessage="Required"></asp:RequiredFieldValidator>
                <br />
                <asp:TextBox runat="server" ID="txtStartYear"></asp:TextBox>
            </td>
            <td nowrap style="vertical-align:bottom;">
                
                End Year
                <asp:RangeValidator runat="server" ID="rvEnd" ControlToValidate="txtendYear" Type="Integer" MinimumValue="1901" MaximumValue="10000" ErrorMessage="Must be greater than 1900"></asp:RangeValidator>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtendYear" ErrorMessage="Required"></asp:RequiredFieldValidator>
                <br />
                <asp:TextBox runat="server" ID="txtEndYear"></asp:TextBox>
            </td>
            <td nowrap style="vertical-align:bottom;padding-bottom:4px;">
                Operating Center : 
                <br />
                <asp:DropDownList runat="server" ID="ddlOpCntr" 
                    DataSourceID="dsOpCenter"
                    DataValueField="OperatingCenterID"
                    DataTextField="opCntrName"
                    AppendDataBoundItems="true"
                    >
                    <asp:ListItem Value="" Text="All"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="vertical-align:bottom;width:8.5in;">
                <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click"/>
            </td>
        </tr>
        
        <tr>
            <td colspan="4" class="RptTDData">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" 
                    Font-Names="Verdana" Font-Size="8pt"
                    Visible="false"
                    height="8.5in" width="11.5in">
                    <LocalReport ReportPath="Reports\Data\RptServicesRenewedByYear.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1_RptServicesRenewedByYear" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="MapCall.DataSet1TableAdapters.RptServicesRenewedByYearTableAdapter">
                    <SelectParameters>
                        <asp:Parameter Name="startYear" Type="Int32" />
                        <asp:Parameter Name="endYear" Type="Int32" />
                        <asp:Parameter Name="opCntr" Type="String"/>
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
    </table>
    
    <asp:SqlDataSource runat="server" ID="dsOpCenter"
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"                
        SelectCommand="Select distinct OperatingCenterID, isNull(OperatingCenterCode,'') + ' - ' + isNull(OperatingCenterName, '') as opCntrName from OperatingCenters order by 2"
        >        
    </asp:SqlDataSource>
    
</asp:Content>
