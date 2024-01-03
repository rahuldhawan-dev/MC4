<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperatingCenterSpoilRemovalCostListView.ascx.cs" Inherits="LINQTo271.Views.OperatingCenterSpoilRemovalCosts.OperatingCenterSpoilRemovalCostListView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<mmsinc:MvpGridView runat="server" ID="gvOperatingCenterSpoilRemovalCosts" DataSourceID="odsOperatingCenterSpoilRemovalCosts"
    AutoGenerateEditButton="true" AutoGenerateColumns="false" DataKeyNames="OperatingCenterSpoilRemovalCostID"
    OnSelectedIndexChanged="ListControl_SelectedIndexChanged" OnRowUpdating="ListControl_RowUpdating"
    AllowSorting="true">
    <Columns>
        <asp:TemplateField HeaderText="Cost" SortExpression="Cost">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Eval("Cost") %>' />
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox runat="server" ID="txtCostEdit" Text='<%# Bind("Cost") %>' />
                <atk:MaskedEditExtender runat="server" ID="meeCostEdit" TargetControlID="txtCostEdit" Mask="99999" />
                <script>
                    $(document).ready(function() {
                        getServerElementById('txtCostEdit').focus();
                    });
                </script>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Operating Center" SortExpression="OperatingCenter.OpCntr">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblOperatingCenter" Text='<%# Eval("OperatingCenter.OpCntr") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</mmsinc:MvpGridView>

<asp:ObjectDataSource runat="server" ID="odsOperatingCenterSpoilRemovalCosts" TypeName="WorkOrders.Model.OperatingCenterSpoilRemovalCostRepository"
    DataObjectTypeName="WorkOrders.Model.OperatingCenterSpoilRemovalCost" SelectMethod="SelectAllSorted" SortParameterName="sortExpression"
    UpdateMethod="Update" />
