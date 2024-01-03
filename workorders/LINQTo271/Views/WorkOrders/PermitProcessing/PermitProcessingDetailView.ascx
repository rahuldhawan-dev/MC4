<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PermitProcessingDetailView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.PermitProcessing.PermitProcessingDetailView" %>
<%@ Register TagPrefix="wo" tagName="WorkOrderStreetOpeningPermitCreateForm" src="~/Controls/WorkOrders/WorkOrderStreetOpeningPermitCreateForm.ascx"%>
<mmsinc:MvpFormView runat="server" ID="fvWorkOrder" DataSourceID="odsWorkOrder" DataKeyNames="WorkOrderID">
    <ItemTemplate>
        WorkOrderID: <mmsinc:MvpLabel runat="server" Text='<%#Eval("WorkOrderID") %>' />
        <wo:WorkOrderStreetOpeningPermitCreateForm runat="server" 
            ID="PermitCreate"
            WorkOrderID='<%#Eval("WorkOrderID") %>'
            Municipality='<%#Eval("Town") %>'
            County='<%#Eval("Town.County") %>'
            State='<%#Eval("Town.State") %>'
            />
    </ItemTemplate>
</mmsinc:MvpFormView>

<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" 
    DataObjectTypeName="WorkOrders.Model.WorkOrder"/>