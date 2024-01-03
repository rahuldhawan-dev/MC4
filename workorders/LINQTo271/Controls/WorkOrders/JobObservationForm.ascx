<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobObservationForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.JobObservationForm" %>

<asp:HyperLink runat="server" Text="Create Job Observation" 
    NavigateUrl='<%# String.Format("../../../../Modules/mvc/HealthAndSafety/JobObservation/New/{0}", Eval("WorkOrderID"))%>'/>

<mmsinc:MvpGridView runat="server" ID="gvJobObservations" DataSourceID="odsWorkOrder"
    AutoGenerateColumns="false" ShowFooter="false">
    <Columns>
        <asp:HyperLinkField Text="View" DataNavigateUrlFields="JobObservationID" DataNavigateUrlFormatString="../../../../Modules/mvc/HealthAndSafety/JobObservation/Show/{0}" />
        <asp:BoundField DataField="ObservationDate" HeaderText="ObservationDate" DataFormatString="{0:d}" />
        <asp:BoundField DataField="OverallSafetyRating" HeaderText="Overall Safety Rating" />
        <asp:BoundField DataField="OverallQualityRating" HeaderText="Overall Quality Rating" />
    </Columns>
</mmsinc:MvpGridView>
<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.JobObservation" />
