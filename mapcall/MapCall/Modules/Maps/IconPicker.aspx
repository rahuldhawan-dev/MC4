<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IconPicker.aspx.cs" Inherits="MapCall.Modules.Maps.IconPicker" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Icon Picker</title>
    <script type="text/javascript">

        var selectIcon = function (src, iconId) {
            var opener = window.top.frames[1] ? window.top.frames[1].document : window.top.document;
            opener.img.src = src;
            opener.hid.value = iconId;
            opener.lightview.hide();
        };

    </script>
</head>
<body style="background:white;">
    <form id="form1" runat="server">
    <div style="overflow:hidden;width:390px;height:250px;text-align:center;">
        <h2>Choose an Icon:</h2>
        <div style="overflow:scroll;width:90%;height:90%">
            <asp:Table runat="server" ID="tblIcons" />
        </div>
    </div>
<%-- This is for when the system lets you add new icons.
        <table width="100%" >
            <tr>
                <td><asp:Table runat="server" ID="tblIcons" /></td>
                <td>
                    <h2>Current Icon:</h2>
                    <asp:Image runat="server" ID="imgCurrentIcon" /><br />
                    Width: <asp:TextBox runat="server" ID="txtCurrentWidth" /><br />
                    Height: <asp:TextBox runat="server" ID="txtCurrentHeight" />
                </td>
            </tr>
        </table>
--%>
    </form>
</body>
</html>
