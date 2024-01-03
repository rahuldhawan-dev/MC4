<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="NextRankingNumber.aspx.cs" Inherits="MapCall.Modules.HR.Administrator.NextRankingNumber" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHeader">
    Next Seniority Ranking Number
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    Please choose the desired OpCode and click 'Look Up'.
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <asp:DropDownList ID="ddlOpCode" runat="server" DataSourceID="sdsOpCodes" 
        DataTextField="OperatingCenterCode" DataValueField="OperatingCenterCode">
    </asp:DropDownList>
    <asp:SqlDataSource ID="sdsOpCodes" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="SELECT DISTINCT [OperatingCenterCode] FROM [OperatingCenters] ORDER BY [OperatingCenterCode]">
    </asp:SqlDataSource>
    <asp:Button runat="server" ID="btnSearch" Text="Look Up" OnClick="btnSearch_Click" />

    <asp:Panel runat="server" ID="pnlDetails" Visible="false">
        <asp:Label runat="server" ID="lblResults" />
        <asp:SqlDataSource ID="sdsNextNumber" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            SelectCommand="
                GROUP BY
                    oc.OperatingCenterCode" />
    </asp:Panel>
</asp:Content>
