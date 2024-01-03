<%@ Page Language="C#" MasterPageFile="~/MapCall.Master" theme="bender" AutoEventWireup="true" CodeBehind="WorkOrder.aspx.cs" Inherits="MapCall.Reports.Production.WorkOrder" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="outer" style="width:50%;" cellpadding="4" border=1>
    <tr>
        <td style="width:80%;">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="400px" Width="400px">
                <LocalReport ReportPath="Reports\Production\WorkOrder.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
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

