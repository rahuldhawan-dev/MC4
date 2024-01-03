<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="PowerPlantAsbuiltReport.aspx.cs" Inherits="LINQTo271.Views.Reports.PowerPlantAsbuiltReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphMain">
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
    <table>
        <tr>
            <th>Operating Center:</th>
            <td>
                <asp:DropDownList runat="server" ID="ddlOpCode" DataSourceID="odsOperatingCenters"
                    DataTextField="FullDescription" DataValueField="OperatingCenterID" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>Start Date:</th>
            <td>
                <asp:TextBox runat="server" ID="txtDateStart" autocomplete="off" />
                <atk:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateStart" />
            </td>
        </tr>
        <tr>
            <th>End Date:</th>
            <td>
                <asp:TextBox runat="server" ID="txtDateEnd" autocomplete="off" />
                <atk:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateEnd" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return validateSearch()" />
            </td>
        </tr>
    </table>
    </mmsinc:MvpPanel>

    <mmsinc:MvpPanel runat="server" ID="pnlResults" Visible="false">
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" AutoGenerateColumns="false" AllowSorting="true" OnSorting="gvSearchResults_Sorting">
            <Columns>
                <asp:BoundField HeaderText="Asset Type" DataField="AssetType" SortExpression="AssetType" />
                <asp:TemplateField HeaderText="Work Order Number" SortExpression="WorkOrderId">
                    <ItemTemplate>
                        <a href="../WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%# Eval("WorkOrderID") %>">
                            <%# Eval("WorkOrderID") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="SAPWorkOrderNumber" DataField="SAPWorkOrderNumber" SortExpression="SAPWorkOrderNumber" />
                <asp:BoundField HeaderText="Account Charged" DataField="AccountCharged" SortExpression="AccountCharged" />
                <asp:BoundField HeaderText="Street Address" DataField="StreetAddress" SortExpression="StreetAddress" />
                <asp:BoundField HeaderText="Town" DataField="Town" SortExpression="Town" />
                <asp:BoundField HeaderText="AssetID or Premise #" DataField="AssetID" SortExpression="AssetId" />
                <asp:BoundField HeaderText="DateCompleted" DataField="DateCompleted" DataFormatString="{0:d}" SortExpression="DateCompleted" />
                <asp:BoundField HeaderText="Work Order Description" DataField="WorkDescription" SortExpression="WorkDescription" />
                <asp:TemplateField HeaderText="Materials Installed">
                    <ItemTemplate>
                        <mmsinc:MvpGridView runat="server" ID="lvMaterialsInstalled" 
                            DataSource='<%# Eval("MaterialsUseds") %>' 
                            AutoGenerateColumns="false" ShowEmptyTable="false" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderText="PartNumber" DataField="PartNumber" />
                                <asp:BoundField HeaderText="Description" DataField="Description" />                                
                                <asp:BoundField HeaderText="NonStockDescription" DataField="NonStockDescription" />
                                <asp:BoundField HeaderText="Quantity" DataField="Quantity" />
                            </Columns>
                        </mmsinc:MvpGridView>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </mmsinc:MvpGridView>

        <mmsinc:MvpObjectDataSource runat="server" ID="odsWorkOrders" TypeName="WorkOrders.Model.WorkOrderRepository"
            DataObjectTypeName="WorkOrders.Model.WorkOrder" SelectMethod="GetCapitalWorkOrdersByOpCenterAndDateRange">
            <SelectParameters>
                <asp:ControlParameter Name="dateStart" ControlID="txtDateStart" DbType="DateTime" />
                <asp:ControlParameter Name="dateEnd" ControlID="txtDateEnd" DbType="DateTime" />
                <asp:ControlParameter Name="operatingCenterID" ControlID="ddlOpCode" DbType="Int32" />
            </SelectParameters>
        </mmsinc:MvpObjectDataSource>
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Return to Search" OnClick="btnReturnToSearch_Click" />
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" />
    </mmsinc:MvpPanel>

    <%-- TODO:  This is becoming redundant, and it's repeated unnecessarily, where repetition isn't needed. --%>
    <script type="text/javascript">
        function validateSearch() {
            var elem, msg, valid = true;
            var ddlOpCode = getServerElementById('ddlOpCode');
            var txtDateStart = getServerElementById('txtDateStart');
            var txtDateEnd = getServerElementById('txtDateEnd');
            switch (true) {
                case ddlOpCode[0].selectedIndex == 0:
                    elem = ddlOpCode;
                    msg = 'Please select an OpCode.';
                    valid = false;
                    break;
                case txtDateStart.val() == '':
                    elem = txtDateStart;
                    msg = 'Please enter a start date.';
                    valid = false;
                    break;
                case txtDateEnd.val() == '':
                    elem = txtDateEnd;
                    msg = 'Please enter an end date.';
                    valid = false;
                    break;
            }
            if (!valid) {
                alert(msg);
                elem.focus();
            }
            return valid;
        }
    </script>

    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
</asp:Content>
