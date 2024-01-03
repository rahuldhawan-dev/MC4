<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="SpoilsTotalsReport.aspx.cs" Inherits="LINQTo271.Views.Reports.SpoilsTotalsReport" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <mmsinc:MvpGridView runat="server" ID="gvSearchResults" DataSourceID="odsSpoilsTotals" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField HeaderText="Operating Center" DataField="OpCode" />
            <asp:BoundField HeaderText="Storage Facility" DataField="SpoilStorageLocation" />
            <asp:BoundField HeaderText="Quantity" DataField="Total" />
            <asp:BoundField HeaderText="Unit Cost" DataField="UnitCost" />
            <asp:BoundField HeaderText="Accrual Value" DataField="AccrualValue" />
        </Columns>
    </mmsinc:MvpGridView>

    <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" />

    <asp:ObjectDataSource runat="server" ID="odsSpoilsTotals" TypeName="WorkOrders.Model.SpoilRepository" SelectMethod="GetSpoilTotalsByOperatingCenter" />

    <script type="text/javascript">
        function validateSearch() {
            var elem, msg, valid = true;
            var ddlOpCode = getServerElementById('ddlOpCode');
            if (ddlOpCode[0].selectedIndex == 0) {
                ddlOpCode.focus();
                alert('Please select an OpCode.');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
