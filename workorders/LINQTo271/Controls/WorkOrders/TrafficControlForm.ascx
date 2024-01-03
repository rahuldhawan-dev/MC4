<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrafficControlForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.TrafficControlForm" %>

<asp:HyperLink runat="server" Text="Create Traffic Control Ticket" NavigateUrl='<%# String.Format("../../../../Modules/mvc/FieldOperations/TrafficControlTicket/New?workOrderId={0}", Eval("WorkOrderId")) %>' />
<%--<asp:HyperLink runat="server" Text="Create Traffic Control Ticket" NavigateUrl='<%# String.Format("http://localhost:15765/FieldOperations/TrafficControlTicket/New?workOrderId={0}", Eval("WorkOrderId")) %>' />--%>

<mmsinc:MvpGridView runat="server" ID="gvTrafficControl" DataSourceID="odsWorkOrder" AutoGenerateColumns="false" ShowFooter="false">
    <Columns>
        <asp:HyperLinkField Text="View" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="../../../../Modules/mvc/FieldOperations/TrafficControlTicket/Show/{0}" />
<%--        <asp:HyperLinkField Text="View" DataNavigateUrlFields="TrafficControlId" DataNavigateUrlFormatString="http://localhost:15765/FieldOperations/TrafficControlTicket/Show/{0}" />--%>
        <asp:BoundField DataField="WorkStartDate" HeaderText="WorkStartDate" DataFormatString="{0:d}" />
        <asp:BoundField DataField="TotalHours" HeaderText="TotalHours" />
        <asp:BoundField DataField="NumberOfOfficers" HeaderText="Number of Officers" />
        <asp:BoundField DataField="BillingParty" HeaderText="Billing Party"/>
    </Columns>
</mmsinc:MvpGridView>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.TrafficControlTicket" />