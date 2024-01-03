<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="number.ascx.cs" Inherits="MapCall.Controls.Data.number" %>
<script type="text/javascript">
    function ControlChange(start, op)
    {
        s = document.getElementById(start);
        o = document.getElementById(op);
        s.style.display = (o.selectedIndex == 5 ? "" : "none");
    }
</script>
<table style="width:100%;">
    <tr>
        <td style="display:none; white-space:nowrap;" runat="server" id="tdStart">
            <asp:TextBox SkinID="clean" runat="Server" ID="txtStart" Width="75px"></asp:TextBox>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlParam">
                <asp:ListItem>=</asp:ListItem>
                <asp:ListItem>>=</asp:ListItem>
                <asp:ListItem>></asp:ListItem>
                <asp:ListItem><</asp:ListItem>
                <asp:ListItem><=</asp:ListItem>
                <asp:ListItem>BETWEEN</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="width:95%;white-space:nowrap;">
            <asp:TextBox SkinID="clean" runat="Server" ID="txtEnd" Width="75px"></asp:TextBox>
            <asp:CompareValidator runat="server" ID="cvEnd" Type="Double" ControlToValidate="txtEnd" Text="Please enter a number"></asp:CompareValidator>
            <asp:CompareValidator runat="server" ID="CompareValidator1" Type="Double" ControlToValidate="txtStart" Text="Please enter a starting number"></asp:CompareValidator>
        </td>
    </tr>
</table>
