<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewServiceInstallationForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.NewServiceInstallationForm" %>

<asp:HyperLink runat="server" Text="Create Set Meter" Visible='<%#(Eval("DateCompleted") == null && Eval("DateRejected") == null && Eval("CancelledAt") == null) %>'
    NavigateUrl='<%# String.Format("../../../../Modules/mvc/FieldOperations/ServiceInstallation/New/{0}", Eval("WorkOrderId")) %>' />

<mmsinc:MvpGridView runat="server" id="gvNewServiceInstallation" DataSourceID="odsWorkOrder" AutoGenerateColumns="False" ShowFooter="False">
    <Columns>
        <asp:HyperLinkField Text="View" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="../../../../Modules/mvc/FieldOperations/ServiceInstallation/Show/{0}" HeaderText="Id"/>
        <asp:BoundField DataField="MeterManufacturerSerialNumber" HeaderText="Meter Manufacturer Serial #" />
        <asp:BoundField DataField="MeterLocationInformation" HeaderText="Meter Location Information"/>
    </Columns>
</mmsinc:MvpGridView>
<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.NewServiceInstallation" />