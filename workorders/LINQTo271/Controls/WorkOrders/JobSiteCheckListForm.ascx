<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobSiteCheckListForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.JobSiteCheckListForm" %>
<%@ Import Namespace="MMSINC.Utilities" %>


<mmsinc:MvpGridView runat="server" ID="gvMaterialsUsed" DataSourceID="odsWorkOrder"
    AutoGenerateColumns="false" ShowFooter="false">
    <Columns>
        <asp:HyperLinkField Text="View/Edit" DataNavigateUrlFields="JobSiteCheckListID" DataNavigateUrlFormatString="../../../../Modules/mvc/HealthAndSafety/JobSiteCheckList/Show/{0}" />
        <asp:TemplateField HeaderText="Check List Date">
            <ItemTemplate>
                <%# Eval("CheckListDate",CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE)%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
    </Columns>
</mmsinc:MvpGridView>
<mmsinc:MvpObjectContainerDataSource runat="server" ID="odsWorkOrder" DataObjectTypeName="WorkOrders.Model.JobSiteCheckList" />
