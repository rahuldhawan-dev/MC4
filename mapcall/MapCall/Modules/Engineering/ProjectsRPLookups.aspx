<%@ Page EnableViewState="true" Title="" Language="C#" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ProjectsRPLookups.aspx.cs" Inherits="MapCall.Modules.Engineering.ProjectsRPLookups" EnableEventValidation="false" %>
<%@ Register TagPrefix="mapcall" Namespace="MapCall.Controls.Data" Assembly="MapCall" %>
<%@ Register TagPrefix="mmsinc" Namespace="MMSINC.Controls" Assembly="MMSINC.Core.WebForms" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
    Project RP Lookup Tables
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Label runat="server" ID="lblPermissionErrors" />
    <asp:Panel runat="server" ID="pnlAll">
        Lookup Table: <asp:DropDownList runat="server" ID="ddlTableName" />
        <asp:Button runat="server" ID="btnSelectTable" OnClick="btnSelectTable_Click"
            CausesValidation="False" Text="Select"/>
    
        <mmsinc:MvpGridView runat="server" ID="gvTable" DataSourceID="dsTable" CssClass="grid"
                      EmptyDataText="No rows exist for this lookup." AutoGenerateEditButton="True" 
                      AutoGenerateColumns="False" OnRowDataBound="gvTable_OnRowDataBound"/>
        <br />
        <mmsinc:MvpDetailsView runat="server" ID="dvTable" DataSourceID="dsTable" 
            AutoGenerateRows="false" DefaultMode="Insert" AutoGenerateInsertButton="True" 
            Visible="False" />
        <mapcall:McProdDataSource runat="server" ID="dsTable" />
    </asp:Panel>
</asp:Content>
