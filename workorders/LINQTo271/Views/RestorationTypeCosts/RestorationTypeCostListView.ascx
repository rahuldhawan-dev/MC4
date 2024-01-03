<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RestorationTypeCostListView.ascx.cs" Inherits="LINQTo271.Views.RestorationTypeCosts.RestorationTypeCostListView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>
<%@ Register TagPrefix="mmsinc" Namespace="MMSINC.Controls" Assembly="MMSINC.Core.WebForms" %>

<mmsinc:MvpGridView runat="server" ID="gvRestorationTypeCosts" DataSourceID="odsRestorationTypeCosts"
    AutoGenerateEditButton="true" AutoGenerateColumns="false" DataKeyNames="RestorationTypeCostID"
    OnSelectedIndexChanged="ListControl_SelectedIndexChanged" OnRowUpdating="ListControl_RowUpdating"
    AllowSorting="true">
    <Columns>
        <asp:BoundField DataField="RestorationType" HeaderText="Restoration Type" SortExpression="RestorationType.Description" ReadOnly="true" />
        <asp:TemplateField HeaderText="Cost" SortExpression="Cost">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Cost") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txtCostEdit" Text='<%# Bind("Cost") %>' />
                <atk:MaskedEditExtender runat="server" ID="meeCostEdit" TargetControlID="txtCostEdit" Mask="9999.99" />
                <script>
                    $(document).ready(function() {
                        getServerElementById('txtCostEdit').focus();
                    });
                </script>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Final Cost" SortExpression="FinalCost">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%#Eval("FinalCost") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txtFinalCostEdit" Text='<%#Bind("FinalCost") %>'/>
                <asp:CompareValidator runat="server" ID="cvFinalCost" ControlToValidate="txtFinalCostEdit" 
                    Operator="DataTypeCheck" Type="Integer" ErrorMessage="Please enter a integer value."
                    Display="dynamic" />
                <asp:RequiredFieldValidator runat="server" ID="rfvFinalCost" ControlToValidate="txtFinalCostEdit"
                    Display="dynamic" ErrorMessage="Please enter a final cost." />
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Operating Center" SortExpression="OperatingCenter.OpCntr">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblOperatingCenter" Text='<%# Eval("OperatingCenter.OpCntr") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</mmsinc:MvpGridView>

<asp:ObjectDataSource runat="server" ID="odsRestorationTypeCosts" TypeName="WorkOrders.Model.RestorationTypeCostRepository"
    DataObjectTypeName="WorkOrders.Model.RestorationTypeCost" SelectMethod="SelectAllSorted" SortParameterName="sortExpression"
    UpdateMethod="Update" />
