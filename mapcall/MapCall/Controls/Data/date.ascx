<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="date.ascx.cs" Inherits="MapCall.Controls.Data.date" %>
<script type="text/javascript">
    function dateControlChange(start, op)
    {
        s = document.getElementById(start);
        if (s) {
            o = document.getElementById(op);
            if (o.selectedIndex == 5) {
                s.style.display = "";
            }
            else {
                s.style.display = "none";
            }
        }
    }
</script>
<table style="width:100%;">
    <tr>
        <td style="display:none;" runat="server" nowrap id="tdDateInstalledStart">
            <cc1:CalendarExtender TargetControlID="txtDateInstalledStart" PopupButtonID="imgCal1" ID="CalendarExtender1" runat="server" />
            <asp:TextBox SkinID="clean" runat="Server" ID="txtDateInstalledStart" Width="75px" autocomplete="off"></asp:TextBox>
            <asp:ImageButton OnClientClick="return false;" runat="server" ID="imgCal1" ImageUrl="~/images/calendar.png" />
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlDateInstalledParam">
                <asp:ListItem>=</asp:ListItem>
                <asp:ListItem>>=</asp:ListItem>
                <asp:ListItem>></asp:ListItem>
                <asp:ListItem><</asp:ListItem>
                <asp:ListItem><=</asp:ListItem>
                <asp:ListItem>BETWEEN</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="width:95%;" nowrap>
            <cc1:CalendarExtender TargetControlID="txtDateInstalledEnd" PopupButtonID="imgCal" ID="CalendarExtender2" runat="server" />
            <asp:TextBox SkinID="clean" runat="Server" ID="txtDateInstalledEnd" Width="75px" autocomplete="off"></asp:TextBox>
            <asp:ImageButton OnClientClick="return false;" runat="server" ID="imgCal" ImageUrl="~/images/calendar.png" />
        </td>
    </tr>
</table>
