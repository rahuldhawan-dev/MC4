<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ddlMcProdOperatingCenter.ascx.cs" Inherits="MapCall.Controls.ddlMcProdOperatingCenter" %>
<%@ Register Assembly="MMSINC.Core.WebForms" Namespace="MMSINC.Controls" TagPrefix="mmsi" %>

<mmsi:MvpDropDownList runat="server" ID="ddlOpCntr" DataSourceID="dsOpCntr" 
    DataTextField="OpCntr" DataValueField="OperatingCenterID" 
    OnDataBound="ddlOpCntr_DataBound"
    OnDataBinding="ddlOpCntr_DataBinding"
    AppendDataBoundItems="true"
/>

<asp:SqlDataSource runat="server" ID="dsOpCntr"
    ConnectionString="<%$ ConnectionStrings:MCProd %>"
    SelectCommand="select OperatingCenterID, OperatingCenterCode + ' - ' + OperatingCenterName as OpCntr from OperatingCenters oc left join states on states.stateID = oc.StateId order by states.Abbreviation, OperatingCenterCode"
>
</asp:SqlDataSource> 

<asp:RequiredFieldValidator runat="server" ID="rfvddlOpCntr"    
    ControlToValidate="ddlOpCntr" Enabled="false" Text="*required" 
/>