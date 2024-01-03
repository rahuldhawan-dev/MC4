<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="KPIReportByHours.aspx.cs" Inherits="LINQTo271.Views.Reports.KPIReportByHours" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
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
            SelectMethod="GetValidWorkOrderCompletionYears">
            <SelectParameters>
                <asp:Parameter Name="operatingCenterID" DbType="Int32" DefaultValue="10" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </mmsinc:MvpPanel>

    <mmsinc:MvpPanel runat="server" ID="pnlResults" Visible="false">
        <div style="width:100%">
            <div style="font-size:large;font-weight:bold">
                KPI Report <br />
                Op Code: <asp:Label runat="server" ID="lblOpCode" />
            </div>
            <div style="font-size: larger;font-weight:bold">
                For Year: <asp:Label runat="server" ID="lblYear" />
            </div><br />
            <div style="float:left">
                <asp:Table runat="server" ID="tblCompletedWorkOrders" CssClass="ReportTable">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell ColumnSpan="14" HorizontalAlign="Center" Font-Bold="true" Font-Size="Larger">
                            Total Man Hours
                        </asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                    <asp:TableHeaderRow>
                        <asp:TableCell>&nbsp;</asp:TableCell>
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
            </div>
            
            <div style="float:left;width:50px">&nbsp;</div>

            <div>
                <asp:Table runat="server" ID="tblIncompleteWorkOrders" CssClass="ReportTable">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell ColumnSpan="14" HorizontalAlign="Center" Font-Bold="true" Font-Size="Larger">
                            Incomplete/Backlog Work Order Hours
                        </asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>Category</asp:TableHeaderCell>
                        <asp:TableHeaderCell>Hours</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </div><br />

        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Return to Search" OnClick="btnReturnToSearch_Click" />
    </mmsinc:MvpPanel>

    <script type="text/javascript">
        $(document).ready(function() {
            var tbls = $('table.ReportTable');
            $('tr:even', tbls).css('background-color', '#F5F8FC');
        });

        function validateSearch() {
            var elem, msg, valid = true;
            var ddlOpCode = getServerElementById('ddlOpCode');
            var ddlYear = getServerElementById('ddlYear');
            switch (true) {
                case ddlOpCode[0].selectedIndex == 0:
                    valid = false;
                    elem = ddlOpCode;
                    msg = 'Please select an OpCode to report on.';
                    break;
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

    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
</asp:Content>
