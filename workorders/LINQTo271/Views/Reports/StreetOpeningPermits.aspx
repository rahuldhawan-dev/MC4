<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="StreetOpeningPermits.aspx.cs" Inherits="LINQTo271.Views.Reports.StreetOpeningPermits" %>
<%@ Register TagPrefix="atk" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="mmsinc" TagName="DateRange" Src="~/Common/DateRange.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
        <table>
            <tr>
                <th>Date Requested:</th>
                <td>
                    <mmsinc:DateRange runat="server" ID="drRequestedDate" Default="LastBusinessWeek" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>

    </mmsinc:MvpPanel>

    <mmsinc:MvpPanel runat="server" ID="pnlResults" Visible="false">
        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Back to Search" OnClick="btnReturnToSearch_Click" />
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" /> <br />
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" AutoGenerateColumns="false" AllowSorting="true" OnDataBinding="gvSearchResults_DataBinding">
            <Columns>
                <mmsinc:MvpBoundField DataField="PermitID" HeaderText="Permit" />
                <mmsinc:MvpBoundField DataField="WorkOrderID" HeaderText="Work Order" />
                <mmsinc:MvpBoundField DataField="DateRequested" HeaderText="Date Requested" />
                <mmsinc:MvpBoundField DataField="DateIssued" HeaderText="Date Issued" />
                <mmsinc:MvpBoundField DataField="ExpirationDate" HeaderText="Expiration Date" />
                <mmsinc:MvpBoundField DataField="TotalCharged" HeaderText="Total Charged" />
                <mmsinc:MvpBoundField DataField="PermitFee" HeaderText="Permit Fee" />
                <mmsinc:MvpBoundField DataField="BondFee" HeaderText="Bond Fee" />
                <mmsinc:MvpBoundField DataField="IsPaidFor" HeaderText="Paid" />
                <mmsinc:MvpBoundField DataField="OperatingCenter" HeaderText="Operating Center" />
                <mmsinc:MvpBoundField DataField="Town" HeaderText="Town" />
                <mmsinc:MvpBoundField DataField="PermitFor" HeaderText="Permit For" />
                <mmsinc:MvpBoundField DataField="WorkDescription" HeaderText="Work Description" />
                <mmsinc:MvpBoundField DataField="AccountingType" HeaderText="Accounting Type" />
                <mmsinc:MvpBoundField DataField="AccountingCode" HeaderText="Account Charged" />
                <mmsinc:MvpBoundField DataField="IsCanceled" HeaderText="Is Canceled" />
            </Columns>
        </mmsinc:MvpGridView>
    </mmsinc:MvpPanel>

    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
</asp:Content>
