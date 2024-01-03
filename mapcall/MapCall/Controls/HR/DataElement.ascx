<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataElement.ascx.cs" Inherits="MapCall.Controls.HR.DataElement" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsinc" %>
&nbsp;
<asp:PlaceHolder runat="server" ID="PlaceHolder1"></asp:PlaceHolder>
<mmsinc:MvpDetailsView ID="DetailsView1" runat="server" 
    AutoGenerateRows="False" DataSourceID="SqlDataSource1"
    Width="100%"
    OnDataBound="DetailsView1_DataBound"
    OnPreRender="DetailsView1_PreRender"
    AlternatingRowStyle-CssClass="HRAlternatingRow"
    >
    <Fields>
    </Fields>
</mmsinc:MvpDetailsView>
<asp:Label runat="server" ID="lblResults" />
<asp:SqlDataSource runat="server" ID="SqlDataSource1" EnableViewState="true" />
&nbsp;
<br />
<br />
&nbsp;

