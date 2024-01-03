<%@ Page Language="C#" MasterPageFile="~/MapCall.Master" theme="bender" AutoEventWireup="true" CodeBehind="DailyReport.aspx.cs" Inherits="MapCall.Reports.Production.DailyReport" Title="Daily Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="outer" style="width:100%;" cellpadding="4" class="RptTable" >
        <tr>
            <td colspan="6" class="RptHeader">
                Daily Report - Scheduled Work Orders
            </td>
        </tr>
        <tr>
            <td nowrap style="vertical-align:bottom;font-size:smaller;">
                Start Date
                <asp:RequiredFieldValidator ControlToValidate="txtStartDate" runat="server" ID="rfvTxtStartDate" Text="*" ></asp:RequiredFieldValidator>
                <br />
                <asp:ImageButton ID="strImage" runat="server" ImageUrl="~/images/calendar.png" OnClientClick="return false;"/>
                <asp:TextBox runat="server" ID="txtStartDate" autocomplete="off"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="strImage" />
            </td>
            <td nowrap style="vertical-align:bottom;font-size:smaller;">
                End Date
                <asp:RequiredFieldValidator ControlToValidate="txtEndDate" runat="server" ID="RequiredFieldValidator1" Text="*" ></asp:RequiredFieldValidator>
                <br />
                <asp:ImageButton ID="endImage" runat="server" ImageUrl="~/images/calendar.png" OnClientClick="return false;"/>
                <asp:TextBox runat="server" ID="txtEndDate" autocomplete="off"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" PopupButtonID="endImage" />            
            </td>
            <td style="width:95%;">
                <asp:Button runat="server" ID="btnView" Text="View" />
            </td>
        </tr>
    <tr>
        <td style="width:100%;" colspan="3">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                Height="100%" Width="100%">
                <LocalReport ReportPath="Reports\Production\DailyReport.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DSProduction_rptProd_DailyReport" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
                TypeName="MapCall.DSProductionTableAdapters.rptProd_DailyReportTableAdapter"
                >
                <SelectParameters>
                    <asp:Parameter Name="startDate" Type="DateTime" />
                    <asp:Parameter Name="endDate" Type="DateTime" />
                </SelectParameters>
            </asp:ObjectDataSource> 
        </td>
    </tr>
</table>
<script type="text/javascript">
    //alert(document.body.clientHeight);
    var hght = document.body.clientHeight-50;
    document.getElementById('outer').style.height=hght;
    document.getElementById('ContentPlaceHolder1_ReportViewer1').style.height=hght;
</script>
</asp:Content>
