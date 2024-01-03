<%@ Page Language="C#" MasterPageFile="~/MapCall.Master" theme="bender" AutoEventWireup="true" CodeBehind="RptServicesInstalled.aspx.cs" Inherits="MapCall.Reports.RptServicesInstalled" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="RptTable" border="0" cellpadding="3" cellspacing="0">
        <tr>
            <td colspan="4" class="RptHeader">
                Monthly Services Installed by Category
            </td>
        </tr>
        <tr>
            <td>Please choose the year you would like to view the report for then click 'View'.</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlYear" 
                    DataSourceID="dsYear"
                    DataTextField="Year"
                    DataValueField="Year"
                    ></asp:DropDownList>
            </td>
            <td style="width:500px;">
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="View" />
            </td>
        </tr>
        <tr>
            <td class="RptTDData" colspan="3">
                <rsweb:ReportViewer Visible="False" ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" height="7.5in" width="11in">
                    <LocalReport ReportPath="Reports\RptServicesInstalled2.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DataSet1_getRptServicesInstalled2" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
            </td>
        </tr>
    </table>
    
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetData"
        TypeName="MapCall.DataSet1TableAdapters.getRptServicesInstalled2TableAdapter">
        <SelectParameters>
            <asp:Parameter Name="Year" Type="Int32" DefaultValue="2005" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:SqlDataSource runat="server" ID="dsYear"
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
        ProviderName="<%$ ConnectionStrings:MCProd.ProviderName %>"
        SelectCommand="select distinct year(dateInstalled) as 'Year' from tblNJAWService order by year(dateInstalled) desc"
        >
    </asp:SqlDataSource>
</asp:Content>
