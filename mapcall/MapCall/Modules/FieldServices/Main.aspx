<%@ Page Language="C#" MasterPageFile="~/MapCallHIB.Master" Theme="bender" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="MapCall.Modules.FieldServices.Main" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Sewer
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Select a link below
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
   <table runat="server" id="trMain" style="width:100%;height:100%;text-align:center;" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <br />
                <asp:Label runat="server" ID="Label1" Text="Data" Font-Bold="true" />
            </td>
        </tr>
        <tr runat="server">
            <td class="TopBorder">
                <asp:HyperLink runat="server" ID="hlManholes" Text="Manholes" NavigateUrl="Manholes.aspx" />
            </td>
        </tr>
        <tr id="Tr1" runat="server">
            <td class="TopBorder">
                <asp:HyperLink runat="server" ID="hlSewerMainCleaning" Text="Sewer Main Cleanings" NavigateUrl="~/Modules/FieldServices/SewerMainCleanings.aspx" />
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <asp:Label runat="server" ID="Label2" Text="Reports" Font-Bold="true" />
            </td>
        </tr>
    </table>
</asp:Content>