<%@ Page Title="Insurance Minimum Requirements" Language="C#" Theme="bender" MasterPageFile="~/MapCallSite.Master" AutoEventWireup="true" CodeBehind="ContractorInsuranceMinimumRequirements.aspx.cs" Inherits="MapCall.Modules.Contractors.ContractorInsuranceMinimumRequirements" %>
<%@ Register Src="~/Controls/LookupControl.ascx" TagPrefix="mapcall" TagName="Lookup"  %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphHeader" runat="server">
Insurance Minimum Requirements
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphMain" runat="server">
    <mapcall:Lookup ID="lookup" runat="server" Label="Contractor Insurance Minimum Requirement" 
        TableName="ContractorInsuranceMinimumRequirements" 
        TablePrimaryKeyFieldName="ContractorInsuranceMinimumRequirementID" />
</asp:Content>
