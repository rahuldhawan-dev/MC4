<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Converter.aspx.cs" Inherits="MapCall.public1.Converter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>Northing:</td>
                <td>
                    <asp:TextBox runat="server" ID="txtNorthing" Text="490998.09"></asp:TextBox>
                    
                </td>
            </tr>
            <tr>
                <td>Easting:</td>
                <td><asp:TextBox runat="server" ID="txtEasting" Text="563158.011"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnConvertLL" Text="Convert to Lat/Lon" OnClick="btnConvertLL_Click" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnConvertNE" Text="Convert to N/E" OnClick="btnConvertNE_Click" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>Latitude:</td>
                <td><asp:TextBox runat="server" ID="txtLat"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Longitude:</td>
                <td><asp:TextBox runat="server" ID="txtLon"></asp:TextBox></td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
