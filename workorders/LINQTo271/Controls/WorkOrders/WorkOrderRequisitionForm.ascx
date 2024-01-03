<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderRequisitionForm.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderRequisitionForm" %>

<mmsinc:MvpUpdatePanel runat="server" ID="upRequisitions" UpdateMode="Conditional" ChildrenAsTriggers="true">
   <ContentTemplate>
       <mmsinc:MvpLabel runat="server" ID="lblRequisitionError" CssClass="error" />
       <mmsinc:MvpGridView runat="server" ID="gvRequisitions" DataKeyNames="RequisitionID"
        AutoGenerateColumns="False" DataSourceID="odsRequisitions" ShowFooter="true"
        RowStyle-VerticalAlign="Top" FooterStyle-VerticalAlign="Top">
           <Columns>
                <%--Button button who's got the button? --%>
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>
                        <mmsinc:MvpLinkButton runat="server" ID="lbEdit" CausesValidation="false" CommandName="Edit"
                            Text="Edit" />
                        <mmsinc:MvpLinkButton runat="server" ID="lbDelete" CausesValidation="false" CommandName="Delete"
                            Text="Delete" OnClientClick="return confirm('Are you sure?');" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton runat="server" ID="lbSave" CausesValidation="true" CommandName="Update" 
                            Text="Update" />
                        <asp:LinkButton runat="server" ID="lbCancel" CausesValidation="false" CommandName="Cancel"
                            Text="Cancel" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton runat="server" ID="lbInsert" CausesValidation="true" Text="Insert" 
                            OnClick="lbInsert_Click" />
                        <asp:LinkButton runat="server" ID="lbCancel" CausesValidation="false" CommandName="Cancel"
                            Text="Cancel" OnClick="lbCancel_Click" />
                    </FooterTemplate>
                </asp:TemplateField>
               <%--Requisition Type--%>
               <asp:TemplateField HeaderText="Purchase Order (PO) Type">
                   <ItemTemplate>
                       <asp:Label runat="server" ID="lblRequisitionType" Text='<%#Eval("RequisitionType") %>'></asp:Label>
                   </ItemTemplate>
                   <EditItemTemplate>
                       <mmsinc:MvpDropDownList runat="server" ID="ddlRequisitionType" DataSourceID="odsRequisitionTypes"
                            SelectedValue='<%#Bind("RequisitionTypeID") %>' Style="font-size:smaller;width:425px;"
                            DataTextField="Description" DataValueField="RequisitionTypeID"
                            AppendDataBoundItems="True">
                           <asp:ListItem Text="--Select Here--" Value="" />
                       </mmsinc:MvpDropDownList>
                   </EditItemTemplate>
                   <FooterTemplate>
                       <mmsinc:MvpDropDownList runat="server" ID="ddlRequisitionType" DataSourceID="odsRequisitionTypes"
                             Style="font-size:smaller;width:425px;"
                            DataTextField="Description" DataValueField="RequisitionTypeID"
                            AppendDataBoundItems="True">
                           <asp:ListItem Text="--Select Here--" Value="" />
                       </mmsinc:MvpDropDownList>
                   </FooterTemplate>
               </asp:TemplateField>
               <%--SAP Requisition Number--%>
               <asp:TemplateField HeaderText="SAP Purchase Order (PO) #">
                   <ItemTemplate>
                       <asp:Label runat="server" ID="lblSAPRequisitionNumber"><%#Eval("SAPRequisitionNumber") %></asp:Label>
                   </ItemTemplate>
                   <EditItemTemplate>
                       <mmsinc:MvpTextBox runat="server" ID="txtSAPRequisitionNumber" Text='<%#Bind("SAPRequisitionNumber") %>' />
                   </EditItemTemplate>
                   <FooterTemplate>
                        <mmsinc:MvpTextBox runat="server" ID="txtSAPRequisitionNumber" />
                   </FooterTemplate>
               </asp:TemplateField>
               <asp:BoundField DataField="RequisitionID" ReadOnly="True" Visible="False"/>
           </Columns>
       </mmsinc:MvpGridView>
       
       <mmsinc:MvpObjectDataSource runat="server" ID="odsRequisitions"
            TypeName="WorkOrders.Model.RequisitionRepository" InsertMethod="InsertRequisition"
            SelectMethod="GetRequisitionsByWorkOrder" 
            UpdateMethod="UpdateRequisition" DeleteMethod="DeleteRequisition"
            OnInserting="odsRequisitions_Inserting">
           <SelectParameters>
               <asp:Parameter Name="WorkOrderID" Type="Int32" />
           </SelectParameters>
           <InsertParameters>
               <asp:Parameter Name="workOrderID" Type="Int32" />
               <asp:Parameter Name="requisitionTypeID" Type="Int32" />
               <asp:Parameter Name="sapRequisitionNumber" Type="String" />
               <asp:Parameter Name="creatorId" Type="Int32" />
           </InsertParameters>
       </mmsinc:MvpObjectDataSource>
       
       <asp:ObjectDataSource runat="server" ID="odsRequisitionTypes" SelectMethod="SelectAllAsList"
            TypeName="WorkOrders.Model.RequisitionTypeRepository"/>
   </ContentTemplate>
</mmsinc:MvpUpdatePanel>