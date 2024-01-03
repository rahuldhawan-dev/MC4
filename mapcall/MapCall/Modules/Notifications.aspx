<%@ Page Title="Notifications" Language="C#" AutoEventWireup="true" MasterPageFile="~/MapCallSite.Master" Theme="bender" CodeBehind="Notifications.aspx.cs" Inherits="MapCall.Modules.Notifications" ValidateRequest="false" %>
<%@ Register Src="~/Controls/HR/DataElement.ascx" TagName="DataElement" TagPrefix="mmsi" %>
<%@ Register Src="~/Controls/Data/DataField.ascx" TagName="DataField" TagPrefix="mmsi" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHeader">
    Notifications
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMain">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
        <center>
        <table style="text-align:left;" border="0">
            <mmsi:DataField runat="server" ID="dfTitle" DataType="String" DataFieldName="Title" HeaderText="Title:" />
            <mmsi:DataField runat="server" ID="dfDescription" DataType="String" DataFieldName="Description" HeaderText="Description:" />
            <mmsi:DataField runat="server" ID="dfIsPublic" DataType="BooleanDropDown" DataFieldName="IsPublic"  HeaderText="Is Public:" />
            <mmsi:DataField runat="server" ID="dfNotificationCategory" DataType="DropDownList" DataFieldName="NotificationCategory" 
                HeaderText="NotificationCategory:" 
                ConnectionString="<%$ ConnectionStrings:MCProd %>"
                SelectCommand="Select LookupID as val, LookupValue as txt from lookup where LookupType = 'NotificationCategory' order by 2"
            />
            <mmsi:DataField runat="server" ID="dfTimeoutDate" DataType="Date" DataFieldName="TimeoutDate" HeaderText="TimeoutDate:" />
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                    <asp:Button runat="server" ID="btnAdd" Text="Add" CausesValidation="False" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
        </center>
        <br />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlResults" Visible="false">
        <asp:HiddenField runat="server" ID="HiddenField1" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
        <asp:Button runat="server" ID="btnMap" Visible="false" PostBackUrl="~/Modules/Maps/Maps.aspx" Text="Map" />
        <asp:Button runat="server" ID="btnBackToSearch" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
        <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" 
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="NotificationID" AllowSorting="true"
            AutoGenerateColumns="false"
            >
            <Columns>
                <asp:CommandField ShowSelectButton="True" SelectText="View" />
                <asp:BoundField DataField="NotificationID" HeaderText="NotificationID:" SortExpression="NotificationID" />
                <asp:BoundField DataField="Title" HeaderText="Title:" SortExpression="Title" />
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <%#Eval("Description") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Category" HeaderText="Category:" SortExpression="Category" />
                <asp:BoundField DataField="TimeoutDate" HeaderText="TimeoutDate:" SortExpression="TimeoutDate" />
                <asp:BoundField DataField="StartDate" HeaderText="StartDate:" SortExpression="StartDate" />
                <asp:BoundField DataField="IsPublic" HeaderText="Is Public:" SortExpression="IsPublic" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MCProd %>"
           SelectCommand="
                Select 
                    N.NotificationID,
                    N.Title,
                    N.Description,
                    L.lookupvalue as Category,
                    N.TimeoutDate,
                    N.StartDate,
                    N.NotificationCategory, 
                    N.IsPublic
                from 
                    Notifications N
                left join
                    lookup l
                on
                    l.lookupID = N.NotificationCategory
           "
        >
        </asp:SqlDataSource>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlDetail" Visible="false">
        <mmsi:DataElement runat="server" ID="DataElement1" 
            OnItemInserted="DataElement1_ItemInserted" 
            DataElementName="Notifications"
            DataElementParameterName="NotificationID"
            DataElementTableName="Notifications"
            ConnectionString="MCProd"
            AllowDelete="true"
            OnPreInit="DataElement1_PreInit"
            OnDataBinding="DataElement1_DataBinding"
        >
        </mmsi:DataElement>
        
        <br />
        <center>
            <asp:Button runat="server" ID="Button4" OnClick="btnBackToSearch_Click" Text="Back to Search"/>
            <asp:Button runat="server" ID="Button5" OnClick="btnBackToResults_Click" Text="Back to Results" />
        </center>
        
    </asp:Panel>
   
</asp:Content>