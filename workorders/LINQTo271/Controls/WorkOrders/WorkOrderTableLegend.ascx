<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkOrderTableLegend.ascx.cs" Inherits="LINQTo271.Controls.WorkOrders.WorkOrderTableLegend" %>
<style>
    .workorders-table-legend td.color-block {
        height:30px;
        width:150px;
        text-align:center;
    }
</style>
<table class="workorders-table-legend">
    <tr>
        <td class="color-block" style="background-color:#BBFFBC;">Completed</td>
        <td class="color-block" style="background-color:#ffe08d;">Cancelled</td>
        <td class="color-block" style="background-color:#fffdb9;">Previously Scheduled</td>
        <td class="color-block" style="background-color:#bbf1ff;">Currently Scheduled</td>
    </tr>
</table>