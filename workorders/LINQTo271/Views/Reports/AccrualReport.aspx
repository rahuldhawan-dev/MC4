﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="AccrualReport.aspx.cs" Inherits="LINQTo271.Views.Reports.AccrualReport" %>
<%@ Import Namespace="WorkOrders.Model" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<asp:Content runat="server" ContentPlaceHolderID="cphInstructions">
    This report will return results where the work order has been Approved, the Accounting Type is O & M, and the Final Restoration Date has not been entered.
</asp:Content>

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
            <th>Start Date:</th>
            <td>
                <asp:TextBox runat="server" ID="txtDateStart" autocomplete="off" />
                <atk:CalendarExtender runat="server" TargetControlID="txtDateStart" />
            </td>
        </tr>
        <tr>
            <th>End Date:</th>
            <td>
                <asp:TextBox runat="server" ID="txtDateEnd" autocomplete="off" />
                <atk:CalendarExtender runat="server" TargetControlID="txtDateEnd" />
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
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" DataSourceID="odsRestorations" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvSearchResults_OnRowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Work Order Number">
                    <ItemTemplate>
                        <a href="../WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&arg=<%# Eval("WorkOrderID") %>">
                            <%# Eval("WorkOrderID") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date Completed">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.DateCompleted", "{0:d}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description of Work">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.WorkDescription") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Business Unit">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("WorkOrder.BusinessUnit") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Type of Restoration" DataField="RestorationType" />
                <asp:BoundField HeaderText="Est Restoration Footage" DataField="RestorationSize" />
                <asp:BoundField HeaderText="Actual Footage" DataField="PartialPavingSquareFootage" SortExpression="PartialPavingSquareFootage" />

                <asp:TemplateField HeaderText="Measurement Type">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("RestorationType.MeasurementTypeString") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Total Accrued Cost" DataField="TotalAccruedCost" DataFormatString="{0:C}" />
                <asp:BoundField HeaderText="Date Initial Completed" DataField="PartialRestorationDate" DataFormatString="{0:d}" />

                <asp:TemplateField HeaderText="Total Initial Cost">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# String.Format("{0:C}", DataBinder.Eval(Container.DataItem, "PartialRestorationActualCost") ?? 0) %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Accrual Value">
                    <ItemTemplate>
                        <asp:Label runat="server" id="lblAccrualValue" Text='<%# DataBinder.Eval(Container.DataItem, "AccrualValue") ?? 0 %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label runat="server" id="lblAccrualValueSum" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </mmsinc:MvpGridView>

        <asp:ObjectDataSource runat="server" ID="odsRestorations" TypeName="WorkOrders.Model.RestorationRepository"
            DataObjectTypeName="WorkOrders.Model.Restoration" SelectMethod="GetRestorationsByWorkOrderDateCompletedAndAccountingType">
            <SelectParameters>
                <asp:ControlParameter Name="dateStart" ControlID="txtDateStart" DbType="DateTime" />
                <asp:ControlParameter Name="dateEnd" ControlID="txtDateEnd" DbType="DateTime" />
                <asp:ControlParameter Name="operatingCenterID" ControlID="ddlOpCode" DbType="Int32" />
                <asp:Parameter Name="accountingTypeID" Type="Int32"/>
            </SelectParameters>
        </asp:ObjectDataSource>

        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Return to Search" OnClick="btnReturnToSearch_Click" />
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" />
        <br />
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
