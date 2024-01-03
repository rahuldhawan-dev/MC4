<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CrewListView.ascx.cs" Inherits="LINQTo271.Views.Crews.CrewListView" %>

<asp:Button runat="server" ID="btnCreate" Text="New Crew" OnClick="btnCreate_Click" Visible="false" /> <br />

<mmsinc:MvpGridView runat="server" ID="gvCrews" AutoGenerateSelectButton="true" AutoGenerateColumns="false"
    DataKeyNames="CrewID" OnSelectedIndexChanged="ListControl_SelectedIndexChanged" AllowSorting="true" OnSorting="ListControl_Sorting">
    <Columns>
        <asp:BoundField DataField="Description" HeaderText="Crew Name" SortExpression="Description" />
        <asp:BoundField DataField="Availability" HeaderText="Availability (hours)" SortExpression="Availability" />        
        <asp:BoundField DataField="OperatingCenter" HeaderText="Operating Center" SortExpression="OperatingCenter.OpCntr" />
        <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" />
    </Columns>
</mmsinc:MvpGridView><br />

<asp:ObjectDataSource runat="server" ID="odsOperatingCenters" TypeName="WorkOrders.Library.Permissions.SecurityService" 
    SelectMethod="SelectUserOperatingCenters" />
