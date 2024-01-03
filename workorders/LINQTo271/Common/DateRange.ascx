<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateRange.ascx.cs" Inherits="LINQTo271.Common.DateRange" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<%-- Table needs z-index or else the calendar will pop behind other controls in IE7. --%>
<table cellpadding="0" cellspacing="0" style="width: 100%; position:relative; z-index:1;">
    <tr>
        <td runat="server" id="tdStartDate" style="display:none">
            <asp:TextBox runat="server" ID="ccStartDate" autocomplete="off" />
            <atk:CalendarExtender runat="server" ID="ceStartDate" TargetControlID="ccStartDate" />
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlSearchOp">
                <asp:ListItem>=</asp:ListItem>
                <asp:ListItem>>=</asp:ListItem>
                <asp:ListItem>></asp:ListItem>
                <asp:ListItem><=</asp:ListItem>
                <asp:ListItem><</asp:ListItem>
                <asp:ListItem>BETWEEN</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="width: 95%">
            <asp:TextBox runat="server" ID="ccEndDate" autocomplete="off" />
            <atk:CalendarExtender runat="server" ID="ceEndDate" TargetControlID="ccEndDate" />
        </td>
    </tr>
</table>

<script type="text/javascript">
    function ddlSearchOp_Change(ddlSearchOp, tdStartDate) {
        var op = $('#' + ddlSearchOp.id + ' option:selected').text();
        var fn = (op == 'BETWEEN') ? 'show' : 'hide';
        tdStartDate[fn]();
    }
</script>
