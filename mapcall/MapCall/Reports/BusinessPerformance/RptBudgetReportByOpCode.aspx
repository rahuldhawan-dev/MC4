﻿<%@ Page Title="Report - Budget Rollup By OpCode" Theme="bender" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="RptBudgetReportByOpCode.aspx.cs" Inherits="MapCall.Modules.BusinessPerformance.RptBudgetReportByOpCode" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/ChartWithSettings.ascx" TagName="ChartWithSettings" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Report - Budget Rollup By OpCode
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" id="DataField1" DataType="DropDownList"
                HeaderText="OpCode :"
                DataFieldName="OpCode"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="select distinct OpCode as val, OpCode as txt from tblAccounting_ExpenseLines order by OpCode asc"
            />  
            <mmsi:DataField runat="server" ID="dfYear" DataType="NumberRange"
                HeaderText="Budget Year" DataFieldName="BudgetYear" />          
            
            <mmsi:DataField runat="server" id="dfExpenseLine" DataType="DropDownList"
                HeaderText="Expense Line :"
                DataFieldName="ExpenseLine"
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select distinct ExpenseLine as val, ExpenseLine as txt from tblAccounting_ExpenseLines order by ExpenseLine asc"
            />

            <tr>
                <td>Budget Category: </td>
                <td>
                    <asp:ListBox runat="server" ID="lbBudgetCategories"
                        SelectionMode="Multiple"
                        DataSourceID="dsBudgetCategories"
                        DataTextField="LookupValue"
                        DataValueField="LookupID"
                        Rows="7"
                    />
                    <asp:SqlDataSource runat="server" ID="dsBudgetCategories"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="select LookupID, LookupValue from lookup where lookuptype = 'budget_category' order by 2" />
                </td>
            </tr>
            
            <tr>            
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                </td>
            </tr>
        </table>
        </center>
        <br />
    </asp:Panel>

   <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="hidFilter" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            AutoGenerateColumns="False" Font-Size="Small" >
            <Columns>
                <asp:BoundField DataField="OpCode" HeaderText="OpCode" 
                    SortExpression="OpCode" />
                <asp:BoundField DataField="BudgetYear" HeaderText="BudgetYear" 
                    SortExpression="BudgetYear" />
                <asp:BoundField DataField="Budget Category" HeaderText="Budget Category" 
                    SortExpression="Budget Category" />
                <asp:BoundField DataField="Jan" DataFormatString="{0:c0}" HeaderText="Jan" 
                    ReadOnly="True" SortExpression="Jan" />
                <asp:BoundField DataField="Feb" DataFormatString="{0:c0}" HeaderText="Feb" 
                    ReadOnly="True" SortExpression="Feb" />
                <asp:BoundField DataField="Mar" DataFormatString="{0:c0}" HeaderText="Mar" 
                    ReadOnly="True" SortExpression="Mar" />
                <asp:BoundField DataField="Apr" DataFormatString="{0:c0}" HeaderText="Apr" 
                    ReadOnly="True" SortExpression="Apr" />
                <asp:BoundField DataField="May" DataFormatString="{0:c0}" HeaderText="May" 
                    ReadOnly="True" SortExpression="May" />
                <asp:BoundField DataField="Jun" DataFormatString="{0:c0}" HeaderText="Jun" 
                    ReadOnly="True" SortExpression="Jun" />
                <asp:BoundField DataField="Jul" DataFormatString="{0:c0}" HeaderText="Jul" 
                    ReadOnly="True" SortExpression="Jul" />
                <asp:BoundField DataField="Aug" DataFormatString="{0:c0}" HeaderText="Aug" 
                    ReadOnly="True" SortExpression="Aug" />
                <asp:BoundField DataField="Sep" DataFormatString="{0:c0}" HeaderText="Sep" 
                    ReadOnly="True" SortExpression="Sep" />
                <asp:BoundField DataField="Oct" DataFormatString="{0:c0}" HeaderText="Oct" 
                    ReadOnly="True" SortExpression="Oct" />
                <asp:BoundField DataField="Nov" DataFormatString="{0:c0}" HeaderText="Nov" 
                    ReadOnly="True" SortExpression="Nov" />
                <asp:BoundField DataField="Dec" DataFormatString="{0:c0}" HeaderText="Dec" 
                    ReadOnly="True" SortExpression="Dec" />
                <asp:BoundField DataField="Total" DataFormatString="{0:c0}" HeaderText="Total" 
                    ReadOnly="True" SortExpression="Total" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
            ProviderName="System.Data.SqlClient"
            CancelSelectOnNullParameter="false"
            SelectCommand="
            -- IF YOU CHANGE THIS, CHANGE THE SQL HACK IN THE CODEBEHIND TOO
Select 
  #2.OpCode,
  BudgetYear,
  #1.LookupValue as [Budget Category],
  CAST(SUM([Jan]) as int) as [Jan],
  CAST(SUM([Feb]) as int) as [Feb],
  CAST(SUM([Mar]) as int) as [Mar],
  CAST(SUM([Apr]) as int) as [Apr],
  CAST(SUM([May]) as int) as [May],
  CAST(SUM([Jun]) as int) as [Jun],
  CAST(SUM([Jul]) as int) as [Jul],
  CAST(SUM([Aug]) as int) as [Aug],
  CAST(SUM([Sep]) as int) as [Sep],
  CAST(SUM([Oct]) as int) as [Oct],
  CAST(SUM([Nov]) as int) as [Nov],
  CAST(SUM([Dec]) as int) as [Dec],
  CAST((sum(jan)+sum(feb)+sum(mar)+sum(apr)+sum(may)+sum(jun)+sum(jul)+sum(aug)+sum(sep)+sum(oct)+sum(nov)+sum(dec)) as int) as Total, 
  Budget_Category
from 
    tblBudget_ExpenseLines_and_Ref 
LEFT JOIN 
    Lookup #1 on #1.LookupID = Budget_Category
LEFT JOIN 
    tblAccounting_ExpenseLines #2 on #2.expenselinenumber_ID = [tblBudget_ExpenseLines_and_Ref].expenselinenumber_ID
GROUP BY 
    #2.OpCode
, BudgetYear
, #1.LookupValue, Budget_Category 
ORDER BY 1, 2
            ">
        </asp:SqlDataSource>
        
        <uc1:ChartWithSettings runat="server" id="cws"></uc1:ChartWithSettings>

    </asp:Panel>
 
</asp:Content>
