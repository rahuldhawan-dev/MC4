<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WorkOrders.Master" Theme="bender" CodeBehind="ReportUseReport.aspx.cs" Inherits="LINQTo271.Views.Reports.ReportUseReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<asp:Content runat="server" ContentPlaceHolderID="cphHeader"> - Report Usage Report</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">
    <mmsinc:MvpPanel runat="server" ID="pnlSearch">
        <table>
            <tr>
                <th>Operating Center:</th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlOpCode" DataSourceID="odsOperatingCenters"
                        DataTextField="FullDescription" DataValueField="OpCntr" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>User Name:</th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlUserName" DataSourceID="odsUserNames"
                        DataTextField="UserName" DataValueField="EmployeeID" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Here--" Value="" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Start Date:</th>
                <td>
                    <asp:TextBox runat="server" ID="txtDateStart" autocomplete="off" />
                    <atk:calendarextender runat="server" targetcontrolid="txtDateStart" />
                </td>
            </tr>
            <tr>
                <th>End Date:</th>
                <td>
                    <asp:TextBox runat="server" ID="txtDateEnd" autocomplete="off" />
                    <atk:calendarextender runat="server" targetcontrolid="txtDateEnd" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click"
                        OnClientClick="return validateSearch()" />
                </td>
            </tr>
        </table>
    </mmsinc:MvpPanel>

    <mmsinc:MvpPanel runat="server" ID="pnlResults" Visible="false">
        <mmsinc:MvpGridView runat="server" ID="gvSearchResults" DataSourceID="odsReportViewings"
            AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="User Name">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Employee.UserName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Report Name" DataField="ReportName" />
                <asp:BoundField HeaderText="Date Viewed" DataField="DateViewed" DataFormatString="{0:d}" />
            </Columns>
        </mmsinc:MvpGridView>

        <asp:ObjectDataSource runat="server" ID="odsReportViewings" TypeName="WorkOrders.Model.ReportViewingRepository"
            DataObjectTypeName="WorkOrders.Model.ReportViewing" SelectMethod="GetReportViewingsByOpCenterAndDateRange">
            <SelectParameters>
                <asp:ControlParameter Name="dateStart" ControlID="txtDateStart" DbType="DateTime" />
                <asp:ControlParameter Name="dateEnd" ControlID="txtDateEnd" DbType="DateTime" />
                <asp:ControlParameter Name="opCode" ControlID="ddlOpCode" DbType="string" />
                <asp:ControlParameter Name="employeeID" ControlID="ddlUserName" DbType="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <mmsinc:MvpButton runat="server" ID="btnReturnToSearch" Text="Return to Search" OnClick="btnReturnToSearch_Click" />
        <mmsinc:MvpButton runat="server" ID="btnExport" Text="Export to Excel" OnClick="btnExport_Click" />
        <br />
    </mmsinc:MvpPanel>
    <asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService"
        SelectMethod="SelectUserOperatingCenters" />
    <asp:ObjectDataSource runat="server" ID="odsUserNames" TypeName="WorkOrders.Model.ReportViewingRepository"
        SelectMethod="GetUserNames" />
</asp:Content>
