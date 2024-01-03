<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTimeRange.ascx.cs" Inherits="MapCall.Controls.DateTimeRange" %>
<%@ Register Assembly="MapCall.Common" Namespace="MapCall.Common.Controls" TagPrefix="mapcall" %>

<%--DO NOT DO NOT DO NOT SET ShowTimePicker HERE. IT'S DONE IN CODE BEHIND!--%>
<div class="dateTimeRange">
    <div style="display:none;" runat="server" id="tdDateInstalledStart">
        <mapcall:DateTimePicker ID="dtpStart" runat="server" ShowCalendarButton="true"></mapcall:DateTimePicker>
    </div>
    <div>
        <asp:DropDownList runat="server" ID="ddlDateInstalledParam">
            <asp:ListItem>=</asp:ListItem>
            <asp:ListItem>>=</asp:ListItem>
            <asp:ListItem>></asp:ListItem>
            <asp:ListItem><</asp:ListItem>
            <asp:ListItem><=</asp:ListItem>
            <asp:ListItem>BETWEEN</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div>
        <mapcall:DateTimePicker ID="dtpEnd" runat="server" ShowCalendarButton="true"></mapcall:DateTimePicker>
    </div>
    <div>
        <mapcall:HelpBox runat="server" Title="Date Search Help" Width="400">
            <div>
            When searching for values between two dates, keep in mind the range of time each date represents:
            <table style="border:solid 1px silver; margin:0px auto 0px;">
                <tr>
                    <td></td>
                    <td><strong>Input Date</strong></td>
                    <td><strong>Actual Date</strong></td>
                </tr>
                <tr>
                    <td>Start Date</td>
                    <td>1/1/2010</td>
                    <td>1/1/2010 12:00:00 AM</td>
                </tr>
                <tr>
                    <td>End Date</td>
                    <td>1/2/2010</td>
                    <td>1/2/2010 11:59:59 AM</td>
                </tr>

            </table>
            </div>
        </mapcall:HelpBox>
    </div>
</div>