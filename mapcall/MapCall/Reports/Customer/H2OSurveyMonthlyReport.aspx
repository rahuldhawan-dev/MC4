<%@ Page Title="H2O Survey Monthly Report" Language="C#" MasterPageFile="~/MapCallSite.Master" ClientIDMode="Static" ViewStateMode="Disabled" Theme="bender" AutoEventWireup="true" CodeBehind="H2OSurveyMonthlyReport.aspx.cs" Inherits="MapCall.Reports.Customer.H2OSurveyMonthlyReport" %>
<%@ Register Src="~/Controls/DateTimeRange.ascx" TagName="DateTimeRange" TagPrefix="mapcall" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
H2O Survey Monthly Report
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">


<style>

.reportHeader { display:block; font-size:16px; font-weight:bold; margin-top:16px; }

.reportWrap table {  width:100%; }
.reportWrap table tr:first-child td { font-weight:bold; }
.reportWrap table tr:first-child td:first-child { width:325px; }
 
.bc-box-search h2 { margin-bottom:12px; }
.row-heading { font-weight:bold; }
.sub-row-heading,.sub-sub-row-heading { font-weight:normal; font-style:italic; text-indent:18px; display:block; }
.sub-sub-row-heading { text-indent:36px; color:Gray; }
.value { font-weight:bold; }
.sub-value, .sub-sub-value { font-style:italic; }
.sub-sub-value { color:Gray; }

#phResults { position:relative; }
#btnExport { position:absolute; top:6px; right:6px; }


</style>


<div class="container boxContainer bc-1col">
    <div>
        <div class="bc-box bc-box-search container">
            <h2>Search by Start and End Month</h2>
            <table>
                <tr>
                    <td>
                    <mapcall:DateTimeRange ID="searchDates" runat="server" ShowTimePicker="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearchOnClick" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblError" runat="server" CssClass="error" Visible="false">
                Invalid date. Dates must be in MM/DD/YYYY format.
            </asp:Label>
        </div>
        <div class="bc-box">
            <%--Visibility of this is set during OnPreRender. Should only be visible when there's a search performed.--%>
            <asp:PlaceHolder ID="phResults" runat="server">
                <h2>Report</h2>
                <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExportOnClick" />
                <asp:PlaceHolder ID="phExportable" runat="server">
                    <div class="reportWrap">
                        <span class="reportHeader">Intake</span>
                        <asp:GridView ID="gridIntake" runat="server" AutoGenerateColumns="true" ShowHeader="false"  />

                        <span class="reportHeader">Customers Completed</span>
                        <asp:GridView ID="gridCustomersCompleted" runat="server" AutoGenerateColumns="true" ShowHeader="false"></asp:GridView>

                        <span class="reportHeader">Customers Remaining in Queue</span>
                        <asp:GridView ID="gridCustomersRemaining" runat="server" AutoGenerateColumns="true" ShowHeader="false"></asp:GridView>

                        <span class="reportHeader">Outcomes from Completions</span>
                        <asp:GridView ID="gridCompletionOutcomes" runat="server" AutoGenerateColumns="true" ShowHeader="false"></asp:GridView>

                        <span class="reportHeader">Misc</span>
                        <asp:GridView ID="gridMisc" runat="server" AutoGenerateColumns="true" ShowHeader="false"></asp:GridView>
                    </div>
                </asp:PlaceHolder>
            </asp:PlaceHolder>
        </div>
    </div>
</div>

</asp:Content>