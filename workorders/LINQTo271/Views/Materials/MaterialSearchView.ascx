<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaterialSearchView.ascx.cs" Inherits="LINQTo271.Views.Materials.MaterialSearchView" %>

<table>
    <tr>
        <td>Active</td>
        <td>
            <mmsinc:MvpDropDownList runat="server" ID="ddlActive">
                <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                <asp:ListItem Text="Active" Value="True"></asp:ListItem>
                <asp:ListItem Text="Inactive" Value="False"></asp:ListItem>
            </mmsinc:MvpDropDownList>
        </td>
        <td>Do Not Order</td>
        <td>
            <mmsinc:MvpDropDownList runat="server" ID="ddlDoNotOrder">
                <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                <asp:ListItem Text="No" Value="False"></asp:ListItem>
            </mmsinc:MvpDropDownList>
        </td>
        <td>Part Number</td>
        <td>
            <mmsinc:MvpTextBox runat="server" ID="txtPartNumber"></mmsinc:MvpTextBox>
        </td>
        <td>Description</td>
        <td>
            <mmsinc:MvpTextBox runat="server" ID="txtDescription"></mmsinc:MvpTextBox>
        </td>
        <td>
            <mmsinc:MvpButton runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Filter"/>
        </td>
    </tr>
</table>