<%@ Page Title="" Language="C#" MasterPageFile="~/iMap.Master" AutoEventWireup="true" CodeBehind="iOneCallTicket.aspx.cs" Inherits="MapCall.Modules.Maps.iOneCallTicket" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <style>
        pre { font-family:courier, 'courier new', monospace; }
    </style>
    <asp:Repeater runat="server" DataSourceID="dsTicket">
        <ItemTemplate>
            <div>Request Number: <strong><a href="../Mvc/FieldOperations/OneCallMarkoutTicket/Show/<%#Eval("Id") %>" target="_new"><%# Eval("RequestNumber") %></a></strong></div>
            <div>Completed Date: <strong><%# Eval("CompletedAt") %></strong></div>
            <hr/>
            <pre><%# Eval("FullText") %></pre>
        </ItemTemplate>
    </asp:Repeater>

    <asp:SqlDataSource runat="server" ID="dsTicket"
        ConnectionString="<%$ ConnectionStrings:MCProd %>" 
        SelectCommand="select * from OneCallMarkoutTickets t left join OneCallMarkoutResponses r on t.Id = r.OneCallMarkoutTicketId where t.id = @Id">
        <SelectParameters>
            <asp:QueryStringParameter Name="Id" QueryStringField="ID" />
        </SelectParameters>    
    </asp:SqlDataSource>  
</asp:Content>
