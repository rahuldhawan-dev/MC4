<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderInvoiceForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderInvoiceForm" %>

<asp:HyperLink runat="server" Text="Create Invoice" NavigateUrl='<%# String.Format("../../../../Modules/mvc/FieldOperations/WorkOrderInvoice/New/{0}", Eval("WorkOrderID"))%>'/>

<mmsinc:MvpGridView runat="server" ID="gvInvoices" DataSourceID="odsWorkOrder" 
    AutoGenerateColumns="false" ShowFooter="false">
    <Columns>
        <asp:HyperLinkField Text="View" DataNavigateUrlFields="WorkOrderInvoiceID" DataNavigateUrlFormatString="../../../../Modules/mvc/FieldOperations/WorkOrderInvoice/Show/{0}" />
        <asp:BoundField HeaderText="Invoice Date" DataField="InvoiceDate" SortExpression="InvoiceDate" DataFormatString="{0:d}"/>
        <asp:BoundField HeaderText="Submitted Date" DataField="SubmittedDate" SortExpression="SubmittedDate" DataFormatString="{0:d}"/>
        <asp:BoundField HeaderText="Canceled Date" DataField="CanceledDate" SortExpression="CanceledDate" DataFormatString="{0:d}"/>
        <asp:BoundField HeaderText="Includes Materials" DataField="IncludeMaterials" SortExpression="IncludeMaterials"/>
    </Columns>
</mmsinc:MvpGridView>
<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.WorkOrderInvoice" />