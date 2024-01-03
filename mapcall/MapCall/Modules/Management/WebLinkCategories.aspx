<%@ Page Title="Web Link Categories" Language="C#" MasterPageFile="~/MapCallSite.Master" Theme="bender" AutoEventWireup="true" CodeBehind="WebLinkCategories.aspx.cs" Inherits="MapCall.Modules.Management.WebLinkCategories" %>
<%@ Register Src="~/Controls/LookupControl.ascx" TagPrefix="mapcall" TagName="Lookup"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
Web Link Categories
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <mapcall:Lookup ID="lookup" runat="server" Label="Web Link Category" 
        TableName="WebLinkCategories" 
        TablePrimaryKeyFieldName="WebLinkCategoryID" />
</asp:Content>
