<%@ Page Title="Completed T & D Orders" Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="OrcomOrders.aspx.cs" Inherits="LINQTo271.Views.Reports.OrcomOrders" %>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    This report aggregates data for all operating centers. 
    It can take up to 10 minutes to finish executing. 
    Please let the report finish before attempting to run it again.
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphHeader"> - Completed T & D Orders </asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <mmsinc:mvppanel runat="server" id="pnlSearch">
    <table>
        <tr>
            <th>Operating Center:</th>
            <td>
                <mmsinc:MvpDropDownList runat="server" ID="ddlOpCode" DataSourceID="odsOperatingCenters"
                    DataTextField="FullDescription" DataValueField="OperatingCenterID" AppendDataBoundItems="true">
                    <asp:ListItem Text="--Select Here--" Value="" />
                </mmsinc:MvpDropDownList>
            </td>
        </tr>
        <tr>
            <th>For Year:</th>
            <td>
                <asp:DropDownList runat="server" ID="ddlYear" DataSourceID="odsWorkOrderYears" AppendDataBoundItems="true">
                    <asp:ListItem Selected="True" Text="--Select Here--" Value="" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return validateSearch()" />
            </td>
        </tr>
    </table>

        <asp:ObjectDataSource runat="server" ID="odsWorkOrderYears" TypeName="WorkOrders.Model.WorkOrderRepository"
            SelectMethod="GetValidORCOMWorkOrderCreationYears">
            <SelectParameters>
                <asp:Parameter Name="operatingCenterID" DbType="Int32" DefaultValue="10" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </mmsinc:mvppanel>
    <mmsinc:mvppanel runat="server" id="pnlResults" visible="false">
        <div style="width:100%">
            <div style="font-size:large;font-weight:bold">
                Orcom - T & D Orders <br />
                Op Code: <asp:Label runat="server" ID="lblOpCode" />
            </div>
            <div style="font-size: larger;font-weight:bold">
                For Year: <asp:Label runat="server" ID="lblYear" />
            </div><br />
            <asp:Table runat="server" ID="tblWorkOrders" CssClass="ReportTable">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Order Type</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Jan</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Feb</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Mar</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Apr</asp:TableHeaderCell>
                    <asp:TableHeaderCell>May</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Jun</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Jul</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Aug</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Sep</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Oct</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Nov</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Dec</asp:TableHeaderCell>
                    <asp:TableHeaderCell>YTD</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>

        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Return to Search" OnClick="btnReturnToSearch_Click" />
    </mmsinc:mvppanel>

    <script type="text/javascript">
        $(document).ready(function() {
            var tbls = $('table.ReportTable');
            $('tr:even', tbls).css('background-color', '#F5F8FC');
        });

        function validateSearch() {
            var elem, msg, valid = true;
            var ddlYear = getServerElementById('ddlYear');
            switch (true) {
                case ddlYear[0].selectedIndex == 0:
                    valid = false;
                    elem = ddlYear;
                    msg = 'Please select a year to report on.';
                    break;
            }
            if (!valid) {
                alert(msg);
                elem.focus();
            }
            return valid;
        }
    </script>

    <asp:objectdatasource runat="server" id="odsOperatingCenters" typename="WorkOrders.Library.Permissions.SecurityService"
        selectmethod="SelectUserOperatingCenters" />
</asp:Content>
