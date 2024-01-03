<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ValveImageLink.ascx.cs" Inherits="MapCall.Controls.ValveImageLink" %>

    <asp:HyperLink runat="server" ID="hl1" Target="_blank"  />

    <asp:SqlDataSource ID="dsNJValveImages" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MCProd %>"
    >
        <SelectParameters>
            <asp:Parameter Name="valnum" />
            <asp:Parameter Name="opCntr" />
            <asp:Parameter Name="valveId"  />
        </SelectParameters>
    </asp:SqlDataSource>
