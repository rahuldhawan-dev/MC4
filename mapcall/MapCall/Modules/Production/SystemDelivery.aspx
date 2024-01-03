<%@ Page Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="SystemDelivery.aspx.cs" Inherits="MapCall.Modules.HR.Accounting.SystemDelivery" Title="Accounting - System Delivery" %>
<%@ Register Src="~/Controls/Data/number.ascx" TagName="Number" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/Notes.ascx" TagName="Notes" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Documents/Documents.ascx" TagName="Documents" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/dropdownlists/ddlOpCode.ascx" TagName="ddlOpCode" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/HR/SystemDelivery.ascx" TagName="SystemDelivery" TagPrefix="mmsi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    Accounting - System Delivery
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphInstructions" runat="server">
    Use these pages to manage the System Delivery table.
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <tr>
                <td style="text-align:right;">
                    OpCode : 
                </td>
                <td style="text-align:left;">
                    <mmsi:ddlOpCode runat="server" ID="ddlOpCntr" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    Budget_Year : 
                </td>
                <td style="text-align:left;">
                    <mmsi:Number ID="txtBudgetYear" runat="server" SelectedIndex="5" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    PWSID : 
                </td>
                <td style="text-align:left;">
                    <asp:TextBox runat="server" ID="txtPWSID" />
                </td>
            </tr>
            <tr>
                <td style="text-align:right;">
                    System Delivery Category : 
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList runat="server" ID="ddlSystem_Delivery_Category" 
                        DataSourceID="dsSystem_Delivery_Category" 
                        AppendDataBoundItems="true"
                        DataTextField="LookupValue"
                        DataValueField="LookupID"
                        >
                        <asp:ListItem Text="--Select Here--" Value=""></asp:ListItem>
                    </asp:DropDownList>                    
                    
                    <asp:SqlDataSource runat="server" ID="dsSystem_Delivery_Category"
                        ConnectionString="<%$ ConnectionStrings:MCProd %>"
                        SelectCommand="select LookupID, LookupValue from Lookup where Lookuptype = 'System_Delivery_Category' order by 1"
                        >
                    </asp:SqlDataSource>
                </td>
            </tr>
                    
                    
                    
                    
             
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                    <asp:Button runat="server" ID="btnAdd" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:GridView runat="server" ID="GridView1" 
            AutoGenerateColumns="False" 
            DataKeyNames="SysDelID" 
            DataSourceID="SqlDataSource1"
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            OnSorting="GridView1_Sorting"
            OnPageIndexChanged="GridView1_PageIndexChanged"
            AllowSorting="true"
            AlternatingRowStyle-CssClass="HRAlternatingRow"
        >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
                <asp:BoundField DataField="SysDelID" HeaderText="SysDelID" InsertVisible="False" ReadOnly="True" SortExpression="SysDelID" />
                <asp:BoundField DataField="OpCode" HeaderText="OpCode" SortExpression="OpCode" />
                <asp:BoundField DataField="Budget_Year" HeaderText="Budget_Year" SortExpression="Budget_Year" />
                <asp:BoundField DataField="Budget_Actual_Text" HeaderText="Budget_Actual" SortExpression="Budget_Actual_Text" />
                <asp:BoundField DataField="PWSID" HeaderText="PWSID" SortExpression="PWSID" />
                <asp:BoundField DataField="PWSID_Name" HeaderText="PWSID_Name" SortExpression="PWSID_Name" />
                <asp:BoundField DataField="Orcom_District" HeaderText="Orcom_District" SortExpression="Orcom_District" />
                <asp:BoundField DataField="System_Delivery_Category_Text" HeaderText="System_Delivery_Category" SortExpression="System_Delivery_Category_Text" />
                <asp:BoundField DataField="Jan" HeaderText="Jan" SortExpression="Jan" />
                <asp:BoundField DataField="Feb" HeaderText="Feb" SortExpression="Feb" />
                <asp:BoundField DataField="Mar" HeaderText="Mar" SortExpression="Mar" />
                <asp:BoundField DataField="Apr" HeaderText="Apr" SortExpression="Apr" />
                <asp:BoundField DataField="May" HeaderText="May" SortExpression="May" />
                <asp:BoundField DataField="Jun" HeaderText="Jun" SortExpression="Jun" />
                <asp:BoundField DataField="Jul" HeaderText="Jul" SortExpression="Jul" />
                <asp:BoundField DataField="Aug" HeaderText="Aug" SortExpression="Aug" />
                <asp:BoundField DataField="Sep" HeaderText="Sep" SortExpression="Sep" />
                <asp:BoundField DataField="Oct" HeaderText="Oct" SortExpression="Oct" />
                <asp:BoundField DataField="Nov" HeaderText="Nov" SortExpression="Nov" />
                <asp:BoundField DataField="Dec" HeaderText="Dec" SortExpression="Dec" />
                <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                <asp:BoundField DataField="NoteCount" HeaderText="Notes" SortExpression="NoteCount" />
                <asp:BoundField DataField="DocumentCount" HeaderText="Documents" SortExpression="DocumentCount" />                
            </Columns>
            <AlternatingRowStyle CssClass="HRAlternatingRow" />
        </asp:GridView>
        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:MCProd %>" 
            SelectCommand="
                SELECT *, jan+feb+mar+apr+may+jun+jul+aug+sep+oct+nov+dec as Total 
                    , #1.LookupValue as [Budget_Actual_Text]
                    , #2.LookupValue as [OrCom_District_Text]
                    , #3.LookupValue as [System_Delivery_Category_Text]
                    , (Select Count(noteID) FROM Note where dataTypeID = 32 and dataLinkID = SysDelID) as [NoteCount]
                    , (Select Count(documentID) FROM DocumentLink where dataTypeID = 32 and dataLinkID = SysDelID) as [DocumentCount]
                    from tblAccounting_Budget_SysDel_PWSID
                    LEFT JOIN Lookup #1 on #1.LookupID = Budget_Actual
                    LEFT JOIN Lookup #2 on #2.LookupID = OrCom_district
                    LEFT JOIN Lookup #3 on #3.LookupID = System_Delivery_Category
            "
            >
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:SystemDelivery runat="server" ID="SystemDelivery1" OnItemInserted="SystemDelivery1_ItemInserted" />
        <mmsi:Notes ID="Notes1" runat="server" DataTypeID="32" />
        <mmsi:Documents ID="Documents1" runat="server" DataTypeID="32" />
        <br />
        <center>
            <asp:Button runat="server" ID="Button1" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="btnBackToResults" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>

</asp:Content>
