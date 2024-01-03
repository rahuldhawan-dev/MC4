<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderStreetOpeningPermitCreateForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderStreetOpeningPermitCreateForm" %>
<%@ Import Namespace="Permits.Data.Client" %>

<%-- Do NOT put a slash at the ende of the api-js url. It'll 404. MVC bundling! --%>
<script type="text/javascript" src="<%=Utilities.BaseAddress %>scripts/api-js"></script>

<style type="text/css">
    fieldset.permitHeading {
        padding: 6px;
    }
    fieldset.permitHeading legend {
        font-size:20px;
    }
</style>

<%-- Ross note: The moment any scripts we send from Permits starts to interfere with scripts
                we use here, we're gonna want to put this in an iframe. An iframe that's
                hosted on 271, but that doesn't load up all the masterpage junk. --%>

<%--Choose where to submit--%>
<mmsinc:MvpPanel runat="server" ID="pnlSelect">
    <fieldset class="permitHeading">
        <legend>Street Opening Permit: Start Permit</legend>
        <div class="container">Please select the State, County, or Municipality to submit the permit to.</div>
        <div class="container">
            <mmsinc:MvpLabel runat="server" ID="lblFlash" />
            <mmsinc:MvpDropDownList runat="server" ID="ddlForms" OnDataBinding="ddlForms_DataBinding" Visible="False" />
            <mmsinc:MvpButton runat="server" ID="btnCreate" Text="Create Permit" Visible="False" OnClick="On_Create" />
        </div>
    </fieldset>
</mmsinc:MvpPanel>

<%--Form--%>
<mmsinc:MvpPanel runat="server" ID="pnlForm" Visible="False">
    <fieldset class="permitHeading">
        <legend>Street Opening Permit: Enter Permit Information</legend>
        <div class="container">
            <mmsinc:MvpPlaceHolder runat="server" ID="phPermitForm" />
        </div>
        <div class="container">
            <mmsinc:MvpButton runat="server" ID="btnSubmit" Text="Submit" OnClick="On_Submit" />
        </div>
    </fieldset>
</mmsinc:MvpPanel>
    
<script type="text/javascript" src="<%=Utilities.BaseAddress %>Scripts/ExpressionLanguage/SimpleExpressionLanguage.js"></script>
<script type="text/javascript">
    $('#validation-summary-errors').hide();
</script>

<%--Drawings--%>
<mmsinc:MvpPanel ID="pnlDrawings" runat="server" Visible="False">
    <fieldset class="permitHeading">
        <legend>Street Opening Permit: Add Drawings</legend>
        <div class="container">
            You must attach a drawing before this permit can be submitted.
        </div>
        <div class="container">
            <div id="file-uploader"></div>
        </div>
        <div class="container">
            <mmsinc:MvpButton runat="server" ClientIDMode="Static" ID="continue" Text="Continue" Style="display:none;" OnClick="On_DrawingUploaded" />
        </div>
    </fieldset>
</mmsinc:MvpPanel>

<%--Payment--%>
<mmsinc:MvpPanel ID="pnlPayment" runat="server" Visible="False">
    <fieldset class="permitHeading">
        <legend>Street Opening Permit: Submit Payment</legend>
        <div class="container">
            <mmsinc:MvpPlaceHolder runat="server" ID="phPaymentForm" />
        </div>
        <div class="container">
            <mmsinc:MvpButton runat="server" ID="btnSubmitPayment" Text="Submit Payment" OnClick="On_SubmitPayment" />
        </div>
    </fieldset>
</mmsinc:MvpPanel>

<%--Done--%>
<mmsinc:MvpPanel ID="pnlSuccess" runat="server" Visible="False">
    <fieldset class="permitHeading">
        <legend>Street Opening Permit</legend>
        <div class="container">
            The permit has been submitted successfully. 
            <a href="../General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%=WorkOrderID%>">Click Here</a>
            to return to the Work Order.
        </div>
    </fieldset>

</mmsinc:MvpPanel>

<mmsinc:MvpHiddenField runat="server" ID="hidPermitId" />

<mmsinc:MvpObjectDataSource runat="server" ID="odsStreetOpeningPermits" 
    UpdateMethod="UpdateStreetOpeningPermitDrawingsPayments"
    TypeName="WorkOrders.Model.StreetOpeningPermitRepository">
    <UpdateParameters>
        <asp:Parameter Name="WorkOrderID" DbType="Int32" />
        <asp:Parameter Name="permitId" DbType="Int32" />
        <asp:Parameter Name="HasMetDrawingRequirements" DbType="Boolean"/>
        <asp:Parameter Name="IsPaidFor" DbType="Boolean"/>
    </UpdateParameters>
</mmsinc:MvpObjectDataSource>

<a href="../General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%=WorkOrderID%>">Back to the WorkOrder</a>