<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderInputDetailView.ascx.cs" Inherits="LINQTo271.Views.WorkOrders.Input.WorkOrderInputDetailView" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderInputFormView" Src="~/Controls/WorkOrders/WorkOrderInputFormView.ascx" %>
<%@ Register TagPrefix="mmsinc" TagName="ClientIDRepository" Src="~/Common/ClientIDRepository.ascx" %>
<%@ Register TagPrefix="wo" TagName="WorkOrderDocumentForm" Src="~/Controls/WorkOrders/WorkOrderDocumentsForm.ascx" %>

<div class="tabsContainer">
    <ul class="ui-tabs-nav">
        <li><a href="#initial" class="tab"><span>Initial Information</span></a></li>
        <mmsinc:MvpPlaceHolder runat="server" ID="phDocumentTab">
            <li><a href="#document" class="tab"><span>Document</span></a></li>
        </mmsinc:MvpPlaceHolder>
    </ul>
    
    <!-- INITIAL INFORMATION -->
    <div id="initial">
        <wo:WorkOrderInputFormView runat="server" ID="fvWorkOrder" OnInserting="ods_Inserting" OnUpdating="ods_Updating" />
    </div>

    <!-- DOCUMENTS -->
    <mmsinc:MvpPanel runat="server" ID="pnlDocumentTab">
        <div id="document">        
            <mmsinc:MvpUpdatePanel runat="server" ID="upDocuments" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <wo:WorkOrderDocumentForm runat="server" ID="woDocumentForm" 
                        InitialMode="Edit"
                     />
                </ContentTemplate>
            </mmsinc:MvpUpdatePanel>
        </div>
    </mmsinc:MvpPanel>
</div>
<asp:Button runat="server" ID="btnEdit" Text="Edit" OnClick="btnEdit_Click" OnClientClick="return WorkOrderInputDetailView.btnEdit_Click();" />

<mmsinc:ClientIDRepository runat="server" />
